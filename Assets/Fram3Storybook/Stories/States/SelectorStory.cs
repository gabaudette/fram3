#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.States
{
    public sealed class SelectorStory : StatefulElement
    {
        public override State CreateState() => new SelectorStoryState();

        private sealed class SelectorStoryState : State<SelectorStory>
        {
            private CounterCubit? _cubit;

            public override void InitState()
            {
                _cubit = new CounterCubit();
            }

            public override Element Build(BuildContext context)
            {
                return new Provider<CounterCubit>(
                    _cubit!,
                    new Column
                    {
                        Children = new Element[]
                        {
                            new Text("Selector rebuilds only when the selected slice changes:"),
                            new Text("(This selector shows 'Even' or 'Odd' -- rebuilds only on parity change.)"),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new Selector<CounterCubit, int, bool>(
                                    selector: count => count % 2 == 0,
                                    builder: (_, isEven) => new Container(
                                        decoration: new BoxDecoration(
                                            Color: isEven
                                                ? FrameColor.Green.WithAlpha(0.2f)
                                                : FrameColor.Red.WithAlpha(0.2f),
                                            BorderRadius: BorderRadius.All(4f)
                                        ),
                                        padding: EdgeInsets.All(12f)
                                    )
                                    {
                                        Child = new Text(
                                            isEven ? "Even" : "Odd",
                                            style: new TextStyle(
                                                FontSize: 18f,
                                                Bold: true
                                            )
                                        )
                                    }
                                )
                            },
                            new CubitBuilder<CounterCubit, int>(
                                builder: (_, count) => new Text($"Raw count: {count}")
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new Row
                                {
                                    Children = new Element[]
                                    {
                                        new Button(
                                            label: "+1",
                                            onPressed: () => _cubit!.Increment()
                                        ),
                                        SizedBox.FromSize(width: 8f),
                                        new Button(
                                            label: "Reset",
                                            onPressed: () => _cubit!.Reset()
                                        )
                                    }
                                }
                            }
                        }
                    }
                );
            }

            public override void Dispose()
            {
                _cubit?.Dispose();
            }
        }
    }
}