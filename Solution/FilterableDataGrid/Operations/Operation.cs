using System;

namespace DProject.Controls.FilterableDataGrid.Operations
{
	/// <summary>
	/// Abstract class representing base filter operation.
	/// </summary>
	public abstract class Operation
	{
		/// <summary>
		/// Gets the display text of an operation.
		/// </summary>
		public string DisplayText { get; protected set; }

		/// <summary>
		/// Gets the type(s) that this operation applies to.
		/// </summary>
		public Type AppliesTo { get; protected set; }																	//TODO: should be a list of similar types (int, long, decimal....)

		/// <summary>
		/// Determines whether the operation is applicable on the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// <c>true</c> if operation is applicable to the specified value; otherwise, <c>false</c>.
		/// </returns>
		public abstract bool IsApplicable(object value);
		/// <summary>
		/// Executes the operation on provided values.
		/// </summary>
		/// <param name="dgValue">The value from data grid column.</param>
		/// <param name="conditionValue">The value from filter condition.</param>
		/// <returns>
		/// <c>true</c> if provided values satisfy the predicate; otherwise, <c>false</c>.
		/// </returns>
		public abstract bool Execute(object dgValue, object conditionValue);
	}
}
