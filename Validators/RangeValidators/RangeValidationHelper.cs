using System;

namespace ValidationFramework
{
    /// <summary>
    /// Static class for RangeValidation methods.
    /// </summary>
    internal static class RangeValidationHelper
    {
        #region Methods

        internal static bool IsRangeValid<T>(T value, T min, T max, bool equalsMinimumIsValid, bool equalsMaximumIsValid) where T : IComparable<T>
        {
            bool isAboveMin;
            if (equalsMinimumIsValid)
            {
                isAboveMin = CompareValidationHelper.Compare(value, min, CompareOperator.GreaterThanEqual);
            }
            else
            {
                isAboveMin = CompareValidationHelper.Compare(value, min, CompareOperator.GreaterThan);
            }

            if (isAboveMin)
            {
                bool isBelowMax;
                if (equalsMaximumIsValid)
                {
                    isBelowMax = CompareValidationHelper.Compare(value, max, CompareOperator.LessThanEqual);
                }
                else
                {
                    isBelowMax = CompareValidationHelper.Compare(value, max, CompareOperator.LessThan);
                }

                return (isBelowMax);
            }
            return false;
        }


        internal static bool IsRangeValid(string value, string min, string max, bool equalsMinimumIsValid, bool equalsMaximumIsValid)
        {
            if (value == null)
            {
                return true;
            }

            bool isAboveMin;
            if (equalsMinimumIsValid)
            {
                isAboveMin = (value.CompareTo(min) >= 0);
            }
            else
            {
                isAboveMin = (value.CompareTo(min) > 0);
            }

            if (isAboveMin)
            {
                bool isBelowMax;

                if (equalsMaximumIsValid)
                {
                    isBelowMax = (value.CompareTo(max) <= 0);
                }
                else
                {
                    isBelowMax = (value.CompareTo(max) < 0);
                }

                return (isBelowMax);
            }
            return false;
        }

        #endregion
    }
}