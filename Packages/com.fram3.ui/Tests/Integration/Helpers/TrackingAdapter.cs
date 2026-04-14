#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core.Internal;

namespace Fram3.UI.Tests.Integration.Helpers
{
    internal sealed class TrackingAdapter : IRenderAdapter
    {
        public enum EventKind { Mounted, Unmounting, Rebuilt }

        public readonly struct AdapterEvent
        {
            public EventKind Kind { get; }
            public Node Node { get; }

            public AdapterEvent(EventKind kind, Node node)
            {
                Kind = kind;
                Node = node;
            }
        }

        private readonly List<AdapterEvent> _events = new List<AdapterEvent>();

        public IReadOnlyList<AdapterEvent> Events => _events;

        public void OnMounted(Node node)
        {
            _events.Add(new AdapterEvent(EventKind.Mounted, node));
        }

        public void OnUnmounting(Node node)
        {
            _events.Add(new AdapterEvent(EventKind.Unmounting, node));
        }

        public void OnRebuilt(Node node)
        {
            _events.Add(new AdapterEvent(EventKind.Rebuilt, node));
        }

        public void Clear()
        {
            _events.Clear();
        }

        public IReadOnlyList<AdapterEvent> EventsOfKind(EventKind kind)
        {
            var result = new List<AdapterEvent>();
            foreach (var e in _events)
            {
                if (e.Kind == kind)
                {
                    result.Add(e);
                }
            }

            return result;
        }
    }
}
