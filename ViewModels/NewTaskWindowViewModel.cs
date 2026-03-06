using Fokus.DataService;
using Fokus.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Fokus.ViewModels
{
    public class NewTaskWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // =====================
        // Serviço de dados
        // =====================
        private readonly TaskDataService _taskDataService;

        // =====================
        // Tasks
        // =====================
        private ObservableCollection<Fokus.Model.Task> _tasks;
        public ObservableCollection<Fokus.Model.Task> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(nameof(Tasks)); }
        }

        // =====================
        // Propriedades da Task
        // =====================
        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        private TaskState _taskState;
        public TaskState TaskState
        {
            get => _taskState;
            set { _taskState = value; OnPropertyChanged(nameof(TaskState)); }
        }

        private TaskCategory _taskCategory;
        public TaskCategory TaskCategory
        {
            get => _taskCategory;
            set { _taskCategory = value; OnPropertyChanged(nameof(TaskCategory)); }
        }

        private TaskImportance _selectedImportance;
        public TaskImportance SelectedImportance
        {
            get => _selectedImportance;
            set { _selectedImportance = value; OnPropertyChanged(nameof(SelectedImportance)); }
        }

        public IEnumerable<TaskImportance> TaskImportanceOptions =>
            Enum.GetValues(typeof(TaskImportance)).Cast<TaskImportance>();

        // =====================
        // CheckList
        // =====================
        public ObservableCollection<TaskCheckList> TaskCheckLists { get; set; }

        public bool IsCheckListFull => TaskCheckLists.Count >= MaxCheckListItems;
        private const int MaxCheckListItems = 15;

        private string _newCheckListDescription;
        public string NewCheckListDescription
        {
            get => _newCheckListDescription;
            set { _newCheckListDescription = value; OnPropertyChanged(nameof(NewCheckListDescription)); }
        }

        // =====================
        // Timer
        // =====================
        private readonly DispatcherTimer _dispatcherTimer;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        private bool _timerEnabled = false;
        public bool TimerEnabled
        {
            get => _timerEnabled;
            set { _timerEnabled = value; OnPropertyChanged(nameof(TimerEnabled)); OnPropertyChanged(nameof(TimerButtonLabel)); }
        }

        private bool _timerRunning = false;
        public bool TimerRunning
        {
            get => _timerRunning;
            set { _timerRunning = value; OnPropertyChanged(nameof(TimerRunning)); OnPropertyChanged(nameof(TimerButtonLabel)); }
        }

        // Texto do botão muda conforme o estado
        public string TimerButtonLabel
        {
            get
            {
                if (!TimerEnabled) return "Incluir Timer";
                return TimerRunning ? "Pausar Timer" : "Iniciar Timer";
            }
        }

        // Tempo formatado para exibir na UI: 00:00:00
        private string _timerDisplay = "00:00:00";
        public string TimerDisplay
        {
            get => _timerDisplay;
            set { _timerDisplay = value; OnPropertyChanged(nameof(TimerDisplay)); }
        }

        // =====================
        // Horário DueDate
        // =====================
        private string _dueHour = "12";
        public string DueHour
        {
            get => _dueHour;
            set { _dueHour = value; OnPropertyChanged(nameof(DueHour)); }
        }

        private string _dueMinute = "00";
        public string DueMinute
        {
            get => _dueMinute;
            set { _dueMinute = value; OnPropertyChanged(nameof(DueMinute)); }
        }

        private bool _isAM = true;
        public bool IsAM
        {
            get => _isAM;
            set { _isAM = value; OnPropertyChanged(nameof(IsAM)); OnPropertyChanged(nameof(IsPM)); }
        }
        public bool IsPM
        {
            get => !_isAM;
            set { _isAM = !value; OnPropertyChanged(nameof(IsAM)); OnPropertyChanged(nameof(IsPM)); }
        }

        // =====================
        // Comandos
        // =====================
        public ICommand AddNewTask => new RelayCommand(ExecuteAddNewTask);
        public ICommand AddCheckListItem => new RelayCommand(AddItem);
        public ICommand RemoveCheckListItem => new RelayCommand<TaskCheckList>(RemoveItem);
        public ICommand ToggleTimerCommand => new RelayCommand(ToggleTimer);
        public ICommand ResetTimerCommand => new RelayCommand(ResetTimer);
        public ICommand AddToCalendarCommand => new RelayCommand(() => { /* lógica do calendário */ });
        public ICommand OpenCalendarCommand => new RelayCommand(() => { /* abrir calendário externo */ });

        public ICommand RemoveCompleted => new RelayCommand(() =>
        {
            var completed = TaskCheckLists.Where(x => x.IsCompleted).ToList();
            foreach (var item in completed)
                TaskCheckLists.Remove(item);
            OnPropertyChanged(nameof(IsCheckListFull));
        });

        public ICommand SelectAll => new RelayCommand(() =>
        {
            foreach (var item in TaskCheckLists)
                item.IsCompleted = true;
        });

        public ICommand DeselectAll => new RelayCommand(() =>
        {
            foreach (var item in TaskCheckLists)
                item.IsCompleted = false;
        });

        public ICommand SelectAllBelow => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i < TaskCheckLists.Count; i++)
                TaskCheckLists[i].IsCompleted = true;
        });

        public ICommand SelectAllAbove => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i >= 0; i--)
                TaskCheckLists[i].IsCompleted = true;
        });

        public ICommand DeselectAllBellow => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i < TaskCheckLists.Count; i++)
                TaskCheckLists[i].IsCompleted = false;
        });

        public ICommand DeselectAllAbove => new RelayCommand<TaskCheckList>(item =>
        {
            var index = TaskCheckLists.IndexOf(item);
            for (int i = index; i >= 0; i--)
                TaskCheckLists[i].IsCompleted = false;
        });

        // =====================
        // Construtor
        // =====================
        public NewTaskWindowViewModel()
        {
            _taskDataService = new TaskDataService();
            TaskCheckLists = new ObservableCollection<TaskCheckList>();
            Tasks = new ObservableCollection<Fokus.Model.Task>(_taskDataService.LoadTasks());
            DueDate = DateTime.Now;

            // Inicializa o DispatcherTimer (tick a cada 1 segundo)
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += OnTimerTick;
        }

        // =====================
        // Lógica do Timer
        // =====================

        // Chamado a cada segundo
        private void OnTimerTick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            TimerDisplay = _elapsedTime.ToString(@"hh\:mm\:ss");
        }

        // 1º clique → ativa e inicia | 2º → pausa | 3º → retoma
        private void ToggleTimer()
        {
            if (!TimerEnabled)
            {
                TimerEnabled = true;
                TimerRunning = true;
                _dispatcherTimer.Start();
                return;
            }

            if (TimerRunning)
            {
                _dispatcherTimer.Stop();
                TimerRunning = false;
            }
            else
            {
                _dispatcherTimer.Start();
                TimerRunning = true;
            }
        }

        // Zera o timer sem desativar
        private void ResetTimer()
        {
            _dispatcherTimer.Stop();
            TimerRunning = false;
            _elapsedTime = TimeSpan.Zero;
            TimerDisplay = "00:00:00";
        }

        // =====================
        // Métodos privados
        // =====================
        private void ExecuteAddNewTask()
        {
            if (string.IsNullOrWhiteSpace(Title)) return;

            // Para o timer ao salvar e guarda o tempo decorrido
            _dispatcherTimer.Stop();

            var newTask = new Fokus.Model.Task
            {
                Title = Title,
                Description = Description,
                Id = _taskDataService.GenerateNewTaskId(),
                DueDate = DueDate,
                IsCompleted = false,
                StartDate = DateTime.Now,
                TaskCategory = TaskCategory,
                TaskCheckLists = TaskCheckLists,
                TaskImportance = SelectedImportance,
                TaskState = TaskState,
                Timer = _elapsedTime, // salva o tempo registrado
            };

            _taskDataService.AddTask(newTask);
            Tasks = new ObservableCollection<Fokus.Model.Task>(_taskDataService.LoadTasks());
            OnPropertyChanged(nameof(Tasks));
        }

        private void AddItem()
        {
            if (string.IsNullOrEmpty(NewCheckListDescription)) return;

            if (TaskCheckLists.Count >= MaxCheckListItems)
            {
                MessageBox.Show("Máximo de itens atingido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            TaskCheckLists.Add(new TaskCheckList { Description = NewCheckListDescription, IsCompleted = false });
            NewCheckListDescription = string.Empty;
            OnPropertyChanged(nameof(IsCheckListFull));
        }

        private void RemoveItem(TaskCheckList item)
        {
            TaskCheckLists.Remove(item);
            OnPropertyChanged(nameof(IsCheckListFull));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}