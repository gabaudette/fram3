#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Navigation;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Navigation
{
    [TestFixture]
    internal sealed class NavigatorTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        private static IReadOnlyDictionary<string, Func<BuildContext, Element>> SingleRoute(
            string name,
            Func<BuildContext, Element> builder)
        {
            return new Dictionary<string, Func<BuildContext, Element>>
            {
                { name, builder }
            };
        }

        private static IReadOnlyDictionary<string, Func<BuildContext, Element>> TwoRoutes(
            string firstName,
            Func<BuildContext, Element> firstBuilder,
            string secondName,
            Func<BuildContext, Element> secondBuilder)
        {
            return new Dictionary<string, Func<BuildContext, Element>>
            {
                { firstName, firstBuilder },
                { secondName, secondBuilder }
            };
        }

        // ---- Construction validation ----

        [Test]
        public void Constructor_NullRoutes_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Navigator(null!, "home"));
        }

        [Test]
        public void Constructor_EmptyRoutes_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Navigator(
                    new Dictionary<string, Func<BuildContext, Element>>(),
                    "home"));
        }

        [Test]
        public void Constructor_NullInitialRoute_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Navigator(
                    SingleRoute("home", _ => new TestLeafElement("h")),
                    null!));
        }

        [Test]
        public void Constructor_UnknownInitialRoute_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new Navigator(
                    SingleRoute("home", _ => new TestLeafElement("h")),
                    "unknown"));
        }

        // ---- Mount / initial render ----

        [Test]
        public void Mount_RendersInitialRoute()
        {
            var navigator = new Navigator(
                SingleRoute("home", _ => new TestLeafElement("home-leaf")),
                "home");

            var navigatorNode = _expander.Mount(navigator, null);

            var scopeNode = navigatorNode.Children[0];
            Assert.That(scopeNode.Element, Is.InstanceOf<NavigatorScope>());
        }

        [Test]
        public void Mount_ExposesNavigatorHandleViaScope()
        {
            NavigatorHandle? captured = null;

            var navigator = new Navigator(
                SingleRoute("home", ctx =>
                {
                    captured = ctx.GetInherited<NavigatorScope>().Navigator;
                    return new TestLeafElement("home-leaf");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured, Is.InstanceOf<NavigatorState>());
        }

        // ---- Push ----

        [Test]
        public void Push_AddsRouteToStack_AndCanPopBecomesTrue()
        {
            NavigatorHandle? handle = null;

            var navigator = new Navigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<NavigatorScope>().Navigator;
                        return new TestLeafElement("home");
                    },
                    "detail", _ => new TestLeafElement("detail")),
                "home");

            _expander.Mount(navigator, null);

            Assert.That(handle, Is.Not.Null);
            Assert.That(handle!.CanPop, Is.False);

            handle.Push("detail");

            Assert.That(handle.CanPop, Is.True);
        }

        [Test]
        public void Push_TriggersRebuild_AndRendersNewRoute()
        {
            NavigatorHandle? handle = null;
            var builtRoutes = new List<string>();

            var navigator = new Navigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<NavigatorScope>().Navigator;
                        builtRoutes.Add("home");
                        return new TestLeafElement("home");
                    },
                    "detail", _ =>
                    {
                        builtRoutes.Add("detail");
                        return new TestLeafElement("detail");
                    }),
                "home");

            var navigatorNode = _expander.Mount(navigator, null);

            handle!.Push("detail");
            _scheduler.Flush(_expander);

            Assert.That(builtRoutes, Contains.Item("detail"));
        }

        [Test]
        public void Push_UnknownRoute_ThrowsArgumentException()
        {
            NavigatorHandle? handle = null;

            var navigator = new Navigator(
                SingleRoute("home", ctx =>
                {
                    handle = ctx.GetInherited<NavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.Throws<ArgumentException>(() => handle!.Push("nonexistent"));
        }

        [Test]
        public void Push_WithArguments_DoesNotThrow()
        {
            NavigatorHandle? handle = null;

            var navigator = new Navigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<NavigatorScope>().Navigator;
                        return new TestLeafElement("home");
                    },
                    "detail", _ => new TestLeafElement("detail")),
                "home");

            _expander.Mount(navigator, null);

            Assert.DoesNotThrow(() => handle!.Push("detail", arguments: 42));
        }

        // ---- Pop ----

        [Test]
        public void Pop_OnSingleRoute_IsNoOp()
        {
            NavigatorHandle? handle = null;

            var navigator = new Navigator(
                SingleRoute("home", ctx =>
                {
                    handle = ctx.GetInherited<NavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.DoesNotThrow(() => handle!.Pop());
            Assert.That(handle!.CanPop, Is.False);
        }

        [Test]
        public void Pop_AfterPush_ReturnsToPreviousRoute()
        {
            NavigatorHandle? handle = null;
            var builtRoutes = new List<string>();

            var navigator = new Navigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<NavigatorScope>().Navigator;
                        builtRoutes.Add("home");
                        return new TestLeafElement("home");
                    },
                    "detail", _ =>
                    {
                        builtRoutes.Add("detail");
                        return new TestLeafElement("detail");
                    }),
                "home");

            var navigatorNode = _expander.Mount(navigator, null);
            handle!.Push("detail");
            _scheduler.Flush(_expander);

            builtRoutes.Clear();

            handle.Pop();
            _scheduler.Flush(_expander);

            Assert.That(builtRoutes, Contains.Item("home"));
            Assert.That(handle.CanPop, Is.False);
        }

        [Test]
        public void Pop_AfterTwoPushes_CanPopRemainsTrue()
        {
            NavigatorHandle? handle = null;

            var navigator = new Navigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<NavigatorScope>().Navigator;
                        return new TestLeafElement("home");
                    },
                    "detail", _ => new TestLeafElement("detail")),
                "home");

            var navigatorNode = _expander.Mount(navigator, null);
            handle!.Push("detail");
            _scheduler.Flush(_expander);
            handle.Push("detail");
            _scheduler.Flush(_expander);

            handle.Pop();
            _scheduler.Flush(_expander);

            Assert.That(handle.CanPop, Is.True);
        }

        // ---- Inherited dependency wiring ----

        [Test]
        public void GetInherited_Descendent_ReceivesNavigatorScope()
        {
            NavigatorScope? captured = null;

            var navigator = new Navigator(
                SingleRoute("home", ctx =>
                {
                    captured = ctx.GetInherited<NavigatorScope>();
                    return new TestLeafElement("home");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.Navigator, Is.Not.Null);
        }

        [Test]
        public void UpdateShouldNotify_SameStateReference_DoesNotNotifyDependents()
        {
            NavigatorScope? capturedScope = null;
            int buildCount = 0;

            var routeBuilder = new Func<BuildContext, Element>(ctx =>
            {
                capturedScope = ctx.GetInherited<NavigatorScope>();
                buildCount++;
                return new TestLeafElement("home");
            });

            var navigator1 = new Navigator(SingleRoute("home", routeBuilder), "home");
            var navigatorNode = _expander.Mount(navigator1, null);

            buildCount = 0;

            var navigator2 = new Navigator(SingleRoute("home", routeBuilder), "home");
            _expander.UpdateElement(navigatorNode, navigator2);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.Zero);
        }
    }
}
