
using ConsoleApp2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Dithering;
using static System.Reflection.Metadata.BlobBuilder;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Введите путь к фото:");
            PhotoTileConverter converter = new PhotoTileConverter(@"C:\Users\USER\Documents\PixelArtCreatorByMixailka\test.jpg");
            PhotoTileConverter.ParseTileData();
            converter.Convert();
            
            //BinaryWorker bw = new BinaryWorker();
            //bw.Read();
        }
    }
    public class BinaryWorker
    {
        /// <summary>
        /// Путь к файлу результата
        /// </summary>
        private string _path; // Приватное поле для хранения пути к файлу

        internal string Path
        {
            get { return _path; } // Возвращает значение из приватного поля
            set { _path = value; } // Устанавливает значение в приватное поле
        }

        // Конструктор класса, принимает путь к файлу, по умолчанию установлен путь к TET.txt
        public BinaryWorker(string path = @"C:\T\TET.txt")
        {
            Path = path; // Инициализация пути
        }

        // Список для хранения значений из файла
        public List<(bool, bool, ushort, byte)> FileValues = new List<(bool, bool, ushort, byte)>();

        // Метод для чтения данных из файла
        internal List<(bool, bool, ushort, byte)> Read()
        {
            List<(bool, bool, ushort, byte)> Array = new List<(bool, bool, ushort, byte)>(); // Создаем новый список для хранения прочитанных данных
            byte[] bytes = File.ReadAllBytes(Path); // Читаем все байты из файла

            // Извлечение ширины и высоты из первых байтов
            ushort WidthStart = (ushort)((bytes[0] & 0xff) + ((bytes[1] & 0xff) << 8));
            ushort Width = (ushort)((bytes[2] & 0xff) + ((bytes[3] & 0xff) << 8));
            ushort Height = (ushort)((bytes[4] & 0xff) + ((bytes[5] & 0xff) << 8));

            // Чтение данных из файла и добавление их в список
            for (int i = 6; i < bytes.Length && i + 4 < bytes.Length; i += 5)
            {
                Array.Add((
                    Convert.ToBoolean(bytes[i]), // Признак стены
                    Convert.ToBoolean(bytes[i + 1]), // Признак факела
                    (ushort)((bytes[i + 2] & 0xff) + ((bytes[i + 3] & 0xff) << 8)), // ID
                    bytes[i + 4] // Цвет
                ));
            }
            return Array; // Возвращаем список прочитанных значений
        }

        /// <summary>
        /// Конвертирует число в формате ushort в строку заданной длины (до 8)
        /// </summary>
        /// <param name="value">Число для конвертации</param>
        /// <param name="length">Длина результирующей строки</param>
        /// <returns>Строка в двоичном формате</returns>
        private static string ConvertToBinary(ushort value, byte length)
        {
            string tmp = String.Empty; // Временная строка для хранения нулей
            string result = Convert.ToString(value, 2); // Конвертируем число в двоичную строку

            // Добавляем нули в начало строки до нужной длины
            for (int i = 0; i < (length - result.Length); i++)
            {
                tmp += "0"; // Добавление нуля
            }
            return tmp + result; // Возвращаем строку с нулями и результатом
        }

        // Метод для записи данных в файл
        internal void Write(ushort Width, ushort Height, ushort WidthStart = 0, List<(bool, bool, ushort, byte)> Array = null)
        {
            Array = Array ?? FileValues; // Если массив не передан, используем значения по умолчанию
            using (var stream = File.Open(Path, FileMode.Create)) // Открываем файл для записи
            {
                using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false)) // Создаем бинарный писатель
                {
                    // Записываем ширину, высоту и значения в файл
                    binaryWriter.Write(WidthStart); // Начальная ширина
                    binaryWriter.Write(Width); // Ширина
                    binaryWriter.Write(Height); // Высота
                    for (int index = 0; index < Array.Count; index++)
                    {
                        binaryWriter.Write(Array[index].Item1); // Стена?
                        binaryWriter.Write(Array[index].Item2); // Факел?
                        binaryWriter.Write(Array[index].Item3); // ID
                        binaryWriter.Write(Array[index].Item4); // Цвет
                    }
                }
            }
        }
    }

    public class Pixels
    {
        // Статический список для хранения объектов Pixel
        public static List<Pixel> Objects = new List<Pixel>();

        // Конструктор класса, принимает массив строк, представляющих пути
        public Pixels(string[] Path)
        {
            foreach (string tileLine in Path)
            {
                // Разделяем строку на части, используя двоеточие (:) в качестве разделителя
                string[] parts = tileLine.Split(':');
                // Создаем объект Pixel и добавляем его в список
                Add(new Pixel(parts, parts[0] == "0" ? true : false));
            }

            /*
            // Закомментированный код для загрузки данных из XML файла
            if (Path is string)
            {
                XElement file = XDocument.Load(Path.ToString()).Element("Settings");
                List<Pixel> pixels = new List<Pixel>();
                if (file.Element("Tiles") == null || file.Element("Walls") == null)
                {
                    throw new Exception("XML файл поврежден");
                }
                foreach (XElement item in file.Element("Tiles").Elements("Tile"))
                {
                    Add(new Pixel(item, false));
                }
                foreach (XElement item in file.Element("Walls").Elements("Wall"))
                {
                    Add(new Pixel(item, true));
                }
            }
            else if (Path is string[])
            {
                foreach (var tileLine in Path)
                {
                    // Разделяем строку на части, используя двоеточие (:) в качестве разделителя
                    string[] parts = tileLine.Split(':');
                    // Создаем объект Pixel и добавляем его в список
                    Add(new Pixel(parts, parts[0] == "0" ? true : false));
                }
            }
            */

            /*
            // Закомментированный код для задания пути к XML файлу
            string path = @"C:\Users\Сисьадмин\Documents\PixelArtCreatorByMixailka\settings.xml";
            */
        }

        // Метод для добавления объекта Pixel в список
        public void Add(Pixel pixel)
        {
            Objects.Add(pixel); // Добавляем пиксель в список
        }

        // Метод для удаления объекта Pixel из списка
        public void Del(Pixel pixel)
        {
            Objects.Remove(pixel); // Удаляем пиксель из списка
        }

        // Метод для получения списка пикселей в виде кортежей
        public List<(bool, bool, ushort, byte)> GetPixels()
        {
            return Objects.Select(x => (x.Wall, x.WallAtached, x.id, x.paint)).ToList();
        }

        // Метод для получения списка цветов пикселей
        public List<(byte, byte, byte)> GetColors()
        {
            return Objects.Select(x => x.color).ToList();
        }
    }

    public class Pixel
    {
        public string Name; // Имя пикселя
        public bool Wall = false; // Признак стены
        public ushort id; // ID пикселя
        public byte paint; // ID краски
        public (byte, byte, byte) color; // Цвет пикселя
        public bool WallAtached = false; // Признак прикрепленного факела

        // Конструктор для создания пикселя из XML элемента
        public Pixel(XElement element, bool wall = false)
        {
            Name = element.Attribute("name").Value; // Получаем имя из атрибута
            Wall = wall; // Устанавливаем признак стены
            id = Convert.ToUInt16(element.Attribute("num").Value); // Получаем ID
            paint = Convert.ToByte(element.Attribute("paintID").Value); // Получаем ID краски
            color = ToBytes(element.Attribute("color").Value); // Получаем цвет
            WallAtached = element.Attribute("Torch").Value == "true" ? true : false; // Устанавливаем признак прикрепленного факела
        }

        // Конструктор для создания пикселя из массива строк
        public Pixel(string[] parts, bool wall = false)
        {
            Name = string.Concat(parts[5], " ", parts[6]); // Формируем имя
            Wall = wall; // Устанавливаем признак стены
            id = Convert.ToUInt16(parts[1]); // Получаем ID
            paint = Convert.ToByte(parts[2]); // Получаем ID краски
            color = ToBytes(parts[3]); // Получаем цвет
            WallAtached = DefTorchs.IndexOf(parts[1]) != -1 ? true : false; // Устанавливаем признак прикрепленного факела
        }

        // Метод для конвертации шестнадцатеричного значения в байты
        private static (byte, byte, byte) ToBytes(string hexValue)
        {
            int hexColor = Convert.ToInt32(hexValue.Replace("#", ""), 16); // Преобразуем шестнадцатеричное значение в целое число
            return ((byte)((hexColor >> 16) & 0xff), // Извлекаем красный компонент
                    (byte)((hexColor >> 8) & 0xff),  // Извлекаем зеленый компонент
                    (byte)(hexColor & 0xff));        // Извлекаем синий компонент
        }

        // Статический список для хранения значений по умолчанию для факелов
        public static List<string> DefTorchs { get; private set; } = new List<string>
        {
            "4", "136", "557", "429", "424", "423", "420"
        };

        // Статический список для хранения значений по умолчанию для исключений стен
        public static List<string> DefExceptionsWalls { get; private set; } = new List<string>
        {
            "168", "169"
        };
        public static List<string> DefExceptionsTiles { get; private set; } = new List<string>
        {
            "3", "5", "10", "11", "12", "13", "14", "15", "16", "17", "18", "20",
            "21", "24", "26", "27", "28", "29", "31", "33", "34", "35", "36",
            "42", "49", "50", "55", "61", "71", "72", "73", "74", "77", "78",
            "79", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90",
            "91", "92", "93", "94", "95", "96", "97", "98", "99", "100", "191",
            "102", "103", "104", "105", "106", "110", "113", "114", "125",
            "126", "128", "129", "132", "133", "134", "135", "137", "138",
            "139", "141", "142", "143", "144", "149", "165", "171", "172",
            "173", "174", "178", "184", "185", "186", "187", "201", "207",
            "209", "210", "212", "215", "216", "217", "218", "219", "220",
            "227", "228", "231", "233", "235", "236", "237", "238", "239",
            "240", "241", "242", "243", "244", "245", "246", "247", "254",
            "269", "270", "271", "275", "276", "277", "278", "279", "280",
            "281", "282", "283", "285", "286", "287", "288", "289", "290",
            "291", "292", "293", "294", "295", "296", "297", "298", "299",
            "300", "301", "302", "303", "304", "305", "306", "307", "308",
            "309", "310", "314", "316", "317", "318", "319", "320", "323",
            "324", "334", "335", "337", "338", "339", "349", "354", "355",
            "356", "358", "359", "360", "361", "362", "363", "364", "372",
            "373", "374", "375", "376", "377", "378", "380", "386", "387",
            "388", "389", "390", "391", "392", "393", "394", "395", "405",
            "406", "410", "411", "412", "413", "414", "419", "425", "527",
            "428", "440", "441", "442", "443", "444", "452", "453", "454",
            "455", "456", "457", "461", "462", "463", "464", "465", "466",
            "467", "468", "469", "470", "471", "475", "476", "480", "484",
            "485", "486", "487", "488", "489", "490", "491", "493", "494",
            "497", "499", "505", "506", "509", "510", "511", "518", "519",
            "520", "521", "522", "523", "524", "525", "526", "527", "529",
            "530", "531", "532", "533", "538", "542", "543", "544", "545",
            "547", "548", "549", "550", "551", "552", "553", "554", "555",
            "556", "558", "559", "560", "564", "565", "567", "568", "569",
            "570", "571", "572", "573", "579", "580", "581",
            "582", "583", "584", "585", "586", "587", "588", "589", "590",
            "591", "592", "593", "594", "595", "596", "597", "598", "599",
            "600", "601", "602", "603", "604", "605", "606", "607", "608",
            "609", "610", "611", "612", "613", "614", "615", "616", "617",
            "619", "620", "621", "622", "623", "624", "629", "630", "631",
            "632", "634", "637", "639", "640", "642", "643", "644", "645",
            "646", "647", "648", "649", "650", "651", "652", "653", "654",
            "656", "657", "658", "660", "663", "664", "665", "127", "52",
            "53", "112", "116", "234", "224", "123", "330", "331", "332",
            "333", "51", "52", "62", "115", "205", "382", "528", "636",
            "638", "32", "69", "352", "655", "80", "101", "124", "179",
            "180", "181", "182", "183", "366", "381", "449", "450", "451",
            "481", "482", "483", "504", "512", "513", "514", "515", "516",
            "517", "546", "574", "575", "576", "577", "578", "56", "495",
            "692", "160", "627", "628", "541"
        };
}
// Статический список для хранения значений по умолчанию для исключений плиток
internal class PhotoTileConverter

    {
        public PhotoTileConverter(string _path)
        {
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

        public static void ParseTileData()
        {
            // Read all lines from the file at the specified tile file path
            string[] tileLines = File.ReadAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), "tiles.txt"));

            // Initialize lists to store colors, tiles, and blocks
            List<Color> colors = new List<Color>();
            List<string> tiles = new List<string>();
            List<string> blocks = new List<string>();

            // Iterate over each line in the file
            foreach (string tileLine in tileLines)
            {
                // Split the line into parts using the colon (:) as a separator
                string[] parts = tileLine.Split(':');
                // Create a color from the HTML code in part 3 and add it to the colors list
                colors.Add(ColorTranslator.FromHtml("#" + parts[3]));
                // Create a tile string by concatenating parts 0, 1, and 2 and add it to the tiles list
                tiles.Add(string.Concat(parts[0], ":", parts[1], ":", parts[2]));
                // Create a block string by concatenating parts 4, 5, and 6 and add it to the blocks list
                blocks.Add(string.Concat(parts[4], parts[5], parts[6]));
            }

            // Assign the blocks list to the Data.list_blocks property

            // Return the colors and tiles lists as arrays
            Tiles = tiles.ToArray();
            Colors = colors.ToArray();
        }
        public static IEnumerable<Color>[] ReadPhoto(Bitmap bitmap, out int x, out int y)
        {
            /*
             * Модуль возвращает массив,
             * содержащий цвета пикселя на каждой .
             * Цвета формируются такким образом:
             * Скрипт идет сверху в низ анализируя каждый пиксель,
             * затем идет на пиксель вправо и повторяет прошлый пункт.
             */
            int hstart;
            Color pixel;
            x = bitmap.Width;
            y = bitmap.Height;
            //bitmap = Blur(bitmap, 2);
            List<Color> Array = new List<Color>(x * y);
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
            ColorApproximater approximater = new ColorApproximater(Colors, Path);
            var bt = (Bitmap)Bitmap.FromFile(Path);
            IEnumerable<Color>[] parts = ReadPhoto(AtkinsonDithering.Do(bt, approximater), out int x, out int y);


            #region "Main six Arrays"
            int BarValue = 0;
            MainFile.Add("0");
            MainFile.Add($"0:{x}");
            MainFile.Add($"{y}");
            foreach (var Chunk in parts)
            {
                AllColors.Clear();
                SingleCopyColors.Clear();
                foreach (var item in Chunk.Distinct().ToArray())
                {
                    BarValue++;
                    SingleCopyColors.Add(item.A < 20 ? Color.FromArgb(0, 0, 0, 0) : approximater.Convert(item) ?? Color.FromArgb(0, 0, 0, 0));
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
            File.WriteAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), "photo" + ".txt"), MainFile.ToArray());
        }
        /*public Bitmap CreatePhoto(int x, int y, string save_path, string art_name)
        {
            string[] list_tiles = _list_tiles;
            Color[] list_colors = _list_colors;
            string[] ImportedTile_Image = Tiles.Length == 0 ? throw new FileNotFoundException("No Tiles File Found") : Tiles;
            int hstart, appreform;
            Bitmap bitmap;
            string[] array;

            if (list_tiles == null)
            {
                array = File.ReadAllLines(extile_path);
                x = System.Convert.ToInt32(array[1].Split(':')[1]) - System.Convert.ToInt32(array[1].Split(':')[0]);
                y = System.Convert.ToInt32(array[2]);
                appreform = 3;
            }
            else
            {
                array = list_tiles;
                appreform = 0;
            }

            bitmap = new Bitmap(x, y);
            List<Color> Colors = new List<Color>();

            for (var i = 0; i < x; i++)
            {
                (sender as BackgroundWorker).ReportProgress(BrogB_Increase(i, bitmap.Width));
                hstart = appreform + i * y;
                if (DoWork == false) { goto endWork; }
                for (var j = 0; j < y; j++)
                {
                    string countion = array[j + hstart];
                    Color color = list_colors[Array.IndexOf(list_tiles, countion)];
                    if (countion == "3:0:0")
                    {
                        bitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0, 0));
                        CellColors.Add(Color.White);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, color);
                        CellColors.Add(color);
                    }
                    Colors.Add(color);
                }
            }

            AllColorsThen = Colors.ToArray();

            if (DoWork == true)
            {
                bitmap.Save(save_path + "modificed_" + art_name + ".jpg");
                return bitmap;
            }
            return new Bitmap(1, 1);
        }*/
    }

    

}
namespace Dithering
{
    #region From Dithering Mischa

    public abstract class DitheringBase<T>
    {
        /// <summary>
        /// Width of bitmap
        /// </summary>
        protected int width;

        /// <summary>
        /// Height of bitmap
        /// </summary>
        protected int height;

        /// <summary>
        /// Long name of the dither method
        /// </summary>
        private readonly string methodLongName = "";

        /// <summary>
        /// Filename addition
        /// </summary>
        private readonly string fileNameAddition = "";

        /// <summary>
        /// Color reduction function/method
        /// </summary>
        protected ColorFunction colorFunction = null;

        /// <summary>
        /// Current bitmap
        /// </summary>
        private IImageFormat<T> currentBitmap;

        /// <summary>
        /// Color function for color reduction
        /// </summary>
        /// <param name="inputColors">Input colors</param>
        /// <param name="outputColors">Output colors</param>
        public delegate void ColorFunction(in T[] inputColors, ref T[] outputColors, ColorApproximater colorApproximater = null);

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="colorfunc">Color reduction function/method</param>
        /// <param name="longName">Long name of dither method</param>
        /// <param name="fileNameAdd">Filename addition</param>
        public DitheringBase(ColorFunction colorfunc, string longName, string fileNameAdd)
        {
            this.colorFunction = colorfunc;
            this.methodLongName = longName;
            this.fileNameAddition = fileNameAdd;
        }

        /// <summary>
        /// Do dithering for chosen image with chosen color reduction method. Work horse, call this when you want to dither something
        /// </summary>
        /// <param name="input">Input image</param>
        /// <returns>Dithered image</returns>
        public IImageFormat<T> DoDithering(IImageFormat<T> input)
        {

            this.width = input.GetWidth();
            this.height = input.GetHeight();
            int channelsPerPixel = input.GetChannelsPerPixel();
            this.currentBitmap = input;

            T[] originalPixel = new T[channelsPerPixel];
            T[] newPixel = new T[channelsPerPixel];
            this.tempBuffer = new T[channelsPerPixel];
            double[] quantError = new double[channelsPerPixel];

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    input.GetPixelChannels(x, y, ref originalPixel);
                    this.colorFunction(in originalPixel, ref newPixel);

                    input.SetPixelChannels(x, y, newPixel);

                    input.GetQuantErrorsPerChannel(in originalPixel, in newPixel, ref quantError);

                    this.PushError(x, y, quantError);
                }
            }

            return input;
        }

        /// <summary>
        /// Get dither method name
        /// </summary>
        /// <returns>String method name</returns>
        public string GetMethodName()
        {
            return this.methodLongName;
        }

        /// <summary>
        /// Get filename addition
        /// </summary>
        /// <returns></returns>
        public string GetFilenameAddition()
        {
            return this.fileNameAddition;
        }

        /// <summary>
        /// Check if image coordinate is valid
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if valid; False otherwise</returns>
        protected bool IsValidCoordinate(int x, int y)
        {
            return (0 <= x && x < this.width && 0 <= y && y < this.height);
        }

        /// <summary>
        /// How error cumulation should be handled. Implement this for every dithering method
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        protected abstract void PushError(int x, int y, double[] quantError);

        private T[] tempBuffer = null;

        /// <summary>
        /// Modify image with error and multiplier
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        /// <param name="multiplier">Multiplier</param>
        public void ModifyImageWithErrorAndMultiplier(int x, int y, double[] quantError, double multiplier)
        {
            this.currentBitmap.GetPixelChannels(x, y, ref this.tempBuffer);

            // We limit the color here because we don't want the value go over min or max
            this.currentBitmap.ModifyPixelChannelsWithQuantError(ref this.tempBuffer, quantError, multiplier);

            this.currentBitmap.SetPixelChannels(x, y, this.tempBuffer);
        }
    }

    //*/

    public sealed class AtkinsonDitheringRGBByte : DitheringBase<byte>
    {
        /// <summary>
        /// Constructor for Atkinson dithering
        /// </summary>
        /// <param name="colorfunc">Color function</param>
        public AtkinsonDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "Atkinson", "_ATK")
        {

        }

        /// <summary>
        /// Push error method for Atkinson dithering
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        override protected void PushError(int x, int y, double[] quantError)
        {
            // Push error
            //        X    1/8   1/8 
            // 1/8   1/8   1/8
            //       1/8

            int xMinusOne = x - 1;
            int xPlusOne = x + 1;
            int xPlusTwo = x + 2;
            int yPlusOne = y + 1;
            int yPlusTwo = y + 2;

            double multiplier = 1.0 / 8.0; // Atkinson Dithering has same multiplier for every item

            // Current row
            int currentRow = y;
            if (this.IsValidCoordinate(xPlusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(xPlusTwo, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, multiplier);
            }

            // Next row
            currentRow = yPlusOne;
            if (this.IsValidCoordinate(xMinusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(x, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(xPlusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
            }

            // Next row
            currentRow = yPlusTwo;
            if (this.IsValidCoordinate(x, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
            }
        }
    }
    public interface IImageFormat<T>
    {
        /// <summary>
        /// Get width
        /// </summary>
        /// <returns>Width of image</returns>
        int GetWidth();

        /// <summary>
        /// Get height
        /// </summary>
        /// <returns>Height of image</returns>
        int GetHeight();

        /// <summary>
        /// Get channels per pixel
        /// </summary>
        /// <returns>Channels per pixel</returns>
        int GetChannelsPerPixel();

        /// <summary>
        /// Get raw content as array
        /// </summary>
        /// <returns>Array</returns>
        T[] GetRawContent();

        /// <summary>
        /// Set pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="newValues">New values</param>
        void SetPixelChannels(int x, int y, T[] newValues);

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Values as array</returns>
        T[] GetPixelChannels(int x, int y);

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="pixelStorage">Array where pixel channels values will be written</param>
        void GetPixelChannels(int x, int y, ref T[] pixelStorage);

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <returns>Error values as double array</returns>
        double[] GetQuantErrorsPerChannel(T[] originalPixel, T[] newPixel);

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <param name="errorValues">Error values as double array</param>
        void GetQuantErrorsPerChannel(in T[] originalPixel, in T[] newPixel, ref double[] errorValues);

        /// <summary>
        /// Modify existing values with quantization errors
        /// </summary>
        /// <param name="modifyValues">Values to modify</param>
        /// <param name="quantErrors">Quantization errors</param>
        /// <param name="multiplier">Multiplier</param>
        void ModifyPixelChannelsWithQuantError(ref T[] modifyValues, double[] quantErrors, double multiplier);
    }
    public sealed class TempByteImageFormat : IImageFormat<byte>
    {
        /// <summary>
        /// Width of bitmap
        /// </summary>
        public readonly int width;

        /// <summary>
        /// Height of bitmap
        /// </summary>
        public readonly int height;

        private readonly byte[,,] content3d;

        private readonly byte[] content1d;

        /// <summary>
        /// How many color channels per pixel
        /// </summary>
        public readonly int channelsPerPixel;

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Input bitmap as three dimensional (widht, height, channels per pixel) byte array</param>
        /// <param name="createCopy">True if you want to create copy of data</param>
        public TempByteImageFormat(byte[,,] input, bool createCopy = false)
        {
            if (createCopy)
            {
                this.content3d = (byte[,,])input.Clone();
            }
            else
            {
                this.content3d = input;
            }

            this.content1d = null;
            this.width = input.GetLength(0);
            this.height = input.GetLength(1);
            this.channelsPerPixel = input.GetLength(2);
        }

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Input byte array</param>
        /// <param name="imageWidth">Width</param>
        /// <param name="imageHeight">Height</param>
        /// <param name="imageChannelsPerPixel">Image channels per pixel</param>
        /// <param name="createCopy">True if you want to create copy of data</param>
        public TempByteImageFormat(byte[] input, int imageWidth, int imageHeight, int imageChannelsPerPixel, bool createCopy = false)
        {
            this.content3d = null;
            if (createCopy)
            {
                this.content1d = new byte[input.Length];
                Buffer.BlockCopy(input, 0, this.content1d, 0, input.Length);
            }
            else
            {
                this.content1d = input;
            }
            this.width = imageWidth;
            this.height = imageHeight;
            this.channelsPerPixel = imageChannelsPerPixel;
        }

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Existing TempByteImageFormat</param>
        public TempByteImageFormat(TempByteImageFormat input)
        {
            if (input.content1d != null)
            {
                this.content1d = input.content1d;
                this.content3d = null;
            }
            else
            {
                this.content3d = input.content3d;
                this.content1d = null;
            }

            this.width = input.width;
            this.height = input.height;
            this.channelsPerPixel = input.channelsPerPixel;
        }

        /// <summary>
        /// Get width of bitmap
        /// </summary>
        /// <returns>Width in pixels</returns>
        public int GetWidth()
        {
            return this.width;
        }

        /// <summary>
        /// Get height of bitmap
        /// </summary>
        /// <returns>Height in pixels</returns>
        public int GetHeight()
        {
            return this.height;
        }

        /// <summary>
        /// Get channels per pixel
        /// </summary>
        /// <returns>Channels per pixel</returns>
        public int GetChannelsPerPixel()
        {
            return this.channelsPerPixel;
        }

        /// <summary>
        /// Get raw content as byte array
        /// </summary>
        /// <returns>Byte array</returns>
        public byte[] GetRawContent()
        {
            if (this.content1d != null)
            {
                return this.content1d;
            }
            else
            {
                byte[] returnArray = new byte[this.width * this.height * this.channelsPerPixel];
                int currentIndex = 0;
                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        for (int i = 0; i < this.channelsPerPixel; i++)
                        {
                            returnArray[currentIndex] = this.content3d[x, y, i];
                            currentIndex++;
                        }
                    }
                }

                return returnArray;
            }
        }

        /// <summary>
        /// Set pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="newValues">New values as object array</param>
        public void SetPixelChannels(int x, int y, byte[] newValues)
        {
            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    this.content1d[indexBase + i] = newValues[i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    this.content3d[x, y, i] = newValues[i];
                }
            }
        }

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Values as byte array</returns>
        public byte[] GetPixelChannels(int x, int y)
        {
            byte[] returnArray = new byte[this.channelsPerPixel];

            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    returnArray[i] = this.content1d[indexBase + i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    returnArray[i] = this.content3d[x, y, i];
                }
            }

            return returnArray;
        }

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="pixelStorage">Array where pixel channels values will be written</param>
        public void GetPixelChannels(int x, int y, ref byte[] pixelStorage)
        {
            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    pixelStorage[i] = this.content1d[indexBase + i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    pixelStorage[i] = this.content3d[x, y, i];
                }
            }
        }

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <returns>Error values as object array</returns>
        public double[] GetQuantErrorsPerChannel(byte[] originalPixel, byte[] newPixel)
        {
            double[] returnValue = new double[this.channelsPerPixel];

            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                returnValue[i] = originalPixel[i] - newPixel[i];
            }

            return returnValue;
        }

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <param name="errorValues">Error values as double array</param>
        public void GetQuantErrorsPerChannel(in byte[] originalPixel, in byte[] newPixel, ref double[] errorValues)
        {
            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                errorValues[i] = originalPixel[i] - newPixel[i];
            }
        }

        /// <summary>
        /// Modify existing values with quantization errors
        /// </summary>
        /// <param name="modifyValues">Values to modify</param>
        /// <param name="quantErrors">Quantization errors</param>
        /// <param name="multiplier">Multiplier</param>
        public void ModifyPixelChannelsWithQuantError(ref byte[] modifyValues, double[] quantErrors, double multiplier)
        {
            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                modifyValues[i] = GetLimitedValue((byte)modifyValues[i], quantErrors[i] * multiplier);
            }
        }

        private static byte GetLimitedValue(byte original, double error)
        {
            double newValue = original + error;
            return Clamp(newValue, byte.MinValue, byte.MaxValue);
        }

        // C# doesn't have a Clamp method so we need this
        private static byte Clamp(double value, double min, double max)
        {
            return (value < min) ? (byte)min : (value > max) ? (byte)max : (byte)value;
        }
    }
    #endregion
    //*/
    public class AtkinsonDithering
    {
        private static ColorApproximater _approximater;
        private static int ColorFunctionMode = 1;
        public static Bitmap Do(Bitmap image, ColorApproximater approximater)
        {
            _approximater = approximater;
            //approximater = new ColorApproximater(new Color[] { Color.White, Color.Black, Color.AliceBlue });
            AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(ColorFunction);
            byte[,,] bytes = ReadBitmapToColorBytes(image);

            TempByteImageFormat temp = new TempByteImageFormat(bytes);
            atkinson.DoDithering(temp);

            WriteToBitmap(image, temp.GetPixelChannels);

            return image;
        }
        private static void ColorFunction(in byte[] input, ref byte[] output, ColorApproximater approximater)
        {

            switch (ColorFunctionMode)
            {
                case 0:
                    TrueColorBytesToWebSafeColorBytes(input, ref output);
                    break;
                default:
                    TrueColorBytesToPalette(input, ref output);
                    break;
            }
        }
        private static void TrueColorBytesToWebSafeColorBytes(in byte[] input, ref byte[] output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(Math.Round(input[i] / 51.0) * 51);
            }
        }
        private static void TrueColorBytesToPalette(in byte[] input, ref byte[] output)
        {
            output = _approximater.Convert((input[0], input[1], input[2]));
            //output = new byte[] { i.R, i.G, i.B};
        }




        
        /*
#if OpenCv
        
        public static Bitmap Do(Mat img, ColorApproximater approximater)
        {
            Bitmap image = img.ToBitmap();
            _approximater = approximater;
            //approximater = new ColorApproximater(new Color[] { Color.White, Color.Black, Color.AliceBlue, Color.Red, Color.Yellow });
            AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(ColorFunction);

            byte[,,] bytes = ReadBitmapToColorBytes(image);

            TempByteImageFormat temp = new TempByteImageFormat(bytes);
            atkinson.DoDithering(temp);

            WriteToBitmap(image, temp.GetPixelChannels);

            return image;
        }
        
#endif
        */
        private static byte[,,] ReadBitmapToColorBytes(Bitmap bitmap)
        {
            byte[,,] returnValue = new byte[bitmap.Width, bitmap.Height, 3];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    returnValue[x, y, 0] = color.R;
                    returnValue[x, y, 1] = color.G;
                    returnValue[x, y, 2] = color.B;
                }
            }
            return returnValue;
        }

        private static void WriteToBitmap(Bitmap bitmap, Func<int, int, byte[]> reader)
        {
            BinaryWorker worker = new BinaryWorker();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {

                    byte[] read = reader(x, y);
                    worker.FileValues.Add(_approximater.GetColor((read[0], read[1], read[2])));
                    Color color = Color.FromArgb(read[0], read[1], read[2]);
                    bitmap.SetPixel(x, y, color);
                }
            }
            worker.Write((ushort)bitmap.Width, (ushort)bitmap.Height);
        }
    }

    public class ColorApproximater
    {

        private static string _tilesPath;
        /// <summary>
        /// Call:
        /// <br></br>     Color color = Color.White;
        /// <br></br>     ColorApproximater Approximater = new ColorApproximater(list_colors);
        /// <br></br>     var cl = Approximater.Convert(color);
        /// </summary>
        public ColorApproximater(Color[] colorslist, string Path, int maxlenght = 1000)
        {
            _tilesPath = Path;
            _maxLenght = maxlenght;
            _hueRgbRange = SetHueEqRgb();
            _findedColors = new List<(byte, byte, byte)>();
            _convertedColors = new List<(byte, byte, byte)>();
            _colors = new List<List<(byte, byte, byte)>>();
            _list_colors = ColorsToBytes(colorslist);
            SetColors();
            _pixels = new Pixels(File.ReadAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_tilesPath), "tiles.txt")));
        }
        public ColorApproximater((byte, byte, byte)[] colorslist, string Path, int maxlenght = 1000)
        {

            _tilesPath = Path; 
            _maxLenght = maxlenght;
            _hueRgbRange = SetHueEqRgb();
            _findedColors = new List<(byte, byte, byte)>();
            _convertedColors = new List<(byte, byte, byte)>();
            _colors = new List<List<(byte, byte, byte)>>();
            _list_colors = colorslist;
            SetColors();
        }
        public ColorApproximater(Pixels colorslist, int maxlenght = 1000)
        {
            _pixels = colorslist;
            _maxLenght = maxlenght;
            _hueRgbRange = SetHueEqRgb();
            _findedColors = new List<(byte, byte, byte)>();
            _convertedColors = new List<(byte, byte, byte)>();
            _colors = new List<List<(byte, byte, byte)>>();
            _list_colors = colorslist.GetColors().ToArray();
            SetColors();
        }
        public (bool, bool, ushort, byte) GetColor((byte, byte, byte) a)
        {
            return _pixels.GetPixels()[_pixels.GetColors().IndexOf(a)];
        }

        /// <summary>
        /// The Convert method takes a Color object as an argument and returns a Color? object.
        /// <br></br>Inside the method, an empty Diffs list is created that will store the differences between the color of the color and each color in the array obtained using the GetColors method and the index obtained using the GetIndexOfColor method.
        /// <br></br> Next, a loop occurs in which for each color from the array the difference is calculated using the ColorDiff method and added to the Diffs list.
        /// <br></br> Finally, the method returns the color from the array that has the minimum color difference.
        /// <br></br>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public byte[] Convert((byte, byte, byte) color)
        {
            //SetColors();
            int index;
            if ((index = _findedColors.IndexOf(color)) != -1)
            {
                return new byte[3] { _convertedColors[index].Item1, _convertedColors[index].Item2, _convertedColors[index].Item3 };
            }
            List<double> Diffs = new List<double>();
            int indas = GetIndexOfColor(color);
            var Array = GetColors(indas);
            foreach (var item in Array)
            {
                Diffs.Add(ColorDiff(item, color));
            }

            _findedColors.Add(color);
            var color2 = Array[Diffs.IndexOf(Diffs.Min())];
            _convertedColors.Add(color2);
            if (_findedColors.Count == _maxLenght)
            {
                ResetAHalfOfConverted();
            }
            return new byte[3] { color2.Item1, color2.Item2, color2.Item3 };
        }
        public Color? Convert(Color _color)
        {
            int index;
            (byte, byte, byte) color = (_color.R, _color.G, _color.B);
            if ((index = _findedColors.IndexOf(color)) != -1)
            {
                return Color.FromArgb(_convertedColors[index].Item1, _convertedColors[index].Item2, _convertedColors[index].Item3);
            }
            List<double> Diffs = new List<double>();
            int indas = GetIndexOfColor(color);
            var Array = GetColors(indas);
            foreach (var item in Array)
            {
                Diffs.Add(ColorDiff(item, color));
            }

            _findedColors.Add(color);
            var color2 = Array[Diffs.IndexOf(Diffs.Min())];
            _convertedColors.Add(color2);
            if (_findedColors.Count == _maxLenght)
            {
                ResetAHalfOfConverted();
            }
            return Color.FromArgb(color2.Item1, color2.Item2, color2.Item3);
        }
        private static (byte, byte, byte)[] ColorsToBytes(Color[] colors)
        {
            (byte, byte, byte)[] result = new (byte, byte, byte)[colors.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (colors[i].R, colors[i].G, colors[i].B);
            }
            return result;
        }
        /// <summary>
        /// Private enumeration called _color, which represents different colors. 
        /// </summary>
        private enum _color
        {
            Red = 0,
            OrangeRed = 1,
            Orange = 2,
            OrangeYellow = 3,
            Yellow = 4,
            LemonYellow = 5,
            YellowGreen = 6,
            SapGreen = 7,
            Green = 8,
            BluishGreen = 9,
            Turquoise = 10,
            GreenishCyan = 11,
            CyanBlue = 12,
            BluishCyan = 13,
            Blue = 14,
            BlueViolet = 15,
            Violet = 16,
            PurpleViolet = 17,
            Purple = 18,
            PurpleMagenta = 19,
            Magenta = 20,
            Crimson = 21,
            Scarlet = 22,
            ScarletRed = 23
            // Here is the documentation for each color:
            // - Red: Represents the color red.Value: 0.
            // - OrangeRed: Represents the color orange-red.Value: 1.
            // - Orange: Represents the color orange. Value: 2.
            // - OrangeYellow: Represents the color orange-yellow.Value: 3.
            // - Yellow: Represents the color yellow. Value: 4.
            // - LemonYellow: Represents the color lemon-yellow.Value: 5.
            // - YellowGreen: Represents the color yellow-green.Value: 6.
            // - SapGreen: Represents the color sap green.Value: 7.
            // - Green: Represents the color green. Value: 8.
            // - BluishGreen: Represents the color bluish green.Value: 9.
            // - Turquoise: Represents the color turquoise. Value: 10.
            // - GreenishCyan: Represents the color greenish cyan.Value: 11.
            // - CyanBlue: Represents the color cyan-blue.Value: 12.
            // - BluishCyan: Represents the color bluish cyan.Value: 13.
            // - Blue: Represents the color blue. Value: 14.
            // - BlueViolet: Represents the color blue-violet.Value: 15.
            // - Violet: Represents the color violet. Value: 16.
            // - PurpleViolet: Represents the color purple-violet.Value: 17.
            // - Purple: Represents the color purple. Value: 18.
            // - PurpleMagenta: Represents the color purple-magenta.Value: 19.
            // - Magenta: Represents the color magenta. Value: 20.
            // - Crimson: Represents the color crimson. Value: 21.
            // - Scarlet: Represents the color scarlet. Value: 22.
            // - ScarletRed: Represents the color scarlet-red.Value: 23.
        }
        /// <summary>
        /// HueRange list that contains the hue degree ranges for each color in the previous code. Each element of the list is a tuple of two numbers representing the starting and ending degrees of hue for the corresponding color.
        /// For example, the first element of the list (7.5, 22.5) indicates that the color shade Red corresponds to the degree range from 7.5 to 22.5.
        /// This list is used to define the range of degrees for the hue of each color when performing color operations.
        /// </summary>
        private static readonly List<(float, float)> HueRange = new List<(float, float)>(24)
        {
            (352.5f, 7.5f),
            (7.5f, 22.5f),
            (22.5f, 37.5f),
            (37.5f, 52.5f),
            (52.5f, 67.5f),
            (67.5f, 82.5f),
            (82.5f, 97.5f),
            (97.5f, 112.5f),
            (112.5f, 127.5f),
            (127.5f, 142.5f),
            (142.5f, 157.5f),
            (157.5f, 172.5f),
            (172.5f, 187.5f),
            (187.5f, 202.5f),
            (202.5f, 217.5f),
            (217.5f, 232.5f),
            (232.5f, 247.5f),
            (247.5f, 262.5f),
            (262.5f, 277.5f),
            (277.5f, 292.5f),
            (292.5f, 307.5f),
            (307.5f, 322.5f),
            (322.5f, 337.5f),
            (337.5f, 352.5f)
        };
        private static int _maxLenght;
        public static int MaxLenght
        {
            get { return _maxLenght; }
        }
        private static List<(byte, byte, byte)> _findedColors;
        private static List<(byte, byte, byte)> _convertedColors;
        public List<List<(byte, byte, byte)>> _hueRgbRange;
        public List<List<(byte, byte, byte)>> _colors;
        private (byte, byte, byte)[] _list_colors;
        private static List<int> skip_colorslist = new List<int>();
        private Pixels _pixels;
        public void Reset()
        {
            _findedColors.Clear();
            _convertedColors.Clear();
        }


        /// <summary>
        ///The Colors class contains several static methods for working with colors.
        /// </summary>
        #region Colors
        /// <summary>
        ///The SetHueEqRgb method creates a new list of color lists, where each inner list contains colors corresponding to a specific degree range. This method uses the GetColorsFromHueRange method.
        /// </summary>
        /// <returns></returns>
        public static List<List<(byte, byte, byte)>> SetHueEqRgb()
        {
            (float, float) Hue;
            List<List<(byte, byte, byte)>> list = new List<List<(byte, byte, byte)>>(24);
            for (int i = 0; i < HueRange.Count; i += 1)
            {
                Hue = HueRange[i];
                list.Add(GetColorsFromHueRange(Hue));
            }
            list[HueRange.Count - 1].RemoveAt(list[HueRange.Count - 1].Count - 1);
            return list;
        }
        private float GetHue(byte r, byte g, byte b)
        {

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out int min, out int max, r, g, b);

            float delta = max - min;
            float hue;

            if (r == max)
                hue = (g - b) / delta;
            else if (g == max)
                hue = (b - r) / delta + 2f;
            else
                hue = (r - g) / delta + 4f;

            hue *= 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }
        private static void MinMaxRgb(out int min, out int max, byte r, byte g, byte b)
        {
            if (r > g)
            {
                max = r;
                min = g;
            }
            else
            {
                max = g;
                min = r;
            }
            if (b > max)
            {
                max = b;
            }
            else if (b < min)
            {
                min = b;
            }
        }
        /// <summary>
        ///The SetColors method initializes the _colors list and fills it with the colors from _list_colors. It then sorts each internal list by its color degree value
        /// </summary>
        public void SetColors()
        {
            for (int i = 0; i < 24; i += 1)
            {
                _colors.Add(new List<(byte, byte, byte)>());
            }
            foreach ((byte, byte, byte) color in _list_colors)
            {
                _colors[GetIndexOfColor(color)].Add((color.Item1, color.Item2, color.Item3));
            }
            for (int ind = 0; ind < _colors.Count; ind += 1)
            {
                _colors[ind] = _colors[ind].OrderBy(x => GetHue(x.Item1, x.Item2, x.Item3)).ToList();
            }
            for (int i = 0; i < GetColors().Count; i++)
            {
                if (GetColors(i).Count == 0)
                    skip_colorslist.Add(i);
            }
        }
        /// <summary>
        ///The GetColors method returns a list of all colors represented as a list of lists. Each inner list contains colors corresponding to a specific range of degrees.
        /// </summary>
        /// <returns><see cref="_colors"/></returns>
        public List<List<(byte, byte, byte)>> GetColors()
        {
            return _colors;
        }
        /// <summary>
        /// The GetColors(int id) method returns a list of colors for a specific id. The ID is used to select a specific degree range.
        /// </summary>
        /// <param name="id">The ID is used to select a specific degree range.</param>
        /// <returns></returns>
        public List<(byte, byte, byte)> GetColors(int id)
        {
            return _colors[id];
        }
        #endregion
        /// <summary>
        ///The Conversation class contains several static methods for converting colors from the hsl color model.
        /// </summary>
        private static class Conversation
        {
            /// <summary>
            /// HSLToRGB takes an H (hue) value as an argument and returns a Color class object representing the corresponding RGB color. 
            /// <br></br>The method uses a formula for converting colors from HSL to RGB. 
            /// First, the values of the saturation (S) and lightness (L) components are determined. 
            /// It then checks to see if the saturation is zero. 
            /// If so, then all color components are set to the lightness value multiplied by 255. 
            /// Otherwise, the v1 and v2 values are calculated based on the S and L values. 
            /// Then, for each color component (r, g, b), the HueToRGB method is called, which calculates the corresponding RGB value based on the H hue and the v1 and v2 values. 
            /// <br></br>Finally, a Color object is created and returned with the resulting component values.
            /// </summary>
            /// <param name="H">Hue of color</param>
            /// <returns>Color which represented in rgb palette</returns>
            /// <remarks><see href="https://en.wikipedia.org/wiki/HSL_and_HSV">Wiki Page</see></remarks>
            public static Color ToRGB(double H, double S, double L)
            {
                // 
                byte r = 0;
                byte g = 0;
                byte b = 0;
                if (S == 0)
                {
                    r = g = b = (byte)(L * 255);
                }
                else
                {
                    double v1, v2;
                    double hue = (double)H / 360;

                    v2 = (L < 0.5) ? (L * (1 + S)) : ((L + S) - (L * S));
                    v1 = 2 * L - v2;

                    r = (byte)(255 * HueToRGB(v1, v2, hue + (1.0f / 3)));
                    g = (byte)(255 * HueToRGB(v1, v2, hue));
                    b = (byte)(255 * HueToRGB(v1, v2, hue - (1.0f / 3)));
                }

                return Color.FromArgb(r, g, b);
            }
            public static (byte, byte, byte) ToBytes(double H, double S, double L)
            {
                // 
                byte r = 0;
                byte g = 0;
                byte b = 0;
                if (S == 0)
                {
                    r = g = b = (byte)(L * 255);
                }
                else
                {
                    double v1, v2;
                    double hue = (double)H / 360;

                    v2 = (L < 0.5) ? (L * (1 + S)) : ((L + S) - (L * S));
                    v1 = 2 * L - v2;

                    r = (byte)(255 * HueToRGB(v1, v2, hue + (1.0f / 3)));
                    g = (byte)(255 * HueToRGB(v1, v2, hue));
                    b = (byte)(255 * HueToRGB(v1, v2, hue - (1.0f / 3)));
                }

                return (r, g, b);
            }
            /// <summary>
            /// HueToRGB takes v1, v2, and vH as arguments and returns a double representing the RGB color component.
            /// The method uses a formula for converting hue to RGB.<br></br>
            /// It first checks whether the vH value is between 0 and 1.
            /// If it is not, the vH value is adjusted by adding or subtracting 1.
            /// The conditions for different ranges of vH values are then checked and the corresponding RGB values are returned.
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <param name="vH"></param>
            /// <returns></returns>
            private static double HueToRGB(double v1, double v2, double vH)
            {
                if (vH < 0)
                    vH += 1;

                if (vH > 1)
                    vH -= 1;

                if ((6 * vH) < 1)
                    return (v1 + (v2 - v1) * 6 * vH);

                if ((2 * vH) < 1)
                    return v2;

                if ((3 * vH) < 2)
                    return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

                return v1;
            }

        }

        /// <summary>
        /// HSLToRGB takes an H (hue) value as an argument and returns a Color class object representing the corresponding RGB color. 
        /// <br></br>The method uses a formula for converting colors from HSL to RGB. 
        /// First, the values of the saturation (S) and lightness (L) components are determined. 
        /// It then checks to see if the saturation is zero. 
        /// If so, then all color components are set to the lightness value multiplied by 255. 
        /// Otherwise, the v1 and v2 values are calculated based on the S and L values. 
        /// Then, for each color component (r, g, b), the HueToRGB method is called, which calculates the corresponding RGB value based on the H hue and the v1 and v2 values. 
        /// <br></br>Finally, a Color object is created and returned with the resulting component values.
        /// </summary>
        /// <param name="H">Hue of color</param>
        /// <returns>Color which represented in rgb palette</returns>
        private static Color HSLToRGB(double H, double S = 1, double L = 0.5)
        {
            return Conversation.ToRGB(H, S, L);
        }
        private static (byte, byte, byte) HSLToBytes(double H, double S = 1, double L = 0.5)
        {
            return Conversation.ToBytes(H, S, L);
        }
        private static void ResetAHalfOfConverted()
        {
            _findedColors = _findedColors.Skip(MaxLenght / 2).ToList();
            _convertedColors = _convertedColors.Skip(MaxLenght / 2).ToList();
        }

        /// <summary>
        ///The static ColorDiff method calculates the difference between two colors. 
        ///The method takes two objects of the Color class as arguments and returns a double value representing the difference between the colors.
        ///<br></br><br></br>The method internally uses the Euclidean distance formula between two points in 3D space to calculate the difference between the components of RGB (red, green, blue) colors. 
        ///<br></br><br></br>The difference between each pair of components is calculated by subtracting one component from the other and then squaring it. 
        ///Then all the differences are added and the square root is taken from the sum. 
        ///This gives the overall difference between the two colors
        /// </summary>
        /// <param name="c1">Color1</param>
        /// <param name="c2">Color2</param>
        /// <returns></returns>
        public static float ColorDiff(Color c1, Color c2)
        {
            return (float)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                 + (c1.G - c2.G) * (c1.G - c2.G)
                                 + (c1.B - c2.B) * (c1.B - c2.B));
        }
        public static float ColorDiff((byte, byte, byte) c1, (byte, byte, byte) c2)
        {
            return (float)Math.Sqrt((c1.Item1 - c2.Item1) * (c1.Item1 - c2.Item1)
                                 + (c1.Item2 - c2.Item2) * (c1.Item2 - c2.Item2)
                                 + (c1.Item3 - c2.Item3) * (c1.Item3 - c2.Item3));
        }

        /// <summary>
        /// The GetIndexOfColor method takes a Color object as an argument and returns the index of the color in the HueRange.
        /// <br></br> Inside the method, a list of diffs is created, which will store the differences between the color color and each color from the _hueRgbRange range.
        /// <br></br> Then there is a double loop where, for each hue range and each color within that range, the difference is calculated using the ColorDiff method and added to the diffs list.
        /// <br></br> Finally, the method returns the index of the minimum value in the diffs list divided by 16.
        /// <br></br> This index is the hue index (one of twenty-four)</summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private int GetIndexOfColor(Color color)
        {
            List<float> diffs = new List<float>(HueRange.Count * 16);
            List<float> tmp = new List<float> {
                720, 720, 720, 720,
                720, 720, 720, 720,
                720, 720, 720, 720,
                720, 720, 720, 720};
            for (int rangeInd = 0; rangeInd < HueRange.Count; rangeInd++)
            {
                if (skip_colorslist.Contains(rangeInd))
                {
                    diffs.AddRange(tmp);
                    continue;
                }
                foreach (var item in _hueRgbRange[rangeInd])
                {
                    diffs.Add(ColorDiff(color, Color.FromArgb(item.Item1, item.Item2, item.Item3)));
                }
            }
            return (int)Math.Floor((double)diffs.IndexOf(diffs.Min()) / 16);
        }
        private int GetIndexOfColor((byte, byte, byte) color)
        {
            List<float> diffs = new List<float>(HueRange.Count * 16);
            List<float> tmp = new List<float> {
                720, 720, 720, 720,
                720, 720, 720, 720,
                720, 720, 720, 720,
                720, 720, 720, 720};
            for (int rangeInd = 0; rangeInd < HueRange.Count; rangeInd++)
            {
                if (skip_colorslist.Contains(rangeInd))
                {
                    diffs.AddRange(tmp);
                    continue;
                }
                foreach (var item in _hueRgbRange[rangeInd])
                {
                    diffs.Add(ColorDiff(color, item));
                }
            }
            return (int)Math.Floor((double)diffs.IndexOf(diffs.Min()) / 16);
        }
        /// <summary>
        /// The GetColorsFromHueRange method takes a hue tuple containing the minimum and maximum hue value and returns a list of colors from that range.
        /// <br></br> It then checks for the special case where the minimum hue value is 352.5
        /// <br></br> In this case, the loop starts with the minimum hue value and continues up to 360, adding each color to the list.
        /// <br></br> The loop then continues from 0.5 to the maximum hue value, also adding each color to the list.
        /// <br></br> Otherwise, the loop goes from the minimum to maximum hue value, adding each color to list.
        /// <br></br> Finally, the method returns a list with colors from a range of shades.
        /// </summary>
        /// <param name="hue"></param>
        /// <returns></returns>
        private static List<(byte, byte, byte)> GetColorsFromHueRange((float, float) hue)
        {
            float min = hue.Item1, max = hue.Item2;
            List<(byte, byte, byte)> list = new List<(byte, byte, byte)>(16);
            if (min == 352.5)
            {
                for (float degree = min; degree < 360; degree += 1)
                {
                    list.Add(HSLToBytes(degree));
                }
                for (float degree = 0.5f; degree <= hue.Item2; degree += 1)
                {
                    list.Add(HSLToBytes(degree));
                }
            }
            else
            {
                for (float degree = hue.Item1; degree <= hue.Item2; degree += 1)
                {
                    list.Add(HSLToBytes(degree));
                }
            }
            return list;
        }

        

    }
}


