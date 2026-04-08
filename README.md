# Fram3

A declarative UI framework for Unity built on UIToolkit. Write composable, reactive interfaces in pure C# with no UXML, no USS files, and no code generation.

## Overview

Fram3 lets you describe your UI as a tree of immutable element objects. The framework handles diffing, reconciliation, and updating the underlying UIToolkit visual tree automatically. State changes trigger targeted rebuilds rather than full redraws.

The API is element-based: you compose small, single-purpose building blocks into larger interfaces. Layout, styling, state, and navigation are all expressed through C# types.

```csharp
public class CounterElement : FStatefulElement
{
    public override FState CreateState() => new CounterState();
}

public class CounterState : FState<CounterElement>
{
    private int _count;

    public override FElement Build(FBuildContext context) =>
        new FColumn(children: new[]
        {
            new FText($"Count: {_count}"),
            new FButton("Increment", onPressed: () => SetState(() => _count++))
        });
}
```

## Requirements

- Unity 6000.3 or later
- UIToolkit (included with Unity)

## Installation

Add the package to your project via the Unity Package Manager using the Git URL:

```
https://github.com/gabaudette/fram3.git?path=Packages/com.fram3.ui
```

Or add it manually to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.fram3.ui": "https://github.com/gabaudette/fram3.git?path=Packages/com.fram3.ui"
  }
}
```

## Status

The framework is under active development. The table below reflects the current implementation state.

| Phase | Scope | Status |
|-------|-------|--------|
| 1 | Project scaffolding | Complete |
| 2 | Core engine (elements, state, build context) | Complete |
| 3 | Reconciliation (differ, patcher, rebuild scheduler) | Complete |
| 4 | Styling types | Planned |
| 5 | Render bridge | Planned |
| 6 | Basic elements (FText, FColumn, FRow, FContainer, FButton, ...) | Planned |
| 7 | State & binding (FValueNotifier, FInheritedElement) | Planned |
| 8 | Input elements (FTextField, FToggle, FSlider, ...) | Planned |
| 9 | Global state (FProvider, FConsumer, FStore, FCubit, ...) | Planned |
| 10 | Animation core | Planned |
| 11 | Implicit animations | Planned |
| 12 | In-scene navigation (FNavigator, FRoute) | Planned |
| 13 | Scene navigation (FSceneNavigator) | Planned |
| 14 | Additional elements (FStack, FScrollView, FImage, ...) | Planned |

The package is not yet suitable for production use. Public APIs may change between phases.

## Architecture

Fram3 maintains three parallel trees:

- **FElement** -- immutable descriptions of the UI, recreated on every build
- **FNode** -- mutable reconciliation nodes that track state and identity across rebuilds
- **VisualElement** -- the UIToolkit DOM, updated in place by the render bridge

Styling is done entirely through composition. There are no USS files. Elements like `FPadding`, `FContainer`, and `FSizedBox` set `VisualElement.style` properties directly.

## Tests

The test suite runs with `dotnet test` and does not require a Unity editor. Tests live in `Packages/com.fram3.ui/Tests/` and use NUnit via the standard .NET test runner.

```
dotnet test Packages/com.fram3.ui/Tests/Fram3.UI.Tests.csproj
```

CI runs on every pull request targeting `main`.

## License

MIT
