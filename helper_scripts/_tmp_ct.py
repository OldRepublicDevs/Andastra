import json
m = json.load(open('docs/tsl_cascade_translated_matches.json'))
print("Count:", len(m))
for x in m:
    print("{} -> {:12s}  {}".format(x['tsl_addr'], x['k1_addr'], x['name']))
