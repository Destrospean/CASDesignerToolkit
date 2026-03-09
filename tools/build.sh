#!/bin/bash
cd "${0%/*}"
docker run --rm --name casdesignertoolkit -v "$(pwd)/..":/CASDesignerToolkit $IMAGE_NAME bash -c "/CASDesignerToolkit/tools/build_debian.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release && rm LibVLCSharp.xml Newtonsoft.Json.xml OpenTK.xml s3pi*.xml System.*.xml TeximpNet.xml && /CASDesignerToolkit/tools/bundle_debian.sh"
