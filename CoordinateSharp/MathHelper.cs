using System;

namespace CoordinateSharp
{
    internal static class MathHelper
    {
        internal static double Round(double input, int decimalDigits)
        {
            // in .NET implementations the two Math.Round overload may have a different implementation,
            // which for example may have some preformance differences
            // hence we have this method to distinguish between them based on requrested decimalDigits

            if (decimalDigits < 0)
            {
                throw new ArgumentException(@"Argument 'decimalDigits' must be a non-negative number", nameof(decimalDigits));
            }

            return decimalDigits > 0 ? Math.Round(input, decimalDigits) : Math.Round(input);
        }
    }
}
