import AppKit
import ObjectiveC

@_cdecl("manta_window_configure_window")
public func manta_window_configure_window(_ windowHandle: UnsafeMutableRawPointer) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()

        guard let contentView = window.contentView else { return }

        window.isOpaque = false
        window.backgroundColor = .clear

        removeVisualEffectViews(from: contentView)

        // contentView.layer?.cornerRadius = 22
        // contentView.layer?.cornerCurve = .continuous

        let effectView = NSVisualEffectView(frame: contentView.bounds)
        effectView.state = .active
        effectView.wantsLayer = true
        effectView.autoresizingMask = [.width, .height]
        effectView.material = .popover
        effectView.blendingMode = .behindWindow

        contentView.addSubview(effectView, positioned: .below, relativeTo: nil)

        // Move traffic light buttons 8px right and 8px down, persistently
        TrafficLightOffsetHelper.install(on: window, dx: 5, dy: 5)
    }
}

// MARK: - Traffic Light Offset Helper

private var trafficLightHelperKey: UInt8 = 0

private class TrafficLightOffsetHelper: NSObject {
    private weak var window: NSWindow?
    private let dx: CGFloat
    private let dy: CGFloat
    private var isApplying = false
    private var containerObservation: NSObjectProtocol?
    private var windowObservations: [NSObjectProtocol] = []

    static func install(on window: NSWindow, dx: CGFloat, dy: CGFloat) {
        let helper = TrafficLightOffsetHelper(window: window, dx: dx, dy: dy)
        // Prevent deallocation by associating with the window
        objc_setAssociatedObject(window, &trafficLightHelperKey, helper, .OBJC_ASSOCIATION_RETAIN_NONATOMIC)
    }

    private init(window: NSWindow, dx: CGFloat, dy: CGFloat) {
        self.window = window
        self.dx = dx
        self.dy = dy
        super.init()

        // Observe the buttons' container view frame changes
        if let closeButton = window.standardWindowButton(.closeButton),
           let container = closeButton.superview {
            container.postsFrameChangedNotifications = true
            containerObservation = NotificationCenter.default.addObserver(
                forName: NSView.frameDidChangeNotification,
                object: container,
                queue: .main
            ) { [weak self] _ in
                self?.applyOffset()
            }
        }

        // Also observe window-level events that trigger re-layout
        let names: [Notification.Name] = [
            NSWindow.didResizeNotification,
            NSWindow.didExitFullScreenNotification,
            NSWindow.didBecomeKeyNotification,
        ]
        for name in names {
            let obs = NotificationCenter.default.addObserver(
                forName: name, object: window, queue: .main
            ) { [weak self] _ in
                self?.applyOffset()
            }
            windowObservations.append(obs)
        }

        // Initial apply (delayed to run after Avalonia's own layout)
        DispatchQueue.main.async { [weak self] in
            self?.applyOffset()
        }
    }

    private func applyOffset() {
        guard !isApplying, let window = window else { return }
        isApplying = true
        defer { isApplying = false }

        guard let closeButton = window.standardWindowButton(.closeButton),
              let container = closeButton.superview,
              let titlebar = container.superview else { return }

        // Compute the expected origin: default flush top-left + our offsets
        let expectedX = container.frame.origin.x.rounded(.toNearestOrAwayFromZero) == dx.rounded(.toNearestOrAwayFromZero)
        let expectedY = (titlebar.bounds.height - container.frame.height - container.frame.origin.y).rounded(.toNearestOrAwayFromZero) == dy.rounded(.toNearestOrAwayFromZero)

        if expectedX && expectedY { return } // Already in the right position

        var frame = container.frame
        frame.origin.x = dx
        // macOS y is bottom-up; move down means reduce y from top
        frame.origin.y = titlebar.bounds.height - container.frame.height - dy
        container.frame = frame
    }

    deinit {
        if let obs = containerObservation {
            NotificationCenter.default.removeObserver(obs)
        }
        for obs in windowObservations {
            NotificationCenter.default.removeObserver(obs)
        }
    }
}

// MARK: - Helpers

private func removeVisualEffectViews(from view: NSView?) {
    guard let view = view else { return }
    let subviews = view.subviews.filter { $0 is NSVisualEffectView }
    subviews.forEach { $0.removeFromSuperview() }
}
