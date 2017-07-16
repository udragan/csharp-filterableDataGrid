using System;
using System.Collections.ObjectModel;
using System.Windows;
using DProject.TestApp.Enumerations;
using DProject.TestApp.Models;

namespace DProject.TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private fields

		ObservableCollection<SourceElement> items;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			//TODO: figure out how to call InitializeComponent() prior to assigning DataContext to view.
			//TODO: test behaviour when dataContext is assigned directly in xaml!
			InitializeComponent();
			this.DataContext = this;
			items = new ObservableCollection<SourceElement>();

			PopulateItems();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the items which will represent source for FiltereableDataGrid control.
		/// </summary>
		public ObservableCollection<SourceElement> Items
		{
			get
			{
				return items;
			}
			set
			{
				items = value;
			}
		}

		#endregion

		#region Private methods

		private void PopulateItems()
		{
			Items.Add(new SourceElement
			{
				Id = 1,
				Name = "NAME1",
				IsValid = true,
				SimpleEnumerationValue = SimpleEnum.First,
				Link = new Uri("http://www.google.com", UriKind.Absolute),
			});
			Items.Add(new SourceElement
			{
				Id = 2,
				Name = "NAME2",
				IsValid = false,
				SimpleEnumerationValue = SimpleEnum.Second,
				Link = new Uri("http://www.msn.com", UriKind.Absolute),
			});

			//for (int i = 3; i < 1000000; i++)
			//{
			//	Items.Add(new SourceElement
			//		{
			//			Id = i,
			//			Name = "Name" + i,
			//			IsValid = i % 2 == 0,
			//			SimpleEnumerationValue = (SimpleEnum)(i % 3),
			//		});
			//}
		}

		#endregion
	}
}
