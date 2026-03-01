using MyCoolListFragmentsWpfApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCoolListFragmentsWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> urls = new()
        {
                "https://upload.wikimedia.org/wikipedia/commons/9/9d/Stones-in-water.jpg",
                "http://www.kryzuy.com/wp-content/uploads/2015/12/KryzUy.Com-Sumilon-Island-Blue-Water-Resort-2.jpg",
                "https://img.freepik.com/free-photo/reflection-of-rocky-mountains-and-sky-on-beautiful-lake_23-2148153610.jpg?w=2000&t=st=1707286026~exp=1707286626~hmac=33cfd04e4e898e7a58210bba5129a7e07a5c4bde22abf12467472f3073d6dd0e",
                "https://img.freepik.com/free-photo/vestrahorn-mountains-in-stokksnes-iceland_335224-667.jpg?w=2000&t=st=1707286136~exp=1707286736~hmac=e87482823b94ab19abc794f7eafcb0356b97e0bbf563aec15dd4596f026aff72",
                "https://img.freepik.com/free-photo/pier-at-a-lake-in-hallstatt-austria_181624-44201.jpg?w=2000&t=st=1707286209~exp=1707286809~hmac=9e29fcb6cf717450de3ebe4b34222b356166d91e6ad651fedbfa63a2822b4f77"
        };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void generationListFragments_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(widthFragmentTB.Text) || string.IsNullOrWhiteSpace(heightFragmentTB.Text) || string.IsNullOrWhiteSpace(countFragmentTB.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            int width = int.Parse(widthFragmentTB.Text.Replace(" ", ""));
            int height = int.Parse(heightFragmentTB.Text.Replace(" ", ""));
            int count = int.Parse(countFragmentTB.Text.Replace(" ", ""));

            List<Fragment> fragmentList = new();

            // Получаем сгенеренный список фрагментов
            fragmentList = GenerationListFragments.Generation(count, urls.Count);
            
            // Заполнение dataGrid и itemsControl
            FragmentsDG.ItemsSource = fragmentList;
            MainItemsControl.ItemsSource = fragmentList;

            // Качаем картиночки
            DownloadImage.StartDownloadImages(fragmentList, urls, width, height);
        }
        // Запрет ввода букв
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}