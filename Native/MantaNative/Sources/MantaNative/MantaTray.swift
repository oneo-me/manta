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
