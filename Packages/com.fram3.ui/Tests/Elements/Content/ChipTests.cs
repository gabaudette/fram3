#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class ChipTests
    {
        [Test]
        public void Constructor_StoresLabel()
        {
            var chip = new Chip("Active");

            Assert.That(chip.Label, Is.EqualTo("Active"));
        }

        [Test]
        public void Constructor_NullLabel_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new Chip(null!));
        }

        [Test]
        public void Constructor_DefaultOnDeleted_IsNull()
        {
            var chip = new Chip("Active");

            Assert.That(chip.OnDeleted, Is.Null);
        }

        [Test]
        public void Constructor_StoresOnDeleted()
        {
            Action callback = () => { };
            var chip = new Chip("Active", onDeleted: callback);

            Assert.That(chip.OnDeleted, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultColor_IsNull()
        {
            var chip = new Chip("Active");

            Assert.That(chip.Color, Is.Null);
        }

        [Test]
        public void Constructor_StoresColor()
        {
            var color = FrameColor.FromHex("#7B61FF");
            var chip = new Chip("Active", color: color);

            Assert.That(chip.Color, Is.EqualTo(color));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("chip");
            var chip = new Chip("Active", key: key);

            Assert.That(chip.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsNoDirectChildren()
        {
            var chip = new Chip("Active");

            Assert.That(chip.GetChildren(), Has.Count.EqualTo(0));
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var chip = new Chip("Active");

            Assert.DoesNotThrow(() => expander.Mount(chip, null));
        }

        [Test]
        public void Mounts_WithDeleteCallback_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var chip = new Chip("Active", onDeleted: () => { });

            Assert.DoesNotThrow(() => expander.Mount(chip, null));
        }
    }
}
