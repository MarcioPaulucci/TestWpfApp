using System.Collections.ObjectModel;
using System.Windows;

namespace TestWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TodoItem> Tasks { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            TodoList.ItemsSource = Tasks;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var desc = TaskInput.Text.Trim();
            if (!string.IsNullOrEmpty(desc))
            {
                Tasks.Add(new TodoItem { Description = desc });
                TaskInput.Text = "";
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is TodoItem item)
            {
                Tasks.Remove(item);
            }
        }
    }
}