using System;
using System.Windows.Input;

namespace ServiceRunnerApp {
    public class NotifyIconCommandBase : ICommand {
        public void Execute(object parameter) {
            Invoked?.Invoke(this, null);
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler Invoked;
        public event EventHandler CanExecuteChanged;
    }

    public class OpenAppCommand : NotifyIconCommandBase {
        public static OpenAppCommand Instance;
    }
    public class QuitCommand : NotifyIconCommandBase {
        public static QuitCommand Instance;
    }
}
