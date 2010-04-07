using ValidationFramework.Extensions;

namespace ValidationFramework
{
    /// <summary>
    /// Specifies the validation comparison operators used <see cref="CompareRule{T}"/>s. 
    /// </summary>
    /// <seealso cref="CompareRule{T}"/>
    public enum CompareOperator
    {
        /// <summary>
        /// A comparison for equality.  
        /// </summary>
        [EnumUserFriendlyString("Equal To")] 
        Equal = 1,
        /// <summary>
        /// A comparison for greater than.  
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// A comparison for greater than or equal to. 
        /// </summary>
        [EnumUserFriendlyString("Greater Than Or Equal To")] 
        GreaterThanEqual = 3,
        /// <summary>
        /// A comparison for less than.  
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// A comparison for less than or equal to.  
        /// </summary>
        [EnumUserFriendlyString("Less Than Or Equal To")] 
        LessThanEqual = 5,
        /// <summary>
        /// A comparison for inequality.  
        /// </summary>
        [EnumUserFriendlyString("Not Equal To")] 
        NotEqual = 6
    }
}