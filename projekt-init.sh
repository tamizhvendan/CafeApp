#!/bin/bash
rm -rf .projekt
url="https://github.com/fsprojects"
wget "$url/Projekt/releases/download/0.0.4/Projekt.zip" -O temp.zip
unzip temp.zip -d .projekt
rm temp.zip