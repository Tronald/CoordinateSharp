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
    /// Class for storing moon illumination Information.
    /// </summary>
    [Serializable]
    public class MoonIllum
    {

        /// <summary>
        /// Moon's fraction
        /// </summary>
        public double Fraction { get; internal set; }
        /// <summary>
        /// Moon's Angle
        /// </summary>
        public double Angle { get; internal set; }
        /// <summary>
        /// Moon's phase
        /// </summary>
        public double Phase { get; internal set; }
        /// <summary>
        /// Moon's phase name for the specified day
        /// </summary>
        public string PhaseName { get; internal set; }
        /// <summary>
        /// Moon's phase name enumerator for the specified day
        /// </summary>
        public PhaseEnum PhaseNameEnum { get; internal set; }

    }
    /// <summary>
    /// Class for storing perigee and apogee details.
    /// </summary>
    [Serializable]
    public class PerigeeApogee
    {
        private DateTime date;
        private double horizontalParallax;
        private Distance distance;

        /// <summary>
        /// Initializes a Perigee or Apogee object
        /// </summary>
        /// <param name="d">Date of Event</param>
        /// <param name="p">Horizontal Parallax</param>
        /// <param name="dist">Distance</param>
        public PerigeeApogee(DateTime d, double p, Distance dist)
        {
            date = d;
            horizontalParallax = p;
            distance = dist;
        }

        /// <summary>
        /// Date of event.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }
        /// <summary>
        /// Horizontal Parallax.
        /// </summary>
        public double HorizontalParallax
        {
            get { return horizontalParallax; }
        }
        /// <summary>
        /// Moon's distance at event.
        /// </summary>
        public Distance Distance
        {
            get { return distance; }
        }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(PerigeeApogee).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
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
        }

    }
    /// <summary>
    /// Class for storing last and next perigee information for a specified DateTime.
    /// </summary>
    [Serializable]
    public class Perigee
    {
        private PerigeeApogee lastPerigee;
        private PerigeeApogee nextPerigee;

        /// <summary>
        /// Initializes an Perigee object.
        /// </summary>
        /// <param name="last">Last perigee</param>
        /// <param name="next">Next perigee</param>
        public Perigee(PerigeeApogee last, PerigeeApogee next)
        {
            lastPerigee = last;
            nextPerigee = next;
        }

        /// <summary>
        /// Last perigee
        /// </summary>
        public PerigeeApogee LastPerigee { get { return lastPerigee; } }
        /// <summary>
        /// Next perigee
        /// </summary>
        public PerigeeApogee NextPerigee { get { return nextPerigee; } }

        internal void ConvertTo_Local_Time(double offset)
        {
            LastPerigee.Convert_To_Local_Time(offset);
            NextPerigee.Convert_To_Local_Time(offset);
        }

    }
    /// <summary>
    /// Class for storing last and next apogee information for a specified DateTime.
    /// </summary>
    [Serializable]
    public class Apogee
    {
        private PerigeeApogee lastApogee;
        private PerigeeApogee nextApogee;

        /// <summary>
        /// Initializes an Apogee object.
        /// </summary>
        /// <param name="last">Last apogee</param>
        /// <param name="next">Next apogee</param>
        public Apogee(PerigeeApogee last, PerigeeApogee next)
        {
            lastApogee = last;
            nextApogee = next;
        }

        /// <summary>
        /// Last apogee
        /// </summary>
        public PerigeeApogee LastApogee { get { return lastApogee; } }
        /// <summary>
        /// Next apogee
        /// </summary>
        public PerigeeApogee NextApogee { get { return nextApogee; } }

        internal void ConvertTo_Local_Time(double offset)
        {
            LastApogee.Convert_To_Local_Time(offset);
            NextApogee.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class for storing last and next lunar eclipse information at a specified DateTime and Coordinate.
    /// </summary>
    [Serializable]
    public class LunarEclipse
    {
        internal LunarEclipseDetails lastEclipse;
        internal LunarEclipseDetails nextEclipse;

        /// <summary>
        /// Initialize a LunarEclipse object
        /// </summary>
        public LunarEclipse()
        {
            lastEclipse = new LunarEclipseDetails();
            nextEclipse = new LunarEclipseDetails();
        }
        /// <summary>
        /// Details about the previous lunar eclipse at the specified DateTime and Coordinate.
        /// </summary>
        public LunarEclipseDetails LastEclipse { get { return lastEclipse; } }
        /// <summary>
        /// Details about the next lunar eclipse at the specified DateTime and Coordinate.
        /// </summary>
        public LunarEclipseDetails NextEclipse { get { return nextEclipse; } }

        internal void ConvertTo_LocalTime(double offset)
        {
            LastEclipse.Convert_To_Local_Time(offset);
            NextEclipse.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class for storing astrological sign information.
    /// </summary>
    [Serializable]
    public class AstrologicalSigns
    {
        internal string moonName;
        internal string moonSign;
        internal string zodiacSign;

        /// <summary>
        /// Astrological Zodiac Sign.
        /// </summary>
        public string MoonName { get { return moonName; } }
        /// <summary>
        /// Astrological Moon Sign.
        /// </summary>
        public string MoonSign { get { return moonSign; } }
        /// <summary>
        /// Astrological Zodiac Sign.
        /// </summary>
        public string ZodiacSign { get { return zodiacSign; } }
    }
    /// <summary>
    /// Class containing detailed lunar eclipse information.
    /// </summary>
    [Serializable]
    public class LunarEclipseDetails
    {
        private DateTime date;
        private LunarEclipseType type;
        private DateTime penumbralEclipseBegin;
        private DateTime partialEclispeBegin;
        private DateTime totalEclipseBegin;
        private DateTime midEclipse;
        private DateTime totalEclipseEnd;
        private DateTime partialEclispeEnd;
        private DateTime penumbralEclipseEnd;

        private bool hasEclipseData;

        /// <summary>
        /// Initialize a LunarEclipseDetails object.
        /// </summary>
        /// <param name="values">Lunar Eclipse String Values.</param>
        public LunarEclipseDetails(List<string> values)
        {
            //Eclipse has value
            hasEclipseData = true;
            //Set Eclipse Date
            date = Convert.ToDateTime(values[0]);
            switch (values[1])
            {
                case "T":
                    type = LunarEclipseType.Total;
                    break;
                case "P":
                    type = LunarEclipseType.Partial;
                    break;
                case "N":
                    type = LunarEclipseType.Penumbral;
                    break;
                default:
                    break;
            }
            TimeSpan ts;
            //Penumbral Eclipse start
            if (TimeSpan.TryParse(values[4], out ts))
            {
                penumbralEclipseBegin = date.Add(ts);
            }
            //PartialEclipse start
            if (TimeSpan.TryParse(values[6], out ts))
            {
                partialEclispeBegin = date.Add(ts);
            }
            //Total start
            if (TimeSpan.TryParse(values[8], out ts))
            {
                totalEclipseBegin = date.Add(ts);
            }
            //Mid Eclipse
            if (TimeSpan.TryParse(values[10], out ts))
            {
                midEclipse = date.Add(ts);
            }
            //Total ends
            if (TimeSpan.TryParse(values[12], out ts))
            {
                totalEclipseEnd = date.Add(ts);
            }
            //Partial Eclipse end
            if (TimeSpan.TryParse(values[14], out ts))
            {
                partialEclispeEnd = date.Add(ts);
            }
            //Penumbral Eclipse end
            if (TimeSpan.TryParse(values[16], out ts))
            {
                penumbralEclipseEnd = date.Add(ts);
            }
            Adjust_Dates();
        }
        /// <summary>
        /// Initialize a default LunarEclipseDetails object.
        /// </summary>
        public LunarEclipseDetails()
        {
            hasEclipseData = false;
        }
        /// <summary>
        /// JS Eclipse Calc formulas didn't account for Z time calculation.
        /// Iterate through and adjust Z dates where eclipse is passed midnight.
        /// </summary>
        private void Adjust_Dates()
        {
            //Load array in squential order.
            DateTime[] dateArray = new DateTime[] { penumbralEclipseBegin, partialEclispeBegin, totalEclipseBegin, midEclipse, totalEclipseEnd, partialEclispeEnd, penumbralEclipseEnd };
            DateTime baseTime = partialEclispeEnd;
            bool multiDay = false; //used to detrmine if eclipse crossed into next Z day
            baseTime = penumbralEclipseBegin;
            for (int x = 0; x < dateArray.Count(); x++)
            {
                DateTime d = dateArray[x];
                //Check if date exist
                if (d > new DateTime())
                {
                    if (d < baseTime)
                    {
                        multiDay = true;
                    }
                }
                baseTime = dateArray[x];
                if (multiDay == true)
                {
                    switch (x)
                    {
                        case 1:
                            partialEclispeBegin = partialEclispeBegin.AddDays(1);
                            break;
                        case 2:
                            totalEclipseBegin = totalEclipseBegin.AddDays(1);
                            break;
                        case 3:
                            midEclipse = midEclipse.AddDays(1);
                            break;
                        case 4:
                            totalEclipseEnd = totalEclipseEnd.AddDays(1);
                            break;
                        case 5:
                            partialEclispeEnd = partialEclispeEnd.AddDays(1);
                            break;
                        case 6:
                            penumbralEclipseEnd = penumbralEclipseEnd.AddDays(1);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Determine if the LunarEclipseDetails object has been populated.
        /// </summary>
        public bool HasEclipseData { get { return hasEclipseData; } }
        /// <summary>
        /// Date of lunar eclipse.
        /// </summary>
        public DateTime Date { get { return date; } }
        /// <summary>
        /// Lunar eclipse type.
        /// </summary>
        public LunarEclipseType Type { get { return type; } }
        /// <summary>
        /// DateTime when the penumbral eclipse begins.
        /// </summary>
        public DateTime PenumbralEclipseBegin { get { return penumbralEclipseBegin; } }
        /// <summary>
        /// DateTime when the partial eclipse begins (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur.</remarks>
        public DateTime PartialEclispeBegin { get { return partialEclispeBegin; } }
        /// <summary>
        /// DateTime when Total eclipse begins (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur.</remarks>
        public DateTime TotalEclipseBegin { get { return totalEclipseBegin; } }
        /// <summary>
        /// DateTime when eclipse is at Mid.
        /// </summary>
        public DateTime MidEclipse { get { return midEclipse; } }
        /// <summary>
        /// DateTime when total eclipse ends (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur.</remarks>
        public DateTime TotalEclipseEnd { get { return totalEclipseEnd; } }
        /// <summary>
        /// DateTime when the partial eclipse ends (if applicable).
        /// </summary>
        /// <remarks>returns 0001/01/01 if event did not occur.</remarks>
        public DateTime PartialEclispeEnd { get { return partialEclispeEnd; } }
        /// <summary>
        /// DateTime when the penumbral eclipse ends.
        /// </summary>
        public DateTime PenumbralEclispeEnd { get { return penumbralEclipseEnd; } }
        /// <summary>
        /// Lunar eclipse default string.
        /// </summary>
        /// <returns>Lunar eclipse base date string.</returns>
        public override string ToString()
        {
            return date.ToString("dd-MMM-yyyy");
        }

        internal void Convert_To_Local_Time(double offset)
        {
            FieldInfo[] fields = typeof(LunarEclipseDetails).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
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
            date = penumbralEclipseBegin.Date;

        }

    }
    internal class MoonTimes
    {
        public DateTime set { get; set; }
        public DateTime rise { get;  set; }
        public CelestialStatus status { get; set; }
    }
    internal class MoonPosition
    {
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
        public Distance Distance { get; set; }
        public double ParallacticAngle { get; set; }
        public double ParallaxCorection { get; set; }
    }
}
