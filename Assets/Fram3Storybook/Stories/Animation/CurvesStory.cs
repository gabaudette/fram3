#nullable enable
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Animation
{
    public sealed class CurvesStory : StatefulElement
    {
        public override State CreateState() => new CurvesStoryState();

        private sealed class CurvesStoryState : State<CurvesStory>
        {
            private bool _running;
            private readonly AnimationController?[] _controllers = new AnimationController?[6];

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
                            onPressed: () => SetState(() =>
                            {
                                foreach (var controller in _controllers)
                                {
                                    controller?.Reset();
                                }

                                _running = true;
                            })
                        ),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Column
                            {
                                Children = new Element[]
                                {
                                    CurveRow(
                                        "Linear",
                                        Curves.Linear,
                                        0,
                                        _running,
                                        () => SetState(() => _running = false)
                                    ),
                                    CurveRow(
                                        "EaseIn",
                                        Curves.EaseIn,
                                        1,
                                        _running,
                                        () => SetState(() => _running = false)
                                    ),
                                    CurveRow(
                                        "EaseOut",
                                        Curves.EaseOut,
                                        2,
                                        _running,
                                        () => SetState(() => _running = false)
                                    ),
                                    CurveRow(
                                        "EaseInOut",
                                        Curves.EaseInOut,
                                        3,
                                        _running,
                                        () => SetState(() => _running = false)
                                    ),
                                    CurveRow(
                                        "ElasticOut",
                                        Curves.ElasticOut,
                                        4, _running,
                                        () => SetState(() => _running = false)
                                    ),
                                    CurveRow(
                                        "BounceOut",
                                        Curves.BounceOut,
                                        5,
                                        _running,
                                        () => SetState(() => _running = false)
                                    )
                                }
                            }
                        }
                    }
                };
            }

            private Element CurveRow(
                string name,
                Curve curve,
                int index,
                bool running,
                System.Action onCompleted
            )
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
                                    _controllers[index] = controller;

                                    if (running && controller.Status == AnimationStatus.Idle)
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
                            )
                        }
                    }
                };
            }
        }
    }
}