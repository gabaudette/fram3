using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine;
using Avatar = Fram3.UI.Elements.Content.Avatar;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class AvatarStory : StatefulElement
    {
        public override State CreateState() => new AvatarStoryState();

        private sealed class AvatarStoryState : State<AvatarStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Initials",
                            content: BuildInitials(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Icon",
                            content: BuildIcon(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Sizes",
                            content: BuildSizes(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Ring border",
                            content: BuildRing(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Party",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildInitials(Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Avatar(initials: "AB"),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            initials: "JD",
                            backgroundColor: new FrameColor(0.13f, 0.59f, 0.95f)
                        ),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            initials: "Z",
                            backgroundColor: new FrameColor(0.61f, 0.15f, 0.69f)
                        )
                    }
                };
            }

            private static Element BuildIcon(Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Avatar(iconSvgPath: "ui/icons/person.svg"),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            iconSvgPath: "ui/icons/shield.svg",
                            backgroundColor: new FrameColor(0.18f, 0.8f, 0.44f)
                        )
                    }
                };
            }

            private static Element BuildSizes(Theme theme)
            {
                return new Row(
                    crossAxisAlignment: CrossAxisAlignment.Center,
                    mainAxisAlignment: MainAxisAlignment.Start
                )
                {
                    Children = new Element[]
                    {
                        new Avatar(
                            initials: "S", size:
                            AvatarSize.Small
                        ),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            initials: "M",
                            size: AvatarSize.Medium
                        ),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            initials: "L", size:
                            AvatarSize.Large
                        )
                    }
                };
            }

            private static Element BuildRing(Theme theme) =>
                new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Avatar(
                            initials: "AB",
                            ring: new Border(theme.PrimaryColor, 2f)
                        ),
                        SizedBox.FromSize(width: theme.Spacing * 2f),
                        new Avatar(
                            initials: "CD",
                            backgroundColor: theme.SurfaceColor,
                            foregroundColor: theme.PrimaryTextColor,
                            ring: new Border(theme.InputBorderColor, 1f)
                        )
                    }
                };

            private static Element BuildGame(Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Avatar(
                                    initials: "KN",
                                    size: AvatarSize.Large,
                                    backgroundColor: new FrameColor(0.83f, 0.18f, 0.18f),
                                    ring: new Border(new FrameColor(1f, 0.84f, 0f), 3f)
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text(
                                    "Knight",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor,
                                        TextAlign: TextAnchor.MiddleCenter
                                    )
                                )
                            }
                        },
                        SizedBox.FromSize(width: theme.Spacing * 3f),
                        new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Avatar(
                                    initials: "MG",
                                    size: AvatarSize.Large,
                                    backgroundColor: new FrameColor(0.13f, 0.59f, 0.95f)
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text(
                                    "Mage",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor,
                                        TextAlign: TextAnchor.MiddleCenter
                                    )
                                )
                            }
                        },
                        SizedBox.FromSize(width: theme.Spacing * 3f),
                        new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Avatar(
                                    initials: "RG",
                                    size: AvatarSize.Large,
                                    backgroundColor: new FrameColor(0.18f, 0.8f, 0.44f)
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text(
                                    "Ranger",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor,
                                        TextAlign: TextAnchor.MiddleCenter
                                    )
                                )
                            }
                        }
                    }
                };
            }
        }
    }
}