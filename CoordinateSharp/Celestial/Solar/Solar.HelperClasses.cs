using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace CoordinateSharp
{
    /// <summary>
    /// Additional Solar Time Information
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
        /// Create an AdditionalSolarTimes object.
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
        /// Returns Civil Dawn Time
        /// </summary>
        public DateTime? CivilDawn { get { return civilDawn; } }
        /// <summary>
        /// Returns Civil Dusk Time
        /// </summary>
        public DateTime? CivilDusk { get { return civilDusk; } }
        /// <summary>
        /// Returns Nautical Dawn Time
        /// </summary>
        public DateTime? NauticalDawn { get { return nauticalDawn; } }
        /// <summary>
        /// Returns Nautical Dusk Time
        /// </summary>
        public DateTime? NauticalDusk { get { return nauticalDusk; } }
        /// <summary>
        /// Returns Astronomical Dawn Time
        /// </summary>
        public DateTime? AstronomicalDawn { get { return astronomicalDawn; } }
        /// <summary>
        /// Returns Astronomical Dusk Time
        /// </summary>
        public DateTime? AstronomicalDusk { get { return astronomicalDusk; } }

        /// <summary>
        /// Returns the time when the bottom of the solar disc touches the horizon after sunrise
        /// </summary>
        public DateTime? SunriseBottomDisc { get { return sunriseBottomDisc; } }
        /// <summary>
        /// Returns the time when the bottom of the solar disc touches the horizon before sunset
        /// </summary>
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
    /// Class containing solar eclipse information
    /// </summary>
    [Serializable]
    public class SolarEclipse
    {
        internal SolarEclipseDetails lastEclipse;
        internal SolarEclipseDetails nextEclipse;

        /// <summary>
        /// Initialize a SolarEclipse object
        /// </summary>
        public SolarEclipse()
        {
            lastEclipse = new SolarEclipseDetails();
            nextEclipse = new SolarEclipseDetails();
        }
        /// <summary>
        /// Details about the previous solar eclipse
        /// </summary>
        public SolarEclipseDetails LastEclipse { get { return lastEclipse; } }
        /// <summary>
        /// Details about the next solar eclipse
        /// </summary>
        public SolarEclipseDetails NextEclipse { get { return nextEclipse; } }

        internal void ConvertTo_LocalTime(double offset)
        {
            LastEclipse.Convert_To_Local_Time(offset);
            NextEclipse.Convert_To_Local_Time(offset);
        }
    }
    /// <summary>
    /// Class containing specific solar eclipse information
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
        /// Initialize a SolarEclipseDetails object
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
        /// Determine if the SolarEclipseDetails object has been populated
        /// </summary>
        public bool HasEclipseData { get { return hasEclipseData; } }
        /// <summary>
        /// Date of solar eclipse
        /// </summary>
        public DateTime Date { get { return date; } }
        /// <summary>
        /// Solar eclipse type
        /// </summary>
        public SolarEclipseType Type { get { return type; } }
        /// <summary>
        /// DateTime when the partial eclipse begins
        /// </summary>
        public DateTime PartialEclispeBegin { get { return partialEclispeBegin; } }
        /// <summary>
        /// DateTime when an Annular or Total eclipse begins (if applicable)
        /// </summary>
        public DateTime AorTEclipseBegin { get { return aorTEclipseBegin; } }
        /// <summary>
        /// DateTime when eclipse is at Maximum
        /// </summary>
        public DateTime MaximumEclipse { get { return maximumEclipse; } }

        /// <summary>
        /// DateTime when the Annular or Total eclipse ends (if applicable)
        /// </summary>
        public DateTime AorTEclipseEnd { get { return aorTEclipseEnd; } }
        /// <summary>
        /// DateTime when the partial elipse ends
        /// </summary>
        public DateTime PartialEclispeEnd { get { return partialEclispeEnd; } }
        /// <summary>
        /// Duration of Annular or Total eclipse (if applicable)
        /// </summary>
        public TimeSpan AorTDuration { get { return aorTDuration; } }
        /// <summary>
        /// Solat eclipse default string
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

    internal class CelCoords
    {
        public double ra { get; set; }
        public double dec { get; set; }
        public double dist { get; set; }
    }
}
