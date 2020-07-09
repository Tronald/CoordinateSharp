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
using System.Reflection;
namespace CoordinateSharp
{
    /// <summary>
    /// Class for storing additional solar time information.
    /// </summary>
    [Serializable]
    public class AdditionalSolarTimes
    {
        internal DateTime? civilDawn;
        internal DateTime? civilDusk;
        internal DateTime? nauticalDawn;
        internal DateTime? nauticalDusk;
        internal DateTime? astronomicalDawn;
        internal DateTime? astronomicalDusk;
        internal DateTime? sunriseBottomDisc;
        internal DateTime? sunsetBottomDisc;

        /// <summary>
        /// Create a default AdditionalSolarTimes object.
        /// </summary>
        public AdditionalSolarTimes()
        {
            //Set dates to avoid null errors. If year return 1900 event did not occur.
            civilDawn = new DateTime();
            civilDusk = new DateTime();
            nauticalDawn = new DateTime();
            nauticalDusk = new DateTime();

        }

        /// <summary>
        /// Civil Dawn Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? CivilDawn { get { return civilDawn; } }
        /// <summary>
        /// Civil Dusk Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? CivilDusk { get { return civilDusk; } }
        /// <summary>
        /// Nautical Dawn Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? NauticalDawn { get { return nauticalDawn; } }
        /// <summary>
        /// Nautical Dusk Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? NauticalDusk { get { return nauticalDusk; } }
        /// <summary>
        /// Astronomical Dawn Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? AstronomicalDawn { get { return astronomicalDawn; } }
        /// <summary>
        /// Astronomical Dusk Time.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? AstronomicalDusk { get { return astronomicalDusk; } }

        /// <summary>
        /// DateTime when the bottom of the solar disc touches the horizon after a sunrise event.
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? SunriseBottomDisc { get { return sunriseBottomDisc; } }
        /// <summary>
        /// DateTime when the bottom of the solar disc touches the horizon before sunset
        /// </summary>
        /// <remarks>
        /// DateTime will be null if event does not occur.
        /// </remarks>
        public DateTime? SunsetBottomDisc { get { return sunsetBottomDisc; } }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(AdditionalSolarTimes).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime?))
                {
                    DateTime? d = (DateTime?)field.GetValue(this);
                    if (d.HasValue)
                    {
                        if (d > new DateTime())
                        {
                            d = d.Value.AddHours(offset);
                            field.SetValue(this, d);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Class for storing last and next solar eclipse information at a specified DateTime and Coordinate.
    /// </summary>
    [Serializable]
    public class SolarEclipse
    {
        internal SolarEclipseDetails lastEclipse;
        internal SolarEclipseDetails nextEclipse;

        /// <summary>
        /// Initialize a SolarEclipse object.
        /// </summary>
        public SolarEclipse()
        {
            lastEclipse = new SolarEclipseDetails();
            nextEclipse = new SolarEclipseDetails();
        }
        /// <summary>
        /// Details about the previous solar eclipse at the specified DateTime and Coordinate.
        /// </summary>
        public SolarEclipseDetails LastEclipse { get { return lastEclipse; } }
        /// <summary>
        /// Details about the next solar eclipse at the specified DateTime and Coordinate.
        /// </summary>
        public SolarEclipseDetails NextEclipse { get { return nextEclipse; } }

        internal void ConvertTo_LocalTime(double offset)
        {
            LastEclipse.Convert_To_Local_Time(offset);
            NextEclipse.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class containing detailed solar eclipse information.
    /// </summary>
    [Serializable]
    public class SolarEclipseDetails
    {
        internal DateTime date;
        internal SolarEclipseType type;
        internal DateTime partialEclispeBegin;
        internal DateTime aorTEclipseBegin;
        internal DateTime maximumEclipse;
        internal DateTime aorTEclipseEnd;
        internal DateTime partialEclispeEnd;
        internal TimeSpan aorTDuration;

        internal bool hasEclipseData;

        /// <summary>
        /// Initialize a SolarEclipseDetails object.
        /// </summary>
        /// <param name="values">Solar Eclipse String Values</param>
        public SolarEclipseDetails(List<string> values)
        {
            //Eclipse has value
            hasEclipseData = true;
            //Set Eclipse Date
            date = Convert.ToDateTime(values[0]);

            switch (values[1])
            {
                case "P":
                    type = SolarEclipseType.Partial;
                    break;
                case "A":
                    type = SolarEclipseType.Annular;
                    break;
                case "T":
                    type = SolarEclipseType.Total;
                    break;
                default:
                    break;
            }
            TimeSpan ts;
            //Eclipse start
            if (TimeSpan.TryParse(values[2], out ts))
            {
                partialEclispeBegin = date.Add(ts);
            }
            //A or T start
            if (TimeSpan.TryParse(values[4], out ts))
            {
                aorTEclipseBegin = date.Add(ts);
            }
            //Maximum Eclipse
            if (TimeSpan.TryParse(values[5], out ts))
            {
                maximumEclipse = date.Add(ts);
            }
            //A or T ends
            if (TimeSpan.TryParse(values[8], out ts))
            {
                aorTEclipseEnd = date.Add(ts);
            }
            //Eclipse end
            if (TimeSpan.TryParse(values[9], out ts))
            {
                partialEclispeEnd = date.Add(ts);
            }
            //A or T Duration
            if (values[13] != "-")
            {
                string s = values[13].Replace("m", ":").Replace("s", "");
                string[] ns = s.Split(':');
                int mins = 0;
                int secs = 0;

                int.TryParse(ns[0], out mins);
                if (ns.Count() > 0)
                {
                    int.TryParse(ns[1], out secs);
                }

                TimeSpan time = new TimeSpan(0, mins, secs);

                aorTDuration = time;
            }
            else
            {
                aorTDuration = new TimeSpan();
            }
            Adjust_Dates();//Adjust dates if required (needed when eclipse crosses into next day).
        }
        /// <summary>
        /// Initialize an empty SolarEclipseDetails object
        /// </summary>
        public SolarEclipseDetails()
        {
            hasEclipseData = false;
        }
        /// <summary>
        /// JS Eclipse Calc formulas didn't account for Z time calculation.
        /// Iterate through and adjust Z dates where eclipse is passed midnight.
        /// </summary>
        private void Adjust_Dates()
        {
            //Load array in reverse event order
            DateTime[] dateArray = new DateTime[] { partialEclispeBegin, aorTEclipseBegin, maximumEclipse, aorTEclipseEnd, partialEclispeEnd };
            DateTime baseTime = partialEclispeEnd;
            bool multiDay = false; //used to detrmine if eclipse crossed into next Z day

            for (int x = 4; x >= 0; x--)
            {
                DateTime d = dateArray[x];
                //Check if date exist
                if (d > new DateTime())
                {

                    //Adjust if time is less than then baseTime.
                    if (d > baseTime)
                    {
                        switch (x)
                        {
                            case 3:
                                aorTEclipseEnd = aorTEclipseEnd.AddDays(-1);
                                break;
                            case 2:
                                maximumEclipse = maximumEclipse.AddDays(-1);
                                break;
                            case 1:
                                aorTEclipseBegin = aorTEclipseBegin.AddDays(-1);
                                break;
                            case 0:
                                partialEclispeBegin = partialEclispeBegin.AddDays(-1);
                                break;
                            default:
                                break;
                        }

                        multiDay = true;//Set true to change base date value.
                    }
                }
            }
            if (multiDay)
            {
                this.date = this.date.AddDays(-1); //Shave day off base date if multiday.
            }
        }
        /// <summary>
        /// Has SolarEclipseDetails object has been populated.
        /// </summary>
        public bool HasEclipseData { get { return hasEclipseData; } }
        /// <summary>
        /// Date of solar eclipse.
        /// </summary>
        public DateTime Date { get { return date; } }
        /// <summary>
        /// Solar eclipse type.
        /// </summary>
        public SolarEclipseType Type { get { return type; } }
        /// <summary>
        /// DateTime when the partial eclipse begins.
        /// </summary>
        public DateTime PartialEclispeBegin { get { return partialEclispeBegin; } }
        /// <summary>
        /// DateTime when an Annular or Total eclipse begins (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur</remarks>
        public DateTime AorTEclipseBegin { get { return aorTEclipseBegin; } }
        /// <summary>
        /// DateTime when eclipse is at Maximum.
        /// </summary>
        public DateTime MaximumEclipse { get { return maximumEclipse; } }
        /// <summary>
        /// DateTime when the Annular or Total eclipse ends (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur</remarks>
        public DateTime AorTEclipseEnd { get { return aorTEclipseEnd; } }
        /// <summary>
        /// DateTime when the partial eclipse ends.
        /// </summary>
        public DateTime PartialEclispeEnd { get { return partialEclispeEnd; } }
        /// <summary>
        /// Duration of Annular or Total eclipse (if applicable).
        /// </summary>
        public TimeSpan AorTDuration { get { return aorTDuration; } }
        /// <summary>
        /// Solar eclipse default string.
        /// </summary>
        /// <returns>Solar eclipse base date string</returns>
        public override string ToString()
        {
            return date.ToString("dd-MMM-yyyy");
        }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(SolarEclipseDetails).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DateTime))
                {
                    DateTime d = (DateTime)field.GetValue(this);
                    if (d > new DateTime())
                    {
                        d = d.AddHours(offset);
                        field.SetValue(this, d);
                    }
                }
            }

            date = partialEclispeBegin.Date;
        }
    }

    /// <summary>
    /// Solstice values
    /// </summary>
    [Serializable]
    public class Solstices
    {
        /// <summary>
        /// Summer solstice DateTime
        /// </summary>
        public DateTime Summer { get; set; }
        /// <summary>
        /// Winter solstice DateTime
        /// </summary>
        public DateTime Winter { get; set; }
    }
    /// <summary>
    /// Equinox values
    /// </summary>
    [Serializable]
    public class Equinoxes
    {
        /// <summary>
        /// Spring equinox DateTime
        /// </summary>
        public DateTime Spring { get; set; }
        /// <summary>
        /// Fall equinox DateTime
        /// </summary>
        public DateTime Fall{ get; set; }
    }

    internal class CelCoords
    {
        public double ra { get; set; }
        public double dec { get; set; }
        public double dist { get; set; }
    }
}
