using Fokus.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fokus.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged // implementa a interface INotifyPropertyChanged para notificar a view sobre mudanças nas propriedades do ViewModel.
    {
        public event PropertyChangedEventHandler PropertyChanged; // evento que é acionado quando uma propriedade é alterada.

        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow); // propriedade que retorna um comando para abrir uma nova janela. O comando é criado usando a classe RelayCommand, que é uma implementação comum de ICommand.

        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow(); // cria uma nova instância da janela NewTaskWindow.
            newTaskWindow.Show(); // exibe a janela NewTaskWindow na tela.
        }

        protected virtual void OnPropertyChanged(string propertyName) // método que é chamado para notificar a view sobre mudanças nas propriedades do ViewModel.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // verifica se o evento PropertyChanged é nulo e, se não for, invoca o evento passando o nome da propriedade que foi alterada.
        }
    }
}

  
