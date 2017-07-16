using System;

namespace DProject.Controls.FiterableDataGrid.Models
{
	/// <summary>
	/// Model representation of a datagrid column available for filtering.
	/// </summary>
	public class FilterableColumn
	{
		#region Properties

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// Gets or sets the caption.
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Gets or sets the model path.
		/// </summary>
		public object ModelPath { get; set; }

		#endregion
	}
}
