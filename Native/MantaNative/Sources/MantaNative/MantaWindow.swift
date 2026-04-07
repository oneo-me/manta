import AppKit
import ObjectiveC

  private let mantaEffectIdentifier = NSUserInterfaceItemIdentifier("manta.window.effect")
  private var mantaObserverKey: UInt8 = 0

  private enum MantaWindowStyle {
      static let cornerRadius: CGFloat = 16
      static let trafficLightLeft: CGFloat = 11
      static let trafficLightTop: CGFloat = 10
      static let trafficLightSpacing: CGFloat = 6
      static let trafficLightScale: CGFloat = 1.15
      static let material: NSVisualEffectView.Material = .popover
  }

  @_cdecl("manta_window_configure_window")
  public func manta_window_configure_window(_ windowHandle: UnsafeMutableRawPointer) {
      runOnMainAsync {
          let window = Unmanaged<NSWindow>.fromOpaque(windowHandle).takeUnretainedValue()
          configureWindow(window)
      }
  }

  private func configureWindow(_ window: NSWindow) {
      guard let contentView = window.contentView else { return }

      window.isOpaque = false
      window.backgroundColor = .clear
      window.hasShadow = true
      window.titleVisibility = .hidden
      window.titlebarAppearsTransparent = true
      window.styleMask.insert(.fullSizeContentView)

      installBackgroundEffect(in: contentView)
      installWindowObserverIfNeeded(window)
      refreshWindowChrome(window)

      DispatchQueue.main.async {
          refreshWindowChrome(window)
      }
  }

  private func installBackgroundEffect(in contentView: NSView) {
      removeMantaEffectView(from: contentView)

      let effectView = NSVisualEffectView(frame: contentView.bounds)
      effectView.identifier = mantaEffectIdentifier
      effectView.state = .active
      effectView.wantsLayer = true
      effectView.autoresizingMask = [.width, .height]
      effectView.material = MantaWindowStyle.material
      effectView.blendingMode = .behindWindow

      if let firstSubview = contentView.subviews.first {
          contentView.addSubview(effectView, positioned: .below, relativeTo: firstSubview)
      } else {
          contentView.addSubview(effectView)
      }
  }

  private func removeMantaEffectView(from view: NSView) {
      for subview in view.subviews where subview.identifier == mantaEffectIdentifier {
          subview.removeFromSuperview()
      }
  }

  private func mantaNearlyEqual(_ lhs: CGFloat, _ rhs: CGFloat, epsilon: CGFloat = 0.25) -> Bool {
      abs(lhs - rhs) <= epsilon
  }

  private func refreshWindowChrome(_ window: NSWindow) {
      NSAnimationContext.runAnimationGroup { context in
          context.duration = 0
          context.allowsImplicitAnimation = false

          CATransaction.begin()
          CATransaction.setDisableActions(true)
          applyCornerRadius(window)
          applyTrafficLightLayout(window)
          CATransaction.commit()
      }
  }

  private func isFullScreen(_ window: NSWindow) -> Bool {
      window.styleMask.contains(.fullScreen)
  }

  private func applyCornerRadius(_ window: NSWindow) {
      guard let frameView = window.contentView?.superview else { return }

      frameView.wantsLayer = true
      guard let layer = frameView.layer else { return }

      if isFullScreen(window) {
          if !mantaNearlyEqual(layer.cornerRadius, 0) {
              layer.cornerRadius = 0
          }
          if layer.masksToBounds {
              layer.masksToBounds = false
          }
      } else {
          if !mantaNearlyEqual(layer.cornerRadius, MantaWindowStyle.cornerRadius) {
              layer.cornerRadius = MantaWindowStyle.cornerRadius
          }
          if !layer.masksToBounds {
              layer.masksToBounds = true
          }
      }
  }

  private func applyTrafficLightLayout(_ window: NSWindow) {
      guard
          let closeButton = window.standardWindowButton(.closeButton),
          let miniButton = window.standardWindowButton(.miniaturizeButton),
          let zoomButton = window.standardWindowButton(.zoomButton),
          let buttonContainer = closeButton.superview
      else {
          return
      }

      let buttons = [closeButton, miniButton, zoomButton]

      if isFullScreen(window) {
          for button in buttons {
              button.wantsLayer = true
              button.layer?.anchorPoint = CGPoint(x: 0, y: 0)
              if let layer = button.layer {
                  let transform = layer.affineTransform()
                  if !transform.isIdentity {
                      layer.setAffineTransform(.identity)
                  }
              }
          }
          return
      }

      let scale = MantaWindowStyle.trafficLightScale
      let baseSize = closeButton.frame.size
      let visualWidth = baseSize.width * scale
      let visualHeight = baseSize.height * scale
      let y = buttonContainer.bounds.height - MantaWindowStyle.trafficLightTop - visualHeight

      for button in buttons {
          button.wantsLayer = true
          if let layer = button.layer {
              let targetAnchorPoint = CGPoint(x: 0, y: 0)
              if layer.anchorPoint != targetAnchorPoint {
                  layer.anchorPoint = targetAnchorPoint
              }

              let targetTransform = CGAffineTransform(scaleX: scale, y: scale)
              if layer.affineTransform() != targetTransform {
                  layer.setAffineTransform(targetTransform)
              }
          }
      }

      updateFrameOrigin(
          of: closeButton,
          to: NSPoint(
              x: MantaWindowStyle.trafficLightLeft,
              y: y
          )
      )

      updateFrameOrigin(
          of: miniButton,
          to: NSPoint(
              x: MantaWindowStyle.trafficLightLeft + visualWidth + MantaWindowStyle.trafficLightSpacing,
              y: y
          )
      )

      updateFrameOrigin(
          of: zoomButton,
          to: NSPoint(
              x: MantaWindowStyle.trafficLightLeft + (visualWidth + MantaWindowStyle.trafficLightSpacing) * 2,
              y: y
          )
      )
  }

  private func updateFrameOrigin(of view: NSView, to point: NSPoint) {
      if !mantaNearlyEqual(view.frame.origin.x, point.x) || !mantaNearlyEqual(view.frame.origin.y, point.y) {
          view.setFrameOrigin(point)
      }
  }

  private func installWindowObserverIfNeeded(_ window: NSWindow) {
      if objc_getAssociatedObject(window, &mantaObserverKey) != nil {
          return
      }

      let observer = MantaWindowObserver(window: window)
      objc_setAssociatedObject(window, &mantaObserverKey, observer, .OBJC_ASSOCIATION_RETAIN_NONATOMIC)
  }

  private final class MantaWindowObserver: NSObject {
      weak var window: NSWindow?
      private var tokens: [NSObjectProtocol] = []

      init(window: NSWindow) {
          self.window = window
          super.init()

          let center = NotificationCenter.default

          tokens.append(
              center.addObserver(
                  forName: NSWindow.didResizeNotification,
                  object: window,
                  queue: .main
              ) { [weak self] _ in
                  self?.refresh()
                  self?.refreshDeferred()
              }
          )

          tokens.append(
              center.addObserver(
                  forName: NSWindow.didEndLiveResizeNotification,
                  object: window,
                  queue: .main
              ) { [weak self] _ in
                  self?.refresh()
                  self?.refreshDeferred()
              }
          )

          tokens.append(
              center.addObserver(
                  forName: NSWindow.didEnterFullScreenNotification,
                  object: window,
                  queue: .main
              ) { [weak self] _ in
                  self?.refresh()
                  self?.refreshDeferred()
              }
          )

          tokens.append(
              center.addObserver(
                  forName: NSWindow.didExitFullScreenNotification,
                  object: window,
                  queue: .main
              ) { [weak self] _ in
                  self?.refresh()
                  self?.refreshDeferred()
              }
          )

          tokens.append(
              center.addObserver(
                  forName: NSWindow.didChangeScreenNotification,
                  object: window,
                  queue: .main
              ) { [weak self] _ in
                  self?.refresh()
                  self?.refreshDeferred()
              }
          )
      }

      deinit {
          let center = NotificationCenter.default
          for token in tokens {
              center.removeObserver(token)
          }
      }

      private func refresh() {
          guard let window else { return }
          refreshWindowChrome(window)
      }

      private func refreshDeferred() {
          DispatchQueue.main.async { [weak self] in
              self?.refresh()
          }
      }
  }
