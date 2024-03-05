using Aspose.Cells;
using PixelArt;
using PixelArt.Properties;
using PixelArt.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace PixelArt.Tools
{
    internal class PhotoTileConverter

    {
        public PhotoTileConverter(string[] _tiles, Color[] _colors, string _path)
        {
            Tiles = _tiles;
            Colors = _colors;
            Path = _path;
        }

        private static string[] _list_tiles;
        public static string[] Tiles
        {
            get { return _list_tiles ?? new string[0]; }
            set { _list_tiles = value; }
        }

        private static Color[] _list_colors;
        public static Color[] Colors
        {
            get { return _list_colors ?? new Color[0]; }
            set { _list_colors = value; }
        }

        private static string _photoPath;
        public static string Path
        {
            get { return _photoPath ?? String.Empty; }
            set { _photoPath = value; }
        }

        private static List<string> _tileResult;
        public static List<string> TileResult
        {
            get { return _tileResult ?? new List<string>(); }
            set { _tileResult = value; }
        }
        
        



        public static IEnumerable<Color>[] ReadPhoto(string filepath, out int x, out int y)
        {
            /*
             * Модуль возвращает массив,
             * содержащий цвета пикселя на каждой .
             * Цвета формируются такким образом:
             * Скрипт идет сверху в низ анализируя каждый пиксель,
             * затем идет на пиксель вправо и повторяет прошлый пункт.
             */
            Bitmap bitmap;
            try
            {
                bitmap = (Bitmap)Image.FromFile(filepath);
            }
            catch
            {

                bitmap = global::PixelArt_EXE.Properties.Resources.DefaultImage;
            }
            int hstart;
            Color pixel;
            x = bitmap.Width;
            y = bitmap.Height;
            //bitmap = Blur(bitmap, 2);
            List <Color> Array = new List<Color>(x*y);
            for (var i = 0; i < bitmap.Width; i++)
            {
                hstart = i * bitmap.Height;
                for (var j = 0; j < bitmap.Height; j++)
                {
                    pixel = bitmap.GetPixel(i, j);
                    Array.Add(pixel);
                }
            }
            return Enumerable.Range(0, (int)Math.Ceiling((double)x * y / 100000.0))
                                  .Select(i => Array.Skip(i * 100000).Take(100000)).ToArray();
        }
        public void Convert()
        {
            List<Color> SingleCopyColors = new List<Color>(100000);
            List<Color> AllColors = new List<Color>(100000);
            List<string> MainFile = TileResult;
            IEnumerable<Color>[] parts = ReadPhoto(Path, out int x, out int y);


            #region "Main six Arrays"
            ColorApproximater approximater = new ColorApproximater(Colors);
            BitmapBar bar = new BitmapBar(x * y);
            Data.WorkName = "Конвертирую Картинку";

            int BarValue = 0;
            foreach (var Chunk in parts)
            {
                AllColors.Clear();
                SingleCopyColors.Clear();
                foreach (var item in Chunk.Distinct().ToArray())
                {
                    BarValue++;
                    SingleCopyColors.Add(item.A < 20 ? Colors.First() : approximater.Convert(item) ?? Colors.First());
                    AllColors.Add(item);
                }
                foreach (var item in Chunk)
                {
                    BarValue++;
                    if (item.A < 20)
                    {
                        MainFile.Add("3:0:0:Air-Null");
                    }
                    else
                    {
                        MainFile.Add(Tiles[Array.IndexOf(Colors, SingleCopyColors[AllColors.IndexOf(item)])]);
                    }
                }
            }
            approximater.Reset();

            #endregion
        }
    }
}
