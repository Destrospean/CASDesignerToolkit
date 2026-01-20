#!/bin/bash
cd "${0%/*}/.."
mdtool build "-c:Release|x86" CASDesignerToolkit.sln
