import Testing
@testable import MantaNative

@Test func testWebViewCreate() async throws {
    let handle = manta_webview_create()
    #expect(handle > 0)
    manta_webview_destroy(handle)
}

@Test func testWebViewGetNativeHandle() async throws {
    let handle = manta_webview_create()
    let nativeHandle = manta_webview_get_native_handle(handle)
    #expect(nativeHandle != nil)
    manta_webview_destroy(handle)
}

@Test func testWebViewCanGoBackForward() async throws {
    let handle = manta_webview_create()
    #expect(manta_webview_can_go_back(handle) == false)
    #expect(manta_webview_can_go_forward(handle) == false)
    manta_webview_destroy(handle)
}

@Test func testWebViewPageZoom() async throws {
    let handle = manta_webview_create()
    manta_webview_set_page_zoom(handle, 1.5)
    #expect(manta_webview_get_page_zoom(handle) == 1.5)
    manta_webview_destroy(handle)
}
