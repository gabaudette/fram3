using System;
using Fram3.UI.Core;
using Fram3.UI.Rendering;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Rendering
{
    [TestFixture]
    internal sealed class FRootElementTests
    {
        [Test]
        public void Child_StoresProvidedElement()
        {
            var leaf = new TestRenderLeaf("a");
            var root = new FRootElement { Child = leaf };

            Assert.That(root.Child, Is.SameAs(leaf));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var leaf = new TestRenderLeaf("b");
            var root = new FRootElement { Child = leaf };

            var children = root.GetChildren();

            Assert.That(children, Has.Count.EqualTo(1));
            Assert.That(children[0], Is.SameAs(leaf));
        }

        [Test]
        public void NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new FRootElement { Child = null! };
            });
        }
    }
}
