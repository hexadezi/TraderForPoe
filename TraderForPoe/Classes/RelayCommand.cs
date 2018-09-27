

using System;
using System.Windows.Input;

namespace TraderForPoe.ViewModel
{
    public class RelayCommand : ICommand
    {
        readonly Func<bool> canExecuteEvaluator;
        readonly Action methodToExecute;
        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, () => true)
        { }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return canExecuteEvaluator.Invoke();
        }
        public void Execute(object parameter)
        {
            methodToExecute.Invoke();
        }
    }
}