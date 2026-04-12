#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Navigation;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Verifies FNavigator push/pop behavior inside a live element tree. Tests that the
    /// FNavigatorScope inherited element propagates correctly to descendants after navigation.
    /// </summary>
    [TestFixture]
    internal sealed class NavigationIntegrationTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void Navigator_Mount_InitialRouteRendered()
        {
            var builtRoutes = new List<string>();

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = _ =>
                {
                    builtRoutes.Add("home");
                    return new TestLeafElement("home");
                }
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            Assert.That(builtRoutes, Contains.Item("home"));
        }

        [Test]
        public void Navigator_CanPop_FalseOnInitialRoute()
        {
            FNavigatorHandle? handle = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            Assert.That(handle, Is.Not.Null);
            Assert.That(handle!.CanPop, Is.False);
        }

        [Test]
        public void Navigator_Push_TopRouteChanges()
        {
            var renderedPages = new List<string>();

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = _ =>
                {
                    renderedPages.Add("home");
                    return new TestLeafElement("home");
                },
                ["detail"] = _ =>
                {
                    renderedPages.Add("detail");
                    return new TestLeafElement("detail");
                }
            };

            FNavigatorHandle? handle = null;
            var navigatorWithHandle = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    renderedPages.Add("home");
                    return new TestLeafElement("home");
                },
                ["detail"] = _ =>
                {
                    renderedPages.Add("detail");
                    return new TestLeafElement("detail");
                }
            };

            var navigator = new FNavigator(navigatorWithHandle, "home");
            TreeBuilder.Mount(navigator, _expander);
            renderedPages.Clear();

            handle!.Push("detail");
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(renderedPages, Contains.Item("detail"));
        }

        [Test]
        public void Navigator_Push_CanPop_BecomesTrue()
        {
            FNavigatorHandle? handle = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                },
                ["detail"] = _ => new TestLeafElement("detail")
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            handle!.Push("detail");
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(handle.CanPop, Is.True);
        }

        [Test]
        public void Navigator_Pop_ReturnsToPreviousRoute()
        {
            var renderedPages = new List<string>();
            FNavigatorHandle? handle = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    renderedPages.Add("home");
                    return new TestLeafElement("home");
                },
                ["detail"] = _ =>
                {
                    renderedPages.Add("detail");
                    return new TestLeafElement("detail");
                }
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            handle!.Push("detail");
            TreeBuilder.Flush(_scheduler, _expander);
            renderedPages.Clear();

            handle.Pop();
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(renderedPages, Contains.Item("home"));
        }

        [Test]
        public void Navigator_PopOnInitialRoute_NoOp()
        {
            FNavigatorHandle? handle = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            handle!.Pop(); // should be a no-op; no exception expected
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(handle.CanPop, Is.False);
        }

        [Test]
        public void Navigator_UnknownRoute_ThrowsArgumentException()
        {
            FNavigatorHandle? handle = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = ctx =>
                {
                    handle = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("home");
                }
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            Assert.Throws<System.ArgumentException>(() => handle!.Push("does-not-exist"));
        }

        [Test]
        public void Navigator_ScopeDescendant_CanReadHandle_ViaGetInherited()
        {
            FNavigatorHandle? handleFromDescendant = null;

            var routes = new Dictionary<string, System.Func<FBuildContext, FElement>>
            {
                ["home"] = _ => new TestStatelessElement(ctx =>
                {
                    handleFromDescendant = ctx.GetInherited<FNavigatorScope>().Navigator;
                    return new TestLeafElement("nested");
                })
            };

            var navigator = new FNavigator(routes, "home");
            TreeBuilder.Mount(navigator, _expander);

            Assert.That(handleFromDescendant, Is.Not.Null);
        }
    }
}
