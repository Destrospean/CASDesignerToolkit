#!/bin/bash
cd "${0%/*}"
mkdir CASDesignerToolkit
export RELEASE_DIR=../CASDesignerToolkit/bin/Release
wine rcedit.exe $RELEASE_DIR/CASDesignerToolkit.exe --set-icon ../CASDesignerToolkit/Icons/CASDesignerToolkit.ico --set-version-string "FileDescription" "CAS Designer Toolkit"
wine rcedit.exe $RELEASE_DIR/Destrospean.CmarNYCBorrowed.dll --set-version-string "FileDescription" "CmarNYC's Code Repurposed"
wine rcedit.exe $RELEASE_DIR/Destrospean.Common.dll --set-version-string "FileDescription" "Destrospean's Shared Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.Graphics.OpenGL.dll --set-version-string "FileDescription" "Destrospean's OpenGL Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.S3PIExtensions.dll --set-version-string "FileDescription" "Destrospean's S3PI Extensions"
wine rcedit.exe $RELEASE_DIR/Destrospean.UI.GTKSharp.dll --set-version-string "FileDescription" "Destrospean's GTK# Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.Updates.dll --set-version-string "FileDescription" "Destrospean's GitHub Update Code"
wine rcedit.exe $RELEASE_DIR/Destrospean.zoeoeBorrowed.dll --set-version-string "FileDescription" "zoeoe's Object Geometry Code Repurposed"
wine rcedit.exe $RELEASE_DIR/System.Destrospean.dll --set-version-string "FileDescription" "Destrospean's System-related Code"
wine 4gb_patch.exe $RELEASE_DIR/CASDesignerToolkit.exe
rm $RELEASE_DIR/CASDesignerToolkit.exe.Backup
rm -rf $RELEASE_DIR/dist
cp ../CASDesignerToolkit/Icons/CASDesignerToolkit.svg CASDesignerToolkit
cp $RELEASE_DIR/* CASDesignerToolkit
rm CASDesignerToolkit/*.log CASDesignerToolkit/*.sh CASDesignerToolkit/ealayer3 CASDesignerToolkit/noupdate
unix2dos CASDesignerToolkit/*.config CASDesignerToolkit/*.md CASDesignerToolkit/*.txt CASDesignerToolkit/*.xml
rar a -sfxwindows.sfx CASDesignerToolkit-win32-i386-Self-Extractor.exe CASDesignerToolkit/*
7z a CASDesignerToolkit-win32-i386.zip CASDesignerToolkit/*
mkdir $RELEASE_DIR/dist
mv CASDesignerToolkit-* $RELEASE_DIR/dist
rm -rf CASDesignerToolkit
