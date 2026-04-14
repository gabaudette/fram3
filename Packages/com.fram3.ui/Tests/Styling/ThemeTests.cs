#nullable enable
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    internal sealed class ThemeTests
    {
        [Test]
        public void Default_IsNotNull()
        {
            Assert.That(Theme.Default, Is.Not.Null);
        }

        [Test]
        public void Default_HasPositiveFontSize()
        {
            Assert.That(Theme.Default.FontSize, Is.GreaterThan(0f));
        }

        [Test]
        public void Default_FontSizeSmall_LessThanFontSize()
        {
            Assert.That(Theme.Default.FontSizeSmall, Is.LessThan(Theme.Default.FontSize));
        }

        [Test]
        public void Default_FontSizeLarge_GreaterThanFontSize()
        {
            Assert.That(Theme.Default.FontSizeLarge, Is.GreaterThan(Theme.Default.FontSize));
        }

        [Test]
        public void Default_HasPositiveBorderRadius()
        {
            Assert.That(Theme.Default.BorderRadius, Is.GreaterThanOrEqualTo(0f));
        }

        [Test]
        public void Default_HasPositiveSpacing()
        {
            Assert.That(Theme.Default.Spacing, Is.GreaterThan(0f));
        }

        [Test]
        public void WithSyntax_OverridesFontSize()
        {
            var custom = Theme.Default with { FontSize = 18f };

            Assert.That(custom.FontSize, Is.EqualTo(18f).Within(0.0001f));
        }

        [Test]
        public void WithSyntax_DoesNotMutateOriginal()
        {
            var original = Theme.Default.FontSize;
            var _ = Theme.Default with { FontSize = 99f };

            Assert.That(Theme.Default.FontSize, Is.EqualTo(original).Within(0.0001f));
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new Theme
            {
                PrimaryColor = FrameColor.White,
                SecondaryColor = FrameColor.Black,
                BackgroundColor = FrameColor.White,
                SurfaceColor = FrameColor.White,
                OnPrimaryColor = FrameColor.White,
                ErrorColor = FrameColor.Red,
                PrimaryTextColor = FrameColor.Black,
                SecondaryTextColor = FrameColor.Black,
                DisabledTextColor = FrameColor.Black,
                FontSize = 14f,
                FontSizeSmall = 12f,
                FontSizeLarge = 20f,
                BorderRadius = 4f,
                Spacing = 8f
            };

            var b = new Theme
            {
                PrimaryColor = FrameColor.White,
                SecondaryColor = FrameColor.Black,
                BackgroundColor = FrameColor.White,
                SurfaceColor = FrameColor.White,
                OnPrimaryColor = FrameColor.White,
                ErrorColor = FrameColor.Red,
                PrimaryTextColor = FrameColor.Black,
                SecondaryTextColor = FrameColor.Black,
                DisabledTextColor = FrameColor.Black,
                FontSize = 14f,
                FontSizeSmall = 12f,
                FontSizeLarge = 20f,
                BorderRadius = 4f,
                Spacing = 8f
            };

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentFontSize_AreNotEqual()
        {
            var a = Theme.Default with { FontSize = 14f };
            var b = Theme.Default with { FontSize = 16f };

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
