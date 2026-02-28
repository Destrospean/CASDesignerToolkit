#!/bin/bash
cd "${0%/*}"
docker build -t cas-designer-toolkit .
docker run --rm --name cas-designer-toolkit -v "$(pwd)/..":/CASDesignerToolkit cas-designer-toolkit bash -c "/CASDesignerToolkit/tools/build_debian.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release && rm Newtonsoft.Json.xml OpenTK.xml s3pi*.xml System.Custom.xml TeximpNet.xml && /CASDesignerToolkit/tools/bundle_debian.sh"
