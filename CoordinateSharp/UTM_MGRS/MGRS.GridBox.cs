using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace CoordinateSharp
{
    /// <summary>
    /// MGRS 100km Square Identifier Box Details
    /// </summary>
    [Serializable]
    public class MGRS_GridBox
    {
        private EagerLoad el;
        private MilitaryGridReferenceSystem mgrs;

        private bool isBoxValid = true;
        private MilitaryGridReferenceSystem bottom_Left;
        private MilitaryGridReferenceSystem bottom_Right;
        private MilitaryGridReferenceSystem top_Left;
        private MilitaryGridReferenceSystem top_Right;
       
        /// <summary>
        /// Bool check to determine if the provided MGRS point contains a valid Grid Zone Designator / 100km Square Identifier combination.
        /// </summary>
        public bool IsBoxValid { get => isBoxValid; }

        /// <summary>
        /// Populate MGRS_GridBox details based on the provided MGRS coordinate.
        /// </summary>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        public MGRS_GridBox(MilitaryGridReferenceSystem mgrs)
        {
            Construct_GridBox(mgrs, new EagerLoad());          
        }
        /// <summary>
        /// Populate MGRS_GridBox details based on the provided MGRS coordinate.
        /// Option to specify eager loading options for more efficient lat / long Coordinate creation.
        /// </summary>
        /// <param name="mgrs">MilitaryGridReferenceSystem</param>
        /// <param name="el">EagerLoad</param>
        public MGRS_GridBox(MilitaryGridReferenceSystem mgrs, EagerLoad el)
        {
            Construct_GridBox(mgrs, el);
        }
        private void Construct_GridBox(MilitaryGridReferenceSystem mgrs, EagerLoad el)
        {
            if(el.UTM_MGRS==false|| el.Extensions.MGRS == false) { throw new InvalidOperationException("MGRS must be set to eager load during MGRS_GridBox creation."); }
            this.el = el;
            this.mgrs = mgrs;

            if (!Check_Valid_Box()) { isBoxValid = false; return; }

            bottom_Left = Bottom_Left_Position();       
            bottom_Right = Bottom_Right_Position();
            top_Left = Top_Left_Position();
            top_Right = Top_Right_Position();
        }

        /// <summary>
        /// MGRS point location at the bottom left of the 100km grid square.
        /// </summary>
        public MilitaryGridReferenceSystem Bottom_Left_MGRS_Point { get => bottom_Left; }
        /// <summary>
        /// MGRS point location at the bottom right of the 100km grid square.
        /// </summary>
        public MilitaryGridReferenceSystem Bottom_Right_MGRS_Point { get => bottom_Right; }

        /// <summary>
        /// MGRS point location at the top left of the 100km grid square.
        /// </summary>
        public MilitaryGridReferenceSystem Top_Left_MGRS_Point { get => top_Left; }

        /// <summary>
        /// MGRS point location at the top right of the 100km grid square.
        /// </summary>
        public MilitaryGridReferenceSystem Top_Right_MGRS_Point { get => top_Right; }

        /// <summary>
        /// Lat/Long point location at the bottom left of the 100km grid square.
        /// </summary>
        public Coordinate Bottom_Left_Coordinate_Point { get => MilitaryGridReferenceSystem.MGRStoLatLong(bottom_Left, el); }
        /// <summary>
        /// Lat/Long point location at the bottom right of the 100km grid square.
        /// </summary>
        public Coordinate Bottom_Right_Coordinate_Point { get => MilitaryGridReferenceSystem.MGRStoLatLong(bottom_Right, el); }
        /// <summary>
        /// Lat/Long point location at the top left of the 100km grid square.
        /// </summary>
        public Coordinate Top_Left_Coordinate_Point { get => MilitaryGridReferenceSystem.MGRStoLatLong(top_Left, el); }
        /// <summary>
        /// Lat/Long point location at the top right of the 100km grid square.
        /// </summary>
        public Coordinate Top_Right_Coordinate_Point { get => MilitaryGridReferenceSystem.MGRStoLatLong(top_Right, el); }


        private MilitaryGridReferenceSystem Bottom_Left_Position()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS); //For performance 

            var cc = CornerCheck(Corner.BL); //Check is basic corner coordinate is valid. If so this will save on iteration time
            if (cc != null) { return cc; }

            var nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 00000, 00000);
            var val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);

            double northing = 00000;           

            while (nMgrs.Digraph != val.MGRS.Digraph)
            {
                int chunk = 10000;
                double easting = 0;
               
                while (easting <= 100000)
                {
                    nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, LC(easting), LC(northing));
                    val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                    
                    if (nMgrs.Digraph == val.MGRS.Digraph)
                    {
                        if (chunk > 1) { easting -= chunk; chunk /= 10; }                      
                        else { return Correct_Min_Precision_Loss(nMgrs); }
                    }

                    easting += chunk;
                }
                northing++;

                if (northing == 100000) { isBoxValid = false; return null; }
            }

            return Correct_Min_Precision_Loss(nMgrs);
        }
        private MilitaryGridReferenceSystem Bottom_Right_Position()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS); //For performance     

            var cc = CornerCheck(Corner.BR);
            if (cc != null) { return cc; }

            var nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999, 00000);
            var val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);

            double northing = 00000;         

            while (nMgrs.Digraph != val.MGRS.Digraph)
            {
                
                int chunk = 10000;
               
                double easting = 100000;

                while (easting >= 0)
                {
                    nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, LC(easting), LC(northing));
                    val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                    if (nMgrs.Digraph == val.MGRS.Digraph)
                    {
                        if (chunk > 1) { easting += chunk; chunk /= 10; }
                        else { return Correct_Min_Precision_Loss(nMgrs); }
                    }

                    easting -= chunk;
                }
                northing++;

                if (northing == 100000) { isBoxValid = false; return null; }
            }

           return Correct_Min_Precision_Loss(nMgrs);
        }
        private MilitaryGridReferenceSystem Top_Left_Position()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS); //For performance          

            var cc = CornerCheck(Corner.TL);
            if (cc != null) { return cc; }

            var nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 00000, 99999);
            var val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);

            var ft = First_Top_Chunk(mgrs);
            double northing = ft[0];
            
            while (nMgrs.Digraph != val.MGRS.Digraph)
            {
                int chunk = 10000;             
                double easting = 0;

                while (easting <= 100000)
                {                  
                    nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, LC(easting), LC(northing));
                    val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                    if (nMgrs.Digraph == val.MGRS.Digraph)
                    {
                        if (chunk > 1) { easting -= chunk; chunk /= 10; }
                        else { return Correct_Min_Precision_Loss(nMgrs); }
                    }

                  

                    easting = easting + chunk;
                  
                }
                northing--;

                if (northing <0) { isBoxValid = false; return null; }
            }

            return Correct_Min_Precision_Loss(nMgrs);
        }
        private MilitaryGridReferenceSystem Top_Right_Position()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS); //For performance 

            var cc = CornerCheck(Corner.TR);
            if (cc != null) { return cc; }

            var nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999, 99999);
            var val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);

            var ft = First_Top_Chunk(mgrs);
            double northing = ft[0];

            while (nMgrs.Digraph != val.MGRS.Digraph)
            {
                int chunk = 10000;
                double easting = 100000;

                while (easting >= 0)
                {
                    if (easting == 0) { easting += ft[1]; } //Adjust for offset specified at top

                    nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, LC(easting), LC(northing));
                    val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                    if (nMgrs.Digraph == val.MGRS.Digraph)
                    {
                        if (chunk > 1) { easting += chunk; chunk /= 10; }
                        else { return Correct_Min_Precision_Loss(nMgrs); }
                    }

                    easting = easting - chunk;
                   
                }
                northing--;

                if (northing <0) { isBoxValid = false; return null; }
            }

            return Correct_Min_Precision_Loss(nMgrs);
        }

        //Check Corners to avoid missed corners due to precision loss
        private MilitaryGridReferenceSystem CornerCheck(Corner corner)
        {
            double e=00000;
            double n=00000;
            int ope = 1;
            int opn = 1;

            if (corner == Corner.BR) { ope = -1; e = 999999; }
            if (corner == Corner.TL) { opn = -1; n = 999999;  }
            if (corner == Corner.TR) { ope = -1; opn = -1; e = 999999; n = 99999; }

            for (int x = 0; x <= 4; x++)
            {
                int nE = x * ope;
                int nN = x * opn;
                var nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, e, n + nN);
                var val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                if (val.MGRS.Digraph == nMgrs.Digraph) { return nMgrs; }
                nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, e + nE, n);
                val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                if (val.MGRS.Digraph == nMgrs.Digraph) { return nMgrs; }
                nMgrs = new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, e + nE, n + nN);
                val = MilitaryGridReferenceSystem.MGRStoLatLong(nMgrs, el);
                if (val.MGRS.Digraph == nMgrs.Digraph) { return nMgrs; }

            }
            
            return null;
        }
      
        /// <summary>
        /// Ensures MGRS coordinate provided contains a valid box
        /// </summary>
        /// <returns></returns>
        private bool Check_Valid_Box()
        {
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 0, 0))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999, 0))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999, 0))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999, 99999))) { return true; }

            //Corner precision loss confirmation
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 2, 2))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99997, 2))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99997, 2))) { return true; }
            if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99997, 99997))) { return true; }

            return false;

        }

        /// <summary>
        /// Checks to see if provided point exists in 100km square by converting to lat/long and back.
        /// </summary>
        /// <param name="mgrs">MGRS</param>
        /// <returns>bool</returns>
        private bool Check_Point(MilitaryGridReferenceSystem mgrs)
        {
            var c = MilitaryGridReferenceSystem.MGRStoLatLong(mgrs, new EagerLoad(EagerLoadType.UTM_MGRS));
            if(c.MGRS.Digraph != mgrs.Digraph) { return false; }
            return true;
        }
   
        /// <summary>
        /// Floors max easting/northing value for MGRS
        /// </summary>
        /// <param name="d">double</param>
        /// <returns>double</returns>
        private double LC(double d)
        {
            if (d > 99999) { return 99999; }
            if (d < 1) { return 1; }
            return d;
        }
        private MilitaryGridReferenceSystem Correct_Min_Precision_Loss(MilitaryGridReferenceSystem nMgrs)
        {
            double e = nMgrs.Easting;
            double n = nMgrs.Northing;
            if (e == 1) { e = 0; }
            if (e == 99998) { e = 99999; }
            if (n == 1) { n = 0; }
            if (n == 99998) { n = 99999; }
            return new MilitaryGridReferenceSystem(nMgrs.LatZone, nMgrs.LongZone, nMgrs.Digraph, e, n);
          
        }
        
        /// <summary>
        /// Chunks down from the top of the MGRS square on both Max/Min Easting to determine nearest top chunk.
        /// This is needed for algorithm efficiency.
        /// </summary>
        /// <param name="mgrs">MGRS</param>
        /// <returns>double</returns>
        private double[] First_Top_Chunk(MilitaryGridReferenceSystem mgrs)
        {
            for (int offset = 0; offset < 3; offset++)
            {
                for (double x = 100000; x >= 0; x -= 10000)
                {
                    if (Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 0+offset, LC(x))) ||
                        Check_Point(new MilitaryGridReferenceSystem(mgrs.LatZone, mgrs.LongZone, mgrs.Digraph, 99999-offset, LC(x))))
                    {
                        if (x != 100000)
                        {
                            return new double[] { x + 10000, offset};
                        }
                        return new double[] { x, offset };
                    }

                }
            }
            return new double[] { 100000, 0 };
        }

      
      
        private enum Corner
        {
            BL, BR, TL, TR
        }
    }
    
}
