using System;
using ValidationFramework.Reflection;

namespace ValidationFramework
{
    public abstract class ValueRule : Rule
    {
        /// <inheritdoc/>
        protected ValueRule(RuntimeTypeHandle? runtimeTypeHandle) : base(runtimeTypeHandle)
        {
        }

        /// <inheritdoc/>
        public sealed override bool Validate(object targetObjectValue, object targetMemberValue, object context, InfoDescriptor infoDescriptor)
        {
            return Validate(targetMemberValue, context, infoDescriptor);
        }

        /// <summary>
        /// Validate the member this <see cref="Rule"/> is applied to.
        /// </summary>
        /// <returns><see langword="true"/> if the member is valid; otherwise <see langword="false"/>.</returns>
        /// <param name="targetMemberValue">The value of the member to validate.</param>
        /// <param name="context">An <see cref="object"/> that contains data for the <see cref="Rule"/> to validate. The default is null.</param>
        /// <param name="infoDescriptor">The <see cref="InfoDescriptor"/> for the member being validated.</param>
        public abstract bool Validate(object targetMemberValue, object context, InfoDescriptor infoDescriptor);

    }
}