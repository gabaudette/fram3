using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class ImageStory : StatefulElement
    {
        public override State CreateState() => new ImageStoryState();

        private sealed class ImageStoryState : State<ImageStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var texture = Resources.Load<Texture2D>("Images/sample");
                var sprite = Resources.Load<Sprite>("Images/sample-sprite");

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Texture2D",
                            content: BuildTexture(texture, theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Sprite",
                            content: BuildSprite(sprite, theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "SVG Icon",
                            content: BuildIcon(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildTexture(Texture2D texture, Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new FrameImage(
                            source: texture,
                            width: 128f,
                            height: 128f
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Text(
                            "Loaded as Texture2D via Resources.Load",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    }
                };
            }

            private static Element BuildSprite(Sprite sprite, Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new FrameImage(
                            source: sprite,
                            width: 128f,
                            height: 128f
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Text(
                            "Loaded as Sprite via Resources.Load",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    }
                };
            }

            private static Element BuildIcon(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Icon(
                            svgPath: "Assets/Fram3Storybook/Icons/sample.svg",
                            width: 64f,
                            height: 64f
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Text(
                            "SVG icon loaded via svgPath",
                            style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)
                        )
                    }
                };
            }
        }
    }
}