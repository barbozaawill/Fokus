using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fokus.ViewModels
{
    internal class RelayCommand : ICommand // classe para criar comandos personalizados para a interface do usuário.
    {
        private readonly Action _execute; // ação que vai ser executada quando o comando for acionado.
        private readonly Func<bool> _canExecute; // função que determina se o comando pode ser executado.

        public RelayCommand(Action execute, Func<bool> canExecute = null) // construtor que recebe a ação a ser executada e a função que determina se o comando pode ser executado.
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute)); // verifica se a ação é nula e, se for, lança uma exceção.
            _canExecute = canExecute; // atribui a função que determina se o comando pode ser executado.
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; } // adiciona um manipulador de eventos para o evento RequerySuggested do CommandManager, que é acionado quando o estado de execução do comando pode ter mudado.
            remove { CommandManager.RequerySuggested -= value; } // remove um manipulador de eventos para o evento RequerySuggested do CommandManager.
        }

        public bool CanExecute(object parameter) // método que determina se o comando pode ser executado.
        {
            return _canExecute == null || _canExecute(); // retorna true se a função que determina se o comando pode ser executado for nula ou se a função retornar true.
        }

        public void Execute(object parameter) // método que executa a ação do comando.
        {
            _execute(); // executa a ação do comando.
        }
    }
    
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute; // ação que vai ser executada quando o comando for acionado, com um parâmetro do tipo T.
        private readonly Predicate<T> _canExecute; // função que determina se o comando pode ser executado, com um parâmetro do tipo T.

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null) // construtor que recebe a ação a ser executada e a função que determina se o comando pode ser executado.
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute)); // verifica se a ação é nula e, se for, lança uma exceção.
            _canExecute = canExecute; // atribui a função que determina se o comando pode ser executado.
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value; // adiciona um manipulador de eventos para o evento RequerySuggested do CommandManager, que é acionado quando o estado de execução do comando pode ter mudado.
            remove => CommandManager.RequerySuggested -= value; // remove um manipulador de eventos para o evento RequerySuggested do CommandManager.
        }

        public bool CanExecute(object parameter) // método que determina se o comando pode ser executado.
        {
            return _canExecute == null || _canExecute((T)parameter); // retorna true se a função que determina se o comando pode ser executado for nula ou se a função retornar true.
        }

        public void Execute(object parameter) // método que executa a ação do comando.
        {
            _execute((T)parameter); // executa a ação do comando.
        }
    }
}
