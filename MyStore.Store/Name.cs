﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    public struct Name : IComparer<Name>
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

        public Name(string first, string last, char? middle = null)
        {
            First = first;
            Last = last;
            MiddleInitial = middle;
        }


        public int Compare(Name x, Name y)
        {
            if(x.First == y.First)
            {
                //both missing middle initial
                if (x.MiddleInitial is null && y.MiddleInitial is null)
                {           
                } else
                {
                    //one missing middle initial
                    if (x.MiddleInitial is null || y.MiddleInitial is null)
                    {
                        if(x.MiddleInitial is null)
                        {
                            return 1;
                        } else
                        {
                            return -1;
                        }
                    } else
                    {
                        //both have middle initial
                        if(x.MiddleInitial != y.MiddleInitial)
                        {
                            if( x.MiddleInitial < y.MiddleInitial)
                            {
                                return 1;
                            } else
                            {
                                return -1;
                            }
                        }
                    }
                }
                return x.Last.CompareTo(y.Last);
            } else
            {
                return x.First.CompareTo(y.First);
            }
        }

        public override string ToString()
        {
            return $"{First,20} {MiddleInitial?.ToString() ?? " "} {Last, 20}";
        }

        public override bool Equals(object obj)
        {
            if(obj is Name)
            {
                Name other = (Name) obj;

                return First == other.First && Last == other.Last && MiddleInitial == other.MiddleInitial;
            } else
            {
                return base.Equals(obj);
            }          
        }
    };
}
