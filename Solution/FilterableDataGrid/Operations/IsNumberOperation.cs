namespace DProject.Controls.FilterableDataGrid.Operations
{
	/// <summary>
	/// Specialisation of number operation checking for equality.
	/// </summary>
	/// <seealso cref="DProject.Controls.FilterableDataGrid.Operations.NumberOperation" />
	public class IsNumberOperation : NumberOperation
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="IsNumberOperation"/> class.
		/// </summary>
		public IsNumberOperation()
		{
			DisplayText = "Is";
		}

		#endregion

		#region Methods

		/// <summary>
		/// Executes the operation on provided values.
		/// </summary>
		/// <param name="dgValue">The value from data grid column.</param>
		/// <param name="conditionValue">The value from filter condition.</param>
		/// <returns>
		/// <c>true</c> if provided values satisfy the predicate, otherwise <c>false</c>.
		/// </returns>
		public override bool Execute(object dgValue, object conditionValue)
		{
			long dgInt = long.Parse(dgValue.ToString());
			long conditionInt = long.Parse(conditionValue.ToString());

			return long.Equals(dgInt, conditionInt);
		}

		#endregion
	}
}
