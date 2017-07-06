using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DProject.Controls.FiterableDataGrid.Infrastructure;

namespace DProject.Controls.FiterableDataGrid
{
	/// <summary>
	/// DataGrid inspired custom control with generic filtering capability.
	/// </summary>
	/// <seealso cref="System.Windows.Controls.Control" />
	public class FilterableDataGrid : Control
	{
		#region Private fields

		private ICommand _execute;

		private string _searchTerm;
		private DataGrid _dataGrid;
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
		/// Gets or sets the search term.
		/// </summary>
		public string SearchTerm
		{
			get { return _searchTerm; }
			set
			{
				_searchTerm = value;
			}
		}

		/// <summary>
		/// Gets the execute filter command.
		/// </summary>
		public ICommand Execute
		{
			get
			{
				if (_execute == null)
				{
					_execute = new ModelCommand(x => _sourcePresenter.Refresh(), x => _sourcePresenter != null);
				}

				return _execute;
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
		}

		private static void ColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilterableDataGrid filterableDataGrid = (FilterableDataGrid)d;

			INotifyCollectionChanged oldList = e.OldValue as INotifyCollectionChanged;
			INotifyCollectionChanged newList = e.NewValue as INotifyCollectionChanged;

			if (oldList != null)
			{
				oldList.CollectionChanged -= filterableDataGrid.OnItemsCollectionChanged;
			}

			if (newList != null)
			{
				newList.CollectionChanged += filterableDataGrid.OnItemsCollectionChanged;
			}
		}

		private void OnItemsCollectionChanged(object source, NotifyCollectionChangedEventArgs e)
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
		}

		//TODO: incorporate filter predicates! currently searching only against string columns as PoC.
		private bool CollectionFilterPredicate(object obj)
		{
			if (string.IsNullOrWhiteSpace(_searchTerm))
			{
				return true;
			}

			PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				if (propertyInfo.PropertyType.Equals(typeof(string)))
				{
					if (((string)propertyInfo.GetValue(obj)).Contains(_searchTerm))
					{
						return true;
					}
				}
			}

			return false;
		}

		#endregion
	}
}
