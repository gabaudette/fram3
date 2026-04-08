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
            Children =
            [
                new FText { Text = $"Count: {_count}" },
                new FButton
                {
                    Label = "Increment",
                    OnPressed = () => SetState(() => _count++)
                }
            ]
        };
}
```

Elements are immutable descriptions. `SetState` schedules a rebuild, and the reconciler updates only what changed in the UIToolkit tree.

## Tests

The test suite runs with `dotnet test` and does not require the Unity editor.

```
dotnet test Packages/com.fram3.ui/Tests/Fram3.UI.Tests.csproj
```

CI runs on every pull request targeting `main`.
