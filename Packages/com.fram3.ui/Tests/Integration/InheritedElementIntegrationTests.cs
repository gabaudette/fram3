#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.State;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Verifies FProvider, FThemeProvider, and FConsumer propagation through a live tree.
    /// Tests the inherited element notification mechanism end-to-end.
    /// </summary>
    [TestFixture]
    internal sealed class InheritedElementIntegrationTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void FProvider_DescendantCanRead_ValueViaGetInherited()
        {
            string? capturedValue = null;

            var consumer = new TestStatelessElement(ctx =>
            {
                var provider = ctx.GetInherited<FProvider<string>>();
                capturedValue = provider.Value;
                return new TestLeafElement("leaf");
            });

            var root = new FProvider<string>("hello", consumer);
            TreeBuilder.Mount(root, _expander);

            Assert.That(capturedValue, Is.EqualTo("hello"));
        }

        [Test]
        public void FProvider_ValueChange_TriggersDescendantRebuild()
        {
            var providerState = new ProviderHostState<string>("v1");
            var host = new TestStatefulElement(() => providerState);

            var rebuildCount = 0;
            var consumer = new TestStatelessElement(ctx =>
            {
                ctx.GetInherited<FProvider<string>>();
                rebuildCount++;
                return new TestLeafElement("leaf");
            });

            providerState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(rebuildCount, Is.EqualTo(1));

            providerState.Transition("v2");
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(rebuildCount, Is.EqualTo(2));
        }

        [Test]
        public void FProvider_SameValue_DescendantStillExpandedByNormalRebuildPath()
        {
            // When the host rebuilds with the same provider value, UpdateShouldNotify
            // returns false so no additional dirty notifications are sent to registered
            // dependents. However the descendant consumer is still re-expanded once via
            // the normal tree-expansion path triggered by the host rebuild.
            var providerState = new ProviderHostState<string>("v1");
            var host = new TestStatefulElement(() => providerState);

            var rebuildCount = 0;
            var consumer = new TestStatelessElement(ctx =>
            {
                ctx.GetInherited<FProvider<string>>();
                rebuildCount++;
                return new TestLeafElement("leaf");
            });

            providerState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(rebuildCount, Is.EqualTo(1));

            // Same value -- UpdateShouldNotify returns false, no extra dirty notifications.
            // The consumer is still re-expanded once as part of the host rebuild cascade.
            providerState.Transition("v1");
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(rebuildCount, Is.EqualTo(2));
        }

        [Test]
        public void FThemeProvider_DescendantReceivesTheme()
        {
            FTheme? capturedTheme = null;

            var consumer = new TestStatelessElement(ctx =>
            {
                var provider = ctx.GetInherited<FThemeProvider>();
                capturedTheme = provider.Theme;
                return new TestLeafElement("leaf");
            });

            var theme = FTheme.Default;
            var root = new FThemeProvider(theme, consumer);
            TreeBuilder.Mount(root, _expander);

            Assert.That(capturedTheme, Is.SameAs(theme));
        }

        [Test]
        public void FThemeProvider_ThemeChange_TriggersDescendantRebuild()
        {
            var themeState = new ThemeHostState(FTheme.Default);
            var host = new TestStatefulElement(() => themeState);

            var rebuildCount = 0;
            var consumer = new TestStatelessElement(ctx =>
            {
                ctx.GetInherited<FThemeProvider>();
                rebuildCount++;
                return new TestLeafElement("leaf");
            });

            themeState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(rebuildCount, Is.EqualTo(1));

            var newTheme = FTheme.Default with { FontSize = 99f };
            themeState.Transition(newTheme);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(rebuildCount, Is.EqualTo(2));
        }

        [Test]
        public void FConsumer_ReadsValue_FromNearestAncestorProvider()
        {
            string? capturedValue = null;

            var consumer = new FConsumer<string>((ctx, val) =>
            {
                capturedValue = val;
                return new TestLeafElement("leaf");
            });

            var root = new FProvider<string>("consumed", consumer);
            TreeBuilder.Mount(root, _expander);

            Assert.That(capturedValue, Is.EqualTo("consumed"));
        }

        [Test]
        public void FConsumer_RebuildsWhenProviderValueChanges()
        {
            var providerState = new ProviderHostState<int>(0);
            var host = new TestStatefulElement(() => providerState);

            var lastSeenValue = -1;
            var consumer = new FConsumer<int>((ctx, val) =>
            {
                lastSeenValue = val;
                return new TestLeafElement("leaf");
            });

            providerState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(lastSeenValue, Is.EqualTo(0));

            providerState.Transition(42);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(lastSeenValue, Is.EqualTo(42));
        }

        [Test]
        public void FindInherited_ReturnsNull_WhenNoAncestorExists()
        {
            FProvider<string>? found = null;
            var consumer = new TestStatelessElement(ctx =>
            {
                found = ctx.FindInherited<FProvider<string>>();
                return new TestLeafElement("leaf");
            });

            TreeBuilder.Mount(consumer, _expander);

            Assert.That(found, Is.Null);
        }

        // ---- Helpers --------------------------------------------------------------------------

        /// <summary>
        /// A state that hosts a FProvider wrapping a consumer element.
        /// Call Transition to rebuild with a new provider value.
        /// </summary>
        private sealed class ProviderHostState<T> : FState
        {
            private T _value;
            private FElement? _consumer;

            public ProviderHostState(T initialValue)
            {
                _value = initialValue;
            }

            public void SetConsumer(FElement consumer)
            {
                _consumer = consumer;
            }

            public void Transition(T newValue)
            {
                SetState(() => _value = newValue);
            }

            public override FElement Build(FBuildContext context)
            {
                return new FProvider<T>(_value, _consumer!);
            }
        }

        private sealed class ThemeHostState : FState
        {
            private FTheme _theme;
            private FElement? _consumer;

            public ThemeHostState(FTheme theme)
            {
                _theme = theme;
            }

            public void SetConsumer(FElement consumer)
            {
                _consumer = consumer;
            }

            public void Transition(FTheme newTheme)
            {
                SetState(() => _theme = newTheme);
            }

            public override FElement Build(FBuildContext context)
            {
                return new FThemeProvider(_theme, _consumer!);
            }
        }
    }
}
