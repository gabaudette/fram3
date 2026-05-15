#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class StatelessElementTests
    {
        [Test]
        public void Build_ReturnsElementFromBuilder()
        {
            var expected = new TestLeafElement("result");
            var element = new TestStatelessElement(_ => expected);
            var node = new Node(element, null);

            var result = element.Build(node.Context);

            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void Build_ReceivesBuildContext()
        {
            BuildContext? receivedContext = null;
            var element = new TestStatelessElement(ctx =>
            {
                receivedContext = ctx;
                return new TestLeafElement("result");
            });

            var node = new Node(element, null);

            element.Build(node.Context);

            Assert.That(receivedContext, Is.Not.Null);
            Assert.That(receivedContext, Is.SameAs(node.Context));
        }

        [Test]
        public void Key_IsPassedThrough()
        {
            var key = new ValueKey<string>("my-key");
            var element = new TestStatelessElement(_ => new TestLeafElement("r"), key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void ShouldRebuild_DefaultReturnsTrue()
        {
            var element = new TestStatelessElement(_ => new TestLeafElement("x"));
            var result = element.ShouldRebuild(element, element);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldRebuild_ReturnsFalse_SkipsRebuild()
        {
            var (scheduler, expander) = TreeBuilder.MakePipeline();
            int buildCount = 0;

            var statelessElement = new MemoStatelessElement(
                _ =>
                {
                    buildCount++;
                    return new TestLeafElement("leaf");
                },
                shouldRebuild: false
            );

            var parentState = new DriveState(() => statelessElement);
            var root = expander.Mount(new TestStatefulElement(() => parentState), null);

            parentState.TriggerRebuild();
            TreeBuilder.Flush(scheduler, expander);

            Assert.That(buildCount, Is.EqualTo(1));
        }

        [Test]
        public void ShouldRebuild_ReturnsTrue_RebuildOccurs()
        {
            var (scheduler, expander) = TreeBuilder.MakePipeline();
            int buildCount = 0;

            var statelessElement = new MemoStatelessElement(
                _ =>
                {
                    buildCount++;
                    return new TestLeafElement("leaf");
                },
                shouldRebuild: true
            );

            var parentState = new DriveState(() => statelessElement);
            expander.Mount(new TestStatefulElement(() => parentState), null);

            parentState.TriggerRebuild();
            TreeBuilder.Flush(scheduler, expander);

            Assert.That(buildCount, Is.EqualTo(2));
        }

        private sealed class MemoStatelessElement : StatelessElement
        {
            private readonly Func<BuildContext, Element> _builder;
            private readonly bool _shouldRebuild;

            public MemoStatelessElement(Func<BuildContext, Element> builder, bool shouldRebuild)
            {
                _builder = builder;
                _shouldRebuild = shouldRebuild;
            }

            public override Element Build(BuildContext context) => _builder(context);

            public override bool ShouldRebuild(StatelessElement oldElement, StatelessElement newElement)
                => _shouldRebuild;
        }

        private sealed class DriveState : State
        {
            private readonly Func<Element> _elementFactory;

            public DriveState(Func<Element> elementFactory)
            {
                _elementFactory = elementFactory;
            }

            public void TriggerRebuild() => SetState(null);

            public override Element Build(BuildContext context) => _elementFactory();
        }
    }
}
