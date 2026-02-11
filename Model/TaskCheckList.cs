using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fokus.Model
{
    internal class TaskCheckList
    {
        public string Description { get; set; } // cria uma descricao para o item do checklist.
        public bool IsCompleted { get; set; } // cria um status de completude para o item do checklist.
    }
}
