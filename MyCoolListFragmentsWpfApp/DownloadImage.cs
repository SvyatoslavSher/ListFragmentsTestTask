using MyCoolListFragmentsWpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace MyCoolListFragmentsWpfApp
{
    public class DownloadImage
    {
        // Метод вызывается при нажатии кнопки - запускает фоновые загрузки
        public static void StartDownloadImages(List<Fragment> fragments, List<string> urls, int width, int height)
        {
            Task.Run(() => DownloadAllImagesAsync(fragments, urls, width, height));
        }

        // Асинхронный метод для загрузки всех изображений
        public static async Task DownloadAllImagesAsync(List<Fragment> fragments, List<string> urls, int width, int height)
        {
            int count = 1;
            var downloadTasks = new List<Task>();
            HashSet<int> seenNumbers = new();

            foreach (var fragment in fragments)
            {
                string savePath = @$"C:\Users\hugeglasses\Desktop\ListFragmentsWPF\MyCoolListFragmentsWpfApp\MyCoolListFragmentsWpfApp\bin\Debug\net9.0-windows\Resources\Image{fragment.Number}.jpg";

                if (seenNumbers.Add(fragment.Number)) // Только уникальные
                {
                    var downloadTask = DownloadImageAsync(urls[fragment.Number], count, savePath, fragment, width, height);

                    if (count % 2 == 0)
                        Thread.Sleep(10000); // Имитация долго работы

                    downloadTasks.Add(downloadTask);
                    count++;
                }
            }

            // Все загрузки выполняются параллельно
            await Task.WhenAll(downloadTasks);
        }

        public static async Task DownloadImageAsync(string url, int count, string savePath, Fragment fragment, int width, int height)
        {
            try
            {
                using (HttpClient httpClient = new())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                    byte[] imageBytes = await httpClient.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(savePath, imageBytes);

                    ImageCropper.CropImage(fragment, savePath, width, height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки {count}: {ex.Message}");
            }
        }
    }
}
