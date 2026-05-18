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
    internal sealed class TreeViewTests
    {
        private static TreeViewNode Leaf(string label = "Leaf", string? icon = null)
            => new TreeViewNode(label, icon: icon);

        private static TreeViewNode Branch(string label, params TreeViewNode[] children)
            => new TreeViewNode(label, children);

        private static TreeViewNode BranchExpanded(string label, params TreeViewNode[] children)
            => new TreeViewNode(label, children, initiallyExpanded: true);

        private static IReadOnlyList<TreeViewNode> Flat()
            => new[] { Leaf("A"), Leaf("B"), Leaf("C") };

        private static IReadOnlyList<TreeViewNode> Nested()
            => new[] { Branch("Parent", Leaf("Child A"), Leaf("Child B")), Leaf("Sibling") };

        // --- TreeViewNode constructor ---

        [Test]
        public void Node_Constructor_StoresLabel()
        {
            var node = new TreeViewNode("My Node");
            Assert.That(node.Label, Is.EqualTo("My Node"));
        }

        [Test]
        public void Node_Constructor_NullLabel_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TreeViewNode(null!));
        }

        [Test]
        public void Node_Constructor_NullChildren_TreatedAsEmpty()
        {
            var node = new TreeViewNode("X", children: null);
            Assert.That(node.Children, Is.Empty);
        }

        [Test]
        public void Node_Constructor_StoresChildren()
        {
            var child = Leaf("child");
            var node = new TreeViewNode("parent", new[] { child });
            Assert.That(node.Children, Has.Count.EqualTo(1));
            Assert.That(node.Children[0], Is.SameAs(child));
        }

        [Test]
        public void Node_Constructor_StoresIcon()
        {
            var node = new TreeViewNode("X", icon: "★");
            Assert.That(node.Icon, Is.EqualTo("★"));
        }

        [Test]
        public void Node_Constructor_DefaultIcon_IsNull()
        {
            var node = new TreeViewNode("X");
            Assert.That(node.Icon, Is.Null);
        }

        [Test]
        public void Node_Constructor_StoresInitiallyExpanded()
        {
            var node = new TreeViewNode("X", new[] { Leaf() }, initiallyExpanded: true);
            Assert.That(node.InitiallyExpanded, Is.True);
        }

        [Test]
        public void Node_Constructor_DefaultInitiallyExpanded_IsFalse()
        {
            var node = new TreeViewNode("X");
            Assert.That(node.InitiallyExpanded, Is.False);
        }

        [Test]
        public void Node_HasChildren_FalseForLeaf()
        {
            Assert.That(Leaf().HasChildren, Is.False);
        }

        [Test]
        public void Node_HasChildren_TrueForBranch()
        {
            Assert.That(Branch("P", Leaf()).HasChildren, Is.True);
        }

        // --- TreeView constructor defaults ---

        [Test]
        public void Constructor_StoresNodes()
        {
            var nodes = Flat();
            var tree = new TreeView(nodes);
            Assert.That(tree.Nodes, Is.SameAs(nodes));
        }

        [Test]
        public void Constructor_NullNodes_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TreeView(null!));
        }

        [Test]
        public void Constructor_DefaultOnNodeTap_IsNull()
        {
            var tree = new TreeView(Flat());
            Assert.That(tree.OnNodeTap, Is.Null);
        }

        [Test]
        public void Constructor_StoresOnNodeTap()
        {
            Action<TreeViewNode> cb = _ => { };
            var tree = new TreeView(Flat(), onNodeTap: cb);
            Assert.That(tree.OnNodeTap, Is.SameAs(cb));
        }

        [Test]
        public void Constructor_DefaultIndent_IsTwenty()
        {
            var tree = new TreeView(Flat());
            Assert.That(tree.Indent, Is.EqualTo(20f));
        }

        [Test]
        public void Constructor_StoresIndent()
        {
            var tree = new TreeView(Flat(), indent: 32f);
            Assert.That(tree.Indent, Is.EqualTo(32f));
        }

        [Test]
        public void Constructor_NegativeIndent_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeView(Flat(), indent: -1f));
        }

        [Test]
        public void Constructor_ZeroIndent_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new TreeView(Flat(), indent: 0f));
        }

        // --- GetChildren ---

        [Test]
        public void GetChildren_IsEmpty()
        {
            Assert.That(new TreeView(Flat()).GetChildren(), Is.Empty);
        }

        // --- CreateState ---

        [Test]
        public void CreateState_ReturnsNonNull()
        {
            Assert.That(new TreeView(Flat()).CreateState(), Is.Not.Null);
        }

        [Test]
        public void CreateState_ReturnsDifferentInstances()
        {
            var tree = new TreeView(Flat());
            Assert.That(tree.CreateState(), Is.Not.SameAs(tree.CreateState()));
        }

        // --- Mount / Build ---

        [Test]
        public void Mount_EmptyNodes_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Array.Empty<TreeViewNode>()), null));
        }

        [Test]
        public void Build_EmptyNodes_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Array.Empty<TreeViewNode>()), null);
            var built = ((State<TreeView>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_FlatNodes_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Flat()), null));
        }

        [Test]
        public void Build_FlatNodes_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Flat()), null);
            var built = ((State<TreeView>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_NestedNodes_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Nested()), null));
        }

        [Test]
        public void Build_NestedNodes_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Nested()), null);
            var built = ((State<TreeView>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithOnNodeTap_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Flat(), onNodeTap: _ => { }), null));
        }

        [Test]
        public void Mount_WithCustomIndent_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Flat(), indent: 32f), null));
        }

        [Test]
        public void Mount_WithZeroIndent_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(Flat(), indent: 0f), null));
        }

        [Test]
        public void Mount_WithInitiallyExpandedBranch_DoesNotThrow()
        {
            var nodes = new[] { BranchExpanded("Root", Leaf("Child")) };
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(nodes), null));
        }

        [Test]
        public void Build_WithInitiallyExpandedBranch_ReturnsNonNull()
        {
            var nodes = new[] { BranchExpanded("Root", Leaf("Child")) };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(nodes), null);
            var built = ((State<TreeView>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithIcons_DoesNotThrow()
        {
            var nodes = new[]
            {
                new TreeViewNode("Folder", new[] { Leaf("File", icon: "f") }, icon: "D"),
                Leaf("Item", icon: "i")
            };
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(nodes), null));
        }

        [Test]
        public void Mount_DeeplyNested_DoesNotThrow()
        {
            var deep = BranchExpanded("L1",
                BranchExpanded("L2",
                    BranchExpanded("L3", Leaf("Leaf"))));
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new TreeView(new[] { deep }), null));
        }

        [Test]
        public void Build_DeeplyNested_ReturnsNonNull()
        {
            var deep = BranchExpanded("L1",
                BranchExpanded("L2",
                    BranchExpanded("L3", Leaf("Leaf"))));
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(new[] { deep }), null);
            var built = ((State<TreeView>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        // --- DidUpdateElement / Rebuild ---

        [Test]
        public void DidUpdateElement_ChangeNodes_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Flat()), null);
            expander.UpdateElement(node, new TreeView(Nested()));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ChangeIndent_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Flat(), indent: 16f), null);
            expander.UpdateElement(node, new TreeView(Flat(), indent: 32f));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_AddOnNodeTap_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Flat()), null);
            expander.UpdateElement(node, new TreeView(Flat(), onNodeTap: _ => { }));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        // --- Unmount ---

        [Test]
        public void Unmount_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(Flat()), null);
            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        [Test]
        public void Unmount_WithExpandedBranch_DoesNotThrow()
        {
            var nodes = new[] { BranchExpanded("Root", Leaf("Child")) };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new TreeView(nodes), null);
            Assert.DoesNotThrow(() => expander.Unmount(node));
        }
    }
}
