#!/bin/bash

# Usage: ./delete_wav_files.sh [dry-run]

DRY_RUN=false
if [ "$1" == "dry-run" ]; then
  DRY_RUN=true
fi

find_files() {
  find . -type f \( -iname "*.wav" -o -iname "*.wav.meta" \)
}

if $DRY_RUN; then
  echo "Dry run mode. The following files would be deleted:"
  find_files
else
  echo "Deleting files..."
  find_files | while read -r file; do
    rm -v "$file"
  done
  echo "Deletion complete."
fi