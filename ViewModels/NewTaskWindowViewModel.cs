using Fokus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fokus.Views;
using Fokus.DataService;

namespace Fokus.ViewModels
{
   public class NewTaskWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; // evento que é acionado quando uma propriedade é alterada.

        public ObservableCollection<TaskCheckList> TaskCheckLists { get; set; } = new ObservableCollection<TaskCheckList>(); // cria uma lista de checklists para a task.

        public ICommand AddCheckListItem => new RelayCommand(AddItem); 
        public ICommand RemoveCheckListItem => new RelayCommand<TaskCheckList>(item => TaskCheckLists.Remove(item));
        public IEnumerable<TaskImportance> TaskImportanceOptions => Enum.GetValues(typeof(TaskImportance)).Cast<TaskImportance>();

        private void AddItem()
        {
            if (string.IsNullOrEmpty(NewCheckListDescription)) return; 
            TaskCheckLists.Add(new TaskCheckList { Description = NewCheckListDescription, IsCompleted = false }); // adiciona um novo item ao checklist com uma descrição padrão e status de completude falso.)
            NewCheckListDescription = string.Empty; // limpa a descrição do novo item após adicioná-lo.
        }

        protected virtual void OnPropertyChanged(string propertyName) // método que é chamado para notificar a view sobre mudanças nas propriedades do ViewModel.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // verifica se o evento PropertyChanged é nulo e, se não for, invoca o evento passando o nome da propriedade que foi alterada.
        }

        private void RemoveItem(TaskCheckList item)
        {
            TaskCheckLists.Remove(item);
        }

        private string _newCheckListDescription; // campo privado para armazenar a descrição do novo item do checklist.
        public string NewCheckListDescription
        {
            get => _newCheckListDescription;
            set
            {
                _newCheckListDescription = value;
                OnPropertyChanged(nameof(NewCheckListDescription)); // chama o método OnPropertyChanged para notificar a view sobre a mudança na propriedade NewCheckListDescription.
            }
        }
        public ICommand SelectAllBelow => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i < TaskCheckLists.Count; i++)
                TaskCheckLists[i].IsCompleted = true;
        });

        public ICommand SelectAll => new RelayCommand(() =>
        {
            foreach (var item in TaskCheckLists)
                item.IsCompleted = true;
        });

        public ICommand SelectAllAbove => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i >= 0; i--)
                TaskCheckLists[i].IsCompleted = true;
        });

        public ICommand RemoveCompleted => new RelayCommand(() =>
        {
            var completed = TaskCheckLists.Where(x => x.IsCompleted).ToList();
            foreach (var item in completed)
                TaskCheckLists.Remove(item);
        });

        private TaskImportance _selectedImportance; 
        public TaskImportance SelectedImportance
        {
            get => _selectedImportance;
            set
            {
                _selectedImportance = value;
                OnPropertyChanged(nameof(SelectedImportance)); // chama o método OnPropertyChanged para notificar a view sobre a mudança na propriedade SelectedImportance.
            }
        }

    }
    
}
