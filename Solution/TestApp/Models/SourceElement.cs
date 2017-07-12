using System;
using DProject.TestApp.Enumerations;

namespace DProject.TestApp.Models
{
	/// <summary>
	/// Mock model whose properties are going to be filtered against.
	/// </summary>
	public class SourceElement
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Returns true if <see cref="SourceElement"/> is valid, otherwise false.
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Gets or sets the simple enumeration value.
		/// </summary>
		public SimpleEnum SimpleEnumerationValue { get; set; }

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		public Uri Link { get; set; }
	}
}
