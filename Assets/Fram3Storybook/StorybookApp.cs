#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Storybook.Stories;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook
{
    /// <summary>
    /// A story entry: a display name, a short description, and a factory that builds the story content element.
    /// </summary>
    public sealed class Story
    {
        /// <summary>The display name shown in the sidebar and as the content-area heading.</summary>
        public string Name { get; }

        /// <summary>
        /// A one-sentence description of the element shown above the live demo.
        /// </summary>
        public string Description { get; }

        /// <summary>Factory that returns the story's root element.</summary>
        public Func<FElement> Build { get; }

        /// <summary>Creates a story entry.</summary>
        public Story(string name, string description, Func<FElement> build)
        {
            Name = name;
            Description = description;
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
                    Child = new FContainer(
                        decoration: new FBoxDecoration(Color: FColor.FromHex("#EBEBEB"))
                    )
                    {
                        Child = new FRow(crossAxisAlignment: FCrossAxisAlignment.Stretch)
                        {
                            Children = new FElement[]
                            {
                                BuildSidebar(theme),
                                new FDivider(
                                    axis: FDividerAxis.Vertical,
                                    thickness: 1f,
                                    color: theme.SecondaryTextColor.WithAlpha(0.2f)
                                ),
                                BuildContentArea(theme),
                            }
                        }
                    }
                };
            }

            private FElement BuildSidebar(FTheme theme)
            {
                return new FContainer(
                    decoration: new FBoxDecoration(Color: FColor.White),
                    width: 240f
                )
                {
                    Child = new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
                    {
                        Children = new FElement[]
                        {
                            BuildSidebarHeader(theme),
                            new FDivider(
                                color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            ),
                            new FExpanded
                            {
                                Child = new FScrollView()
                                {
                                    Child = new FPadding(FEdgeInsets.Symmetric(vertical: theme.Spacing,
                                        horizontal: theme.Spacing))
                                    {
                                        Child = BuildChapterList(theme),
                                    }
                                }
                            },
                        }
                    }
                };
            }

            private static FElement BuildSidebarHeader(FTheme theme)
            {
                return new FContainer(
                    padding: FEdgeInsets.Symmetric(vertical: theme.Spacing * 2f, horizontal: theme.Spacing * 1.5f)
                )
                {
                    Child = new FText("Fram3", new FTextStyle(
                        FontSize: theme.FontSizeLarge,
                        Bold: true,
                        Color: theme.PrimaryColor
                    ))
                };
            }

            private FElement BuildChapterList(FTheme theme)
            {
                var items = new List<FElement>();

                for (var ci = 0; ci < _chapters!.Count; ci++)
                {
                    var chapter = _chapters[ci];
                    var capturedCi = ci;

                    if (ci > 0)
                    {
                        items.Add(FSizedBox.FromSize(height: theme.Spacing));
                    }

                    items.Add(
                        new FPadding(FEdgeInsets.Symmetric(vertical: 2f, horizontal: 4f))
                        {
                            Child = new FText(chapter.Title.ToUpperInvariant(), new FTextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor,
                                Bold: true,
                                LetterSpacing: 1f
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

                return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
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
                var bgColor = isSelected ? theme.PrimaryColor.WithAlpha(0.12f) : FColor.Transparent;
                var textColor = isSelected ? theme.PrimaryColor : theme.PrimaryTextColor;

                return new FGestureDetector(
                    onTap: onTap,
                    child: new FContainer(
                        decoration: new FBoxDecoration(
                            Color: bgColor,
                            BorderRadius: FBorderRadius.All(theme.BorderRadius)
                        ),
                        padding: FEdgeInsets.Symmetric(vertical: 7f, horizontal: theme.Spacing)
                    )
                    {
                        Child = new FText(name, new FTextStyle(
                            FontSize: theme.FontSize,
                            Color: textColor,
                            Bold: isSelected
                        ))
                    }
                );
            }

            private FElement BuildContentArea(FTheme theme)
            {
                return new FExpanded
                {
                    Child = new FScrollView()
                    {
                        Child = new FPadding(FEdgeInsets.All(theme.Spacing * 3f))
                        {
                            Child = BuildSelectedStoryCard(theme),
                        }
                    }
                };
            }

            private FElement BuildSelectedStoryCard(FTheme theme)
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

                return new FContainer(
                    decoration: new FBoxDecoration(
                        Color: FColor.White,
                        BorderRadius: FBorderRadius.All(8f),
                        Shadow: new FShadow(
                            FColor.Black.WithAlpha(0.06f),
                            OffsetX: 0f,
                            OffsetY: 2f,
                            BlurRadius: 8f
                        )
                    ),
                    padding: FEdgeInsets.All(theme.Spacing * 3f)
                )
                {
                    Child = new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
                    {
                        Children = new FElement[]
                        {
                            BuildStoryHeader(story, theme),
                            new FDivider(
                                color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            ),
                            FSizedBox.FromSize(height: theme.Spacing * 2f),
                            story.Build(),
                        }
                    }
                };
            }

            private static FElement BuildStoryHeader(Story story, FTheme theme)
            {
                return new FPadding(FEdgeInsets.OnlyBottom(theme.Spacing * 2f))
                {
                    Child = new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
                    {
                        Children = new FElement[]
                        {
                            new FText(story.Name, new FTextStyle(
                                FontSize: 26f,
                                Bold: true,
                                Color: theme.PrimaryTextColor
                            )),
                            FSizedBox.FromSize(height: 6f),
                            new FText(story.Description, new FTextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )),
                        }
                    }
                };
            }

            private static IReadOnlyList<Chapter> BuildChapters()
            {
                return new Chapter[]
                {
                    new Chapter("Layout", LayoutStories.All()),
                    new Chapter("Content", ContentStories.All()),
                    new Chapter("Input", InputStories.All()),
                    new Chapter("State", StateStories.All()),
                    new Chapter("Animation", AnimationStories.All()),
                    new Chapter("Navigation", NavigationStories.All()),
                };
            }
        }
    }
}