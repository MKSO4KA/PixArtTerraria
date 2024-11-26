using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PixelArt.Structs
{
    public class Pixels
    {
        public static List<Pixel> Objects = new List<Pixel>();
        public Pixels()
        {
            string path = @"C:\Users\Сисьадмин\Documents\PixelArtCreatorByMixailka\settings.xml";

            XElement file = XDocument.Load(path).Element("Settings");
            List<Pixel> pixels = new List<Pixel>();
            foreach (XElement item in file.Element("Tiles").Elements("Tile"))
            {
                Add(new Pixel(item, false));
            }
            foreach (XElement item in file.Element("Walls").Elements("Wall"))
            {
                Add(new Pixel(item, true));
            }
        }
        public void Add(Pixel pixel)
        {
            Objects.Add(pixel);
        }
        public void Del(Pixel pixel)
        {
            Objects.Remove(pixel);
        }
        public List<(bool, bool, ushort, byte)> GetPixels() { return Objects.Select(x => (x.Wall, x.WallAtached, x.id, x.paint)).ToList(); }
        public List<(byte, byte, byte)> GetColors() { return Objects.Select(x => x.color).ToList(); }
    }
    public class Pixel
    {
        public string Name;
        public bool Wall = false;
        public ushort id;
        public byte paint;
        public (byte, byte, byte) color;
        public bool WallAtached = false;
        public Pixel(XElement element, bool wall = false)
        {
            Name = element.Attribute("name").Value;
            Wall = wall;
            id = Convert.ToUInt16(element.Attribute("num").Value);
            paint = Convert.ToByte(element.Attribute("paintID").Value);
            color = ToBytes(element.Attribute("color").Value);
            WallAtached = element.Attribute("Torch").Value == "true" ? true : false;
        }
        private static (byte, byte, byte) ToBytes(string hexValue)
        {
            int hexColor = Convert.ToInt32(hexValue.Replace("#", ""), 16);
            return ((byte)((hexColor >> 16) & 0xff), (byte)((hexColor >> 8) & 0xff), (byte)(hexColor & 0xff));
        }
    }

}
