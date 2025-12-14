#!/usr/bin/env python3
"""Analyze what needs to be ported from PyKotor to CSharpKOTOR."""

import os
import sys
from pathlib import Path
from collections import defaultdict

def get_pykotor_modules():
    """Get all Python modules from PyKotor."""
    pykotor_root = Path(__file__).parent.parent / "vendor" / "PyKotor" / "Libraries" / "PyKotor" / "src"
    modules = {}
    
    for py_file in pykotor_root.rglob("*.py"):
        if "__pycache__" in str(py_file) or py_file.name == "__init__.py":
            continue
        rel_path = py_file.relative_to(pykotor_root)
        module_path = str(rel_path.with_suffix("")).replace(os.sep, ".")
        modules[module_path] = py_file
    return modules

def get_csharp_files():
    """Get all C# files from CSharpKOTOR."""
    csharp_root = Path(__file__).parent.parent / "src" / "CSharpKOTOR"
    files = {}
    
    for cs_file in csharp_root.rglob("*.cs"):
        rel_path = cs_file.relative_to(csharp_root)
        file_path = str(rel_path.with_suffix("")).replace(os.sep, ".")
        files[file_path] = cs_file
    return files

def map_python_to_csharp(python_path):
    """Map Python module path to C# namespace/class path."""
    # Remove pykotor prefix
    if python_path.startswith("pykotor."):
        python_path = python_path[8:]
    elif python_path.startswith("utility."):
        python_path = python_path[8:]
        return f"Utility.{python_path}"
    
    # Map common patterns
    parts = python_path.split(".")
    
    # Format specific mappings
    if len(parts) > 1 and parts[0] == "resource" and parts[1] == "formats":
        # pykotor.resource.formats.gff -> CSharpKOTOR.Formats.GFF
        format_name = parts[2].upper()
        if len(parts) > 3:
            return f"Formats.{format_name}.{'.'.join(parts[3:])}"
        return f"Formats.{format_name}"
    
    # Generic mapping: pykotor.module.submodule -> CSharpKOTOR.Module.Submodule
    return ".".join(p.title() for p in parts)

def main():
    pykotor_modules = get_pykotor_modules()
    csharp_files = get_csharp_files()
    
    print(f"Found {len(pykotor_modules)} Python modules")
    print(f"Found {len(csharp_files)} C# files")
    print("\n=== Module Analysis ===\n")
    
    # Group by top-level module
    by_module = defaultdict(list)
    for module_path in sorted(pykotor_modules.keys()):
        top_level = module_path.split(".")[0]
        by_module[top_level].append(module_path)
    
    for top_level in sorted(by_module.keys()):
        modules = by_module[top_level]
        print(f"\n{top_level} ({len(modules)} modules):")
        for module in modules[:10]:  # Show first 10
            print(f"  - {module}")
        if len(modules) > 10:
            print(f"  ... and {len(modules) - 10} more")

if __name__ == "__main__":
    main()
