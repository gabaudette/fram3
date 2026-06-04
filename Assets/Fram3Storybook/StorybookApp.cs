#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Storybook.Stories.Theming;
using Fram3.UI.Storybook.Stories.Animation;
using Fram3.UI.Storybook.Stories.Content;
using Fram3.UI.Storybook.Stories.Input;
using Fram3.UI.Storybook.Stories.Layout;
using Fram3.UI.Storybook.Stories.Navigation;
using Fram3.UI.Storybook.Stories.States;
using Fram3.UI.Styling;
using UnityEngine;
using FontAsset = UnityEngine.TextCore.Text.FontAsset;

namespace Fram3.UI.Storybook
{
    public sealed class Story
    {
        public string Name { get; }

        public string Description { get; }

        public Func<Element> Build { get; }

        public Story(string name, string description, Func<Element> build)
        {
            Name = name;
            Description = description;
            Build = build;
        }
    }

    public sealed class Chapter
    {
        public string Title { get; }

        public IReadOnlyList<Story> Stories { get; }

        public Chapter(string title, IReadOnlyList<Story> stories)
        {
            Title = title;
            Stories = stories;
        }
    }

    public sealed class StorybookApp : StatefulElement
    {
        /// <summary>
        /// Font set on the StorybookRunner's DisplayFont inspector field.
        /// Exposed here so stories can reference it for spot-override demos.
        /// </summary>
        internal static FontAsset? DisplayFont { get; private set; }

        /// <summary>
        /// The name of the story currently displayed, in "Chapter / Story" format.
        /// Updated whenever the user navigates to a different story.
        /// </summary>
        internal static string? ActiveStory { get; private set; }

        private static readonly Theme StorybookBaseTheme = new Theme
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
            InputBorderColor = FrameColor.FromHex("#1F2436"),
            TrackColor = FrameColor.FromHex("#1A1F30"),
            ScrollbarWidth = 4f,
            ScrollbarBorder = false,
            FontSize = 14f,
            FontSizeSmall = 12f,
            FontSizeLarge = 20f,
            BorderRadius = 8f,
            SliderDraggerRadius = 50f,
            Spacing = 8f
        };

        public static Element Create()
        {
            var primaryRaw = Resources.Load<Font>("LiberationSans-Regular");
            var displayRaw = Resources.Load<Font>("LiberationSerif-Regular");
            var primaryAsset = primaryRaw != null ? UnityEngine.TextCore.Text.FontAsset.CreateFontAsset(primaryRaw) : null;
            DisplayFont = displayRaw != null ? UnityEngine.TextCore.Text.FontAsset.CreateFontAsset(displayRaw) : null;
            var theme = primaryAsset != null
                ? StorybookBaseTheme with { FontFamily = primaryAsset }
                : StorybookBaseTheme;
            return new ThemeProvider(theme, new StorybookApp());
        }

        /// <inheritdoc/>
        public override State CreateState() => new StorybookAppState();

        private sealed class StorybookAppState : State<StorybookApp>
        {
            private IReadOnlyList<Chapter>? _chapters;
            // One notifier per sidebar item: fires only when that item's selection state changes.
            private Dictionary<(int, int), ValueNotifier<bool>>? _itemSelected;
            // Drives the content area; fires when the active story changes.
            private ValueNotifier<(int chapter, int story)>? _activeSelection;
            private readonly Dictionary<(int, int), Action> _tapCallbacks = new();

            public override void InitState()
            {
                _chapters = BuildChapters();
                _itemSelected = new Dictionary<(int, int), ValueNotifier<bool>>();
                for (var ci = 0; ci < _chapters.Count; ci++)
                {
                    for (var si = 0; si < _chapters[ci].Stories.Count; si++)
                    {
                        _itemSelected[(ci, si)] = new ValueNotifier<bool>(ci == 0 && si == 0);
                    }
                }
                _activeSelection = new ValueNotifier<(int, int)>((0, 0));
                UpdateActiveStory(0, 0);
            }

            public override void Dispose()
            {
                foreach (var n in _itemSelected!.Values)
                {
                    n.Dispose();
                }
                _activeSelection?.Dispose();
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
                                BuildContentArea(theme)
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
                        Border: new Border(
                            Color: FrameColor.FromHex("#1E2235"),
                            Width: 1f
                        )
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
                                Child = new ScrollView
                                {
                                    Child = new Padding(
                                        EdgeInsets.Symmetric(
                                            vertical: theme.Spacing,
                                            horizontal: theme.Spacing
                                        )
                                    )
                                    {
                                        Child = BuildChapterList(theme)
                                    }
                                }
                            }
                        }
                    }
                };
            }

            private static Element BuildSidebarHeader(Theme theme)
            {
                return new Container(
                    padding: EdgeInsets.Symmetric(
                        vertical: theme.Spacing * 2.5f,
                        horizontal: theme.Spacing * 2f
                    ),
                    decoration: new BoxDecoration(
                        Border: new Border(
                            Color: FrameColor.FromHex("#1E2235"),
                            Width: 1f
                        )
                    )
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Icon(
                                svgPath: "Assets/Fram3Storybook/Icons/fram3-logo.svg",
                                width: 48f,
                                height: 48f
                            ),
                            SizedBox.FromSize(height: 6f),
                            new Text("A declarative UI Framework",
                                style: new TextStyle(
                                    FontSize: 10f,
                                    Color: theme.SecondaryColor,
                                    LetterSpacing: 2f
                                )
                            )
                        }
                    }
                };
            }

            // Called once from Build. Each item embeds its own ValueListenableBuilder<bool>
            // so that only the two items whose isSelected state changes will ever rebuild.
            private Element BuildChapterList(Theme theme)
            {
                var items = new List<Element>();

                for (var chapterIndex = 0; chapterIndex < _chapters!.Count; chapterIndex++)
                {
                    var chapter = _chapters[chapterIndex];
                    var ci = chapterIndex;

                    if (chapterIndex > 0)
                    {
                        items.Add(
                            SizedBox.FromSize(height: theme.Spacing * 1.5f)
                        );
                    }

                    items.Add(
                        new Padding(new EdgeInsets(4f, 4f, 6f, 4f))
                        {
                            Child = new Text(
                                chapter.Title.ToUpperInvariant(),
                                style: new TextStyle(
                                    FontSize: 10f,
                                    Color: theme.SecondaryTextColor,
                                    Bold: true,
                                    LetterSpacing: 1.5f
                                )
                            )
                        }
                    );

                    for (var storyIndex = 0; storyIndex < chapter.Stories.Count; storyIndex++)
                    {
                        var story = chapter.Stories[storyIndex];
                        var si = storyIndex;

                        items.Add(new ValueListenableBuilder<bool>(
                            _itemSelected![(ci, si)],
                            (ctx, isSelected) => BuildSidebarItem(
                                name: story.Name,
                                isSelected: isSelected,
                                theme: theme,
                                onTap: GetTapCallback(ci, si)
                            )
                        ));
                    }
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = items.ToArray()
                };
            }

            private Action GetTapCallback(int chapterIndex, int storyIndex)
            {
                var key = (chapterIndex, storyIndex);
                if (!_tapCallbacks.TryGetValue(key, out var callback))
                {
                    var ci = chapterIndex;
                    var si = storyIndex;
                    callback = () =>
                    {
                        var old = _activeSelection!.Value;
                        if (old == (ci, si))
                        {
                            return;
                        }
                        _itemSelected![old].Value = false;
                        _itemSelected[(ci, si)].Value = true;
                        _activeSelection.Value = (ci, si);
                        UpdateActiveStory(ci, si);
                    };
                    _tapCallbacks[key] = callback;
                }
                return callback;
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
                        Child = new Text(
                            name,
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: textColor,
                                Bold: isSelected
                            )
                        )
                    }
                );
            }

            private Element BuildContentArea(Theme theme)
            {
                return new Expanded
                {
                    Child = new ScrollView
                    {
                        Child = new Padding(EdgeInsets.All(theme.Spacing * 3f))
                        {
                            Child = new ValueListenableBuilder<(int chapter, int story)>(
                                _activeSelection!,
                                (ctx, sel) => BuildSelectedStoryCard(sel.chapter, sel.story, theme)
                            )
                        }
                    }
                };
            }

            private Element BuildSelectedStoryCard(int chapterIndex, int storyIndex, Theme theme)
            {
                if (_chapters == null || _chapters.Count == 0)
                {
                    return new Text(
                        "No stories registered.",
                        style: new TextStyle(Color: theme.PrimaryTextColor)
                    );
                }

                var chapter = _chapters[chapterIndex];
                if (chapter.Stories.Count == 0)
                {
                    return new Text(
                        "No stories in this chapter.",
                        style: new TextStyle(Color: theme.PrimaryTextColor)
                    );
                }

                var story = chapter.Stories[storyIndex];

                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(12f),
                        Border: new Border(
                            Color: FrameColor.FromHex("#1E2235"),
                            Width: 1f
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
                                                Border: new Border(
                                                    Color: FrameColor.FromHex("#1E2235"),
                                                    Width: 1f
                                                )
                                            ),
                                            padding: EdgeInsets.Symmetric(vertical: 0f, horizontal: 0f)
                                        ),
                                        SizedBox.FromSize(height: theme.Spacing * 2f),
                                        story.Build()
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
                    padding: EdgeInsets.Symmetric(
                        vertical: theme.Spacing,
                        horizontal: theme.Spacing * 3f
                    )
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
                            new Text(
                                chapter.Title.ToUpperInvariant(),
                                style: new TextStyle(
                                    FontSize: 14f,
                                    Color: theme.PrimaryColor,
                                    LetterSpacing: 1.5f,
                                    Bold: true
                                )
                            ),
                            SizedBox.FromSize(width: 6f),
                            new Text(
                                "/",
                                style: new TextStyle(
                                    FontSize: 14f,
                                    Color: theme.SecondaryTextColor
                                )
                            ),
                            SizedBox.FromSize(width: 6f),
                            new Text(
                                story.Name,
                                style: new TextStyle(
                                    FontSize: 14f,
                                    Color: theme.SecondaryTextColor,
                                    LetterSpacing: 0.5f
                                )
                            )
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
                            new Text(
                                story.Name,
                                style: new TextStyle(
                                    FontSize: 26f,
                                    Bold: true,
                                    Color: theme.PrimaryTextColor
                                )
                            ),
                            SizedBox.FromSize(height: 6f),
                            new Text(
                                story.Description,
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: FrameColor.FromHex("#94A3B8")
                                )
                            )
                        }
                    }
                };
            }

            private void UpdateActiveStory(int chapterIndex, int storyIndex)
            {
                if (_chapters == null || _chapters.Count == 0) return;
                var chapter = _chapters[chapterIndex];
                if (chapter.Stories.Count == 0) return;
                ActiveStory = $"{chapter.Title} / {chapter.Stories[storyIndex].Name}";
            }

            private static IReadOnlyList<Chapter> BuildChapters()
            {
                return new Chapter[]
                {
                    new Chapter(title: "Animation", stories: AnimationStories.All()),
                    new Chapter(title: "Content", stories: ContentStories.All()),
                    new Chapter(title: "Input", stories: InputStories.All()),
                    new Chapter(title: "Layout", stories: LayoutStories.All()),
                    new Chapter(title: "Navigation", stories: NavigationStories.All()),
                    new Chapter(title: "State", stories: StateStories.All()),
                    new Chapter(title: "Theming", stories: ThemingStories.All())
                };
            }
        }
    }
}
