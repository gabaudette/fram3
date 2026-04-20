#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.GlobalState;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the State chapter.</summary>
    public static class StateStories
    {
        /// <summary>Returns all state stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("Provider / Consumer",
                    "Injects an arbitrary value into a subtree via Provider and reads the nearest matching value back out via Consumer.",
                    BuildProviderConsumer),
                new Story("ValueListenableBuilder",
                    "Rebuilds its subtree whenever a ValueNotifier's value changes, enabling lightweight reactive state without a full cubit.",
                    BuildValueListenable),
                new Story("CubitBuilder",
                    "Connects a Cubit to the element tree and rebuilds automatically each time the cubit emits a new state.",
                    BuildCubitBuilder),
                new Story("Selector",
                    "Like CubitBuilder but rebuilds only when a derived slice of state changes, minimising unnecessary rebuilds.",
                    BuildSelector),
                new Story("Store",
                    "A Redux-style global store: dispatches typed actions through a pure reducer function and exposes the resulting state.",
                    BuildStore),
            };
        }

        // ---------------------------------------------------------------------------
        // Provider / Consumer
        // ---------------------------------------------------------------------------

        private static Element BuildProviderConsumer()
        {
            return new Provider<string>(
                "Hello from Provider!",
                new Column
                {
                    Children = new Element[]
                    {
                        new Text("Consumer reads the nearest Provider<string>:"),
                        new Consumer<string>((_, value) => new Container(
                            decoration: new BoxDecoration(
                                 Color: FrameColor.FromHex("#7B61FF").WithAlpha(0.2f),
                                BorderRadius: BorderRadius.All(4f)
                            ),
                            padding: EdgeInsets.All(12f)
                        )
                        {
                            Child = new Text(value, new TextStyle(
                                Color: FrameColor.FromHex("#E2E8F0"),
                                Bold: true
                            ))
                        }),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Text("Nested Provider<int> overrides for its subtree:")
                        },
                        new Provider<int>(
                            42,
                            new Consumer<int>((_, n) => new Text($"Consumed int: {n}"))
                        ),
                    }
                }
            );
        }

        // ---------------------------------------------------------------------------
        // ValueListenableBuilder
        // ---------------------------------------------------------------------------

        private static Element BuildValueListenable()
        {
            return new ValueListenableStory();
        }

        private sealed class ValueListenableStory : StatefulElement
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
                                    Child = new Text($"Count: {count}", new TextStyle(
                                        FontSize: 20f,
                                        Bold: true
                                    ))
                                }
                            ),
                            new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new Row
                                {
                                    Children = new Element[]
                                    {
                                        new Button(label: "Increment", onPressed: () => { _counter!.Value += 1; }),
                                        SizedBox.FromSize(width: 8f),
                                        new Button(label: "Decrement", onPressed: () => { _counter!.Value -= 1; }),
                                        SizedBox.FromSize(width: 8f),
                                        new Button(label: "Reset", onPressed: () => { _counter!.Value = 0; }),
                                    }
                                }
                            },
                        }
                    };
                }

                public override void Dispose()
                {
                    _counter?.Dispose();
                }
            }
        }

        // ---------------------------------------------------------------------------
        // CubitBuilder
        // ---------------------------------------------------------------------------

        private static Element BuildCubitBuilder()
        {
            return new CubitBuilderStory();
        }

        private sealed class CubitBuilderStory : StatefulElement
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
                                        Child = new Text($"Count: {count}", new TextStyle(
                                            FontSize: 20f,
                                            Bold: true
                                        ))
                                    }
                                ),
                                new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                                {
                                    Child = new Row
                                    {
                                        Children = new Element[]
                                        {
                                            new Button(label: "+1", onPressed: () => _cubit!.Increment()),
                                            SizedBox.FromSize(width: 8f),
                                            new Button(label: "-1", onPressed: () => _cubit!.Decrement()),
                                            SizedBox.FromSize(width: 8f),
                                            new Button(label: "Reset", onPressed: () => _cubit!.Reset()),
                                        }
                                    }
                                },
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

        // ---------------------------------------------------------------------------
        // Selector
        // ---------------------------------------------------------------------------

        private static Element BuildSelector()
        {
            return new SelectorStory();
        }

        private sealed class SelectorStory : StatefulElement
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
                                                new TextStyle(FontSize: 18f, Bold: true)
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
                                            new Button(label: "+1", onPressed: () => _cubit!.Increment()),
                                            SizedBox.FromSize(width: 8f),
                                            new Button(label: "Reset", onPressed: () => _cubit!.Reset()),
                                        }
                                    }
                                },
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

        // ---------------------------------------------------------------------------
        // Store
        // ---------------------------------------------------------------------------

        private static Element BuildStore()
        {
            var store = new Store<TodoState>(
                new TodoState(items: new string[] { "Buy groceries", "Write tests" }, doneCount: 0),
                TodoReducer
            );

            return new Provider<Store<TodoState>>(
                store,
                new Column
                {
                    Children = new Element[]
                    {
                        new Text("Store (Redux-style) -- TodoState:"),
                        new CubitBuilder<Store<TodoState>, TodoState>(
                            builder: (_, state) => BuildTodoView(state, store)
                        ),
                    }
                }
            );
        }

        private static Element BuildTodoView(TodoState state, Store<TodoState> store)
        {
            var itemElements = new Element[state.Items.Count + 2];
            for (var i = 0; i < state.Items.Count; i++)
            {
                var label = state.Items[i];
                itemElements[i] = new Padding(EdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                {
                    Child = new Text($"- {label}")
                };
            }

            itemElements[state.Items.Count] = new Padding(
                EdgeInsets.Symmetric(vertical: 6f, horizontal: 0f))
            {
                Child = new Text($"Done count: {state.DoneCount}")
            };
            itemElements[state.Items.Count + 1] = new Button(
                label: "Mark one done",
                onPressed: () => store.Dispatch(new MarkOneDoneAction())
            );

            return new Column { Children = itemElements };
        }

        private static TodoState TodoReducer(TodoState state, FrameAction action)
        {
            if (action is MarkOneDoneAction)
            {
                return new TodoState(state.Items, state.DoneCount + 1);
            }

            return state;
        }

        // ---------------------------------------------------------------------------
        // Supporting types
        // ---------------------------------------------------------------------------

        private sealed class CounterCubit : Cubit<int>
        {
            public CounterCubit() : base(0)
            {
            }

            public void Increment() => Emit(State + 1);
            public void Decrement() => Emit(State - 1);
            public void Reset() => Emit(0);
        }

        private sealed class TodoState
        {
            public IReadOnlyList<string> Items { get; }
            public int DoneCount { get; }

            public TodoState(IReadOnlyList<string> items, int doneCount)
            {
                Items = items;
                DoneCount = doneCount;
            }
        }

        private sealed class MarkOneDoneAction : FrameAction
        {
        }
    }
}