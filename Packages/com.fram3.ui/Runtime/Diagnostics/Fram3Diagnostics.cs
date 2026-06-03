#nullable enable
#if FRAM3_FRAMEWORK_DIAGNOSTICS
using System;
using System.Collections.Generic;

namespace Fram3.UI.Diagnostics
{
    /// <summary>
    /// Entry point for framework-level performance diagnostics. Subscribe to
    /// <see cref="OnFrameMetrics"/> to receive a <see cref="FrameMetrics"/> snapshot
    /// at the end of every <c>Renderer.Tick</c> call.
    /// <para>
    /// Only available when the <c>FRAM3_FRAMEWORK_DIAGNOSTICS</c> scripting define is set.
    /// This is intentional: diagnostics instrumentation is for framework development only
    /// and must not be exposed to end users of the package.
    /// </para>
    /// <para>
    /// When multiple <c>Renderer</c> instances are active in the same frame their metrics
    /// are accumulated into a single snapshot per tick, ordered by call sequence.
    /// </para>
    /// </summary>
    public static class Fram3Diagnostics
    {
        /// <summary>
        /// Raised at the end of each <c>Renderer.Tick</c> with a snapshot of that
        /// frame's reconciler activity. The snapshot is reset at the start of the next tick.
        /// </summary>
        public static event Action<FrameMetrics>? OnFrameMetrics;

        /// <summary>
        /// The distinct element type names that were rebuilt during the last frame,
        /// in the order they were first encountered. Reset at the start of each tick.
        /// </summary>
        public static IReadOnlyList<string> LastFrameRebuiltTypes => _rebuiltTypes;

        internal static FrameMetrics CurrentFrame;

        private static readonly List<string> _rebuiltTypes = new();

        internal static void TrackRebuild(string elementTypeName)
        {
            CurrentFrame.NodesRebuilt++;
            if (!_rebuiltTypes.Contains(elementTypeName))
            {
                _rebuiltTypes.Add(elementTypeName);
            }
        }

        internal static void ResetFrame()
        {
            CurrentFrame = default;
            _rebuiltTypes.Clear();
        }

        internal static void Emit() => OnFrameMetrics?.Invoke(CurrentFrame);
    }
}
#endif
