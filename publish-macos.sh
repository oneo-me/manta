#!/bin/bash

# Manta Demo - macOS ARM64 Publish Script
# Creates a NativeAOT compiled app bundle and DMG

set -e

# Configuration
APP_NAME="Manta Demo"
BUNDLE_NAME="MantaDemo"
BUNDLE_ID="com.mantademo"
VERSION="1.0.0"
PROJECT_DIR="$(cd "$(dirname "$0")" && pwd)"
DEMO_PROJECT="$PROJECT_DIR/MantaDemo/MantaDemo.csproj"
OUTPUT_DIR="$PROJECT_DIR/publish"
APP_BUNDLE="$OUTPUT_DIR/$APP_NAME.app"
DMG_NAME="MantaDemo-$VERSION-macOS-arm64"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}======================================${NC}"
echo -e "${BLUE}  Manta Demo - macOS ARM64 Publisher${NC}"
echo -e "${BLUE}======================================${NC}"
echo ""

# Clean previous builds
echo -e "${YELLOW}[1/5] Cleaning previous builds...${NC}"
rm -rf "$OUTPUT_DIR"
mkdir -p "$OUTPUT_DIR"

# Build with NativeAOT
echo -e "${YELLOW}[2/5] Building with NativeAOT for macOS ARM64...${NC}"
echo -e "      This may take a few minutes..."

dotnet publish "$DEMO_PROJECT" \
    -c Release \
    -r osx-arm64 \
    --self-contained true \
    -p:PublishAot=true \
    -p:PublishTrimmed=true \
    -p:TrimMode=full \
    -p:StripSymbols=true \
    -p:OptimizationPreference=Size \
    -p:IlcOptimizationPreference=Size \
    -p:IlcGenerateStackTraceData=false \
    -p:IlcFoldIdenticalMethodBodies=true \
    -p:IlcDisableReflection=false \
    -p:IlcTrimMetadata=true \
    -p:IlcGenerateMstatFile=false \
    -p:IlcGenerateDgmlFile=false \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeBinaryFormatterSerialization=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:EventSourceSupport=false \
    -p:HttpActivityPropagationSupport=false \
    -p:MetadataUpdaterSupport=false \
    -p:UseSystemResourceKeys=true \
    -p:JsonSerializerIsReflectionEnabledByDefault=false \
    -p:UseNativeHttpHandler=true \
    -p:StackTraceSupport=false \
    -o "$OUTPUT_DIR/native"

if [ ! -f "$OUTPUT_DIR/native/$BUNDLE_NAME" ]; then
    echo -e "${RED}Error: Build failed - executable not found${NC}"
    exit 1
fi

echo -e "${GREEN}      Build successful!${NC}"

# Create App Bundle structure
echo -e "${YELLOW}[3/5] Creating macOS App Bundle...${NC}"

mkdir -p "$APP_BUNDLE/Contents/MacOS"
mkdir -p "$APP_BUNDLE/Contents/Resources"

# Copy executable and libraries
cp "$OUTPUT_DIR/native/$BUNDLE_NAME" "$APP_BUNDLE/Contents/MacOS/"
chmod +x "$APP_BUNDLE/Contents/MacOS/$BUNDLE_NAME"

# Copy any native libraries (.dylib files)
if ls "$OUTPUT_DIR/native"/*.dylib 1> /dev/null 2>&1; then
    cp "$OUTPUT_DIR/native"/*.dylib "$APP_BUNDLE/Contents/MacOS/"
fi

# Copy Info.plist
cp "$PROJECT_DIR/MantaDemo/Info.plist" "$APP_BUNDLE/Contents/"

# Copy icon if exists
if [ -f "$PROJECT_DIR/MantaDemo/AppIcon.icns" ]; then
    cp "$PROJECT_DIR/MantaDemo/AppIcon.icns" "$APP_BUNDLE/Contents/Resources/"
    echo -e "      App icon copied"
else
    echo -e "${YELLOW}      Warning: No AppIcon.icns found, app will use default icon${NC}"
fi

# Create PkgInfo
echo -n "APPL????" > "$APP_BUNDLE/Contents/PkgInfo"

# Sign the app (ad-hoc signing for local use)
echo -e "${YELLOW}[4/5] Signing application (ad-hoc)...${NC}"
codesign --force --deep --sign - \
    --entitlements "$PROJECT_DIR/MantaDemo/Entitlements.plist" \
    "$APP_BUNDLE"

echo -e "${GREEN}      App bundle created: $APP_BUNDLE${NC}"

# Create DMG
echo -e "${YELLOW}[5/5] Creating DMG installer...${NC}"

DMG_TEMP="$OUTPUT_DIR/dmg_temp"
DMG_PATH="$OUTPUT_DIR/$DMG_NAME.dmg"

mkdir -p "$DMG_TEMP"
cp -R "$APP_BUNDLE" "$DMG_TEMP/"

# Create symbolic link to Applications folder
ln -s /Applications "$DMG_TEMP/Applications"

# Create DMG
hdiutil create -volname "$APP_NAME" \
    -srcfolder "$DMG_TEMP" \
    -ov -format UDZO \
    "$DMG_PATH"

# Cleanup
rm -rf "$DMG_TEMP"
rm -rf "$OUTPUT_DIR/native"

echo ""
echo -e "${GREEN}======================================${NC}"
echo -e "${GREEN}  Build Complete!${NC}"
echo -e "${GREEN}======================================${NC}"
echo ""
echo -e "App Bundle: ${BLUE}$APP_BUNDLE${NC}"
echo -e "DMG File:   ${BLUE}$DMG_PATH${NC}"
echo ""

# Show file sizes
APP_SIZE=$(du -sh "$APP_BUNDLE" | cut -f1)
DMG_SIZE=$(du -sh "$DMG_PATH" | cut -f1)
echo -e "App Size: $APP_SIZE"
echo -e "DMG Size: $DMG_SIZE"
echo ""
