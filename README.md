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
new FProvider<CounterCubit>
{
    Value = new CounterCubit(),
    Child = new MyWidget()
}

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
new FCubitBuilder<CounterCubit, int>
{
    Builder = (context, count) => new FText { Text = $"Count: {count}" }
}
```

### FSelector

`FSelector<TCubit, TState, TValue>` is like `FCubitBuilder` but only rebuilds when a derived value changes, skipping unnecessary rebuilds.

```csharp
// Only rebuilds when the parity of the count changes.
new FSelector<CounterCubit, int, bool>
{
    Selector = count => count % 2 == 0,
    Builder = (context, isEven) => new FText { Text = isEven ? "Even" : "Odd" }
}
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

## Tests

The test suite runs with `dotnet test` and does not require the Unity editor.

```
dotnet test Packages/com.fram3.ui/Tests/Fram3.UI.Tests.csproj
```

CI runs on every pull request targeting `main`.
