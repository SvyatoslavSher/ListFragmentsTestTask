using MyCoolListFragmentsWpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyCoolListFragmentsWpfApp
{
    public class GenerationListFragments
    {
        public static List<Fragment> Generation(int countFragment, int countUrls) 
        {
            List<Fragment> fragmentList = new();
            Random rnd = new Random();

            for (int i = 0; i < countFragment; i++)
            {
                fragmentList.Add(new Fragment()
                {
                    Number = rnd.Next(countUrls),
                    X = rnd.Next(0, 600),
                    Y = rnd.Next(0, 600),
                    // Заглушка
                    Image = "C:\\Users\\hugeglasses\\Desktop\\ListFragmentsWPF\\MyCoolListFragmentsWpfApp\\MyCoolListFragmentsWpfApp\\Resources\\default.jpg"
                });
            }
            return fragmentList;
        }
    }
}
