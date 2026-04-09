#nullable enable
using System;
using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class FTreePatcherTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [Test]
        public void Patch_EmptyToEmpty_LeavesChildrenEmpty()
        {
            var parent = new TestLeafElement("parent");
            var parentNode = _expander.Mount(parent, null);

            FTreePatcher.Patch(parentNode, Array.Empty<FElement>(), _expander);

            Assert.That(parentNode.Children, Is.Empty);
        }

        [Test]
        public void Patch_InsertsNewChild()
        {
            var parentElement = new TestMultiChildElement { Children = Array.Empty<FElement>() };
            var parentNode = _expander.Mount(parentElement, null);

            var newChild = new TestLeafElement("new");

            FTreePatcher.Patch(parentNode, new FElement[] { newChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(newChild));
        }

        [Test]
        public void Patch_RemovesOldChild()
        {
            var child = new TestLeafElement("child");
            var parentElement = new TestMultiChildElement { Children = new FElement[] { child } };
            var parentNode = _expander.Mount(parentElement, null);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));

            FTreePatcher.Patch(parentNode, Array.Empty<FElement>(), _expander);

            Assert.That(parentNode.Children, Is.Empty);
        }

        [Test]
        public void Patch_UpdatesChildInPlace()
        {
            var originalChild = new TestLeafElement("old");
            var parentElement = new TestMultiChildElement { Children = new FElement[] { originalChild } };
            var parentNode = _expander.Mount(parentElement, null);

            var originalChildNode = parentNode.Children[0];
            var updatedChild = new TestLeafElement("new");

            FTreePatcher.Patch(parentNode, new FElement[] { updatedChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0], Is.SameAs(originalChildNode));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(updatedChild));
        }

        [Test]
        public void Patch_RemovesUnmatchedAndInsertsNew()
        {
            var oldChild = new TestLeafElement("old");
            var parentElement = new TestMultiChildElement { Children = new FElement[] { oldChild } };
            var parentNode = _expander.Mount(parentElement, null);

            var newChild = new TestSingleChildElement { Child = new TestLeafElement("x") };

            FTreePatcher.Patch(parentNode, new FElement[] { newChild }, _expander);

            Assert.That(parentNode.Children.Count, Is.EqualTo(1));
            Assert.That(parentNode.Children[0].Element, Is.SameAs(newChild));
        }

        [Test]
        public void Patch_KeyedChild_MovedCorrectly()
        {
            var key = new FValueKey<int>(1);
            var keyedChild = new TestLeafElement("keyed", key);
            var unkeyedChild = new TestLeafElement("unkeyed");
            var parentElement = new TestMultiChildElement { Children = new FElement[] { keyedChild, unkeyedChild } };

            var parentNode = _expander.Mount(parentElement, null);
            var keyedNode = parentNode.Children[0];

            var newElements = new FElement[]
            {
                new TestLeafElement("unkeyed2"),
                new TestLeafElement("keyed2", key)
            };

            FTreePatcher.Patch(parentNode, newElements, _expander);

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

            var parentElement = new TestMultiChildElement { Children = new FElement[] { statefulChild } };
            var parentNode = _expander.Mount(parentElement, null);

            FTreePatcher.Patch(parentNode, Array.Empty<FElement>(), _expander);

            Assert.That(state != null && state.DisposeCalled, Is.True);
        }
    }
}
