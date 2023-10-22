using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aspose.Cells;
using System.IO.Ports;
using System.Threading;

namespace PixelArt
{
    /// <summary>
    /// Инструменты/Функции, которые требуются для работы
    /// </summary>  
    internal static class Tools
    {
        private static int pixelcount = 87;
        private static string HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
        public static List<Color> CellColors = new List<Color>();
        //public static char[] CellChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static Color ReverseCol(Color color)
        {
            ColorConverter colorConverter = new ColorConverter();
            ColorToHSV(color, out double hue, out double saturation, out double value);
            hue = (hue + 180) % 360;
            //saturation = (saturation > 0.9) ? Math.Abs(saturation - 0.5) : Math.Abs(saturation - 0.25);
            //value = (value > 0.5) ? Math.Abs(value - 0.5) : Math.Abs(value + 0.5);
            value = (value > 0.5) ? 0 : 1;
            return ColorFromHSV(hue, 1, value);
        }
        /// <summary>
        /// Создание экселевской книги. 
        /// Книга создана таким образом, что каждая ячейка представляет собой покрашеную клетку с точным цветом и названием тайла. 
        /// </summary> 
        /// <param name="array">Главый файл, который обычно записывается в photo.txt</param>
        public static void Create_XLSX(object sender)
        {
            int Xend = Convert.ToInt32(Data.x);
            int Yend = Convert.ToInt32(Data.y);
            string[] Cells = Data.File_Photo_list.ToArray();
            Data.WorkName = "Создаю таблицу";
            Color[] Colors = CellColors.ToArray();
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];
            int index = 0;
            Cell cell;
            Style style;
            Color color;
            string[] list_tiles = Data.list_tiles;
            string[] blocks = Data.list_blocks;
            string item;
            string[] items;
            string[] Countion = new string[Cells.Length];
            for (int x = 0; x < Xend; x++)
            {
                for (int y = 0; y < Yend; y++)
                {
                    
                    if (Data.DoWork == false) { return; }
                    int indent = y * Xend + x;
                    color = Colors[index];
                    //colorstr = HexConverter(color);
                    wb.ChangePalette(color, 55);
                    (sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(index, Cells.Length));
                    cell = sheet.Cells[column: x, row: y];
                    //if (color.GetBrightness() < 0.2) { style.Font.Color = color.}
                    #region SetStyle
                    style = cell.GetStyle();
                    style.ForegroundColor = wb.Colors[55];
                    style.BackgroundColor = wb.Colors[55];
                    style.Pattern = BackgroundType.Solid;
                    style.Font.Color = ReverseCol(color);
                    style.Font.Size = 7;
                    style.IsTextWrapped = true;
                    style.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, color == Color.Black? Color.Gray : Color.Black);
                    style.SetBorder(BorderType.RightBorder, CellBorderType.Thin, color == Color.Black ? Color.Gray : Color.Black);
                    style.SetBorder(BorderType.TopBorder, CellBorderType.Thin, color == Color.Black ? Color.Gray : Color.Black);
                    style.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, color == Color.Black ? Color.Gray : Color.Black);
                    #endregion
                    cell.SetStyle(style);
                    item = blocks[Array.IndexOf(list_tiles, Cells[index])];
                    Countion[index] = item;
                    items = item.Split(':');
                    //item = Cells[index].Split()[]
                    cell.PutValue($"{indent}\n{Cells[index]}\n{index}\n{items[0]}\n{items[1]}\n{items[2]}");
                    //cell.HtmlString = "<Font Style=\"FONT-FAMILY: Arial;FONT-SIZE: 10pt;COLOR: " + colorstr + ";\"></Font>";
                    index++;
                }

            }
            // how many insect(хз, вхождений тип)
            int count;
            
            string[] UniqueCountion = Countion.Distinct().ToArray();
            string[] counts = new string[UniqueCountion.Length];
            int[] int_counts = new int[UniqueCountion.Length];
            for (int i = 0; i < UniqueCountion.Length; i++)
            {
                count = Countion.Count(a => a == UniqueCountion[i]);
                int_counts[i] = count;
                counts[i] = $"{UniqueCountion[i]}  -  {count}";
            }
            Array.Sort(int_counts,counts);
            File.WriteAllLines(Data.save_path + Data.art_name + "-blocks.txt",counts);




            Cells cells = sheet.Cells;
            for (int x = 0; x < Xend; x++)
            {
                cells.SetColumnWidthPixel(x, pixelcount);
                for (int y = 0; y < Yend; y++)
                {
                    cells.SetRowHeightPixel(y, pixelcount);

                }
            }
            //if (!Directory.Exists(Data.save_path))
            wb.Save(Data.save_path + Data.art_name + ".xlsx", SaveFormat.Xlsx);
        }
        #region Photo-Pixels


        /// <summary>
        /// Главная функция всей программы. 
        /// Конвертирует изображение из одной палитры в другую.
        /// Конвертация может длится больше 10 часов.
        /// Затем генерирует это изображение в picturebox-объекте, для удобства просмотра.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="xstart">Начальная координата по Х для создания арта.</param>
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
            if (Data.DoWork == false) { return; }
            //string[] list = new string[] { "1", xstart.ToString() + ":" + x.ToString(), y.ToString() };
            Data.x = x.ToString();
            Data.y = y.ToString();
            Data.xstart = xstart.ToString();
            //File.WriteAllLines(Data.save_path + "photo.txt", list);
            int MainIndex = 0;
            Color CellColor;
            Color[] ThenColors = new Color[0];
            foreach (var Chunk2 in parts)
            {
                if (Data.DoWork == false) { return; }
                (sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(MainIndex, parts.Length));
                MainIndex++;
                int index = 0;
                if (Data.DoWork == false) { return; }
                Color[] Chunk = Chunk2.ToArray();
                Color[] Photo_Sort = Tools.RemDuple(Chunk); // 4 mass
                Color[] Main_File = new Color[Photo_Sort.Length];
                //(sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
                var maxindex = Photo_Sort.Length;
                foreach (var item in Photo_Sort)
                {
                    if (Data.DoWork == false) { return; }
                    (sender as BackgroundWorker).ReportProgress(Tools.EqProgressBarInc(index, maxindex, parts.Length, 2));
                    // 3 arg = max lenght
                    //(sender as BackgroundWorker).ReportProgress(Tools.BrogB_Increase(index, Photo_Sort.Length));

                    Main_File[index] = (item.A < 20) ? Color.FromArgb(0,0,0,0) : Tools.nearest(list_colors, item);
                    index++;
                } // 5 mass
                Data.Percent = Data.Percent2;
                //(sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
                string[] FileMassive = new string[Chunk.Length]; // 6 mass
                index = 0;
                maxindex = Chunk.Length;
                foreach (var item in Chunk)
                {
                    if (Data.DoWork == false) { return; }
                    string lp;
                    (sender as BackgroundWorker).ReportProgress(Tools.EqProgressBarInc(index, maxindex, parts.Length, 2));
                    if (item.A < 20)
                    {
                        lp = "3:0:0:Air-Null";
                        CellColors.Add(Color.White);
                    }
                    else
                    {
                        
                        CellColor = Main_File[Array.IndexOf(Photo_Sort, item)];
                        lp = list_tiles[Array.IndexOf(list_colors, CellColor)];
                        CellColors.Add(CellColor);
                    }
                    FileMassive[index] = lp;
                    index++;
                }
                ThenColors = ThenColors.Concat(Main_File).ToArray();
                MainFile = MainFile.Concat(FileMassive).ToList();
                //File.AppendAllLines(@"C:\ARTs\sda\aasd.txt", FileMassive);

            }
            #endregion
            
            Data.ThenColors = Tools.RemDuple(ThenColors);
            Data.File_Photo_list = MainFile.ToArray();
            (sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());




            Data.WorkName = null;
            //Data.extile_path = @"C:\ARTs\sda\aasd.txt";
            // Мейн файл - эт перебраный Фото Сорт с цветами ТЕррарии

            //return "TEMP";
        }
        /// <summary>
        /// Создаёт два массива, которые выходят из него. 
        /// </summary>
        /// <param name="list_colors">System.Drawing.Color лист</param>
        /// <param name="list_tiles">System.String лист</param>
        public static void Enumerating(out Color[] list_colors, out string[] list_tiles)
        {
            
            // vid 1:4:29:000000
            //(double, double, double)[] list_frompic = Tools.RemDuple(GetHueMass());
            string filepath = Data.tiles_path;
            Color color;
            //double hue, saturation, value;
            list_tiles = fileREAD(filepath);
            list_colors = new Color[list_tiles.Length];
            string[] list_blocks = new string[list_tiles.Length];
            // Create list of tiles colors.
            for (int i = 0; i < list_tiles.Length; i++)
            {
                color = ColorTranslator.FromHtml("#" + list_tiles[i].Split(':')[3]);
                list_colors[i] = color;
            }

            foreach (var item in list_tiles)
            {
                int index = Array.IndexOf(list_tiles, item);
                var line = item.Split(':');
                var new_line = line[0] + ":" + line[1] + ":" + line[2];
                var new_line2 = line[4] + ":" + line[5] + ":" + line[6];
                list_tiles[index] = new_line;
                list_blocks[index] = new_line2;
            }
            //list_tiles = list_tiles.ToList().Append("3:0:0").ToArray();
            //list_colors = list_colors.ToList().Append(Color.FromArgb(0,0,0,0)).ToArray();
            Data.list_blocks = list_blocks;
        }
        #endregion
        
        #region Bitmaps
        /// <summary>
        /// Генерирует изображение и возвращает его же
        /// </summary>
        /// <param name="list_colors">Лист с цветами в шестнадцатеричном формате, в виде строки</param>
        /// <param name="list_tiles">Тайлы, в формате {index,pix-id,paint}, где index = 1(или 2 или 3), pix-id = id тайла или стены, paint = id покраски.</param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>System.Drawing.Bitmap объект</returns>
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
            Data.x = (Data.x == "" || Data.x == null) ? x.ToString() : Data.x;
            Data.y = (Data.y == "" || Data.y == null) ? y.ToString() : Data.y;
            Data.File_Photo_list = array;
            (sender as BackgroundWorker).ReportProgress(Tools.SetZeroPercentage());
            bitmap = new Bitmap(x, y);
            Data.WorkName = "Генерирую Картинку";
            Color color;
            List<Color> Colors = new List<Color>();
            for (var i = 0; i < x; i++)
            {
                //mc.BrogB_Increase(i, bitmap.Width);
                (sender as BackgroundWorker).ReportProgress(BrogB_Increase(i, bitmap.Width));
                //mc.BrogB_Increase2(10);
                hstart = appreform + i * y;
                //System.Threading.Thread.Sleep(100);
                if (Data.DoWork == false) { goto endWork; }
                for (var j = 0; j < y; j++)
                {
                    string countion = array[j + hstart];
                    color = list_colors[Array.IndexOf(list_tiles, countion)];
                    if (countion == "3:0:0")
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(0,0,0,0));
                        CellColors.Add(Color.White);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, color);
                        CellColors.Add(color);
                    }
                    Colors.Add(color);
                    //var pixel = bitmap.GetPixel(i, j);
                }
            }
            Data.AllColorsThen = Colors.ToArray();
            Data.WorkName = null;
            endWork:
            SaveAll();
            if (Data.DoWork == true)
            {
                bitmap.Save(Data.save_path + "modificed_" + Data.art_name + ".jpg");
                return bitmap;
            }
            return new Bitmap(1, 1);
        }
        private static void SaveAll()
        {
            if (Data.extile_path != "" && Data.extile_path != null) { File.Copy(Data.extile_path, Data.save_path + "photo.txt"); } else { File.WriteAllLines(Data.save_path + "photo.txt", new string[] { "1", Data.xstart + ":" + Data.x, Data.y }); File.AppendAllLines(Data.save_path + "photo.txt", Data.File_Photo_list); }
            if (Data.photo_path != "" && Data.photo_path != null) { File.Copy(Data.photo_path, Data.save_path + "original_" + Data.art_name + ".jpg"); };
            File.Copy(Data.tiles_path, Data.save_path + "used tiles.txt");
        }
        /// <summary>
        /// Модуль возвращает массив,
        ///     содержащий цвета пикселя на каждой.
        ///     Цвета формируются такким образом:
        ///     Скрипт идет сверху в низ анализируя каждый пиксель,
        ///     затем идет на пиксель вправо и повторяет прошлый пункт.
        /// </summary>
        /// <param name="filepath">Путь до фотографии</param>
        /// <param name="x">Длина фото</param>
        /// <param name="y">Высота фото</param>
        /// <returns></returns>
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
        /// <summary>
        /// Возвращает лист с количеством встреч каждого цвета
        /// </summary>
        /// <param name="UniqueColors"> отсортированые цвета </param>
        /// <param name="AllColors"> Неотсортированые цвета </param>
        /// <returns></returns>
        public static int[] ColorsCount(Color[] UniqueColors, Color[] AllColors) //Архив с колвом встреч
        {
            int[] list = new int[UniqueColors.Length];
            for (int i = 0; i < UniqueColors.Length; i++)
            {
                list[i] = AllColors.Count(a => a == UniqueColors[i]);
            }
            return list;
        }

        private static string[] ColorInputs()
        {
            // 3 mass
            string[] TilesList = Data.list_tiles;
            Color[] ColorsList = Data.list_colors;
            Color[] SortColors = Data.ThenColors;
            Color[] NotSortColors = Data.AllColorsThen;  
            // Индексы фото-сорта и Файл-листа равны всегда.
            int[] list = Tools.ColorsCount(SortColors, NotSortColors);
            string[] inXls = new string[SortColors.Length];
            Color color;
            for (int i = 0; i < SortColors.Length;i++)
            {
                color = SortColors[i];
                inXls[i] = $"{TilesList[Array.IndexOf(ColorsList, color)].Split(':')[3]}:{list[i]}";
            }
            return inXls;
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
        /// <summary>
        /// Сортирует лист с цветами с помощью метода Distinct
        /// </summary>
        /// <param name="lst">Лист с цветами</param>
        /// <returns></returns>
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
                Thread.Sleep(500);
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
