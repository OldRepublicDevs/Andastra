#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="Debug"
FRAMEWORK="net9.0"
ALIAS_DIR=""

usage() {
  cat <<'USAGE'
Usage: helper_scripts/link-odytools-dotnet-aliases.sh [options]

Creates a central alias directory with OdyTools.NET, OdyTools.<EDITOR>, and
Holocron-compatible *-editor launchers for the built standalone editor apphosts.

Options:
  --configuration  Build/output configuration. Default: Debug.
  --framework      Target framework. Default: net9.0.
  --alias-dir      Output directory for alias launchers. Default: OdyTools bin aliases.
  -h, --help       Show this help.
USAGE
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --configuration)
      CONFIGURATION="${2:?missing value for --configuration}"
      shift 2
      ;;
    --framework)
      FRAMEWORK="${2:?missing value for --framework}"
      shift 2
      ;;
    --alias-dir)
      ALIAS_DIR="${2:?missing value for --alias-dir}"
      shift 2
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown option: $1" >&2
      usage >&2
      exit 2
      ;;
  esac
done

if [[ -z "$ALIAS_DIR" ]]; then
  ALIAS_DIR="$ROOT_DIR/src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/aliases"
elif [[ "$ALIAS_DIR" != /* ]]; then
  ALIAS_DIR="$ROOT_DIR/$ALIAS_DIR"
fi

mkdir -p "$ALIAS_DIR"

write_wrapper() {
  local alias_name="$1"
  local target="$2"
  shift 2
  local args=("$@")
  local wrapper="$ALIAS_DIR/$alias_name"

  if [[ ! -x "$target" ]]; then
    echo "Missing executable target for $alias_name: $target" >&2
    return 1
  fi

  {
    printf '%s\n' '#!/usr/bin/env bash'
    printf '%s\n' 'set -euo pipefail'
    printf 'exec %q' "$target"
    for arg in "${args[@]}"; do
      printf ' %q' "$arg"
    done
    printf ' "$@"\n'
  } > "$wrapper"
  chmod +x "$wrapper"
}

unified="$ROOT_DIR/src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools.Standalone"
write_wrapper "OdyTools.NET" "$unified"
write_wrapper "odytools-net" "$unified"
write_wrapper "holocron-toolset" "$unified"
write_wrapper "kotor-editor" "$unified"
write_wrapper "OdyTools.ModuleDesigner" "$unified" --editor module-designer
write_wrapper "OdyTools.IndoorBuilder" "$unified" --editor indoor-builder

write_wrapper "OdyTools.2DA" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyTool2DA.Standalone/$CONFIGURATION/$FRAMEWORK/OdyTool2DA"
write_wrapper "twoda-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyTool2DA.Standalone/$CONFIGURATION/$FRAMEWORK/OdyTool2DA"
write_wrapper "OdyTools.ARE" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolARE.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolARE.Standalone"
write_wrapper "are-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolARE.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolARE.Standalone"
write_wrapper "OdyTools.BWM" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolBWM.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolBWM.Standalone"
write_wrapper "bwm-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolBWM.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolBWM.Standalone"
write_wrapper "OdyTools.DLG" "$ROOT_DIR/src/Tools/OdyTools/Editors/DLG/bin/OdyToolDLG.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolDLG.Standalone"
write_wrapper "dlg-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/DLG/bin/OdyToolDLG.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolDLG.Standalone"
write_wrapper "OdyTools.ERF" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolERF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolERF.Standalone"
write_wrapper "erf-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolERF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolERF.Standalone"
write_wrapper "OdyTools.FAC" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin_fac/$CONFIGURATION/$FRAMEWORK/OdyToolFAC.Standalone"
write_wrapper "fac-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin_fac/$CONFIGURATION/$FRAMEWORK/OdyToolFAC.Standalone"
write_wrapper "OdyTools.GFF" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolGFF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGFF.Standalone"
write_wrapper "gff-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolGFF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGFF.Standalone"
write_wrapper "OdyTools.GIT" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolGIT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGIT.Standalone"
write_wrapper "git-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolGIT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGIT.Standalone"
write_wrapper "OdyTools.GUI" "$ROOT_DIR/src/Tools/OdyTools/Editors/GUI/bin/OdyToolGUI.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGUI.Standalone"
write_wrapper "gui-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/GUI/bin/OdyToolGUI.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGUI.Standalone"
write_wrapper "OdyTools.IFO" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolIFO.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolIFO.Standalone"
write_wrapper "ifo-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolIFO.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolIFO.Standalone"
write_wrapper "OdyTools.JRL" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolJRL.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolJRL.Standalone"
write_wrapper "jrl-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolJRL.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolJRL.Standalone"
write_wrapper "OdyTools.LIP" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLIP.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLIP.Standalone"
write_wrapper "lip-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLIP.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLIP.Standalone"
write_wrapper "OdyTools.LTR" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLTR.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLTR.Standalone"
write_wrapper "ltr-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLTR.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLTR.Standalone"
write_wrapper "OdyTools.LYT" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLYT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLYT.Standalone"
write_wrapper "lyt-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolLYT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolLYT.Standalone"
write_wrapper "OdyTools.MDL" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolMDL.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolMDL.Standalone"
write_wrapper "mdl-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolMDL.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolMDL.Standalone"
write_wrapper "OdyTools.NSS" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolNSS.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolNSS.Standalone"
write_wrapper "nss-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolNSS.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolNSS.Standalone"
write_wrapper "OdyTools.PTH" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolPTH.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolPTH.Standalone"
write_wrapper "pth-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolPTH.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolPTH.Standalone"
write_wrapper "OdyTools.SAV" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolSAV.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolSAV.Standalone"
write_wrapper "OdyTools.SaveGame" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolSAV.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolSAV.Standalone"
write_wrapper "savegame-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolSAV.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolSAV.Standalone"
write_wrapper "OdyTools.SSF" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolSSF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolSSF.Standalone"
write_wrapper "ssf-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolSSF.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolSSF.Standalone"
write_wrapper "OdyTools.TLK" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTLK.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTLK.Standalone"
write_wrapper "tlk-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTLK.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTLK.Standalone"
write_wrapper "OdyTools.TPC" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTPC.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTPC.Standalone"
write_wrapper "tpc-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTPC.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTPC.Standalone"
write_wrapper "OdyTools.TXT" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTXT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTXT.Standalone"
write_wrapper "txt-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolTXT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolTXT.Standalone"
write_wrapper "OdyTools.UTC" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTC.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTC.Standalone"
write_wrapper "utc-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTC.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTC.Standalone"
write_wrapper "OdyTools.UTD" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTD.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTD.Standalone"
write_wrapper "utd-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTD.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTD.Standalone"
write_wrapper "OdyTools.UTE" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTE.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTE.Standalone"
write_wrapper "ute-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTE.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTE.Standalone"
write_wrapper "OdyTools.UTI" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTI.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTI.Standalone"
write_wrapper "uti-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTI.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTI.Standalone"
write_wrapper "OdyTools.UTM" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTM.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTM.Standalone"
write_wrapper "utm-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTM.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTM.Standalone"
write_wrapper "OdyTools.UTP" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTP.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTP.Standalone"
write_wrapper "utp-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTP.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTP.Standalone"
write_wrapper "OdyTools.UTS" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTS.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTS.Standalone"
write_wrapper "uts-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTS.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTS.Standalone"
write_wrapper "OdyTools.UTT" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTT.Standalone"
write_wrapper "utt-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTT.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTT.Standalone"
write_wrapper "OdyTools.UTW" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTW.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTW.Standalone"
write_wrapper "utw-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolUTW.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolUTW.Standalone"
write_wrapper "OdyTools.WAV" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolWAV.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolWAV.Standalone"
write_wrapper "wav-editor" "$ROOT_DIR/src/Tools/OdyTools/Editors/bin/OdyToolWAV.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolWAV.Standalone"
write_wrapper "module-designer" "$unified" --editor module-designer
write_wrapper "indoor-builder" "$unified" --editor indoor-builder

find "$ALIAS_DIR" -maxdepth 1 -type f -executable -printf '%f\n' | sort
