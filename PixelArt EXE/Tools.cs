﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PixelArt
{
    internal static class Tools
    {


        /// <summary>
        /// Инструменты/Функции, которые требуются для работы
        /// </summary>    
        #region Photo-Terraria
        public static void CreateTilesPhoto(object sender, DoWorkEventArgs e, int xstart = 0)
        {
            /*
             * Суть такова:
             * Всего массивов - 6.
             * Тайлы, Колорсы, ХСВ, ХСВ-бездуплы, итоговый тэкст, ФОТО-Террария(сорт)
             */
            #region "Main six Arrays"
            string[] list_tiles = Data.list_tiles; Color[] list_colors = Data.list_colors; // 1 and 2 mass 
            Color[] Photo_notSort = Tools.ReadPhoto(Data.photo_path, out int x, out int y); ; // 3 mass
            int PS_Lenght = Photo_notSort.Length;
            Data.WorkName = "Конвертирую Картинку";
            var nChunks = (int)Math.Ceiling((double)Photo_notSort.Length / 100000.0);
            List<string> MainFile = new List<string>();
            var totalLength = Photo_notSort.Length;
            var chunkLength = (int)Math.Ceiling(totalLength / (double)nChunks);
            var parts = Enumerable.Range(0, nChunks)
                                  .Select(i => Photo_notSort.Skip(i * chunkLength).Take(chunkLength)).ToArray();
            //string[] list = new string[] { "1", xstart.ToString() + ":" + x.ToString(), y.ToString() };
            Data.x = x.ToString();
            Data.y = y.ToString();
            Data.xstart = xstart.ToString();
            //File.WriteAllLines(Data.save_path + "photo.txt", list);
            int MainIndex = 0;

            foreach (var Chunk2 in parts)
            {
                (sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(MainIndex, parts.Length));
                MainIndex++;
                int index = 0;

                Color[] Chunk = Chunk2.ToArray();
                Color[] Photo_Sort = Tools.RemDuple(Chunk); // 4 mass
                Color[] Main_File = new Color[Photo_Sort.Length];
                //(sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
                var maxindex = Photo_Sort.Length;
                foreach (var item in Photo_Sort)
                {
                    (sender as BackgroundWorker).ReportProgress(Tools.EqProgressBarInc(index, maxindex, parts.Length, 2));
                    // 3 arg = max lenght
                    //(sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(index, Photo_Sort.Length));

                    Main_File[index] = Tools.nearest(list_colors, item);
                    index++;
                } // 5 mass
                Data.Percent = Data.Percent2;
                //(sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
                string[] FileMassive = new string[Chunk.Length]; // 6 mass
                index = 0;
                maxindex = Chunk.Length;
                foreach (var item in Chunk)
                {
                    (sender as BackgroundWorker).ReportProgress(Tools.EqProgressBarInc(index, maxindex, parts.Length, 2));
                    //(sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(MainIndex, parts.Length));
                    //var lp = list_tiles[Array.IndexOf(list_colors, Main_File[index])];
                    var lp = list_tiles[Array.IndexOf(list_colors, Main_File[Array.IndexOf(Photo_Sort, item)])];
                    //var lp = list_tiles[Array.IndexOf(list_colors, Main_File[i])];
                    FileMassive[index] = lp;
                    index++;
                }
                MainFile = MainFile.Concat(FileMassive).ToList();
                //File.AppendAllLines(@"C:\ARTs\sda\aasd.txt", FileMassive);

            }
            Data.File_Photo_list = MainFile.ToArray();
            (sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());


            #endregion

            Data.WorkName = null;
            //Data.extile_path = @"C:\ARTs\sda\aasd.txt";
            // Мейн файл - эт перебраный Фото Сорт с цветами ТЕррарии

            //return "TEMP";
        }
        public static void Enumerating(out Color[] list_colors, out string[] list_tiles)
        {
            // vid 1:4:29:000000

            //(double, double, double)[] list_frompic = Tools.RemDuple(GetHueMass());
            string filepath = Data.tiles_path;
            Color color;
            double hue, saturation, value;
            list_tiles = fileREAD(filepath);
            list_colors = new Color[list_tiles.Length];
            // Create list of tiles colors.
            for (int i = 0; i < list_tiles.Length; i++)
            {
                color = ColorTranslator.FromHtml("#" + list_tiles[i].Split(':')[3]);
                list_colors[i] = color;
            }

            foreach (var item in list_tiles)
            {
                var line = item.Split(':');
                var new_line = line[0] + ":" + line[1] + ":" + line[2];
                list_tiles[Array.IndexOf(list_tiles, item)] = new_line;
            }

        }
        #endregion
        
        #region Bitmaps
        public static Bitmap CreatePhoto(Color[] list_colors, string[] list_tiles, object sender, DoWorkEventArgs e)
        {
            //CreateDirectory(Data.output_path + Data.art_name, 2);
            int x, y, hstart, appreform;
            Bitmap bitmap;
            string[] array;
            Data.WorkName = null;
            if (Data.File_Photo_list == null)
            {
                array = File.ReadAllLines(Data.extile_path);
                x = Convert.ToInt32(array[1].Split(':')[1]) - Convert.ToInt32(array[1].Split(':')[0]);
                y = Convert.ToInt32(array[2]);
                appreform = 3;
            }
            else
            {
                array = Data.File_Photo_list;
                x = Convert.ToInt32(Data.x) - Convert.ToInt32(Data.xstart);
                y = Convert.ToInt32(Data.y);
                appreform = 0;
            }


            (sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
            bitmap = new Bitmap(x, y);
            Data.WorkName = "Генерирую Картинку";

            for (var i = 0; i < x; i++)
            {
                //mc.BrogB_Increase(i, bitmap.Width);
                (sender as BackgroundWorker).ReportProgress(BrogB_Increase(i, bitmap.Width));
                //mc.BrogB_Increase2(10);
                hstart = appreform + i * y;
                //System.Threading.Thread.Sleep(100);
                for (var j = 0; j < y; j++)
                {
                    bitmap.SetPixel(i, j, list_colors[Array.IndexOf(list_tiles, array[j + hstart])]);
                    //var pixel = bitmap.GetPixel(i, j);
                }
            }
            Data.WorkName = null;

            bitmap.Save(Data.save_path + "modificed_" + Data.art_name + ".jpg");
            if (Data.extile_path != "" && Data.extile_path != null) { File.Copy(Data.extile_path, Data.save_path + "photo.txt"); } else { File.WriteAllLines(Data.save_path + "photo.txt", new string[] { "1", Data.xstart + ":" + Data.x, Data.y }); File.AppendAllLines(Data.save_path + "photo.txt", Data.File_Photo_list); }
            if (Data.photo_path != "" && Data.photo_path != null) { File.Copy(Data.photo_path, Data.save_path + "original_" + Data.art_name + ".jpg"); };
            File.Copy(Data.tiles_path, Data.save_path + "used tiles.txt");
            return bitmap;
        }
        public static Color[] ReadPhoto(string filepath,out int x, out int y)
        {
            /*
             * Модуль возвращает массив,
             * содержащий цвета пикселя на каждой .
             * Цвета формируются такким образом:
             * Скрипт идет сверху в низ анализируя каждый пиксель,
             * затем идет на пиксель вправо и повторяет прошлый пункт.
             */
            var bitmap = (Bitmap)Image.FromFile(filepath);
            x = bitmap.Width;
            y = bitmap.Height;
            //bitmap = Blur(bitmap, 2);
            var array = new Color[bitmap.Height * bitmap.Width];
            for (var i = 0; i < bitmap.Width; i++)
            {
                int hstart = i * bitmap.Height;
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    array[j + hstart] = (pixel);
                }
            }
            return array;
        }
        #endregion
        
        #region Color Manipulation
        public static int[] ColorsCount(Color[] UniqueColors, Color[] AllColors) //Архив с колвом встреч
        {
            int[] list = new int[UniqueColors.Length];
            for (int i = 0; i < UniqueColors.Length; i++)
            {
                list[i] = AllColors.Count(a => a == UniqueColors[i]);
            }
            return list;
        }

        private static int ColorInputs(Color[] Photo_notSort)
        {
            // 3 mass
            Color[] Photo_Sort = Tools.RemDuple(Photo_notSort);
            //label1.Text = Tools.CountInput_color(Photo_notSort, Photo_Sort[2]).ToString();
            int[] list = Tools.ColorsCount(Photo_Sort, Photo_notSort);
            return list.Length;
        }
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }
        private static (double, double, double) SortFunc((double, double, double) hsv, (double, double, double) hsv2)
        {
            // First color
            double h, s, v;
            h = hsv.Item1;
            s = hsv.Item2;
            v = hsv.Item3;
            // Second color
            double h2, s2, v2;
            h2 = hsv2.Item1;
            s2 = hsv2.Item2;
            v2 = hsv2.Item3;
            // MInus btw
            h -= h2;
            s -= s2;
            v -= v2;

            return (Math.Abs(h), Math.Abs(s), Math.Abs(v));

        }
        public static float getBrightness(Color c)
        { return (c.R * 0.299f + c.G * 0.587f + c.B * 0.114f) / 256f; }

        public static float ColorNum(Color c)
        {
            return c.GetSaturation() + getBrightness(c);
            //return getBrightness(c);
        }
        public static float getHueDistance(float hue1, float hue2)
        {
            float d = Math.Abs(hue1 - hue2); return d > 180 ? 360 - d : d;
        }
        public static int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                 + (c1.G - c2.G) * (c1.G - c2.G)
                                 + (c1.B - c2.B) * (c1.B - c2.B));
        }
        public static Color closestColor1(List<Color> colors, Color target)
        {
            var hue1 = target.GetHue();
            var diffs = colors.Select(n => getHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(n => n);
            return colors[diffs.ToList().FindIndex(n => n == diffMin)];
        }
        public static Color closestColor2(List<Color> colors, Color target)
        {
            var colorDiffs = colors.Select(n => ColorDiff(n, target)).Min(n => n);
            return colors[colors.FindIndex(n => ColorDiff(n, target) == colorDiffs)];
        }
        public static Color closestColor3(List<Color> colors, Color target)
        {
            float hue1 = target.GetHue();
            var num1 = ColorNum(target);
            var diffs = colors.Select(n => Math.Abs(ColorNum(n) - num1) +
                                           getHueDistance(n.GetHue(), hue1));
            var diffMin = diffs.Min(x => x);
            return colors[diffs.ToList().FindIndex(n => n == diffMin)];
        }

        public static Color nearest(Color[] colors, Color target)
        {
            List<Color> colors2 = colors.ToList();
            return closestColor2(colors2, target);
        }
        public static Color[] RemDuple(Color[] lst)
        {
            lst = lst.Distinct().ToArray();
            return lst;
        }
        #endregion

        #region Constant Functions
        public static int CountInput_string(string[] array, string a)
        {
            return array.Count(a.Equals);
        }
        private static bool Color_Equals(Color a, Color b)
        {
            if (a != b)
            {
                return false;
            }
            return true;
        }
        public static int CountInput_color(Color[] array, Color a)
        {
            return array.Count(i => Color_Equals(i, a));
        }
        public static bool ContainsAnyOf(this string source, params string[] strings)
        {
            return strings.Any(x => source.Contains(x));
        }
        public static int BrogB_Increase(int current_value, int max_value)
        {
            double value = current_value * 100 / max_value;
            int percent = (int)Math.Round(value);
            Data.Now_Stage = current_value;
            Data.Then_Stage = max_value;
            Data.Percent = percent;
            return percent;
        }
        public static int SetZeroPercentage()
        {
            //double value = current_value * 100 / max_value;
            //int percent = (int)Math.Round(value);
            Data.Now_Stage = 0;
            Data.Then_Stage = 0;
            Data.Percent = 0;
            return 0;
        }
        public static string[] fileREAD(string filepath)
        {
            string[] arr = File.ReadAllLines(filepath);
            return arr;
        }
        private static int HeightWidth(int x, int y, int xstart)
        {
            x = x - xstart;
            return x * xstart;
        }
        public static void CreateDirectory(string path, int method)
        {
            if (!Directory.Exists(path) || method == 2)
            {
                Directory.CreateDirectory(path);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }
        public static int EqProgressBarInc(int current, int max, int maxvalue, int Chunks = 2, int param = 0)
        {
            int percent;
            // Используется в цикле форыча, без деления на части
            percent = Data.Percent;
            if (Chunks == 0 || Chunks < 0) { Chunks = 1; }
            percent += (int)Math.Floor((double)100 * current / max / maxvalue / Chunks);
            Data.Percent2 = percent;
            return percent;
        }

        public static string Filepath_Dialog(string message)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"";
                openFileDialog.Filter = $"txt files (*.txt)|*.txt|{message} (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                return "";
            }
        }

        public static string FolderPath_Dialog()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Custom Description";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    return fbd.SelectedPath;
                }
                return "";
            }
        }
        private static Bitmap Blur(Bitmap image, Int32 blurSize)
        {
            return Blur(image, new Rectangle(0, 0, image.Width, image.Height), blurSize);
        }
        private static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (int x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            Color pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (int x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (int y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return blurred;
        }
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        #endregion



    }
}
