#!/bin/bash
cd "${0%/*}"
docker build -t cas-designer-toolkit .
docker run -it --rm --name cas-designer-toolkit -v "$(pwd)/..":/CASDesignerToolkit cas-designer-toolkit /CASDesignerToolkit/tools/bundle_debian.sh
