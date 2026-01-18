// swift-tools-version: 6.0
// The swift-tools-version declares the minimum version of Swift required to build this package.

import PackageDescription

let package = Package(
    name: "MantaNative",
    platforms: [.macOS(.v11)],
    products: [
        .library(
            name: "MantaNative",
            type: .dynamic,
            targets: ["MantaNative"]
        ),
    ],
    targets: [
        .target(
            name: "MantaNative",
            dependencies: [],
            swiftSettings: [
                .swiftLanguageMode(.v5)
            ],
            linkerSettings: [
                .linkedFramework("WebKit"),
                .linkedFramework("AppKit"),
                .linkedFramework("Foundation"),
            ]
        ),
        .testTarget(
            name: "MantaNativeTests",
            dependencies: ["MantaNative"],
            swiftSettings: [
                .swiftLanguageMode(.v5)
            ]
        ),
    ]
)
