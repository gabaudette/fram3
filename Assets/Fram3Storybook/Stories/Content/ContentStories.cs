#nullable enable
using System.Collections.Generic;

namespace Fram3.UI.Storybook.Stories.Content
{
    public static class ContentStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "Accordion",
                    description: "A vertically stacked set of collapsible panels." +
                                 " Each item has a header that toggles " +
                                 "its content. Supports single-expand and multi-expand modes.",
                    build: () => new AccordionStory()
                ),
                new Story(
                    name: "Avatar",
                    description: "A circular element displaying a user image, initials placeholder, or icon. " +
                                 "Available in small, medium, and large sizes with optional ring border.",
                    build: () => new AvatarStory()
                ),
                new Story(
                    name: "Alert",
                    description: "An inline notification surface that displays a " +
                                 "severity-tinted title, an optional message, " +
                                 "and optional action buttons. Use Info, Success, Warning, or Error severity.",
                    build: () => new AlertStory()
                ),
                new Story(
                    name: "Badge",
                    description: "Overlays a colored pip on a child element to show a count or status dot. " +
                                 "Common on inventory slots, ability icons, and minimap markers.",
                    build: () => new BadgeStory()
                ),
                new Story(
                    name: "Card",
                    description: "An elevated surface container with an optional header and footer. " +
                                 "Supports elevated (shadow) and outlined (border) variants.",
                    build: () => new CardStory()
                ),
                new Story(
                    name: "Chip",
                    description: "A compact, pill-shaped label used to represent an attribute, filter, or selection. " +
                                 "Supports an optional dismiss button for removable chips.",
                    build: () => new ChipStory()
                ),
                new Story(
                    name: "Dialog",
                    description: "A modal overlay dialog with a title, optional body content, and action buttons. " +
                                 "Centered on a dimmed backdrop. Toggled by mounting or unmounting the element.",
                    build: () => new DialogStory()
                ),
                new Story(
                    name: "Images & Icons",
                    description: "Displays a Texture2D and Sprite loaded via Resources.Load," +
                                 " and an SVG icon loaded via svgPath.",
                    build: () => new ImageStory()
                ),
                new Story(
                    name: "ListView",
                    description: "Virtualized list with search, class filter, " +
                                 "and pagination over a roster of game characters.",
                    build: () => new ListViewStory()
                ),
                new Story(
                    name: "ProgressBar",
                    description: "Shows a bounded progress value between a min and max, " +
                                 "with an optional title label above the track.",
                    build: () => new ProgressBarStory()
                ),
                new Story(
                    name: "Snackbar",
                    description: "Shows a transient message bar triggered" +
                                 " by a button, with and without an action label.",
                    build: () => new SnackbarStory()
                ),
                new Story(
                    name: "Spinner",
                    description: "Displays an indeterminate loading indicator " +
                                 "as a spinning ring, configurable in size, stroke width, color, and rotation speed.",
                    build: () => new SpinnerStory()
                ),
                new Story(
                    name: "Stepper",
                    description: "A numeric input with decrement and increment buttons." +
                                 " Supports min/max clamping, configurable step size, optional label," +
                                 " and a disabled state.",
                    build: () => new StepperStory()
                ),
                new Story(
                    name: "Table",
                    description: "A sortable, selectable data table for displaying structured row and column data." +
                                 " Supports column sorting, row selection, striped rows, and fixed-width columns.",
                    build: () => new TableStory()
                ),
                new Story
                (
                    name: "TabView",
                    description: "Renders a row of tab labels and swaps the" +
                                 " visible content panel when a tab is selected.",
                    () => new TabViewStory()
                ),
                new Story(
                    name: "Text",
                    description: "Renders a string with optional font size, color," +
                                 " weight, style, and letter-spacing overrides.",
                    build: () => new TextStory()
                ),
                new Story(
                    name: "Tooltip",
                    description: "Attaches a plain-text tooltip to its child that appears on hover.",
                    build: () => new TooltipStory()
                ),
                new Story(
                    name: "TreeView",
                    description: "A hierarchical expandable/collapsible tree." +
                                 " Nodes with children toggle their subtree on tap." +
                                 " Supports optional icons, pre-expanded nodes, configurable indent," +
                                 " and a tap callback on leaf nodes.",
                    build: () => new TreeViewStory()
                )
            };
        }
    }
}