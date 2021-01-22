#define DETERMINISTIC_SEED  // Comment for further coverage during development, keep for deterministic outcome (so the CI/CD pipeline doesn't randomly fail)

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CoordinateSharp;
using System.Threading;
using System.Globalization;

namespace CoordinateSharp_UnitTests
{
    /// <summary>
    /// Class to verify that parsing and converting to string eliminate each other (up to accuracy issues) 
    /// </summary>
    [TestClass]
    public class Parsing_String_Representations_Fuzzy
    {
        static Parsing_String_Representations_Fuzzy()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        // Empirically determined limit on accuracy
        private static readonly Distance AccuracyLimit = new Distance(2.0, DistanceType.Meters);

#if DETERMINISTIC_SEED
        private const int rngSeed = 0;
        private static readonly Random random = new Random(rngSeed);
#else
        private static readonly Random random = new Random();
#endif

        private const int nrRepetitions = 100;

        /// <summary>
        /// Generates a random <see cref="double"/> between <paramref name="bound1"/> and <paramref name="bound2"/>.
        /// </summary>
        /// <param name="bound1"></param>
        /// <param name="bound2"></param>
        /// <returns></returns>
        private static double GetRandomDoubleBetween(double bound1, double bound2)
        {
            double lambda = random.NextDouble();
            return lambda * bound1 + (1 - lambda) * bound2;
        }

        /// <summary>
        /// Creats a <see cref="Coordinate"/> object with a uniformly distributed random latitude and uniformly distributed random longitude with specified eager loading options.
        /// </summary>
        /// <param name="eagerLoad"></param>
        /// <returns></returns>
        private static Coordinate GetRandomCoordinate(EagerLoad eagerLoad)
        {
            double latitude = GetRandomDoubleBetween(-90.0, 90.0);
            double longitude = GetRandomDoubleBetween(-180.0, 180.0);

            return new Coordinate(latitude, longitude, eagerLoad);
        }

        /// <summary>
        /// Asserts whether <paramref name="actual"/> is "close enough" to <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="s"></param>
        private static void AsserCoordinatesAreClose(Coordinate expected, Coordinate actual)
        {
            Distance distance = new Distance(expected, actual);

            Assert.AreEqual(expected: 0.0,
                            actual: distance.Meters,
                            delta: AccuracyLimit.Meters,
                            message: $"Expected something close to <{expected}>, but got <{actual}>. Coordinate was parsed <{actual.Parse_Format}>.");
        }

        /// <summary>
        /// Tests whether parsing string representations (depending on <paramref name="options"/>) of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        /// <param name="options"></param>
        private void Parse_Geodetic_Strings(CoordinateFormatOptions options)
        {
            EagerLoad el = new EagerLoad(false);

            void DoOneRandomTest()
            {
                Coordinate expected = GetRandomCoordinate(el);
                string s = expected.ToString(options);
                Coordinate actual = Coordinate.Parse(s);
                AsserCoordinatesAreClose(expected, actual);
            }

            for (int i = 0; i < nrRepetitions; ++i)
            {
                DoOneRandomTest();
            }
        }

        /// <summary>
        /// Tests whether parsing string representations (in <see cref="CoordinateFormatType.Decimal"/> format) of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_From_Decimal_String()
        {
            CoordinateFormatOptions options = new CoordinateFormatOptions()
            {
                Format = CoordinateFormatType.Decimal,
                Round = 6
            };
            Parse_Geodetic_Strings(options);
        }

        /// <summary>
        /// Tests whether parsing string representations (in <see cref="CoordinateFormatType.Decimal_Degree"/> format) of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_Decimal_Degrees_String()
        {
            CoordinateFormatOptions options = new CoordinateFormatOptions()
            {
                Format = CoordinateFormatType.Decimal_Degree,
                Round = 6
            };
            Parse_Geodetic_Strings(options);
        }

        /// <summary>
        /// Tests whether parsing string representations (in <see cref="CoordinateFormatType.Degree_Decimal_Minutes"/> format) of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_Degree_Decimal_Minutes_String()
        {
            CoordinateFormatOptions options = new CoordinateFormatOptions()
            {
                Format = CoordinateFormatType.Degree_Decimal_Minutes
            };
            Parse_Geodetic_Strings(options);
        }

        /// <summary>
        /// Tests whether parsing string representations (in <see cref="CoordinateFormatType.Degree_Minutes_Seconds"/> format) of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_Degree__Minute_Decimal_Seconds_String()
        {
            CoordinateFormatOptions options = new CoordinateFormatOptions()
            {
                Format = CoordinateFormatType.Degree_Minutes_Seconds
            };
            Parse_Geodetic_Strings(options);
        }

        /// <summary>
        /// Tests whether parsing MGRS string representations of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_MGRS_String()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);

            void DoOneRandomTest()
            {
                Coordinate expected = GetRandomCoordinate(el);
                string s = expected.MGRS.ToString();
                Coordinate actual = Coordinate.Parse(s, el);

                Distance distance = new Distance(expected, actual);
                Assert.AreEqual(expected: 0.0,
                                actual: distance.Meters,
                                delta: AccuracyLimit.Meters,
                                message: $"Expected something close to <{expected}>, but got <{actual}>. String representation was <{s}>.");
            }

            for (int i = 0; i < nrRepetitions; ++i)
            {
                DoOneRandomTest();
            }
        }

        /// <summary>
        /// Tests whether parsing UTM string representations of 100 random <see cref="Coordinate"/> objects returns "close enough" <see cref="Coordinate"/> objects.
        /// </summary>
        [TestMethod]
        public void Parse_UTM_String()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.UTM_MGRS);

            void DoOneRandomTest()
            {
                Coordinate expected = GetRandomCoordinate(el);
                string s = expected.UTM.ToString();
                Coordinate actual = Coordinate.Parse(s, el);
                AsserCoordinatesAreClose(expected, actual);
            }

            for (int i = 0; i < nrRepetitions; ++i)
            {
                DoOneRandomTest();
            }
        }

        /// <summary>
        /// Tests whether parsing string representations of random <see cref="Coordinate"/> in ECEF results in close enough <see cref="Coordinate"/>.
        /// </summary>
        [TestMethod]
        public void Parse_ECEF_String()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.ECEF);

            void DoOneRandomTest()
            {
                Coordinate expected = GetRandomCoordinate(el);
                string s = expected.ECEF.ToString();
                Coordinate actual = Coordinate.Parse(s, CartesianType.ECEF, el);
                AsserCoordinatesAreClose(expected, actual);
            }

            for (int i = 0; i < nrRepetitions; ++i)
            {
                DoOneRandomTest();
            }
        }

        /// <summary>
        /// Tests whether parsing string representations of random <see cref="Coordinate"/> in ECEF results in close enough <see cref="Coordinate"/>.
        /// </summary>
        [TestMethod]
        public void Parse_Cartesian_String()
        {
            EagerLoad el = new EagerLoad(EagerLoadType.Cartesian);

            void DoOneRandomTest()
            {
                Coordinate expected = GetRandomCoordinate(el);
                string s = expected.Cartesian.ToString();
                Coordinate actual = Coordinate.Parse(s, CartesianType.Cartesian, el);
                AsserCoordinatesAreClose(expected, actual);
            }

            for (int i = 0; i < nrRepetitions; ++i)
            {
                DoOneRandomTest();
            }
        }
    }
}
