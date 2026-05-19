#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class AvatarTests
    {
        [Test]
        public void Constructor_DefaultSource_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.Source, Is.Null);
        }

        [Test]
        public void Constructor_DefaultInitials_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.Initials, Is.Null);
        }

        [Test]
        public void Constructor_DefaultIconSource_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.IconSource, Is.Null);
        }

        [Test]
        public void Constructor_DefaultIconSvgPath_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.IconSvgPath, Is.Null);
        }

        [Test]
        public void Constructor_DefaultBackgroundColor_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.BackgroundColor, Is.Null);
        }

        [Test]
        public void Constructor_DefaultForegroundColor_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.ForegroundColor, Is.Null);
        }

        [Test]
        public void Constructor_DefaultRing_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.Ring, Is.Null);
        }

        [Test]
        public void Constructor_DefaultSize_IsMedium()
        {
            var avatar = new Avatar();

            Assert.That(avatar.Size, Is.EqualTo(AvatarSize.Medium));
        }

        [Test]
        public void Constructor_DefaultKey_IsNull()
        {
            var avatar = new Avatar();

            Assert.That(avatar.Key, Is.Null);
        }

        [Test]
        public void Constructor_StoresSource()
        {
            var source = new object();
            var avatar = new Avatar(source: source);

            Assert.That(avatar.Source, Is.SameAs(source));
        }

        [Test]
        public void Constructor_StoresInitials()
        {
            var avatar = new Avatar(initials: "AB");

            Assert.That(avatar.Initials, Is.EqualTo("AB"));
        }

        [Test]
        public void Constructor_StoresIconSource()
        {
            var icon = new object();
            var avatar = new Avatar(iconSource: icon);

            Assert.That(avatar.IconSource, Is.SameAs(icon));
        }

        [Test]
        public void Constructor_StoresIconSvgPath()
        {
            var avatar = new Avatar(iconSvgPath: "icons/user.svg");

            Assert.That(avatar.IconSvgPath, Is.EqualTo("icons/user.svg"));
        }

        [Test]
        public void Constructor_StoresBackgroundColor()
        {
            var color = new FrameColor(1f, 0f, 0f);
            var avatar = new Avatar(backgroundColor: color);

            Assert.That(avatar.BackgroundColor, Is.EqualTo(color));
        }

        [Test]
        public void Constructor_StoresForegroundColor()
        {
            var color = new FrameColor(0f, 1f, 0f);
            var avatar = new Avatar(foregroundColor: color);

            Assert.That(avatar.ForegroundColor, Is.EqualTo(color));
        }

        [Test]
        public void Constructor_StoresRing()
        {
            var ring = new Border(new FrameColor(0f, 0f, 1f), 2f);
            var avatar = new Avatar(ring: ring);

            Assert.That(avatar.Ring, Is.EqualTo(ring));
        }

        [Test]
        public void Constructor_StoresSize_Small()
        {
            var avatar = new Avatar(size: AvatarSize.Small);

            Assert.That(avatar.Size, Is.EqualTo(AvatarSize.Small));
        }

        [Test]
        public void Constructor_StoresSize_Large()
        {
            var avatar = new Avatar(size: AvatarSize.Large);

            Assert.That(avatar.Size, Is.EqualTo(AvatarSize.Large));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("av");
            var avatar = new Avatar(key: key);

            Assert.That(avatar.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var avatar = new Avatar(initials: "AB");

            Assert.That(avatar.GetChildren(), Is.Empty);
        }

        [Test]
        public void CreateState_ReturnsNonNullState()
        {
            var avatar = new Avatar(initials: "AB");

            Assert.That(avatar.CreateState(), Is.Not.Null);
        }

        [Test]
        public void CreateState_ReturnsDifferentInstances()
        {
            var avatar = new Avatar(initials: "AB");

            Assert.That(avatar.CreateState(), Is.Not.SameAs(avatar.CreateState()));
        }

        [Test]
        public void Mounts_Blank_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(), null));
        }

        [Test]
        public void Mounts_WithInitials_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "JD"), null));
        }

        [Test]
        public void Mounts_WithSingleInitial_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "J"), null));
        }

        [Test]
        public void Mounts_WithIconSvgPath_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(iconSvgPath: "icons/user.svg"), null));
        }

        [Test]
        public void Mounts_WithImageSource_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(source: new object()), null));
        }

        [Test]
        public void Mounts_WithIconSource_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(iconSource: new object()), null));
        }

        [Test]
        public void Mounts_Small_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "AB", size: AvatarSize.Small), null));
        }

        [Test]
        public void Mounts_Large_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "AB", size: AvatarSize.Large), null));
        }

        [Test]
        public void Mounts_WithBackgroundColor_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var color = new FrameColor(0.2f, 0.4f, 0.8f);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "AB", backgroundColor: color), null));
        }

        [Test]
        public void Mounts_WithForegroundColor_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var color = new FrameColor(1f, 1f, 1f);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "AB", foregroundColor: color), null));
        }

        [Test]
        public void Mounts_WithRing_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var ring = new Border(new FrameColor(1f, 1f, 1f), 2f);

            Assert.DoesNotThrow(() => expander.Mount(new Avatar(initials: "AB", ring: ring), null));
        }

        [Test]
        public void Build_Blank_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithInitials_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "JD"), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithIconSvgPath_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(iconSvgPath: "icons/user.svg"), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_WithImageSource_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(source: new object()), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_Small_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB", size: AvatarSize.Small), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Build_Large_ReturnsNonNull()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB", size: AvatarSize.Large), null);

            var built = ((State<Avatar>)node.State!).Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void DidUpdateElement_ChangingInitials_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB"), null);

            expander.UpdateElement(node, new Avatar(initials: "CD"));

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_SwitchingToImageSource_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB"), null);

            expander.UpdateElement(node, new Avatar(source: new object()));

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_SwitchingSize_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB", size: AvatarSize.Small), null);

            expander.UpdateElement(node, new Avatar(initials: "AB", size: AvatarSize.Large));

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void Unmount_WithInitials_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(initials: "AB"), null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        [Test]
        public void Unmount_Blank_DoesNotThrow()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var node = expander.Mount(new Avatar(), null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }
    }
}