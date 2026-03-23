#!/bin/bash
cd "${0%/*}"
docker build -t casdesignertoolkit ..
docker run --rm --name casdesignertoolkit -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/build_debian.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release && rm *.pdb *.mdb LibVLCSharp.xml Newtonsoft.Json.xml NLog.xml OpenTK.xml s3pi*.xml System.*.xml TeximpNet.xml && /CASDesignerToolkit/tools/bundle_debian.sh"
