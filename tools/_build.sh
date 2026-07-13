#!/bin/bash
cd "${0%/*}/.."
sed -i 's#<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>#<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>#' CASDesignerToolkit/CASDesignerToolkit.csproj
sed -i 's#<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>#<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>#' Destrospean.UI.GTKSharp/Destrospean.UI.GTKSharp.csproj
mdtool build "-c:Release|x86" CASDesignerToolkit.sln
sed -i 's#<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>#<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>#' CASDesignerToolkit/CASDesignerToolkit.csproj
sed -i 's#<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>#<TargetFrameworkVersion>v4.5</TargetFrameworkVersion>#' Destrospean.UI.GTKSharp/Destrospean.UI.GTKSharp.csproj
