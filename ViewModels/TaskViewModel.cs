using Fokus.DataService;
using Fokus.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Fokus.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TaskDataService _taskDataService;

        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        public TaskViewModel()
        {
            _taskDataService = new TaskDataService();
            LoadTasks();
        }

        public void LoadTasks()
        {
            Tasks = new ObservableCollection<Task>(_taskDataService.LoadTasks());
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