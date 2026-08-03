#!/bin/bash
cd "${0%/*}"
docker build -t casdesignertoolkit ..
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_build.sh && cd /CASDesignerToolkit/CASDesignerToolkit/bin/Release/ && rm *.mdb *.pdb && cp /CASDesignerToolkit/GameFolders.xml ."
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_bundle_windows.sh"
docker run --rm --name casdesignertoolkit-build -v "$(pwd)/..":/CASDesignerToolkit casdesignertoolkit bash -c "/CASDesignerToolkit/tools/_bundle_debian.sh"
