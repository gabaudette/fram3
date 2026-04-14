#nullable enable
using System;
using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class TreePatcherTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        [Test]
        public void Patch_EmptyToEmpty_LeavesChildrenEmpty()
        {
            var parent = new TestLeafElement("parent");
            var parentNode = _expander.Mount(parent, null);

            TreePatcher.Patch(parentNode, Array.Empty<Element>(), _expander);

            Assert.That(parentNode.Children, Is.Empty);
        }

        [Test]
        public void Patch_InsertsNewChild()
        {
            var parentElement = new TestMultiChildElement { Children = Array.Empty<Element>() };
            var parentNode = _expander.Mount(parentElement, null);

            var newChild = new TestLeafElement("new");

            TreePatcher.Patch(parentNode, new Element[] { newChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(newChild));
        }

        [Test]
        public void Patch_RemovesOldChild()
        {
            var child = new TestLeafElement("child");
            var parentElement = new TestMultiChildElement { Children = new Element[] { child } };
            var parentNode = _expander.Mount(parentElement, null);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));

            TreePatcher.Patch(parentNode, Array.Empty<Element>(), _expander);

            Assert.That(parentNode.Children, Is.Empty);
        }

        [Test]
        public void Patch_UpdatesChildInPlace()
        {
            var originalChild = new TestLeafElement("old");
            var parentElement = new TestMultiChildElement { Children = new Element[] { originalChild } };
            var parentNode = _expander.Mount(parentElement, null);

            var originalChildNode = parentNode.Children[0];
            var updatedChild = new TestLeafElement("new");

            TreePatcher.Patch(parentNode, new Element[] { updatedChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0], Is.SameAs(originalChildNode));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(updatedChild));
        }

        [Test]
        public void Patch_RemovesUnmatchedAndInsertsNew()
        {
            var oldChild = new TestLeafElement("old");
            var parentElement = new TestMultiChildElement { Children = new Element[] { oldChild } };
            var parentNode = _expander.Mount(parentElement, null);

            var newChild = new TestSingleChildElement { Child = new TestLeafElement("x") };

            TreePatcher.Patch(parentNode, new Element[] { newChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(newChild));
        }

        [Test]
        public void Patch_KeyedChild_MovedCorrectly()
        {
            var key = new ValueKey<int>(1);
            var keyedChild = new TestLeafElement("keyed", key);
            var unkeyedChild = new TestLeafElement("unkeyed");
            var parentElement = new TestMultiChildElement { Children = new Element[] { keyedChild, unkeyedChild } };

            var parentNode = _expander.Mount(parentElement, null);
            var keyedNode = parentNode.Children[0];

            var newElements = new Element[]
            {
                new TestLeafElement("unkeyed2"),
                new TestLeafElement("keyed2", key)
            };

            TreePatcher.Patch(parentNode, newElements, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(2));
            Assert.That(parentNode.Children[1], Is.SameAs(keyedNode));
        }

        [Test]
        public void Patch_UnmountedChildState_IsDisposed()
        {
            TestState? state = null;
            var statefulChild = new TestStatefulElement(() =>
            {
                state = new TestState(_ => new TestLeafElement("x"));
                return state;
            });

            var parentElement = new TestMultiChildElement { Children = new Element[] { statefulChild } };
            var parentNode = _expander.Mount(parentElement, null);

            TreePatcher.Patch(parentNode, Array.Empty<Element>(), _expander);

            Assert.That(state != null && state.DisposeCalled, Is.True);
        }
    }
}
