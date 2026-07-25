#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="Debug"
FRAMEWORK="net9.0"
DISPLAY_NAME="${DISPLAY:-:0}"
BUILD=false
RESULTS_FILE=""
TIMEOUT_STEPS=80
SLEEP_SECONDS=0.25

usage() {
  cat <<'USAGE'
Usage: helper_scripts/smoke-odytools-routed-open.sh [options]

Launches OdyTools.Standalone with --open for representative resource files,
waits for the routed editor window, and fails on missing windows or startup
error windows.

Options:
  --build          Build OdyTools.Standalone before launching.
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
  Xvfb "$DISPLAY_NAME" -screen 0 1280x800x24 >/tmp/odytools-routed-open-xvfb.out 2>/tmp/odytools-routed-open-xvfb.err &
  XVFB_PID=$!
  for _ in $(seq 1 40); do
    if DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
      break
    fi

    if ! kill -0 "$XVFB_PID" 2>/dev/null; then
      echo "Xvfb failed to start for display $DISPLAY_NAME" >&2
      cat /tmp/odytools-routed-open-xvfb.err >&2 || true
      exit 1
    fi

    sleep 0.25
  done

  if ! DISPLAY="$DISPLAY_NAME" xdotool getdisplaygeometry >/dev/null 2>&1; then
    echo "Timed out waiting for Xvfb display $DISPLAY_NAME" >&2
    cat /tmp/odytools-routed-open-xvfb.err >&2 || true
    exit 1
  fi
fi

if [[ -z "$RESULTS_FILE" ]]; then
  RESULTS_FILE="$(mktemp /tmp/odytools-routed-open.XXXXXX.tsv)"
fi

cd "$ROOT_DIR"

if [[ "$BUILD" == true ]]; then
  dotnet build src/Tools/OdyTools/OdyTools.Standalone.csproj --configuration "$CONFIGURATION" --framework "$FRAMEWORK" -m:1 -p:UseSharedCompilation=false -v quiet
fi

APPHOST="src/Tools/OdyTools/bin/$CONFIGURATION/$FRAMEWORK/OdyTools.Standalone"
if [[ ! -x "$APPHOST" ]]; then
  echo "Missing OdyTools.Standalone apphost: $APPHOST" >&2
  exit 1
fi

WORK_DIR="$(mktemp -d /tmp/odytools-routed-open.XXXXXX)"
cleanup() {
  rm -rf "$WORK_DIR"
  cleanup_xvfb
}
trap cleanup EXIT

cat > "$WORK_DIR/sample.txt" <<'TXT'
This is a routed-open TXT smoke file.
TXT

cat > "$WORK_DIR/sample.ini" <<'INI'
[RoutedOpen]
value=ini
INI

cat > "$WORK_DIR/sample.cfg" <<'CFG'
routed_open_cfg=true
CFG

cat > "$WORK_DIR/sample.log" <<'LOG'
Routed-open LOG smoke file.
LOG

cat > "$WORK_DIR/sample.2da_bak" <<'BAK'
2DA V2.0

label value
0 backup alpha
BAK

cat > "$WORK_DIR/sample.txi" <<'TXI'
proceduretype cycle
TXI

cat > "$WORK_DIR/sample.vis" <<'VIS'
sample_room
  visible_room
VIS

cat > "$WORK_DIR/sample.2da" <<'2DA'
2DA V2.0

label value
0 row0 alpha
2DA

cat > "$WORK_DIR/sample.2da.csv" <<'CSV'
label,value
row0,alpha
CSV

cp "$ROOT_DIR/vendor/tests/test_files/mdl/c_dewback.mdl" "$WORK_DIR/sample.mdl"
cp "$ROOT_DIR/vendor/tests/test_files/mdl/c_dewback.mdx" "$WORK_DIR/sample.mdx"

PTH_GEN="$WORK_DIR/PthFixture"
dotnet new console -o "$PTH_GEN" --framework "$FRAMEWORK" >/dev/null
dotnet add "$PTH_GEN" reference "$ROOT_DIR/src/BioWare/BioWare.csproj" >/dev/null
cat > "$PTH_GEN/Program.cs" <<'CS'
using BioWare.Resource.Formats.GFF.Generics;

var pth = new PTH();
var first = pth.Add(10, 20);
var second = pth.Add(30, 40);
pth.Connect(first, second);
pth.Connect(second, first);
PTHAuto.WritePth(pth, args[0]);
CS
dotnet run --project "$PTH_GEN" --framework "$FRAMEWORK" -- "$WORK_DIR/sample.pth" >/dev/null

SAV_GEN="$WORK_DIR/SavFixture"
dotnet new console -o "$SAV_GEN" --framework "$FRAMEWORK" >/dev/null
dotnet add "$SAV_GEN" reference "$ROOT_DIR/src/BioWare/BioWare.csproj" >/dev/null
cat > "$SAV_GEN/Program.cs" <<'CS'
using System.Text;
using BioWare.Common;
using BioWare.Resource.Formats.ERF;

var sav = new ERF(ERFType.MOD, isSave: true);
sav.SetData("readme", ResourceType.TXT, Encoding.ASCII.GetBytes("routed-open sav smoke"));
ERFAuto.WriteErf(sav, args[0], ResourceType.SAV);
CS
dotnet run --project "$SAV_GEN" --framework "$FRAMEWORK" -- "$WORK_DIR/sample.sav" >/dev/null

GFF_GEN="$WORK_DIR/GffFixture"
dotnet new console -o "$GFF_GEN" --framework "$FRAMEWORK" >/dev/null
dotnet add "$GFF_GEN" reference "$ROOT_DIR/src/BioWare/BioWare.csproj" >/dev/null
cat > "$GFF_GEN/Program.cs" <<'CS'
using System;
using System.IO;
using System.Numerics;
using BioWare.Common;
using BioWare.Extract.SaveData;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.CNV;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using BioWare.Resource.Formats.GFF.Generics.DLG.IO;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.LTR;
using BioWare.Resource.Formats.LYT;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.TLK;
using BioWare.Resource.Formats.TPC;
using BioWare.Resource.Formats.WAV;

static void WriteGff(string directory, string name, GFFContent content, ResourceType restype)
{
    File.WriteAllBytes(Path.Combine(directory, "sample." + name), GFFAuto.BytesGff(new GFF(content), restype));
}

var outputDirectory = args[0];
var dlg = new DLG();
var dlgEntry = new DLGEntry
{
    Comment = "Routed open DLG entry",
    Text = LocalizedString.FromEnglish("Routed open dialog smoke")
};
dlg.EntryList.Add(dlgEntry);
dlg.Starters.Add(new DLGLink(dlgEntry, 0));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.dlg"), DLGHelper.BytesDlg(dlg));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.dlg.json"), DLGHelper.BytesDlg(dlg, fileFormat: ResourceType.DLG_JSON));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.twine.html"), Twine.BytesTwine(dlg, "html"));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.twine.json"), Twine.BytesTwine(dlg, "json"));

var cnv = new CNV();
var cnvEntry = new CNVEntry
{
    Comment = "Routed open CNV entry",
    Text = LocalizedString.FromEnglish("Routed open CNV smoke")
};
cnv.Starters.Add(new CNVLink(cnvEntry, 0));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.cnv"), CNVHelper.BytesCnv(cnv, BioWareGame.DA));

WriteGff(outputDirectory, "are", GFFContent.ARE, ResourceType.ARE);
WriteGff(outputDirectory, "are.xml", GFFContent.ARE, ResourceType.ARE_XML);
WriteGff(outputDirectory, "fac", GFFContent.FAC, ResourceType.FAC);
WriteGff(outputDirectory, "fac.xml", GFFContent.FAC, ResourceType.FAC_XML);
WriteGff(outputDirectory, "gff", GFFContent.GFF, ResourceType.GFF);
WriteGff(outputDirectory, "gff.xml", GFFContent.GFF, ResourceType.GFF_XML);
WriteGff(outputDirectory, "gff.json", GFFContent.GFF, ResourceType.GFF_JSON);
WriteGff(outputDirectory, "res", GFFContent.GFF, ResourceType.RES);
WriteGff(outputDirectory, "res.xml", GFFContent.GFF, ResourceType.RES_XML);
WriteGff(outputDirectory, "gam", GFFContent.GAM, ResourceType.GAM);
WriteGff(outputDirectory, "git", GFFContent.GIT, ResourceType.GIT);
WriteGff(outputDirectory, "git.xml", GFFContent.GIT, ResourceType.GIT_XML);
WriteGff(outputDirectory, "gui", GFFContent.GUI, ResourceType.GUI);
WriteGff(outputDirectory, "ifo", GFFContent.IFO, ResourceType.IFO);
WriteGff(outputDirectory, "ifo.xml", GFFContent.IFO, ResourceType.IFO_XML);
WriteGff(outputDirectory, "jrl", GFFContent.JRL, ResourceType.JRL);
WriteGff(outputDirectory, "utc", GFFContent.UTC, ResourceType.UTC);
WriteGff(outputDirectory, "utc.xml", GFFContent.UTC, ResourceType.UTC_XML);
WriteGff(outputDirectory, "btc", GFFContent.BTC, ResourceType.BTC);
WriteGff(outputDirectory, "bic", GFFContent.BIC, ResourceType.BIC);
WriteGff(outputDirectory, "utd", GFFContent.UTD, ResourceType.UTD);
WriteGff(outputDirectory, "utd.xml", GFFContent.UTD, ResourceType.UTD_XML);
WriteGff(outputDirectory, "btd", GFFContent.BTD, ResourceType.BTD);
WriteGff(outputDirectory, "ute", GFFContent.UTE, ResourceType.UTE);
WriteGff(outputDirectory, "ute.xml", GFFContent.UTE, ResourceType.UTE_XML);
WriteGff(outputDirectory, "bte", GFFContent.BTE, ResourceType.BTE);
WriteGff(outputDirectory, "uti", GFFContent.UTI, ResourceType.UTI);
WriteGff(outputDirectory, "uti.xml", GFFContent.UTI, ResourceType.UTI_XML);
WriteGff(outputDirectory, "bti", GFFContent.BTI, ResourceType.BTI);
WriteGff(outputDirectory, "utm", GFFContent.UTM, ResourceType.UTM);
WriteGff(outputDirectory, "utm.xml", GFFContent.UTM, ResourceType.UTM_XML);
WriteGff(outputDirectory, "utm.json", GFFContent.UTM, ResourceType.UTM_JSON);
WriteGff(outputDirectory, "btm", GFFContent.BTM, ResourceType.BTM);
WriteGff(outputDirectory, "utp", GFFContent.UTP, ResourceType.UTP);
WriteGff(outputDirectory, "utp.xml", GFFContent.UTP, ResourceType.UTP_XML);
WriteGff(outputDirectory, "utp.json", GFFContent.UTP, ResourceType.UTP_JSON);
WriteGff(outputDirectory, "btp", GFFContent.BTP, ResourceType.BTP);
WriteGff(outputDirectory, "uts", GFFContent.UTS, ResourceType.UTS);
WriteGff(outputDirectory, "uts.xml", GFFContent.UTS, ResourceType.UTS_XML);
WriteGff(outputDirectory, "utt", GFFContent.UTT, ResourceType.UTT);
WriteGff(outputDirectory, "utt.xml", GFFContent.UTT, ResourceType.UTT_XML);
WriteGff(outputDirectory, "btt", GFFContent.BTT, ResourceType.BTT);
WriteGff(outputDirectory, "utw", GFFContent.UTW, ResourceType.UTW);

var bwm = new BWM();
bwm.Faces.Add(new BWMFace(
    new Vector3(0, 0, 0),
    new Vector3(1, 0, 0),
    new Vector3(0, 1, 0))
{
    Material = SurfaceMaterial.Stone
});
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.wok"), BWMAuto.BytesBwm(bwm, ResourceType.WOK));

var lip = new LIP { Length = 1.5f };
lip.Add(0.25f, LIPShape.AH);
lip.Add(1.00f, LIPShape.OH);
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.lip"), LIPAuto.BytesLip(lip));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.lip.xml"), LIPAuto.BytesLip(lip, ResourceType.LIP_XML));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.lip.json"), LIPAuto.BytesLip(lip, ResourceType.LIP_JSON));

var ltr = new LTR();
ltr.SetSinglesStart("a", 0.50f);
ltr.SetSinglesMiddle("b", 0.25f);
ltr.SetSinglesEnd("c", 0.75f);
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.ltr"), LTRAuto.BytesLtr(ltr));

var lyt = new LYT();
lyt.Rooms.Add(new LYTRoom("sample_room", Vector3.Zero));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.lyt"), LYTAuto.BytesLyt(lyt));

var ssf = new SSF();
ssf.SetData(SSFSound.BATTLE_CRY_1, 100);
ssf.SetData(SSFSound.SELECT_1, 200);
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.ssf"), SSFAuto.BytesSsf(ssf));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.ssf.xml"), SSFAuto.BytesSsf(ssf, ResourceType.SSF_XML));

var tlk = new TLK();
tlk.Add("Routed open TLK smoke", "smoke_vo");
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.tlk"), TLKAuto.BytesTlk(tlk));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.tlk.xml"), TLKAuto.BytesTlk(tlk, ResourceType.TLK_XML));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.tlk.json"), TLKAuto.BytesTlk(tlk, ResourceType.TLK_JSON));

var erf = new ERF(ERFType.ERF);
erf.SetData("sample_txt", ResourceType.TXT, System.Text.Encoding.ASCII.GetBytes("routed-open erf smoke"));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.erf"), ERFAuto.BytesErf(erf, ResourceType.ERF));

var mod = new ERF(ERFType.MOD);
mod.SetData("module_txt", ResourceType.TXT, System.Text.Encoding.ASCII.GetBytes("routed-open mod smoke"));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.mod"), ERFAuto.BytesErf(mod, ResourceType.MOD));

var hak = new ERF(ERFType.ERF);
hak.SetData("hak_txt", ResourceType.TXT, System.Text.Encoding.ASCII.GetBytes("routed-open hak smoke"));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.hak"), ERFAuto.BytesErf(hak, ResourceType.HAK));

var rim = new RIM();
rim.SetData("rim_txt", ResourceType.TXT, System.Text.Encoding.ASCII.GetBytes("routed-open rim smoke"));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.rim"), RIMAuto.BytesRim(rim, ResourceType.RIM));

var bif = new BIF(BIFType.BIF);
bif.SetData(new ResRef("bif_txt"), ResourceType.TXT, System.Text.Encoding.ASCII.GetBytes("routed-open bif smoke"), 7);
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.bif"), new BIFBinaryWriter(bif).Write());

var saveFolderPath = Path.Combine(outputDirectory, "savegame-folder");
Directory.CreateDirectory(saveFolderPath);
var saveFolder = new SaveFolderEntry(saveFolderPath);
saveFolder.SaveInfo.SavegameName = "Routed Open Save";
saveFolder.SaveInfo.PcName = "RoutedPlayer";
saveFolder.SaveInfo.AreaName = "Routed Area";
saveFolder.SaveInfo.LastModule = "routed_module";
saveFolder.SaveInfo.TimePlayed = 120;
saveFolder.PartyTable.Members.Add(new PartyMemberEntry { Index = -1, IsLeader = true });
saveFolder.PartyTable.Gold = 50;
saveFolder.GlobalVars.SetBool("ROUTED_OPEN_SAVE", true);
saveFolder.Save();

File.WriteAllBytes(Path.Combine(outputDirectory, "sample.tpc"), TPCAuto.BytesTpc(TPC.FromBlank()));
var commonTexture = TPC.FromBlank();
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.tga"), TPCAuto.BytesTpc(commonTexture, ResourceType.TGA));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.dds"), TPCAuto.BytesTpc(commonTexture, ResourceType.DDS));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.bmp"), TPCAuto.BytesTpc(commonTexture, ResourceType.BMP));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.plt"), new byte[]
{
    (byte)'P', (byte)'L', (byte)'T', (byte)' ',
    (byte)'V', (byte)'1', (byte)' ', (byte)' ',
    0, 0, 0, 0,
    0, 0, 0, 0,
    2, 0, 0, 0,
    1, 0, 0, 0,
    16, 0,
    220, 4
});
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.wav"), WAVAuto.BytesWav(new WAV(data: new byte[128])));
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.mp3"), new byte[] { 0xFF, 0xFB, 0x90, 0x64, 0, 0, 0, 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.ogg"), new byte[] { (byte)'O', (byte)'g', (byte)'g', (byte)'S', 0, 0, 0, 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.wma"), new byte[] { 0x30, 0x26, 0xB2, 0x75, 0, 0, 0, 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.flac"), new byte[] { (byte)'f', (byte)'L', (byte)'a', (byte)'C', 0, 0, 0, 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.bmu"), new byte[] { (byte)'B', (byte)'M', (byte)'U', 0, 0xFF, 0xFB, 0, 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.wmv"), new byte[] { 0x30, 0x26, 0xB2, 0x75, (byte)'W', (byte)'M', (byte)'V', 0 });
File.WriteAllBytes(Path.Combine(outputDirectory, "sample.xmv"), new byte[] { (byte)'X', (byte)'M', (byte)'V', 0, 1, 2, 3, 4 });
CS
dotnet run --project "$GFF_GEN" --framework "$FRAMEWORK" -- "$WORK_DIR" >/dev/null

printf '%s' 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+/p9sAAAAASUVORK5CYII=' | base64 -d > "$WORK_DIR/sample.png"
printf '%s' '/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAP//////////////////////////////////////////////////////////////////////////////////////2wBDAf//////////////////////////////////////////////////////////////////////////////////////wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAX/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIQAxAAAAGf/8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABBQJ//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAwEBPwF//8QAFBEBAAAAAAAAAAAAAAAAAAAAAP/aAAgBAgEBPwF//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQAGPwJ//8QAFBABAAAAAAAAAAAAAAAAAAAAAP/aAAgBAQABPyF//9k=' | base64 -d > "$WORK_DIR/sample.jpg"

cat > "$WORK_DIR/sample.nss" <<'NSS'
void main() {
    SpeakString("Routed open NSS smoke");
}
NSS

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

window_ids() {
  DISPLAY="$DISPLAY_NAME" xdotool search --class . 2>/dev/null || true
}

routes() {
  printf '%s\t%s\t%s\n' "txt" "$WORK_DIR/sample.txt" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "ini" "$WORK_DIR/sample.ini" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "cfg" "$WORK_DIR/sample.cfg" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "log" "$WORK_DIR/sample.log" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "2da-bak" "$WORK_DIR/sample.2da_bak" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "txi" "$WORK_DIR/sample.txi" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "vis" "$WORK_DIR/sample.vis" "OdyToolTXT"
  printf '%s\t%s\t%s\n' "twoda" "$WORK_DIR/sample.2da" "OdyTool2DA"
  printf '%s\t%s\t%s\n' "twoda-csv" "$WORK_DIR/sample.2da.csv" "OdyTool2DA"
  printf '%s\t%s\t%s\t%s\n' "twoda-positional" "$WORK_DIR/sample.2da" "OdyTool2DA" "positional"
  printf '%s\t%s\t%s\t%s\n' "twoda-editor-positional" "$WORK_DIR/sample.2da" "OdyTool2DA" "editor-positional"
  printf '%s\t%s\t%s\n' "are" "$WORK_DIR/sample.are" "OdyToolARE"
  printf '%s\t%s\t%s\n' "are-xml" "$WORK_DIR/sample.are.xml" "OdyToolARE"
  printf '%s\t%s\t%s\n' "bwm" "$WORK_DIR/sample.wok" "OdyToolBWM"
  printf '%s\t%s\t%s\n' "dlg" "$WORK_DIR/sample.dlg" "DLG Editor"
  printf '%s\t%s\t%s\n' "dlg-json" "$WORK_DIR/sample.dlg.json" "DLG Editor"
  printf '%s\t%s\t%s\n' "cnv" "$WORK_DIR/sample.cnv" "DLG Editor"
  printf '%s\t%s\t%s\n' "twine-html" "$WORK_DIR/sample.twine.html" "DLG Editor"
  printf '%s\t%s\t%s\n' "twine-json" "$WORK_DIR/sample.twine.json" "DLG Editor"
  printf '%s\t%s\t%s\n' "fac" "$WORK_DIR/sample.fac" "OdyToolFAC"
  printf '%s\t%s\t%s\n' "fac-xml" "$WORK_DIR/sample.fac.xml" "OdyToolFAC"
  printf '%s\t%s\t%s\n' "gff" "$WORK_DIR/sample.gff" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "gff-xml" "$WORK_DIR/sample.gff.xml" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "gff-json" "$WORK_DIR/sample.gff.json" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "res" "$WORK_DIR/sample.res" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "res-xml" "$WORK_DIR/sample.res.xml" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "gam" "$WORK_DIR/sample.gam" "OdyToolGFF"
  printf '%s\t%s\t%s\n' "git" "$WORK_DIR/sample.git" "OdyToolGIT"
  printf '%s\t%s\t%s\n' "git-xml" "$WORK_DIR/sample.git.xml" "OdyToolGIT"
  printf '%s\t%s\t%s\n' "gui" "$WORK_DIR/sample.gui" "OdyToolGUI"
  printf '%s\t%s\t%s\n' "ifo" "$WORK_DIR/sample.ifo" "OdyToolIFO"
  printf '%s\t%s\t%s\n' "ifo-xml" "$WORK_DIR/sample.ifo.xml" "OdyToolIFO"
  printf '%s\t%s\t%s\n' "jrl" "$WORK_DIR/sample.jrl" "OdyToolJRL"
  printf '%s\t%s\t%s\n' "lip" "$WORK_DIR/sample.lip" "OdyToolLIP"
  printf '%s\t%s\t%s\n' "lip-xml" "$WORK_DIR/sample.lip.xml" "OdyToolLIP"
  printf '%s\t%s\t%s\n' "lip-json" "$WORK_DIR/sample.lip.json" "OdyToolLIP"
  printf '%s\t%s\t%s\n' "ltr" "$WORK_DIR/sample.ltr" "OdyToolLTR"
  printf '%s\t%s\t%s\n' "lyt" "$WORK_DIR/sample.lyt" "OdyToolLYT"
  printf '%s\t%s\t%s\n' "mdl" "$WORK_DIR/sample.mdl" "OdyToolMDL"
  printf '%s\t%s\t%s\n' "mdx" "$WORK_DIR/sample.mdx" "OdyToolMDL"
  printf '%s\t%s\t%s\n' "nss" "$WORK_DIR/sample.nss" "OdyToolNSS"
  printf '%s\t%s\t%s\n' "pth" "$WORK_DIR/sample.pth" "OdyToolPTH"
  printf '%s\t%s\t%s\n' "erf" "$WORK_DIR/sample.erf" "OdyToolERF"
  printf '%s\t%s\t%s\n' "mod" "$WORK_DIR/sample.mod" "OdyToolERF"
  printf '%s\t%s\t%s\n' "rim" "$WORK_DIR/sample.rim" "OdyToolERF"
  printf '%s\t%s\t%s\n' "hak" "$WORK_DIR/sample.hak" "OdyToolERF"
  printf '%s\t%s\t%s\n' "bif" "$WORK_DIR/sample.bif" "OdyToolERF"
  printf '%s\t%s\t%s\n' "sav" "$WORK_DIR/sample.sav" "OdyToolERF"
  printf '%s\t%s\t%s\n' "savegame-folder" "$WORK_DIR/savegame-folder" "OdyToolSAV"
  printf '%s\t%s\t%s\n' "savegame-sav" "$WORK_DIR/savegame-folder/savegame.sav" "OdyToolSAV"
  printf '%s\t%s\t%s\n' "ssf" "$WORK_DIR/sample.ssf" "OdyToolSSF"
  printf '%s\t%s\t%s\n' "ssf-xml" "$WORK_DIR/sample.ssf.xml" "OdyToolSSF"
  printf '%s\t%s\t%s\n' "tlk" "$WORK_DIR/sample.tlk" "OdyToolTLK"
  printf '%s\t%s\t%s\n' "tlk-xml" "$WORK_DIR/sample.tlk.xml" "OdyToolTLK"
  printf '%s\t%s\t%s\n' "tlk-json" "$WORK_DIR/sample.tlk.json" "OdyToolTLK"
  printf '%s\t%s\t%s\n' "tpc" "$WORK_DIR/sample.tpc" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "tga" "$WORK_DIR/sample.tga" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "dds" "$WORK_DIR/sample.dds" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "png" "$WORK_DIR/sample.png" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "jpg" "$WORK_DIR/sample.jpg" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "bmp" "$WORK_DIR/sample.bmp" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "plt" "$WORK_DIR/sample.plt" "OdyToolTPC"
  printf '%s\t%s\t%s\n' "utc" "$WORK_DIR/sample.utc" "OdyToolUTC"
  printf '%s\t%s\t%s\n' "utc-xml" "$WORK_DIR/sample.utc.xml" "OdyToolUTC"
  printf '%s\t%s\t%s\n' "btc" "$WORK_DIR/sample.btc" "OdyToolUTC"
  printf '%s\t%s\t%s\n' "bic" "$WORK_DIR/sample.bic" "OdyToolUTC"
  printf '%s\t%s\t%s\n' "utd" "$WORK_DIR/sample.utd" "OdyToolUTD"
  printf '%s\t%s\t%s\n' "utd-xml" "$WORK_DIR/sample.utd.xml" "OdyToolUTD"
  printf '%s\t%s\t%s\n' "btd" "$WORK_DIR/sample.btd" "OdyToolUTD"
  printf '%s\t%s\t%s\n' "ute" "$WORK_DIR/sample.ute" "OdyToolUTE"
  printf '%s\t%s\t%s\n' "ute-xml" "$WORK_DIR/sample.ute.xml" "OdyToolUTE"
  printf '%s\t%s\t%s\n' "bte" "$WORK_DIR/sample.bte" "OdyToolUTE"
  printf '%s\t%s\t%s\n' "uti" "$WORK_DIR/sample.uti" "OdyToolUTI"
  printf '%s\t%s\t%s\n' "uti-xml" "$WORK_DIR/sample.uti.xml" "OdyToolUTI"
  printf '%s\t%s\t%s\n' "bti" "$WORK_DIR/sample.bti" "OdyToolUTI"
  printf '%s\t%s\t%s\n' "utm" "$WORK_DIR/sample.utm" "OdyToolUTM"
  printf '%s\t%s\t%s\n' "utm-xml" "$WORK_DIR/sample.utm.xml" "OdyToolUTM"
  printf '%s\t%s\t%s\n' "utm-json" "$WORK_DIR/sample.utm.json" "OdyToolUTM"
  printf '%s\t%s\t%s\n' "btm" "$WORK_DIR/sample.btm" "OdyToolUTM"
  printf '%s\t%s\t%s\n' "utp" "$WORK_DIR/sample.utp" "OdyToolUTP"
  printf '%s\t%s\t%s\n' "utp-xml" "$WORK_DIR/sample.utp.xml" "OdyToolUTP"
  printf '%s\t%s\t%s\n' "utp-json" "$WORK_DIR/sample.utp.json" "OdyToolUTP"
  printf '%s\t%s\t%s\n' "btp" "$WORK_DIR/sample.btp" "OdyToolUTP"
  printf '%s\t%s\t%s\n' "uts" "$WORK_DIR/sample.uts" "OdyToolUTS"
  printf '%s\t%s\t%s\n' "uts-xml" "$WORK_DIR/sample.uts.xml" "OdyToolUTS"
  printf '%s\t%s\t%s\n' "utt" "$WORK_DIR/sample.utt" "OdyToolUTT"
  printf '%s\t%s\t%s\n' "utt-xml" "$WORK_DIR/sample.utt.xml" "OdyToolUTT"
  printf '%s\t%s\t%s\n' "btt" "$WORK_DIR/sample.btt" "OdyToolUTT"
  printf '%s\t%s\t%s\n' "utw" "$WORK_DIR/sample.utw" "OdyToolUTW"
  printf '%s\t%s\t%s\n' "wav" "$WORK_DIR/sample.wav" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "mp3" "$WORK_DIR/sample.mp3" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "ogg" "$WORK_DIR/sample.ogg" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "wma" "$WORK_DIR/sample.wma" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "flac" "$WORK_DIR/sample.flac" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "bmu" "$WORK_DIR/sample.bmu" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "wmv" "$WORK_DIR/sample.wmv" "OdyToolWAV"
  printf '%s\t%s\t%s\n' "xmv" "$WORK_DIR/sample.xmv" "OdyToolWAV"
}

printf 'route\tresult\twindow\texit_or_pid\n' > "$RESULTS_FILE"
failed=0

while IFS=$'\t' read -r route file expected_title launch_mode; do
  [[ -n "$route" ]] || continue

  log_file="$(mktemp "/tmp/${route}.routed-open.XXXXXX.log")"
  before_windows="$(window_ids | tr '\n' ' ' || true)"

  if [[ "$launch_mode" == "positional" ]]; then
    DISPLAY="$DISPLAY_NAME" setsid "$ROOT_DIR/$APPHOST" "$file" --theme light > "$log_file" 2>&1 < /dev/null &
  elif [[ "$launch_mode" == "editor-positional" ]]; then
    DISPLAY="$DISPLAY_NAME" setsid "$ROOT_DIR/$APPHOST" --editor 2da "$file" --theme light > "$log_file" 2>&1 < /dev/null &
  else
    DISPLAY="$DISPLAY_NAME" setsid "$ROOT_DIR/$APPHOST" --open "$file" --theme light > "$log_file" 2>&1 < /dev/null &
  fi
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
        if [[ "$title" == *"$expected_title"* || "$title" == *"Editor failed to start"* ]]; then
          found_window="$id"
          found_title="$title"
          break 2
        fi
      fi
    done

    sleep "$SLEEP_SECONDS"
  done

  if [[ -n "$found_window" && "$found_title" == *"$expected_title"* && "$found_title" != *"Editor failed to start"* ]]; then
    printf '%s\twindow\t%s\t%s\n' "$route" "$found_title" "$pid" >> "$RESULTS_FILE"
    DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
    sleep "$SLEEP_SECONDS"
    kill "$pid" 2>/dev/null || true
  elif [[ -n "$found_window" ]]; then
    printf '%s\twrong-window\t%s\t%s\n' "$route" "$found_title" "$pid" >> "$RESULTS_FILE"
    DISPLAY="$DISPLAY_NAME" xdotool windowclose "$found_window" 2>/dev/null || true
    kill "$pid" 2>/dev/null || true
    failed=1
  elif kill -0 "$pid" 2>/dev/null; then
    printf '%s\tno-window-running\t\t%s\n' "$route" "$pid" >> "$RESULTS_FILE"
    kill "$pid" 2>/dev/null || true
    failed=1
  else
    wait "$pid" || exit_code=$?
    exit_code="${exit_code:-0}"
    first_line="$(sed -n '1,1p' "$log_file" | tr '\t' ' ')"
    printf '%s\texited\t%s\t%s\n' "$route" "$first_line" "$exit_code" >> "$RESULTS_FILE"
    failed=1
  fi

  rm -f "$log_file"
done < <(routes)

cat "$RESULTS_FILE"

if [[ "$failed" -ne 0 ]]; then
  echo "One or more routed-open smoke checks failed. Results: $RESULTS_FILE" >&2
  exit 1
fi
