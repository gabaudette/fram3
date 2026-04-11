#nullable enable

namespace Fram3.UI.Core
{
    /// <summary>
    /// Optional base class for state types used with <see cref="FCubit{TState}"/>
    /// and <see cref="FStore{TState}"/> that need structural value equality.
    /// Subclass this and override <see cref="Equals(FEquatable?)"/> to express
    /// what constitutes a meaningful state change. The cubit's <c>Emit</c> method
    /// uses this equality to skip rebuilds when the state has not changed.
    /// </summary>
    /// <remarks>
    /// Alternatively, any C# <c>record</c> type satisfies the same contract because
    /// records generate value equality automatically. You are not required to subclass
    /// <see cref="FEquatable"/> -- any type whose <see cref="object.Equals(object)"/>
    /// correctly reflects value equality is accepted by the framework.
    /// </remarks>
    public abstract class FEquatable
    {
        /// <summary>
        /// Determines whether this state is meaningfully equal to <paramref name="other"/>.
        /// Return <c>true</c> to suppress a rebuild; return <c>false</c> to trigger one.
        /// </summary>
        /// <param name="other">The state to compare against. May be null.</param>
        /// <returns>
        /// <c>true</c> if the two states are considered equal and a rebuild should be suppressed;
        /// <c>false</c> otherwise.
        /// </returns>
        public abstract bool Equals(FEquatable? other);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as FEquatable);

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();
    }
}
