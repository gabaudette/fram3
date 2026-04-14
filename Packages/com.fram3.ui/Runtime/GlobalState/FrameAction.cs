#nullable enable

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// Marker base class for all actions dispatched to an <see cref="Store{TState}"/>.
    /// Subclass this to create strongly-typed action types that your reducer can
    /// pattern-match on.
    /// </summary>
    /// <example>
    /// <code>
    /// public sealed class IncrementAction : FrameAction { }
    /// public sealed class SetCountAction : FrameAction
    /// {
    ///     public int Value { get; }
    ///     public SetCountAction(int value) { Value = value; }
    /// }
    /// </code>
    /// </example>
    public abstract class FrameAction
    {
    }
}