
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace DProject.Controls.FiterableDataGrid.Models
{
	/// <summary>
	/// Model representation of filter predicate containing column, operation and value to filter against.
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	public class FilterInstruction : INotifyPropertyChanged
	{
		#region Private fields

		private FilterableColumn _column;

		private object _operation;

		private object _value;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the column to filter.
		/// </summary>
		public FilterableColumn Column
		{
			get { return _column; }
			set
			{
				_column = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the operation to apply to the value.
		/// </summary>
		public object Operation
		{
			get { return _operation; }
			set
			{
				_operation = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that the operation is applied  to.
		/// </summary>
		public object Value
		{
			get { return _value; }
			set
			{
				_value = value;
				NotifyPropertyChanged();
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}
