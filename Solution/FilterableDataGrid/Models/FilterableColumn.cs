using System;

namespace DProject.Controls.FilterableDataGrid.Models
{
	/// <summary>
	/// Model representation of a datagrid column available for filtering.
	/// </summary>
	public class FilterableColumn
	{
		#region Properties

		/// <summary>
		/// Gets or sets the type of bound property.
		/// </summary>
		public Type TargetType { get; set; }

		/// <summary>
		/// Gets or sets the caption.
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Gets or sets the model path.
		/// </summary>
		public string ModelPath { get; set; }

		#endregion
	}
}
