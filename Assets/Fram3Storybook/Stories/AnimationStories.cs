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
                new Story("AnimationBuilder",
                    "Drives a single animation controller and rebuilds its child on every frame tick, giving full programmatic control over the animated value.",
                    BuildAnimationBuilder),
                new Story("AnimatedContainer",
                    "A container whose size, decoration, and padding interpolate smoothly to new values whenever its properties change.",
                    BuildAnimatedContainer),
                new Story("ImplicitAnimation",
                    "Animates one or more named values toward their latest targets using a shared duration and curve, without manual controller management.",
                    BuildImplicitAnimation),
                new Story("Curves",
                    "A library of pre-built easing functions -- Linear, EaseIn, EaseOut, EaseInOut, ElasticOut, and BounceOut -- used to shape animation playback.",
                    BuildCurves),
            };
        }

        // ---------------------------------------------------------------------------
        // AnimationBuilder
        // ---------------------------------------------------------------------------

        private static Element BuildAnimationBuilder()
        {
            return new AnimationBuilderStory();
        }

        private sealed class AnimationBuilderStory : StatefulElement
        {
            public override State CreateState() => new AnimationBuilderStoryState();

            private sealed class AnimationBuilderStoryState : State<AnimationBuilderStory>
            {
                private bool _running;

                public override void InitState()
                {
                    _running = false;
                }

                public override Element Build(BuildContext context)
                {
                    return new Column
                    {
                        Children = new Element[]
                        {
                            new Text("AnimationBuilder: press Play to animate opacity 0->1."),
                            new Button(
                                label: _running ? "Playing..." : "Play",
                                onPressed: () => SetState(() => _running = true)
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new AnimationBuilder(
                                    duration: 1.5f,
                                    curve: Curves.EaseInOut,
                                    builder: (_, controller) =>
                                    {
                                        if (_running && (controller.Status == AnimationStatus.Idle || controller.Status == AnimationStatus.Completed))
                                        {
                                            controller.Forward();
                                        }

                                        if (_running && controller.Status == AnimationStatus.Completed)
                                        {
                                            SetState(() => _running = false);
                                        }

                                        var value = controller.Value;

                                        return new Container(
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#6200EE").WithAlpha(value),
                                                BorderRadius: BorderRadius.All(8f)
                                            ),
                                            width: 200f,
                                            height: 60f,
                                            padding: EdgeInsets.All(8f)
                                        )
                                        {
                                            Child = new Text(
                                                $"Opacity: {value:F2}",
                                                new TextStyle(Color: FrameColor.White)
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
        // AnimatedContainer
        // ---------------------------------------------------------------------------

        private static Element BuildAnimatedContainer()
        {
            return new AnimatedContainerStory();
        }

        private sealed class AnimatedContainerStory : StatefulElement
        {
            public override State CreateState() => new AnimatedContainerStoryState();

            private sealed class AnimatedContainerStoryState : State<AnimatedContainerStory>
            {
                private bool _expanded;

                public override void InitState()
                {
                    _expanded = false;
                }

                public override Element Build(BuildContext context)
                {
                    return new Column
                    {
                        Children = new Element[]
                        {
                            new Text("AnimatedContainer: tap to toggle size and color."),
                            new Button(
                                label: _expanded ? "Collapse" : "Expand",
                                onPressed: () => SetState(() => _expanded = !_expanded)
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new AnimatedContainer(
                                    duration: 0.4f,
                                    curve: Curves.EaseInOut,
                                    decoration: new BoxDecoration(
                                        Color: _expanded
                                            ? FrameColor.FromHex("#03DAC6").WithAlpha(0.7f)
                                            : FrameColor.FromHex("#6200EE").WithAlpha(0.4f),
                                        BorderRadius: BorderRadius.All(_expanded ? 16f : 4f)
                                    ),
                                    width: _expanded ? 280f : 100f,
                                    height: _expanded ? 100f : 40f,
                                    child: new Center
                                    {
                                        Child = new Text(
                                            _expanded ? "Expanded" : "Small",
                                            new TextStyle(Color: FrameColor.White, Bold: true)
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
        // ImplicitAnimation
        // ---------------------------------------------------------------------------

        private static Element BuildImplicitAnimation()
        {
            return new ImplicitAnimationStory();
        }

        private sealed class ImplicitAnimationStory : StatefulElement
        {
            public override State CreateState() => new ImplicitAnimationStoryState();

            private sealed class ImplicitAnimationStoryState : State<ImplicitAnimationStory>
            {
                private bool _toggled;

                public override void InitState()
                {
                    _toggled = false;
                }

                public override Element Build(BuildContext context)
                {
                    var targetWidth = _toggled ? 240f : 80f;

                    return new Column
                    {
                        Children = new Element[]
                        {
                            new Text("ImplicitAnimation: animates width between 80 and 240."),
                            new Button(
                                label: "Toggle width",
                                onPressed: () => SetState(() => _toggled = !_toggled)
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new ImplicitAnimation(
                                    values: new IAnimatedValue[]
                                    {
                                        new AnimatedValue<float>(
                                            key: "width",
                                            target: targetWidth,
                                            lerp: (a, b, t) => a + (b - a) * t
                                        )
                                    },
                                    duration: 0.5f,
                                    curve: Curves.EaseOut,
                                    builder: (_, snapshot) =>
                                    {
                                        var w = snapshot.Get<float>("width");
                                        return new Container(
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#018786").WithAlpha(0.6f),
                                                BorderRadius: BorderRadius.All(4f)
                                            ),
                                            width: w,
                                            height: 40f
                                        )
                                        {
                                            Child = new Center
                                            {
                                                Child = new Text(
                                                    $"{w:F0}px",
                                                    new TextStyle(Color: FrameColor.White)
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
        // Curves
        // ---------------------------------------------------------------------------

        private static Element BuildCurves()
        {
            return new CurvesStory();
        }

        private sealed class CurvesStory : StatefulElement
        {
            public override State CreateState() => new CurvesStoryState();

            private sealed class CurvesStoryState : State<CurvesStory>
            {
                private bool _running;

                public override void InitState()
                {
                    _running = false;
                }

                public override Element Build(BuildContext context)
                {
                    return new Column
                    {
                        Children = new Element[]
                        {
                            new Text("Curves: six pre-built easing curves. Press Play to animate all."),
                            new Button(
                                label: _running ? "Playing..." : "Play all",
                                onPressed: () => SetState(() => _running = true)
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new Column
                                {
                                    Children = new Element[]
                                    {
                                        CurveRow("Linear", Curves.Linear, _running,
                                            () => SetState(() => _running = false)),
                                        CurveRow("EaseIn", Curves.EaseIn, _running,
                                            () => SetState(() => _running = false)),
                                        CurveRow("EaseOut", Curves.EaseOut, _running,
                                            () => SetState(() => _running = false)),
                                        CurveRow("EaseInOut", Curves.EaseInOut, _running,
                                            () => SetState(() => _running = false)),
                                        CurveRow("ElasticOut", Curves.ElasticOut, _running,
                                            () => SetState(() => _running = false)),
                                        CurveRow("BounceOut", Curves.BounceOut, _running,
                                            () => SetState(() => _running = false)),
                                    }
                                }
                            },
                        }
                    };
                }

                private static Element CurveRow(string name, Curve curve, bool running, System.Action onCompleted)
                {
                    return new Padding(EdgeInsets.Symmetric(vertical: 3f, horizontal: 0f))
                    {
                        Child = new Row
                        {
                            Children = new Element[]
                            {
                                new Container(width: 90f)
                                {
                                    Child = new Text(name, new TextStyle(FontSize: 11f))
                                },
                                new AnimationBuilder(
                                    duration: 1.2f,
                                    curve: curve,
                                    builder: (_, controller) =>
                                    {
                                        if (running && (controller.Status == AnimationStatus.Idle || controller.Status == AnimationStatus.Completed))
                                        {
                                            controller.Forward();
                                        }

                                        if (running && controller.Status == AnimationStatus.Completed)
                                        {
                                            onCompleted();
                                        }

                                        var value = controller.Value;

                                        return new Container(
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#6200EE").WithAlpha(0.15f)
                                            ),
                                            width: 200f,
                                            height: 20f
                                        )
                                        {
                                            Child = new Container(
                                                decoration: new BoxDecoration(
                                                    Color: FrameColor.FromHex("#6200EE"),
                                                    BorderRadius: BorderRadius.All(2f)
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