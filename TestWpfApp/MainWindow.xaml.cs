using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

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

        private async void SearchImage_Click(object sender, RoutedEventArgs e)
        {
            string query = ImageSearchInput.Text.Trim();
            if (string.IsNullOrEmpty(query))
                return;

            string imageUrl = await GetFirstGoogleImageUrl(query);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                bitmap.EndInit();
                ResultImage.Source = bitmap;
            }
            else
            {
                ResultImage.Source = null;
                MessageBox.Show("Nenhuma imagem encontrada.");
            }
        }

        private async Task<string> GetFirstGoogleImageUrl(string query)
        {
            string searchUrl = $"https://www.google.com/search?tbm=isch&q={Uri.EscapeDataString(query)}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                try
                {
                    var html = await client.GetStringAsync(searchUrl);

                    // Regex para encontrar URLs de imagens (src="...")
                    var match = Regex.Match(html, @"<img[^>]+src=""(https://[^""]+)""", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
                catch
                {
                    // Trate erros de rede
                }
            }
            return null;
        }
    }
}