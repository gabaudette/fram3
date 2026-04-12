#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;

namespace Fram3.UI.Tests.Integration.Helpers
{
    /// <summary>
    /// Provides factory methods for assembling small element trees and expander/scheduler
    /// pairs used across integration test suites.
    /// </summary>
    internal static class TreeBuilder
    {
        /// <summary>
        /// Creates a fresh <see cref="FRebuildScheduler"/> and <see cref="FNodeExpander"/>
        /// wired to the given adapter. Both objects are ready to use immediately.
        /// </summary>
        internal static (FRebuildScheduler Scheduler, FNodeExpander Expander) MakePipeline(
            IRenderAdapter? adapter = null
        )
        {
            var scheduler = new FRebuildScheduler();
            var expander = new FNodeExpander(scheduler, adapter);
            return (scheduler, expander);
        }

        /// <summary>
        /// Mounts <paramref name="root"/> and returns the root <see cref="FNode"/>.
        /// </summary>
        internal static FNode Mount(FElement root, FNodeExpander expander)
        {
            return expander.Mount(root, null);
        }

        /// <summary>
        /// Flushes all pending rebuilds through <paramref name="expander"/>.
        /// </summary>
        internal static void Flush(FRebuildScheduler scheduler, FNodeExpander expander)
        {
            scheduler.Flush(expander);
        }
    }
}
