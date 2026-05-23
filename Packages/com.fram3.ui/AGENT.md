# Fram3.UI — Agent Instructions

This file is the authoritative reference for AI agents working in codebases that consume Fram3.UI. Keep it updated whenever public API changes are made.

Fram3.UI is a Flutter-inspired, declarative UI framework for Unity built on top of UIToolkit. Elements are immutable descriptions of UI; the renderer diffs and patches the UIToolkit visual tree each frame.

Unity version: 6000.3.10f1

---

## Bootstrapping

### Recommended: subclass AppRoot

```csharp
using Fram3.UI.Rendering;

public class MyAppRoot : AppRoot
{
    public override Element Build() => new MyApp();
}
```

Attach `MyAppRoot` to a GameObject that also has a `UIDocument`. No other setup required.

### Manual: use Renderer directly

```csharp
using Fram3.UI.Rendering;

public class MyManualRoot : MonoBehaviour
{
    private Renderer _renderer;

    void Start()
    {
        _renderer = new Renderer();
        _renderer.Mount(new MyApp(), GetComponent<UIDocument>().rootVisualElement);
    }

    void Update() => _renderer.Tick(Time.deltaTime);
    void OnDestroy() => _renderer.Dispose();
}
```

---

## Element Hierarchy

All UI is composed of `Element` subclasses. Elements are immutable — rebuild by returning new instances from `Build`.

| Base class | When to use |
|---|---|
| `StatelessElement` | Pure function of inputs. Override `Build(BuildContext)`. |
| `StatefulElement` | Needs mutable local state. Override `CreateState()` returning a `State<T>`. |
| `InheritedElement` | Provides data down the tree. Override `UpdateShouldNotify()`. |
| `LeafElement` | Direct wrapper around a UIToolkit native. Rarely subclassed by consumers. |

### State lifecycle

```csharp
public class MyState : State<MyElement>
{
    public override void InitState() { }           // called once on mount
    public override void DidUpdateElement(MyElement old) { }  // called when element rebuilds
    public override void Dispose() { }             // called on unmount
    public override Element Build(BuildContext context) => ...;
}
```

Call `SetState(() => { /* mutate fields */ })` to trigger a rebuild.

---

## Keys

| Type | Constructor |
|---|---|
| `StringKey` | `new StringKey("id")` |
| `IntKey` | `new IntKey(42)` |

Pass as `key:` to any element constructor. Used to preserve state across rebuilds when element type or position changes.

---

## Layout Elements

Namespace: `Fram3.UI.Elements.Layout`

```csharp
new Column(
    children: new Element[] { ... },
    mainAxisAlignment: MainAxisAlignment.Start,   // Start Center End SpaceBetween SpaceAround SpaceEvenly
    crossAxisAlignment: CrossAxisAlignment.Start  // Start Center End Stretch
)

new Row(
    children: new Element[] { ... },
    mainAxisAlignment: MainAxisAlignment.Start,
    crossAxisAlignment: CrossAxisAlignment.Start
)

new Stack(
    children: new Element[] { ... },
    alignment: Alignment.TopLeft  // see Alignment section
)

new Wrap(
    children: new Element[] { ... },
    spacing: 8f,
    runSpacing: 4f
)

new Container(
    decoration: new BoxDecoration(...),
    width: 200f,
    height: 100f,
    padding: EdgeInsets.All(16f),
    alignment: Alignment.Center
)
// Container.Child is optional — set via property initializer: new Container(...) { Child = ... }

new SizedBox(width: 100f, height: 50f)
// SizedBox.Child is optional: new SizedBox(100f) { Child = ... }
// SizedBox.Expand() equivalent: new SizedBox(float.MaxValue, float.MaxValue)

new Padding(insets: EdgeInsets.Symmetric(horizontal: 16f)) { Child = ... }
new Margin(insets: EdgeInsets.All(8f)) { Child = ... }
new Center() { Child = ... }
new Expanded(flex: 1) { Child = ... }
new Divider(axis: DividerAxis.Horizontal, thickness: 1f, color: FrameColor.FromHex("#E0E0E0"))

new Grid<MyItem>(
    columnCount: 4,
    items: myList,
    itemBuilder: item => new Text(item.Name),
    columnSpacing: 8f,   // gap between cells within a row (default 0)
    rowSpacing: 8f       // gap between rows (default 0)
)
// Grid<T> is a StatelessElement. It builds a Column of Rows of Expanded cells internally.
// Partial last rows are padded with empty Expanded cells to maintain column alignment.
// columnCount must be > 0. items and itemBuilder must not be null.
```

---

## Content Elements

Namespace: `Fram3.UI.Elements.Content`

```csharp
new Text("Hello", style: new TextStyle(fontSize: 16f, bold: true))

new Icon(source: preloadedVectorImage, width: 24f, height: 24f)
new Icon(resourcePath: "Icons/star", width: 24f, height: 24f)   // Resources.Load — works in builds
new Icon(svgPath: "Assets/Icons/star.svg", width: 24f, height: 24f)  // AssetDatabase — Editor only

new FrameImage("path/to/image", width: 128f, height: 128f, fit: BoxFit.Contain)

new ProgressBar(value: 0.75f, height: 4f, color: FrameColor.FromHex("#2196F3"))

new Spinner(size: 32f, color: FrameColor.FromHex("#2196F3"))

new ScrollView(child: new Column(...), direction: ScrollDirection.Vertical)

new TabView(
    tabs: new[]
    {
        new Tab("Tab A", new Text("Content A")),
        new Tab("Tab B", new Text("Content B")),
    },
    initialIndex: 0
)

new Snackbar(
    message: "Saved!",
    duration: 4f,
    onDismiss: () => SetState(() => _showSnackbar = false)  // optional — snackbar hides itself regardless
)

new Tooltip(message: "More info", child: new Icon("info"))
```

### ListView

`ListView` requires an `IListViewDescriptor`. Use the generic helper:

```csharp
new ListView(
    descriptor: new ListViewDescriptor<MyItem>(
        items: myList,
        itemBuilder: item => new Text(item.Name),
        itemHeight: 48f
    ),
    selectionMode: ListSelectionMode.Single
)
```

---

## Input Elements

Namespace: `Fram3.UI.Elements.Input`

```csharp
new Button(
    label: "Save",
    onPressed: () => Debug.Log("clicked")
    // onPressed: null means disabled
)

new TextField(
    value: _text,
    onChanged: v => SetState(() => _text = v),
    placeholder: "Enter text..."
)

new PasswordField(value: _pass, onChanged: v => SetState(() => _pass = v))

new Checkbox(value: _checked, onChanged: v => SetState(() => _checked = v))

new FrameToggle(value: _on, onChanged: v => SetState(() => _on = v))

new FrameSlider(value: _val, min: 0f, max: 100f, onChanged: v => SetState(() => _val = v))

new MinMaxSlider(
    minValue: _min, maxValue: _max,
    min: 0f, max: 100f,
    onChanged: (lo, hi) => SetState(() => { _min = lo; _max = hi; })
)

new Dropdown<string>(
    value: _selected,
    items: new[] { "A", "B", "C" },
    onChanged: v => SetState(() => _selected = v)
)

new RadioGroup<string>(
    value: _selected,
    items: new[] { "A", "B", "C" },
    onChanged: v => SetState(() => _selected = v)
)

new IntField(value: _intVal, onChanged: v => SetState(() => _intVal = v), label: "Count")

new FloatField(value: _floatVal, onChanged: v => SetState(() => _floatVal = v))

new EnumField<MyEnum>(value: _enum, onChanged: v => SetState(() => _enum = v))
// EnumField<T> constraint: T : struct, Enum
```

---

## Gesture Elements

Namespace: `Fram3.UI.Elements.Gesture`

```csharp
new GestureDetector(
    child: new Text("click me"),
    onTap: () => Debug.Log("tapped"),
    onDoubleTap: () => Debug.Log("double tapped"),
    onLongPress: () => Debug.Log("long press"),
    onPointerEnter: () => Debug.Log("hover in"),
    onPointerExit: () => Debug.Log("hover out")
)

new Opacity(opacity: 0.5f, child: new Text("faded"))
// opacity is clamped to [0, 1]

new Modal(
    visible: _showModal,
    child: new Container(...),
    barrierColor: new FrameColor(0f, 0f, 0f, 0.5f),
    onBarrierTap: () => SetState(() => _showModal = false)
)
```

---

## Styling Types

Namespace: `Fram3.UI.Styling`

### FrameColor

```csharp
FrameColor.FromHex("#2196F3")
FrameColor.FromHex("#2196F380")      // with alpha
new FrameColor(0.13f, 0.59f, 0.95f) // float r/g/b, alpha defaults to 1
new FrameColor(33, 150, 243)         // int 0-255, alpha defaults to 255
FrameColor.White
FrameColor.Black
FrameColor.Transparent
```

### EdgeInsets

```csharp
EdgeInsets.All(16f)
EdgeInsets.Symmetric(horizontal: 24f, vertical: 8f)
EdgeInsets.Only(left: 8f, top: 4f, right: 8f, bottom: 4f)
new EdgeInsets(left: 8f, top: 4f, right: 8f, bottom: 4f)
```

### BorderRadius

```csharp
BorderRadius.All(8f)
BorderRadius.Only(topLeft: 8f, topRight: 8f)
new BorderRadius(topLeft: 4f, topRight: 4f, bottomRight: 8f, bottomLeft: 8f)
```

### BoxDecoration

```csharp
new BoxDecoration(
    Color: FrameColor.FromHex("#FFFFFF"),
    BorderRadius: BorderRadius.All(8f),
    Border: new Border(FrameColor.FromHex("#E0E0E0"), 1f),
    Shadow: Shadow.Ambient(new FrameColor(0f, 0f, 0f, 0.1f), 8f)
)
```

### TextStyle

```csharp
new TextStyle(
    FontSize: 14f,
    Color: FrameColor.Black,
    Bold: true,
    Italic: false,
    LetterSpacing: 0f,
    LineHeight: 1.4f,
    TextAlign: TextAnchor.MiddleCenter,      // maps to style.unityTextAlign
    FontAsset: myFontAsset                   // UnityEngine.TextCore.Text.FontAsset — SDF rendering
)
```

### Alignment

```csharp
Alignment.TopLeft      Alignment.TopCenter     Alignment.TopRight
Alignment.CenterLeft   Alignment.Center        Alignment.CenterRight
Alignment.BottomLeft   Alignment.BottomCenter  Alignment.BottomRight
new Alignment(x: -1f, y: -1f)  // x/y in [-1, 1]
```

---

## Theming

Namespace: `Fram3.UI.Elements.Theme`

Wrap a subtree to inject a theme:

```csharp
new ThemeProvider(
    theme: Theme.Default,  // or a custom Theme(...)
    child: new MyApp()
)
```

Read the theme anywhere in the tree:

```csharp
// Option 1: static method (preferred in StatelessElement.Build)
var theme = ThemeConsumer.Of(context);

// Option 2: builder element
new ThemeConsumer(builder: (ctx, theme) => new Text("hello", style: theme.DefaultTextStyle))
```

`Button` is a `StatefulElement` that reads the theme internally. It renders a full styled widget tree, not a raw UIToolkit button. Do not expect `Button` to expose a `VisualElement` directly.

Custom theme tokens are passed to the `Theme` constructor — all parameters are optional and named.

---

## State Management

### Local reactive state: ValueNotifier + ValueListenableBuilder

```csharp
// In StatefulElement state class:
private readonly ValueNotifier<int> _counter = new(0);

public override Element Build(BuildContext context) =>
    new ValueListenableBuilder<int>(
        valueGetter: () => _counter.Value,
        builder: (ctx, val) => new Text($"Count: {val}")
    );

// Increment:
_counter.Value++;
```

### Cubit (BLoC-style)

```csharp
public class CounterCubit : Cubit<int>
{
    public CounterCubit() : base(0) { }
    public void Increment() => Emit(State + 1);
}

// Provide:
new Provider<CounterCubit>(
    store: new CounterCubit(),   // note: Provider wraps any Store<T> or Cubit<T>
    child: new MyWidget()
)

// Consume:
new CubitBuilder<int>(
    cubit: context.GetInherited<...>(),  // or pass cubit reference directly
    builder: (ctx, count) => new Text($"{count}")
)
```

### Selector (prevent rebuilds on unrelated state changes)

```csharp
new Selector<AppState, string>(
    selector: state => state.Username,
    builder: (ctx, name) => new Text(name)
)
```

### Store (Redux-style)

```csharp
var store = new Store<AppState>(
    initialState: new AppState(),
    reducer: (state, action) => action switch
    {
        IncrementAction => state with { Count = state.Count + 1 },
        _ => state
    }
);

store.Dispatch(new IncrementAction());
```

### PersistentStore

```csharp
var store = new PersistentStore<AppState>(
    initialState: new AppState(),
    reducer: MyReducer,
    adapter: new PlayerPrefsAdapter<AppState>("app_state")  // Unity only; uses System.Text.Json
);
```

### Provider / Consumer pattern

```csharp
// Provide a store to the tree:
new Provider<AppState>(store: myStore, child: new MyApp())

// Consume anywhere below:
new Consumer<AppState>(builder: (ctx, state) => new Text(state.Username))
```

---

## Navigation

Namespace: `Fram3.UI.Navigation`

### In-tree navigation

```csharp
new Navigator(home: new HomeScreen())
```

Navigate from within a `Build` method:

```csharp
var nav = Navigator.Of(context);
nav.Push(new DetailScreen());
nav.Pop();
nav.Replace(new OtherScreen());
bool canPop = nav.CanPop;
```

### Scene navigation (Unity only)

```csharp
SceneNavigator.LoadScene("GameScene");
SceneNavigator.LoadSceneAsync("GameScene");
SceneNavigator.UnloadSceneAsync("GameScene");
```

---

## Animation

Namespace: `Fram3.UI.Animation` / `Fram3.UI.Elements.Animation`

### Explicit animation

```csharp
new AnimationBuilder(
    duration: 0.3f,
    curve: Curves.EaseOut,
    builder: (ctx, controller) =>
    {
        var opacity = controller.Value;
        return new Opacity(opacity, child: new Text("Fade me"));
    }
)
```

`AnimationController` methods: `Forward()`, `Reverse()`, `Stop()`, `Reset()`.
`AnimationStatus` values: `Idle`, `Forward`, `Reverse`, `Completed`, `Dismissed`.

Available curves: `Curves.Linear`, `Curves.EaseIn`, `Curves.EaseOut`, `Curves.EaseInOut`.

### Implicit animation

```csharp
new ImplicitAnimation(
    duration: 0.25f,
    values: new IAnimatedValue[]
    {
        new AnimatedValue<float>("opacity", target: _visible ? 1f : 0f, lerp: Lerp.Float)
    },
    builder: (ctx, snapshot) =>
    {
        var opacity = snapshot.Get<float>("opacity");
        return new Opacity(opacity, child: new Text("Animated"));
    }
)
```

### Animated container (shorthand)

```csharp
new AnimatedContainer(
    duration: 0.3f,
    child: new Text("hello"),
    decoration: new BoxDecoration(Color: _highlighted ? FrameColor.FromHex("#FFFF00") : FrameColor.White),
    width: _expanded ? 300f : 150f,
    curve: Curves.EaseInOut
)
```

### Lerp helpers

```csharp
Lerp.Float(a, b, t)
Lerp.Color(a, b, t)
Lerp.BoxDecoration(a, b, t)
Lerp.EdgeInsets(a, b, t)
Lerp.BorderRadius(a, b, t)
Lerp.Shadow(a, b, t)
```

---

## Known Gotchas

- `Button.OnPressed = null` disables the button. There is no separate `enabled` flag.
- `Container.Child`, `SizedBox.Child`, `Center.Child`, `Expanded.Child`, `Padding.Child`, `Margin.Child` are all optional and set via property initializer after construction, not as constructor parameters.
- `Opacity` constructor signature is `(float opacity, Element child, Key? key)` — child is positional second argument.
- `GestureDetector` constructor signature is `(Element child, ...)` — child is positional first argument.
- `Modal` constructor signature is `(bool visible, Element child, ...)` — child is positional second argument.
- `Button` is a `StatefulElement` that internally consumes the theme and renders a composed widget tree. It is not a thin wrapper around `UnityEngine.UIElements.Button`.
- `EnumField<T>` requires `T : struct, Enum`.
- `Dropdown<T>` and `RadioGroup<T>` accept `IReadOnlyList<T>`, not arrays — wrap arrays with `.ToList()` or cast if needed.
- `ListView` takes an `IListViewDescriptor`, not a raw list. Use the provided `ListViewDescriptor<T>` implementation.
- `AnimationSystem.Tick(deltaTime)` must be called each frame if using `AnimationController` outside of `AnimationBuilder`. When using `AppRoot` this is handled automatically.
- `ThemeConsumer.Of(context)` is the preferred way to read the theme. `ThemeConsumer` as an element is an alternative when you need to scope a builder.
- All `Element` constructors accept an optional `Key? key` as the last parameter.

---

## Maintenance

Update this file whenever:
- A public constructor signature changes
- A new public element, type, or namespace is added
- A property is renamed or removed
- A behavioral contract changes (e.g. null semantics, defaults)
