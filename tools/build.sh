#!/bin/bash
cd "${0%/*}"
docker build -t cas-designer-toolkit .
docker run -it --rm --name cas-designer-toolkit -v "$(pwd)/..":/CASDesignerToolkit cas-designer-toolkit bash -c "/CASDesignerToolkit/tools/build_debian.sh && /CASDesignerToolkit/tools/bundle_debian.sh"
