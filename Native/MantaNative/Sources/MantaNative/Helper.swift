//
//  Helper.swift
//  MantaNative
//
//  Created by ONEO on 2026/1/19.
//

import Foundation

// MARK: - Main Thread Helpers

func runOnMain<T>(_ block: @escaping () -> T) -> T {
    if Thread.isMainThread {
        return block()
    } else {
        return DispatchQueue.main.sync { block() }
    }
}

func runOnMainAsync(_ block: @escaping () -> Void) {
    if Thread.isMainThread {
        block()
    } else {
        DispatchQueue.main.async { block() }
    }
}
