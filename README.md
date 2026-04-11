# Fram3

Inspired by Flutter, Fram3 is a declarative UI framework for Unity built on UIToolkit. Compose reactive interfaces in pure C# with no UXML, no USS files, and no code generation.

> **This project is under active development and is not ready for use.** The public API is incomplete and will change without notice until a stable release is published.

## Overview

Fram3 lets you describe your UI as a tree of immutable element objects. On each build cycle, the framework diffs the new element tree against the previous one and applies the minimal set of changes to the underlying UIToolkit visual tree. State changes trigger targeted rebuilds, not full redraws.

Layout, styling, state, and navigation are all expressed through C# types. There are no markup files to maintain alongside your code.

## Requirements

- Unity 6000.3 or later
- UIToolkit (included with Unity)

## Installation

Add the package via the Unity Package Manager using a Git URL:

```
https://github.com/gabaudette/fram3.git?path=Packages/com.fram3.ui
```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.fram3.ui": "https://github.com/gabaudette/fram3.git?path=Packages/com.fram3.ui"
  }
}
```

## Example

```csharp
public class CounterElement : FStatefulElement
{
    public override FState CreateState() => new CounterState();
}

public class CounterState : FState<CounterElement>
{
    private int _count;

    public override FElement Build(FBuildContext context) =>
        new FColumn
        {
            Children = new FElement[]
            {
                new FText { Text = $"Count: {_count}" },
                new FButton
                {
                    Label = "Increment",
                    OnPressed = () => SetState(() => _count++)
                }
            }
        };
}
```

Elements are immutable descriptions. `SetState` schedules a rebuild, and the reconciler updates only what changed in the UIToolkit tree.

## Global state

### FCubit

`FCubit<TState>` is a reactive state holder. Subclass it, call `Emit` to transition state, and attach listeners to react to changes. Equality is checked before notifying; identical states are ignored.

```csharp
public class CounterCubit : FCubit<int>
{
    public CounterCubit() : base(0) { }

    public void Increment() => Emit(State + 1);
    public void Decrement() => Emit(State - 1);
}
```

`FStore<TState>` extends `FCubit` with a Redux-style reducer for action-driven transitions:

```csharp
public record CounterState(int Count);

public class IncrementAction : FAction { }
public class DecrementAction : FAction { }

public class CounterStore : FStore<CounterState>
{
    public CounterStore() : base(
        new CounterState(0),
        (state, action) => action switch
        {
            IncrementAction => state with { Count = state.Count + 1 },
            DecrementAction => state with { Count = state.Count - 1 },
            _ => state
        }) { }
}
```

### FProvider and FConsumer

`FProvider<T>` injects a value into the subtree. `FConsumer<T>` reads the nearest `FProvider<T>` above it.

```csharp
// Provide a cubit to a subtree.
new FProvider<CounterCubit>(
    value: new CounterCubit(),
    child: new MyWidget()
)

// Read it anywhere in that subtree.
public class MyWidget : FStatelessElement
{
    public override FElement Build(FBuildContext context)
    {
        var cubit = FConsumer<CounterCubit>.Of(context);
        return new FText { Text = $"State: {cubit.State}" };
    }
}
```

### FCubitBuilder

`FCubitBuilder<TCubit, TState>` subscribes to a cubit from the nearest `FProvider<TCubit>` and rebuilds its subtree whenever the state changes.

```csharp
new FCubitBuilder<CounterCubit, int>(
    (context, count) => new FText { Text = $"Count: {count}" }
)
```

### FSelector

`FSelector<TCubit, TState, TValue>` is like `FCubitBuilder` but only rebuilds when a derived value changes, skipping unnecessary rebuilds.

```csharp
// Only rebuilds when the parity of the count changes.
new FSelector<CounterCubit, int, bool>(
    selector: count => count % 2 == 0,
    builder: (context, isEven) => new FText { Text = isEven ? "Even" : "Odd" }
)
```

### FPersistentStore

`FPersistentStore<TState>` extends `FCubit` and automatically persists state through an `IPersistenceAdapter<TState>`. In Unity, use `PlayerPrefsAdapter<TState>` which serialises state to `PlayerPrefs` via JSON.

```csharp
public class SettingsCubit : FPersistentStore<SettingsState>
{
    public SettingsCubit()
        : base(new SettingsState(Volume: 1f), new PlayerPrefsAdapter<SettingsState>("settings")) { }

    public void SetVolume(float v) => Emit(State with { Volume = v });
}
```

## Animation

### FAnimationController

`FAnimationController` is a time-driven 0-to-1 value that advances each frame. Create one inside `FState.InitState`, start it with `Forward` or `Reverse`, and dispose it in `FState.Dispose`. The controller is ticked automatically by `FRenderer.Tick(deltaTime)`.

```csharp
public class FadeElement : FStatefulElement
{
    public override FState CreateState() => new FadeState();
}

public class FadeState : FState<FadeElement>
{
    private FAnimationController _controller = null!;

    public override void InitState()
    {
        _controller = new FAnimationController(duration: 0.4f, curve: FCurves.EaseOut);
        _controller.AddListener(_ => SetState(null));
        _controller.Forward();
    }

    public override FElement Build(FBuildContext context) =>
        new FContainer(
            decoration: new FBoxDecoration(
                Color: FColor.Blue.WithAlpha(_controller.Value)
            )
        );

    public override void Dispose()
    {
        _controller.Dispose();
    }
}
```

### FAnimationBuilder

`FAnimationBuilder` is a self-contained element that owns its controller. Supply a duration, an optional easing curve, and a builder that receives the controller. Start playback by calling `Forward` or `Reverse` on the controller from within the builder or a gesture handler.

```csharp
new FAnimationBuilder(
    duration: 0.3f,
    curve: FCurves.EaseInOut,
    builder: (context, ctrl) =>
        new FGestureDetector(
            onTap: () => ctrl.Status == FAnimationStatus.Forward
                ? ctrl.Reverse()
                : ctrl.Forward(),
            child: new FContainer(
                decoration: new FBoxDecoration(
                    Color: FLerp.Color(FColor.White, FColor.Blue, ctrl.Value)
                )
            )
        )
)
```

### FCurves

A set of built-in easing curves available as static fields on `FCurves`. Any `Func<float, float>` is also a valid curve.

| Curve | Description |
|---|---|
| `FCurves.Linear` | Constant rate |
| `FCurves.EaseIn` | Starts slowly, accelerates |
| `FCurves.EaseOut` | Starts quickly, decelerates |
| `FCurves.EaseInOut` | Slow at both ends |
| `FCurves.ElasticOut` | Overshoots then settles |
| `FCurves.BounceOut` | Bounces at the end |

### FLerp

`FLerp` provides linear interpolation for all Fram3 styling types. Pass a start value, an end value, and the controller's current `Value` to blend between them each frame.

```csharp
// Blend a color.
FLerp.Color(FColor.Black, FColor.White, ctrl.Value)

// Blend padding.
FLerp.EdgeInsets(FEdgeInsets.All(0f), FEdgeInsets.All(24f), ctrl.Value)

// Blend a full box decoration.
FLerp.BoxDecoration(decorationA, decorationB, ctrl.Value)
```

Available overloads: `Float`, `NullableFloat`, `Color`, `NullableColor`, `EdgeInsets`, `Alignment`, `BorderRadius`, `Border`, `Shadow`, `BoxDecoration`, `TextStyle`.

## Implicit animations

Implicit animations automatically tween between old and new property values when those values change between builds. No controller setup is required -- the framework detects changes and drives the interpolation internally.

### FImplicitAnimation

`FImplicitAnimation` is a general-purpose element that takes a list of `FAnimatedValue<T>` entries and a builder. When any target value changes, the element tweens from the previous snapshot to the new target over `duration`. The builder receives an `FImplicitAnimationSnapshot` from which the current interpolated values can be read by key.

```csharp
new FImplicitAnimation(
    duration: 0.3f,
    values: new IFAnimatedValue[]
    {
        new FAnimatedValue<float>("opacity", _isVisible ? 1f : 0f, FLerp.Float),
        new FAnimatedValue<FColor>("color", _isActive ? FColor.Blue : FColor.Grey, FLerp.Color),
    },
    builder: (context, snapshot) =>
        new FContainer(
            decoration: new FBoxDecoration(
                Color: snapshot.Get<FColor>("color").WithAlpha(snapshot.Get<float>("opacity"))
            )
        )
)
```

### FAnimatedContainer

`FAnimatedContainer` is a convenience wrapper that implicitly animates container decoration, width, height, and padding. When any of these properties change between builds, the element tweens smoothly to the new values.

```csharp
new FAnimatedContainer(
    duration: 0.3f,
    curve: FCurves.EaseInOut,
    child: new FText("Hello"),
    decoration: _isHighlighted
        ? new FBoxDecoration(Color: FColor.Blue, BorderRadius: FBorderRadius.All(8f))
        : new FBoxDecoration(Color: FColor.Grey),
    width: _isExpanded ? 300f : 100f,
    padding: FEdgeInsets.All(_isExpanded ? 16f : 8f)
)
```

### FAnimatedValue

`FAnimatedValue<T>` pairs a target value with a lerp function. Use the built-in `FLerp` methods for Fram3 types, or supply a custom delegate for any value type.

```csharp
// Built-in type with FLerp.
new FAnimatedValue<float>("radius", targetRadius, FLerp.Float)

// Custom lerp for a game-specific value type.
new FAnimatedValue<Vector2>("position", targetPos, (a, b, t) =>
    new Vector2(FLerp.Float(a.x, b.x, t), FLerp.Float(a.y, b.y, t)))
```

## Navigation

### FNavigator

`FNavigator` is a stack-based in-scene navigator. Declare your routes as a dictionary mapping names to builder functions, provide an initial route name, and place the navigator anywhere in your element tree.

```csharp
new FNavigator(
    routes: new Dictionary<string, Func<FBuildContext, FElement>>
    {
        { "home",   _ => new HomePage() },
        { "detail", _ => new DetailPage() },
        { "settings", _ => new SettingsPage() },
    },
    initialRoute: "home"
)
```

Only the top route is built and visible at any time. The navigator renders its content as a subtree -- wrap it in a container to constrain its size within a larger layout.

### Pushing and popping

Any descendant can obtain the navigator handle from context and call `Push` or `Pop` on it.

```csharp
public class HomePage : FStatelessElement
{
    public override FElement Build(FBuildContext context)
    {
        var nav = context.GetInherited<FNavigatorScope>().Navigator;

        return new FButton
        {
            Label = "Go to detail",
            OnPressed = () => nav.Push("detail")
        };
    }
}
```

`Push` accepts an optional arguments object that is forwarded to the route builder:

```csharp
nav.Push("detail", arguments: new DetailArgs(id: 42));
```

`Pop` removes the top route. It is a no-op when the stack contains only the initial route:

```csharp
public class DetailPage : FStatelessElement
{
    public override FElement Build(FBuildContext context)
    {
        var nav = context.GetInherited<FNavigatorScope>().Navigator;

        return new FColumn
        {
            Children = new FElement[]
            {
                new FText { Text = "Detail" },
                new FButton
                {
                    Label = "Back",
                    OnPressed = () => nav.Pop()
                }
            }
        };
    }
}
```

Use `CanPop` to check whether a back action is available before showing a back button:

```csharp
nav.CanPop
    ? new FButton { Label = "Back", OnPressed = () => nav.Pop() }
    : new FSizedBox()
```

### FSceneNavigator

`FSceneNavigator.GoTo` loads a Unity scene by name, replacing the current scene. It is a static call -- no build context or element tree is required.

```csharp
FSceneNavigator.GoTo("MainMenu");
```

`GoTo` returns an `FSceneOperation` that tracks loading progress. Subscribe to `Completed` to run logic once the scene is active, or poll `Progress` each frame to drive a loading bar.

```csharp
var op = FSceneNavigator.GoTo("GameScene");
op.Completed += () => Debug.Log("Scene loaded.");
```

A typical loading screen stores the operation in state and updates progress on each renderer tick:

```csharp
public class LoadingScreen : FStatefulElement
{
    public string TargetScene { get; init; } = string.Empty;
    public override FState CreateState() => new LoadingScreenState();
}

public class LoadingScreenState : FState<LoadingScreen>
{
    private FSceneOperation _operation = null!;

    public override void InitState()
    {
        _operation = FSceneNavigator.GoTo(Element!.TargetScene);
        _operation.Completed += () => SetState(null);
    }

    public override FElement Build(FBuildContext context) =>
        new FText { Text = $"{_operation.Progress * 100:0}%" };
}
```

Call `SetState(null)` from a `MonoBehaviour.Update` or from a per-frame listener to refresh the progress display each frame. `Completed` fires once when the scene is fully active, triggering a final rebuild.

For simpler cases where you only need to trigger a scene change on button press:

```csharp
new FButton
{
    Label = "Play",
    OnPressed = () => FSceneNavigator.GoTo("GameScene")
}
```

### Nested navigators

Multiple `FNavigator` instances can coexist in the same tree. `context.GetInherited<FNavigatorScope>()` always resolves to the nearest enclosing navigator, so nested navigators manage their own independent stacks.

```csharp
new FRow
{
    Children = new FElement[]
    {
        new FNavigator(routes: sidebarRoutes, initialRoute: "menu"),
        new FNavigator(routes: contentRoutes, initialRoute: "home"),
    }
}
```

## Tests

The test suite runs with `dotnet test` and does not require the Unity editor.

```
dotnet test Packages/com.fram3.ui/Tests/Fram3.UI.Tests.csproj
```

CI runs on every pull request targeting `main`.
