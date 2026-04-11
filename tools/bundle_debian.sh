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
cp -r ../libs/Windows/* CASDesignerToolkit
rm CASDesignerToolkit/*.log CASDesignerToolkit/*.sh CASDesignerToolkit/noupdate
unix2dos CASDesignerToolkit/*.config CASDesignerToolkit/*.md CASDesignerToolkit/*.txt CASDesignerToolkit/*.xml
rar a -sfxwindows.sfx CASDesignerToolkit-win32-i386-Self-Extractor.exe CASDesignerToolkit/*
7z a CASDesignerToolkit-win32-i386.zip CASDesignerToolkit/*
mkdir $RELEASE_DIR/dist
mv CASDesignerToolkit-* $RELEASE_DIR/dist
rm -rf CASDesignerToolkit/*
cd $RELEASE_DIR
mkbundle --simple --static -o ../../../tools/CASDesignerToolkit/CASDesignerToolkit --nodeps CASDesignerToolkit.exe BouncyCastle.Crypto.dll Destrospean.*.dll GDImageLibrary.dll GLWidget.dll LibVLCSharp.dll Mono.*.dll Newtonsoft.Json.dll OpenTK.dll System.Custom.dll System.Destrospean.dll TeximpNet.dll /usr/lib/mono/4.5/System.dll /usr/lib/mono/4.5/System.Configuration.dll /usr/lib/mono/4.5/System.Xml.dll /usr/lib/mono/4.5/System.Security.dll /usr/lib/mono/4.5/System.Numerics.dll /usr/lib/mono/4.5/System.Core.dll /usr/lib/mono/4.5/Mono.Security.dll /usr/lib/mono/4.5/mscorlib.dll /usr/lib/cli/gtk-sharp-2.0/gtk-sharp.dll /usr/lib/cli/glib-sharp-2.0/glib-sharp.dll /usr/lib/cli/gdk-sharp-2.0/gdk-sharp.dll /usr/lib/cli/pango-sharp-2.0/pango-sharp.dll /usr/lib/cli/atk-sharp-2.0/atk-sharp.dll /usr/lib/mono/4.5/System.Windows.Forms.dll /usr/lib/mono/4.5/System.Drawing.dll /usr/lib/mono/4.5/Mono.WebBrowser.dll /usr/lib/mono/4.5/System.Runtime.Serialization.Formatters.Soap.dll /usr/lib/mono/4.5/Accessibility.dll /usr/lib/mono/4.5/System.Data.dll /usr/lib/mono/4.5/System.Transactions.dll /usr/lib/mono/4.5/System.EnterpriseServices.dll /usr/lib/mono/4.5/System.Xml.Linq.dll /usr/lib/mono/4.5/System.Runtime.Serialization.dll /usr/lib/mono/4.5/System.ServiceModel.Internals.dll /usr/lib/mono/4.5/I18N.West.dll /usr/lib/mono/4.5/I18N.dll --config ../../../config --machine-config /etc/mono/4.5/machine.config --library /usr/lib/libmono-native.so --library /usr/lib/cli/glib-sharp-2.0/libglibsharpglue-2.so --library /usr/lib/cli/gdk-sharp-2.0/libgdksharpglue-2.so --library /usr/lib/cli/gtk-sharp-2.0/libgtksharpglue-2.so --library /lib/x86_64-linux-gnu/libjpeg.so.62 --library /lib/x86_64-linux-gnu/libjbig.so.0 --library /lib/x86_64-linux-gnu/libLerc.so.4 --library /lib/x86_64-linux-gnu/libtiff.so.6 --library /usr/lib/libgdiplus.so.0 --library /usr/lib/libMonoPosixHelper.so
cd ../../../tools
cp ../libs/Linux/* CASDesignerToolkit
cp $RELEASE_DIR/Acknowledgements.txt CASDesignerToolkit
cp $RELEASE_DIR/GameFolders.xml CASDesignerToolkit
cp $RELEASE_DIR/LICENSE.md CASDesignerToolkit
cp $RELEASE_DIR/*License.txt CASDesignerToolkit
cp $RELEASE_DIR/s3pi* CASDesignerToolkit
cp $RELEASE_DIR/*.sh CASDesignerToolkit
cp ../CASDesignerToolkit/Icons/CASDesignerToolkit.svg CASDesignerToolkit
rar a -sfxlinux.sfx CASDesignerToolkit-linux-amd64-Self-Extractor CASDesignerToolkit/*
7z a CASDesignerToolkit-linux-amd64.zip CASDesignerToolkit/*
mv CASDesignerToolkit-* $RELEASE_DIR/dist
rm -rf CASDesignerToolkit
