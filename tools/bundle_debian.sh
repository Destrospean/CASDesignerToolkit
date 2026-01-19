#!/bin/bash
cd "${0%/*}"
mkdir CASDesignerToolkit
cd ../CASDesignerToolkit/bin/Release
mkbundle --simple --static -o ../../../tools/CASDesignerToolkit/CASDesignerToolkit --nodeps CASDesignerToolkit.exe Destrospean.*.dll GDImageLibrary.dll GLWidget.dll Newtonsoft.Json.dll OpenTK.dll System.Custom.dll TeximpNet.dll /usr/lib/mono/4.5/System.dll /usr/lib/mono/4.5/System.Configuration.dll /usr/lib/mono/4.5/System.Xml.dll /usr/lib/mono/4.5/System.Security.dll /usr/lib/mono/4.5/System.Numerics.dll /usr/lib/mono/4.5/System.Core.dll /usr/lib/mono/4.5/Mono.Security.dll /usr/lib/mono/4.5/mscorlib.dll /usr/lib/cli/gtk-sharp-2.0/gtk-sharp.dll /usr/lib/cli/glib-sharp-2.0/glib-sharp.dll /usr/lib/cli/gdk-sharp-2.0/gdk-sharp.dll /usr/lib/mono/4.5/Mono.Cairo.dll /usr/lib/cli/pango-sharp-2.0/pango-sharp.dll /usr/lib/cli/atk-sharp-2.0/atk-sharp.dll /usr/lib/mono/4.5/System.Windows.Forms.dll /usr/lib/mono/4.5/System.Drawing.dll /usr/lib/mono/4.5/Mono.WebBrowser.dll /usr/lib/mono/4.5/System.Runtime.Serialization.Formatters.Soap.dll /usr/lib/mono/4.5/Accessibility.dll /usr/lib/mono/4.5/System.Data.dll /usr/lib/mono/4.5/System.Transactions.dll /usr/lib/mono/4.5/System.EnterpriseServices.dll /usr/lib/mono/4.5/System.Xml.Linq.dll /usr/lib/mono/4.5/System.Runtime.Serialization.dll /usr/lib/mono/4.5/System.ServiceModel.Internals.dll /usr/lib/mono/4.5/I18N.West.dll /usr/lib/mono/4.5/I18N.dll --config ../../../tools/config --machine-config /etc/mono/4.5/machine.config --library /usr/lib/libmono-native.so --library /usr/lib/cli/glib-sharp-2.0/libglibsharpglue-2.so --library /usr/lib/cli/gdk-sharp-2.0/libgdksharpglue-2.so --library /usr/lib/cli/gtk-sharp-2.0/libgtksharpglue-2.so --library /lib/x86_64-linux-gnu/libjpeg.so.62 --library /lib/x86_64-linux-gnu/libjbig.so.0 --library /lib/x86_64-linux-gnu/libLerc.so.4 --library /lib/x86_64-linux-gnu/libtiff.so.6 --library /usr/lib/libgdiplus.so.0 --library /usr/lib/libMonoPosixHelper.so
cd ../../../tools
cp ../CASDesignerToolkit/bin/Release/Acknowledgements.txt CASDesignerToolkit
cp ../CASDesignerToolkit/bin/Release/*License.txt CASDesignerToolkit
cp ../CASDesignerToolkit/bin/Release/GameFolders.xml CASDesignerToolkit
cp ../CASDesignerToolkit/bin/Release/LICENSE.md CASDesignerToolkit
cp ../CASDesignerToolkit/bin/Release/Mono.Posix.dll CASDesignerToolkit
cp ../CASDesignerToolkit/bin/Release/s3pi* CASDesignerToolkit
cp ../CASDesignerToolkit/Icons/CASDesignerToolkit.svg CASDesignerToolkit
cp start.sh CASDesignerToolkit
rar a CASDesignerToolkit-linux-amd64.rar CASDesignerToolkit/*
mv CASDesignerToolkit-*.rar ../CASDesignerToolkit/bin/Release
rm -rf CASDesignerToolkit
