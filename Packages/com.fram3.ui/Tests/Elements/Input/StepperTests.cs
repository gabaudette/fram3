#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Input;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class StepperTests
    {
        // --- Constructor defaults ---

        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var stepper = new Stepper();
            Assert.That(stepper.Value, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var stepper = new Stepper();
            Assert.That(stepper.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_DefaultMin_IsNull()
        {
            var stepper = new Stepper();
            Assert.That(stepper.Min, Is.Null);
        }

        [Test]
        public void Constructor_DefaultMax_IsNull()
        {
            var stepper = new Stepper();
            Assert.That(stepper.Max, Is.Null);
        }

        [Test]
        public void Constructor_DefaultStep_IsOne()
        {
            var stepper = new Stepper();
            Assert.That(stepper.Step, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var stepper = new Stepper();
            Assert.That(stepper.Label, Is.Null);
        }

        // --- Constructor stores ---

        [Test]
        public void Constructor_StoresValue()
        {
            var stepper = new Stepper(value: 5);
            Assert.That(stepper.Value, Is.EqualTo(5));
        }

        [Test]
        public void Constructor_StoresOnChanged()
        {
            Action<int> cb = _ => { };
            var stepper = new Stepper(onChanged: cb);
            Assert.That(stepper.OnChanged, Is.SameAs(cb));
        }

        [Test]
        public void Constructor_StoresMin()
        {
            var stepper = new Stepper(min: 0);
            Assert.That(stepper.Min, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_StoresMax()
        {
            var stepper = new Stepper(max: 10);
            Assert.That(stepper.Max, Is.EqualTo(10));
        }

        [Test]
        public void Constructor_StoresStep()
        {
            var stepper = new Stepper(step: 5);
            Assert.That(stepper.Step, Is.EqualTo(5));
        }

        [Test]
        public void Constructor_StoresLabel()
        {
            var stepper = new Stepper(label: "Quantity");
            Assert.That(stepper.Label, Is.EqualTo("Quantity"));
        }

        // --- Constructor validation ---

        [Test]
        public void Constructor_StepZero_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Stepper(step: 0));
        }

        [Test]
        public void Constructor_StepNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Stepper(step: -1));
        }

        [Test]
        public void Constructor_MinGreaterThanMax_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Stepper(min: 10, max: 5));
        }

        [Test]
        public void Constructor_MinEqualsMax_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new Stepper(min: 5, max: 5));
        }

        // --- GetChildren ---

        [Test]
        public void GetChildren_IsEmpty()
        {
            var stepper = new Stepper();
            Assert.That(stepper.GetChildren(), Is.Empty);
        }

        // --- CreateState ---

        [Test]
        public void CreateState_ReturnsNonNull()
        {
            var stepper = new Stepper();
            Assert.That(stepper.CreateState(), Is.Not.Null);
        }

        [Test]
        public void CreateState_ReturnsDifferentInstances()
        {
            var stepper = new Stepper();
            Assert.That(stepper.CreateState(), Is.Not.SameAs(stepper.CreateState()));
        }

        // --- Mount / Build ---

        [Test]
        public void Mount_Default_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Stepper(), null));
        }

        [Test]
        public void Build_Default_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var stepper = new Stepper();
            var node = expander.Mount(stepper, null);
            var built = ((State<Stepper>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithLabel_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Stepper(label: "Qty"), null));
        }

        [Test]
        public void Build_WithLabel_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var stepper = new Stepper(label: "Qty");
            var node = expander.Mount(stepper, null);
            var built = ((State<Stepper>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithMinMax_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Stepper(value: 5, min: 0, max: 10), null));
        }

        [Test]
        public void Build_WithMinMax_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var stepper = new Stepper(value: 5, min: 0, max: 10);
            var node = expander.Mount(stepper, null);
            var built = ((State<Stepper>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_Disabled_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Stepper(onChanged: null), null));
        }

        [Test]
        public void Build_AtMin_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var stepper = new Stepper(value: 0, min: 0, max: 10, onChanged: _ => { });
            var node = expander.Mount(stepper, null);
            var built = ((State<Stepper>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_AtMax_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var stepper = new Stepper(value: 10, min: 0, max: 10, onChanged: _ => { });
            var node = expander.Mount(stepper, null);
            var built = ((State<Stepper>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithStep_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Stepper(step: 5), null));
        }

        // --- DidUpdateElement / Rebuild ---

        [Test]
        public void DidUpdateElement_ChangeValue_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Stepper(value: 0), null);
            expander.UpdateElement(node, new Stepper(value: 1));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ChangeLabel_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Stepper(label: "A"), null);
            expander.UpdateElement(node, new Stepper(label: "B"));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_EnableDisable_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Stepper(), null);
            expander.UpdateElement(node, new Stepper(onChanged: _ => { }));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        // --- Unmount ---

        [Test]
        public void Unmount_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Stepper(), null);
            Assert.DoesNotThrow(() => expander.Unmount(node));
        }
    }
}
