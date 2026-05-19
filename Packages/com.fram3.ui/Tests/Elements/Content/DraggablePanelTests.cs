#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class DraggablePanelTests
    {
        private static TestLeafElement Body() => new("body");

        [Test]
        public void Constructor_StoresChild()
        {
            var body = Body();
            var panel = new DraggablePanel(body);

            Assert.That(panel.Child, Is.SameAs(body));
        }

        [Test]
        public void Constructor_NullChild_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new DraggablePanel(null!));
        }

        [Test]
        public void Constructor_DefaultInitialX()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel.InitialX, Is.EqualTo(100f));
        }

        [Test]
        public void Constructor_DefaultInitialY()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel.InitialY, Is.EqualTo(100f));
        }

        [Test]
        public void Constructor_StoresInitialX()
        {
            var panel = new DraggablePanel(Body(), initialX: 250f);

            Assert.That(panel.InitialX, Is.EqualTo(250f));
        }

        [Test]
        public void Constructor_StoresInitialY()
        {
            var panel = new DraggablePanel(Body(), initialY: 180f);

            Assert.That(panel.InitialY, Is.EqualTo(180f));
        }

        [Test]
        public void Constructor_DefaultTitle_IsNull()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel.Title, Is.Null);
        }

        [Test]
        public void Constructor_StoresTitle()
        {
            var panel = new DraggablePanel(Body(), title: "Stats");

            Assert.That(panel.Title, Is.EqualTo("Stats"));
        }

        [Test]
        public void Constructor_DefaultWidth_IsNull()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel.Width, Is.Null);
        }

        [Test]
        public void Constructor_StoresWidth()
        {
            var panel = new DraggablePanel(Body(), width: 320f);

            Assert.That(panel.Width, Is.EqualTo(320f));
        }

        [Test]
        public void Constructor_DefaultOnClose_IsNull()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel.OnClose, Is.Null);
        }

        [Test]
        public void Constructor_StoresOnClose()
        {
            Action cb = () => { };
            var panel = new DraggablePanel(Body(), onClose: cb);

            Assert.That(panel.OnClose, Is.SameAs(cb));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("panel");
            var panel = new DraggablePanel(Body(), key: key);

            Assert.That(panel.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var body = Body();
            var panel = new DraggablePanel(body);

            Assert.That(panel.GetChildren(), Has.Count.EqualTo(1));
            Assert.That(panel.GetChildren()[0], Is.SameAs(body));
        }

        [Test]
        public void ImplementsIRootAttachedElement()
        {
            var panel = new DraggablePanel(Body());

            Assert.That(panel, Is.InstanceOf<IRootAttachedElement>());
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var panel = new DraggablePanel(Body());

            Assert.DoesNotThrow(() => expander.Mount(panel, null));
        }

        [Test]
        public void Mounts_WithAllOptions_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var panel = new DraggablePanel(
                Body(),
                initialX: 200f,
                initialY: 150f,
                title: "Inventory",
                width: 400f,
                onClose: () => { }
            );

            Assert.DoesNotThrow(() => expander.Mount(panel, null));
        }
    }
}
