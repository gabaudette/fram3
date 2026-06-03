#nullable enable
#if FRAM3_FRAMEWORK_DIAGNOSTICS
namespace Fram3.UI.Diagnostics
{
    /// <summary>
    /// Per-frame performance snapshot emitted by <see cref="Fram3Diagnostics.OnFrameMetrics"/>
    /// at the end of each <c>Renderer.Tick</c>. All timing values are in
    /// <see cref="System.Diagnostics.Stopwatch"/> ticks; divide by
    /// <see cref="System.Diagnostics.Stopwatch.Frequency"/> to get seconds.
    /// </summary>
    public struct FrameMetrics
    {
        /// <summary>Total time spent inside <c>RebuildScheduler.Flush</c>.</summary>
        public long FlushDurationTicks;

        /// <summary>
        /// Total time spent inside <c>TreeDiffer.Diff</c> across all rebuilds in this frame.
        /// </summary>
        public long DiffDurationTicks;

        /// <summary>Number of dirty nodes actually rebuilt by the scheduler.</summary>
        public int DirtyNodesFlushed;

        /// <summary>Number of nodes mounted (new elements inserted into the tree).</summary>
        public int NodesMounted;

        /// <summary>Number of nodes unmounted (elements removed from the tree).</summary>
        public int NodesUnmounted;

        /// <summary>
        /// Number of nodes rebuilt via <c>NodeExpander.Rebuild</c>. Includes both
        /// scheduler-initiated rebuilds and those triggered by Update/Move patch ops.
        /// </summary>
        public int NodesRebuilt;

        /// <summary>Insert operations produced by <c>TreeDiffer</c> this frame.</summary>
        public int DiffOpsInsert;

        /// <summary>
        /// Update operations produced by <c>TreeDiffer</c> this frame. High values
        /// relative to inserts indicate healthy key/positional matching.
        /// </summary>
        public int DiffOpsUpdate;

        /// <summary>Remove operations produced by <c>TreeDiffer</c> this frame.</summary>
        public int DiffOpsRemove;

        /// <summary>
        /// Move operations produced by <c>TreeDiffer</c> this frame. High values
        /// indicate that list order is changing frequently.
        /// </summary>
        public int DiffOpsMove;

        /// <summary>Number of exceptions thrown by <c>Build()</c> calls this frame.</summary>
        public int BuildExceptions;
    }
}
#endif