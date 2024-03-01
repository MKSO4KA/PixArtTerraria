using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PixelArt.Tools
{
    internal class ColorApproximater
    {
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
        }
        private static List<(double, double)> HueRange = new List<(double, double)>(24)
        {
            (7.5, 22.5),
            (22.5, 37.5),
            (37.5, 52.5),
            (52.5, 67.5),
            (67.5, 82.5),
            (82.5, 97.5),
            (97.5, 112.5),
            (112.5, 127.5),
            (127.5, 142.5),
            (142.5, 157.5),
            (157.5, 172.5),
            (172.5, 187.5),
            (187.5, 202.5),
            (202.5, 217.5),
            (217.5, 232.5),
            (232.5, 247.5),
            (247.5, 262.5),
            (262.5, 277.5),
            (277.5, 292.5),
            (292.5, 307.5),
            (307.5, 322.5),
            (322.5, 337.5),
            (337.5, 352.5)
        };
        public static List<List<Color>> HueRgbRange = Colors.SetHueEqRgb();
        public static List<List<Color>> _colors = new List<List<Color>>();
        private static List<Color> list_colors = Data.list_colors.ToList();
        internal static class Colors
        {
            public static List<List<Color>> GetColors()
            {
                return _colors;
            }
            public static List<Color> GetColors(int id)
            {
                return _colors[id];
            }
            public static List<List<Color>> SetHueEqRgb()
            {
                (double, double) Hue;
                List<List<Color>> list = new List<List<Color>>(24);
                for (int i = 0; i < HueRange.Count; i += 1)
                {
                    Hue = HueRange[i];
                    list.Add(GetColorsFromHueRange(Hue));
                }
                list[HueRange.Count - 1].RemoveAt(list[HueRange.Count - 1].Count - 1);
                return list;
            }
            public static void SetColors()
            {
                for (int i = 0; i < 24; i += 1)
                {
                    _colors.Add(new List<Color>());
                }
                foreach (Color color in list_colors)
                {
                    _colors[GetIndexOfColor(color)].Add(color);
                }
                for (int ind = 0; ind < _colors.Count; ind += 1)
                {
                    _colors[ind] = _colors[ind].OrderBy(x => x.GetHue()).ToList();
                }
            }
        }

        public static double ColorDiff(Color c1, Color c2)
        {
            return (double)Math.Sqrt((c1.R - c2.R) * (c1.R - c2.R)
                                 + (c1.G - c2.G) * (c1.G - c2.G)
                                 + (c1.B - c2.B) * (c1.B - c2.B));
        }
        public static Color HSLToRGB(double H)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            double S = 1, L = 0.5;
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

            return Color.FromArgb(r,g,b);
        }

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

        private static int GetIndexOfColor(Color color)
        {
            List<double> diffs = new List<double>(HueRange.Count * 16);
            for (int rangeInd = 0; rangeInd < HueRange.Count; rangeInd++)
            {
                foreach (var item in HueRgbRange[rangeInd])
                {
                    diffs.Add(ColorDiff(color, item));
                }
            }
            return (int)Math.Floor((double)diffs.IndexOf(diffs.Min()) / 16);
        }
        private static List<Color> GetColorsFromHueRange((double,double) hue)
        {
            double min = hue.Item1, max = hue.Item2;
            List<Color> list = new List<Color>(16);
            if (min == 352.5)
            {
                for (double degree = min; degree < 360; degree+= 1)
                {
                    list.Add(HSLToRGB(degree));
                }
                for (double degree = 0.5; degree <= hue.Item2; degree += 1)
                {
                    list.Add(HSLToRGB(degree));
                }
            } else
            {
                for (double degree = hue.Item1; degree <= hue.Item2; degree += 1)
                {
                    list.Add(HSLToRGB(degree));
                }
            }
            return list;
        }
        public static Color? Convert(Color color)
        {
            List<double> Diffs = new List<double>();
            var Array = Colors.GetColors(GetIndexOfColor(color));
            foreach (var item in Array)
            {
                Diffs.Add(ColorDiff(item, color));
            }
            return Array[Diffs.IndexOf(Diffs.Min())];
        }
    }
}
