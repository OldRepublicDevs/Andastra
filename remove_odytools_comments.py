#!/usr/bin/env python3
"""Remove all 'Matching PyKotor' and '// Original:' comment lines from src/Tools/OdyTools."""
import os
import re

def process_file(path):
    try:
        with open(path, 'r', encoding='utf-8', newline='') as f:
            lines = f.readlines()
    except Exception as e:
        return False, 0

    out = []
    removed = 0
    for line in lines:
        stripped = line.strip()
        # Skip ConfigInfo.cs URL lines (contain "HolocronToolset" in string literal)
        if 'ConfigInfo' in path and 'HolocronToolset' in line and '["' in line and 'http' in line:
            out.append(line)
            continue
        # Remove lines that are comment-only containing these patterns
        if stripped.startswith('//') or stripped.startswith('///'):
            if 'Matching PyKotor' in line or 'Original:' in stripped:
                removed += 1
                continue
        # Also remove XML comment lines with these patterns
        if '<!--' in line and ('Matching PyKotor' in line or 'Original:' in line):
            removed += 1
            continue
        out.append(line)

    if removed == 0:
        return False, 0
    try:
        with open(path, 'w', encoding='utf-8', newline='') as f:
            f.writelines(out)
    except Exception as e:
        print(f"Error writing {path}: {e}")
        return False, 0
    return True, removed

def main():
    root = os.path.dirname(os.path.abspath(__file__))
    odytools = os.path.join(root, 'src', 'Tools', 'OdyTools')
    total_files = 0
    total_lines = 0
    for dirpath, _, filenames in os.walk(odytools):
        for name in filenames:
            if not name.endswith(('.cs', '.axaml')):
                continue
            path = os.path.join(dirpath, name)
            ok, n = process_file(path)
            if ok:
                total_files += 1
                total_lines += n
                rel = os.path.relpath(path, root)
                print(f"{rel}: removed {n} lines")
    print(f"\nTotal: {total_files} files, {total_lines} lines removed")

if __name__ == '__main__':
    main()
