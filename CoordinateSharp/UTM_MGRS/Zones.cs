/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2019, Signature Group, LLC
  
This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 
as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): 
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY Signature Group, LLC. Signature Group, LLC DISCLAIMS THE WARRANTY OF 
NON INFRINGEMENT OF THIRD PARTY RIGHTS.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU 
Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the 
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL:

https://www.gnu.org/licenses/agpl-3.0.html

The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, 
as required under Section 5 of the GNU Affero General Public License.

You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory 
as soon as you develop commercial activities involving the CoordinateSharp software without disclosing the source code of your own applications. 
These activities include: offering paid services to customers as an ASP, on the fly location based calculations in a web application, 
or shipping CoordinateSharp with a closed source product.

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request.
-Department of Defense
-Department of Homeland Security
-Open source contributors to this library
-Scholarly or scientific uses on a case by case basis.
-Emergency response / management uses on a case by case basis.

For more information, please contact Signature Group, LLC at this address: sales@signatgroup.com
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoordinateSharp
{
    /// <summary>
    /// Used for UTM/MGRS Conversions
    /// </summary>
    [Serializable]
    internal class LatZones
    {
        public static List<string> longZongLetters = new List<string>(new string[]{"A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T",
        "U", "V", "W", "X", "Y", "Z"});
    }
    /// <summary>
    /// Used for handling digraph determination
    /// </summary>
    [Serializable]
    internal class Digraphs
    {
        public List<Digraph> digraph1;
        public List<Digraph> digraph2;

        private String[] digraph1Array = { "A", "B", "C", "D", "E", "F", "G", "H",
        "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z" };

        private String[] digraph2Array = { "V", "A", "B", "C", "D", "E", "F", "G",
        "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V" };

       
        public Digraphs(MGRS_Type systemType, string zone)
        {
            if(systemType== MGRS_Type.MGRS)
            {
                Create_MGRS_Digraphs();
            }
            else
            {
                if (zone.ToUpper() == "A" || zone.ToUpper() == "B")
                {
                    Create_MGRS_PolarS_Digraphs();
                }
                else if (zone.ToUpper() == "Y" || zone.ToUpper() == "Z")
                {
                    Create_MGRS_PolarN_Digraphs();
                }
                else { throw new ArgumentOutOfRangeException("MGRS Polar Coordinate Zone \'" + zone + "\" is invalid."); }
            }
        }

        private void Create_MGRS_Digraphs()
        {
            digraph1 = new List<Digraph>();
            digraph2 = new List<Digraph>();

            digraph1.Add(new Digraph() { Zone = 1, Letter = "A" });
            digraph1.Add(new Digraph() { Zone = 2, Letter = "B" });
            digraph1.Add(new Digraph() { Zone = 3, Letter = "C" });
            digraph1.Add(new Digraph() { Zone = 4, Letter = "D" });
            digraph1.Add(new Digraph() { Zone = 5, Letter = "E" });
            digraph1.Add(new Digraph() { Zone = 6, Letter = "F" });
            digraph1.Add(new Digraph() { Zone = 7, Letter = "G" });
            digraph1.Add(new Digraph() { Zone = 8, Letter = "H" });
            digraph1.Add(new Digraph() { Zone = 9, Letter = "J" });
            digraph1.Add(new Digraph() { Zone = 10, Letter = "K" });
            digraph1.Add(new Digraph() { Zone = 11, Letter = "L" });
            digraph1.Add(new Digraph() { Zone = 12, Letter = "M" });
            digraph1.Add(new Digraph() { Zone = 13, Letter = "N" });
            digraph1.Add(new Digraph() { Zone = 14, Letter = "P" });
            digraph1.Add(new Digraph() { Zone = 15, Letter = "Q" });
            digraph1.Add(new Digraph() { Zone = 16, Letter = "R" });
            digraph1.Add(new Digraph() { Zone = 17, Letter = "S" });
            digraph1.Add(new Digraph() { Zone = 18, Letter = "T" });
            digraph1.Add(new Digraph() { Zone = 19, Letter = "U" });
            digraph1.Add(new Digraph() { Zone = 20, Letter = "V" });
            digraph1.Add(new Digraph() { Zone = 21, Letter = "W" });
            digraph1.Add(new Digraph() { Zone = 22, Letter = "X" });
            digraph1.Add(new Digraph() { Zone = 23, Letter = "Y" });
            digraph1.Add(new Digraph() { Zone = 24, Letter = "Z" });
            digraph1.Add(new Digraph() { Zone = 1, Letter = "A" });

            digraph2.Add(new Digraph() { Zone = 0, Letter = "V" });
            digraph2.Add(new Digraph() { Zone = 1, Letter = "A" });
            digraph2.Add(new Digraph() { Zone = 2, Letter = "B" });
            digraph2.Add(new Digraph() { Zone = 3, Letter = "C" });
            digraph2.Add(new Digraph() { Zone = 4, Letter = "D" });
            digraph2.Add(new Digraph() { Zone = 5, Letter = "E" });
            digraph2.Add(new Digraph() { Zone = 6, Letter = "F" });
            digraph2.Add(new Digraph() { Zone = 7, Letter = "G" });
            digraph2.Add(new Digraph() { Zone = 8, Letter = "H" });
            digraph2.Add(new Digraph() { Zone = 9, Letter = "J" });
            digraph2.Add(new Digraph() { Zone = 10, Letter = "K" });
            digraph2.Add(new Digraph() { Zone = 11, Letter = "L" });
            digraph2.Add(new Digraph() { Zone = 12, Letter = "M" });
            digraph2.Add(new Digraph() { Zone = 13, Letter = "N" });
            digraph2.Add(new Digraph() { Zone = 14, Letter = "P" });
            digraph2.Add(new Digraph() { Zone = 15, Letter = "Q" });
            digraph2.Add(new Digraph() { Zone = 16, Letter = "R" });
            digraph2.Add(new Digraph() { Zone = 17, Letter = "S" });
            digraph2.Add(new Digraph() { Zone = 18, Letter = "T" });
            digraph2.Add(new Digraph() { Zone = 19, Letter = "U" });
            digraph2.Add(new Digraph() { Zone = 20, Letter = "V" });
        }
        private void Create_MGRS_PolarS_Digraphs()
        {
            digraph1 = new List<Digraph>();
            digraph2 = new List<Digraph>();

            digraph1.Add(new Digraph() { Zone = 0, Letter = "J" });
            digraph1.Add(new Digraph() { Zone = 1, Letter = "K" });
            digraph1.Add(new Digraph() { Zone = 2, Letter = "L" });
            digraph1.Add(new Digraph() { Zone = 3, Letter = "P" });
            digraph1.Add(new Digraph() { Zone = 4, Letter = "Q" });
            digraph1.Add(new Digraph() { Zone = 5, Letter = "R" });
            digraph1.Add(new Digraph() { Zone = 6, Letter = "S" });
            digraph1.Add(new Digraph() { Zone = 7, Letter = "T" });
            digraph1.Add(new Digraph() { Zone = 8, Letter = "U" });
            digraph1.Add(new Digraph() { Zone = 9, Letter = "X" });
            digraph1.Add(new Digraph() { Zone = 10, Letter = "Y" });
            digraph1.Add(new Digraph() { Zone = 11, Letter = "Z" });
            digraph1.Add(new Digraph() { Zone = 12, Letter = "A" });
            digraph1.Add(new Digraph() { Zone = 13, Letter = "B" });
            digraph1.Add(new Digraph() { Zone = 14, Letter = "C" });
            digraph1.Add(new Digraph() { Zone = 15, Letter = "F" });
            digraph1.Add(new Digraph() { Zone = 16, Letter = "G" });
            digraph1.Add(new Digraph() { Zone = 17, Letter = "H" });
            digraph1.Add(new Digraph() { Zone = 18, Letter = "J" });
            digraph1.Add(new Digraph() { Zone = 19, Letter = "K" });
            digraph1.Add(new Digraph() { Zone = 20, Letter = "L" });
            digraph1.Add(new Digraph() { Zone = 21, Letter = "P" });
            digraph1.Add(new Digraph() { Zone = 22, Letter = "Q" });
            digraph1.Add(new Digraph() { Zone = 23, Letter = "R" });         
         
            digraph2.Add(new Digraph() { Zone = 0, Letter = "W" });
            digraph2.Add(new Digraph() { Zone = 1, Letter = "X" });
            digraph2.Add(new Digraph() { Zone = 2, Letter = "Y" });
            digraph2.Add(new Digraph() { Zone = 3, Letter = "Z" });
            digraph2.Add(new Digraph() { Zone = 4, Letter = "A" });
            digraph2.Add(new Digraph() { Zone = 5, Letter = "B" });
            digraph2.Add(new Digraph() { Zone = 6, Letter = "C" });
            digraph2.Add(new Digraph() { Zone = 7, Letter = "D" });
            digraph2.Add(new Digraph() { Zone = 8, Letter = "E" });
            digraph2.Add(new Digraph() { Zone = 9, Letter = "F" });
            digraph2.Add(new Digraph() { Zone = 10, Letter = "G" });
            digraph2.Add(new Digraph() { Zone = 11, Letter = "H" });
            digraph2.Add(new Digraph() { Zone = 12, Letter = "J" });
            digraph2.Add(new Digraph() { Zone = 13, Letter = "K" });
            digraph2.Add(new Digraph() { Zone = 14, Letter = "L" });
            digraph2.Add(new Digraph() { Zone = 15, Letter = "M" });
            digraph2.Add(new Digraph() { Zone = 16, Letter = "N" });
            digraph2.Add(new Digraph() { Zone = 17, Letter = "P" });
            digraph2.Add(new Digraph() { Zone = 18, Letter = "Q" });
            digraph2.Add(new Digraph() { Zone = 19, Letter = "R" });
            digraph2.Add(new Digraph() { Zone = 20, Letter = "S" });
            digraph2.Add(new Digraph() { Zone = 21, Letter = "T" });
            digraph2.Add(new Digraph() { Zone = 22, Letter = "U" });
            digraph2.Add(new Digraph() { Zone = 23, Letter = "V" });
            digraph2.Add(new Digraph() { Zone = 24, Letter = "W" });
            digraph2.Add(new Digraph() { Zone = 25, Letter = "X" });
            digraph2.Add(new Digraph() { Zone = 26, Letter = "Y" });
            digraph2.Add(new Digraph() { Zone = 27, Letter = "Z" });
         
        }
        private void Create_MGRS_PolarN_Digraphs()
        {
            digraph1 = new List<Digraph>();
            digraph2 = new List<Digraph>();

            digraph1.Add(new Digraph() { Zone = 0, Letter = "J" });
            digraph1.Add(new Digraph() { Zone = 1, Letter = "K" });
            digraph1.Add(new Digraph() { Zone = 2, Letter = "L" });
            digraph1.Add(new Digraph() { Zone = 3, Letter = "P" });
            digraph1.Add(new Digraph() { Zone = 4, Letter = "Q" });
            digraph1.Add(new Digraph() { Zone = 5, Letter = "R" });
            digraph1.Add(new Digraph() { Zone = 6, Letter = "S" });
            digraph1.Add(new Digraph() { Zone = 7, Letter = "T" });
            digraph1.Add(new Digraph() { Zone = 8, Letter = "U" });
            digraph1.Add(new Digraph() { Zone = 9, Letter = "X" });
            digraph1.Add(new Digraph() { Zone = 10, Letter = "Y" });
            digraph1.Add(new Digraph() { Zone = 11, Letter = "Z" });
            digraph1.Add(new Digraph() { Zone = 12, Letter = "A" });
            digraph1.Add(new Digraph() { Zone = 13, Letter = "B" });
            digraph1.Add(new Digraph() { Zone = 14, Letter = "C" });
            digraph1.Add(new Digraph() { Zone = 15, Letter = "F" });
            digraph1.Add(new Digraph() { Zone = 16, Letter = "G" });
            digraph1.Add(new Digraph() { Zone = 17, Letter = "H" });
            digraph1.Add(new Digraph() { Zone = 18, Letter = "J" });
            digraph1.Add(new Digraph() { Zone = 19, Letter = "K" });
            digraph1.Add(new Digraph() { Zone = 20, Letter = "L" });
            digraph1.Add(new Digraph() { Zone = 21, Letter = "P" });
            digraph1.Add(new Digraph() { Zone = 22, Letter = "Q" });
            digraph1.Add(new Digraph() { Zone = 23, Letter = "R" });




            digraph2.Add(new Digraph() { Zone = 0, Letter = "F" });
            digraph2.Add(new Digraph() { Zone = 1, Letter = "G" });
            digraph2.Add(new Digraph() { Zone = 2, Letter = "H" });
            digraph2.Add(new Digraph() { Zone = 3, Letter = "J" });
            digraph2.Add(new Digraph() { Zone = 4, Letter = "K" });
            digraph2.Add(new Digraph() { Zone = 5, Letter = "L" });
            digraph2.Add(new Digraph() { Zone = 6, Letter = "M" });
            digraph2.Add(new Digraph() { Zone = 7, Letter = "N" });
            digraph2.Add(new Digraph() { Zone = 8, Letter = "P" });
            digraph2.Add(new Digraph() { Zone = 9, Letter = "A" });
            digraph2.Add(new Digraph() { Zone = 10, Letter = "B" });
            digraph2.Add(new Digraph() { Zone = 11, Letter = "C" });
            digraph2.Add(new Digraph() { Zone = 12, Letter = "D" });
            digraph2.Add(new Digraph() { Zone = 13, Letter = "E" });
            digraph2.Add(new Digraph() { Zone = 14, Letter = "F" });
            digraph2.Add(new Digraph() { Zone = 15, Letter = "G" });
            digraph2.Add(new Digraph() { Zone = 16, Letter = "H" });
            digraph2.Add(new Digraph() { Zone = 17, Letter = "J" });
            digraph2.Add(new Digraph() { Zone = 18, Letter = "K" });
            digraph2.Add(new Digraph() { Zone = 19, Letter = "L" });
            digraph2.Add(new Digraph() { Zone = 20, Letter = "M" });
            digraph2.Add(new Digraph() { Zone = 21, Letter = "N" });
            digraph2.Add(new Digraph() { Zone = 22, Letter = "P" });
            digraph2.Add(new Digraph() { Zone = 23, Letter = "A" });
            digraph2.Add(new Digraph() { Zone = 24, Letter = "B" });
            digraph2.Add(new Digraph() { Zone = 25, Letter = "C" });
            digraph2.Add(new Digraph() { Zone = 26, Letter = "D" });
            digraph2.Add(new Digraph() { Zone = 27, Letter = "E" });
        }

        internal int getDigraph1Index(String letter)
        {
            for (int i = 0; i < digraph1Array.Length; i++)
            {
                if (digraph1Array[i].Equals(letter))
                {
                    return i + 1;
                }
            }

            return -1;
        }

        internal int getDigraph2Index(string letter)
        {
            for (int i = 0; i < digraph2Array.Length; i++)
            {
                if (digraph2Array[i].Equals(letter))
                {
                    return i;
                }
            }

            return -1;
        }

        internal string getDigraph1(int longZone, double easting)
        {
            int a1 = longZone;
            double a2 = 8 * ((a1 - 1) % 3) + 1;

            double a3 = easting;
            double a4 = a2 + ((int)(a3 / 100000)) - 1;
            return digraph1.Where(x => x.Zone == Math.Floor(a4)).FirstOrDefault().Letter;
        }

        internal string getDigraph2(int longZone, double northing)
        {
            int a1 = longZone;
            double a2 = 1 + 5 * ((a1 - 1) % 2);
            double a3 = northing;
            double a4 = (a2 + ((int)(a3 / 100000)));
            a4 = (a2 + ((int)(a3 / 100000.0))) % 20;
            a4 = Math.Floor(a4);
            if (a4 < 0)
            {
                a4 = a4 + 19;
            }
            return digraph2.Where(x => x.Zone == Math.Floor(a4)).FirstOrDefault().Letter;
        }
        internal string getDigraph2_Polar(int longZone, double northing)
        {
            //REMOVE %20 as squares are all 100kx100K
            //Verify what should happen if below 0
            int a1 = longZone;
            double a2 = 1 + 5 * ((a1 - 1) % 2);
            double a3 = northing;
            double a4 = (a2 + ((int)(a3 / 100000)));
            a4 = (a2 + ((int)(a3 / 100000.0)));
            a4 = Math.Floor(a4);
            if (a4 < 0)
            {
                a4 = a4 + 19;
            }
            return digraph2.Where(x => x.Zone == Math.Floor(a4)).FirstOrDefault().Letter;

        }

    }
    /// <summary>
    /// Digraph model
    /// </summary>
    [Serializable]
    internal class Digraph
    {
        public int Zone { get; set; }
        public string Letter { get; set; }
    }
}
