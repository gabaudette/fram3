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
    public sealed class AnimationBuilderStory : StatefulElement
    {
        public override State CreateState() => new AnimationBuilderStoryState();

        private sealed class AnimationBuilderStoryState : State<AnimationBuilderStory>
        {
            private bool _running;
            private AnimationController? _controller;

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
                            onPressed: () => SetState(() =>
                            {
                                _controller?.Reset();
                                _running = true;
                            })
                        ),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new AnimationBuilder(
                                duration: 1.5f,
                                curve: Curves.EaseInOut,
                                builder: (_, controller) =>
                                {
                                    _controller = controller;

                                    if (_running && controller.Status == AnimationStatus.Idle)
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
                                            style: new TextStyle(Color: FrameColor.White)
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