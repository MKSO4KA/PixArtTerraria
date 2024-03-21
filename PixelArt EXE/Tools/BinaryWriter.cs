using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelArt.Tools
{
    public class BinaryWorker
    {
        /// <summary>
        /// Path of result while <3
        /// </summary>
        internal string Path
        {
            private get { return @"C:\wallpapers\TET.txt"; }
            set { Path = value; }
        }
        public List<(bool, bool, ushort, byte)> FileValues = new List<(bool, bool, ushort, byte)>();

        internal List<(bool, bool, ushort, byte)> Read()
        {
            List<(bool, bool, ushort, byte)> Array = new List<(bool, bool, ushort, byte)>();
            byte[] bytes = File.ReadAllBytes(Path);
            ushort WidthStart = (ushort)((bytes[0] & 0xff) + ((bytes[1] & 0xff) << 8));
            ushort Width = (ushort)((bytes[2] & 0xff) + ((bytes[3] & 0xff) << 8));
            ushort Height = (ushort)((bytes[4] & 0xff) + ((bytes[5] & 0xff) << 8));
            for (int i = 6; i < bytes.Length && i + 4 < bytes.Length; i += 5)
            {
                Array.Add((
                    Convert.ToBoolean(bytes[i]),
                    Convert.ToBoolean(bytes[i + 1]),
                    (ushort)((bytes[i + 2] & 0xff) + ((bytes[i + 3] & 0xff) << 8)),
                    bytes[i + 4]
                    ));
            }
            return Array;
        }
        /// <summary>
        /// Конвертирует число в формате ushort, в строку длинной lenght.(до 8) 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        private static string ConvertToBinary(ushort value, byte lenght)
        {
            string tmp = String.Empty, result = Convert.ToString(value, 2);

            for (int i = 0; i < (lenght - result.Length); i++)
            {
                tmp += "0";
            }
            // Temp Also Add TO Git
            return tmp + result;
        }
        internal void Write(ushort Width, ushort Height, ushort WidthStart = 0, List<(bool, bool, ushort, byte)> Array = null)
        {
            Array = Array ?? FileValues;
            using (var stream = File.Open(Path, FileMode.Create))
            {

                using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    binaryWriter.Write(WidthStart); // WidthStart
                    binaryWriter.Write(Width); // width
                    binaryWriter.Write(Height); // height
                    for (int index = 0; index < Array.Count; index++)
                    {
                        binaryWriter.Write(Array[index].Item1); // Wall?
                        binaryWriter.Write(Array[index].Item2); // Torch?
                        binaryWriter.Write(Array[index].Item3); // Id
                        binaryWriter.Write(Array[index].Item4); // Paint

                    }
                }

            }


        }
    }

}
