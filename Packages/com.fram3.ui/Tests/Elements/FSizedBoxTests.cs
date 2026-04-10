#nullable enable
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FSizedBoxTests
    {
        [Test]
        public void FromSize_StoresWidthAndHeight()
        {
            var element = FSizedBox.FromSize(width: 100f, height: 50f);

            Assert.That(element.Width, Is.EqualTo(100f));
            Assert.That(element.Height, Is.EqualTo(50f));
            Assert.That(element.IsExpand, Is.False);
        }

        [Test]
        public void FromSize_NullDimensions_AreNull()
        {
            var element = FSizedBox.FromSize();

            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }

        [Test]
        public void Square_StoresEqualDimensions()
        {
            var element = FSizedBox.Square(32f);

            Assert.That(element.Width, Is.EqualTo(32f));
            Assert.That(element.Height, Is.EqualTo(32f));
            Assert.That(element.IsExpand, Is.False);
        }

        [Test]
        public void Expand_SetsIsExpand()
        {
            var element = FSizedBox.Expand();

            Assert.That(element.IsExpand, Is.True);
            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }
    }
}
