//
//  Helper.swift
//  MantaNative
//
//  Created by ONEO on 2026/1/19.
//

import Foundation

// MARK: - Main Thread Helpers

func runOnMainAsync(_ block: @escaping () -> Void) {
    if Thread.isMainThread {
        block()
    } else {
        DispatchQueue.main.async { block() }
    }
}
