using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCoolListFragmentsWpfApp.Models
{
    public class Fragment : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string? _image { get; set; }
        // Обработчик события
        public string? Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
        }
    }
}
