using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Animation
{
    public sealed class AnimatedContainerStory : StatefulElement
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
                                        style: new TextStyle(Color: FrameColor.White, Bold: true)
                                    )
                                }
                            )
                        }
                    }
                };
            }
        }
    }
}