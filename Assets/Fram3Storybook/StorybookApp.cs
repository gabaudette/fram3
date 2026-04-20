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
        private static readonly Theme StorybookTheme = new Theme
        {
            PrimaryColor = FrameColor.FromHex("#7B61FF"),
            SecondaryColor = FrameColor.FromHex("#00D4AA"),
            BackgroundColor = FrameColor.FromHex("#0C0E1A"),
            SurfaceColor = FrameColor.FromHex("#141726"),
            OnPrimaryColor = FrameColor.FromHex("#FFFFFF"),
            ErrorColor = FrameColor.FromHex("#FF6B6B"),
            PrimaryTextColor = FrameColor.FromHex("#E2E8F0"),
            SecondaryTextColor = FrameColor.FromHex("#6B7691"),
            DisabledTextColor = FrameColor.FromHex("#2E3347"),
            FontSize = 14f,
            FontSizeSmall = 12f,
            FontSizeLarge = 20f,
            BorderRadius = 10f,
            Spacing = 8f
        };

        /// <summary>Returns the root element of the storybook wrapped in a theme provider.</summary>
        public static Element Create()
        {
            return new ThemeProvider(
                StorybookTheme,
                new StorybookApp()
            );
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new StorybookAppState();

        private sealed class StorybookAppState : Fram3.UI.Core.State<StorybookApp>
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
                        decoration: new BoxDecoration(Color: theme.BackgroundColor)
                    )
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                BuildSidebar(theme),
                                BuildContentArea(theme),
                            }
                        }
                    }
                };
            }

            private Element BuildSidebar(Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: FrameColor.FromHex("#0A0C16"),
                        Border: new Border(FrameColor.FromHex("#1E2235"), 1f)
                    ),
                    width: 260f
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildSidebarHeader(theme),
                            new Expanded
                            {
                                Child = new ScrollView()
                                {
                                    Child = new Padding(EdgeInsets.Symmetric(vertical: theme.Spacing, horizontal: theme.Spacing))
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
                    padding: EdgeInsets.Symmetric(vertical: theme.Spacing * 2.5f, horizontal: theme.Spacing * 2f),
                    decoration: new BoxDecoration(
                        Border: new Border(FrameColor.FromHex("#1E2235"), 1f)
                    )
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text("FRAM3", new TextStyle(
                                FontSize: theme.FontSizeLarge,
                                Bold: true,
                                Color: theme.PrimaryColor,
                                LetterSpacing: 3f
                            )),
                            SizedBox.FromSize(height: 2f),
                            new Text("UI FRAMEWORK", new TextStyle(
                                FontSize: 10f,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 2f
                            )),
                        }
                    }
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
                        items.Add(SizedBox.FromSize(height: theme.Spacing * 1.5f));
                    }

                    items.Add(
                        new Padding(new EdgeInsets(4f, 4f, 6f, 4f))
                        {
                            Child = new Text(chapter.Title.ToUpperInvariant(), new TextStyle(
                                FontSize: 10f,
                                Color: theme.SecondaryTextColor,
                                Bold: true,
                                LetterSpacing: 1.5f
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
                var bgColor = isSelected
                    ? theme.PrimaryColor.WithAlpha(0.15f)
                    : FrameColor.Transparent;
                var textColor = isSelected ? theme.PrimaryColor : theme.PrimaryTextColor;
                var accentBar = isSelected
                    ? new Border(theme.PrimaryColor, 3f)
                    : new Border(FrameColor.Transparent, 3f);

                return new GestureDetector(
                    onTap: onTap,
                    child: new Container(
                        decoration: new BoxDecoration(
                            Color: bgColor,
                            BorderRadius: BorderRadius.All(6f),
                            Border: accentBar
                        ),
                        padding: EdgeInsets.Symmetric(vertical: 8f, horizontal: theme.Spacing)
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
                    return new Text("No stories registered.", new TextStyle(Color: theme.PrimaryTextColor));
                }

                var chapter = _chapters[_selectedChapter];
                if (chapter.Stories.Count == 0)
                {
                    return new Text("No stories in this chapter.", new TextStyle(Color: theme.PrimaryTextColor));
                }

                var story = chapter.Stories[_selectedStory];

                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(12f),
                        Border: new Border(FrameColor.FromHex("#1E2235"), 1f),
                        Shadow: new Shadow(
                            FrameColor.Black.WithAlpha(0.4f),
                            OffsetX: 0f,
                            OffsetY: 4f,
                            BlurRadius: 24f
                        )
                    )
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildStoryCardAccent(story, chapter, theme),
                            new Padding(EdgeInsets.All(theme.Spacing * 3f))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        BuildStoryHeader(story, theme),
                                        new Container(
                                            decoration: new BoxDecoration(
                                                Border: new Border(FrameColor.FromHex("#1E2235"), 1f)
                                            ),
                                            padding: EdgeInsets.Symmetric(vertical: 0f, horizontal: 0f)
                                        ),
                                        SizedBox.FromSize(height: theme.Spacing * 2f),
                                        story.Build(),
                                    }
                                }
                            }
                        }
                    }
                };
            }

            private static Element BuildStoryCardAccent(Story story, Chapter chapter, Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.PrimaryColor.WithAlpha(0.12f),
                        BorderRadius: BorderRadius.Only(topLeft: 12f, topRight: 12f)
                    ),
                    padding: EdgeInsets.Symmetric(vertical: theme.Spacing, horizontal: theme.Spacing * 3f)
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                width: 3f,
                                height: 16f,
                                decoration: new BoxDecoration(
                                    Color: theme.PrimaryColor,
                                    BorderRadius: BorderRadius.All(2f)
                                )
                            ),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Text(chapter.Title.ToUpperInvariant(), new TextStyle(
                                FontSize: 14f,
                                Color: theme.PrimaryColor,
                                LetterSpacing: 1.5f,
                                Bold: true
                            )),
                            SizedBox.FromSize(width: 6f),
                            new Text("/", new TextStyle(
                                FontSize: 14f,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(width: 6f),
                            new Text(story.Name, new TextStyle(
                                FontSize: 14f,
                                Color: theme.SecondaryTextColor,
                                LetterSpacing: 0.5f
                            )),
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
                                Color: FrameColor.FromHex("#94A3B8")
                            )),
                        }
                    }
                };
            }

            private static IReadOnlyList<Chapter> BuildChapters()
            {
                return new Chapter[]
                {
                    new Chapter("Theming", ThemeStories.All()),
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
