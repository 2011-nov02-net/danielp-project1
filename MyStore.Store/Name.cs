using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyStore.Store
{
    /// <summary>
    /// A struct used to store customer names for easy accsess and identififcation.
    /// </summary>
    public struct Name : IComparer<Name>
    {
        /// <summary>
        /// A first name
        /// </summary>
        public string First { get; }
        /// <summary>
        /// A middle initial
        /// </summary>
        public char? MiddleInitial { get; }
        /// <summary>
        /// A last name
        /// </summary>
        public string Last { get; }

        /// <summary>
        /// Convert a Name.tostring() back into a name.
        /// </summary>
        /// <param name="namestr">A name that's been transformed into a string.</param>
        public Name(string namestr)
        {
            char[] splitchar = new char[1];
            splitchar[0] = ' ';
            List<string> nameparts = namestr.Split(splitchar, StringSplitOptions.RemoveEmptyEntries).ToList() ;

            foreach (var str in nameparts)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    nameparts.Remove(str);
                    str.Trim(); 
                }
            }

            /*
            for (int i = 0; i < nameparts.Count; i++)
            {
                nameparts[i].Trim();
            }
            */

            //default value
            MiddleInitial = null;

            if(nameparts.Count == 2 || nameparts.Count == 3)
            {
                First = nameparts[0].Trim();

                if(nameparts.Count == 3)
                {
                    //middle initial 
                    if(nameparts[1].Length > 1)
                    {
                        Console.Error.WriteLine("Warning: middle name has more than 1 character in the name constructor.");
                        Console.Error.WriteLine($"Origional name: {namestr}. \t Percieved middle name: {nameparts[1]}");

                        MiddleInitial = nameparts[1][0];
                    } else if(nameparts[1].Length == 1)
                    {
                        MiddleInitial = nameparts[1][0];
                    } else if (nameparts[1].Length < 1)
                    {
                        Console.Error.WriteLine("More than two entries in the name, but the second one is empty.");
                        throw new ArgumentException("Empty string entries not removed.");
                    }

                    Last = nameparts[2];
                } else
                {
                    Last = nameparts[1];
                }
            } else
            {
                throw new ArgumentException("Invalid name, must have at least a first and a last name.");
            }
        }


        /// <summary>
        /// Create a name with no middle initial.
        /// </summary>
        /// <param name="first">The person's first name.</param>
        /// <param name="last">The person's last name.</param>
        public Name(string first, string last)
        {
            First = first;
            Last = last;
            MiddleInitial = null;
        }

        /// <summary>
        /// Create a name with a first and last name and optionally a middle initial.
        /// </summary>
        /// <param name="first">The person's first name.</param>
        /// <param name="last">The person's last name.</param>
        /// <param name="middle">The person's middle initial.</param>
        public Name(string first, string last, char? middle = null)
        {
            First = first;
            Last = last;
            MiddleInitial = middle;
        }


        /// <summary>
        /// Compares two names to eachother
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts a name to a string
        /// </summary>
        /// <returns>The name in a regular format.</returns>
        public override string ToString()
        {
            return $"{First,20} {MiddleInitial?.ToString() ?? " "} {Last, 20}";
        }

        /// <summary>
        /// Compares the equality of two names.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
