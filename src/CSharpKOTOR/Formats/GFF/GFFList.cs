using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CSharpKOTOR.Formats.GFF
{

    /// <summary>
    /// A collection of GFFStructs.
    /// </summary>
    public class GFFList : IEnumerable<GFFStruct>
    {
        private readonly List<GFFStruct> _structs = new List<GFFStruct>();

        public int Count => _structs.Count;

        public GFFStruct Add(int structId = 0)
        {
            var newStruct = new GFFStruct(structId);
            _structs.Add(newStruct);
            return newStruct;
        }

        [CanBeNull]
        public GFFStruct At(int index)
        {
            return index < _structs.Count ? _structs[index] : null;
        }

        public void Remove(int index)
        {
            if (index >= 0 && index < _structs.Count)
            {
                _structs.RemoveAt(index);
            }
        }

        public GFFStruct this[int index]
        {
            get => _structs[index];
            set => _structs[index] = value;
        }

        public IEnumerator<GFFStruct> GetEnumerator() => _structs.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

