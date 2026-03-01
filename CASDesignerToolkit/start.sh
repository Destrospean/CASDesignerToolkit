#!/bin/bash
cd "${0%/*}"
export MONO_REGISTRY_PATH=registry LIBGL_ALWAYS_SOFTWARE=true GALLIUM_DRIVER=llvmpipe GTK_PATH=$HOME/.steam/steam/ubuntu12_32/steam-runtime/amd64/usr/lib/x86_64-linux-gnu/gtk-2.0 LD_PRELOAD=$HOME/.steam/steam/ubuntu12_32/steam-runtime/amd64/usr/lib/x86_64-linux-gnu/libgtk-x11-2.0.so.0:$HOME/.steam/steam/ubuntu12_32/steam-runtime/amd64/usr/lib/x86_64-linux-gnu/libgdk-x11-2.0.so.0 LD_LIBRARY_PATH=.:$LD_LIBRARY_PATH
if [ ! -d $MONO_REGISTRY_PATH ]; then mkdir $MONO_REGISTRY_PATH; fi
if [ "$#" -gt 0 ]; then ./CASDesignerToolkit "$1"; else ./CASDesignerToolkit; fi
