#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FSpinnerTests
    {
        [Test]
        public void DefaultSize_IsThirtyTwo()
        {
            var spinner = new FSpinner();

            Assert.That(spinner.Size, Is.EqualTo(32f));
        }

        [Test]
        public void DefaultStrokeWidth_IsFour()
        {
            var spinner = new FSpinner();

            Assert.That(spinner.StrokeWidth, Is.EqualTo(4f));
        }

        [Test]
        public void DefaultColor_IsNull()
        {
            var spinner = new FSpinner();

            Assert.That(spinner.Color, Is.Null);
        }

        [Test]
        public void DefaultSpeed_IsOne()
        {
            var spinner = new FSpinner();

            Assert.That(spinner.Speed, Is.EqualTo(1f));
        }

        [Test]
        public void CustomSize_IsStored()
        {
            var spinner = new FSpinner(size: 64f);

            Assert.That(spinner.Size, Is.EqualTo(64f));
        }

        [Test]
        public void CustomStrokeWidth_IsStored()
        {
            var spinner = new FSpinner(strokeWidth: 8f);

            Assert.That(spinner.StrokeWidth, Is.EqualTo(8f));
        }

        [Test]
        public void CustomColor_IsStored()
        {
            var color = new FColor(0f, 0.5f, 1f);
            var spinner = new FSpinner(color: color);

            Assert.That(spinner.Color, Is.EqualTo(color));
        }

        [Test]
        public void CustomSpeed_IsStored()
        {
            var spinner = new FSpinner(speed: 2f);

            Assert.That(spinner.Speed, Is.EqualTo(2f));
        }

        [Test]
        public void Key_IsStored()
        {
            var key = new FValueKey<string>("spin");
            var spinner = new FSpinner(key: key);

            Assert.That(spinner.Key, Is.EqualTo(key));
        }
    }
}
