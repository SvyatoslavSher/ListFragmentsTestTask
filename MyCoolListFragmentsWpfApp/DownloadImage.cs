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
        public static void StartDownloadImages(List<Fragment> fragments, List<string> urls, int width, int height)
        {
            Task.Run(() => DownloadAllImagesAsync(fragments, urls, width, height));
        }

        // Асинхронный метод для загрузки всех изображений
        public static async Task DownloadAllImagesAsync(List<Fragment> fragments, List<string> urls, int width, int height)
        {
            int count = 1;
            var downloadTasks = new List<Task>();
            // Добавляем список для задач обрезки
            var cropTasks = new List<Task>();
            // Кэш для хранения путей к загруженным изображениям
            Dictionary<int, string> downloadedImages = new();

            foreach (var fragment in fragments)
            {
                string savePath = @$"C:\Users\hugeglasses\Desktop\ListFragmentsWPF\MyCoolListFragmentsWpfApp\MyCoolListFragmentsWpfApp\bin\Debug\net9.0-windows\Resources\Image{fragment.Number}.jpg";

                // Проверяем, есть ли уже загруженное изображение
                if (!downloadedImages.ContainsKey(fragment.Number)) 
                {
                    var downloadTask = DownloadImageAsync(urls[fragment.Number], count, savePath, fragment, width, height);

                    // Добавляем продолжение, которое сохранит путь в кэш после загрузки
                    downloadTask.ContinueWith(t => {
                        if (t.Status == TaskStatus.RanToCompletion)
                        {
                            downloadedImages[fragment.Number] = savePath;
                        }
                    });

                    if (count % 2 == 0)
                        Thread.Sleep(10000); // Имитация работы

                    downloadTasks.Add(downloadTask);
                    count++;
                }
                else
                {
                    // Для дубликата создаем отдельную задачу кроппинга
                    var cropTask = Task.Run(() =>
                    {
                        ImageCropper.CropImage(fragment, downloadedImages[fragment.Number], width, height);
                    });
                    cropTasks.Add(cropTask);
                }
            }

            // Сначала ждем завершения всех загрузок
            await Task.WhenAll(downloadTasks);

            // Затем ждем завершения всех операций обрезки для дубликатов
            await Task.WhenAll(cropTasks);
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
