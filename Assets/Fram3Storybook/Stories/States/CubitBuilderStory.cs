#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.States
{
    public sealed class CubitBuilderStory : StatefulElement
    {
        public override State CreateState() => new CubitBuilderStoryState();

        private sealed class CubitBuilderStoryState : State<CubitBuilderStory>
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
                            new Text("CubitBuilder rebuilds on CounterCubit state change:"),
                            new CubitBuilder<CounterCubit, int>(
                                builder: (_, count) => new Container(
                                    decoration: new BoxDecoration(
                                        Color: FrameColor.Blue.WithAlpha(0.15f),
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
                                            label: "+1",
                                            onPressed: () => _cubit!.Increment()
                                        ),
                                        SizedBox.FromSize(width: 8f),
                                        new Button(
                                            label: "-1",
                                            onPressed: () => _cubit!.Decrement()
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