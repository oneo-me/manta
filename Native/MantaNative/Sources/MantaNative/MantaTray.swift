// MantaTray.swift
// Native Tray implementation for Manta framework

import Foundation
import AppKit

// MARK: - Tray Click Callback Type

/// Callback type for tray icon click events
/// Parameters: x coordinate, y coordinate (center-bottom of the icon in Avalonia coordinates),
///             screenX, screenY (origin of the screen containing the tray icon)
public typealias TrayClickCallback = @convention(c) (Double, Double, Double, Double) -> Void

// MARK: - Tray Manager

/// Manages Tray instances and provides a stable handle-based API
final class TrayManager {
    static let shared = TrayManager()

    private var trays: [Int64: MantaTray] = [:]
    private var nextHandle: Int64 = 1
    private let lock = NSLock()

    private init() {}

    func create(iconData: Data, isTemplate: Bool, callback: TrayClickCallback?) -> Int64 {
        lock.lock()
        defer { lock.unlock() }

        let handle = nextHandle
        nextHandle += 1

        let tray = MantaTray(iconData: iconData, isTemplate: isTemplate, callback: callback)
        trays[handle] = tray

        return handle
    }

    func destroy(handle: Int64) {
        lock.lock()
        defer { lock.unlock() }

        if let tray = trays.removeValue(forKey: handle) {
            tray.cleanup()
        }
    }

    func get(handle: Int64) -> MantaTray? {
        lock.lock()
        defer { lock.unlock() }
        return trays[handle]
    }
}

// MARK: - MantaTray

/// Wrapper around NSStatusItem with template icon support
final class MantaTray: NSObject {
    private let statusItem: NSStatusItem
    private let icon: NSImage?
    private var callback: TrayClickCallback?

    init(iconData: Data, isTemplate: Bool, callback: TrayClickCallback?) {
        // Create image from data
        self.icon = Self.createMenuBarImage(from: iconData, isTemplate: isTemplate)
        self.callback = callback

        // Create status item
        self.statusItem = NSStatusBar.system.statusItem(withLength: NSStatusItem.squareLength)

        super.init()

        NSLog("MantaTray: StatusItem created, button exists: \(statusItem.button != nil)")

        // Configure the button
        if let button = statusItem.button {
            // Use the icon if valid, otherwise create a test image
            if let validIcon = icon, validIcon.isValid {
                button.image = validIcon
                NSLog("MantaTray: Using provided icon")
            } else {
                // Create a simple test image (red square) to verify status item works
                let testImage = NSImage(size: NSSize(width: 18, height: 18))
                testImage.lockFocus()
                NSColor.red.setFill()
                NSRect(x: 0, y: 0, width: 18, height: 18).fill()
                testImage.unlockFocus()
                button.image = testImage
                NSLog("MantaTray: Using test image (red square)")
            }

            button.target = self
            button.action = #selector(statusItemClicked(_:))
            button.sendAction(on: [.leftMouseUp, .rightMouseUp])

            NSLog("MantaTray: Button configured, image set: \(button.image != nil), image size: \(button.image?.size ?? .zero)")
        }
    }

    /// Creates a properly sized image for the menu bar
    /// Standard menu bar icon size is 18x18 points. For retina displays, provide 36x36 pixel images.
    private static func createMenuBarImage(from data: Data, isTemplate: Bool) -> NSImage? {
        guard let image = NSImage(data: data) else {
            NSLog("MantaTray: Failed to create NSImage from data (length: \(data.count))")
            return nil
        }

        NSLog("MantaTray: Original image size: \(image.size), reps: \(image.representations.count), valid: \(image.isValid)")

        // Standard menu bar icon size in points
        let menuBarIconSize: CGFloat = 18.0

        // Set the image size to the standard menu bar size in points
        // If the source image is 36x36 pixels, it will be displayed at 2x on retina
        // If the source image is 18x18 pixels, it will be 1x
        image.size = NSSize(width: menuBarIconSize, height: menuBarIconSize)

        // Set as template for proper menu bar rendering (system adjusts color automatically)
        image.isTemplate = isTemplate

        NSLog("MantaTray: Adjusted image size to: \(image.size)")

        return image
    }

    func cleanup() {
        NSStatusBar.system.removeStatusItem(statusItem)
        callback = nil
    }

    @objc private func statusItemClicked(_ sender: NSStatusBarButton) {
        guard let callback = callback else { return }

        // Get the frame of the status item button
        guard let button = statusItem.button,
              let window = button.window,
              let screen = window.screen else { return }

        // Get the button's frame in screen coordinates (macOS global coordinates)
        let buttonFrame = button.convert(button.bounds, to: nil)
        let screenFrame = window.convertToScreen(buttonFrame)

        // Get the primary screen height for coordinate conversion
        // macOS: origin at bottom-left, y increases upward
        // Avalonia: origin at top-left, y increases downward
        let primaryScreenHeight = NSScreen.screens.first?.frame.height ?? screen.frame.height

        // Calculate center-bottom position in Avalonia coordinates
        let centerX = screenFrame.midX
        // Convert Y: Avalonia Y = primaryScreenHeight - macOS Y
        let bottomY = primaryScreenHeight - screenFrame.minY

        // Get the screen origin in Avalonia coordinates
        let screenOriginX = screen.frame.origin.x
        let screenOriginY = primaryScreenHeight - screen.frame.maxY

        callback(centerX, bottomY, screenOriginX, screenOriginY)
    }

    func setCallback(_ callback: TrayClickCallback?) {
        self.callback = callback
    }
}

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

// MARK: - Main Thread Helpers

private func runOnMain<T>(_ block: @escaping () -> T) -> T {
    if Thread.isMainThread {
        return block()
    } else {
        return DispatchQueue.main.sync { block() }
    }
}

private func runOnMainAsync(_ block: @escaping () -> Void) {
    if Thread.isMainThread {
        block()
    } else {
        DispatchQueue.main.async { block() }
    }
}

// MARK: - C Exports for Tray

/// Creates a new Tray instance and returns its handle
/// - Parameters:
///   - iconData: Pointer to icon PNG data
///   - iconLength: Length of icon data
///   - isTemplate: Whether to treat the icon as a template image (true for monochrome icons)
///   - callback: Click callback function pointer
/// - Returns: Handle to the created tray instance
@_cdecl("manta_tray_create")
public func manta_tray_create(
    _ iconData: UnsafePointer<UInt8>,
    _ iconLength: Int32,
    _ isTemplate: Bool,
    _ callback: TrayClickCallback?
) -> Int64 {
    return runOnMain {
        NSLog("MantaTray: manta_tray_create called, iconLength: \(iconLength), isTemplate: \(isTemplate)")
        let data = Data(bytes: iconData, count: Int(iconLength))
        return TrayManager.shared.create(iconData: data, isTemplate: isTemplate, callback: callback)
    }
}

/// Destroys a Tray instance by its handle
@_cdecl("manta_tray_destroy")
public func manta_tray_destroy(_ handle: Int64) {
    runOnMainAsync {
        TrayManager.shared.destroy(handle: handle)
    }
}

/// Sets the click callback for a Tray instance
@_cdecl("manta_tray_set_callback")
public func manta_tray_set_callback(_ handle: Int64, _ callback: TrayClickCallback?) {
    runOnMainAsync {
        TrayManager.shared.get(handle: handle)?.setCallback(callback)
    }
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

