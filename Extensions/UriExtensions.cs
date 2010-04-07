using System;

namespace ValidationFramework.Extensions
{
	internal static class UriExtensions
	{
        internal static bool IsFile(this Uri value)
        {
			return (value.Scheme == Uri.UriSchemeFile);
        }
	}
}
