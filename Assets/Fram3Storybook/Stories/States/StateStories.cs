#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.GlobalState;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.States
{
    public static class StateStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "CubitBuilder",
                    description: "Connects a Cubit to the element tree and rebuilds automatically" +
                                 " each time the cubit emits a new state.",
                    build: () => new CubitBuilderStory()
                ),
                new Story(
                    name: "Provider / Consumer",
                    description: "Injects an arbitrary value into a subtree via Provider and reads" +
                                 " the nearest matching value back out via Consumer.",
                    build: () => new ProviderConsumerStory()
                ),
                new Story(
                    name: "Selector",
                    description: "Like CubitBuilder but rebuilds only when a derived slice of state changes," +
                                 " minimising unnecessary rebuilds.",
                    build: () => new SelectorStory()
                ),
                new Story(
                    name: "Store",
                    description: "A Redux-style global store: dispatches typed" +
                                 " actions through a pure reducer function" +
                                 " and exposes the resulting state.",
                    build: BuildStore
                ),
                new Story(
                    name: "ValueListenableBuilder",
                    description: "Rebuilds its subtree whenever a ValueNotifier's value changes," +
                                 " enabling lightweight reactive state without a full cubit.",
                    build: () => new ValueListenableStory()
                )
            };
        }

        private static Element BuildStore()
        {
            var store = new Store<TodoState>(
                new TodoState(
                    items: new string[] { "Buy groceries", "Write tests" },
                    doneCount: 0
                ),
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
                        )
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
            return action is MarkOneDoneAction
                ? new TodoState(state.Items, state.DoneCount + 1)
                : state;
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