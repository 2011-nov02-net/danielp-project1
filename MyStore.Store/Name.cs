using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public struct Name
    {
        public string First { get; }
        public char? MiddleInitial { get; }
        public string Last { get; }

        public Name(string first, string last)
        {
            First = first;
            Last = last;
            MiddleInitial = null;
        }

        public Name(string first, string last, char middle)
        {
            First = first;
            Last = last;
            MiddleInitial = middle;
        }
    };
}
