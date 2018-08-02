using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokerNirvana_MVVM_ADO.ViewModel
{
    public class Command : ICommand
    {
        readonly Action<object> actionAExecuter;

        public Command(Action<object> execute) : this(execute, null) { }
        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            actionAExecuter = execute;
        }

        public bool CanExecute(object param)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object param)
        {
            actionAExecuter(param);
        }

    }
}
