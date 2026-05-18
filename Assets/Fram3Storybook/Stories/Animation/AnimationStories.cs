#nullable enable
using System.Collections.Generic;

namespace Fram3.UI.Storybook.Stories.Animation
{
    public static class AnimationStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "AnimatedContainer",
                    description: "A container whose size, decoration, and padding interpolate smoothly " +
                                 "to new values whenever its properties change.",
                    build: () => new AnimatedContainerStory()
                ),
                new Story(
                    name: "AnimationBuilder",
                    description: "Drives a single animation controller and rebuilds its child on every frame tick, " +
                                 "giving full programmatic control over the animated value.",
                    build: () => new AnimationBuilderStory()
                ),
                new Story(
                    name: "Curves",
                    description: "A library of pre-built easing functions -- Linear, EaseIn, EaseOut, EaseInOut, " +
                                 "ElasticOut, and BounceOut -- used to shape animation playback.",
                    build: () => new CurvesStory()
                ),
                new Story(
                    name: "ImplicitAnimation",
                    description: "Animates one or more named values toward their latest targets using " +
                                 "a shared duration and curve, without manual controller management.",
                    build: () => new ImplicitAnimationStory()
                )
            };
        }
    }
}