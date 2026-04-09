#if FRAM3_PURE_TESTS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.VisualElement</c> used in pure C# tests.
    /// </summary>
    public class VisualElement
    {
        private readonly List<VisualElement> _children = new();
        private VisualElement? _parent;

        public IReadOnlyList<VisualElement> Children => _children;
        public VisualElement? Parent => _parent;
        public int ChildCount => _children.Count;

        public void Add(VisualElement child)
        {
            child._parent = this;
            _children.Add(child);
        }

        public void Remove(VisualElement child)
        {
            if (_children.Remove(child))
            {
                child._parent = null;
            }
        }

        public void RemoveFromHierarchy()
        {
            _parent?.Remove(this);
        }
    }
}

namespace UnityEngine
{
    public struct Color
    {
        public float r, g, b, a;

        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static Color white => new Color(1, 1, 1, 1);
        public static Color black => new Color(0, 0, 0, 1);
        public static Color red => new Color(1, 0, 0, 1);
        public static Color green => new Color(0, 1, 0, 1);
        public static Color blue => new Color(0, 0, 1, 1);
        public static Color clear => new Color(0, 0, 0, 0);

        public override bool Equals(object? obj)
        {
            if (obj is Color other)
            {
                return r == other.r && g == other.g && b == other.b && a == other.a;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(r, g, b, a);
        }

        public static bool operator ==(Color lhs, Color rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Color lhs, Color rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    public struct Vector2
    {
        public float x, y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 zero => new Vector2(0, 0);
        public static Vector2 one => new Vector2(1, 1);
    }

    public static class Mathf
    {
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Math.Clamp(t, 0f, 1f);
        }

        public static float Clamp01(float value)
        {
            return Math.Clamp(value, 0f, 1f);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Clamp(value, min, max);
        }
    }
}
#endif
