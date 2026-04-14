#nullable enable
using Fram3.UI.Elements.Layout;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class SizedBoxTests
    {
        [Test]
        public void FromSize_StoresWidthAndHeight()
        {
            var element = SizedBox.FromSize(width: 100f, height: 50f);

            Assert.That(element.Width, Is.EqualTo(100f));
            Assert.That(element.Height, Is.EqualTo(50f));
            Assert.That(element.IsExpand, Is.False);
        }

        [Test]
        public void FromSize_NullDimensions_AreNull()
        {
            var element = SizedBox.FromSize();

            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }

        [Test]
        public void Square_StoresEqualDimensions()
        {
            var element = SizedBox.Square(32f);

            Assert.That(element.Width, Is.EqualTo(32f));
            Assert.That(element.Height, Is.EqualTo(32f));
            Assert.That(element.IsExpand, Is.False);
        }

        [Test]
        public void Expand_SetsIsExpand()
        {
            var element = SizedBox.Expand();

            Assert.That(element.IsExpand, Is.True);
            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }
    }
}