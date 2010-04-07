using System;

namespace ValidationFramework
{
    internal static class CompareValidationHelper
    {
        #region Methods

        internal static bool Compare<T>(T left, T right, CompareOperator oper) where T : IComparable<T>
        {
            var compareResult = left.CompareTo(right);
            return CheckCompareResult(compareResult, oper);
        }


        internal static bool Compare(object left, object right, CompareOperator oper)
        {
            var compareResult = ((IComparable) left).CompareTo(right);
            return CheckCompareResult(compareResult, oper);
        }


        internal static bool CheckCompareResult(int compareResult, CompareOperator oper)
        {
            switch (oper)
            {
                case CompareOperator.GreaterThanEqual:
                    {
                        return (compareResult >= 0);
                    }
                case CompareOperator.LessThanEqual:
                    {
                        return (compareResult <= 0);
                    }
                case CompareOperator.GreaterThan:
                    {
                        return (compareResult > 0);
                    }
                case CompareOperator.LessThan:
                    {
                        return (compareResult < 0);
                    }
                case CompareOperator.Equal:
                    {
                        return (compareResult == 0);
                    }
                case CompareOperator.NotEqual:
                    {
                        return (compareResult != 0);
                    }
                default:
            		{
            			throw new InvalidOperationException();
            		}
            }
        }

        #endregion
    }
}