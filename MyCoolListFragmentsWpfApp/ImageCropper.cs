using MyCoolListFragmentsWpfApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MyCoolListFragmentsWpfApp
{
    internal class ImageCropper
    {
        public static void CropImage(Fragment fragment, string sourcePath, int width, int height)
        {
            try
            {
                BitmapImage sourceImage = new BitmapImage();

                using (FileStream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    sourceImage.BeginInit();
                    sourceImage.StreamSource = stream;
                    sourceImage.CacheOption = BitmapCacheOption.OnLoad;
                    sourceImage.EndInit();
                }

                sourceImage.Freeze(); // Фиксируем для использования из другого потока

                CroppedBitmap croppedImage = new CroppedBitmap(sourceImage, new Int32Rect(fragment.X, fragment.Y, width, height));

                // Сохраняем обрезанное изображение
                string directory = Path.GetDirectoryName(sourcePath);
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string newFileName = $"{fileNameWithoutExt}_crop_{fragment.Number}_{fragment.X}_{fragment.Y}{extension}";
                string newFilePath = Path.Combine(directory, newFileName);

                using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create, FileAccess.Write))
                {
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 90;
                    encoder.Frames.Add(BitmapFrame.Create(croppedImage));
                    encoder.Save(fileStream);
                }

                // Очищаем ресурсы
                croppedImage = null;
                sourceImage = null;
                // Принудительная сборка мусора
                GC.Collect(); 

                fragment.Image = newFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вырезании фрагмента: {ex.Message}");
                throw;
            }
        }
    }
}
