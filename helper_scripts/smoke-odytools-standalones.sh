#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="Debug"
FRAMEWORK="net9.0"
DISPLAY_NAME="${DISPLAY:-:0}"
BUILD=false
CLEAN_STALE=false
TIMEOUT_STEPS=60
SLEEP_SECONDS=0.25
RESULTS_FILE=""
ALIAS_DIR=""

usage() {
  cat <<'USAGE'
Usage: helper_scripts/smoke-odytools-standalones.sh [options]

Launches each canonical OdyTools standalone editor on X11, waits for a visible
window, closes it, and fails if any editor exits or never shows a window.

Options:
  --build          Build all standalone editor projects before launching.
  --clean-stale    Remove old flat standalone output folders before launching.
  --configuration  Build/output configuration. Default: Debug.
  --framework      Target framework. Default: net9.0.
  --display        X display to use. Default: current DISPLAY or :0.
  --results-file   Write TSV results to this path. Default: temp file.
  --alias-dir      Directory for OdyTools.NET/OdyTools.<EDITOR> aliases. Default: temp dir.
  -h, --help       Show this help.
USAGE
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --build)
      BUILD=true
      shift
      ;;
    --clean-stale)
      CLEAN_STALE=true
      shift
      ;;
    --configuration)
      CONFIGURATION="${2:?missing value for --configuration}"
      shift 2
      ;;
    --framework)
      FRAMEWORK="${2:?missing value for --framework}"
      shift 2
      ;;
    --display)
      DISPLAY_NAME="${2:?missing value for --display}"
      shift 2
      ;;
    --results-file)
      RESULTS_FILE="${2:?missing value for --results-file}"
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

require_tool() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "Missing required tool: $1" >&2
    exit 2
  fi
}

require_tool xdotool
require_tool dotnet

if [[ -z "$DISPLAY_NAME" ]]; then
  echo "No X display configured. Set DISPLAY or pass --display." >&2
  exit 2
fi

XVFB_PID=""
cleanup_xvfb() {
  if [[ -n "$XVFB_PID" ]]; then
    kill "$XVFB_PID" 2>/dev/null || true
  fi
}
trap cleanup_xvfb EXIT

if ! DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
  require_tool Xvfb
  Xvfb "$DISPLAY_NAME" -screen 0 1280x800x24 >/tmp/odytools-standalone-smoke-xvfb.out 2>/tmp/odytools-standalone-smoke-xvfb.err &
  XVFB_PID=$!
  for _ in $(seq 1 40); do
    if DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
      break
    fi

    if ! kill -0 "$XVFB_PID" 2>/dev/null; then
      echo "Xvfb failed to start for display $DISPLAY_NAME" >&2
      cat /tmp/odytools-standalone-smoke-xvfb.err >&2 || true
      exit 1
    fi

    sleep 0.25
  done

  if ! DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
    echo "Timed out waiting for Xvfb display $DISPLAY_NAME" >&2
    cat /tmp/odytools-standalone-smoke-xvfb.err >&2 || true
    exit 1
  fi
fi

if [[ -z "$RESULTS_FILE" ]]; then
  RESULTS_FILE="$(mktemp /tmp/odytools-standalone-smoke.XXXXXX.tsv)"
fi
if [[ -z "$ALIAS_DIR" ]]; then
  ALIAS_DIR="$(mktemp -d /tmp/odytools-dotnet-aliases.XXXXXX)"
fi

cd "$ROOT_DIR"

if [[ "$BUILD" == true ]]; then
  echo "Building src/Tools/OdyTools/OdyTools.Standalone.csproj"
  dotnet build src/Tools/OdyTools/OdyTools.Standalone.csproj --configuration "$CONFIGURATION" --framework "$FRAMEWORK" -m:1 -p:UseSharedCompilation=false -v quiet

  while IFS= read -r csproj; do
    echo "Building $csproj"
    dotnet build "$csproj" --configuration "$CONFIGURATION" --framework "$FRAMEWORK" -m:1 -p:UseSharedCompilation=false -v quiet
  done < <(find src/Tools/OdyTools/Editors -maxdepth 2 -name 'OdyTool*.Standalone.csproj' | sort)
fi

if [[ "$CLEAN_STALE" == true ]]; then
  rm -rf \
    src/Tools/OdyTools/Editors/bin/"$CONFIGURATION" \
    src/Tools/OdyTools/Editors/DLG/bin/"$CONFIGURATION" \
    src/Tools/OdyTools/Editors/GUI/bin/"$CONFIGURATION"
fi

canonical_apphosts() {
  printf '%s\n' "src/Tools/OdyTools/Editors/bin/OdyTool2DA.Standalone/$CONFIGURATION/$FRAMEWORK/OdyTool2DA"
  find src/Tools/OdyTools/Editors/bin -path "*/$CONFIGURATION/$FRAMEWORK/OdyTool*.Standalone" -type f | sort
  printf '%s\n' "src/Tools/OdyTools/Editors/DLG/bin/OdyToolDLG.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolDLG.Standalone"
  printf '%s\n' "src/Tools/OdyTools/Editors/GUI/bin/OdyToolGUI.Standalone/$CONFIGURATION/$FRAMEWORK/OdyToolGUI.Standalone"
  printf '%s\n' "src/Tools/OdyTools/Editors/bin_fac/$CONFIGURATION/$FRAMEWORK/OdyToolFAC.Standalone"
}

canonical_launches() {
  while IFS= read -r exe; do
    [[ -n "$exe" ]] || continue
    printf '%s\t%s\t%s\n' "$(basename "$exe")" "$exe" "--theme light"
  done < <(canonical_apphosts | awk '!seen[$0]++')

  while IFS= read -r key; do
    [[ -n "$key" ]] || continue
    printf '%s\t%s\t%s\n' "OdyTools.Standalone($key)" "src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools.Standalone" "--editor $key --theme light"
  done < <(unified_launcher_keys)

  alias_launches
}

alias_launches() {
  printf '%s\t%s\t%s\n' "OdyTools.NET(alias)" "$ALIAS_DIR/OdyTools.NET" "--editor OdyTools.DLG --theme light"
  printf '%s\t%s\t%s\n' "odytools-net(alias)" "$ALIAS_DIR/odytools-net" "--editor dlg-editor --theme light"
  printf '%s\t%s\t%s\n' "holocron-toolset(alias)" "$ALIAS_DIR/holocron-toolset" "--editor twoda --theme light"
  printf '%s\t%s\t%s\n' "kotor-editor(alias)" "$ALIAS_DIR/kotor-editor" "--editor twoda --theme light"

  local direct_aliases=(
    OdyTools.2DA OdyTools.ARE OdyTools.BWM OdyTools.DLG OdyTools.ERF
    OdyTools.FAC OdyTools.GFF OdyTools.GIT OdyTools.GUI OdyTools.IFO OdyTools.JRL
    OdyTools.LIP OdyTools.LTR OdyTools.LYT OdyTools.MDL OdyTools.NSS OdyTools.PTH
    OdyTools.SAV OdyTools.SaveGame OdyTools.SSF OdyTools.TLK OdyTools.TPC OdyTools.TXT
    OdyTools.UTC OdyTools.UTD OdyTools.UTE OdyTools.UTI OdyTools.UTM OdyTools.UTP
    OdyTools.UTS OdyTools.UTT OdyTools.UTW OdyTools.WAV OdyTools.ModuleDesigner
    OdyTools.IndoorBuilder are-editor bwm-editor dlg-editor erf-editor fac-editor
    gff-editor git-editor gui-editor ifo-editor jrl-editor lip-editor ltr-editor lyt-editor mdl-editor nss-editor
    pth-editor savegame-editor ssf-editor tlk-editor tpc-editor txt-editor twoda-editor
    utc-editor utd-editor ute-editor uti-editor utm-editor utp-editor uts-editor utt-editor
    utw-editor wav-editor module-designer indoor-builder
  )

  local alias_name
  for alias_name in "${direct_aliases[@]}"; do
    printf '%s\t%s\t%s\n' "$alias_name(alias)" "$ALIAS_DIR/$alias_name" "--theme light"
  done
}

unified_launcher_keys() {
  local apphost="src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools.Standalone"
  if [[ ! -x "$apphost" ]]; then
    return
  fi

  "$ROOT_DIR/$apphost" --list | awk '
    /^Editors:/ { section = "editors"; next }
    /^Apps:/ { section = "apps"; next }
    NF == 0 { next }
    section == "editors" || section == "apps" {
      key = $1
      sub(/,$/, "", key)
      print key
    }
  ' | awk '!seen[$0]++'
}

check_unified_launcher_list() {
  local apphost="src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools.Standalone"
  local list_output

  if [[ ! -x "$apphost" ]]; then
    printf '%s\tmissing\t\t\n' "OdyTools.Standalone(--list)" >> "$RESULTS_FILE"
    failed=1
    return
  fi

  list_output="$("$ROOT_DIR/$apphost" --list)"
  if grep -Fq "erf              ERF/MOD/RIM/SAV/BIF/HAK .erf .mod .rim .sav .bif .hak" <<<"$list_output" \
    && grep -Fq "sav, savegame    Save Game Editor" <<<"$list_output" \
    && ! grep -Fq "sav, savegame    Save Game Editor        .sav" <<<"$list_output"; then
    printf '%s\tlisted\tHolocron-style .sav routing\t0\n' "OdyTools.Standalone(--list)" >> "$RESULTS_FILE"
  else
    printf '%s\tbad-list\t%s\t1\n' "OdyTools.Standalone(--list)" "$(tr '\n' ' ' <<<"$list_output")" >> "$RESULTS_FILE"
    failed=1
  fi
}

check_dotnet_aliases() {
  local alias_output
  alias_output="$(helper_scripts/link-odytools-dotnet-aliases.sh --configuration "$CONFIGURATION" --framework "$FRAMEWORK" --alias-dir "$ALIAS_DIR")"
  local required=(
    OdyTools.NET OdyTools.2DA OdyTools.ARE OdyTools.BWM OdyTools.DLG OdyTools.ERF
    OdyTools.FAC OdyTools.GFF OdyTools.GIT OdyTools.GUI OdyTools.IFO OdyTools.JRL
    OdyTools.LIP OdyTools.LTR OdyTools.LYT OdyTools.MDL OdyTools.NSS OdyTools.PTH
    OdyTools.SAV OdyTools.SaveGame OdyTools.SSF OdyTools.TLK OdyTools.TPC OdyTools.TXT
    OdyTools.UTC OdyTools.UTD OdyTools.UTE OdyTools.UTI OdyTools.UTM OdyTools.UTP
    OdyTools.UTS OdyTools.UTT OdyTools.UTW OdyTools.WAV OdyTools.ModuleDesigner
    OdyTools.IndoorBuilder odytools-net holocron-toolset kotor-editor are-editor bwm-editor dlg-editor erf-editor fac-editor
    gff-editor git-editor gui-editor ifo-editor jrl-editor lip-editor ltr-editor lyt-editor mdl-editor nss-editor
    pth-editor savegame-editor ssf-editor tlk-editor tpc-editor txt-editor twoda-editor
    utc-editor utd-editor ute-editor uti-editor utm-editor utp-editor uts-editor utt-editor
    utw-editor wav-editor module-designer indoor-builder
  )

  for alias_name in "${required[@]}"; do
    if [[ ! -x "$ALIAS_DIR/$alias_name" ]] || ! grep -Fxq "$alias_name" <<<"$alias_output"; then
      printf '%s\tmissing-alias\t%s\t1\n' "OdyTools.NET aliases" "$alias_name" >> "$RESULTS_FILE"
      failed=1
      return
    fi
  done

  printf '%s\taliases\t%s\t0\n' "OdyTools.NET aliases" "$ALIAS_DIR" >> "$RESULTS_FILE"
}

window_belongs_to_pid() {
  local window_id="$1"
  local root_pid="$2"
  local window_pid

  window_pid="$(DISPLAY="$DISPLAY_NAME" xdotool getwindowpid "$window_id" 2>/dev/null || true)"
  [[ -n "$window_pid" ]] || return 2

  while [[ -n "$window_pid" && "$window_pid" != "1" ]]; do
    [[ "$window_pid" == "$root_pid" ]] && return 0
    window_pid="$(ps -o ppid= -p "$window_pid" 2>/dev/null | tr -d '[:space:]' || true)"
  done

  return 1
}

window_matches_launch() {
  local window_id="$1"
  local root_pid="$2"

  # The harness launches one process at a time and snapshots pre-existing
  # windows before each launch. Avalonia/Xvfb can omit or misreport _NET_WM_PID,
  # so a newly-created titled window is the authoritative signal here.
  [[ -n "$window_id" && -n "$root_pid" ]]
}

window_ids() {
  DISPLAY="$DISPLAY_NAME" xdotool search --onlyvisible --class . 2>/dev/null || true
}

printf 'editor\tresult\twindow\texit_or_pid\n' > "$RESULTS_FILE"
failed=0

check_unified_launcher_list
check_dotnet_aliases

while IFS=$'\t' read -r name exe args; do
  [[ -n "$exe" ]] || continue
  launch_exe="$exe"
  if [[ "$launch_exe" != /* ]]; then
    launch_exe="$ROOT_DIR/$launch_exe"
  fi

  if [[ ! -x "$launch_exe" ]]; then
    printf '%s\tmissing\t\t\n' "$name" >> "$RESULTS_FILE"
    failed=1
    continue
  fi

  log_file="$(mktemp "/tmp/${name}.smoke.XXXXXX.log")"
  before_windows="$(window_ids | tr '\n' ' ')"

  # shellcheck disable=SC2086 # args are controlled by canonical_launches.
  DISPLAY="$DISPLAY_NAME" setsid "$launch_exe" $args > "$log_file" 2>&1 < /dev/null &
  pid=$!
  found_window=""
  found_title=""

  for _ in $(seq 1 "$TIMEOUT_STEPS"); do
    if ! kill -0 "$pid" 2>/dev/null; then
      break
    fi

    ids="$(window_ids)"
    for id in $ids; do
      case " $before_windows " in
        *" $id "*) continue ;;
      esac

      title="$(DISPLAY="$DISPLAY_NAME" xdotool getwindowname "$id" 2>/dev/null || true)"
      if [[ -n "$title" ]] && window_matches_launch "$id" "$pid"; then
        if [[ "$title" == "Window" ]]; then
          continue
        fi
        found_window="$id"
        found_title="$title"
        break 2
      fi
    done

    sleep "$SLEEP_SECONDS"
  done

  if [[ -n "$found_window" && "$found_title" != *"Editor failed to start"* ]]; then
    printf '%s\twindow\t%s\t%s\n' "$name" "$found_title" "$pid" >> "$RESULTS_FILE"
    DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
    sleep "$SLEEP_SECONDS"
    kill "$pid" 2>/dev/null || true
    wait "$pid" 2>/dev/null || true
  elif [[ -n "$found_window" ]]; then
    printf '%s\tstartup-error\t%s\t%s\n' "$name" "$found_title" "$pid" >> "$RESULTS_FILE"
    DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
    kill "$pid" 2>/dev/null || true
    wait "$pid" 2>/dev/null || true
    failed=1
  elif kill -0 "$pid" 2>/dev/null; then
    printf '%s\tno-window-running\t\t%s\n' "$name" "$pid" >> "$RESULTS_FILE"
    kill "$pid" 2>/dev/null || true
    wait "$pid" 2>/dev/null || true
    failed=1
  else
    wait "$pid" || exit_code=$?
    exit_code="${exit_code:-0}"
    first_line="$(sed -n '1,1p' "$log_file" | tr '\t' ' ')"
    printf '%s\texited\t%s\t%s\n' "$name" "$first_line" "$exit_code" >> "$RESULTS_FILE"
    failed=1
  fi

  rm -f "$log_file"
done < <(canonical_launches)

cat "$RESULTS_FILE"

if [[ "$failed" -ne 0 ]]; then
  echo "One or more standalone editors failed the smoke check. Results: $RESULTS_FILE" >&2
  exit 1
fi
