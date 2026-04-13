#nullable enable
#if FRAM3_PURE_TESTS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
    /// <summary>
    /// Stub for UIToolkit change events used by input element callbacks.
    /// </summary>
    public sealed class ChangeEvent<T>
    {
        public T newValue { get; }
        public T previousValue { get; }

        public ChangeEvent(T previousValue, T newValue)
        {
            this.previousValue = previousValue;
            this.newValue = newValue;
        }
    }

    /// <summary>Stub for UIToolkit ClickEvent.</summary>
    public sealed class ClickEvent { }

    /// <summary>Stub for UIToolkit PointerEnterEvent.</summary>
    public sealed class PointerEnterEvent { }

    /// <summary>Stub for UIToolkit PointerLeaveEvent.</summary>
    public sealed class PointerLeaveEvent { }

    public delegate void EventCallback<TEventType>(TEventType evt);

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
        public Align? alignSelf;
        public Overflow? overflow;
        public Wrap? flexWrap;
        public UnityEngine.FontStyle? unityFontStyleAndWeight;
        public Position? position;
        public float? opacity;
    }

    public enum Position
    {
        Relative,
        Absolute
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

    public enum Wrap
    {
        NoWrap,
        Wrap,
        WrapReverse
    }

    public enum Overflow
    {
        Visible,
        Hidden
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.VisualElement</c> used in pure C# tests.
    /// </summary>
    public class VisualElement
    {
        private readonly List<VisualElement> _children = new List<VisualElement>();
        private VisualElement? _parent;
        private readonly Dictionary<Type, List<object>> _callbacks = new Dictionary<Type, List<object>>();

        public StyleSheet style { get; } = new StyleSheet();
        public IReadOnlyList<VisualElement> Children => _children;
        public VisualElement? Parent => _parent;
        public int childCount => _children.Count;
        public string tooltip { get; set; } = string.Empty;

        public void Add(VisualElement child)
        {
            child._parent = this;
            _children.Add(child);
        }

        public void Clear()
        {
            foreach (var child in _children)
            {
                child._parent = null;
            }
            _children.Clear();
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

        public void RegisterCallback<TEventType>(EventCallback<TEventType> callback)
        {
            var key = typeof(TEventType);
            if (!_callbacks.ContainsKey(key))
            {
                _callbacks[key] = new List<object>();
            }

            _callbacks[key].Add(callback);
        }

        /// <summary>
        /// Test helper: fires all registered callbacks for the given event type.
        /// </summary>
        public void SimulateEvent<TEventType>(TEventType evt)
        {
            var key = typeof(TEventType);
            if (!_callbacks.TryGetValue(key, out var list))
            {
                return;
            }

            foreach (var cb in list)
            {
                ((EventCallback<TEventType>)cb)(evt);
            }
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

    /// <summary>
    /// Stub for <c>UnityEngine.UIElements.ITextEdition</c>.
    /// </summary>
    public sealed class TextEditionStub
    {
        public string? placeholder { get; set; }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.TextField</c>.
    /// </summary>
    public class TextField : VisualElement
    {
        public string value { get; set; } = string.Empty;
        public bool isReadOnly { get; set; }
        public bool multiline { get; set; }
        public bool isPasswordField { get; set; }
        public TextEditionStub textEdition { get; } = new TextEditionStub();

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<string>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new value.</summary>
        public void SimulateValueChanged(string newValue)
        {
            SimulateEvent(new ChangeEvent<string>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.Toggle</c>.
    /// </summary>
    public class Toggle : VisualElement
    {
        public bool value { get; set; }
        public string? label { get; set; }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<bool>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new value.</summary>
        public void SimulateValueChanged(bool newValue)
        {
            SimulateEvent(new ChangeEvent<bool>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.Slider</c>.
    /// </summary>
    public class Slider : VisualElement
    {
        public float value { get; set; }
        public float lowValue { get; set; }
        public float highValue { get; set; }
        public string? label { get; set; }

        public Slider(float start = 0f, float end = 1f)
        {
            lowValue = start;
            highValue = end;
        }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<float>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new value.</summary>
        public void SimulateValueChanged(float newValue)
        {
            SimulateEvent(new ChangeEvent<float>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.DropdownField</c>.
    /// </summary>
    public class DropdownField : VisualElement
    {
        public List<string> choices { get; set; }
        public int index { get; set; }
        public string? label { get; set; }

        public DropdownField(List<string> choices, int defaultIndex = -1)
        {
            this.choices = choices ?? new List<string>();
            index = defaultIndex;
        }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<string>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks simulating a selection change.</summary>
        public void SimulateIndexChanged(int newIndex)
        {
            var prev = index;
            index = newIndex;
            var prevValue = prev >= 0 && prev < choices.Count ? choices[prev] : string.Empty;
            var newValue = newIndex >= 0 && newIndex < choices.Count ? choices[newIndex] : string.Empty;
            SimulateEvent(new ChangeEvent<string>(prevValue, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.ScrollView</c>.
    /// </summary>
    public class ScrollView : VisualElement
    {
        public ScrollViewMode mode { get; set; }

        public ScrollView(ScrollViewMode mode = ScrollViewMode.Vertical)
        {
            this.mode = mode;
        }
    }

    public enum ScrollViewMode
    {
        Vertical,
        Horizontal,
        VerticalAndHorizontal
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.IntegerField</c>.
    /// </summary>
    public class IntegerField : VisualElement
    {
        public int value { get; set; }
        public string? label { get; set; }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<int>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new value.</summary>
        public void SimulateValueChanged(int newValue)
        {
            SimulateEvent(new ChangeEvent<int>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.FloatField</c>.
    /// </summary>
    public class FloatField : VisualElement
    {
        public float value { get; set; }
        public string? label { get; set; }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<float>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new value.</summary>
        public void SimulateValueChanged(float newValue)
        {
            SimulateEvent(new ChangeEvent<float>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.MinMaxSlider</c>.
    /// </summary>
    public class MinMaxSlider : VisualElement
    {
        public float minValue { get; set; }
        public float maxValue { get; set; }
        public float lowLimit { get; set; }
        public float highLimit { get; set; }
        public string? label { get; set; }

        public MinMaxSlider(float minValue = 0f, float maxValue = 1f, float lowLimit = 0f, float highLimit = 1f)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.lowLimit = lowLimit;
            this.highLimit = highLimit;
        }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<UnityEngine.Vector2>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new range.</summary>
        public void SimulateValueChanged(float newMin, float newMax)
        {
            var prev = new UnityEngine.Vector2(minValue, maxValue);
            var next = new UnityEngine.Vector2(newMin, newMax);
            SimulateEvent(new ChangeEvent<UnityEngine.Vector2>(prev, next));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.EnumField</c>.
    /// </summary>
    public class EnumField : VisualElement
    {
        public Enum? value { get; set; }
        public string? label { get; set; }

        public EnumField(Enum? initialValue = null)
        {
            value = initialValue;
        }

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<Enum?>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new enum value.</summary>
        public void SimulateValueChanged(Enum? newValue)
        {
            SimulateEvent(new ChangeEvent<Enum?>(value, newValue));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.ProgressBar</c>.
    /// </summary>
    public class ProgressBar : VisualElement
    {
        public float value { get; set; }
        public float lowValue { get; set; }
        public float highValue { get; set; }
        public string? title { get; set; }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.Image</c>.
    /// </summary>
    public class Image : VisualElement
    {
        public object? sprite { get; set; }
        public object? vectorImage { get; set; }
    }

    public enum SelectionType
    {
        None,
        Single,
        Multiple
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.RadioButtonGroup</c>.
    /// </summary>
    public class RadioButtonGroup : VisualElement
    {
        public int value { get; set; } = -1;
        public List<string> choices { get; set; } = new List<string>();

        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<int>> callback)
        {
            RegisterCallback(callback);
        }

        /// <summary>Test helper: fires value-changed callbacks with the given new index.</summary>
        public void SimulateValueChanged(int newIndex)
        {
            SimulateEvent(new ChangeEvent<int>(value, newIndex));
        }
    }

    /// <summary>
    /// Minimal stub for <c>UnityEngine.UIElements.ListView</c>.
    /// </summary>
    public class ListView : VisualElement
    {
        public Func<VisualElement>? makeItem { get; set; }
        public Action<VisualElement, int>? bindItem { get; set; }
        public float fixedItemHeight { get; set; } = 32f;
        public SelectionType selectionType { get; set; } = SelectionType.None;

        private readonly List<object> _selectedItems = new List<object>();
        public IReadOnlyList<object> selectedItems => _selectedItems;

        private Action<IEnumerable<object>>? _selectionChanged;

        public event Action<IEnumerable<object>> selectionChanged
        {
            add { _selectionChanged += value; }
            remove { _selectionChanged -= value; }
        }

        public void SetSelectionWithoutNotify(IEnumerable<int> indices)
        {
        }

        /// <summary>Test helper: fires selectionChanged with the provided items.</summary>
        public void SimulateSelectionChange(IEnumerable<object> items)
        {
            _selectionChanged?.Invoke(items);
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
