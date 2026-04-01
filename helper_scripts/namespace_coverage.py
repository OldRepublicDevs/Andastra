#!/usr/bin/env python3
"""
Generate a namespace-level coverage report showing which K1 classes
have their TSL counterparts matched vs still outstanding.

Output: docs/namespace_coverage_report.json
"""
import json
from collections import Counter

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/namespace_coverage_report.json"


def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    # K1 namespace -> method list
    k1_ns_methods: dict = {}
    for f in k1_cg["functions"]:
        ns = f.get("ns", "")
        if ns:
            k1_ns_methods.setdefault(ns, []).append(f["n"])

    # TSL named (non-FUN_xxx) namespace -> method list
    tsl_named_ns: dict = {}
    for f in tsl_cg["functions"]:
        if f["n"].startswith("FUN_"):
            continue
        ns = f.get("ns", "")
        if ns:
            tsl_named_ns.setdefault(ns, []).append(f["n"])

    # cascade namespace -> k1_addr -> tsl_addr mappings
    cas_by_ns: dict = {}
    for m in cas["matches"]:
        ns = m.get("class", "")
        cas_by_ns.setdefault(ns, []).append(m)

    # Build report
    report = []
    all_namespaces = set(k1_ns_methods.keys()) | set(tsl_named_ns.keys())

    for ns in sorted(all_namespaces):
        k1_methods   = k1_ns_methods.get(ns, [])
        tsl_methods  = tsl_named_ns.get(ns, [])
        cas_entries  = cas_by_ns.get(ns, [])
        # Methods in K1 that are in cascade (thus TSL mapping found)
        matched_methods = {m["method"] for m in cas_entries}

        entry = {
            "namespace":         ns,
            "k1_method_count":   len(k1_methods),
            "tsl_method_count":  len(tsl_methods),
            "cascade_count":     len(cas_entries),
            "k1_only_methods":   sorted(set(k1_methods) - set(tsl_methods) - matched_methods),
            "tsl_only_methods":  sorted(set(tsl_methods) - set(k1_methods)),
            "status": (
                "k1_only"  if not tsl_methods and not matched_methods else
                "tsl_only" if not k1_methods else
                "partial"  if (set(k1_methods) - matched_methods - set(tsl_methods)) else
                "covered"
            ),
        }
        report.append(entry)

    # Sort by K1 unmatched count descending
    report.sort(key=lambda x: len(x["k1_only_methods"]), reverse=True)

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(report, fh, indent=2)
    print(f"Wrote {len(report)} namespace entries to {OUT}")

    # Summary
    covered  = [r for r in report if r["status"] == "covered"]
    partial  = [r for r in report if r["status"] == "partial"]
    k1_only  = [r for r in report if r["status"] == "k1_only"]
    tsl_only = [r for r in report if r["status"] == "tsl_only"]

    print(f"\nSummary:")
    print(f"  Fully covered (all methods matched): {len(covered)}")
    print(f"  Partial (some matched, rest TODO):   {len(partial)}")
    print(f"  K1-only (no TSL match yet):          {len(k1_only)}")
    print(f"  TSL-only (new TSL classes):          {len(tsl_only)}")

    print(f"\nTop 20 highest-priority K1 classes to resolve (most unmatched methods):")
    for r in report[:20]:
        if r["k1_only_methods"]:
            print(f"  {r['namespace']:40s}  k1={r['k1_method_count']}  "
                  f"tsl={r['tsl_method_count']}  unmatched={len(r['k1_only_methods'])}")


if __name__ == "__main__":
    main()
