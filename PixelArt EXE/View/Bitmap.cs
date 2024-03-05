using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelArt.View
{
    internal class Bitmap : INotifyPropertyChanged
    {
        /* Для меня. Вызов и Инит класса
         * 
         *  Bitmap m = new Bitmap();
         *  m.PropertyChanged += bmp_PropertyChanged;
         *  public static void bmp_PropertyChanged(object sender, EventArgs e)
         *  {
         *     Console.WriteLine($"The property was reached. Message = {mess}");
         *  }
         * 
         */
        private int _progress = -1;
        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
