using Library.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Library.page
{
    /// <summary>
    /// Interaction logic for ReadersPage.xaml
    /// </summary>
    public partial class ReadersPage : Page
    {
        private string token;
        public List<Reader> readers { get; set; }
        public ReadersPage(string token)
        {
            this.InitializeComponent();

            this.token = token;

            var request = (HttpWebRequest)WebRequest.Create($"https://localhost:7256/GetReaders?token={this.token}");

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            var response = (HttpWebResponse)request.GetResponse();
            var resp = new StreamReader(response.GetResponseStream()).ReadToEnd();

            this.readers = JsonSerializer.Deserialize<List<Reader>>(resp);
            this.DataContext = new ReadersPageVM(this.readers);
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ReadersPageVM context = (ReadersPageVM)this.DataContext;

            if (this.searchTB.Text == "")
            {
                context.Readers = this.readers;
                return;
            }

            context.Readers = context.Readers.Where(r => r.FullName.ToLower().Contains(this.searchTB.Text.ToLower())).ToList();
        }

        private void CSVMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string file = "";
            file += $"ФИО\n";

            foreach (var reader in this.readers)
            {
                file += $"{reader.FullName}\n";
            }

            File.WriteAllText($@"{Directory.GetCurrentDirectory()}\readers.csv", file, Encoding.Default);

            MessageBox.Show($@"Файл сохранён по пути {Directory.GetCurrentDirectory()}\readers.csv");
        }

    
        private void btnExet_Click(object sender, RoutedEventArgs e)
        {
           if ( MessageBox.Show("Вы точно хотите выйти?", "",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new AuthorizationPage());
            }

        }
    }

    public class ReadersPageVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPorpertyChanged ([CallerMemberName] string prop = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private List<Reader> readers;
        public List<Reader> Readers
        {
            get => this.readers;
            set
            {
                this.readers = value;
                this.OnPorpertyChanged();
            }
        }

        public ReadersPageVM(List<Reader> readers)
        {
            this.Readers = readers;
        }
    }
}
