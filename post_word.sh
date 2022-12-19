#! /bin/sh         


curl -ii  -X POST -H "Content-Type: text/plain"  -d "$1"  http://localhost:5072/st/$1
