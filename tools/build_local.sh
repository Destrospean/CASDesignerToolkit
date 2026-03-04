#!/bin/bash
cd "${0%/*}"
docker build -t casdesignertoolkit ..
docker run --rm --name casdesignertoolkit -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/build_debian.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release && rm Newtonsoft.Json.xml OpenTK.xml s3pi*.xml System.Custom.xml TeximpNet.xml && /CASDesignerToolkit/tools/bundle_debian.sh"
