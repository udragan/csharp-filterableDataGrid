using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DProject.Controls.FiterableDataGrid.Infrastructure;
using DProject.Controls.FiterableDataGrid.Models;

namespace DProject.Controls.FiterableDataGrid
{
	/// <summary>
	/// DataGrid inspired custom control with generic filtering capability.
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	/// <seealso cref="System.Windows.Controls.Control" />
	public class FilterableDataGrid : Control, INotifyPropertyChanged
	{
		#region Private fields

		private ICommand _execute;

		private DataGrid _dataGrid;
		private IList<FilterableColumn> _filterableColumns;																//reflected columns of dataGrid
		private IList<PropertyInfo> _modelProperties;																	//available bound properties of underlying model
		private IList<FilterInstruction> _filterInstructions;															//filter instructions to be applied to Source
		private ICollectionView _sourcePresenter;

		#endregion

		#region Dependency properties

		/// <summary>
		/// Gets or sets the control source.
		/// </summary>
		public IEnumerable Source
		{
			get { return (IEnumerable)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}
		/// <summary>
		/// The control source dependency property
		/// </summary>
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof(IEnumerable), typeof(FilterableDataGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, SourcePropertyChanged));

		/// <summary>
		/// Gets or sets the datagrid columns.
		/// </summary>
		public ObservableCollection<DataGridColumn> Columns
		{
			get { return (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}
		/// <summary>
		/// The datagrid columns dependency property
		/// </summary>
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register("Columns", typeof(ObservableCollection<DataGridColumn>), typeof(FilterableDataGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, ColumnsPropertyChanged));

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="FilterableDataGrid"/> class.
		/// </summary>
		static FilterableDataGrid()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterableDataGrid),
				new FrameworkPropertyMetadata(typeof(FilterableDataGrid)));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterableDataGrid"/> class.
		/// </summary>
		public FilterableDataGrid()
		{
			DataGrid = new DataGrid();
			DataGrid.AutoGenerateColumns = false;
			DataGrid.CanUserAddRows = false;
			DataGrid.CanUserDeleteRows = false;
			DataGrid.CanUserResizeRows = false;

			SetCurrentValue(SourceProperty, new ArrayList());
			SetCurrentValue(ColumnsProperty, new ObservableCollection<DataGridColumn>());

			FilterInstructions = new List<FilterInstruction>
			{
				new FilterInstruction(),
			};
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the underlying data grid.
		/// </summary>
		public DataGrid DataGrid
		{
			get { return _dataGrid; }
			private set
			{
				_dataGrid = value;
			}
		}

		/// <summary>
		/// Gets or sets the filterable columns.
		/// </summary>
		public IList<FilterableColumn> FilterableColumns
		{
			get { return _filterableColumns; }
			set
			{
				_filterableColumns = value;
			}
		}

		/// <summary>
		/// Gets the filter instructions.
		/// </summary>
		public IList<FilterInstruction> FilterInstructions
		{
			get { return _filterInstructions; }
			private set
			{
				_filterInstructions = value;
			}
		}

		/// <summary>
		/// Gets the execute filter command.
		/// </summary>
		public ICommand Execute
		{
			get
			{
				return _execute ?? (_execute = new ModelCommand(x => _sourcePresenter.Refresh(), x => _sourcePresenter != null));
			}
		}

		#endregion

		#region Private methods

		private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilterableDataGrid filterableDataGrid = (FilterableDataGrid)d;

			if (filterableDataGrid.Source == null)
			{
				return;
			}

			filterableDataGrid._sourcePresenter = CollectionViewSource.GetDefaultView(filterableDataGrid.Source);
			filterableDataGrid._sourcePresenter.Filter = filterableDataGrid.CollectionFilterPredicate;
			filterableDataGrid._dataGrid.ItemsSource = filterableDataGrid._sourcePresenter;

			//TODO: what to do in case of nonGeneric lists??
			if (filterableDataGrid.Source is IEnumerable &&
				filterableDataGrid.Source.GetType().GetGenericArguments().Length > 0)
			{
				filterableDataGrid._modelProperties = filterableDataGrid.Source.GetType().GetGenericArguments()[0].GetProperties();
			}
		}

		private static void ColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilterableDataGrid filterableDataGrid = (FilterableDataGrid)d;

			INotifyCollectionChanged oldList = e.OldValue as INotifyCollectionChanged;
			INotifyCollectionChanged newList = e.NewValue as INotifyCollectionChanged;

			if (oldList != null)
			{
				oldList.CollectionChanged -= filterableDataGrid.OnColumnsPropertyItemsCollectionChanged;
			}

			if (newList != null)
			{
				newList.CollectionChanged += filterableDataGrid.OnColumnsPropertyItemsCollectionChanged;
			}
		}

		private void OnColumnsPropertyItemsCollectionChanged(object source, NotifyCollectionChangedEventArgs e)
		{
			foreach (DataGridColumn dgc in e.NewItems)
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						DataGrid.Columns.Add(dgc);
						break;
					case NotifyCollectionChangedAction.Remove:
						DataGrid.Columns.Remove(dgc);
						break;
				}
			}

			FilterableColumns = new List<FilterableColumn>();

			//TODO: convert these to strategies
			foreach (DataGridColumn dgc in Columns)
			{
				string header = dgc.Header.ToString();
				string path = string.Empty;

				if (dgc is DataGridTextColumn)
				{
					Binding t = (dgc as DataGridTextColumn).Binding as Binding;
					path = t.Path.Path;
				}
				else if (dgc is DataGridCheckBoxColumn)
				{
					Binding t = (dgc as DataGridCheckBoxColumn).Binding as Binding;
					path = t.Path.Path;
				}
				else if (dgc is DataGridComboBoxColumn)
				{
					//Binding t = (dgc as DataGridComboBoxColumn). as Binding;
					path = dgc.SortMemberPath;
				}
				else if (dgc is DataGridHyperlinkColumn)
				{
					Binding t = (dgc as DataGridHyperlinkColumn).Binding as Binding;
					path = t.Path.Path;
				}

				FilterableColumns.Add(new FilterableColumn { Caption = header, ModelPath = path });
			}
		}

		//TODO: incorporate filter predicates! currently searching only against string columns as PoC.
		private bool CollectionFilterPredicate(object obj)
		{
			//if (_filterInstruction != null &&
			//	_filterInstruction.Column != null &&
			//	_filterInstruction.Column.ModelPath != null)
			//{
			//	//TODO: get property info for nested objects, extract to handler
			//	PropertyInfo property = obj.GetType().GetProperty(_filterInstruction.Column.ModelPath.ToString());
			//	object value = property.GetValue(obj);


			//	if (value.ToString().Contains(_filterInstruction.Value.ToString()))
			//	{
			//		return true;
			//	}

			//	return false;
			//}

			return true;
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
