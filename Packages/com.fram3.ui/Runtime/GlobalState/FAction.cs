#nullable enable

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// Marker base class for all actions dispatched to an <see cref="FStore{TState}"/>.
    /// Subclass this to create strongly-typed action types that your reducer can
    /// pattern-match on.
    /// </summary>
    /// <example>
    /// <code>
    /// public sealed class IncrementAction : FAction { }
    /// public sealed class SetCountAction : FAction
    /// {
    ///     public int Value { get; }
    ///     public SetCountAction(int value) { Value = value; }
    /// }
    /// </code>
    /// </example>
    public abstract class FAction
    {
    }
}
