
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DProject.Controls.FilterableDataGrid.Operations;
namespace DProject.Controls.FilterableDataGrid.Models
{
	/// <summary>
	/// Model representation of filter predicate containing column, operation and value to filter against.
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	public class FilterCondition : INotifyPropertyChanged
	{
		#region Private fields

		private FilterableColumn _column;
		private Operation _operation;
		private object _value;

		private IList<FilterableColumn> _filterableColumns;
		private IList<Operation> _registeredOperations;																	//can be removed? only passed in constructor
		private ICollectionView _columnOperations;

		#endregion

		#region Constructors

		//TODO: inject both filterableColumns and Operations
		/// <summary>
		/// Initializes a new instance of the <see cref="FilterCondition" /> class.
		/// </summary>
		/// <param name="filterableColumns">The filterable columns extracted from bound dataGrid.</param>
		public FilterCondition(IList<FilterableColumn> filterableColumns)
		{
			_filterableColumns = filterableColumns;

			_registeredOperations = new List<Operation>
			{
				new IsNumberOperation(),
			};

			_columnOperations = CollectionViewSource.GetDefaultView(_registeredOperations);
			_columnOperations.Filter = ColumnOperationsPredicate;
		}

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
				_columnOperations.Refresh();
			}
		}

		/// <summary>
		/// Gets or sets the operation to apply to the value.
		/// </summary>
		public Operation Operation
		{
			get { return _operation; }
			set
			{
				_operation = value;
				NotifyPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that the operation is applied against.
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

		/// <summary>
		/// Gets the filterable columns.
		/// </summary>
		public IList<FilterableColumn> FilterableColumns
		{
			get { return _filterableColumns; }
			private set
			{
				_filterableColumns = value;
			}
		}

		/// <summary>
		/// Gets the column operations.
		/// </summary>
		public ICollectionView ColumnOperations
		{
			get { return _columnOperations; }
			private set
			{
				_columnOperations = value;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Executes the condition against provided value.
		/// </summary>
		/// <param name="dataGridValue">The data grid value.</param>
		/// <returns>True if condition is satisfied or condition is not valid, false otherwise.</returns>
		/// <exception cref="System.ArgumentNullException">dataGridValue</exception>
		public bool ExecuteCondition(object dataGridValue)
		{
			if (dataGridValue == null)
				throw new ArgumentNullException("dataGridValue");

			if (_column != null &&
				_operation != null &&
				_value != null)
			{
				PropertyInfo property = dataGridValue.GetType().GetProperty(_column.ModelPath);
				object value = property.GetValue(dataGridValue);

				return _operation.Execute(value, Value);
			}

			return true;
		}

		#endregion

		#region Private methods

		private bool ColumnOperationsPredicate(object obj)
		{
			if (Column != null)
			{
				Operation operation = obj as Operation;

				if (operation != null)
				{
					return operation.AppliesTo == Column.Type;
				}
			}

			return false;
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
