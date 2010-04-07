using System;
using System.Collections.Generic;

namespace ValidationFramework
{
	/// <summary>
	/// Helps in the formatting of <see cref="ValidationError"/>s.
	/// </summary>
	public static class ResultFormatter
	{
		#region Methods

		/// <summary>
		/// Gets a <see see="ICollection{T}"/> of <see langword="string"/>s that contain all the <see cref="ValidationError.ErrorMessage"/>s for all the <see cref="ValidationError"/>s in <paramref name="validationErrors"/>.
		/// </summary>
		/// <param name="validationErrors">A <see cref="ICollection{T}"/> if <see cref="ValidationError"/>s to extract the <see cref="ValidationError.ErrorMessage"/>s from.</param>
		public static IList<string> GetErrorMessages(ICollection<ValidationError> validationErrors)
		{
			var errors = new List<string>();
			foreach (var validationError in validationErrors)
			{
				errors.Add(validationError.ErrorMessage);
			}
			return errors;
		}



		/// <summary>
		/// Gets a <see see="ICollection{T}"/> of <see langword="string"/>s that contain all the <see cref="ValidationError.ErrorMessage"/>s for all the <see cref="ValidationError"/>s in <paramref name="validationErrors"/>.
		/// </summary>
		/// <param name="splitMessagesOnNewLine"><c>true</c> to split any <see cref="ValidationError.ErrorMessage"/> that contains <see cref="Environment.NewLine"/> into multiple entries.</param>
		/// <param name="validationErrors">A <see cref="ICollection{T}"/> if <see cref="ValidationError"/>s to extract the <see cref="ValidationError.ErrorMessage"/>s from.</param>
		public static IList<string> GetErrorMessages(ICollection<ValidationError> validationErrors, bool splitMessagesOnNewLine)
		{
			var errors = new List<string>();
			foreach (var validationError in validationErrors)
			{
				if (validationError.ErrorMessage.Contains(Environment.NewLine))
				{
				   var tempErrorMessage = validationError.ErrorMessage.Replace("\r", "");
                   var split = tempErrorMessage.Split('\n');
					foreach (var errorMessage in split)
					{
						errors.Add(errorMessage);
					}
				}
				else
				{
					errors.Add(validationError.ErrorMessage);
				}
			}
			return errors;
		}


		/// <summary>
		/// Concatenate a <see cref="ICollection{T}"/> of <see cref="ValidationError"/>.
		/// </summary>
		/// <remarks>Use a <see cref="Environment.NewLine"/> a the separator.</remarks>
		/// <param name="validationErrors">The <see cref="ValidationError"/>s to concatenate.</param>
		/// <returns>A concatenated string of all <see cref="ValidationError.ErrorMessage"/>s.</returns>
		public static string GetConcatenatedErrorMessages(ICollection<ValidationError> validationErrors)
		{
			return GetConcatenatedErrorMessages(Environment.NewLine, validationErrors);
		}


		//TODO: there is some refactoring to be done here
		/// <summary>
		/// Concatenate a <see cref="ICollection{T}"/> of <see cref="ValidationError"/>.
		/// </summary>
		/// <param name="separator">The separator to use between each validation message.</param>
		/// <param name="validationErrors">The <see cref="ValidationError"/>s to concatenate.</param>
		/// <returns>A concatenated string of all <see cref="ValidationError.ErrorMessage"/>s.</returns>
		public static string GetConcatenatedErrorMessages(string separator, ICollection<ValidationError> validationErrors)
		{
			ICollection<string> strings = GetErrorMessages(validationErrors);
			var destinationArray = new string[strings.Count];
			strings.CopyTo(destinationArray, 0);
			return string.Join(separator, destinationArray);
		}


		/// <summary>
		/// Concatenate a <see cref="ICollection{T}"/> of <see cref="ValidationError"/>.
		/// </summary>
		/// <param name="separator">The separator to use between each validation message.</param>
		/// <param name="errorMessages">The <see cref="ValidationError"/>s to concatenate.</param>
		/// <returns>A concatenated string of all <see cref="ValidationError.ErrorMessage"/>s.</returns>
		public static string GetConcatenatedErrorMessages(string separator, ICollection<string> errorMessages)
		{
			var destinationArray = new string[errorMessages.Count];
			errorMessages.CopyTo(destinationArray, 0);
			return string.Join(separator, destinationArray);
		}


		/// <summary>
		/// Concatenate a <see cref="ICollection{T}"/> of <see cref="ValidationError"/>.
		/// </summary>
		/// <param name="errorMessages">The <see cref="ValidationError"/>s to concatenate.</param>
		/// <returns>A concatenated string of all <see cref="ValidationError.ErrorMessage"/>s.</returns>
		public static string GetConcatenatedErrorMessages(ICollection<string> errorMessages)
		{
			return GetConcatenatedErrorMessages(Environment.NewLine, errorMessages);
		}

		#endregion
	}
}