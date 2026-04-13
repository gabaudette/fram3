#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Storybook.Stories;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook
{
    /// <summary>
    /// A story entry: a display name and a factory that builds the story content element.
    /// </summary>
    public sealed class Story
    {
        /// <summary>The display name shown in the sidebar.</summary>
        public string Name { get; }

        /// <summary>Factory that returns the story's root element.</summary>
        public Func<FElement> Build { get; }

        /// <summary>Creates a story entry.</summary>
        public Story(string name, Func<FElement> build)
        {
            Name = name;
            Build = build;
        }
    }

    /// <summary>
    /// A chapter groups related stories under a heading.
    /// </summary>
    public sealed class Chapter
    {
        /// <summary>The chapter heading shown in the sidebar.</summary>
        public string Title { get; }

        /// <summary>The stories belonging to this chapter.</summary>
        public IReadOnlyList<Story> Stories { get; }

        /// <summary>Creates a chapter.</summary>
        public Chapter(string title, IReadOnlyList<Story> stories)
        {
            Title = title;
            Stories = stories;
        }
    }

    /// <summary>
    /// The root stateful element of the storybook. Renders a sidebar on the left
    /// listing every chapter and story, and a content area on the right that
    /// renders the currently selected story live.
    /// </summary>
    public sealed class StorybookApp : FStatefulElement
    {
        /// <summary>Returns the root element of the storybook wrapped in a theme provider.</summary>
        public static FElement Create()
        {
            return new FThemeProvider(
                FTheme.Default,
                new StorybookApp()
            );
        }

        /// <inheritdoc/>
        public override FState CreateState() => new StorybookAppState();

        private sealed class StorybookAppState : FState<StorybookApp>
        {
            private IReadOnlyList<Chapter>? _chapters;
            private int _selectedChapter;
            private int _selectedStory;

            public override void InitState()
            {
                _chapters = BuildChapters();
                _selectedChapter = 0;
                _selectedStory = 0;
            }

            public override FElement Build(FBuildContext context)
            {
                var theme = FThemeConsumer.Of(context);

                return new FExpanded
                {
                    Child = new FRow
                    {
                        Children = new FElement[]
                        {
                            BuildSidebar(theme),
                            new FDivider(
                                axis: FDividerAxis.Vertical,
                                thickness: 1f,
                                color: theme.SecondaryTextColor.WithAlpha(0.3f)
                            ),
                            BuildContentArea(theme),
                        }
                    }
                };
            }

            private FElement BuildSidebar(FTheme theme)
            {
                return new FContainer(
                    decoration: new FBoxDecoration(Color: theme.SurfaceColor),
                    width: 240f
                )
                {
                    Child = new FScrollView(FScrollDirection.Vertical)
                    {
                        Child = new FPadding(FEdgeInsets.All(theme.Spacing))
                        {
                            Child = BuildChapterList(theme),
                        }
                    }
                };
            }

            private FElement BuildChapterList(FTheme theme)
            {
                var items = new List<FElement>();

                for (var ci = 0; ci < _chapters!.Count; ci++)
                {
                    var chapter = _chapters[ci];
                    var capturedCi = ci;

                    items.Add(
                        new FPadding(FEdgeInsets.Symmetric(vertical: theme.Spacing * 0.5f, horizontal: 0f))
                        {
                            Child = new FText(chapter.Title, new FTextStyle(
                                FontSize: theme.FontSizeLarge,
                                Color: theme.PrimaryTextColor,
                                Bold: true
                            ))
                        }
                    );

                    for (var si = 0; si < chapter.Stories.Count; si++)
                    {
                        var story = chapter.Stories[si];
                        var capturedSi = si;
                        var isSelected = _selectedChapter == capturedCi && _selectedStory == capturedSi;

                        items.Add(BuildSidebarItem(story.Name, isSelected, theme, () =>
                        {
                            SetState(() =>
                            {
                                _selectedChapter = capturedCi;
                                _selectedStory = capturedSi;
                            });
                        }));
                    }
                }

                return new FColumn
                {
                    Children = items.ToArray()
                };
            }

            private static FElement BuildSidebarItem(
                string name,
                bool isSelected,
                FTheme theme,
                Action onTap
            )
            {
                var bgColor = isSelected ? theme.PrimaryColor.WithAlpha(0.15f) : FColor.Transparent;

                return new FPadding(FEdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                {
                    Child = new FContainer(
                        decoration: new FBoxDecoration(
                            Color: bgColor,
                            BorderRadius: FBorderRadius.All(theme.BorderRadius)
                        ),
                        padding: FEdgeInsets.Symmetric(vertical: 6f, horizontal: theme.Spacing)
                    )
                    {
                        Child = new FText(name, new FTextStyle(
                            FontSize: theme.FontSize,
                            Color: isSelected ? theme.PrimaryColor : theme.PrimaryTextColor
                        ))
                    }
                };
            }

            private FElement BuildContentArea(FTheme theme)
            {
                return new FExpanded
                {
                    Child = new FScrollView(FScrollDirection.Vertical)
                    {
                        Child = new FPadding(FEdgeInsets.All(theme.Spacing * 2f))
                        {
                            Child = BuildSelectedStory(),
                        }
                    }
                };
            }

            private FElement BuildSelectedStory()
            {
                if (_chapters == null || _chapters.Count == 0)
                {
                    return new FText("No stories registered.");
                }

                var chapter = _chapters[_selectedChapter];
                if (chapter.Stories.Count == 0)
                {
                    return new FText("No stories in this chapter.");
                }

                var story = chapter.Stories[_selectedStory];
                return story.Build();
            }

            private static IReadOnlyList<Chapter> BuildChapters()
            {
                return new Chapter[]
                {
                    new Chapter("Layout",     LayoutStories.All()),
                    new Chapter("Content",    ContentStories.All()),
                    new Chapter("Input",      InputStories.All()),
                    new Chapter("State",      StateStories.All()),
                    new Chapter("Animation",  AnimationStories.All()),
                    new Chapter("Navigation", NavigationStories.All()),
                };
            }
        }
    }
}
