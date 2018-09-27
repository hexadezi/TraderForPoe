using System;
using System.Windows.Input;

/// <summary>
/// Many thanks to Artentus for providing this useful class
/// https://www.vb-paradise.de/index.php/Thread/108687-Grundlagen-Commands/
/// </summary>
namespace TraderForPoe.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Func<bool> canExecuteEvaluator;
        private readonly Action methodToExecute;

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