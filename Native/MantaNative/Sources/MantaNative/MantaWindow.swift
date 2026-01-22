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



@_cdecl("manta_window_configure_tray_window")
public func manta_window_configure_tray_window(_ windowHandle: UnsafeMutableRawPointer) {
    runOnMainAsync {
        let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()

        window.level = .popUpMenu
        
        guard let contentView = window.contentView else { return }
        
        window.isOpaque = false
        window.backgroundColor = .clear
        
        removeVisualEffectViews(from: contentView)
        
        if #available(macOS 26.0, *) {
            let effectView = NSGlassEffectView(frame: contentView.bounds)
            effectView.wantsLayer = true
            effectView.autoresizingMask = [.width, .height]
            // effectView.layer?.cornerRadius = 12
            // effectView.layer?.cornerCurve = .continuous

            contentView.addSubview(effectView, positioned: .below, relativeTo: nil)
        } else {
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
}

private func removeVisualEffectViews(from view: NSView?) {
    guard let view = view else { return }
    let subviews = view.subviews.filter { $0 is NSVisualEffectView }
    subviews.forEach { $0.removeFromSuperview() }
}

private class ObserverHolder: @unchecked Sendable {
    var observer: NSObjectProtocol?
}

@_cdecl("manta_window_begin_sheet")
public func manta_window_begin_sheet(
    _ ownerHandle: UnsafeMutableRawPointer,
    _ sheetHandle: UnsafeMutableRawPointer,
    _ callback: @escaping @convention(c) (UnsafeMutableRawPointer) -> Void,
    _ state: UnsafeMutableRawPointer
) {
    runOnMainAsync {
        let owner = Unmanaged<NSWindow>.fromOpaque(ownerHandle).takeUnretainedValue()
        let sheet = Unmanaged<NSWindow>.fromOpaque(sheetHandle).takeUnretainedValue()

        // 监听 sheet 窗口关闭事件
        let holder = ObserverHolder()
        holder.observer = NotificationCenter.default.addObserver(
            forName: NSWindow.willCloseNotification,
            object: sheet,
            queue: .main
        ) { _ in
            if let obs = holder.observer {
                NotificationCenter.default.removeObserver(obs)
                holder.observer = nil
            }
            // 确保调用 endSheet 来触发 completionHandler
            owner.endSheet(sheet)
        }

        owner.beginSheet(sheet) { _ in
            callback(state)
        }
    }
}
