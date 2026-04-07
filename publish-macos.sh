#!/bin/bash

set -euo pipefail

APP_NAME="Manta Demo"
PROJECT_DIR="$(cd "$(dirname "$0")" && pwd)"
DEMO_PROJECT="$PROJECT_DIR/MantaDemo/MantaDemo.csproj"
PLIST_TEMPLATE="$PROJECT_DIR/MantaDemo/Info.plist"
ENTITLEMENTS="$PROJECT_DIR/MantaDemo/Entitlements.plist"
OUTPUT_DIR="$PROJECT_DIR/publish"
PUBLISH_DIR="$OUTPUT_DIR/native"
APP_BUNDLE="$OUTPUT_DIR/$APP_NAME.app"

BLUE='\033[0;34m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

require_file() {
    local path="$1"
    local label="$2"

    if [ ! -f "$path" ]; then
        echo -e "${RED}Error: Missing ${label}: ${path}${NC}"
        exit 1
    fi
}

find_executable() {
    local directory="$1"
    local candidate

    candidate="$(find "$directory" -maxdepth 1 -type f -perm -111 ! -name '*.dylib' ! -name '*.dbg' ! -name '*.pdb' | head -n 1)"

    if [ -z "$candidate" ]; then
        echo -e "${RED}Error: Publish output does not contain a runnable executable.${NC}"
        exit 1
    fi

    printf '%s\n' "$candidate"
}

prune_publish_output() {
    local directory="$1"

    find "$directory" -maxdepth 1 \( -name '*.pdb' -o -name '*.dbg' \) -exec rm -f {} +
    find "$directory" -maxdepth 1 -name '*.dSYM' -exec rm -rf {} +
    rm -rf "$directory/runtimes"
}

echo -e "${BLUE}======================================${NC}"
echo -e "${BLUE}  Manta Demo - macOS Publisher${NC}"
echo -e "${BLUE}======================================${NC}"
echo ""

require_file "$DEMO_PROJECT" "project file"
require_file "$PLIST_TEMPLATE" "Info.plist"

echo -e "${YELLOW}[1/4] Cleaning previous publish output...${NC}"
rm -rf "$OUTPUT_DIR"
mkdir -p "$PUBLISH_DIR"

echo -e "${YELLOW}[2/4] Publishing app...${NC}"
dotnet publish "$DEMO_PROJECT" \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -o "$PUBLISH_DIR"

prune_publish_output "$PUBLISH_DIR"

EXECUTABLE_PATH="$(find_executable "$PUBLISH_DIR")"
EXECUTABLE_NAME="$(basename "$EXECUTABLE_PATH")"
VERSION="$(defaults read "$PLIST_TEMPLATE" CFBundleShortVersionString 2>/dev/null || echo "1.0.0")"
DMG_PATH="$OUTPUT_DIR/MantaDemo-$VERSION-macOS-arm64.dmg"

echo -e "${GREEN}      Publish successful: ${EXECUTABLE_NAME}${NC}"

echo -e "${YELLOW}[3/4] Building app bundle...${NC}"
mkdir -p "$APP_BUNDLE/Contents/MacOS"
cp -R "$PUBLISH_DIR"/. "$APP_BUNDLE/Contents/MacOS/"
cp "$PLIST_TEMPLATE" "$APP_BUNDLE/Contents/Info.plist"
echo -n "APPL????" > "$APP_BUNDLE/Contents/PkgInfo"

/usr/libexec/PlistBuddy -c "Set :CFBundleExecutable $EXECUTABLE_NAME" "$APP_BUNDLE/Contents/Info.plist"

if [ ! -f "$APP_BUNDLE/Contents/Resources/AppIcon.icns" ]; then
    /usr/libexec/PlistBuddy -c "Delete :CFBundleIconFile" "$APP_BUNDLE/Contents/Info.plist" >/dev/null 2>&1 || true
fi

if [ -n "${MACOS_CODESIGN_IDENTITY:-}" ]; then
    require_file "$ENTITLEMENTS" "Entitlements.plist"
    echo -e "      Signing with identity: ${MACOS_CODESIGN_IDENTITY}"
    codesign --force --deep --sign "$MACOS_CODESIGN_IDENTITY" \
        --entitlements "$ENTITLEMENTS" \
        "$APP_BUNDLE"
else
    echo -e "      Skipping codesign. Set ${BLUE}MACOS_CODESIGN_IDENTITY${NC} to enable signing."
fi

echo -e "${YELLOW}[4/4] Creating DMG...${NC}"
DMG_TEMP="$OUTPUT_DIR/dmg-temp"
mkdir -p "$DMG_TEMP"
cp -R "$APP_BUNDLE" "$DMG_TEMP/"
ln -s /Applications "$DMG_TEMP/Applications"

hdiutil create -volname "$APP_NAME" \
    -srcfolder "$DMG_TEMP" \
    -ov -format UDZO \
    "$DMG_PATH"

rm -rf "$DMG_TEMP"
rm -rf "$PUBLISH_DIR"

echo ""
echo -e "${GREEN}======================================${NC}"
echo -e "${GREEN}  Build Complete${NC}"
echo -e "${GREEN}======================================${NC}"
echo ""
echo -e "App Bundle: ${BLUE}$APP_BUNDLE${NC}"
echo -e "DMG File:   ${BLUE}$DMG_PATH${NC}"
