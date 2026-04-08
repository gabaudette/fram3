# Fram3

A declarative UI framework for Unity built on UIToolkit. Compose reactive interfaces in pure C# -- no UXML, no USS files, no code generation.

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

## Status

The framework is under active development and not yet suitable for production use. Public APIs may change between phases.

| Phase | Scope | Status |
|-------|-------|--------|
| 1 | Project scaffolding | Complete |
| 2 | Core engine (elements, state, build context) | Complete |
| 3 | Reconciliation (differ, patcher, rebuild scheduler) | Complete |
| 4 | Styling types | Planned |
| 5 | Render bridge | Planned |
| 6 | Basic elements (FText, FColumn, FRow, FContainer, FButton, ...) | Planned |
| 7 | State and binding (FValueNotifier, FInheritedElement) | Planned |
| 8 | Input elements (FTextField, FToggle, FSlider, ...) | Planned |
| 9 | Global state (FProvider, FConsumer, FStore, FCubit, ...) | Planned |
| 10 | Animation core | Planned |
| 11 | Implicit animations | Planned |
| 12 | In-scene navigation (FNavigator, FRoute) | Planned |
| 13 | Scene navigation (FSceneNavigator) | Planned |
| 14 | Additional elements (FStack, FScrollView, FImage, ...) | Planned |

## Architecture

Fram3 maintains three parallel trees at runtime:

- **FElement** -- immutable descriptions of the UI, recreated on every build
- **FNode** -- mutable reconciliation nodes that track state and identity across rebuilds
- **VisualElement** -- the UIToolkit DOM, updated in place by the render bridge

Styling is done through composition. Elements like `FPadding`, `FContainer`, and `FSizedBox` set `VisualElement.style` properties directly in C#.

## Tests

The test suite runs with `dotnet test` and does not require the Unity editor.

```
dotnet test Packages/com.fram3.ui/Tests/Fram3.UI.Tests.csproj
```

CI runs on every pull request targeting `main`.

## License

MIT
