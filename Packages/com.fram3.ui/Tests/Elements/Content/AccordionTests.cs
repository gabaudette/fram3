#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class AccordionTests
    {
        private static AccordionItem Item(string header = "Header", string label = "body")
            => new AccordionItem(header, new TestLeafElement(label));

        private static IReadOnlyList<AccordionItem> TwoItems()
            => new[] { Item("A", "a"), Item("B", "b") };

        private static IReadOnlyList<AccordionItem> ThreeItems()
            => new[] { Item("A", "a"), Item("B", "b"), Item("C", "c") };

        // ── AccordionItem constructor ──────────────────────────────────────────

        [Test]
        public void AccordionItem_Constructor_StoresHeader()
        {
            var item = new AccordionItem("My Header", new TestLeafElement("x"));

            Assert.That(item.Header, Is.EqualTo("My Header"));
        }

        [Test]
        public void AccordionItem_Constructor_StoresContent()
        {
            var content = new TestLeafElement("x");
            var item = new AccordionItem("H", content);

            Assert.That(item.Content, Is.SameAs(content));
        }

        [Test]
        public void AccordionItem_Constructor_NullHeader_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new AccordionItem(null!, new TestLeafElement("x")));
        }

        [Test]
        public void AccordionItem_Constructor_NullContent_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new AccordionItem("H", null!));
        }

        // ── Accordion constructor ──────────────────────────────────────────────

        [Test]
        public void Constructor_StoresItems()
        {
            var items = TwoItems();
            var accordion = new Accordion(items);

            Assert.That(accordion.Items, Is.SameAs(items));
        }

        [Test]
        public void Constructor_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new Accordion(null!));
        }

        [Test]
        public void Constructor_DefaultAllowMultiple_IsFalse()
        {
            var accordion = new Accordion(TwoItems());

            Assert.That(accordion.AllowMultiple, Is.False);
        }

        [Test]
        public void Constructor_StoresAllowMultiple_True()
        {
            var accordion = new Accordion(TwoItems(), allowMultiple: true);

            Assert.That(accordion.AllowMultiple, Is.True);
        }

        [Test]
        public void Constructor_DefaultInitialIndex_IsMinusOne()
        {
            var accordion = new Accordion(TwoItems());

            Assert.That(accordion.InitialIndex, Is.EqualTo(-1));
        }

        [Test]
        public void Constructor_StoresInitialIndex()
        {
            var accordion = new Accordion(TwoItems(), initialIndex: 1);

            Assert.That(accordion.InitialIndex, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("acc");
            var accordion = new Accordion(TwoItems(), key: key);

            Assert.That(accordion.Key, Is.EqualTo(key));
        }

        [Test]
        public void Constructor_EmptyItems_IsAllowed()
        {
            Assert.DoesNotThrow(() => _ = new Accordion(Array.Empty<AccordionItem>()));
        }

        // ── Mounting ──────────────────────────────────────────────────────────

        [Test]
        public void Mounts_WithEmptyItems_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Accordion(Array.Empty<AccordionItem>()), null));
        }

        [Test]
        public void Mounts_WithItems_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Accordion(TwoItems()), null));
        }

        [Test]
        public void Mounts_WithInitialIndex_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Accordion(TwoItems(), initialIndex: 0), null));
        }

        [Test]
        public void Mounts_WithAllowMultiple_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Accordion(TwoItems(), allowMultiple: true), null));
        }

        [Test]
        public void Mounts_WithThreeItems_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Accordion(ThreeItems()), null));
        }

        // ── GetChildren (StatefulElement returns empty) ───────────────────────

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var accordion = new Accordion(TwoItems());

            Assert.That(accordion.GetChildren(), Is.Empty);
        }

        // ── InitialIndex behaviour ────────────────────────────────────────────

        [Test]
        public void InitialIndex_MinusOne_AllItemsCollapsed()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = ThreeItems();
            var accordion = new Accordion(items, initialIndex: -1);

            var node = expander.Mount(accordion, null);
            var state = (State<Accordion>)node.State!;

            var built = state.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void InitialIndex_Zero_FirstItemExpanded()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems(), initialIndex: 0);

            Assert.DoesNotThrow(() => expander.Mount(accordion, null));
        }

        [Test]
        public void InitialIndex_Last_LastItemExpanded()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = ThreeItems();
            var accordion = new Accordion(items, initialIndex: 2);

            Assert.DoesNotThrow(() => expander.Mount(accordion, null));
        }

        [Test]
        public void InitialIndex_OutOfRange_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems(), initialIndex: 99);

            Assert.DoesNotThrow(() => expander.Mount(accordion, null));
        }

        [Test]
        public void InitialIndex_OnEmptyList_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(Array.Empty<AccordionItem>(), initialIndex: 0);

            Assert.DoesNotThrow(() => expander.Mount(accordion, null));
        }

        // ── Toggle behaviour (single-select) ─────────────────────────────────

        [Test]
        public void Toggle_OpensItem_WhenAllCollapsed()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = TwoItems();
            var accordion = new Accordion(items, initialIndex: -1);

            var node = expander.Mount(accordion, null);
            var state = node.State!;

            state.SetState(() => { });
            scheduler.Flush(expander);

            Assert.That(node.State, Is.Not.Null);
        }

        [Test]
        public void Toggle_SingleSelect_ClosesOtherItemWhenNewOneOpens()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = TwoItems();
            var accordion = new Accordion(items, allowMultiple: false, initialIndex: 0);

            var node = expander.Mount(accordion, null);

            Assert.DoesNotThrow(() => scheduler.Flush(expander));
        }

        [Test]
        public void Toggle_MultiSelect_AllowsBothItemsOpen()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = TwoItems();
            var accordion = new Accordion(items, allowMultiple: true, initialIndex: 0);

            var node = expander.Mount(accordion, null);

            Assert.DoesNotThrow(() => scheduler.Flush(expander));
        }

        // ── Build output structure ────────────────────────────────────────────

        [Test]
        public void Build_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems());
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithNoItems_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(Array.Empty<AccordionItem>());
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithSingleItem_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(new[] { Item() });
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithThreeItems_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(ThreeItems());
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithExpandedItem_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems(), initialIndex: 0);
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_AllItemsExpanded_MultiSelect_ReturnsNonNullElement()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems(), allowMultiple: true, initialIndex: 0);
            var node = expander.Mount(accordion, null);

            var built = ((State<Accordion>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        // ── DidUpdateElement ──────────────────────────────────────────────────

        [Test]
        public void DidUpdateElement_SameItemCount_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems());
            var node = expander.Mount(accordion, null);

            var newAccordion = new Accordion(TwoItems());
            expander.UpdateElement(node, newAccordion);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_MoreItems_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems());
            var node = expander.Mount(accordion, null);

            var newAccordion = new Accordion(ThreeItems());
            expander.UpdateElement(node, newAccordion);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_FewerItems_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(ThreeItems());
            var node = expander.Mount(accordion, null);

            var newAccordion = new Accordion(TwoItems());
            expander.UpdateElement(node, newAccordion);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ToEmptyItems_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var accordion = new Accordion(TwoItems());
            var node = expander.Mount(accordion, null);

            var newAccordion = new Accordion(Array.Empty<AccordionItem>());
            expander.UpdateElement(node, newAccordion);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        // ── Unmount ───────────────────────────────────────────────────────────

        [Test]
        public void Unmount_WithoutExpanding_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Accordion(TwoItems()), null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        [Test]
        public void Unmount_WithExpandedItem_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Accordion(TwoItems(), initialIndex: 0), null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        [Test]
        public void Unmount_WithEmptyList_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Accordion(Array.Empty<AccordionItem>()), null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        // ── CreateState ───────────────────────────────────────────────────────

        [Test]
        public void CreateState_ReturnsNonNullState()
        {
            var accordion = new Accordion(TwoItems());

            Assert.That(accordion.CreateState(), Is.Not.Null);
        }

        [Test]
        public void CreateState_ReturnsDifferentInstances()
        {
            var accordion = new Accordion(TwoItems());

            var s1 = accordion.CreateState();
            var s2 = accordion.CreateState();

            Assert.That(s1, Is.Not.SameAs(s2));
        }
    }
}
