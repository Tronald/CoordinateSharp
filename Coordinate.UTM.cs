using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{  
    internal class UniversalTransverseMercator
    {
        public static String convertLatLonToUTM(double latitude, double longitude)
        {            //validate(latitude, longitude);
            String UTM = "";

            setVariables(latitude, longitude);

            String longZone = getLongZone(longitude);
            LatZones latZones = new LatZones();
            String latZone = latZones.getLatZone(latitude);

            double _easting = getEasting();
            double _northing = getNorthing(latitude);

            UTM = longZone + " " + latZone + " " + ((int)_easting) + " "
                + ((int)_northing);
            // UTM = longZone + " " + latZone + " " + decimalFormat.format(_easting) +
            // " "+ decimalFormat.format(_northing);

            return UTM;

        }
        private static double degreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
        private static void setVariables(double latitude, double longitude)
        {
            latitude = degreeToRadian(latitude);
            rho = equatorialRadius * (1 - e * e)
                / Math.Pow(1 - Math.Pow(e * Math.Sin(latitude), 2), 3 / 2.0);

            nu = equatorialRadius / Math.Pow(1 - Math.Pow(e * Math.Sin(latitude), 2), (1 / 2.0));

            double var1;
            if (longitude < 0.0)
            {
                var1 = ((int)((180 + longitude) / 6.0)) + 1;
            }
            else
            {
                var1 = ((int)(longitude / 6)) + 31;
            }
            double var2 = (6 * var1) - 183;
            double var3 = longitude - var2;
            p = var3 * 3600 / 10000;

            S = A0 * latitude - B0 * Math.Sin(2 * latitude) + C0 * Math.Sin(4 * latitude) - D0
                * Math.Sin(6 * latitude) + E0 * Math.Sin(8 * latitude);

            K1 = S * k0;
            K2 = nu * Math.Sin(latitude) * Math.Cos(latitude) * Math.Pow(sin1, 2) * k0 * (100000000)
                / 2;
            K3 = ((Math.Pow(sin1, 4) * nu * Math.Sin(latitude) * Math.Pow(Math.Cos(latitude), 3)) / 24)
                * (5 - Math.Pow(Math.Tan(latitude), 2) + 9 * e1sq * Math.Pow(Math.Cos(latitude), 2) + 4
                    * Math.Pow(e1sq, 2) * Math.Pow(Math.Cos(latitude), 4))
                * k0
                * (10000000000000000L);

            K4 = nu * Math.Cos(latitude) * sin1 * k0 * 10000;

            K5 = Math.Pow(sin1 * Math.Cos(latitude), 3) * (nu / 6)
                * (1 - Math.Pow(Math.Tan(latitude), 2) + e1sq * Math.Pow(Math.Cos(latitude), 2)) * k0
                * 1000000000000L;

            A6 = (Math.Pow(p * sin1, 6) * nu * Math.Sin(latitude) * Math.Pow(Math.Cos(latitude), 5) / 720)
                * (61 - 58 * Math.Pow(Math.Tan(latitude), 2) + Math.Pow(Math.Tan(latitude), 4) + 270
                    * e1sq * Math.Pow(Math.Cos(latitude), 2) - 330 * e1sq
                    * Math.Pow(Math.Sin(latitude), 2)) * k0 * (1E+24);

        }

        private static String getLongZone(double longitude)
        {
            double longZone = 0;
            if (longitude < 0.0)
            {
                longZone = ((180.0 + longitude) / 6) + 1;
            }
            else
            {
                longZone = (longitude / 6) + 31;
            }
            String val = Convert.ToString((int)longZone);
            if (val.Length == 1)
            {
                val = "0" + val;
            }
            return val;
        }

        private static double getNorthing(double latitude)
        {
            double northing = K1 + K2 * p * p + K3 * Math.Pow(p, 4);
            if (latitude < 0.0)
            {
                northing = 10000000 + northing;
            }
            return northing;
        }

        private static double getEasting()
        {
            return 500000 + (K4 * p + K5 * Math.Pow(p, 3));
        }
      
        // Lat Lon to UTM variables

        // equatorial radius
        private static double equatorialRadius = 6378137;

        // polar radius
        private static double polarRadius = 6356752.314;

        // flattening
        private static double flattening = 0.00335281066474748;// (equatorialRadius-polarRadius)/equatorialRadius;

        // inverse flattening 1/flattening
        private static double inverseFlattening = 298.257223563;// 1/flattening;

        // Mean radius
        private static double rm = Math.Pow(6378137 * 6356752.314, 1 / 2.0);

        // scale factor
        private static double k0 = 0.9996;

        // eccentricity
        private static double e = Math.Sqrt(1 - Math.Pow(6356752.314 / 6378137, 2));

        private static double e1sq = e * e / (1 - e * e);

        private static double n = (6378137 - 6356752.314)
            / (6378137 + 6356752.314);

        // r curv 1
        private static double rho = 6368573.744;

        // r curv 2
        private static double nu = 6389236.914;

        // Calculate Meridional Arc Length
        // Meridional Arc
        private static double S = 5103266.421;

        private static double A0 = 6367449.146;

        private static double B0 = 16038.42955;

        private static double C0 = 16.83261333;

        private static double D0 = 0.021984404;

        private static double E0 = 0.000312705;

        // Calculation Constants
        // Delta Long
        private static double p = -0.483084;

        private static double sin1 = 4.84814E-06;

        // Coefficients for UTM Coordinates
        private static double K1 = 5101225.115;

        private static double K2 = 3750.291596;

        private static double K3 = 1.397608151;

        private static double K4 = 214839.3105;

        private static double K5 = -2.995382942;

        private static double A6 = -1.00541E-07;

    }
    internal class UTM2LatLon
    {
        static double easting;

        static double northing;

        static int zone;

        static String southernHemisphere = "ACDEFGHJKLM";

        private static String getHemisphere(String latZone)
        {
            String hemisphere = "N";
            if (southernHemisphere.IndexOf(latZone) > -1)
            {
                hemisphere = "S";
            }
            return hemisphere;
        }

        public static double[] convertUTMToLatLong(String UTM)
        {
            double[] latlon = { 0.0, 0.0 };
            String[] utm = UTM.Split(' ');
            zone = int.Parse(utm[0]);
            String latZone = utm[1];
            easting = Double.Parse(utm[2]);
            northing = Double.Parse(utm[3]);
            String hemisphere = getHemisphere(latZone);
            double latitude = 0.0;
            double longitude = 0.0;

            if (hemisphere == "S")
            {
                northing = 10000000 - northing;
            }
            setVariables();
            latitude = 180 * (phi1 - fact1 * (fact2 + fact3 + fact4)) / Math.PI;

            if (zone > 0)
            {
                zoneCM = 6 * zone - 183.0;
            }
            else
            {
                zoneCM = 3.0;

            }

            longitude = zoneCM - _a3;
            if (hemisphere == "S")
            {
                latitude = -latitude;
            }

            latlon[0] = latitude;
            latlon[1] = longitude;
            return latlon;

        }

        private static void setVariables()
        {
            arc = northing / k0;
            mu = arc
                / (a * (1 - Math.Pow(e, 2) / 4.0 - 3 * Math.Pow(e, 4) / 64.0 - 5 * Math.Pow(e, 6) / 256.0));

            ei = (1 - Math.Pow((1 - e * e), (1 / 2.0)))
                / (1 + Math.Pow((1 - e * e), (1 / 2.0)));

            ca = 3 * ei / 2 - 27 * Math.Pow(ei, 3) / 32.0;

            cb = 21 * Math.Pow(ei, 2) / 16 - 55 * Math.Pow(ei, 4) / 32;
            cc = 151 * Math.Pow(ei, 3) / 96;
            cd = 1097 * Math.Pow(ei, 4) / 512;
            phi1 = mu + ca * Math.Sin(2 * mu) + cb * Math.Sin(4 * mu) + cc * Math.Sin(6 * mu) + cd
                * Math.Sin(8 * mu);

            n0 = a / Math.Pow((1 - Math.Pow((e * Math.Sin(phi1)), 2)), (1 / 2.0));

            r0 = a * (1 - e * e) / Math.Pow((1 - Math.Pow((e * Math.Sin(phi1)), 2)), (3 / 2.0));
            fact1 = n0 * Math.Tan(phi1) / r0;

            _a1 = 500000 - easting;
            dd0 = _a1 / (n0 * k0);
            fact2 = dd0 * dd0 / 2;

            t0 = Math.Pow(Math.Tan(phi1), 2);
            Q0 = e1sq * Math.Pow(Math.Cos(phi1), 2);
            fact3 = (5 + 3 * t0 + 10 * Q0 - 4 * Q0 * Q0 - 9 * e1sq) * Math.Pow(dd0, 4)
                / 24;

            fact4 = (61 + 90 * t0 + 298 * Q0 + 45 * t0 * t0 - 252 * e1sq - 3 * Q0
                * Q0)
                * Math.Pow(dd0, 6) / 720;

            //
            lof1 = _a1 / (n0 * k0);
            lof2 = (1 + 2 * t0 + Q0) * Math.Pow(dd0, 3) / 6.0;
            lof3 = (5 - 2 * Q0 + 28 * t0 - 3 * Math.Pow(Q0, 2) + 8 * e1sq + 24 * Math.Pow(t0, 2))
                * Math.Pow(dd0, 5) / 120;
            _a2 = (lof1 - lof2 + lof3) / Math.Cos(phi1);
            _a3 = _a2 * 180 / Math.PI;

        }


        static double arc;

        static double mu;

        static double ei;

        static double ca;

        static double cb;

        static double cc;

        static double cd;

        static double n0;

        static double r0;

        static double _a1;

        static double dd0;

        static double t0;

        static double Q0;

        static double lof1;

        static double lof2;

        static double lof3;

        static double _a2;

        static double phi1;

        static double fact1;

        static double fact2;

        static double fact3;

        static double fact4;

        static double zoneCM;

        static double _a3;

        static double b = 6356752.314;

        static double a = 6378137;

        static double e = 0.081819191;

        static double e1sq = 0.006739497;

        static double k0 = 0.9996;

    }

    internal class LatZones
    {
        private char[] letters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Z' };

        private int[] degrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24, -16,
        -8, 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private char[] negLetters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M' };

        private int[] negDegrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24,
        -16, -8 };

        private char[] posLetters = { 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
        'X', 'Z' };

        private int[] posDegrees = { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private int arrayLength = 22;

        public LatZones()
        {
        }
        public int getLatZoneDegree(String letter)
        {
            char ltr = letter[0];
            for (int i = 0; i < arrayLength; i++)
            {
                if (letters[i] == ltr)
                {
                    return degrees[i];
                }
            }
            return -100;
        }

        public String getLatZone(double latitude)
        {
            int latIndex = -2;
            int lat = (int)latitude;

            if (lat >= 0)
            {
                int len = posLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == posDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat > posDegrees[i])
                    {
                        continue;
                    }
                    else
                    {
                        latIndex = i - 1;
                        break;
                    }
                }
            }
            else
            {
                int len = negLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == negDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat < negDegrees[i])
                    {
                        latIndex = i - 1;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

            }

            if (latIndex == -1)
            {
                latIndex = 0;
            }
            if (lat >= 0)
            {
                if (latIndex == -2)
                {
                    latIndex = posLetters.Length - 1;
                }
                return Convert.ToString(posLetters[latIndex]);
            }
            else
            {
                if (latIndex == -2)
                {
                    latIndex = negLetters.Length - 1;
                }
                return Convert.ToString(negLetters[latIndex]);

            }
        }
    }

}
