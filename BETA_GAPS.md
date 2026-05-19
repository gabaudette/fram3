# Beta Gaps

This document lists known missing elements, infrastructure gaps, and performance limitations as of the beta release. Items here are out of scope for the beta but are tracked for post-beta work.

## Missing Elements

The following UI elements are not yet implemented:

- **DraggablePanel** - resizable and draggable floating panel that attaches to the root container like Modal; will require the same `AttachChildrenToNative` skip treatment, likely via a shared `IRootAttached` marker interface

## Missing Infrastructure

- **Accessibility** - no focus management, keyboard navigation, or screen reader support; deferred to a dedicated beta

## Known Performance Limitations

- **RebuildNativeGrid full clear/rebuild** - on every parent rebuild, the native grid is fully cleared and recreated from scratch rather than diffed; this causes visible flicker and O(n) native allocations on any parent state change
- **Unkeyed child diffing is O(n^2)** - `TreePatcher` matches old and new children by element type scan without keys; adding or removing items in the middle of a list causes worst-case quadratic patching
