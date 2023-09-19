/*
 * using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

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
            Console.WriteLine("assdsaa");
        }

        
        public static string[] fileREAD(string filepath)
        {
            string[] arr = File.ReadAllLines(filepath);
            return arr;
        }
        public static (double, double, double)[] GetHueMass()
        {
            /* Необходимо отметить лишь фото и файл
             * 
             * 
             

            var array = Tools.ReadPhoto(Tools.Filepath_Dialog("Photo"));
            var lst = new (double,double,double) [array.Length];
            int index = 0;
            foreach (var item in array)
            {
                
                double hue;
                double saturation;
                double value;
                Tools.ColorToHSV(item, out hue, out saturation, out value);
                lst[index] = (hue, saturation, value);
                index += 1;
            }
            return lst;

        }

        public static void Enumerating(out (double, double, double)[] list_colors, out string[] list_tiles)
        {
            // vid 1:4:29:000000

            //(double, double, double)[] list_frompic = Tools.RemDuple(GetHueMass());
            
            Color color;
            double hue, saturation, value;
            list_tiles = fileREAD(Tools.Filepath_Dialog("tiles{1:4:29:000000}"));
            list_colors = new (double, double, double)[list_tiles.Length];
            // Create list of tiles colors.
            for (int i = 0; i < list_tiles.Length; i++)
            {
                Tools.ColorToHSV(ColorTranslator.FromHtml("#" + list_tiles[i].Split(':')[3]), out hue, out saturation, out value);
                list_colors[i] = (hue, saturation, value);
            }

            foreach (var item in list_tiles)
            {
                var line = item.Split(':');
                var new_line = line[0] + ":" + line[1] + ":" + line[2];
                list_tiles[Array.IndexOf(list_tiles, item)] = new_line;
            }

        }

        public static string main()
        {
            /*
             * Суть такова:
             * Всего массивов - 6.
             * Тайлы, Колорсы, ХСВ, ХСВ-бездуплы, итоговый тэкст, ФОТО-Террария(сорт)
             
            #region "Main six Arrays"
            string[] list_tiles; (double, double, double)[] list_colors; Enumerating(out list_colors, out list_tiles); // 1 and 2 mass 
            (double, double, double)[] Photo_notSort = GetHueMass(); // 3 mass
            (double, double, double)[] Photo_Sort = Tools.RemDuple(Photo_notSort); // 4 mass
            (double, double, double)[] Main_File = new (double, double, double)[Photo_Sort.Length]; foreach (var item in Photo_Sort){ Main_File[Array.IndexOf(Photo_Sort, item)] = Tools.nearest(list_colors, item); } // 5 mass
            string[] FileMassive = new string[Photo_notSort.Length]; // 6 mass
            int index = 0;
            #endregion

            // Мейн файл - эт перебраный Фото Сорт с цветами ТЕррарии

            foreach (var item in Photo_notSort)
            {
                var lp = list_tiles[Array.IndexOf(list_colors,Main_File[Array.IndexOf(Photo_Sort,item)])];
                FileMassive[index] = lp;
                index++;
            }
            File.AppendAllLines(Tools.Filepath_Dialog("TCT"), FileMassive);

            return "TEMP";
        }
    }
}

*/