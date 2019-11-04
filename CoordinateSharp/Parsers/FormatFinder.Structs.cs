using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal struct DirectionFinder
    {
        public DirectionFinder(string coordinatePartString)
        {
            rad = 1;
            partString = string.Empty;
            coordinateType = null;
            position = null;
            success = false;
            
            if (coordinatePartString.ToLower().Contains("n"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.N;
                coordinateType = CoordinateSharp.CoordinateType.Lat;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("s"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.S;
                coordinateType = CoordinateSharp.CoordinateType.Lat;
                rad = -1;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("e"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.E;
                coordinateType = CoordinateSharp.CoordinateType.Long;
                success = true;
            }
            else if (coordinatePartString.ToLower().Contains("w"))
            {
                partString = coordinatePartString;
                position = CoordinatesPosition.W;
                coordinateType = CoordinateSharp.CoordinateType.Long;
                rad = -1;
                success = true;
            }
        }

        private Nullable<CoordinateType> coordinateType;
        private Nullable<CoordinatesPosition> position;
        private string partString;
        private int rad;
        private bool success;

        public Nullable<CoordinateType> CoordinateType { get { return coordinateType; } }
        public Nullable<CoordinatesPosition> Position { get { return position; } }
        public string PartString { get { return partString; } }
        public int Rad { get { return rad; } }
        public int RadZero
        {
            get
            {
                if (rad == 1) { return 0; }
                else { return 1; }
            }
        }
        public bool Success { get { return success; } }
    }
   
}
