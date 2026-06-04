# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [3.2.0-beta] - 2026-06-04

### Performance

- **ShouldRebuild on all built-in elements**: the reconciler now skips `Paint` calls for elements whose props have not changed, eliminating redundant VisualElement style writes on every reconcile pass (#90)
- **Storybook navigation**: replaced the `SetState` cascade with a per-item `ValueNotifier<bool>` pattern; a story selection now rebuilds only the two changed sidebar items and the content area — rebuilt count reduced from 66 to 29, update ops from ~120 to ~26 per navigation

## [3.1.0-beta] - 2026-06-03

### Added

- **ProfilerMarkers**: `Fram3.Tick`, `Fram3.Flush`, `Fram3.Rebuild`, `Fram3.Diff`, `Fram3.Mount`, and `Fram3.Unmount` markers are now emitted unconditionally. They appear in the Unity Profiler timeline with zero overhead when the profiler is not attached (#89)
- **FRAM3_FRAMEWORK_DIAGNOSTICS**: opt-in scripting define that activates per-frame `FrameMetrics` collection (flush/diff timing, mount/unmount/rebuild counts, diff op breakdown, build exceptions). Metrics are emitted via `Fram3Diagnostics.OnFrameMetrics` at the end of each `Renderer.Tick`. Completely invisible to end users unless the define is set (#89)

## [3.0.1-beta] - 2026-05-30

### Fixed

- **package.json**: removed incorrect `com.unity.modules.uielements` dependency that was never required (#88)

## [3.0.0-beta] - 2026-05-23

### Added

- **Theme.FontFamily**: new token for setting a global `FontAsset` applied to all text elements; individual `TextStyle` instances can override it per-element (#82)
- **Theme.ScrollbarBorder**: new boolean token to toggle scrollbar border visibility (#80)
- **Theme.ScrollbarBorderWidth**: new token controlling scrollbar border width in pixels (#80)
- **Theme.ScrollbarBorderColor**: new token controlling scrollbar border color (#80)

### Fixed

- **All text inputs** (TextField, PasswordField, IntField, FloatField): themed blinking caret now appears using `PrimaryColor`; cursor width set to 2px via reflection on `m_CursorWidth`; blinks at 530ms interval on focus and hides on blur (#83)
- **RadioGroup**: label text (`.unity-radio-button__text`) now has correct left margin and theme color; previous code was targeting the empty `BaseField` label element instead (#84)
- **FrameSlider**: track, fill, and drag container now render with `BorderRadius` and rounded corners via `Painter2D`; background images cleared to prevent Unity defaults overriding theme styles (#81)
- **Dropdown**: closed-state input now applies `BorderRadius` and `InputBorderColor` correctly (#81)
- **All inputs and controls**: `Theme.BorderRadius` now propagates to inner sub-elements (checkmark backgrounds, input containers, scrollbars) (#81)
- **DraggablePanel**: resize grip rewritten with `generateVisualContent`; endpoints inset to prevent corner overflow (#78)
- **PlayerPrefsAdapter**: replaced external JSON dependencies with `JsonUtility` to remove Newtonsoft.Json and fix serialization on all Unity platforms (#77, #79)

## [2.0.0-beta.2] - 2026-05-19

### Added

- **Theme.ScrollbarWidth**: new token controlling the width of vertical scrollbars and height of horizontal scrollbars (#73)
- **Theme.ScrollbarBorder**: new boolean token to toggle scrollbar border visibility (#73)
- **Theme.ScrollbarBorderWidth**: new token controlling scrollbar border width in pixels (#73)
- **Theme.ScrollbarBorderColor**: new token controlling scrollbar border color (#73)
- **DraggablePanel**: new element with drag-to-move and resize grip with min/max constraints (#70)
- **ContextMenu / ContextMenuItem**: new elements for right-click context menus (#69)
- **Table**: new element with column sorting, row selection, and theme-driven styling (#68)
- **TreeView**: new element with expand/collapse and chevron indicator (#66)
- **Stepper**: new input element for incrementing/decrementing a numeric value (#65)
- **Card**: new content element with elevated/outlined variants and optional header/footer (#64)
- **Avatar**: new content element with image, initials, and icon variants (#63)
- **Accordion**: new content element with slide open/close animation (#62)
- **Alert / Dialog**: new overlay elements with full-screen modal backdrop (#61)
- **Chip**: new content element with optional dismiss button (#60)
- **Badge**: new content element with count and dot variants (#59)
- **GlobalKey**: static registry for cross-tree node access (#58)
- **ShouldRebuild**: hook on stateless elements to short-circuit unnecessary rebuilds (#57)
- **DidMount**: lifecycle hook on stateful elements (#56)
- **SetStateIfMounted**: guard for safe `SetState` calls in async callbacks (#55)

### Fixed

- **ScrollView / ListView**: scrollbar width now visually applies correctly by propagating to the inner slider child and zeroing all USS-sourced inner borders (#73)
- **Container**: `flexShrink` set to 0 when explicit width is set; grows and stretches correctly without explicit width (#68)
- **Text**: `whiteSpace` set to `Normal` on all labels to enable text wrapping (#64)

### Performance

- **TreeDiffer**: replaced O(n²) `HasUnmatchedBefore` scan with O(n log n) `SortedSet` lookup (#72)
- **Grid**: replaced full `RebuildNativeGrid` with incremental `PatchNativeGrid` to avoid unnecessary child churn (#71)

## [1.0.0-beta.1] - 2026-05-14

### Fixed

- **Button**: `DidUpdateElement` no longer calls `SetState` unconditionally on every parent rebuild; it now skips the rebuild when neither `Label` nor the disabled state has changed, eliminating spurious re-renders (#51)
- **Error boundaries**: exceptions thrown inside any element's `Build()` method are now caught per-node; the faulted node is marked with `IsFaulted = true`, the exception is logged via `Debug.WriteLine`, and an `ErrorPlaceholder` is rendered in place of the failed subtree instead of crashing the whole tree (#52)
- **Scheduler**: faulted nodes are skipped in `RebuildScheduler.FlushInternal` so a faulted node does not repeatedly attempt to rebuild (#52)
- **Checkbox / Toggle**: checkmark is now drawn via `Painter2D` rather than relying on Unity's background image tint, fixing the invisible checkmark on custom themes (#50)
- **Dropdown**: hover highlight now uses `PrimaryColor` at 15% alpha instead of a hardcoded colour (#50)

### Performance

- **ImplicitAnimation**: `BuildSnapshot` now reuses a pre-allocated `Dictionary` field on `ImplicitAnimationState`, clearing and refilling it each tick instead of allocating a new `Dictionary` per animation frame (#53)
- **Theme resolution**: `NativeAdapter` caches the resolved `Theme` per node; the cache is invalidated for descendant nodes when a `ThemeProvider` is rebuilt and cleared on unmount, eliminating the full ancestor walk on every `OnMounted` and `OnRebuilt` call (#53)
- **ListView**: `PaintListView` now caches `IndexListCount` on `ListViewDescriptorHolder` and only rebuilds and reassigns `itemsSource` when `ItemCount` changes, skipping the list allocation on repaints with the same count (#53)

### Added

- `BETA_GAPS.md` at the repo root documents all known missing elements, infrastructure gaps, and performance limitations that are out of scope for the beta (#54)

## [1.0.0-alpha] - 2026-04-15

Initial alpha release. Includes the full element library, animation system, state management, theme support, navigation, storybook, and API documentation site.
