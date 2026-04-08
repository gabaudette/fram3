using System;
using Fram3.UI.Core;

namespace Fram3.UI.Tests.Mocks
{
    internal sealed class TestLeafElement : FLeafElement
    {
        public string Label { get; }

        public TestLeafElement(string label, FKey key = null) : base(key)
        {
            Label = label;
        }
    }

    internal sealed class TestSingleChildElement : FSingleChildElement
    {
        public TestSingleChildElement(FElement? child, FKey key = null) : base(child, key)
        {
        }
    }

    internal sealed class TestMultiChildElement : FMultiChildElement
    {
        public TestMultiChildElement(FElement?[] children, FKey key = null) : base(children, key)
        {
        }
    }

    internal sealed class TestStatelessElement : FStatelessElement
    {
        private readonly Func<FBuildContext, FElement> _builder;

        public TestStatelessElement(Func<FBuildContext, FElement> builder, FKey key = null) : base(key)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override FElement Build(FBuildContext context)
        {
            return _builder(context);
        }
    }

    internal sealed class TestStatefulElement : FStatefulElement
    {
        private readonly Func<FState> _stateFactory;

        public string Config { get; }

        public TestStatefulElement(Func<FState> stateFactory, string config = "", FKey key = null) : base(key)
        {
            _stateFactory = stateFactory ?? throw new ArgumentNullException(nameof(stateFactory));
            Config = config;
        }

        public override FState CreateState()
        {
            return _stateFactory();
        }
    }

    internal sealed class TestState : FState<TestStatefulElement>
    {
        public bool InitStateCalled { get; private set; }
        public bool DisposeCalled { get; private set; }
        public FStatefulElement LastOldElement { get; private set; }
        public int BuildCount { get; private set; }

        private readonly Func<FBuildContext, FElement> _builder;

        public TestState(Func<FBuildContext, FElement> builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public override void InitState()
        {
            InitStateCalled = true;
        }

        public override FElement Build(FBuildContext context)
        {
            BuildCount++;
            return _builder(context);
        }

        public override void DidUpdateElement(FStatefulElement oldElement)
        {
            LastOldElement = oldElement;
        }

        public override void Dispose()
        {
            DisposeCalled = true;
        }
    }
}
