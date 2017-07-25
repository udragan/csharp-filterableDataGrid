namespace DProject.Controls.FiterableDataGrid.Operations
{
	/// <summary>
	/// Specialisation of number operation checking for equality.
	/// </summary>
	/// <seealso cref="DProject.Controls.FiterableDataGrid.Operations.NumberOperation" />
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
		/// True if provided values satisfy the predicate, false otherwise.
		/// </returns>
		public override bool Execute(object dgValue, object conditionValue)
		{
			int dgInt = int.Parse(dgValue.ToString());
			int conditionInt = int.Parse(conditionValue.ToString());

			return int.Equals(dgInt, conditionInt);
		}

		#endregion
	}
}
