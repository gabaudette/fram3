#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Tests.Mocks
{
    internal sealed class TestLeafElement : LeafElement
    {
        public string Label { get; }

        public TestLeafElement(string label, Key? key = null) : base(key)
        {
            Label = label;
        }
    }

    /// <summary>
    /// A leaf element used in render bridge tests. Named distinctly from
    /// <see cref="TestLeafElement"/> to keep rendering tests self-contained.
    /// </summary>
    internal sealed class TestRenderLeaf : LeafElement
    {
        public string Label { get; }

        public TestRenderLeaf(string label, Key? key = null) : base(key)
        {
            Label = label;
        }
    }

    internal sealed class TestSingleChildElement : SingleChildElement
    {
        public TestSingleChildElement(Key? key = null) : base(key)
        {
        }
    }

    internal sealed class TestMultiChildElement : MultiChildElement
    {
        public TestMultiChildElement(Key? key = null) : base(key)
        {
        }
    }

    internal sealed class TestStatelessElement : StatelessElement
    {
        private readonly Func<BuildContext, Element> _builder;

        public TestStatelessElement(Func<BuildContext, Element> builder, Key? key = null) : base(key)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override Element Build(BuildContext context)
        {
            return _builder(context);
        }
    }

    internal sealed class TestStatefulElement : StatefulElement
    {
        private readonly Func<State> _stateFactory;

        public string Config { get; }

        public TestStatefulElement(Func<State> stateFactory, string config = "", Key? key = null) : base(key)
        {
            _stateFactory = stateFactory ?? throw new ArgumentNullException(nameof(stateFactory));
            Config = config;
        }

        public override State CreateState()
        {
            return _stateFactory();
        }
    }

    internal sealed class TestState : State<TestStatefulElement>
    {
        public bool InitStateCalled { get; private set; }
        public bool DisposeCalled { get; private set; }
        public StatefulElement? LastOldElement { get; private set; }
        public int BuildCount { get; private set; }

        private readonly Func<BuildContext, Element> _builder;

        public TestState(Func<BuildContext, Element> builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override void InitState()
        {
            InitStateCalled = true;
        }

        public override Element Build(BuildContext context)
        {
            BuildCount++;
            return _builder(context);
        }

        public override void DidUpdateElement(StatefulElement oldElement)
        {
            LastOldElement = oldElement;
        }

        public override void Dispose()
        {
            DisposeCalled = true;
        }
    }
}
