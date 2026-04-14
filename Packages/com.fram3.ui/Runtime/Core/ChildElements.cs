#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Base class for elements that contain exactly one child element.
    /// Use this for wrapper elements that add behavior or styling around
    /// a single child, such as padding, alignment, or opacity.
    /// </summary>
    public abstract class SingleChildElement : Element
    {
        private readonly Element? _child;

        /// <summary>
        /// The single child element wrapped by this element.
        /// Must be set during object initialization.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when set to null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by <see cref="GetChildren"/> when this property was never assigned.
        /// </exception>
        public Element Child
        {
            get => _child!;
            init => _child = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Returns true when <see cref="Child"/> was assigned during object initialization.
        /// Subclasses that support optional children should check this before calling
        /// <see cref="GetChildren"/> to avoid the null-child exception.
        /// </summary>
        protected bool HasChild => _child is not null;

        /// <summary>
        /// Creates a new single-child element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected SingleChildElement(Key? key = null) : base(key)
        {
        }

        /// <summary>
        /// Returns a list containing the single child element.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="Child"/> was never assigned during initialization.
        /// </exception>
        public override IReadOnlyList<Element> GetChildren()
        {
            if (_child is null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} requires the Child property to be set during initialization."
                );
            }

            return new[] { _child };
        }
    }

    /// <summary>
    /// Base class for elements that contain multiple child elements.
    /// Use this for layout elements that arrange several children,
    /// such as columns, rows, or stacks.
    /// </summary>
    public abstract class MultiChildElement : Element
    {
        private readonly Element[]? _children;

        /// <summary>
        /// The child elements contained in this element.
        /// Must be set during object initialization.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when set to null.</exception>
        /// <exception cref="ArgumentException">Thrown when any element in the array is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by <see cref="GetChildren"/> when this property was never assigned.
        /// </exception>
        public Element[] Children
        {
            get => _children!;
            init
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                ValidateNoNullChildren(value);
                _children = value;
            }
        }

        /// <summary>
        /// Creates a new multi-child element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected MultiChildElement(Key? key = null) : base(key)
        {
        }

        /// <summary>
        /// Returns the child elements of this element.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="Children"/> was never assigned during initialization.
        /// </exception>
        public override IReadOnlyList<Element> GetChildren()
        {
            if (_children is null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} requires the Children property to be set during initialization."
                );
            }

            return _children;
        }

        private static void ValidateNoNullChildren(Element[] children)
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
    public abstract class LeafElement : Element
    {
        /// <summary>
        /// Creates a new leaf element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected LeafElement(Key? key = null) : base(key)
        {
        }
    }
}