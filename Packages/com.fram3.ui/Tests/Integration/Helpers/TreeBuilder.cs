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
        /// Creates a fresh <see cref="RebuildScheduler"/> and <see cref="NodeExpander"/>
        /// wired to the given adapter. Both objects are ready to use immediately.
        /// </summary>
        internal static (RebuildScheduler Scheduler, NodeExpander Expander) MakePipeline(
            IRenderAdapter? adapter = null
        )
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler, adapter);
            return (scheduler, expander);
        }

        /// <summary>
        /// Mounts <paramref name="root"/> and returns the root <see cref="Node"/>.
        /// </summary>
        internal static Node Mount(Element root, NodeExpander expander)
        {
            return expander.Mount(root, null);
        }

        /// <summary>
        /// Flushes all pending rebuilds through <paramref name="expander"/>.
        /// </summary>
        internal static void Flush(RebuildScheduler scheduler, NodeExpander expander)
        {
            scheduler.Flush(expander);
        }
    }
}
