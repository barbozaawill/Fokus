using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fokus.Model
{
    public class Task
    {
        public int Id { get; set; } // cria um Id para a task.
        public string Title { get; set; } // cria um titulo para a task.
        public string Description { get; set; } // cria uma descricao para a task.
        public DateTime DueDate { get; set; } // cria uma data de vencimento para a task.
        public DateTime StartDate { get; set; } // cria uma data de inicio para a task.
        public bool IsCompleted { get; set; } // cria um status de completude para a task.
        public TimeSpan Timer { get; set; } // cria um timer para a task.
        public TaskState TaskState { get; set; } // cria um estado para a task.
        public TaskImportance TaskImportance { get; set; } // cria uma importancia para a task.
        public TaskCategory TaskCategory { get; set; } // cria uma categoria para a task.
        //public ObservableCollection<TaskCheckList> TaskCheckLists { get; set; } // cria uma lista de checklists para a task.
    }

    //Agora vamos criar os ENUMS para TaskState, TaskImportance e TaskCategory. Eles servem para definir valores fixos para essas propriedades.

    public enum TaskState // cria um enum para o estado da task.
    {
        NotStarted,
        InProgress,
        Completed,
        Deleted,
        Late,
        Archived
    }

    public enum TaskImportance // cria um enum para a importancia da task.
    {
        Low,
        Med,
        High,
        Crit
    }

    public enum TaskCategory // cria um enum para a categoria da task.
    {
        Work,
        Personal,
        Shopping,
        Health,
        Finance,
        Education,
        Other
    }
}
