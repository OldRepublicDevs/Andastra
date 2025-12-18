#!/usr/bin/env python3
"""Extract all test function signatures from test_indoor_builder.py for porting to C#."""

from __future__ import annotations

import re
import sys
from pathlib import Path


def extract_tests(python_file: Path) -> list[tuple[str, str, str]]:
    """Extract all test functions and their classes from Python test file."""
    content = Path(python_file).read_text(encoding="utf-8")

    # Find all test classes
    classes: list[tuple[int, str]] = []
    for match in re.finditer(r"^class (Test\w+):", content, re.MULTILINE):
        classes.append((match.start(), match.group(1)))

    # Find all test functions
    tests: list[tuple[int, str, str]] = []
    for match in re.finditer(r"^\s+def (test_\w+)\(([^)]*)\):", content, re.MULTILINE):
        tests.append((match.start(), match.group(1), match.group(2)))

    # Group tests by class
    result: list[tuple[str, str, str]] = []
    current_class_idx = 0
    for test_start, test_name, test_params in tests:
        # Find which class this test belongs to
        while (
            current_class_idx < len(classes) - 1
            and test_start > classes[current_class_idx + 1][0]
        ):
            current_class_idx += 1
        class_name = (
            classes[current_class_idx][1]
            if current_class_idx < len(classes)
            else "Unknown"
        )
        result.append((class_name, test_name, test_params))

    return result


if __name__ == "__main__":
    python_file = Path(
        "vendor/PyKotor/Tools/HolocronToolset/tests/gui/windows/test_indoor_builder.py"
    )
    if not python_file.exists():
        print(f"Error: {python_file} not found", file=sys.stderr)
        sys.exit(1)

    tests = extract_tests(python_file)
    print(f"Total tests: {len(tests)}")
    print("\nTest list:")
    for i, (class_name, test_name, test_params) in enumerate(tests, 1):
        print(f"{i:3d}. {class_name}::{test_name}")
