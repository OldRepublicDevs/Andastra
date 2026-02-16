#!/bin/sh
# For filter-branch index-filter: make every ScriptDefs.cs and ScriptLib.cs
# use the same blob as src/BioWare/Common/Script/ (canonical). Reduces repo
# size by deduplicating ~1.3MB x 2 files x N duplicate paths across history.
# Uses GIT_COMMIT (commit being rewritten), not HEAD.
set -e
CANON_DEFS=$(git ls-tree "$GIT_COMMIT" "src/BioWare/Common/Script/ScriptDefs.cs" 2>/dev/null | awk '{print $3}')
CANON_LIB=$(git ls-tree "$GIT_COMMIT" "src/BioWare/Common/Script/ScriptLib.cs" 2>/dev/null | awk '{print $3}')

# If no canonical in this commit, nothing to do
[ -z "$CANON_DEFS" ] && [ -z "$CANON_LIB" ] && exit 0

for path in \
  "src/Andastra/Parsing/Common/Script/ScriptDefs.cs" \
  "src/BioWare.NET/Common/Script/ScriptDefs.cs" \
  "src/BioWareCSharp/Common/Script/ScriptDefs.cs" \
  "src/CSharpKOTOR/Common/Script/ScriptDefs.cs" \
  "src/TSLPatcher.Core/Common/Script/ScriptDefs.cs"; do
  if [ -n "$CANON_DEFS" ] && git ls-tree "$GIT_COMMIT" "$path" >/dev/null 2>&1; then
    git rm --cached --ignore-unmatch "$path" 2>/dev/null || true
    git update-index --add --cacheinfo 100644,"$CANON_DEFS","$path"
  fi
done

for path in \
  "src/Andastra/Parsing/Common/Script/ScriptLib.cs" \
  "src/BioWare.NET/Common/Script/ScriptLib.cs" \
  "src/BioWareCSharp/Common/Script/ScriptLib.cs" \
  "src/CSharpKOTOR/Common/Script/ScriptLib.cs" \
  "src/TSLPatcher.Core/Common/Script/ScriptLib.cs"; do
  if [ -n "$CANON_LIB" ] && git ls-tree "$GIT_COMMIT" "$path" >/dev/null 2>&1; then
    git rm --cached --ignore-unmatch "$path" 2>/dev/null || true
    git update-index --add --cacheinfo 100644,"$CANON_LIB","$path"
  fi
done
