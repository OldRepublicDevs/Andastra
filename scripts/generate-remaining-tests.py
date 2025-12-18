#!/usr/bin/env python3
"""Generate test stubs for remaining tests from test_indoor_builder.py"""
from __future__ import annotations

import re

# Read Python test file
with open(
    r"vendor/PyKotor/Tools/HolocronToolset/tests/gui/windows/test_indoor_builder.py",
    "r",
    encoding="utf-8",
) as f:
    python_content = f.read()

# Read C# test file to find what's already ported
with open(
    r"src/Tests/HolocronToolset.Tests/Windows/IndoorBuilderTests.cs",
    "r",
    encoding="utf-8",
) as f:
    csharp_content = f.read()

# Find all Python tests with their docstrings
python_tests: list[tuple[str, str]] = []
for match in re.finditer(
    r"^\s+def (test_\w+)\([^)]*\):\s*\n(\s+)\"\"\"([^\"]*?)\"\"\"",
    python_content,
    re.MULTILINE | re.DOTALL,
):
    name = match.group(1)
    docstring = match.group(3).strip().split("\n")[0]
    python_tests.append((name, docstring))

# Find all ported tests
ported = set(re.findall(r"// Original: def (test_\w+)\(", csharp_content))

# Find missing tests
missing = [(name, doc) for name, doc in python_tests if name not in ported]

print(f"Total Python tests: {len(python_tests)}")
print(f"Ported: {len(ported)}")
print(f"Missing: {len(missing)}")
print("\nMissing test names:")
for name, doc in missing:
    print(f"{name}")
