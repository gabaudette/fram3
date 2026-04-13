# Fram3

Inspired by Flutter, Fram3 is a declarative UI framework for Unity built on UIToolkit. Compose reactive interfaces in pure C# with no UXML, no USS files, and no code generation.

**Documentation: [fram3-docs.vercel.app](https://fram3-docs.vercel.app)**

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

## Storybook

The repository includes a Storybook that demonstrates every element and feature of the framework. Open the project in Unity and run the `Storybook` scene to browse live, interactive examples covering layout, state, animation, navigation, and input.

For full API documentation and guides, see **[fram3-docs.vercel.app](https://fram3-docs.vercel.app)**.
