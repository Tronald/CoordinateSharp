using System;
using System.Collections.Generic;

namespace CoordinateSharp
{
    //CURRENT ALTITUDE IS SET CONSTANT AT 100M. POSSIBLY NEED TO ADJUST TO ALLOW USER PASS.
    //Altitude adjustments appear to have minimal effect on eclipse timing. These were mainly used
    //to signify eclipses that had already started during rise and set times on the NASA calculator

    //SOME TIMES AND ALTS WERE RETURNED WITH COLOR AND STYLING. DETERMINE WHY AND ADJUST VALUE AS REQUIRED. SEARCH "WAS ITALIC".

    //ELLIPSOID ADJUSTMENT
    //6378140.0 Ellipsoid is used in the NASA Calculator
    //WGS84 Ellipsoid is 6378137.0. Adjustments to the ellipsoid appear to effect eclipse seconds in fractions.
    //This can be modified if need to allow users to pass custom number with the Coordinate SetDatum() functions.

    //CURRENT RANGE 1601-2600.
    internal class LunarEclipseCalc
    {              
        public static List<List<string>> CalculateLunarEclipse(DateTime d, double latRad, double longRad)
        {
            return Calculate(d, latRad, longRad);
        }
        public static List<LunarEclipseDetails> CalculateLunarEclipse(DateTime d, double latRad, double longRad, double[] events)
        {
            List<List<string>> evs = Calculate(d, latRad, longRad, events);
            List<LunarEclipseDetails> deetsList = new List<LunarEclipseDetails>();
            foreach (List<string> ls in evs)
            {
                LunarEclipseDetails deets = new LunarEclipseDetails(ls);
                deetsList.Add(deets);
            }
            return deetsList;
        }
        public static List<List<string>> CalculateLunarEclipse(DateTime d, Coordinate coord)
        {
            return Calculate(d, coord.Latitude.ToRadians(), coord.Longitude.ToRadians());
        }


        // CALCULATE!
        private static List<List<string>> Calculate(DateTime d, double latRad, double longRad, double[] ev = null)
        {
            //DECLARE ARRAYS
            double[] obsvconst = new double[6];
            double[] mid = new double[41];
            double[] p1 = new double[41];
            double[] u1 = new double[41];
            double[] u2 = new double[41];
            double[] u3 = new double[41];
            double[] u4 = new double[41];
            double[] p4 = new double[41];

            List<List<string>> events = new List<List<string>>();

            double[] el;
            if (ev == null)
            {
                el = Eclipse.LunarData.LunarDateData(d);//Get 100 year solar data;
            }
            else
            {
                el = ev;
            }

            events = new List<List<string>>();
            ReadData(latRad, longRad, obsvconst);
            
            for (int i = 0; i < el.Length; i += 22)
            {
                if (el[5 + i] <= obsvconst[5])
                {
                    List<string> values = new List<string>();
                    obsvconst[4] = i;
                    GetAll(el, obsvconst, mid, p1, u1, u2,u3,u4,p4);
                    // Is there an event...
                    if (mid[5] != 1)
                    {

                        values.Add(GetDate(el, p1, obsvconst));
                   
                        if (el[5 + i] == 1)
                        {
                            values.Add("T");
                        }
                        else if (el[5 + i] == 2)
                        {
                            values.Add("P");
                        }
                        else
                        {
                            values.Add("N");
                        }

                        // Pen. Mag
                        values.Add(el[3 + i].ToString());

                        // Umbral Mag
                        values.Add(el[4 + i].ToString());

                        // P1
                        values.Add(GetTime(el, p1, obsvconst));

                        // P1 alt
                        values.Add(GetAlt(p1));

                        if (u1[5] == 1)
                        {
                            values.Add("-");
                            values.Add("-");
                        }
                        else
                        {
                            // U1
                            values.Add(GetTime(el, u1, obsvconst));

                            // U1 alt
                            values.Add(GetAlt(u1));
                        }
                        if (u2[5] == 1)
                        {
                            values.Add("-");
                            values.Add("-");
                        }
                        else
                        {
                            // U2
                            values.Add(GetTime(el, u2, obsvconst));

                            // U2 alt
                            values.Add(GetAlt(u2));
                        }
                        // mid

                        values.Add(GetTime(el, mid, obsvconst));

                        // mid alt

                        values.Add(GetAlt(mid));

                        if (u3[5] == 1)
                        {
                            values.Add("-");
                            values.Add("-");
                        }
                        else
                        {
                            // u3
                            values.Add(GetTime(el, u3, obsvconst));

                            // u3 alt
                            values.Add(GetAlt(u3));
                        }
                        if (u4[5] == 1)
                        {
                            values.Add("-");
                            values.Add("-");
                        }
                        else
                        {
                            // u4
                            values.Add(GetTime(el, u4, obsvconst));

                            // u4 alt
                            values.Add(GetAlt(u4));

                        }
                        // P4
                        values.Add(GetTime(el, p4, obsvconst));

                        // P4 alt
                        values.Add(GetAlt(p4));
                        events.Add(values);
                    }
                }
            }
            return events;
        }
        // Read the data that's in the form, and populate the obsvconst array
        private static void ReadData(double latRad, double longRad, double[] obsvconst)
        {

            // Get the latitude
            obsvconst[0] = latRad;

            // Get the longitude
            obsvconst[1] = -1 * longRad; //PASS REVERSE RADIAN.

            // Get the altitude
            obsvconst[2] = 100; //CHANGE TO ALLOW USER TO PASS.

            // Get the time zone
            obsvconst[3] = 0; //GMT TIME

            obsvconst[4] = 0; //INDEX

            //SET MAX ECLIPSE TYPE
            obsvconst[5] = 4;//4 is ALL Eclipses

        }
        // Populate the p1, u1, u2, mid, u3, u4 and p4 arrays
        private static void GetAll(double[] elements, double[] obsvconst, double[] mid, double[] p1, double[] u1, double[] u2, double[] u3, double[] u4, double[] p4)
        {
            int index = (int)obsvconst[4];
            p1[1] = elements[index + 9];
            PopulateCircumstances(elements, p1, obsvconst);
            mid[1] = elements[index + 12];
            PopulateCircumstances(elements, mid, obsvconst);
            p4[1] = elements[index + 15];
            PopulateCircumstances(elements, p4, obsvconst);
            if (elements[index + 5] < 3)
            {
                u1[1] = elements[index + 10];
                PopulateCircumstances(elements, u1, obsvconst);
                u4[1] = elements[index + 14];
                PopulateCircumstances(elements, u4, obsvconst);
                if (elements[index + 5] < 2)
                {
                    u2[1] = elements[index + 11];
                    u3[1] = elements[index + 13];
                    PopulateCircumstances(elements, u2, obsvconst);
                    PopulateCircumstances(elements, u3, obsvconst);
                }
                else
                {
                    u2[5] = 1;
                    u3[5] = 1;
                }
            }
            else
            {
                u1[5] = 1;
                u2[5] = 1;
                u3[5] = 1;
                u4[5] = 1;
            }
            if ((p1[5] != 0) && (u1[5] != 0) && (u2[5] != 0) && (mid[5] != 0) && (u3[5] != 0) && (u4[5] != 0) && (p4[5] != 0))
            {
                mid[5] = 1;
            }
        }
        // Populate the circumstances array
        // entry condition - circumstances[1] must contain the correct value
        private static void PopulateCircumstances(double[] elements, double[] circumstances, double[] obsvconst)
        {
            double t, ra, dec, h;

            int index = (int)obsvconst[4];
            t = circumstances[1];
            ra = elements[18 + index] * t + elements[17 + index];
            ra = ra * t + elements[16 + index];
            dec = elements[21 + index] * t + elements[20 + index];
            dec = dec * t + elements[19 + index];
            dec = dec * Math.PI / 180.0;
            circumstances[3] = dec;
            h = 15.0 * (elements[6 + index] + (t - elements[2 + index] / 3600.0) * 1.00273791) - ra;
            h = h * Math.PI / 180.0 - obsvconst[1];
            circumstances[2] = h;
            circumstances[4] = Math.Asin(Math.Sin(obsvconst[0]) * Math.Sin(dec) + Math.Cos(obsvconst[0]) * Math.Cos(dec) * Math.Cos(h));
            circumstances[4] -= Math.Asin(Math.Sin(elements[7 + index] * Math.PI / 180.0) * Math.Cos(circumstances[4]));
            if (circumstances[4] * 180.0 / Math.PI < elements[8 + index] - 0.5667)
            {
                circumstances[5] = 2;
            }
            else if (circumstances[4] < 0.0)
            {
                circumstances[4] = 0.0;
                circumstances[5] = 0;
            }
            else
            {
                circumstances[5] = 0;
            }
        }
        // Get the date of an event
        private static string GetDate(double[] elements, double[] circumstances, double[] obsvconst)
        {
            string[] month = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };//Month string array
            double t, jd, a, b, c, d, e;
            string ans = "";
            int index = (int)obsvconst[4];
            // Calculate the JD for noon (TDT) the day before the day that contains T0
            jd = Math.Floor(elements[index] - (elements[1 + index] / 24.0));
            // Calculate the local time (ie the offset in hours since midnight TDT on the day containing T0).
            t = circumstances[1] + elements[1 + index] - obsvconst[3] - (elements[2 + index] - 30.0) / 3600.0;

            if (t < 0.0)
            {
                jd--;
            }
            if (t >= 24.0)
            {
                jd++;
            }
            if (jd >= 2299160.0)
            {
                a = Math.Floor((jd - 1867216.25) / 36524.25);
                a = jd + 1 + a - Math.Floor(a / 4.0);
            }
            else
            {
                a = jd;
            }
            b = a + 1525.0;
            c = Math.Floor((b - 122.1) / 365.25);
            d = Math.Floor(365.25 * c);
            e = Math.Floor((b - d) / 30.6001);
            d = b - d - Math.Floor(30.6001 * e);
            if (e < 13.5)
            {
                e = e - 1;
            }
            else
            {
                e = e - 13;
            }
            double year;
            if (e > 2.5)
            {
                ans = c - 4716 + "-";
                year = c - 4716;
            }
            else
            {
                ans = c - 4715 + "-";
                year = c - 4715;
            }
            string m = month[(int)e - 1];
            ans += m+ "-";
            if (d < 10)
            {
                ans = ans + "0";
            }
            ans = ans + d;
            //Leap Year Integrity Check

            if (m == "Feb" && d == 29 && !DateTime.IsLeapYear((int)year))
            {
                ans = year.ToString() + "-Mar-01";
            }
            return ans;
        }
        // Get the time of an event
        private static string GetTime(double[] elements, double[] circumstances, double[] obsvconst)
        {
            double t;
            string ans = "";

            int index = (int)obsvconst[4];
          t = circumstances[1] + elements[1 + index] - obsvconst[3] - (elements[2 + index] - 30.0) / 3600.0;
            if (t < 0.0)
            {
                t = t + 24.0;
            }
            if (t >= 24.0)
            {
                t = t - 24.0;
            }
            if (t < 10.0)
            {
                ans = ans + "0";
            }
            ans = ans + Math.Floor(t) + ":";
            t = (t * 60.0) - 60.0 * Math.Floor(t);
            if (t < 10.0)
            {
                ans = ans + "0";
            }
            ans = ans + Math.Floor(t);
            if (circumstances[5] == 2)
            {
                return ans; //RETURNED IN ITAL DETERMINE WHY            
            }
            else
            {
                return ans;
            }
        }
        // Get the altitude
        private static string GetAlt(double[] circumstances)
        {
            double t;
            string ans = "";
            t = circumstances[4] * 180.0 / Math.PI;
            t = Math.Floor(t + 0.5);
            if (t < 0.0)
            {
                ans = "-";
                t = -t;
            }
            else
            {
                ans = "+";
            }
            if (t < 10.0)
            {
                ans = ans + "0";
            }
            ans = ans + t;
            if (circumstances[5] == 2)
            {
                return ans; //returned in italics determine why

            }
            else
            {
                return ans;
            }
        }
    }

}
