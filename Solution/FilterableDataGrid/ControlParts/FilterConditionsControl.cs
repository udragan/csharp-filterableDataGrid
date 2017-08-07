using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using DProject.Controls.FilterableDataGrid.Models;

namespace DProject.Controls.FilterableDataGrid.ControlParts
{
	/// <summary>
	/// Custom control representing a collection of <see cref="FilterCondition"/>
	/// </summary>
	/// <seealso cref="System.Windows.Controls.ItemsControl" />
	public class FilterConditionsControl : ItemsControl
	{
		#region Dependency properties

		/// <summary>
		/// Gets or sets the filter conditions.
		/// </summary>
		public ObservableCollection<FilterCondition> FilterConditions
		{
			get { return (ObservableCollection<FilterCondition>)GetValue(FilterConditionsProperty); }
			set { SetValue(FilterConditionsProperty, value); }
		}
		//TODO: should be ordinary property? because only binding is needed in control view.
		/// <summary>
		/// The filter conditions dependency property.
		/// </summary>
		public static readonly DependencyProperty FilterConditionsProperty =
			DependencyProperty.Register("FilterConditions", typeof(ObservableCollection<FilterCondition>), typeof(FilterConditionsControl), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the source model properties.
		/// </summary>
		public IDictionary<string, PropertyInfo> SourceModelProperties
		{
			get { return (IDictionary<string, PropertyInfo>)GetValue(SourceModelPropertiesProperty); }
			set { SetValue(SourceModelPropertiesProperty, value); }
		}
		//TODO: should be ordinary property? because only binding is needed in control view.
		/// <summary>
		/// The source model properties dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceModelPropertiesProperty =
			DependencyProperty.Register("SourceModelProperties", typeof(IDictionary<string, PropertyInfo>), typeof(FilterConditionsControl), new PropertyMetadata(null, SourceModelPropertiesPropertyChanged));

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="FilterConditionsControl"/> class.
		/// </summary>
		static FilterConditionsControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterConditionsControl),
				new FrameworkPropertyMetadata(typeof(FilterConditionsControl)));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterConditionsControl"/> class.
		/// </summary>
		public FilterConditionsControl()
		{
			SetCurrentValue(FilterConditionsProperty, new ObservableCollection<FilterCondition>());
			SetCurrentValue(SourceModelPropertiesProperty, new Dictionary<string, PropertyInfo>());

			//FilterConditions = new ObservableCollection<FilterCondition>
			//{
			//	new FilterCondition(new List<FilterableColumn>
			//	{
			//		new FilterableColumn
			//		{
			//			Caption = "c1"
			//		}
			//	}),
			//};
		}

		#endregion

		#region Private methods

		/// <remarks>
		/// TODO: can be removed, SourceModelProperties will become ordinary property.
		/// </remarks>
		private static void SourceModelPropertiesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FilterConditionsControl filterConditionsControl = (FilterConditionsControl)d;

			if (filterConditionsControl.SourceModelProperties != null &&
				filterConditionsControl.SourceModelProperties.Count > 0)
			{

			}
		}

		#endregion
	}
}
