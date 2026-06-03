#nullable enable
#if FRAM3_FRAMEWORK_DIAGNOSTICS
using System;
using System.Collections.Generic;
using System.IO;
using Fram3.UI.Diagnostics;
using Fram3.UI.Storybook;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class Fram3MetricsLogger : MonoBehaviour
{
    private StreamWriter? _logWriter;

    private void OnEnable()
    {
        var dir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Logs", "Fram3"));
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, $"fram3-perf-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
        _logWriter = new StreamWriter(path, append: false) { AutoFlush = true };
        _logWriter.WriteLine($"# Fram3 performance log — {DateTime.Now:O}");
        _logWriter.WriteLine($"# Thresholds: flush OK<1ms WARN<3ms CRIT>=3ms | dirty OK<=5 WARN<=15 CRIT>15 | insert ratio WARN>50%");
        _logWriter.WriteLine();

        Fram3Diagnostics.OnFrameMetrics += OnMetrics;
    }

    private void OnDisable()
    {
        Fram3Diagnostics.OnFrameMetrics -= OnMetrics;
        _logWriter?.Dispose();
        _logWriter = null;
    }

    private void OnMetrics(FrameMetrics m)
    {
        if (m.DirtyNodesFlushed == 0) return;

        var flushMs     = m.FlushDurationTicks * 1000.0 / Stopwatch.Frequency;
        var diffMs      = m.DiffDurationTicks  * 1000.0 / Stopwatch.Frequency;
        var totalOps    = m.DiffOpsInsert + m.DiffOpsUpdate + m.DiffOpsRemove + m.DiffOpsMove;
        var insertRatio = totalOps > 0 ? (float)m.DiffOpsInsert / totalOps : 0f;

        var severity = Severity.Good;
        var hints    = new List<string>();

        if      (flushMs >= 3.0) { severity = Worst(severity, Severity.Bad);     hints.Add($"flush {flushMs:F1}ms — check Build() cost or reduce dirty nodes"); }
        else if (flushMs >= 1.0) { severity = Worst(severity, Severity.Warning); hints.Add($"flush {flushMs:F1}ms — watch for growth"); }

        if      (m.DirtyNodesFlushed > 15) { severity = Worst(severity, Severity.Bad);     hints.Add($"{m.DirtyNodesFlushed} dirty nodes — coalesce SetState calls"); }
        else if (m.DirtyNodesFlushed > 5)  { severity = Worst(severity, Severity.Warning); hints.Add($"{m.DirtyNodesFlushed} dirty nodes"); }

        if (insertRatio > 0.5f && totalOps > 2)
        {
            severity = Worst(severity, Severity.Warning);
            hints.Add($"{insertRatio:P0} inserts — add Keys to list items to enable node reuse");
        }

        if (m.BuildExceptions > 0)
        {
            severity = Severity.Bad;
            hints.Add($"{m.BuildExceptions} Build() exception(s) — check ErrorPlaceholder in UI");
        }

        var prefix = severity switch
        {
            Severity.Good    => "[OK]  ",
            Severity.Warning => "[WARN]",
            _                => "[CRIT]"
        };

        var story    = StorybookApp.ActiveStory ?? "unknown";
        var elements = Fram3Diagnostics.LastFrameRebuiltTypes;
        var hintStr  = hints.Count > 0 ? $"\n  -> {string.Join("\n  -> ", hints)}" : "";

        var msg =
            $"[FRAM3] {prefix}  [{story}]  flush={flushMs:F2}ms diff={diffMs:F2}ms | " +
            $"rebuilt={m.NodesRebuilt} mounted={m.NodesMounted} unmounted={m.NodesUnmounted} | " +
            $"ops: +{m.DiffOpsInsert} ~{m.DiffOpsUpdate} -{m.DiffOpsRemove} >{m.DiffOpsMove} | " +
            $"elements: {string.Join(", ", elements)}" +
            hintStr;

        switch (severity)
        {
            case Severity.Good: Debug.Log(msg);        break;
            default:            Debug.LogWarning(msg); break;
        }

        if (severity == Severity.Bad)
        {
            _logWriter?.WriteLine($"[frame {Time.frameCount}] {DateTime.Now:HH:mm:ss.fff}  story: {story}");
            _logWriter?.WriteLine($"  flush={flushMs:F2}ms diff={diffMs:F2}ms");
            _logWriter?.WriteLine($"  rebuilt={m.NodesRebuilt} mounted={m.NodesMounted} unmounted={m.NodesUnmounted}");
            _logWriter?.WriteLine($"  ops: +{m.DiffOpsInsert}(insert) ~{m.DiffOpsUpdate}(update) -{m.DiffOpsRemove}(remove) >{m.DiffOpsMove}(move)");
            _logWriter?.WriteLine($"  elements: {string.Join(", ", elements)}");
            foreach (var hint in hints)
            {
                _logWriter?.WriteLine($"  ! {hint}");
            }
            _logWriter?.WriteLine();
        }
    }

    private enum Severity { Good, Warning, Bad }

    private static Severity Worst(Severity a, Severity b) => a > b ? a : b;
}
#endif
