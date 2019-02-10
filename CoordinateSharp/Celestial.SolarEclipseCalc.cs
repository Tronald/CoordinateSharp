using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    internal class SolarEclipseCalc
    {                             
        public static List<List<string>> CalculateSolarEclipse(DateTime d, double latRad, double longRad)
        {
            return Calculate(d, latRad, longRad, null);
        }
        public static List<SolarEclipseDetails> CalculateSolarEclipse(DateTime d, double latRad, double longRad, double[] events)
        {
            List<List<string>> evs = Calculate(d, latRad, longRad, events);
            List<SolarEclipseDetails> deetsList = new List<SolarEclipseDetails>();
            foreach(List<string> ls in evs)
            {
                SolarEclipseDetails deets = new SolarEclipseDetails(ls);
                deetsList.Add(deets);
            }
            return deetsList;
        }
        public static List<List<string>> CalculateSolarEclipse(DateTime d, Coordinate coord)
        {
            return Calculate(d, coord.Latitude.ToRadians(), coord.Longitude.ToRadians(), null);
        }

        private static List<List<string>> Calculate(DateTime d, double latRad, double longRad, double[] ev)
        {
            //Declare storage arrays
            double[] obsvconst = new double[7];
            double[] mid = new double[41];//Check index to see if array needs to be this size
            double[] c1 = new double[41];
            double[] c2 = new double[41];
            double[] c3 = new double[41];
            double[] c4 = new double[41];

        List<List<string>> events = new List<List<string>>();
            double[] el;
            if (ev == null)
            {
                el = Eclipse.SolarData.SolarDateData(d);//Get 100 year solar data;
            }
            else
            {
                el = ev;
            }
           
            events = new List<List<string>>();
            ReadData(latRad, longRad, obsvconst);
            for (int i = 0; i < el.Length; i += 28)
            {
                obsvconst[6] = i;
                GetAll(el, obsvconst, mid,c1,c2,c3,c4);
                // Is there an event...
                if (mid[39] > 0)
                {
                    List<string> values = new List<string>();             
                    values.Add(GetDate(el, mid, obsvconst));
                    if (mid[39] == 1)
                    {
                        values.Add("P");
                    }
                    else if (mid[39] == 2)
                    {
                        values.Add("A");
                    }
                    else
                    {
                        values.Add("T");
                    }

                    // Partial eclipse start
                    if (c1[40] == 4)
                    {
                        values.Add("-");
                        values.Add(" ");
                    }
                    else
                    {
                        // Partial eclipse start time
                        values.Add(GetTime(el, c1, obsvconst));
                        values.Add(GetAlt(c1));
                    }
                    // Central eclipse time                
                    if ((mid[39] > 1) && (c2[40] != 4))
                    {
                        values.Add(GetTime(el, c2, obsvconst));
                    }
                    else
                    {
                        values.Add("-");
                    }

                    //Mid Time
                    values.Add(GetTime(el, mid, obsvconst));

                    // Maximum eclipse alt                  
                    values.Add(GetAlt(mid));

                    // Maximum eclipse azi                    
                    values.Add(GetAzi(mid));
                    // Central eclipse ends
                    if ((mid[39] > 1) && (c3[40] != 4))
                    {
                        values.Add(GetTime(el, c3, obsvconst));
                    }
                    else
                    {
                        values.Add("-");
                    }
                    // Partial eclipse ends
                    if (c4[40] == 4)
                    {
                        values.Add("-");
                        values.Add(" ");
                    }
                    else
                    {
                        // Partial eclipse ends
                        values.Add(GetTime(el, c4, obsvconst));

                        // ... sun alt
                        values.Add(GetAlt(c4));
                    }
                    // Eclipse magnitude
                    values.Add(GetMagnitude(mid));


                    // Coverage                
                    values.Add(GetCoverage(mid));

                    // Central duration                   
                    if (mid[39] > 1)
                    {
                        values.Add(GetDuration(mid,c2,c3));
                    }
                    else
                    {
                        values.Add("-");
                    }
                   
                    events.Add(values);
                }
            }
            return events;
        }
        //Populates the obsvcont array
        private static  void ReadData(double latRad, double longRad, double[] obsvconst)
        {
            // Get the latitude
            obsvconst[0] = latRad;

            //// Get the longitude
            obsvconst[1] = -1 * longRad; //PASS REVERSE RADIAN.

            // Get the altitude
            obsvconst[2] = 100; //CHANGE TO ALLOW USER TO PASS.

            // Get the time zone
            obsvconst[3] = 0; //ALWAYS GMT

            // Get the observer's geocentric position
            double tmp = Math.Atan(0.99664719 * Math.Tan(obsvconst[0]));
            obsvconst[4] = 0.99664719 * Math.Sin(tmp) + (obsvconst[2] / 6378140.0) * Math.Sin(obsvconst[0]);
            obsvconst[5] = Math.Cos(tmp) + (obsvconst[2] / 6378140.0 * Math.Cos(obsvconst[0]));
          
        }
        // Populate the c1, c2, mid, c3 and c4 arrays
        private static  void GetAll(double[] elements, double[] obsvconst, double[] mid, double[] c1, double[] c2,double[] c3, double[] c4)
        {
            GetMid(elements, obsvconst, mid);
            MidObservational(obsvconst, mid);
            if (mid[37] > 0.0)
            {
                Getc1c4(elements, obsvconst, mid,c1,c2,c3,c4);
                if ((mid[36] < mid[29]) || (mid[36] < -mid[29]))
                {
                    Getc2c3(elements, obsvconst, mid,c2,c3);
                    if (mid[29] < 0.0)
                    {
                        mid[39] = 3; // Total eclipse
                    }
                    else
                    {
                        mid[39] = 2; // Annular eclipse
                    }
                    Observational(c1, obsvconst, mid);
                    Observational(c2, obsvconst, mid);
                    Observational(c3, obsvconst, mid);
                    Observational(c4, obsvconst, mid);
                    c2[36] = 999.9;
                    c3[36] = 999.9;
                    // Calculate how much of the eclipse is above the horizon
                    double pattern = 0;
                    if (c1[40] == 0) { pattern += 10000; }
                    if (c2[40] == 0) { pattern += 1000; }
                    if (mid[40] == 0) { pattern += 100; }
                    if (c3[40] == 0) { pattern += 10; }
                    if (c4[40] == 0) { pattern += 1; }
                    // Now, time to make sure that all my Observational[39] and Observational[40] are OK
                    if (pattern == 11110)
                    {
                        GetSunset(elements, c4, obsvconst);
                        Observational(c4, obsvconst, mid);
                        c4[40] = 3;
                    }
                    else if (pattern == 11100)
                    {
                        GetSunset(elements, c3, obsvconst);
                        Observational(c3, obsvconst, mid);
                        c3[40] = 3;
                        CopyCircumstances(c3, c4);
                    }
                    else if (pattern == 11000)
                    {
                        c3[40] = 4;
                        GetSunset(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 10000)
                    {
                        mid[39] = 1;
                        GetSunset(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 1111)
                    {
                        GetSunrise(elements, c1, obsvconst);
                        Observational(c1, obsvconst, mid);
                        c1[40] = 2;
                    }
                    else if (pattern == 111)
                    {
                        GetSunrise(elements, c2, obsvconst);
                        Observational(c2, obsvconst, mid);
                        c2[40] = 2;
                        CopyCircumstances(c2, c1);
                    }
                    else if (pattern == 11)
                    {
                        c2[40] = 4;
                        GetSunrise(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 2;
                        CopyCircumstances(mid, c1);
                    }
                    else if (pattern == 1)
                    {
                        mid[39] = 1;
                        GetSunrise(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 2;
                        CopyCircumstances(mid, c1);
                    }
                    else if (pattern == 0)
                    {
                        mid[39] = 0;
                    }
                    // There are other patterns, but those are the only ones we're covering!
                }
                else
                {
                    mid[39] = 1; // Partial eclipse
                    double pattern = 0;
                    Observational(c1, obsvconst, mid);
                    Observational(c4, obsvconst, mid);
                    if (c1[40] == 0) { pattern += 100; }
                    if (mid[40] == 0) { pattern += 10; }
                    if (c4[40] == 0) { pattern += 1; }
                    if (pattern == 110)
                    {
                        GetSunset(elements, c4, obsvconst);
                        Observational(c4, obsvconst, mid);
                        c4[40] = 3;
                    }
                    else if (pattern == 100)
                    {
                        GetSunset(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 11)
                    {
                        GetSunrise(elements, c1, obsvconst);
                        Observational(c1, obsvconst, mid);
                        c1[40] = 2;
                    }
                    else if (pattern == 1)
                    {
                        GetSunrise(elements, mid, obsvconst);
                        MidObservational(obsvconst, mid);
                        mid[40] = 2;
                        CopyCircumstances(mid, c1);
                    }
                    else if (pattern == 0)
                    {
                        mid[39] = 0;
                    }
                    // There are other patterns, but those are the only ones we're covering!
                }
            }
            else
            {
                mid[39] = 0; // No eclipse
            }
            // Magnitude for total and annular eclipse is moon/sun ratio
            if ((mid[39] == 2) || (mid[39] == 3))
            {
                mid[37] = mid[38];
            }
        }
        // Calculate mid eclipse
        private static  void GetMid(double[] elements, double[] obsvconst, double[] mid)
        {
            double iter, tmp;

            mid[0] = 0;
            mid[1] = 0.0;
            iter = 0;
            tmp = 1.0;
            TimeLocDependent(elements, mid, obsvconst);
            while (((tmp > 0.000001) || (tmp < -0.000001)) && (iter < 50))
            {
                tmp = (mid[24] * mid[26] + mid[25] * mid[27]) / mid[30];
                mid[1] = mid[1] - tmp;
                iter++;
                TimeLocDependent(elements, mid, obsvconst);
            }
        }
        // Populate the circumstances array with the time and location dependent circumstances
        private static  double[] TimeLocDependent(double[] elements, double[] circumstances, double[] obsvconst)
        {
            double index, type;

            TimeDependent(elements, circumstances, obsvconst);
            index = obsvconst[6];
            // Calculate h, sin h, cos h
            circumstances[16] = circumstances[7] - obsvconst[1] - (elements[(int)index + 5] / 13713.44);
            circumstances[17] = Math.Sin(circumstances[16]);
            circumstances[18] = Math.Cos(circumstances[16]);
            // Calculate xi
            circumstances[19] = obsvconst[5] * circumstances[17];
            // Calculate eta
            circumstances[20] = obsvconst[4] * circumstances[6] - obsvconst[5] * circumstances[18] * circumstances[5];
            // Calculate zeta
            circumstances[21] = obsvconst[4] * circumstances[5] + obsvconst[5] * circumstances[18] * circumstances[6];
            // Calculate dxi
            circumstances[22] = circumstances[13] * obsvconst[5] * circumstances[18];
            // Calculate deta
            circumstances[23] = circumstances[13] * circumstances[19] * circumstances[5] - circumstances[21] * circumstances[12];
            // Calculate u
            circumstances[24] = circumstances[2] - circumstances[19];
            // Calculate v
            circumstances[25] = circumstances[3] - circumstances[20];
            // Calculate a
            circumstances[26] = circumstances[10] - circumstances[22];
            // Calculate b
            circumstances[27] = circumstances[11] - circumstances[23];
            // Calculate l1'
            type = circumstances[0];
            if ((type == -2) || (type == 0) || (type == 2))
            {
                circumstances[28] = circumstances[8] - circumstances[21] * elements[26 + (int)index];
            }
            // Calculate l2'
            if ((type == -1) || (type == 0) || (type == 1))
            {
                circumstances[29] = circumstances[9] - circumstances[21] * elements[27 + (int)index];
            }
            // Calculate n^2
            circumstances[30] = circumstances[26] * circumstances[26] + circumstances[27] * circumstances[27];
            return circumstances;
        }
        // Populate the circumstances array with the time-only dependent circumstances (x, y, d, m, ...)
        private static  double[] TimeDependent(double[] elements, double[] circumstances, double[] obsvconst)
        {
            double type, t, ans;

            t = circumstances[1];
            int index = (int)obsvconst[6];
            // Calculate x
            ans = elements[9 + index] * t + elements[8 + index];
            ans = ans * t + elements[7 + index];
            ans = ans * t + elements[6 + index];
            circumstances[2] = ans;
            // Calculate dx
            ans = 3.0 * elements[9 + index] * t + 2.0 * elements[8 + index];
            ans = ans * t + elements[7 + index];
            circumstances[10] = ans;
            // Calculate y
            ans = elements[13 + index] * t + elements[12 + index];
            ans = ans * t + elements[11 + index];
            ans = ans * t + elements[10 + index];
            circumstances[3] = ans;
            // Calculate dy
            ans = 3.0 * elements[13 + index] * t + 2.0 * elements[12 + index];
            ans = ans * t + elements[11 + index];
            circumstances[11] = ans;
            // Calculate d
            ans = elements[16 + index] * t + elements[15 + index];
            ans = ans * t + elements[14 + index];
            ans = ans * Math.PI / 180.0;
            circumstances[4] = ans;
            // sin d and cos d
            circumstances[5] = Math.Sin(ans);
            circumstances[6] = Math.Cos(ans);
            // Calculate dd
            ans = 2.0 * elements[16 + index] * t + elements[15 + index];
            ans = ans * Math.PI / 180.0;
            circumstances[12] = ans;
            // Calculate m
            ans = elements[19 + index] * t + elements[18 + index];
            ans = ans * t + elements[17 + index];
            if (ans >= 360.0)
            {
                ans = ans - 360.0;
            }
            ans = ans * Math.PI / 180.0;
            circumstances[7] = ans;
            // Calculate dm
            ans = 2.0 * elements[19 + index] * t + elements[18 + index];
            ans = ans * Math.PI / 180.0;
            circumstances[13] = ans;
            // Calculate l1 and dl1
            type = circumstances[0];
            if ((type == -2) || (type == 0) || (type == 2))
            {
                ans = elements[22 + index] * t + elements[21 + index];
                ans = ans * t + elements[20 + index];
                circumstances[8] = ans;
                circumstances[14] = 2.0 * elements[22 + index] * t + elements[21 + index];
            }
            // Calculate l2 and dl2
            if ((type == -1) || (type == 0) || (type == 1))
            {
                ans = elements[25 + index] * t + elements[24 + index];
                ans = ans * t + elements[23 + index];
                circumstances[9] = ans;
                circumstances[15] = 2.0 * elements[25 + index] * t + elements[24 + index];
            }
            return circumstances;
        }
        // Get the observational circumstances for mid eclipse
        private static  void MidObservational(double[] obsvconst, double[] mid)
        {
            Observational(mid, obsvconst, mid);
            // Calculate m, magnitude and moon/sun
            mid[36] = Math.Sqrt(mid[24] * mid[24] + mid[25] * mid[25]);
            mid[37] = (mid[28] - mid[36]) / (mid[28] + mid[29]);
            mid[38] = (mid[28] - mid[29]) / (mid[28] + mid[29]);
        }
        // Get the observational circumstances
        private static  void Observational(double[] circumstances, double[] obsvconst, double[] mid)
        {
            double contacttype, coslat, sinlat;

            // We are looking at an "external" contact UNLESS this is a total eclipse AND we are looking at
            // c2 or c3, in which case it is an INTERNAL contact! Note that if we are looking at mid eclipse,
            // then we may not have determined the type of eclipse (mid[39]) just yet!
            if (circumstances[0] == 0)
            {
                contacttype = 1.0;
            }
            else
            {
                if ((mid[39] == 3) && ((circumstances[0] == -1) || (circumstances[0] == 1)))
                {
                    contacttype = -1.0;
                }
                else
                {
                    contacttype = 1.0;
                }
            }
            // Calculate p
            circumstances[31] = Math.Atan2(contacttype * circumstances[24], contacttype * circumstances[25]);
            // Calculate alt
            sinlat = Math.Sin(obsvconst[0]);
            coslat = Math.Cos(obsvconst[0]);
            circumstances[32] = Math.Asin(circumstances[5] * sinlat + circumstances[6] * coslat * circumstances[18]);
            // Calculate q
            circumstances[33] = Math.Asin(coslat * circumstances[17] / Math.Cos(circumstances[32]));
            if (circumstances[20] < 0.0)
            {
                circumstances[33] = Math.PI - circumstances[33];
            }
            // Calculate v
            circumstances[34] = circumstances[31] - circumstances[33];
            // Calculate azi
            circumstances[35] = Math.Atan2(-1.0 * circumstances[17] * circumstances[6], circumstances[5] * coslat - circumstances[18] * sinlat * circumstances[6]);
            // Calculate visibility
            if (circumstances[32] > -0.00524)
            {
                circumstances[40] = 0;
            }
            else
            {
                circumstances[40] = 1;
            }
        }
        // Get C1 and C4 data
        //   Entry conditions -
        //   1. The mid array must be populated
        //   2. The magnitude at mid eclipse must be > 0.0
        private static  void Getc1c4(double[] elements, double[] obsvconst, double[] mid, double[] c1, double[] c2, double[] c3, double[] c4)
        {
            double tmp, n;

            n = Math.Sqrt(mid[30]);
            tmp = mid[26] * mid[25] - mid[24] * mid[27];
            tmp = tmp / n / mid[28];
            tmp = Math.Sqrt(1.0 - tmp * tmp) * mid[28] / n;
            c1[0] = -2;
            c4[0] = 2;
            c1[1] = mid[1] - tmp;
            c4[1] = mid[1] + tmp;
            c1c4iterate(elements, c1, obsvconst);
            c1c4iterate(elements, c4, obsvconst);
        }
        // Iterate on C1 or C4
        private static  double[] c1c4iterate(double[] elements, double[] circumstances, double[] obsvconst)
        {
            double sign, iter, tmp, n;

            TimeLocDependent(elements, circumstances, obsvconst);
            if (circumstances[0] < 0)
            {
                sign = -1.0;
            }
            else
            {
                sign = 1.0;
            }
            tmp = 1.0;
            iter = 0;
            while (((tmp > 0.000001) || (tmp < -0.000001)) && (iter < 50))
            {
                n = Math.Sqrt(circumstances[30]);
                tmp = circumstances[26] * circumstances[25] - circumstances[24] * circumstances[27];
                tmp = tmp / n / circumstances[28];
                tmp = sign * Math.Sqrt(1.0 - tmp * tmp) * circumstances[28] / n;
                tmp = (circumstances[24] * circumstances[26] + circumstances[25] * circumstances[27]) / circumstances[30] - tmp;
                circumstances[1] = circumstances[1] - tmp;
                TimeLocDependent(elements, circumstances, obsvconst);
                iter++;
            }
            return circumstances;
        }
        // Get C2 and C3 data
        //   Entry conditions -
        //   1. The mid array must be populated
        //   2. There must be either a total or annular eclipse at the location!
        private static void Getc2c3(double[] elements, double[] obsvconst, double[] mid, double[] c2, double[] c3)
        {
            double tmp, n;

            n = Math.Sqrt(mid[30]);
            tmp = mid[26] * mid[25] - mid[24] * mid[27];
            tmp = tmp / n / mid[29];
            tmp = Math.Sqrt(1.0 - tmp * tmp) * mid[29] / n;
            c2[0] = -1;
            c3[0] = 1;
            if (mid[29] < 0.0)
            {
                c2[1] = mid[1] + tmp;
                c3[1] = mid[1] - tmp;
            }
            else
            {
                c2[1] = mid[1] - tmp;
                c3[1] = mid[1] + tmp;
            }
            c2c3iterate(elements, c2, obsvconst, mid);
            c2c3iterate(elements, c3, obsvconst, mid);
        }
        // Iterate on C2 or C3
        private static  double[] c2c3iterate(double[] elements, double[] circumstances, double[] obsvconst, double[] mid)
        {
            double sign, iter, tmp, n;

            TimeLocDependent(elements, circumstances, obsvconst);
            if (circumstances[0] < 0)
            {
                sign = -1.0;
            }
            else
            {
                sign = 1.0;
            }
            if (mid[29] < 0.0)
            {
                sign = -sign;
            }
            tmp = 1.0;
            iter = 0;
            while (((tmp > 0.000001) || (tmp < -0.000001)) && (iter < 50))
            {
                n = Math.Sqrt(circumstances[30]);
                tmp = circumstances[26] * circumstances[25] - circumstances[24] * circumstances[27];
                tmp = tmp / n / circumstances[29];
                tmp = sign * Math.Sqrt(1.0 - tmp * tmp) * circumstances[29] / n;
                tmp = (circumstances[24] * circumstances[26] + circumstances[25] * circumstances[27]) / circumstances[30] - tmp;
                circumstances[1] = circumstances[1] - tmp;
                TimeLocDependent(elements, circumstances, obsvconst);
                iter++;
            }
            return circumstances;
        }
        // Get the date of an event
        private static string GetDate(double[] elements, double[] circumstances, double[] obsvconst)
        {
            string[] month = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            double t, jd, a, b, c, d, e, index;
            string ans = "";
            index = obsvconst[6];
            // Calculate the JD for noon (TDT) the day before the day that contains T0
            jd = Math.Floor(elements[(int)index] - (elements[1 + (int)index] / 24.0));
            // Calculate the local time (ie the offset in hours since midnight TDT on the day containing T0).
            t = circumstances[1] + elements[1 + (int)index] - obsvconst[3] - (elements[4 + (int)index] - 0.5) / 3600.0;
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
            ans += m + "-";
            if (d < 10)
            {
                ans = ans + "0";
            }
           
            ans = ans + d;
            //Leap Year Integrity Check

            if(m  =="Feb" && d ==29 && !DateTime.IsLeapYear((int)year))
            {
                ans = year.ToString() + "-Mar-01";
            }
            return ans;
        }
        // Calculate the time of sunset
        private static  void GetSunset(double[] elements, double[] circumstances, double[] obsvconst)
        {
            GetSunriset(elements, circumstances, 1.0, obsvconst);
        }
        // Calculate the time of sunrise
        private static  void GetSunrise(double[] elements, double[] circumstances, double[] obsvconst)
        {
            GetSunriset(elements, circumstances, -1.0, obsvconst);
        }
        // Calculate the time of sunrise or sunset
        private static  void GetSunriset(double[] elements, double[] circumstances, double riset, double[] obsvconst)
        {
            double h0, diff, iter;

            diff = 1.0;
            iter = 0;
            while ((diff > 0.00001) || (diff < -0.00001))
            {
                iter++;
                if (iter == 4) { return; }
                h0 = Math.Acos((Math.Sin(-0.00524) - Math.Sin(obsvconst[0]) * circumstances[5]) / Math.Cos(obsvconst[0]) / circumstances[6]);
                diff = (riset * h0 - circumstances[16]) / circumstances[13];
                while (diff >= 12.0) { diff -= 24.0; }
                while (diff <= -12.0) { diff += 24.0; }
                circumstances[1] += diff;
                TimeLocDependent(elements, circumstances, obsvconst);
            }
        }
        // Copy a set of circumstances
        private static  void CopyCircumstances(double[] circumstancesfrom, double[] circumstancesto)
        {
            for (int i = 1; i < 41; i++)
            {
                circumstancesto[i] = circumstancesfrom[i];
            }
        }
        // Get the local time of an event
        private static  string GetTime(double[] elements, double[] circumstances, double[] obsvconst)
        {
            string ans = "";
            int index = (int)obsvconst[6];
            double t = circumstances[1] + elements[1 + index] - obsvconst[3] - (elements[4 + index] - 0.5) / 3600.0;
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
            if (circumstances[40] <= 1)
            { // not sunrise or sunset
                ans = ans + ":";
                t = (t * 60.0) - 60.0 * Math.Floor(t);
                if (t < 10.0)
                {
                    ans = ans + "0";
                }
                ans = ans + Math.Floor(t);
            }
            if (circumstances[40] == 1)
            {
                //WAS ITALIC
                return ans;
            }
            else if (circumstances[40] == 2)
            {
                //Rise (CHANGED FROM NASA CALC THE INDICATES (r) WITH STRING, INVESTIGATE REMOVAL)
                return ans;
            }
            else if (circumstances[40] == 3)
            {
                //Set (CHANGED FROM NASA CALC THE INDICATES (s) WITH STRING, INVESTIGATE REMOVAL)
                return ans;
            }
            else
            {
                return ans;
            }
        }
        // Get the altitude
        private static  string GetAlt(double[] circumstances)
        {
            double t;
            string ans = "";

            if (circumstances[40] == 2)
            {
                return "0(r)";
            }
            if (circumstances[40] == 3)
            {
                return "0(s)";
            }
            if ((circumstances[32] < 0.0) && (circumstances[32] >= -0.00524))
            {
                // Crude correction for refraction (and for consistency's sake)
                t = 0.0;
            }
            else
            {
                t = circumstances[32] * 180.0 / Math.PI;
            }
            if (t < 0.0)
            {
                ans = "-";
                t = -t;
            }
            else
            {
                ans = "";
            }
            t = Math.Floor(t + 0.5);
            if (t < 10.0)
            {
                ans = ans + "0";
            }
            ans = ans + t;
            if (circumstances[40] == 1)
            {
                //WAS ITALIC
                return ans;
            }
            else
            {
                return ans;
            }
        }
        // Get the azimuth
        private static  string GetAzi(double[] circumstances)
        {
            string ans = "";
            double t = circumstances[35] * 180.0 / Math.PI;
            if (t < 0.0)
            {
                t = t + 360.0;
            }
            if (t >= 360.0)
            {
                t = t - 360.0;
            }
            t = Math.Floor(t + 0.5);
            if (t < 100.0)
            {
                ans = ans + "0";
            }
            if (t < 10.0)
            {
                ans = ans + "0";
            }
            ans = ans + t;
            if (circumstances[40] == 1)
            {
                //WAS ITALIC
                return ans;
            }
            else
            {
                return ans;
            }
        }
        // Get the magnitude
        private static  string GetMagnitude(double[] mid)
        {
            double a = Math.Floor(1000.0 * mid[37] + 0.5) / 1000.0;
            string ans = a.ToString();
            if (mid[40] == 1)
            {
                return ans;
            }
            if (mid[40] == 2)
            {
                ans = a.ToString() + "(r)";
            }
            if (mid[40] == 3)
            {
                ans = a.ToString() + "(s)";
            }
            return ans;
        }
        // Get the coverage
        private static  string GetCoverage(double[] mid)
        {
            double a=0, b, c;
            string ans = "";
            if (mid[37] <= 0.0)
            {
                ans = "0.0";
            }
            else if (mid[37] >= 1.0)
            {
                ans = "1.000";
            }
            else
            {
                if (mid[39] == 2)
                {
                    c = mid[38] * mid[38];
                }
                else
                {
                    c = Math.Acos((mid[28] * mid[28] + mid[29] * mid[29] - 2.0 * mid[36] * mid[36]) / (mid[28] * mid[28] - mid[29] * mid[29]));
                    b = Math.Acos((mid[28] * mid[29] + mid[36] * mid[36]) / mid[36] / (mid[28] + mid[29]));
                    a = Math.PI - b - c;
                    c = ((mid[38] * mid[38] * a + b) - mid[38] * Math.Sin(c)) / Math.PI;
                }
                a = Math.Floor(1000.0 * c + 0.5) / 1000.0;
                ans = a.ToString();
            }
            if (mid[40] == 1)
            {
                //WAS ITALIC
                return ans;
            }
            if (mid[40] == 2)
            {
                ans = a.ToString() + "(r)";
            }
            if (mid[40] == 3)
            {
                ans = a + "(s)";
            }
            return ans;
        }
        // Get the duration in mm:ss.s format
        // Adapted from code written by Stephen McCann - 27/04/2001
        private static  string GetDuration(double[] mid, double[] c2, double[] c3)
        {
            double tmp;
            string ans;

            if (c3[40] == 4)
            {
                tmp = mid[1] - c2[1];
            }
            else if (c2[40] == 4)
            {
                tmp = c3[1] - mid[1];
            }
            else
            {
                tmp = c3[1] - c2[1];
            }
            if (tmp < 0.0)
            {
                tmp = tmp + 24.0;
            }
            else if (tmp >= 24.0)
            {
                tmp = tmp - 24.0;
            }
            tmp = (tmp * 60.0) - 60.0 * Math.Floor(tmp) + 0.05 / 60.0;
            ans = Math.Floor(tmp) + "m";
            tmp = (tmp * 60.0) - 60.0 * Math.Floor(tmp);
            if (tmp < 10.0)
            {
                ans = ans + "0";
            }
            ans += Math.Floor(tmp) + "s";
            return ans;
        }
    }
}
