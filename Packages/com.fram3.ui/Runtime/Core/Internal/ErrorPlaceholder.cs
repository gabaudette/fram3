#nullable enable
using System;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// <status>live</status>
    /// A fallback element rendered in place of a subtree whose Build method threw an exception.
    /// Displays the exception type and message inside a red-bordered box.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class ErrorPlaceholder : StatelessElement
    {
        private readonly Exception _exception;

        internal ErrorPlaceholder(Exception exception)
        {
            _exception = exception;
        }

        public override Element Build(BuildContext context)
        {
            return new Container(
                padding: EdgeInsets.All(8f),
                decoration: new BoxDecoration(
                    Color: FrameColor.FromHex("#1A0000"),
                    Border: new Border(FrameColor.FromHex("#FF4444"), 2f),
                    BorderRadius: BorderRadius.All(4f)
                )
            )
            {
                Child = new Text(
                    $"[fram3] {_exception.GetType().Name}: {_exception.Message}",
                    new TextStyle(
                        FontSize: 11f,
                        Color: FrameColor.FromHex("#FF8888")
                    )
                )
            };
        }
    }
}