#!/usr/bin/env python3
"""Quality-classify and merge callee_name_matches into cascade matches."""
import json

GENERIC = {
    "CExoString", "operator_new", "operator_delete", "void_return", "operator=",
    "~CExoString", "CResRef", "~CResRef", "operator+", "operator new", "operator delete",
    "operator!=", "operator==", "operator<", "malloc", "free", "memcpy", "memset",
}

matches = json.load(open("docs/tsl_callee_name_matches.json", encoding="utf-8"))
cascade = json.load(open("docs/k1_tsl_cascade_matches.json", encoding="utf-8"))

strong = []
weak = []
for m in matches:
    callees = set(m["named_callees"])
    specific = callees - GENERIC
    if len(specific) >= 2:
        strong.append(m)
    elif len(specific) == 1 and len(callees) >= 3:
        strong.append(m)
    else:
        weak.append(m)

print(f"Strong matches (2+ specific callees): {len(strong)}")
print(f"Weak matches (mostly generic callees): {len(weak)}")
print()
print("Weak matches:")
for m in weak:
    print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | {m['named_callees']}")

print()
print("First 10 strong matches:")
for m in strong[:10]:
    print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | {m['named_callees'][:4]}")

# Merge strong matches into cascade (add via field)
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
            "via": "callee_name_fingerprint",
        }
        new_entries.append(entry)

print(f"\nNew cascade entries to add: {len(new_entries)}")
cascade["matches"].extend(new_entries)
print(f"New total: {len(cascade['matches'])}")

with open("docs/k1_tsl_cascade_matches.json", "w", encoding="utf-8") as f:
    json.dump(cascade, f, indent=2)
print("Written to docs/k1_tsl_cascade_matches.json")
