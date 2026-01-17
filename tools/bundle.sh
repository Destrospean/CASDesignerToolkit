#!/bin/bash
cd "${0%/*}"
mkdir CASDesignerToolkit
export RELEASE_DIR=../CASDesignerToolkit/bin/Release
wine rcedit.exe $RELEASE_DIR/CASDesignerToolkit.exe --set-icon ../../Icons/CASDesignerToolkit.ico --set-version-string "FileDescription" "CAS Designer Toolkit"
wine rcedit.exe $RELEASE_DIR/Destrospean.CmarNYCBorrowed.dll --set-version-string "FileDescription" "CmarNYC's Code Repurposed"
wine rcedit.exe $RELEASE_DIR/Destrospean.Common.dll --set-version-string "FileDescription" "Destrospean's Shared Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.Graphics.OpenGL.dll --set-version-string "FileDescription" "Destrospean's OpenGL Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.S3PIExtensions.dll --set-version-string "FileDescription" "Destrospean's S3PI Extensions"
wine rcedit.exe $RELEASE_DIR/Destrospean.UI.GTKSharp.dll --set-version-string "FileDescription" "Destrospean's GTK# Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.zoeoeBorrowed.dll --set-version-string "FileDescription" "zoeoe's Object Geometry Code Repurposed"
rm $RELEASE_DIR/CASDesignerToolkit-*.rar
rm $RELEASE_DIR/CASDesignerToolkit-Self-Extractor.exe
cp ../CASDesignerToolkit/Icons/CASDesignerToolkit.svg CASDesignerToolkit
cp $RELEASE_DIR/* CASDesignerToolkit
rm CASDesignerToolkit/*.log
rar a -sfxwindows.sfx "CASDesignerToolkit-Self-Extractor.exe" CASDesignerToolkit/*
rar a CASDesignerToolkit-win32-i386.rar CASDesignerToolkit/*
mv CASDesignerToolkit-Self-Extractor.exe $RELEASE_DIR
mv CASDesignerToolkit-win32-i386.rar $RELEASE_DIR
rm -rf CASDesignerToolkit
distrobox enter debian-bookworm -- ./bundle_debian.sh
