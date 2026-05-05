#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Navigation;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the Navigation chapter.</summary>
    public static class NavigationStories
    {
        private const string RouteHome = "/";
        private const string RouteDetail = "/detail";
        private const string RouteSettings = "/settings";

        /// <summary>Returns all navigation stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("Navigator",
                    "A stack-based router that maps string route keys to builder functions; supports Push, Pop, and replace operations via NavigatorHandle.",
                    BuildNavigator),
            };
        }

        private static Element BuildNavigator()
        {
            return new Container(
                decoration: new BoxDecoration(Color: FrameColor.FromHex("#0C0E1A")),
                height: 320f
            )
            {
                Child = new Navigator(
                    routes: new Dictionary<string, System.Func<BuildContext, Element>>
                    {
                        [RouteHome] = BuildHomeRoute,
                        [RouteDetail] = BuildDetailRoute,
                        [RouteSettings] = BuildSettingsRoute,
                    },
                    initialRoute: RouteHome
                )
            };
        }

        private static Element BuildHomeRoute(BuildContext context)
        {
            var navigator = context.GetInherited<NavigatorScope>().Navigator;

            return new Padding(EdgeInsets.All(16f))
            {
                Child = new Column
                {
                    Children = new Element[]
                    {
                        new Text("Home Route", new TextStyle(FontSize: 20f, Bold: true, Color: FrameColor.FromHex("#E2E8F0"))),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Text("Push a route using the buttons below.", new TextStyle(Color: FrameColor.FromHex("#E2E8F0")))
                        },
                        new Row
                        {
                            Children = new Element[]
                            {
                                new Button(
                                    label: "Go to Detail",
                                    onPressed: () => navigator.Push(RouteDetail)
                                ),
                                SizedBox.FromSize(width: 8f),
                                new Button(
                                    label: "Go to Settings",
                                    onPressed: () => navigator.Push(RouteSettings)
                                ),
                            }
                        },
                    }
                }
            };
        }

        private static Element BuildDetailRoute(BuildContext context)
        {
            var navigator = context.GetInherited<NavigatorScope>().Navigator;

            return new Padding(EdgeInsets.All(16f))
            {
                Child = new Column
                {
                    Children = new Element[]
                    {
                        new Text("Detail Route", new TextStyle(FontSize: 20f, Bold: true, Color: FrameColor.FromHex("#E2E8F0"))),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Text("This is the detail page.", new TextStyle(Color: FrameColor.FromHex("#E2E8F0")))
                        },
                        new Row
                        {
                            Children = new Element[]
                            {
                                new Button(
                                    label: navigator.CanPop ? "Back" : "Back (disabled)",
                                    onPressed: () =>
                                    {
                                        if (navigator.CanPop)
                                        {
                                            navigator.Pop();
                                        }
                                    }
                                ),
                                SizedBox.FromSize(width: 8f),
                                new Button(
                                    label: "Go to Settings",
                                    onPressed: () => navigator.Push(RouteSettings)
                                ),
                            }
                        },
                    }
                }
            };
        }

        private static Element BuildSettingsRoute(BuildContext context)
        {
            var navigator = context.GetInherited<NavigatorScope>().Navigator;

            return new Padding(EdgeInsets.All(16f))
            {
                Child = new Column
                {
                    Children = new Element[]
                    {
                        new Text("Settings Route", new TextStyle(FontSize: 20f, Bold: true, Color: FrameColor.FromHex("#E2E8F0"))),
                        new Padding(EdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new Text("CanPop: " + navigator.CanPop, new TextStyle(Color: FrameColor.FromHex("#E2E8F0")))
                        },
                        new Button(
                            label: navigator.CanPop ? "Back" : "Back (disabled)",
                            onPressed: () =>
                            {
                                if (navigator.CanPop)
                                {
                                    navigator.Pop();
                                }
                            }
                        ),
                    }
                }
            };
        }
    }
}