using System;
using System.Collections.Generic;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Base class for elements that contain exactly one child element.
    /// Use this for wrapper elements that add behavior or styling around
    /// a single child, such as padding, alignment, or opacity.
    /// </summary>
    public abstract class FSingleChildElement : FElement
    {
        /// <summary>
        /// The single child element wrapped by this element.
        /// </summary>
        public FElement Child { get; }

        /// <summary>
        /// Creates a new single-child element.
        /// </summary>
        /// <param name="child">The child element to wrap.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
        protected FSingleChildElement(FElement child, FKey? key = null) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        /// <summary>
        /// Returns a list containing the single child element.
        /// </summary>
        public override IReadOnlyList<FElement> GetChildren()
        {
            return new[] { Child };
        }
    }

    /// <summary>
    /// Base class for elements that contain multiple child elements.
    /// Use this for layout elements that arrange several children,
    /// such as columns, rows, or stacks.
    /// </summary>
    public abstract class FMultiChildElement : FElement
    {
        private readonly FElement[] _children;

        /// <summary>
        /// The child elements contained in this element.
        /// </summary>
        public IReadOnlyList<FElement> ElementChildren => _children;

        /// <summary>
        /// Creates a new multi-child element.
        /// </summary>
        /// <param name="children">The child elements.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when children is null.</exception>
        protected FMultiChildElement(FElement[] children, FKey? key = null) : base(key)
        {
            _children = children ?? throw new ArgumentNullException(nameof(children));
            ValidateNoNullChildren(_children);
        }

        /// <summary>
        /// Returns the child elements of this element.
        /// </summary>
        public override IReadOnlyList<FElement> GetChildren()
        {
            return _children;
        }

        private static void ValidateNoNullChildren(FElement[] children)
        {
            for (var i = 0; i < children.Length; i++)
            {
                if (children[i] is null)
                {
                    throw new ArgumentException(
                        $"Child element at index {i} is null. All children must be non-null.",
                        nameof(children)
                    );
                }
            }
        }
    }

    /// <summary>
    /// Base class for leaf elements that have no children and produce rendered output.
    /// Use this for elements that map directly to visual components such as text labels,
    /// images, or interactive controls.
    /// </summary>
    public abstract class FLeafElement : FElement
    {
        /// <summary>
        /// Creates a new leaf element.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected FLeafElement(FKey? key = null) : base(key)
        {
        }
    }
}
