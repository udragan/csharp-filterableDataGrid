using System;
using System.Windows.Input;

namespace DProject.Controls.FilterableDataGrid.Infrastructure
{
	/// <summary>
	/// Implementation of <see cref="ICommand"/> to be used as base command.
	/// </summary>
	/// <seealso cref="System.Windows.Input.ICommand" />
	public class ModelCommand : ICommand
	{
		#region Private fields

		private readonly Action<object> _execute = null;
		private readonly Predicate<object> _canExecute = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelCommand"/> class.
		/// </summary>
		/// <param name="execute">The action to execute.</param>
		public ModelCommand(Action<object> execute)
			: this(execute, null)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelCommand"/> class.
		/// </summary>
		/// <param name="execute">The action to execute.</param>
		/// <param name="canExecute">The can execute predicate.</param>
		public ModelCommand(Action<object> execute, Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion

		#region ICommand Members

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// True if this command can be executed; otherwise, false.
		/// </returns>
		public bool CanExecute(object parameter)
		{
			return _canExecute != null ? _canExecute(parameter) : true;
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			if (_execute != null)
			{
				_execute(parameter);
			}
		}

		/// <summary>
		/// Called when [can execute changed].
		/// </summary>
		public void OnCanExecuteChanged()
		{
			CanExecuteChanged(this, EventArgs.Empty);
		}

		#endregion
	}
}
