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
                sourceImage.BeginInit();
                sourceImage.UriSource = new Uri(sourcePath, UriKind.Absolute);
                sourceImage.CacheOption = BitmapCacheOption.OnLoad;
                sourceImage.EndInit();

                CroppedBitmap croppedImage = new CroppedBitmap(sourceImage, new Int32Rect(fragment.X, fragment.Y, width, height));

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 90;
                encoder.Frames.Add(BitmapFrame.Create(croppedImage));

                using (FileStream fileStream = new FileStream(sourcePath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fileStream);
                }

                croppedImage.Freeze();
                fragment.Image = sourcePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вырезании фрагмента: {ex.Message}");
                throw;
            }
        }
    }
}
