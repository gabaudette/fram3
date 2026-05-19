# Beta Gaps

This document lists known missing elements, infrastructure gaps, and performance limitations as of the beta release. Items here are out of scope for the beta but are tracked for post-beta work.

## Missing Elements

No elements are currently outstanding for the beta release.

## Missing Infrastructure

- **Accessibility** - no focus management, keyboard navigation, or screen reader support; deferred to a dedicated beta

## Known Performance Limitations

- **Unkeyed child diffing is O(n^2)** - `TreePatcher` matches old and new children by element type scan without keys; adding or removing items in the middle of a list causes worst-case quadratic patching
