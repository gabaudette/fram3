#nullable enable
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the Animation chapter.</summary>
    public static class AnimationStories
    {
        /// <summary>Returns all animation stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("FAnimationBuilder",   BuildAnimationBuilder),
                new Story("FAnimatedContainer",  BuildAnimatedContainer),
                new Story("FImplicitAnimation",  BuildImplicitAnimation),
                new Story("FCurves",             BuildCurves),
            };
        }

        // ---------------------------------------------------------------------------
        // FAnimationBuilder
        // ---------------------------------------------------------------------------

        private static FElement BuildAnimationBuilder()
        {
            return new AnimationBuilderStory();
        }

        private sealed class AnimationBuilderStory : FStatefulElement
        {
            public override FState CreateState() => new AnimationBuilderStoryState();

            private sealed class AnimationBuilderStoryState : FState<AnimationBuilderStory>
            {
                private bool _running;

                public override void InitState()
                {
                    _running = false;
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FColumn
                    {
                        Children = new FElement[]
                        {
                            new FText("FAnimationBuilder: press Play to animate opacity 0->1."),
                            new FButton(
                                label: _running ? "Playing..." : "Play",
                                onPressed: () => SetState(() => _running = true)
                            ),
                            new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new FAnimationBuilder(
                                    duration: 1.5f,
                                    curve: FCurves.EaseInOut,
                                    builder: (_, controller) =>
                                    {
                                        if (_running && controller.Status == FAnimationStatus.Idle)
                                        {
                                            controller.Forward();
                                        }

                                        var value = controller.Value;

                                        return new FContainer(
                                            decoration: new FBoxDecoration(
                                                Color: FColor.FromHex("#6200EE").WithAlpha(value),
                                                BorderRadius: FBorderRadius.All(8f)
                                            ),
                                            width: 200f,
                                            height: 60f,
                                            padding: FEdgeInsets.All(8f)
                                        )
                                        {
                                            Child = new FText(
                                                $"Opacity: {value:F2}",
                                                new FTextStyle(Color: FColor.White)
                                            )
                                        };
                                    }
                                )
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // FAnimatedContainer
        // ---------------------------------------------------------------------------

        private static FElement BuildAnimatedContainer()
        {
            return new AnimatedContainerStory();
        }

        private sealed class AnimatedContainerStory : FStatefulElement
        {
            public override FState CreateState() => new AnimatedContainerStoryState();

            private sealed class AnimatedContainerStoryState : FState<AnimatedContainerStory>
            {
                private bool _expanded;

                public override void InitState()
                {
                    _expanded = false;
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FColumn
                    {
                        Children = new FElement[]
                        {
                            new FText("FAnimatedContainer: tap to toggle size and color."),
                            new FButton(
                                label: _expanded ? "Collapse" : "Expand",
                                onPressed: () => SetState(() => _expanded = !_expanded)
                            ),
                            new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new FAnimatedContainer(
                                    duration: 0.4f,
                                    curve: FCurves.EaseInOut,
                                    decoration: new FBoxDecoration(
                                        Color: _expanded
                                            ? FColor.FromHex("#03DAC6").WithAlpha(0.7f)
                                            : FColor.FromHex("#6200EE").WithAlpha(0.4f),
                                        BorderRadius: FBorderRadius.All(_expanded ? 16f : 4f)
                                    ),
                                    width: _expanded ? 280f : 100f,
                                    height: _expanded ? 100f : 40f,
                                    child: new FCenter
                                    {
                                        Child = new FText(
                                            _expanded ? "Expanded" : "Small",
                                            new FTextStyle(Color: FColor.White, Bold: true)
                                        )
                                    }
                                )
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // FImplicitAnimation
        // ---------------------------------------------------------------------------

        private static FElement BuildImplicitAnimation()
        {
            return new ImplicitAnimationStory();
        }

        private sealed class ImplicitAnimationStory : FStatefulElement
        {
            public override FState CreateState() => new ImplicitAnimationStoryState();

            private sealed class ImplicitAnimationStoryState : FState<ImplicitAnimationStory>
            {
                private bool _toggled;

                public override void InitState()
                {
                    _toggled = false;
                }

                public override FElement Build(FBuildContext context)
                {
                    var targetWidth = _toggled ? 240f : 80f;

                    return new FColumn
                    {
                        Children = new FElement[]
                        {
                            new FText("FImplicitAnimation: animates width between 80 and 240."),
                            new FButton(
                                label: "Toggle width",
                                onPressed: () => SetState(() => _toggled = !_toggled)
                            ),
                            new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new FImplicitAnimation(
                                    values: new IFAnimatedValue[]
                                    {
                                        new FAnimatedValue<float>(
                                            key: "width",
                                            target: targetWidth,
                                            lerp: (a, b, t) => a + (b - a) * t
                                        )
                                    },
                                    duration: 0.5f,
                                    curve: FCurves.EaseOut,
                                    builder: (_, snapshot) =>
                                    {
                                        var w = snapshot.Get<float>("width");
                                        return new FContainer(
                                            decoration: new FBoxDecoration(
                                                Color: FColor.FromHex("#018786").WithAlpha(0.6f),
                                                BorderRadius: FBorderRadius.All(4f)
                                            ),
                                            width: w,
                                            height: 40f
                                        )
                                        {
                                            Child = new FCenter
                                            {
                                                Child = new FText(
                                                    $"{w:F0}px",
                                                    new FTextStyle(Color: FColor.White)
                                                )
                                            }
                                        };
                                    }
                                )
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // FCurves
        // ---------------------------------------------------------------------------

        private static FElement BuildCurves()
        {
            return new CurvesStory();
        }

        private sealed class CurvesStory : FStatefulElement
        {
            public override FState CreateState() => new CurvesStoryState();

            private sealed class CurvesStoryState : FState<CurvesStory>
            {
                private bool _running;

                public override void InitState()
                {
                    _running = false;
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FColumn
                    {
                        Children = new FElement[]
                        {
                            new FText("FCurves: six pre-built easing curves. Press Play to animate all."),
                            new FButton(
                                label: _running ? "Playing..." : "Play all",
                                onPressed: () => SetState(() => _running = true)
                            ),
                            new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new FColumn
                                {
                                    Children = new FElement[]
                                    {
                                        CurveRow("Linear",    FCurves.Linear,    _running),
                                        CurveRow("EaseIn",    FCurves.EaseIn,    _running),
                                        CurveRow("EaseOut",   FCurves.EaseOut,   _running),
                                        CurveRow("EaseInOut", FCurves.EaseInOut, _running),
                                        CurveRow("ElasticOut",FCurves.ElasticOut,_running),
                                        CurveRow("BounceOut", FCurves.BounceOut, _running),
                                    }
                                }
                            },
                        }
                    };
                }

                private static FElement CurveRow(string name, FCurve curve, bool running)
                {
                    return new FPadding(FEdgeInsets.Symmetric(vertical: 3f, horizontal: 0f))
                    {
                        Child = new FRow
                        {
                            Children = new FElement[]
                            {
                                new FContainer(width: 90f)
                                {
                                    Child = new FText(name, new FTextStyle(FontSize: 11f))
                                },
                                new FAnimationBuilder(
                                    duration: 1.2f,
                                    curve: curve,
                                    builder: (_, controller) =>
                                    {
                                        if (running && controller.Status == FAnimationStatus.Idle)
                                        {
                                            controller.Forward();
                                        }

                                        var value = controller.Value;

                                        return new FContainer(
                                            decoration: new FBoxDecoration(
                                                Color: FColor.FromHex("#6200EE").WithAlpha(0.15f)
                                            ),
                                            width: 200f,
                                            height: 20f
                                        )
                                        {
                                            Child = new FContainer(
                                                decoration: new FBoxDecoration(
                                                    Color: FColor.FromHex("#6200EE"),
                                                    BorderRadius: FBorderRadius.All(2f)
                                                ),
                                                width: 200f * System.Math.Max(0f, value),
                                                height: 20f
                                            )
                                        };
                                    }
                                ),
                            }
                        }
                    };
                }
            }
        }
    }
}
