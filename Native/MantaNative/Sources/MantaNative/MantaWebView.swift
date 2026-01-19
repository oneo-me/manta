// MantaWebView.swift
// Native WebView implementation for Manta.WebView library

import Foundation
import WebKit
import AppKit

// MARK: - WebView Manager

/// Manages WebView instances and provides a stable handle-based API
final class WebViewManager {
    static let shared = WebViewManager()

    private var webViews: [UnsafeMutableRawPointer: MantaWebView] = [:]
    private let lock = NSLock()

    private init() {}

    func create() -> UnsafeMutableRawPointer {
        lock.lock()
        defer { lock.unlock() }

        let webView = MantaWebView()
        let handle = webView.nativeHandle
        webViews[handle] = webView

        return handle
    }

    func destroy(handle: UnsafeMutableRawPointer) {
        lock.lock()
        defer { lock.unlock() }

        if let webView = webViews.removeValue(forKey: handle) {
            webView.cleanup()
        }
    }

    func get(handle: UnsafeMutableRawPointer) -> MantaWebView? {
        lock.lock()
        defer { lock.unlock() }
        return webViews[handle]
    }
}

// MARK: - MantaWebView

/// Custom WKWebView subclass that disables native context menu
final class NoMenuWebView: WKWebView {
    override func willOpenMenu(_ menu: NSMenu, with event: NSEvent) {
        // Don't call super to prevent context menu from showing
    }

    override func menu(for event: NSEvent) -> NSMenu? {
        return nil
    }

    override func rightMouseDown(with event: NSEvent) {
        // Don't call super to prevent right-click handling
    }
}

/// Wrapper around WKWebView with minimal configuration
final class MantaWebView {
    let webView: WKWebView

    init() {
        let configuration = WKWebViewConfiguration()
        configuration.preferences.javaScriptCanOpenWindowsAutomatically = true

        webView = NoMenuWebView(frame: NSRect(x: 0, y: 0, width: 100, height: 100), configuration: configuration)
        webView.wantsLayer = true
    }

    func cleanup() {
        webView.stopLoading()
        webView.removeFromSuperview()
    }

    func loadUrl(_ urlString: String) {
        guard let url = URL(string: urlString) else { return }
        webView.load(URLRequest(url: url))
    }

    func loadHtml(_ html: String, baseUrl: String?) {
        let base = baseUrl.flatMap { URL(string: $0) }
        webView.loadHTMLString(html, baseURL: base)
    }

    func setFrame(x: Double, y: Double, width: Double, height: Double) {
        webView.frame = NSRect(x: x, y: y, width: width, height: height)
    }

    var nativeHandle: UnsafeMutableRawPointer {
        Unmanaged.passUnretained(webView).toOpaque()
    }
}

// MARK: - C Exports

/// Creates a new WebView instance and returns its handle
@_cdecl("manta_webview_create")
public func manta_webview_create() -> UnsafeMutableRawPointer {
    return runOnMain {
        WebViewManager.shared.create()
    }
}

/// Destroys a WebView instance by its handle
@_cdecl("manta_webview_destroy")
public func manta_webview_destroy(_ handle: UnsafeMutableRawPointer) {
    runOnMainAsync {
        WebViewManager.shared.destroy(handle: handle)
    }
}

/// Loads a URL in the WebView
@_cdecl("manta_webview_load_url")
public func manta_webview_load_url(_ handle: UnsafeMutableRawPointer, _ url: UnsafePointer<CChar>?) {
    guard let url = url, let urlString = String(cString: url, encoding: .utf8) else { return }
    runOnMainAsync {
        WebViewManager.shared.get(handle: handle)?.loadUrl(urlString)
    }
}

/// Loads HTML content in the WebView
@_cdecl("manta_webview_load_html")
public func manta_webview_load_html(_ handle: UnsafeMutableRawPointer, _ html: UnsafePointer<CChar>?, _ baseUrl: UnsafePointer<CChar>?) {
    guard let html = html, let htmlString = String(cString: html, encoding: .utf8) else { return }
    let baseUrlString = baseUrl.flatMap { String(cString: $0, encoding: .utf8) }
    runOnMainAsync {
        WebViewManager.shared.get(handle: handle)?.loadHtml(htmlString, baseUrl: baseUrlString)
    }
}
