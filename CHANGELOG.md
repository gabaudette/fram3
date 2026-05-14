# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

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
