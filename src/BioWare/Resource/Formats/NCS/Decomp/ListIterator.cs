// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BioWare.Resource.Formats.NCS.Decomp.Node;

namespace BioWare.Resource.Formats.NCS.Decomp
{
    public interface ListIterator
    {
        bool HasNext();
        object Next();
        bool HasPrevious();
        object Previous();
        int NextIndex();
        int PreviousIndex();
        void Remove();
        void Set(object o);
        void Add(object o);
    }
}





