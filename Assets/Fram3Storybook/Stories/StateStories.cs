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
                new Story("FProvider / FConsumer",
                    "Injects an arbitrary value into a subtree via FProvider and reads the nearest matching value back out via FConsumer.",
                    BuildProviderConsumer),
                new Story("FValueListenableBuilder",
                    "Rebuilds its subtree whenever a FValueNotifier's value changes, enabling lightweight reactive state without a full cubit.",
                    BuildValueListenable),
                new Story("FCubitBuilder",
                    "Connects a FCubit to the element tree and rebuilds automatically each time the cubit emits a new state.",
                    BuildCubitBuilder),
                new Story("FSelector",
                    "Like FCubitBuilder but rebuilds only when a derived slice of state changes, minimising unnecessary rebuilds.",
                    BuildSelector),
                new Story("FStore",
                    "A Redux-style global store: dispatches typed actions through a pure reducer function and exposes the resulting state.",
                    BuildStore),
            };
        }

        // ---------------------------------------------------------------------------
        // FProvider / FConsumer
        // ---------------------------------------------------------------------------

        private static FElement BuildProviderConsumer()
        {
            return new FProvider<string>(
                "Hello from FProvider!",
                new FColumn
                {
                    Children = new FElement[]
                    {
                        new FText("FConsumer reads the nearest FProvider<string>:"),
                        new FConsumer<string>((_, value) => new FContainer(
                            decoration: new FBoxDecoration(
                                Color: FColor.FromHex("#6200EE").WithAlpha(0.1f),
                                BorderRadius: FBorderRadius.All(4f)
                            ),
                            padding: FEdgeInsets.All(12f)
                        )
                        {
                            Child = new FText(value, new FTextStyle(
                                Color: FColor.FromHex("#6200EE"),
                                Bold: true
                            ))
                        }),
                        new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new FText("Nested FProvider<int> overrides for its subtree:")
                        },
                        new FProvider<int>(
                            42,
                            new FConsumer<int>((_, n) => new FText($"Consumed int: {n}"))
                        ),
                    }
                }
            );
        }

        // ---------------------------------------------------------------------------
        // FValueListenableBuilder
        // ---------------------------------------------------------------------------

        private static FElement BuildValueListenable()
        {
            return new ValueListenableStory();
        }

        private sealed class ValueListenableStory : FStatefulElement
        {
            public override FState CreateState() => new ValueListenableStoryState();

            private sealed class ValueListenableStoryState : FState<ValueListenableStory>
            {
                private FValueNotifier<int>? _counter;

                public override void InitState()
                {
                    _counter = new FValueNotifier<int>(0);
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FColumn
                    {
                        Children = new FElement[]
                        {
                            new FText("FValueListenableBuilder rebuilds on notifier change:"),
                            new FValueListenableBuilder<int>(
                                notifier: _counter!,
                                builder: (_, count) => new FContainer(
                                    decoration: new FBoxDecoration(
                                        Color: FColor.Green.WithAlpha(0.15f),
                                        BorderRadius: FBorderRadius.All(4f)
                                    ),
                                    padding: FEdgeInsets.All(12f)
                                )
                                {
                                    Child = new FText($"Count: {count}", new FTextStyle(
                                        FontSize: 20f,
                                        Bold: true
                                    ))
                                }
                            ),
                            new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                            {
                                Child = new FRow
                                {
                                    Children = new FElement[]
                                    {
                                        new FButton(label: "Increment", onPressed: () => { _counter!.Value += 1; }),
                                        FSizedBox.FromSize(width: 8f),
                                        new FButton(label: "Decrement", onPressed: () => { _counter!.Value -= 1; }),
                                        FSizedBox.FromSize(width: 8f),
                                        new FButton(label: "Reset", onPressed: () => { _counter!.Value = 0; }),
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
        // FCubitBuilder
        // ---------------------------------------------------------------------------

        private static FElement BuildCubitBuilder()
        {
            return new CubitBuilderStory();
        }

        private sealed class CubitBuilderStory : FStatefulElement
        {
            public override FState CreateState() => new CubitBuilderStoryState();

            private sealed class CubitBuilderStoryState : FState<CubitBuilderStory>
            {
                private CounterCubit? _cubit;

                public override void InitState()
                {
                    _cubit = new CounterCubit();
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FProvider<CounterCubit>(
                        _cubit!,
                        new FColumn
                        {
                            Children = new FElement[]
                            {
                                new FText("FCubitBuilder rebuilds on CounterCubit state change:"),
                                new FCubitBuilder<CounterCubit, int>(
                                    builder: (_, count) => new FContainer(
                                        decoration: new FBoxDecoration(
                                            Color: FColor.Blue.WithAlpha(0.15f),
                                            BorderRadius: FBorderRadius.All(4f)
                                        ),
                                        padding: FEdgeInsets.All(12f)
                                    )
                                    {
                                        Child = new FText($"Count: {count}", new FTextStyle(
                                            FontSize: 20f,
                                            Bold: true
                                        ))
                                    }
                                ),
                                new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                                {
                                    Child = new FRow
                                    {
                                        Children = new FElement[]
                                        {
                                            new FButton(label: "+1", onPressed: () => _cubit!.Increment()),
                                            FSizedBox.FromSize(width: 8f),
                                            new FButton(label: "-1", onPressed: () => _cubit!.Decrement()),
                                            FSizedBox.FromSize(width: 8f),
                                            new FButton(label: "Reset", onPressed: () => _cubit!.Reset()),
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
        // FSelector
        // ---------------------------------------------------------------------------

        private static FElement BuildSelector()
        {
            return new SelectorStory();
        }

        private sealed class SelectorStory : FStatefulElement
        {
            public override FState CreateState() => new SelectorStoryState();

            private sealed class SelectorStoryState : FState<SelectorStory>
            {
                private CounterCubit? _cubit;

                public override void InitState()
                {
                    _cubit = new CounterCubit();
                }

                public override FElement Build(FBuildContext context)
                {
                    return new FProvider<CounterCubit>(
                        _cubit!,
                        new FColumn
                        {
                            Children = new FElement[]
                            {
                                new FText("FSelector rebuilds only when the selected slice changes:"),
                                new FText("(This selector shows 'Even' or 'Odd' -- rebuilds only on parity change.)"),
                                new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                                {
                                    Child = new FSelector<CounterCubit, int, bool>(
                                        selector: count => count % 2 == 0,
                                        builder: (_, isEven) => new FContainer(
                                            decoration: new FBoxDecoration(
                                                Color: isEven
                                                    ? FColor.Green.WithAlpha(0.2f)
                                                    : FColor.Red.WithAlpha(0.2f),
                                                BorderRadius: FBorderRadius.All(4f)
                                            ),
                                            padding: FEdgeInsets.All(12f)
                                        )
                                        {
                                            Child = new FText(
                                                isEven ? "Even" : "Odd",
                                                new FTextStyle(FontSize: 18f, Bold: true)
                                            )
                                        }
                                    )
                                },
                                new FCubitBuilder<CounterCubit, int>(
                                    builder: (_, count) => new FText($"Raw count: {count}")
                                ),
                                new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                                {
                                    Child = new FRow
                                    {
                                        Children = new FElement[]
                                        {
                                            new FButton(label: "+1", onPressed: () => _cubit!.Increment()),
                                            FSizedBox.FromSize(width: 8f),
                                            new FButton(label: "Reset", onPressed: () => _cubit!.Reset()),
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
        // FStore
        // ---------------------------------------------------------------------------

        private static FElement BuildStore()
        {
            var store = new FStore<TodoState>(
                new TodoState(items: new string[] { "Buy groceries", "Write tests" }, doneCount: 0),
                TodoReducer
            );

            return new FProvider<FStore<TodoState>>(
                store,
                new FColumn
                {
                    Children = new FElement[]
                    {
                        new FText("FStore (Redux-style) -- TodoState:"),
                        new FCubitBuilder<FStore<TodoState>, TodoState>(
                            builder: (_, state) => BuildTodoView(state, store)
                        ),
                    }
                }
            );
        }

        private static FElement BuildTodoView(TodoState state, FStore<TodoState> store)
        {
            var itemElements = new FElement[state.Items.Count + 2];
            for (var i = 0; i < state.Items.Count; i++)
            {
                var label = state.Items[i];
                itemElements[i] = new FPadding(FEdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                {
                    Child = new FText($"- {label}")
                };
            }

            itemElements[state.Items.Count] = new FPadding(
                FEdgeInsets.Symmetric(vertical: 6f, horizontal: 0f))
            {
                Child = new FText($"Done count: {state.DoneCount}")
            };
            itemElements[state.Items.Count + 1] = new FButton(
                label: "Mark one done",
                onPressed: () => store.Dispatch(new MarkOneDoneAction())
            );

            return new FColumn { Children = itemElements };
        }

        private static TodoState TodoReducer(TodoState state, FAction action)
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

        private sealed class CounterCubit : FCubit<int>
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

        private sealed class MarkOneDoneAction : FAction
        {
        }
    }
}