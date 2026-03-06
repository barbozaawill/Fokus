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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TaskViewModel _taskViewModel;

        public TaskViewModel TaskViewModel => _taskViewModel; // expõe para o XAML bindar

        public ICommand IOpenNewWindow => new RelayCommand(OpenNewWindow);

        public MainWindowViewModel()
        {
            _taskViewModel = new TaskViewModel();
        }

        private void OpenNewWindow()
        {
            var newTaskWindow = new NewTaskWindow();
            newTaskWindow.ShowDialog(); // ← aguarda fechar
            _taskViewModel.LoadTasks(); // ← recarrega as tasks
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

  
