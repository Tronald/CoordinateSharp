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
        public static List<string> longZongLetters = new List<string>(new string[]{"C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T",
        "U", "V", "W", "X"});
    }
    /// <summary>
    /// Used for handling diagraph determination
    /// </summary>
    [Serializable]
    internal class Digraphs
    {
        private List<Digraph> digraph1;
        private List<Digraph> digraph2;      

        private String[] digraph1Array = { "A", "B", "C", "D", "E", "F", "G", "H",
        "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z" };

        private String[] digraph2Array = { "V", "A", "B", "C", "D", "E", "F", "G",
        "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V" };

        public Digraphs()
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

            digraph2.Add(new Digraph() { Zone = 0, Letter = "V"});
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

        internal int getDigraph2Index(String letter)
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
       
        internal String getDigraph1(int longZone, double easting)
        {
            int a1 = longZone;
            double a2 = 8 * ((a1 - 1) % 3) + 1;

            double a3 = easting;
            double a4 = a2 + ((int)(a3 / 100000)) - 1;
            return digraph1.Where(x=>x.Zone == Math.Floor(a4)).FirstOrDefault().Letter;
        }

        internal String getDigraph2(int longZone, double northing)
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

    }
    /// <summary>
    /// Diagraph model
    /// </summary>
    [Serializable]
    internal class Digraph
    {
        public int Zone { get; set; }
        public string Letter { get; set; }
    }
    /// <summary>
    /// Used for setting whether a coordinate part is latitudinal or longitudinal.
    /// </summary>
    [Serializable]
    public enum CoordinateType
    {
        /// <summary>
        /// Latitude
        /// </summary>
        Lat,
        /// <summary>
        /// Longitude
        /// </summary>
        Long
    }
    /// <summary>
    /// Used to set a coordinate part position.
    /// </summary>
    [Serializable]
    public enum CoordinatesPosition :int
    {
        /// <summary>
        /// North
        /// </summary>
        N,
        /// <summary>
        /// East
        /// </summary>
        E,
        /// <summary>
        /// South
        /// </summary>
        S,
        /// <summary>
        /// West
        /// </summary>
        W
    }
  
 
    /// <summary>
    /// Coordinate type datum specification
    /// </summary>
    [Serializable]
    [Flags]
    public enum Coordinate_Datum
    {
        /// <summary>
        /// Lat Long GeoDetic
        /// </summary>
        LAT_LONG = 1,
        /// <summary>
        /// UTM and MGRS
        /// </summary>
        UTM_MGRS = 2,
        /// <summary>
        /// ECEF
        /// </summary>
        ECEF = 4,      
    }
   
    /// <summary>
    /// Cartesian Coordinate Type
    /// </summary>
    public enum CartesianType
    {
        /// <summary>
        /// Spherical Cartesian
        /// </summary>
        Cartesian,
        /// <summary>
        /// Earth Centered Earth Fixed
        /// </summary>
        ECEF,
    }
    /// <summary>
    /// Used for easy read math functions
    /// </summary>
    [Serializable]
    internal static class ModM
    {
        public static double Mod(double x, double y)
        {
            return x - y * Math.Floor(x / y);
        }

        public static double ModLon(double x)
        {
            return Mod(x + Math.PI, 2 * Math.PI) - Math.PI;
        }

        public static double ModCrs(double x)
        {
            return Mod(x, 2 * Math.PI);
        }

        public static double ModLat(double x)
        {
            return Mod(x + Math.PI / 2, 2 * Math.PI) - Math.PI / 2;
        }
    }
    /// <summary>
    /// Earth Shape for Calculations.
    /// </summary>

    [Serializable]
    public enum Shape
    {
        /// <summary>
        /// Calculate as sphere (less accurate, more efficient).
        /// </summary>
        Sphere,
        /// <summary>
        /// Calculate as ellipsoid (more accurate, less efficient).
        /// </summary>
        Ellipsoid
    }
   

}
