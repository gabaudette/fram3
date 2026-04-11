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
    internal sealed class FNavigatorTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        private static IReadOnlyDictionary<string, Func<FBuildContext, FElement>> SingleRoute(
            string name,
            Func<FBuildContext, FElement> builder)
        {
            return new Dictionary<string, Func<FBuildContext, FElement>>
            {
                { name, builder }
            };
        }

        private static IReadOnlyDictionary<string, Func<FBuildContext, FElement>> TwoRoutes(
            string firstName,
            Func<FBuildContext, FElement> firstBuilder,
            string secondName,
            Func<FBuildContext, FElement> secondBuilder)
        {
            return new Dictionary<string, Func<FBuildContext, FElement>>
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
                new FNavigator(null!, "home"));
        }

        [Test]
        public void Constructor_EmptyRoutes_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new FNavigator(
                    new Dictionary<string, Func<FBuildContext, FElement>>(),
                    "home"));
        }

        [Test]
        public void Constructor_NullInitialRoute_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FNavigator(
                    SingleRoute("home", _ => new TestLeafElement("h")),
                    null!));
        }

        [Test]
        public void Constructor_UnknownInitialRoute_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new FNavigator(
                    SingleRoute("home", _ => new TestLeafElement("h")),
                    "unknown"));
        }

        // ---- Mount / initial render ----

        [Test]
        public void Mount_RendersInitialRoute()
        {
            var navigator = new FNavigator(
                SingleRoute("home", _ => new TestLeafElement("home-leaf")),
                "home");

            var navigatorNode = _expander.Mount(navigator, null);

            var scopeNode = navigatorNode.Children[0];
            Assert.That(scopeNode.Element, Is.InstanceOf<FNavigatorScope>());
        }

        [Test]
        public void Mount_ExposesNavigatorHandleViaScope()
        {
            FNavigatorHandle? captured = null;

            var navigator = new FNavigator(
                SingleRoute("home", ctx =>
                {
                    captured = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home-leaf");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured, Is.InstanceOf<FNavigatorState>());
        }

        // ---- Push ----

        [Test]
        public void Push_AddsRouteToStack_AndCanPopBecomesTrue()
        {
            FNavigatorHandle? handle = null;

            var navigator = new FNavigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorHandle? handle = null;
            var builtRoutes = new List<string>();

            var navigator = new FNavigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorHandle? handle = null;

            var navigator = new FNavigator(
                SingleRoute("home", ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }),
                "home");

            _expander.Mount(navigator, null);

            Assert.Throws<ArgumentException>(() => handle!.Push("nonexistent"));
        }

        [Test]
        public void Push_WithArguments_DoesNotThrow()
        {
            FNavigatorHandle? handle = null;

            var navigator = new FNavigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorHandle? handle = null;

            var navigator = new FNavigator(
                SingleRoute("home", ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorHandle? handle = null;
            var builtRoutes = new List<string>();

            var navigator = new FNavigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorHandle? handle = null;

            var navigator = new FNavigator(
                TwoRoutes(
                    "home", ctx =>
                    {
                        handle = ctx.GetInherited<FNavigatorScope>().Navigator;
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
            FNavigatorScope? captured = null;

            var navigator = new FNavigator(
                SingleRoute("home", ctx =>
                {
                    captured = ctx.GetInherited<FNavigatorScope>();
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
            FNavigatorScope? capturedScope = null;
            int buildCount = 0;

            var routeBuilder = new Func<FBuildContext, FElement>(ctx =>
            {
                capturedScope = ctx.GetInherited<FNavigatorScope>();
                buildCount++;
                return new TestLeafElement("home");
            });

            var navigator1 = new FNavigator(SingleRoute("home", routeBuilder), "home");
            var navigatorNode = _expander.Mount(navigator1, null);

            buildCount = 0;

            var navigator2 = new FNavigator(SingleRoute("home", routeBuilder), "home");
            _expander.UpdateElement(navigatorNode, navigator2);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.Zero);
        }
    }
}
