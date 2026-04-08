using System;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FBuildContextTests
    {
        [Test]
        public void Element_ReturnsNodeElement()
        {
            var element = new TestLeafElement("test");
            var node = new FNode(element, null);

            Assert.That(node.Context.Element, Is.SameAs(element));
        }

        [Test]
        public void Depth_RootNode_IsZero()
        {
            var node = new FNode(new TestLeafElement("root"), null);

            Assert.That(node.Context.Depth, Is.EqualTo(0));
        }

        [Test]
        public void Depth_ChildNode_IsParentPlusOne()
        {
            var parentNode = new FNode(new TestLeafElement("parent"), null);
            var childNode = new FNode(new TestLeafElement("child"), parentNode);

            Assert.That(childNode.Context.Depth, Is.EqualTo(1));
        }

        [Test]
        public void Depth_GrandchildNode_IsTwo()
        {
            var root = new FNode(new TestLeafElement("root"), null);
            var child = new FNode(new TestLeafElement("child"), root);
            var grandchild = new FNode(new TestLeafElement("grandchild"), child);

            Assert.That(grandchild.Context.Depth, Is.EqualTo(2));
        }

        [Test]
        public void FindAncestorOfType_WhenAncestorExists_ReturnsIt()
        {
            var parentElement = new TestSingleChildElement { Child = new TestLeafElement("inner") };
            var parentNode = new FNode(parentElement, null);
            var childElement = new TestLeafElement("child");
            var childNode = new FNode(childElement, parentNode);

            var found = childNode.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(parentElement));
        }

        [Test]
        public void FindAncestorOfType_WhenNoAncestorExists_ReturnsNull()
        {
            var element = new TestLeafElement("alone");
            var node = new FNode(element, null);

            var found = node.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindAncestorOfType_SkipsNonMatchingAncestors()
        {
            var grandparentElement = new TestSingleChildElement { Child = new TestLeafElement("gp-inner") };
            var grandparentNode = new FNode(grandparentElement, null);

            var parentElement = new TestLeafElement("parent");
            var parentNode = new FNode(parentElement, grandparentNode);

            var childElement = new TestLeafElement("child");
            var childNode = new FNode(childElement, parentNode);

            var found = childNode.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(grandparentElement));
        }

        [Test]
        public void GetAncestorOfType_WhenAncestorExists_ReturnsIt()
        {
            var parentElement = new TestSingleChildElement { Child = new TestLeafElement("inner") };
            var parentNode = new FNode(parentElement, null);
            var childNode = new FNode(new TestLeafElement("child"), parentNode);

            var found = childNode.Context.GetAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(parentElement));
        }

        [Test]
        public void GetAncestorOfType_WhenNoAncestorExists_ThrowsInvalidOperationException()
        {
            var node = new FNode(new TestLeafElement("alone"), null);

            Assert.Throws<InvalidOperationException>(() =>
            {
                node.Context.GetAncestorOfType<TestSingleChildElement>();
            });
        }
    }
}
