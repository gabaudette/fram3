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
                new Story("FNavigator", "A stack-based router that maps string route keys to builder functions; supports Push, Pop, and replace operations via FNavigatorHandle.",  BuildNavigator),
            };
        }

        private static FElement BuildNavigator()
        {
            return new FContainer(
                decoration: new FBoxDecoration(Color: FColor.FromHex("#FAFAFA")),
                height: 320f
            )
            {
                Child = new FNavigator(
                    routes: new Dictionary<string, System.Func<FBuildContext, FElement>>
                    {
                        [RouteHome]     = BuildHomeRoute,
                        [RouteDetail]   = BuildDetailRoute,
                        [RouteSettings] = BuildSettingsRoute,
                    },
                    initialRoute: RouteHome
                )
            };
        }

        private static FElement BuildHomeRoute(FBuildContext context)
        {
            var navigator = context.GetInherited<FNavigatorScope>().Navigator;

            return new FPadding(FEdgeInsets.All(16f))
            {
                Child = new FColumn
                {
                    Children = new FElement[]
                    {
                        new FText("Home Route", new FTextStyle(FontSize: 20f, Bold: true)),
                        new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new FText("Push a route using the buttons below.")
                        },
                        new FRow
                        {
                            Children = new FElement[]
                            {
                                new FButton(
                                    label: "Go to Detail",
                                    onPressed: () => navigator.Push(RouteDetail)
                                ),
                                FSizedBox.FromSize(width: 8f),
                                new FButton(
                                    label: "Go to Settings",
                                    onPressed: () => navigator.Push(RouteSettings)
                                ),
                            }
                        },
                    }
                }
            };
        }

        private static FElement BuildDetailRoute(FBuildContext context)
        {
            var navigator = context.GetInherited<FNavigatorScope>().Navigator;

            return new FPadding(FEdgeInsets.All(16f))
            {
                Child = new FColumn
                {
                    Children = new FElement[]
                    {
                        new FText("Detail Route", new FTextStyle(FontSize: 20f, Bold: true)),
                        new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new FText("This is the detail page.")
                        },
                        new FRow
                        {
                            Children = new FElement[]
                            {
                                new FButton(
                                    label: navigator.CanPop ? "Back" : "Back (disabled)",
                                    onPressed: () =>
                                    {
                                        if (navigator.CanPop)
                                        {
                                            navigator.Pop();
                                        }
                                    }
                                ),
                                FSizedBox.FromSize(width: 8f),
                                new FButton(
                                    label: "Go to Settings",
                                    onPressed: () => navigator.Push(RouteSettings)
                                ),
                            }
                        },
                    }
                }
            };
        }

        private static FElement BuildSettingsRoute(FBuildContext context)
        {
            var navigator = context.GetInherited<FNavigatorScope>().Navigator;

            return new FPadding(FEdgeInsets.All(16f))
            {
                Child = new FColumn
                {
                    Children = new FElement[]
                    {
                        new FText("Settings Route", new FTextStyle(FontSize: 20f, Bold: true)),
                        new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                        {
                            Child = new FText("CanPop: " + navigator.CanPop)
                        },
                        new FButton(
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
