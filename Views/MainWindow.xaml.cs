using Fokus.ViewModels;
using Fokus.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fokus
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskViewModel _taskViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _taskViewModel = new TaskViewModel();
            this.DataContext = _taskViewModel;
        }

        private void AbrirNovaTask(object sender, RoutedEventArgs e)
        {
            var newTaskWindow = new NewTaskWindow();
            newTaskWindow.ShowDialog(); // aguarda fechar
            _taskViewModel.LoadTasks(); // recarrega as tasks
        }
    }
}
