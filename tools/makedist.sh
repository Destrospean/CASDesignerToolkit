#!/bin/bash
cd "${0%/*}"
docker build -t casdesignertoolkit ..
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_build.sh"
rm ../CASDesignerToolkit/bin/Release/*.mdb ../CASDesignerToolkit/bin/Release/*.pdb ../CASDesignerToolkit/bin/Release/*.xml
cp ../GameFolders.xml ../CASDesignerToolkit/bin/Release
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_bundle_windows.sh"
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_bundle_debian.sh"
