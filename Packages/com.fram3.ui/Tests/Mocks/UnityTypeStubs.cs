#nullable enable
#if FRAM3_PURE_TESTS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
    /// <summary>
    /// Minimal stub for style properties set by <c>FElementPainter</c> in pure C# tests.
    /// </summary>
    public sealed class StyleSheet
    {
        public float? flexGrow;
        public float? flexShrink;
        public float? width;
        public float? height;
        public float? paddingTop;
        public float? paddingRight;
        public float? paddingBottom;
        public float? paddingLeft;
        public float? marginTop;
        public float? marginRight;
        public float? marginBottom;
        public float? marginLeft;
        public float? borderTopWidth;
        public float? borderRightWidth;
        public float? borderBottomWidth;
        public float? borderLeftWidth;
        public float? borderTopLeftRadius;
        public float? borderTopRightRadius;
        public float? borderBottomRightRadius;
        public float? borderBottomLeftRadius;
        public float? fontSize;
        public UnityEngine.Color? color;
        public UnityEngine.Color? backgroundColor;
        public UnityEngine.Color? borderTopColor;
        public UnityEngine.Color? borderRightColor;
        public UnityEngine.Color? borderBottomColor;
        public UnityEngine.Color? borderLeftColor;
        public FlexDirection? flexDirection;
        public Justify? justifyContent;
        public Align? alignItems;
        public UnityEngine.FontStyle? unityFontStyleAndWeight;
    }

    public enum FlexDirection
    {
        Column,
        Row,
        ColumnReverse,
        RowReverse
    }

    public enum Justify
    {
        FlexStart,
        Center,
        FlexEnd,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly
    }

    public enum Align
    {
        Auto,
        FlexStart,
        Center,
        FlexEnd,
        Stretch
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.VisualElement</c> used in pure C# tests.
    /// </summary>
    public class VisualElement
    {
        private readonly List<VisualElement> _children = new();
        private VisualElement? _parent;

        public StyleSheet style { get; } = new StyleSheet();
        public IReadOnlyList<VisualElement> Children => _children;
        public VisualElement? Parent => _parent;
        public int childCount => _children.Count;

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

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.Label</c>.
    /// </summary>
    public class Label : VisualElement
    {
        public string text { get; set; }

        public Label(string text = "")
        {
            this.text = text ?? string.Empty;
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.Button</c>.
    /// </summary>
    public class Button : VisualElement
    {
        public string text { get; set; }
        public Action? clickedAction { get; }

        public Button(Action? clicked = null)
        {
            clickedAction = clicked;
            text = string.Empty;
        }
    }
}

namespace UnityEngine
{
    public enum FontStyle
    {
        Normal,
        Bold,
        Italic,
        BoldAndItalic
    }

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
