#!/usr/bin/env python3
"""Merge caller_name_matches into cascade after quality filtering."""
import json

GENERIC = {
    "CExoString", "operator_new", "operator_delete", "void_return", "operator=",
    "~CExoString", "CResRef", "~CResRef", "operator+", "operator new", "operator delete",
    "operator!=", "operator==", "operator<", "malloc", "free", "memcpy", "memset",
    "~CSWCObject",
}

matches = json.load(open("docs/tsl_caller_name_matches.json", encoding="utf-8"))
cascade = json.load(open("docs/k1_tsl_cascade_matches.json", encoding="utf-8"))

strong = []
weak = []
for m in matches:
    callers = set(m.get("named_callers", []))
    specific = callers - GENERIC
    # Caller fingerprints are generally stronger (caller names are more meaningful)
    if len(specific) >= 2 or (len(specific) >= 1 and len(callers) >= 2):
        strong.append(m)
    else:
        weak.append(m)

print(f"Strong matches: {len(strong)}")
print(f"Weak matches:   {len(weak)}")
if weak:
    print("Weak:")
    for m in weak:
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | {m.get('named_callers', [])}")

existing_tsl = {m["tsl_addr"] for m in cascade["matches"]}
new_entries = []
for m in strong:
    if m["tsl_addr"] not in existing_tsl:
        entry = {
            "k1_addr": m["k1_addr"],
            "tsl_addr": m["tsl_addr"],
            "name": m["name"],
            "class": m["class"],
            "method": m["method"],
            "via": m.get("via", "caller_name_fingerprint"),
        }
        new_entries.append(entry)

print(f"\nNew cascade entries to add: {len(new_entries)}")
cascade["matches"].extend(new_entries)
print(f"New total: {len(cascade['matches'])}")

with open("docs/k1_tsl_cascade_matches.json", "w", encoding="utf-8") as f:
    json.dump(cascade, f, indent=2)
print("Written to docs/k1_tsl_cascade_matches.json")
