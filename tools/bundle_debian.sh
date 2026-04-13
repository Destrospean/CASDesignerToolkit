#!/bin/bash
cd "${0%/*}"
docker run --rm --name casdesignertoolkit-bundle-debian -v "$(pwd)/..":/CASDesignerToolkit $IMAGE_NAME bash -c "/CASDesignerToolkit/tools/_bundle_debian.sh"
