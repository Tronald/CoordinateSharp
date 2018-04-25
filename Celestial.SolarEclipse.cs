using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal class SolarEclipse
    {
        private double[] obsvconst = new double[6];
        private double[] mid = new double[40];//Check index to see if array needs to be this size
        private string[] month = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };//Month string array
        private List<string> values = new List<string>(); //Used to store values that would otherwise be printer. Covert to class later.
        private List<List<string>> events = new List<List<string>>(); //List of values list;

        public void Calculate(DateTime d, Coordinate coord)
        {
            //CONFIRM ELLIPSOID VALUES?
            double[] el = Eclipse.SolarData.SolarDateData(d);//Get 100 year solar data;
            ReadData(coord);
            #region
            //results = document.getElementById("el_results");
            //p = document.createElement("p");
            //p.setAttribute("id", "el_locationtable");
            //b = document.createElement("h2");
            //b.appendChild(document.createTextNode("Solar Eclipses visible from  " + document.eclipseform.loc_name.value));
            //p.appendChild(b);
            //resultsTable = document.createElement("table");
            //resultsTable.setAttribute("border", "0");
            //tbody = document.createElement("tbody");
            //row = document.createElement("tr");
            //td = document.createElement("td");
            //td.setAttribute("align", "right");
            //td.setAttribute("nowrap", "");
            //td.appendChild(document.createTextNode("Latitude: "));
            //row.appendChild(td);
            //td = document.createElement("td");
            //td.setAttribute("nowrap", "");
            //text = document.eclipseform.latd.value;
            //text += "\u00b0 ";
            //if (document.eclipseform.latm.value < 10) text += "0";
            //text += document.eclipseform.latm.value;
            //text += "' ";
            //if (document.eclipseform.lats.value < 10) text += "0";
            //text += document.eclipseform.lats.value;
            //text += '" ';
            //text += (document.eclipseform.latx.options[document.eclipseform.latx.selectedIndex]).text;
            //td.appendChild(document.createTextNode(text));
            //row.appendChild(td);
            //tbody.appendChild(row);
            //row = document.createElement("tr");
            //td = document.createElement("td");
            //td.setAttribute("align", "right");
            //td.setAttribute("nowrap", "");
            //td.appendChild(document.createTextNode("Longitude: "));
            //row.appendChild(td);
            //td = document.createElement("td");
            //td.setAttribute("nowrap", "");
            //text = document.eclipseform.lond.value;
            //text += "\u00b0 ";
            //if (document.eclipseform.lonm.value < 10) text += "0";
            //text += document.eclipseform.lonm.value;
            //text += "' ";
            //if (document.eclipseform.lons.value < 10) text += "0";
            //text += document.eclipseform.lons.value;
            //text += '" ';
            //text += (document.eclipseform.lonx.options[document.eclipseform.lonx.selectedIndex]).text;
            //td.appendChild(document.createTextNode(text));
            //row.appendChild(td);
            //tbody.appendChild(row);
            //row = document.createElement("tr");
            //td = document.createElement("td");
            //td.setAttribute("align", "right");
            //td.setAttribute("nowrap", "");
            //td.appendChild(document.createTextNode("Altitude: "));
            //row.appendChild(td);
            //td = document.createElement("td");
            //td.setAttribute("nowrap", "");
            //text = document.eclipseform.alt.value;
            //text += "m";
            //td.appendChild(document.createTextNode(text));
            //row.appendChild(td);
            //tbody.appendChild(row);
            //row = document.createElement("tr");
            //td = document.createElement("td");
            //td.setAttribute("align", "right");
            //td.setAttribute("nowrap", "");
            //td.appendChild(document.createTextNode("Time Zone: "));
            //row.appendChild(td);
            //td = document.createElement("td");
            //td.setAttribute("nowrap", "");
            //text = (document.eclipseform.tzh.options[document.eclipseform.tzh.selectedIndex]).text;
            //text += ":";
            //text += (document.eclipseform.tzm.options[document.eclipseform.tzm.selectedIndex]).text;
            //text += " ";
            //text += (document.eclipseform.tzx.options[document.eclipseform.tzx.selectedIndex]).text;
            //td.appendChild(document.createTextNode(text));
            //row.appendChild(td);
            //tbody.appendChild(row);
            //resultsTable.appendChild(tbody);
            //p.appendChild(resultsTable);
            //results.appendChild(p);

            //resultsTable = document.createElement("table");
            //resultsTable.setAttribute("id", "el_resultstable");
            //resultsTable.setAttribute("width", "150");
            //resultsTable.setAttribute("border", "2");
            //tbody = document.createElement("tbody");
            //row = document.createElement("tr");
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Calendar Date"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Eclipse Type"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Partial Eclipse Begins"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Sun Alt"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("A or T Eclipse Begins"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Maximum Eclipse"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Sun Alt"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Sun Azi"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("A or T Eclipse Ends"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Partial Eclipse Ends"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Sun Alt"));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Eclipse Mag."));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("Eclipse Obs."));
            //row.appendChild(td);
            //td = document.createElement("th");
            //td.appendChild(document.createTextNode("A or T Eclipse Duration"));
            //row.appendChild(td);
            //tbody.appendChild(row);
            #endregion
            for (int i = 0; i < el.Length; i += 28)
            {
                obsvconst[6] = i;
                GetAll(el);
                // Is there an event...
                if (mid[39] > 0)
                {
                    values = new List<string>();
                    values.Add(GetDate(el, mid));

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
                        values.Add(GetTime(el, c1));
                        values.Add(GetAlt(c1));
                    }
                    // Central eclipse time                
                    if ((mid[39] > 1) && (c2[40] != 4))
                    {
                        values.Add(GetTime(el, c2));
                    }
                    else
                    {
                        values.Add("-");
                    }

                    //Mid Time
                    values.Add(GetTime(el, mid));

                    // Maximum eclipse alt                  
                    values.Add(GetAlt(mid));

                    // Maximum eclipse azi                    
                    values.Add(GetAzi(mid));
                    // Central eclipse ends
                    if ((mid[39] > 1) && (c3[40] != 4))
                    {
                        values.Add(GetTime(el, c3));
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
                        values.Add(GetTime(el, c4));

                        // ... sun alt
                        values.Add(GetAlt(c4));
                    }
                    // Eclipse magnitude
                    GetMagnitude());

            // Coverage                
            GetCoverage();

            // Central duration                   
            if (mid[39] > 1)
            {
                values.Add(GetDuration());
            }
            else
            {
                values.Add("-");
            }
            events.Add(values);
        }

        //Populates the obsvcont array
        private void ReadData(Coordinate coord)
        {
            // Get the latitude
            obsvconst[0] = coord.Latitude.ToRadians();

            //// Get the longitude
            obsvconst[1] = coord.Longitude.ToRadians();

            // Get the altitude
            obsvconst[2] = 1000; //CHANGE TO ALLOW USER TO PASS.

            // Get the time zone
            obsvconst[3] = 0; //ALWAYS GMT

            // Get the observer's geocentric position
            double tmp = Math.Atan(0.99664719 * Math.Tan(obsvconst[0]));
            obsvconst[4] = 0.99664719 * Math.Sin(tmp) + (obsvconst[2] / 6378140.0) * Math.Sin(obsvconst[0]);
            obsvconst[5] = Math.Cos(tmp) + (obsvconst[2] / 6378140.0 * Math.Cos(obsvconst[0]));
        }
        // Populate the c1, c2, mid, c3 and c4 arrays
        private void GetAll(double[] elements)
        {
            GetMid(elements);
            MidObservational();
            if (mid[37] > 0.0)
            {
                getc1c4(elements);
                if ((mid[36] < mid[29]) || (mid[36] < -mid[29]))
                {
                    getc2c3(elements);
                    if (mid[29] < 0.0)
                    {
                        mid[39] = 3; // Total eclipse
                    }
                    else
                    {
                        mid[39] = 2; // Annular eclipse
                    }
                    Observational(c1);
                    Observational(c2);
                    Observational(c3);
                    Observational(c4);
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
                        GetSunset(elements, c4);
                        Observational(c4);
                        c4[40] = 3;
                    }
                    else if (pattern == 11100)
                    {
                        GetSunset(elements, c3);
                        Observational(c3);
                        c3[40] = 3;
                        CopyCircumstances(c3, c4);
                    }
                    else if (pattern == 11000)
                    {
                        c3[40] = 4;
                        GetSunset(elements, mid);
                        MidObservational();
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 10000)
                    {
                        mid[39] = 1;
                        GetSunset(elements, mid);
                        MidObservational();
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 1111)
                    {
                        GetSunrise(elements, c1);
                        Observational(c1);
                        c1[40] = 2;
                    }
                    else if (pattern == 111)
                    {
                        GetSunrise(elements, c2);
                        Observational(c2);
                        c2[40] = 2;
                        copycircumstances(c2, c1);
                    }
                    else if (pattern == 11)
                    {
                        c2[40] = 4;
                        GetSunrise(elements, mid);
                        MidObservational();
                        mid[40] = 2;
                        copycircumstances(mid, c1);
                    }
                    else if (pattern == 1)
                    {
                        mid[39] = 1;
                        GetSunrise(elements, mid);
                        MidObservational();
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
                    Observational(c1);
                    Observational(c4);
                    if (c1[40] == 0) { pattern += 100; }
                    if (mid[40] == 0) { pattern += 10; }
                    if (c4[40] == 0) { pattern += 1; }
                    if (pattern == 110)
                    {
                        GetSunset(elements, c4);
                        Observational(c4);
                        c4[40] = 3;
                    }
                    else if (pattern == 100)
                    {
                        GetSunset(elements, mid);
                        MidObservational();
                        mid[40] = 3;
                        CopyCircumstances(mid, c4);
                    }
                    else if (pattern == 11)
                    {
                        GetSunrise(elements, c1);
                        Observational(c1);
                        c1[40] = 2;
                    }
                    else if (pattern == 1)
                    {
                        GetSunrise(elements, mid);
                        MidObservational();
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
        private void GetMid(double[] elements)
        {
            double iter, tmp;

            mid[0] = 0;
            mid[1] = 0.0;
            iter = 0;
            tmp = 1.0;
            TimeLocDependent(elements, mid);
            while (((tmp > 0.000001) || (tmp < -0.000001)) && (iter < 50))
            {
                tmp = (mid[24] * mid[26] + mid[25] * mid[27]) / mid[30];
                mid[1] = mid[1] - tmp;
                iter++;
                TimeLocDependent(elements, mid);
            }
        }
        // Populate the circumstances array with the time and location dependent circumstances
        private double[] TimeLocDependent(double[] elements, double[] circumstances)
        {
            double index, type;

            TimeDependent(elements, circumstances);
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
        private double[] TimeDependent(double[] elements, double[] circumstances)
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

        // Get the date of an event
        private string GetDate(double[] elements, double[] circumstances)
        {
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
            if (e > 2.5)
            {
                ans = c - 4716 + "-";
            }
            else
            {
                ans = c - 4715 + "-";
            }
            ans += month[(int)e - 1] + "-";
            if (d < 10)
            {
                ans = ans + "0";
            }
            ans = ans + d;
            return ans;
        }
    }
}
