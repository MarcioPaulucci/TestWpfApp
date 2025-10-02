using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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

            // Exemplo: busca uma imagem do Unsplash (API pública para demonstração)
            string imageUrl = await GetImageUrlFromUnsplash(query);
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

        private async Task<string> GetImageUrlFromUnsplash(string query)
        {
            // Atenção: para uso real, obtenha uma chave de API do Unsplash ou outro serviço.
            // Aqui, exemplo de busca simples usando a API de imagens aleatórias do Unsplash.
            string url = $"https://source.unsplash.com/400x400/?{Uri.EscapeDataString(query)}";
            using (var client = new HttpClient())
            {
                try
                {
                    // Unsplash redireciona para a imagem
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                        return response.RequestMessage.RequestUri.ToString();
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