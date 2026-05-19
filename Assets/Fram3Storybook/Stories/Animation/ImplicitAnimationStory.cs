using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Animation
{
    public sealed class ImplicitAnimationStory : StatefulElement
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
                                                style: new TextStyle(
                                                    Color: FrameColor.White
                                                )
                                            )
                                        }
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