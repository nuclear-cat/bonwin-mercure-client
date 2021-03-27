#!/bin/bash
csc -platform:x86 src/*.cs -out:build/BonwinClient.exe && mono ./build/BonwinClient.exe