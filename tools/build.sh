#!/bin/bash
cd "${0%/*}"
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit $IMAGE_NAME bash -c "/CASDesignerToolkit/tools/_build.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release/ && rm *.pdb *.xml && cp /CASDesignerToolkit/GameFolders.xml ."
