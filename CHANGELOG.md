# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Added
- **MTabTitleBar** - Tab-based title bar control for browser-style tabbed interfaces
  - Extends ListBox for tab item management
  - Properties: RightContent, RightContentTemplate
  - Horizontal scrolling support when tabs overflow

- **MTextBox Animated Cursor** - Custom animated text cursor
  - Smooth position animation using CubicEaseOut easing
  - IME (Input Method Editor) support
  - Configurable via CaretAnimationDuration property

- **MAnimatedExpander** - Container with animated height changes
  - Solves Avalonia's limitation of not supporting transitions for Auto-sized values
  - Properties: IsExpanded, Duration, Easing

- **SmoothBrushTransition** - Smooth color/brush interpolation
  - Supports SolidColorBrush and GradientBrush
  - Premultiplied alpha blending for correct opacity handling

- **MCommand** - Command palette / searchable command menu control
  - Variants: Default, Separator, Group, NoResults

- **MToggleButton** - Toggle button with variants
  - Variants: Outline, WithText
  - Sizes: Default, Small, Large

- **MCheckBox** - Enhanced checkbox with custom header support
  - Variants: Default, Bordered
  - Properties: Header, HeaderTemplate, HeaderPresenter

- **PopupRoot** - Root container for popup windows

### Changed
- Project renamed from Mantan to Manta
- Control prefix changed from Light* to M*
- Optimized scroll view styles
- Optimized title bar styles

### Fixed
- Fixed text box scrolling incomplete issue
