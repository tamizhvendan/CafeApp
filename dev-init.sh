#!/bin/bash
rm -rf .projekt
rm -rf .paket
mkdir .paket
url="https://github.com/fsprojects"
paketLoc=".paket/paket.bootstrapper.exe"
wget "$url/Paket/releases/download/2.51.4/paket.bootstrapper.exe" -O "$paketLoc"
wget "$url/Projekt/releases/download/0.0.4/Projekt.zip" -O temp.zip
unzip temp.zip -d .projekt
rm temp.zip