#nullable enable
using System.Collections.Generic;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public static class LayoutStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "Center",
                    description: "Centers its child both horizontally and vertically within the available space.",
                    build: () => new CenterStory()
                ),
                new Story(
                    name: "Column",
                    description: "Arranges children vertically in a single column," +
                                 " with configurable main-axis and cross-axis alignment.",
                    build: () => new ColumnStory()
                ),
                new Story(
                    name: "Container",
                    description: "A versatile single-child box that combines" +
                                 " a background decoration, explicit sizing, " +
                                 "and inner padding in one element.",
                    build: () => new ContainerStory()
                ),
                new Story(
                    name: "Divider",
                    description: "Renders a thin horizontal or vertical rule," +
                                 " useful as a visual separator between sections.",
                    build: () => new DividerStory()),
                new Story(
                    name: "Expanded",
                    description: "Fills all remaining space along the parent axis," +
                                 " optionally weighted by a flex factor.",
                    build: () => new ExpandedStory()
                ),
                new Story(
                    name: "Grid",
                    description: "Arranges a list of items into a fixed-column grid." +
                                 " Each row is a Row of Expanded cells;" +
                                 " rows are stacked in a Column.",
                    build: () => new GridStory()
                ),
                new Story(
                    name: "Margin",
                    description: "Adds outer spacing around a single child, pushing surrounding siblings away.",
                    build: () => new MarginStory()
                ),
                new Story(
                    name: "Padding",
                    description: "Inserts empty space between a single child" +
                                 " and its parent boundaries using per-side insets.",
                    build: () => new PaddingStory()
                ),
                new Story(
                    name: "Row",
                    description: "Arranges children horizontally in a single row," +
                                 " with configurable main-axis and cross-axis alignment.",
                    build: () => new RowStory()
                ),
                new Story(
                    name: "ScrollView",
                    description: "Makes its child scrollable along one axis" +
                                 " when the content exceeds the available viewport size.",
                    build: () => new ScrollViewStory()
                ),
                new Story(
                    name: "SizedBox",
                    description: "Occupies a fixed width and/or height;" +
                                 " can hold an optional child or act as a gap element between siblings.",
                    build: () => new SizedBoxStory()
                ),
                new Story(
                    name: "Stack",
                    description: "Layers children on top of each other" +
                                 " using absolute positioning, useful for overlapping elements.",
                    build: () => new StackStory()
                ),
                new Story(
                    name: "Wrap",
                    description: "Flows children left-to-right and" +
                                 " wraps onto new lines when the available width is exhausted.",
                    build: () => new WrapStory()
                )
            };
        }
    }
}