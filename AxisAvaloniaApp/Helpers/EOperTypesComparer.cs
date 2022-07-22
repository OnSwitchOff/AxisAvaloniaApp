using Microinvest.CommonLibrary.Enums;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Helpers
{
    /// <summary>
    /// EOperTypes comparer to LINQ GroupBy method
    /// </summary>
    public class EOperTypesComparer : IComparer<EOperTypes>
    {
        /// <summary>
        /// Compare two EOperTypes
        /// </summary>
        /// <param name="x">The first EOperTypes</param>
        /// <param name="y">The second EOperTypes</param>
        /// <returns>Value Meaning Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y</returns>
        /// <developer>Serhii Rozniuk</developer>
        /// <date>20.07.2022.</date>
        public int Compare(EOperTypes x, EOperTypes y)
        {
            if ((int)x < (int)y)
            {
                return -1;
            }

            if ((int)x > (int)y)
            {
                return 1;
            }

            return 0;
        }
    }
}
