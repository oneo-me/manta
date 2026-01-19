// MantaTray.swift
// Native Tray implementation for Manta framework

import Foundation
import AppKit

// MARK: - Window Level Management

/// Window level constants matching NSWindow.Level
public enum MantaWindowLevel: Int {
    case normal = 0
    case floating = 3
    case submenu = 4
    case tornOffMenu = 5
    case mainMenu = 11
    case status = 12
    case modalPanel = 13
    case popUpMenu = 101
    case screenSaver = 1000
}

/// Window collection behavior flags
public struct MantaWindowCollectionBehavior: OptionSet {
    public let rawValue: UInt

    public init(rawValue: UInt) {
        self.rawValue = rawValue
    }

    public static let `default` = MantaWindowCollectionBehavior([])
    public static let canJoinAllSpaces = MantaWindowCollectionBehavior(rawValue: 1 << 0)
    public static let moveToActiveSpace = MantaWindowCollectionBehavior(rawValue: 1 << 1)
    public static let managed = MantaWindowCollectionBehavior(rawValue: 1 << 2)
    public static let transient = MantaWindowCollectionBehavior(rawValue: 1 << 3)
    public static let stationary = MantaWindowCollectionBehavior(rawValue: 1 << 4)
    public static let participatesInCycle = MantaWindowCollectionBehavior(rawValue: 1 << 5)
    public static let ignoresCycle = MantaWindowCollectionBehavior(rawValue: 1 << 6)
    public static let fullScreenPrimary = MantaWindowCollectionBehavior(rawValue: 1 << 7)
    public static let fullScreenAuxiliary = MantaWindowCollectionBehavior(rawValue: 1 << 8)
    public static let fullScreenNone = MantaWindowCollectionBehavior(rawValue: 1 << 9)
    public static let fullScreenAllowsTiling = MantaWindowCollectionBehavior(rawValue: 1 << 11)
    public static let fullScreenDisallowsTiling = MantaWindowCollectionBehavior(rawValue: 1 << 12)
}

// MARK: - C Exports for Window Level/Type

/// Sets the window level for an NSWindow
/// - Parameters:
///   - windowHandle: Native NSWindow pointer
///   - level: Window level value
@_cdecl("manta_window_set_level")
public func manta_window_set_level(_ windowHandle: UnsafeMutableRawPointer, _ level: Int32) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        window.level = NSWindow.Level(rawValue: Int(level))
    }
}

/// Sets the window collection behavior
/// - Parameters:
///   - windowHandle: Native NSWindow pointer
///   - behavior: Collection behavior flags
@_cdecl("manta_window_set_collection_behavior")
public func manta_window_set_collection_behavior(_ windowHandle: UnsafeMutableRawPointer, _ behavior: UInt) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        window.collectionBehavior = NSWindow.CollectionBehavior(rawValue: behavior)
    }
}

/// Gets the current window level
/// - Parameter windowHandle: Native NSWindow pointer
/// - Returns: Current window level
@_cdecl("manta_window_get_level")
public func manta_window_get_level(_ windowHandle: UnsafeMutableRawPointer) -> Int32 {
    return runOnMain {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        return Int32(window.level.rawValue)
    }
}

// MARK: - Blur/Visual Effect State

/// Sets the NSVisualEffectView state to keep blur active even when window loses focus.
/// This prevents the blur from dimming when the window is not active.
/// - Parameters:
///   - windowHandle: Native NSWindow pointer
///   - alwaysActive: If true, blur stays active even when window loses focus
@_cdecl("manta_window_set_blur_always_active")
public func manta_window_set_blur_always_active(_ windowHandle: UnsafeMutableRawPointer, _ alwaysActive: Bool) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        setVisualEffectState(for: window.contentView, alwaysActive: alwaysActive)
    }
}

/// Recursively sets NSVisualEffectView state in the view hierarchy
private func setVisualEffectState(for view: NSView?, alwaysActive: Bool) {
    guard let view = view else { return }

    if let effectView = view as? NSVisualEffectView {
        effectView.state = alwaysActive ? .active : .followsWindowActiveState
    }

    for subview in view.subviews {
        setVisualEffectState(for: subview, alwaysActive: alwaysActive)
    }
}

// MARK: - Liquid Glass Effect Configuration

/// Configures a window with liquid glass (vibrancy) effect
/// This creates a frosted glass appearance similar to macOS HUD windows
/// - Parameter windowHandle: Native NSWindow pointer
@_cdecl("manta_window_configure_liquid_glass")
public func manta_window_configure_liquid_glass(_ windowHandle: UnsafeMutableRawPointer) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        guard let contentView = window.contentView else { return }

        NSLog("MantaTray: Configuring liquid glass effect")

        // Configure window for transparent background
        window.isOpaque = false
        window.backgroundColor = .clear

        // Keep shadow enabled for proper window appearance

        // Remove any existing visual effect views first
        removeVisualEffectViews(from: contentView)

        // Ensure content view has layer backing and is transparent
        contentView.wantsLayer = true
        contentView.layer?.cornerRadius = 0

        if #available(macOS 26.0, *) {
            NSLog("MantaTray: Using NSGlassEffectView (macOS 26.0+)")
            let effectView = NSGlassEffectView()
            effectView.translatesAutoresizingMaskIntoConstraints = false
            effectView.wantsLayer = true

            // Ensure proper layer configuration
            effectView.layer?.cornerRadius = 0
            effectView.layer?.masksToBounds = false

            // Configure autoresizing mask for autoresizing mask layout
            effectView.autoresizingMask = [.width, .height]

            // Add as subview
            contentView.addSubview(effectView)
            // Move to back using layer zPosition
            effectView.layer?.zPosition = -10
            
            // Pin to all edges
            NSLayoutConstraint.activate([
                effectView.leadingAnchor.constraint(equalTo: contentView.leadingAnchor),
                effectView.trailingAnchor.constraint(equalTo: contentView.trailingAnchor),
                effectView.topAnchor.constraint(equalTo: contentView.topAnchor),
                effectView.bottomAnchor.constraint(equalTo: contentView.bottomAnchor)
            ])

            // Force layout update
            contentView.layoutSubtreeIfNeeded()
        } else {
            NSLog("MantaTray: Using NSVisualEffectView fallback")
            // Fallback to NSVisualEffectView
            let effectView = NSVisualEffectView()
            effectView.translatesAutoresizingMaskIntoConstraints = false
            effectView.material = .windowBackground
            effectView.state = .active
            effectView.blendingMode = .withinWindow
            effectView.isEmphasized = true
            effectView.wantsLayer = true
            effectView.layer?.cornerRadius = 0
            effectView.autoresizingMask = [.width, .height]

            // Add as subview
            contentView.addSubview(effectView)
            // Move to back using layer zPosition
            effectView.layer?.zPosition = -1000

            NSLayoutConstraint.activate([
                effectView.leadingAnchor.constraint(equalTo: contentView.leadingAnchor),
                effectView.trailingAnchor.constraint(equalTo: contentView.trailingAnchor),
                effectView.topAnchor.constraint(equalTo: contentView.topAnchor),
                effectView.bottomAnchor.constraint(equalTo: contentView.bottomAnchor)
            ])

            // Force layout update
            contentView.layoutSubtreeIfNeeded()
        }
    }
}

/// Removes existing NSVisualEffectView from the view hierarchy
private func removeVisualEffectViews(from view: NSView?) {
    guard let view = view else { return }

    // Collect subviews first to avoid modifying while iterating
    var subviews: [NSView] = view.subviews.filter { $0 is NSVisualEffectView }

    if #available(macOS 26.0, *) {
        subviews.append(contentsOf: view.subviews.filter { $0 is NSGlassEffectView })
    }

    for subview in subviews {
        subview.removeFromSuperview()
    }

    // Recursively check nested views
    for subview in view.subviews {
        removeVisualEffectViews(from: subview)
    }
}

