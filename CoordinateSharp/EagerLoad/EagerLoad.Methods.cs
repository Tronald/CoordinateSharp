/*
CoordinateSharp is a .NET standard library that is intended to ease geographic coordinate 
format conversions and location based celestial calculations.
https://github.com/Tronald/CoordinateSharp

Many celestial formulas in this library are based on Jean Meeus's 
Astronomical Algorithms (2nd Edition). Comments that reference only a chapter
are referring to this work.

License

CoordinateSharp is split licensed and may be licensed under the GNU Affero General Public License version 3 or a commercial use license as stated.

Copyright (C) 2023, Signature Group, LLC
  
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

Organizations or use cases that fall under the following conditions may receive a free commercial use license upon request on a case by case basis.


	-Open source contributors to this library.
	-Scholarly or scientific research.
	-Emergency response / management uses.

Please visit http://coordinatesharp.com/licensing or contact Signature Group, LLC to purchase a commercial license, or for any questions regarding the AGPL 3.0 license requirements or free use license: sales@signatgroup.com.
*/

namespace CoordinateSharp
{
    public partial class EagerLoad
    {
        /// <summary>
        /// Creates a default EagerLoad object.
        /// </summary>
        /// <remarks>
        /// All properties are set with eager loading turned on.
        /// </remarks>
        /// <example>
        /// The following example turns off eager loading for a Coordinate objects CelestialInfo property.
        /// <code>
        /// //Create a default EagerLoading object.
        /// EagerLoad el = new EagerLoad();
        ///
        /// //Turn of eagerloading of celestial information.
        /// el.Celestial = false;
        /// 
        /// //Create coordinate with defined eager loading settings.
        /// Coordinate coord = new Coordinate(25, 25, new DateTime(2018, 3, 2), el);
        /// 
        /// //Load celestial information when ready.
        /// //Failure to do this will cause NullReference Exceptions in the Coordinate objects CelestialInfo Property.
        /// coord.LoadCelestialInfo();
        /// 
        /// //Display UTC sunset time at the location.
        /// Console.WriteLine(coord.CelestialInfo.SunSet); //3/2/2018 4:23:46 PM
        /// </code>
        /// </example>
        public EagerLoad()
        {
            Extensions = new EagerLoad_Extensions();
            Celestial = true;
            UTM_MGRS = true;
            Cartesian = true;
            ECEF = true;
            WebMercator = true;
            GEOREF = true;          
        }

        /// <summary>
        /// Create an EagerLoad object with all options on or off
        /// </summary>
        /// <param name="isOn">Turns EagerLoad on or off</param>
        /// <example>
        /// The following example turns off eagerloading for a Coordinate objects UTM/MGRS, Cartesian/ECEF and CelestialInfo properties.
        /// <code>
        /// //Create an EagerLoading object with all properties turned off.
        /// //(All properties will now be set to lazy load).
        /// EagerLoad el = new EagerLoad(false);
        /// 
        /// //Create coordinate with defined eager loading settings.
        /// Coordinate coord = new Coordinate(25, 25, new DateTime(2018, 3, 2), el);
        /// 
        /// //Load celestial information when ready.
        /// //Failure to do this will cause NullReference Exceptions in the Coordinate objects CelestialInfo Property.
        /// coord.LoadCelestialInfo();
        /// 
        /// //Display UTC sunset time at the location.
        /// Console.WriteLine(coord.CelestialInfo.SunSet); //3/2/2018 4:23:46 PM
        /// </code>
        /// </example>
        public EagerLoad(bool isOn)
        {
            Extensions = new EagerLoad_Extensions();
            Celestial = isOn;
            UTM_MGRS = isOn;
            Cartesian = isOn;
            ECEF = isOn;
            WebMercator= isOn;
            GEOREF = isOn;
        }

        /// <summary>
        /// Creates an EagerLoad object. Only the specified flags will be set to eager load.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        /// <example>
        /// The following example sets CelestialInfo and Cartesian properties to eager load. Other conversions will be lazy loaded.
        /// <code>
        /// //Create an EagerLoading object with only CelestialInfo and Cartesian properties set to eager load.
        /// EagerLoad el = new EagerLoad(EagerLoadType.Celestial | EagerLoadType.Cartesian);
        /// 
        /// //Create coordinate with defined eagerloading settings.
        /// Coordinate coord = new Coordinate(25, 25, new DateTime(2018, 3, 2), el);
        /// 
        /// //Display UTC sunset time at the location.
        /// Console.WriteLine(coord.CelestialInfo.SunSet); //3/2/2018 4:23:46 PM
        /// Console.WriteLine(coord.Cartesian); //0.8213938 0.38302222 0.42261826
        /// 
        /// //Load UTM_MGRS when ready.
        /// coord.LoadUTM_MGRS_Info();
        /// Console.WriteLine(coord.UTM); //35R 298154mE 2766437mN
        /// </code>
        /// </example>
        public EagerLoad(EagerLoadType et)
        {
            Extensions = new EagerLoad_Extensions();
            Cartesian = false;
            Celestial = false;
            UTM_MGRS = false;
            ECEF = false;
            WebMercator= false;
            GEOREF = false;

            if (et.HasFlag(EagerLoadType.Cartesian))
            {
                Cartesian = true;
            }
            if (et.HasFlag(EagerLoadType.Celestial))
            {
                Celestial = true;
            }
            if (et.HasFlag(EagerLoadType.UTM_MGRS))
            {
                UTM_MGRS = true;
            }
            if (et.HasFlag(EagerLoadType.ECEF))
            {
                ECEF = true;
            }
            if (et.HasFlag(EagerLoadType.WebMercator))
            {
                WebMercator = true;
            }
            if (et.HasFlag(EagerLoadType.GEOREF))
            {
                GEOREF = true;
            }
        }

        /// <summary>
        /// Creates an EagerLoad object. Only the specified flags will be set to eager load.
        /// </summary>
        /// <param name="et">EagerLoadType</param>
        /// <returns>EagerLoad</returns>
        /// <example>
        /// The following example sets CelestialInfo and Cartesian properties to eager load using a static method. Other conversions will be lazy loaded.
        /// <code>
        /// //Create coordinate with defined eagerloading settings.
        /// Coordinate coord = new Coordinate(25, 25, new DateTime(2018, 3, 2), EagerLoad.Create(EagerLoadType.Celestial | EagerLoadType.Cartesian));
        /// 
        /// //Display UTC sunset time at the location.
        /// Console.WriteLine(coord.CelestialInfo.SunSet); //3/2/2018 4:23:46 PM
        /// Console.WriteLine(coord.Cartesian); //0.8213938 0.38302222 0.42261826
        /// 
        /// //Load UTM_MGRS when ready.
        /// coord.LoadUTM_MGRS_Info();
        /// Console.WriteLine(coord.UTM); //35R 298154mE 2766437mN
        /// </code>
        /// </example>
        public static EagerLoad Create(EagerLoadType et)
        {
            EagerLoad el = new EagerLoad(et);
            return el;
        }      
    }
    public partial class EagerLoad_Extensions
    {
        /// <summary>
        /// Create a new EagerLoad_Extensions object. 
        /// All values will be set to true.
        /// </summary>
        public EagerLoad_Extensions()
        {
            Solar_Cycle = true;
            Solstice_Equinox = true;
            Lunar_Cycle = true;
            Solar_Eclipse = true;
            Lunar_Eclipse = true;
            Zodiac = true;
            MGRS = true;
        }
        /// <summary>
        /// Create a new EagerLoad_Extensions object. 
        /// All values will be set to the pass parameter.
        /// </summary>
        /// <param name="eagerLoad">bool</param>
        public EagerLoad_Extensions(bool eagerLoad)
        {
            Solar_Cycle = eagerLoad;
            Solstice_Equinox = eagerLoad;
            Lunar_Cycle = eagerLoad;
            Solar_Eclipse = eagerLoad;
            Lunar_Eclipse = eagerLoad;
            Zodiac = eagerLoad;
            MGRS = eagerLoad;
        }

        /// <summary>
        /// Create a new EagerLoad_Extensions object.
        /// Extension values can be specified with enum flags.
        /// </summary>
        /// <param name="et">EagerLoad_ExtensionsType flag</param>
        public EagerLoad_Extensions(EagerLoad_ExtensionsType et)
        {

            Solar_Cycle = false;
            Solstice_Equinox = false;
            Lunar_Cycle = false;
            Solar_Eclipse = false;
            Lunar_Eclipse = false;
            Zodiac = false;
            MGRS = false;

            if (et.HasFlag(EagerLoad_ExtensionsType.Solar_Cycle))
            {
                Solar_Cycle = true;
            }
            if (et.HasFlag(EagerLoad_ExtensionsType.Solstice_Equinox))
            {
                Solstice_Equinox = true;
            }
            if (et.HasFlag(EagerLoad_ExtensionsType.Lunar_Cycle))
            {
                Lunar_Cycle = true;
            }
            if (et.HasFlag(EagerLoad_ExtensionsType.Solar_Eclipse))
            {
                Solar_Eclipse = true;
            }
            if (et.HasFlag(EagerLoad_ExtensionsType.Lunar_Eclipse))
            {
                Lunar_Eclipse = true;
            }
            if (et.HasFlag(EagerLoad_ExtensionsType.Zodiac))
            {
                Zodiac = true;
            }
            if(et.HasFlag(EagerLoad_ExtensionsType.MGRS))
            {
                MGRS = true;
            }
        }

        /// <summary>
        /// Sets all celestial related extensions 
        /// </summary>
        /// <param name="option">bool</param>
        internal void Set_Celestial_Items(bool option)
        {
            Solar_Cycle = option;
            Solstice_Equinox = option;
            Lunar_Cycle = option;
            Solar_Eclipse = option;
            Lunar_Eclipse = option;
            Zodiac = option;
        }
    }
}
