#!/bin/bash
if test "$OS" = "Windows_NT"
then # For Windows
    .paket/paket.bootstrapper.exe
    exit_code=$?
    if [ $exit_code -ne 0 ]; then
      exit $exit_code
    fi
    .paket/paket.exe restore
    exit_code=$?
    if [ $exit_code -ne 0 ]; then
    	exit $exit_code
    fi
    packages/FAKE/tools/FAKE.exe $@ --fsiargs build.fsx
else # For NonWindows
    mono .paket/paket.bootstrapper.exe
    exit_code=$?
    if [ $exit_code -ne 0 ]; then
      exit $exit_code
    fi
    mono .paket/paket.exe restore
    exit_code=$?
    if [ $exit_code -ne 0 ]; then
    	exit $exit_code
    fi
    # For travis-ci
    export NPM_FILE_PATH=$(which npm)
    mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi