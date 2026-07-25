#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="Debug"
FRAMEWORK="net9.0"
DISPLAY_NAME="${DISPLAY:-:0}"
BUILD=false
RESULTS_FILE=""
TIMEOUT_STEPS=60
SLEEP_SECONDS=0.25

usage() {
  cat <<'USAGE'
Usage: helper_scripts/smoke-odytools-aio.sh [options]

Launches the combined OdyTools main window on X11, waits for a visible
"OdyTools" window, closes it, and fails if the app exits or shows a startup
error window.

Options:
  --build          Build OdyTools before launching.
  --configuration  Build/output configuration. Default: Debug.
  --framework      Target framework. Default: net9.0.
  --display        X display to use. Default: current DISPLAY or :0.
  --results-file   Write TSV results to this path. Default: temp file.
  -h, --help       Show this help.
USAGE
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --build)
      BUILD=true
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

require_tool dotnet
require_tool xdotool

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
  Xvfb "$DISPLAY_NAME" -screen 0 1280x800x24 >/tmp/odytools-aio-smoke-xvfb.out 2>/tmp/odytools-aio-smoke-xvfb.err &
  XVFB_PID=$!
  for _ in $(seq 1 40); do
    if DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
      break
    fi

    if ! kill -0 "$XVFB_PID" 2>/dev/null; then
      echo "Xvfb failed to start for display $DISPLAY_NAME" >&2
      cat /tmp/odytools-aio-smoke-xvfb.err >&2 || true
      exit 1
    fi

    sleep 0.25
  done

  if ! DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
    echo "Timed out waiting for Xvfb display $DISPLAY_NAME" >&2
    cat /tmp/odytools-aio-smoke-xvfb.err >&2 || true
    exit 1
  fi
fi

if [[ -z "$RESULTS_FILE" ]]; then
  RESULTS_FILE="$(mktemp /tmp/odytools-aio-smoke.XXXXXX.tsv)"
fi

cd "$ROOT_DIR"

if [[ "$BUILD" == true ]]; then
  dotnet build src/Tools/OdyTools/OdyTools.csproj --configuration "$CONFIGURATION" --framework "$FRAMEWORK" -m:1 -p:UseSharedCompilation=false -v quiet
fi

APPHOST="src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools"
if [[ ! -x "$APPHOST" ]]; then
  echo "Missing OdyTools apphost: $APPHOST" >&2
  exit 1
fi

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
  local status=0

  window_belongs_to_pid "$window_id" "$root_pid" || status=$?

  # Some Avalonia/Xvfb combinations do not expose _NET_WM_PID. The harness
  # snapshots pre-existing windows, so a newly-created titled window is still
  # authoritative for this single-launch smoke check.
  [[ "$status" -eq 0 || "$status" -eq 2 ]]
}

printf 'app\tresult\twindow\texit_or_pid\n' > "$RESULTS_FILE"
failed=0
log_file="$(mktemp /tmp/odytools-aio-smoke.XXXXXX.log)"
before_windows="$(DISPLAY="$DISPLAY_NAME" xdotool search --onlyvisible --class . 2>/dev/null | tr '\n' ' ' || true)"

DISPLAY="$DISPLAY_NAME" setsid "$ROOT_DIR/$APPHOST" --theme light > "$log_file" 2>&1 < /dev/null &
pid=$!
found_window=""
found_title=""

for _ in $(seq 1 "$TIMEOUT_STEPS"); do
  if ! kill -0 "$pid" 2>/dev/null; then
    break
  fi

  ids="$(DISPLAY="$DISPLAY_NAME" xdotool search --onlyvisible --class . 2>/dev/null || true)"
  for id in $ids; do
    case " $before_windows " in
      *" $id "*) continue ;;
    esac

    title="$(DISPLAY="$DISPLAY_NAME" xdotool getwindowname "$id" 2>/dev/null || true)"
    if [[ -n "$title" ]] && window_matches_launch "$id" "$pid"; then
      found_window="$id"
      found_title="$title"
      if [[ "$title" == "OdyTools" ]]; then
        break 2
      fi
    fi
  done

  sleep "$SLEEP_SECONDS"
done

if [[ -n "$found_window" && "$found_title" == "OdyTools" ]]; then
  printf '%s\twindow\t%s\t%s\n' "OdyTools" "$found_title" "$pid" >> "$RESULTS_FILE"
  DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
  sleep "$SLEEP_SECONDS"
  kill "$pid" 2>/dev/null || true
elif [[ -n "$found_window" ]]; then
  printf '%s\twrong-window\t%s\t%s\n' "OdyTools" "$found_title" "$pid" >> "$RESULTS_FILE"
  DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
  kill "$pid" 2>/dev/null || true
  failed=1
elif kill -0 "$pid" 2>/dev/null; then
  printf '%s\tno-window-running\t\t%s\n' "OdyTools" "$pid" >> "$RESULTS_FILE"
  kill "$pid" 2>/dev/null || true
  failed=1
else
  wait "$pid" || exit_code=$?
  exit_code="${exit_code:-0}"
  first_line="$(sed -n '1,1p' "$log_file" | tr '\t' ' ')"
  printf '%s\texited\t%s\t%s\n' "OdyTools" "$first_line" "$exit_code" >> "$RESULTS_FILE"
  failed=1
fi

rm -f "$log_file"
cat "$RESULTS_FILE"

if [[ "$failed" -ne 0 ]]; then
  echo "OdyTools AIO smoke check failed. Results: $RESULTS_FILE" >&2
  exit 1
fi
