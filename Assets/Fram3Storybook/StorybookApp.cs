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
        public Func<Element> Build { get; }

        /// <summary>Creates a story entry.</summary>
        public Story(string name, string description, Func<Element> build)
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
    public sealed class StorybookApp : StatefulElement
    {
        /// <summary>Returns the root element of the storybook wrapped in a theme provider.</summary>
        public static Element Create()
        {
            return new ThemeProvider(
                Theme.Default,
                new StorybookApp()
            );
        }

        /// <inheritdoc/>
        public override State CreateState() => new StorybookAppState();

        private sealed class StorybookAppState : State<StorybookApp>
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

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                return new Expanded
                {
                    Child = new Container(
                        decoration: new BoxDecoration(Color: FrameColor.FromHex("#EBEBEB"))
                    )
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                BuildSidebar(theme),
                                new Divider(
                                    axis: DividerAxis.Vertical,
                                    thickness: 1f,
                                    color: theme.SecondaryTextColor.WithAlpha(0.2f)
                                ),
                                BuildContentArea(theme),
                            }
                        }
                    }
                };
            }

            private Element BuildSidebar(Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(Color: FrameColor.White),
                    width: 240f
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildSidebarHeader(theme),
                            new Divider(
                                color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            ),
                            new Expanded
                            {
                                Child = new ScrollView()
                                {
                                    Child = new Padding(EdgeInsets.Symmetric(vertical: theme.Spacing,
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

            private static Element BuildSidebarHeader(Theme theme)
            {
                return new Container(
                    padding: EdgeInsets.Symmetric(vertical: theme.Spacing * 2f, horizontal: theme.Spacing * 1.5f)
                )
                {
                    Child = new Text("Fram3", new TextStyle(
                        FontSize: theme.FontSizeLarge,
                        Bold: true,
                        Color: theme.PrimaryColor
                    ))
                };
            }

            private Element BuildChapterList(Theme theme)
            {
                var items = new List<Element>();

                for (var ci = 0; ci < _chapters!.Count; ci++)
                {
                    var chapter = _chapters[ci];
                    var capturedCi = ci;

                    if (ci > 0)
                    {
                        items.Add(SizedBox.FromSize(height: theme.Spacing));
                    }

                    items.Add(
                        new Padding(EdgeInsets.Symmetric(vertical: 2f, horizontal: 4f))
                        {
                            Child = new Text(chapter.Title.ToUpperInvariant(), new TextStyle(
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

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = items.ToArray()
                };
            }

            private static Element BuildSidebarItem(
                string name,
                bool isSelected,
                Theme theme,
                Action onTap
            )
            {
                var bgColor = isSelected ? theme.PrimaryColor.WithAlpha(0.12f) : FrameColor.Transparent;
                var textColor = isSelected ? theme.PrimaryColor : theme.PrimaryTextColor;

                return new GestureDetector(
                    onTap: onTap,
                    child: new Container(
                        decoration: new BoxDecoration(
                            Color: bgColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        ),
                        padding: EdgeInsets.Symmetric(vertical: 7f, horizontal: theme.Spacing)
                    )
                    {
                        Child = new Text(name, new TextStyle(
                            FontSize: theme.FontSize,
                            Color: textColor,
                            Bold: isSelected
                        ))
                    }
                );
            }

            private Element BuildContentArea(Theme theme)
            {
                return new Expanded
                {
                    Child = new ScrollView()
                    {
                        Child = new Padding(EdgeInsets.All(theme.Spacing * 3f))
                        {
                            Child = BuildSelectedStoryCard(theme),
                        }
                    }
                };
            }

            private Element BuildSelectedStoryCard(Theme theme)
            {
                if (_chapters == null || _chapters.Count == 0)
                {
                    return new Text("No stories registered.");
                }

                var chapter = _chapters[_selectedChapter];
                if (chapter.Stories.Count == 0)
                {
                    return new Text("No stories in this chapter.");
                }

                var story = chapter.Stories[_selectedStory];

                return new Container(
                    decoration: new BoxDecoration(
                        Color: FrameColor.White,
                        BorderRadius: BorderRadius.All(8f),
                        Shadow: new Shadow(
                            FrameColor.Black.WithAlpha(0.06f),
                            OffsetX: 0f,
                            OffsetY: 2f,
                            BlurRadius: 8f
                        )
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 3f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildStoryHeader(story, theme),
                            new Divider(
                                color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            story.Build(),
                        }
                    }
                };
            }

            private static Element BuildStoryHeader(Story story, Theme theme)
            {
                return new Padding(EdgeInsets.OnlyBottom(theme.Spacing * 2f))
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text(story.Name, new TextStyle(
                                FontSize: 26f,
                                Bold: true,
                                Color: theme.PrimaryTextColor
                            )),
                            SizedBox.FromSize(height: 6f),
                            new Text(story.Description, new TextStyle(
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