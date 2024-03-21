//#define OpenCv
using Aspose.Cells.Revisions;
using PixelArt.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelArt.Tools
{
    public class ColorApproximater
    {

        /// <summary>
        /// Call:
        /// <br></br>     Color color = Color.White;
        /// <br></br>     ColorApproximater Approximater = new ColorApproximater(list_colors);
        /// <br></br>     var cl = Approximater.Convert(color);
        /// </summary>
        public ColorApproximater(Color[] colorslist, int maxlenght = 1000)
        {
            _maxLenght = maxlenght;
            _hueRgbRange = SetHueEqRgb();
            _findedColors = new List<(byte, byte, byte)>();
            _convertedColors = new List<(byte, byte, byte)>();
            _colors = new List<List<(byte, byte, byte)>>();
            _list_colors = ColorsToBytes(colorslist);
            SetColors();
        }
        public ColorApproximater((byte, byte, byte)[] colorslist, int maxlenght = 1000)
        {
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

#if OpenCv
#region _             Calc Bluriness. It needs OpenCv. NOW IT DID NOT WORK
    static (double, double) CalcBlurriness(Mat src, Mat dst)
    {
        return (calcBlurriness(src), calcBlurriness(dst));
    }
    static double calcBlurriness(Mat src)
    {
        Mat Gx = new Mat();
        Mat Gy = new Mat();
        CvInvoke.Sobel(src, Gx, Emgu.CV.CvEnum.DepthType.Cv32F, 1, 0);
        CvInvoke.Sobel(src, Gy, Emgu.CV.CvEnum.DepthType.Cv32F, 0, 1);
        double normGx = CvInvoke.Norm(Gx);
        double normGy = CvInvoke.Norm(Gy);
        double sumSq = normGx * normGx + normGy * normGy;
        double result = (double)(1.0 / (sumSq / (src.Size.Height * src.Size.Width) + 1e-6));
        return result;
    }
    public static void GradientFinder()
    {
        var approximater = new ColorApproximater(new Color[] { Color.White, Color.Black });
        Bitmap image = new Bitmap("C:\\wallpapers\\2.jpg");
        Bitmap result = (Bitmap)image.Clone();

        Rectangle rect;
        Mat def, dst = new Mat();
        int sqr = 20;
        int squareSize = image.Width < sqr ? image.Width : sqr, squareSizeX = squareSize, squareSizeY = squareSize; // Разбиваем на квадраты 30x30
        for (int x = 0; x < image.Width; x += squareSize)
        {
            for (int y = 0; y < image.Height; y += squareSize)
            {
                if (x + squareSize >= image.Width)
                {
                    squareSizeX = image.Width - x;
                    if (squareSizeX <= 0) { continue; }
                }
                else
                {
                    squareSizeX = squareSize;
                }
                if (y + squareSize >= image.Height)
                {
                    squareSizeY = image.Height - y;
                    if (squareSizeY <= 0) { continue; }
                }
                else
                {
                    squareSizeY = squareSize;
                }
                rect = new Rectangle(x, y, squareSizeX, squareSizeY);

                using (Bitmap square = image.Clone(rect, image.PixelFormat))
                {
                    def = square.ToMat();
                    CvInvoke.Canny(def, dst, 250, 300);
                    var bluriness = CalcBlurriness(def, dst);
                    Graphics gr;
                    switch (bluriness.Item1)
                    {
                        case > 0.00001 when Math.Round(bluriness.Item2, 7) > 0.000009 && bluriness.Item2 != 1000000: // 0.00006
                            gr = Graphics.FromImage(result);
                            gr.DrawImage(AtkinsonDithering.Do(def, approximater), rect);
                            gr.Dispose();

                            break;
                        case > 0.00001 when bluriness.Item2 != 1000000: // 0.00006
                            Console.WriteLine($"Найдены границы, объект {bluriness} {(x, y)}");
                            CvInvoke.Imshow("dst image", dst);
                            CvInvoke.Imshow("orig image", def);
                            { CvInvoke.WaitKey(1); }
                            break;
                        default:
                            //Console.WriteLine($"Градиент, скорее всего. Не найдено границ. {bluriness}");
                            gr = Graphics.FromImage(result);
                            gr.DrawImage(AtkinsonDithering.Do(def, approximater), rect);
                            gr.Dispose();
                            break;
                    }
                }
            }
        }

        result.Save("C:\\wallpapers\\7.jpg");
    }
#endregion
#endif

}
