namespace DProject.Controls.FilterableDataGrid.Operations
{
	/// <summary>
	/// Abastract class representing operations with numbers.
	/// </summary>
	/// <seealso cref="DProject.Controls.FilterableDataGrid.Operations.Operation" />
	public abstract class NumberOperation : Operation
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NumberOperation"/> class.
		/// </summary>
		protected NumberOperation()
		{
			AppliesTo = typeof(long);
		}

		#endregion
	}
}
