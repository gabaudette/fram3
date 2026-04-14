using System;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class BuildContextTests
    {
        [Test]
        public void Element_ReturnsNodeElement()
        {
            var element = new TestLeafElement("test");
            var node = new Node(element, null);

            Assert.That(node.Context.Element, Is.SameAs(element));
        }

        [Test]
        public void Depth_RootNode_IsZero()
        {
            var node = new Node(new TestLeafElement("root"), null);

            Assert.That(node.Context.Depth, Is.EqualTo(0));
        }

        [Test]
        public void Depth_ChildNode_IsParentPlusOne()
        {
            var parentNode = new Node(new TestLeafElement("parent"), null);
            var childNode = new Node(new TestLeafElement("child"), parentNode);

            Assert.That(childNode.Context.Depth, Is.EqualTo(1));
        }

        [Test]
        public void Depth_GrandchildNode_IsTwo()
        {
            var root = new Node(new TestLeafElement("root"), null);
            var child = new Node(new TestLeafElement("child"), root);
            var grandchild = new Node(new TestLeafElement("grandchild"), child);

            Assert.That(grandchild.Context.Depth, Is.EqualTo(2));
        }

        [Test]
        public void FindAncestorOfType_WhenAncestorExists_ReturnsIt()
        {
            var parentElement = new TestSingleChildElement { Child = new TestLeafElement("inner") };
            var parentNode = new Node(parentElement, null);
            var childElement = new TestLeafElement("child");
            var childNode = new Node(childElement, parentNode);

            var found = childNode.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(parentElement));
        }

        [Test]
        public void FindAncestorOfType_WhenNoAncestorExists_ReturnsNull()
        {
            var element = new TestLeafElement("alone");
            var node = new Node(element, null);

            var found = node.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindAncestorOfType_SkipsNonMatchingAncestors()
        {
            var grandparentElement = new TestSingleChildElement { Child = new TestLeafElement("gp-inner") };
            var grandparentNode = new Node(grandparentElement, null);

            var parentElement = new TestLeafElement("parent");
            var parentNode = new Node(parentElement, grandparentNode);

            var childElement = new TestLeafElement("child");
            var childNode = new Node(childElement, parentNode);

            var found = childNode.Context.FindAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(grandparentElement));
        }

        [Test]
        public void GetAncestorOfType_WhenAncestorExists_ReturnsIt()
        {
            var parentElement = new TestSingleChildElement { Child = new TestLeafElement("inner") };
            var parentNode = new Node(parentElement, null);
            var childNode = new Node(new TestLeafElement("child"), parentNode);

            var found = childNode.Context.GetAncestorOfType<TestSingleChildElement>();

            Assert.That(found, Is.SameAs(parentElement));
        }

        [Test]
        public void GetAncestorOfType_WhenNoAncestorExists_ThrowsInvalidOperationException()
        {
            var node = new Node(new TestLeafElement("alone"), null);

            Assert.Throws<InvalidOperationException>(() =>
            {
                node.Context.GetAncestorOfType<TestSingleChildElement>();
            });
        }
    }
}
