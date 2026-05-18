using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.State;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.States
{
    public class ProviderConsumerStory : StatelessElement
    {
        public override Element Build(BuildContext context)
        {
            return new Provider<string>(
                "Hello from Provider!",
                new Column
                {
                    Children = new Element[]
                    {
                        new Text("Consumer reads the nearest Provider<string>:"),
                        new Consumer<string>((_, value) => new Container(
                            decoration: new BoxDecoration(
                                Color: FrameColor.FromHex("#7B61FF").WithAlpha(0.2f),
                                BorderRadius: BorderRadius.All(4f)
                            ),
                            padding: EdgeInsets.All(12f)
                        )
                        {
                            Child = new Text(value, new TextStyle(
                                Color: FrameColor.FromHex("#E2E8F0"),
                                Bold: true
                            ))
                        }),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Text("Nested Provider<int> overrides for its subtree:")
                        },
                        new Provider<int>(
                            42,
                            new Consumer<int>((_, n) => new Text($"Consumed int: {n}"))
                        )
                    }
                }
            );
        }
    }
}