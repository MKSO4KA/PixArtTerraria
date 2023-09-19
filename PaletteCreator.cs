using System;
using System.Windows.Forms;

namespace PixelArt
{
    internal static class Palette
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
            //Application.Run(new PixelArtVisual());
            //Console.WriteLine("assdsaa");
        }

    }
}

