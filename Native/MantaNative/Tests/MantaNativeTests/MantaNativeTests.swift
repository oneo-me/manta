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

