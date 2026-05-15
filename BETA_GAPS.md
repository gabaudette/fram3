# Beta Gaps

This document lists known missing elements, infrastructure gaps, and performance limitations as of the beta release. Items here are out of scope for the beta but are tracked for post-beta work.

## Missing Elements

The following UI elements are not yet implemented:

- **Badge** - numeric or status indicator overlaid on another element
- **Chip** - compact, dismissable label or filter tag
- **Alert / Dialog** - modal or inline alert with title, message, and actions
- **Accordion** - collapsible section list
- **Switch (iOS-style)** - toggle with a sliding thumb, distinct from the checkbox-based Toggle
- **Avatar** - circular image or initials placeholder
- **Card** - elevated surface container with optional header and footer
- **Stepper** - numeric input with increment/decrement buttons
- **Breadcrumb** - hierarchical navigation path
- **Rating** - star or icon-based rating input
- **TreeView** - hierarchical expandable/collapsible list
- **Table** - columnar data display with optional sorting and row selection
- **Context Menu** - right-click or long-press popup menu
- **Autocomplete** - text input with filtered suggestions dropdown
- **DatePicker** - calendar-based date selection input

## Missing Infrastructure

- **Accessibility** - no ARIA roles, focus management, keyboard navigation, or screen reader support
- **Live theme switching** - changing the active `ThemeProvider` at runtime does not update already-mounted native elements; a full unmount/remount is required
- **Dark mode tokens** - `Theme` has no built-in light/dark token set; consumers must define their own theme variants

## Known Performance Limitations

- **RebuildNativeGrid full clear/rebuild** - on every parent rebuild, the native grid is fully cleared and recreated from scratch rather than diffed; this causes visible flicker and O(n) native allocations on any parent state change
- **Unkeyed child diffing is O(n^2)** - `TreePatcher` matches old and new children by element type scan without keys; adding or removing items in the middle of a list causes worst-case quadratic patching
