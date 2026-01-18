#!/bin/bash

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
projectDir="$SCRIPT_DIR/MantaNative"

#echo "🧹 Cleaning build cache..."
#swift package --package-path "$projectDir" clean
#rm -rf "$projectDir/.build"

echo "🔨 Building the project..."
swift build -c release --package-path "$projectDir"

echo "✅ Build completed."

echo "📦 Copying dynamic libraries..."
cp -L "$projectDir/.build/release/libMantaNative.dylib" "$SCRIPT_DIR/libMantaNative.dylib"

echo "✅ Done."
ls -lh "$SCRIPT_DIR/libMantaNative.dylib"
