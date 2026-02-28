"""One-off script: replace OLD test_roundtrip_k1_wok_face_count body with fixed version."""

from pathlib import Path

TEST_FILE = Path(__file__).resolve().parent / "test_indoor_builder_roundtrip.py"

NEW_BODY = '''        """Test K1: WOK face count preserved through roundtrip.

        Do NOT use _read_original_module_resources for WOKs: danm13.mod has 0 WOKs (in models.bif).
        Compare total faces from indoor_map room walkmeshes to rebuilt MOD WOKs.
        """
        for module_root in k1_module_roots:
            indoor_map = _import_module_into_indoor_map(module_root, k1_pykotor_installation)
            rebuilt_path = tmp_path / f"{module_root}_rebuilt.mod"
            _export_indoor_map_to_mod(indoor_map, k1_pykotor_installation, rebuilt_path)
            rebuilt_resources = _read_archive_resources(rebuilt_path)

            rebuilt_woks = {resref: data for (resref, restype), data in rebuilt_resources.items() if restype == ResourceType.WOK}
            assert len(rebuilt_woks) == len(indoor_map.rooms), (
                f"{module_root}: WOK count mismatch - rebuilt={len(rebuilt_woks)}, rooms={len(indoor_map.rooms)}"
            )

            original_total_faces = sum(len(room.base_walkmesh().faces) for room in indoor_map.rooms)
            rebuilt_total_faces = sum(len(read_bwm(data).faces) for data in rebuilt_woks.values())

            assert rebuilt_total_faces == original_total_faces, (
                f"{module_root}: Total WOK face count mismatch - original={original_total_faces}, rebuilt={rebuilt_total_faces}"
            )'''


def main() -> None:
    text = TEST_FILE.read_text(encoding="utf-8")
    start_marker = "def test_roundtrip_k1_wok_face_count("
    idx = text.find(start_marker)
    if idx == -1:
        print("Could not find test_roundtrip_k1_wok_face_count")
        return
    after_def = text[idx:]
    end_search = after_def.find("\n    def test_", 1)
    if end_search == -1:
        end_search = len(after_def)
    method_block = after_def[:end_search]
    sig_end = method_block.find("):\n") + 3
    if sig_end < 3:
        print("Could not find method signature end")
        return
    body = method_block[sig_end:]
    if "original_resources" not in body or "original_woks" not in body:
        print("File already has fixed WOK face count test (uses indoor_map, not archive).")
        return
    new_method = method_block[:sig_end] + "\n" + NEW_BODY + "\n\n"
    new_text = text[:idx] + new_method + after_def[end_search:]
    TEST_FILE.write_text(new_text, encoding="utf-8")
    print("Patched test_roundtrip_k1_wok_face_count to use indoor_map room walkmeshes.")


if __name__ == "__main__":
    main()
