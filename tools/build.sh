#!/bin/bash
cd "${0%/*}"
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit $IMAGE_NAME bash -c "/CASDesignerToolkit/tools/_build.sh"
rm ../CASDesignerToolkit/bin/Release/*.pdb ../CASDesignerToolkit/bin/Release/*.xml
cp ../GameFolders.xml ../CASDesignerToolkit/bin/Release
