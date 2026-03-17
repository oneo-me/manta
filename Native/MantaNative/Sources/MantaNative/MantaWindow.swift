import AppKit

@_cdecl("manta_window_configure_window")
public func manta_window_configure_window(_ windowHandle: UnsafeMutableRawPointer) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
        
        guard let contentView = window.contentView else { return }
        
        window.isOpaque = false
        window.backgroundColor = .clear
        
        removeVisualEffectViews(from: contentView)
        
        let effectView = NSVisualEffectView(frame: contentView.bounds)
        effectView.state = .active
        effectView.wantsLayer = true
        effectView.autoresizingMask = [.width, .height]
        effectView.material = .popover
        effectView.blendingMode = .behindWindow
        // effectView.layer?.cornerRadius = 12
        // effectView.layer?.cornerCurve = .continuous

        contentView.addSubview(effectView, positioned: .below, relativeTo: nil)
    }
}

private func removeVisualEffectViews(from view: NSView?) {
    guard let view = view else { return }
    let subviews = view.subviews.filter { $0 is NSVisualEffectView }
    subviews.forEach { $0.removeFromSuperview() }
}
