#!/bin/bash
cd "${0%/*}"
docker run --rm --name casdesignertoolkit-bundle-windows -v "$(pwd)/..":/CASDesignerToolkit $IMAGE_NAME bash -c "/CASDesignerToolkit/tools/_bundle_windows.sh"
