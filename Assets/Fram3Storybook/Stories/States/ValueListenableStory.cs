#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.States
{
    public sealed class ValueListenableStory : StatefulElement
    {
        public override State CreateState() => new ValueListenableStoryState();

        private sealed class ValueListenableStoryState : State<ValueListenableStory>
        {
            private ValueNotifier<int>? _counter;

            public override void InitState()
            {
                _counter = new ValueNotifier<int>(0);
            }

            public override Element Build(BuildContext context)
            {
                return new Column
                {
                    Children = new Element[]
                    {
                        new Text("ValueListenableBuilder rebuilds on notifier change:"),
                        new ValueListenableBuilder<int>(
                            notifier: _counter!,
                            builder: (_, count) => new Container(
                                decoration: new BoxDecoration(
                                    Color: FrameColor.Green.WithAlpha(0.15f),
                                    BorderRadius: BorderRadius.All(4f)
                                ),
                                padding: EdgeInsets.All(12f)
                            )
                            {
                                Child = new Text(
                                    $"Count: {count}",
                                    style: new TextStyle(
                                        FontSize: 20f,
                                        Bold: true
                                    )
                                )
                            }
                        ),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Row
                            {
                                Children = new Element[]
                                {
                                    new Button(
                                        label: "Increment",
                                        onPressed: () => { _counter!.Value += 1; }
                                    ),
                                    SizedBox.FromSize(width: 8f),
                                    new Button(
                                        label: "Decrement",
                                        onPressed: () => { _counter!.Value -= 1; }
                                    ),
                                    SizedBox.FromSize(width: 8f),
                                    new Button(
                                        label: "Reset",
                                        onPressed: () => { _counter!.Value = 0; }
                                    )
                                }
                            }
                        }
                    }
                };
            }

            public override void Dispose()
            {
                _counter?.Dispose();
            }
        }
    }
}