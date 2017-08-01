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

		#region Overrides

		/// <summary>
		/// Determines whether the operation is applicable to the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// <c>true</c> if operation is applicable to the specified value; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsApplicable(object value)
		{
			long result;
			return long.TryParse(value.ToString(), out result);
		}

		#endregion
	}
}
