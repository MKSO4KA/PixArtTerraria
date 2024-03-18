using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelArt.Tools
{
    internal class BinaryWorker
    {
        public static void Main()
        {
            BinaryWorker worker = new BinaryWorker();
            worker.Write(new List<ushort>() { 12, 32, 324, 352, 564 });
            worker.Read();
        }
        /// <summary>
        /// Path of result while <3
        /// </summary>
        internal string Path
        {
            private get { return @"C:\фыв\TET.txt"; }
            set { Path = value; }
        }


        internal List<ushort> Read()
        {
            List<ushort> Array = new List<ushort>();
            byte[] bytes = File.ReadAllBytes(Path);
            for (int i = 0; i < bytes.Length && i + 1 < bytes.Length; i += 2)
            {
                Array.Add((ushort)((bytes[i] & 0xff) + ((bytes[i + 1] & 0xff) << 8)));
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
        internal void Write(List<ushort> Array = null)
        {

            using (var stream = File.Open(Path, FileMode.Create))
            {

                using (var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    binaryWriter.Write((ushort)Array[0]); // WidthStart
                    binaryWriter.Write((ushort)Array[1]); // width
                    binaryWriter.Write((ushort)Array[2]); // height
                    for (int index = 3; index < Array.Count; index++)
                    {
                        binaryWriter.Write(Array[index]); // Pixel
                    }
                }

            }


        }
    }
}
