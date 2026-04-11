#nullable enable
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    internal sealed class FThemeTests
    {
        [Test]
        public void Default_IsNotNull()
        {
            Assert.That(FTheme.Default, Is.Not.Null);
        }

        [Test]
        public void Default_HasPositiveFontSize()
        {
            Assert.That(FTheme.Default.FontSize, Is.GreaterThan(0f));
        }

        [Test]
        public void Default_FontSizeSmall_LessThanFontSize()
        {
            Assert.That(FTheme.Default.FontSizeSmall, Is.LessThan(FTheme.Default.FontSize));
        }

        [Test]
        public void Default_FontSizeLarge_GreaterThanFontSize()
        {
            Assert.That(FTheme.Default.FontSizeLarge, Is.GreaterThan(FTheme.Default.FontSize));
        }

        [Test]
        public void Default_HasPositiveBorderRadius()
        {
            Assert.That(FTheme.Default.BorderRadius, Is.GreaterThanOrEqualTo(0f));
        }

        [Test]
        public void Default_HasPositiveSpacing()
        {
            Assert.That(FTheme.Default.Spacing, Is.GreaterThan(0f));
        }

        [Test]
        public void WithSyntax_OverridesFontSize()
        {
            var custom = FTheme.Default with { FontSize = 18f };

            Assert.That(custom.FontSize, Is.EqualTo(18f).Within(0.0001f));
        }

        [Test]
        public void WithSyntax_DoesNotMutateOriginal()
        {
            var original = FTheme.Default.FontSize;
            var _ = FTheme.Default with { FontSize = 99f };

            Assert.That(FTheme.Default.FontSize, Is.EqualTo(original).Within(0.0001f));
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new FTheme
            {
                PrimaryColor = FColor.White,
                SecondaryColor = FColor.Black,
                BackgroundColor = FColor.White,
                SurfaceColor = FColor.White,
                OnPrimaryColor = FColor.White,
                ErrorColor = FColor.Red,
                PrimaryTextColor = FColor.Black,
                SecondaryTextColor = FColor.Black,
                DisabledTextColor = FColor.Black,
                FontSize = 14f,
                FontSizeSmall = 12f,
                FontSizeLarge = 20f,
                BorderRadius = 4f,
                Spacing = 8f
            };

            var b = new FTheme
            {
                PrimaryColor = FColor.White,
                SecondaryColor = FColor.Black,
                BackgroundColor = FColor.White,
                SurfaceColor = FColor.White,
                OnPrimaryColor = FColor.White,
                ErrorColor = FColor.Red,
                PrimaryTextColor = FColor.Black,
                SecondaryTextColor = FColor.Black,
                DisabledTextColor = FColor.Black,
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
            var a = FTheme.Default with { FontSize = 14f };
            var b = FTheme.Default with { FontSize = 16f };

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
