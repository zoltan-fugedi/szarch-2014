using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.TestClient.Utils
{
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _command;
        private readonly Func<bool> _canexecute;

        public AsyncCommand(Func<Task> command)
        {
            _command = command;
            _canexecute = null;
        }

        public AsyncCommand(Func<Task> command, Func<bool> canExecute)
        {
            _command = command;
            _canexecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            return _canexecute == null ? true : _canexecute(); ;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }

    }
}
