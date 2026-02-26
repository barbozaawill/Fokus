using Fokus.DataService;
using Fokus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace Fokus.ViewModels
{
    internal class TaskViewModel : INotifyPropertyChanged
    {
        private readonly TaskDataService _taskDataService;

        private ObservableCollection<Task> _task;

        public ObservableCollection<Task> Tasks
        {
            get => _task;
            set  
            {
                _task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskViewModel()
        {
            _taskDataService = new TaskDataService();
        }

        private void LoadTasks()
        {
            var TaskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(TaskList);
        }

        public void AddNewTask(Task newTask)
        {
            _taskDataService.AddTask(newTask);
            LoadTasks();
        }

        public void UpdateTask(Task updateTask)
        {
            _taskDataService?.UpdateTask(updateTask);
            LoadTasks();
        }

        public void DeleteTask(int taskId)
        {
            _taskDataService.DeleteTasks(taskId);
            LoadTasks();
        }
        protected virtual void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
