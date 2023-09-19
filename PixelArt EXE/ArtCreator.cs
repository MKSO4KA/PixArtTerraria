







#region NOT - USE
/*
using System.Security.Policy;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;

namespace PixelArt
{
    internal class ArtCreator
    {
        public static string Filepath_Dialog(string message)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\ARTs\colors";
                openFileDialog.Filter = $"txt files (*.txt)|*.txt|{message} (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                }
                else
                {
                    return "";
                }

            }

            return filePath;
        }

        static string location_png = @"C:\Users\Сисьадмин\Downloads\Без имени-1.png";
        static string name = "TEST";
        static string locationX = "C:\\ARTs\\";
        static string location_name = locationX + "ARTS" + "\\" + name + "\\" + "backup" + "\\";
        static Image imgX = Image.FromFile(location_png);
        static int OldOrNew = 1;

        private static (string,string,string) hexornEX(int fline, (string, string, string, string)[] massiv)
        {
            (string, string, string, string) line = massiv[fline];
            string tileORpaint = line.Item1;
            string tile = line.Item2;
            string paint = line.Item3;
            // string hex = line.Item4; // no use'
            return (tileORpaint, tile, paint);
        }

        private static int chosecor(Color num, string[] file)
        {
            int[] site = new int[file.Length];
            for (int i = 0; i < file.Length; i++)
            {
                Color l = ColorTranslator.FromHtml(file[i]);
                Color o = num;
                site[i] = o.R * o.R - 2 * o.R * l.R + l.R * l.R + o.G * o.G - 2 * o.G * l.G + l.G * l.G + o.B * o.B - 2 * o.B * l.B + l.B * l.B;
                
            }
            return Array.IndexOf(site, site.Min());
        }
           
        
        public static (Color[] list_colors, (string, string, string)[] list_tiles) k()
        {
            #region Start-of-script 
            /* Создание основ и перебор тайлов для дальнейшего использования
             
            string location = locationX;
            Image img = imgX;
            int h = img.Size.Height;
            int w = img.Size.Width;
            Color[] massive = img.Palette.Entries;
            Array.Sort(massive);
            string[] f = File.ReadAllLines(location + "tiles.txt");
            int i = f.Length;


            (string, string, string, string)[] tilesNpaint = new (string, string, string, string)[i];
            string[] files = new string[i];
            (string, string, string)[] blocks = new (string, string, string)[i];

            
            for (int i2 = 0; i2 < i; i2++)
            {
                string lineT = f[i2];
                string[] line = lineT.Split(':');
                tilesNpaint[i2] = (line[0], line[1], line[2], line[3]);
                files[i2] = line[3];
                blocks[i2] = (line[0], line[1], line[2]);


            }
            #endregion

            #region Perebor
            Color[] list_colors = new Color[massive.Length];
            (string, string, string)[] list_tiles = new (string, string, string)[massive.Length];
            int count = 0;
            foreach (Color x in massive)
            {
                //int Red = x.R;
                //i = Array.IndexOf(massive, x);
                int numb = chosecor(x, files);
                string col = files[numb];
                list_colors[count] = x;
                list_tiles[count] = (hexornEX(numb, tilesNpaint));
                count++;
            }

            #endregion

            return (list_colors,list_tiles);

        }
        #region func
        private static ((string, string, string)[] list_tiles, string[] list_colors) func()
        {
            Color[] list_colors;
            (string, string, string)[] list_tiles;
            (list_colors, list_tiles) = k();
            
            if (Directory.Exists(location_name))
            {
                Directory.Delete(location_name, true);
                Directory.CreateDirectory(location_name);
            }
            else
            {
                Directory.CreateDirectory(location_name);
            }
            string[] lines = new string[list_colors.Length];
            
            File.Move(location_png, location_name.Substring(0, location_name.Length - 7) + name + ".png");
            //string[] lines = Array.ConvertAll(list_colors, s => string.Compare);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = list_colors[i].ToArgb().ToString();
            }
            File.WriteAllLines(location_name + "massiveC.ob", lines);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = list_tiles[i].Item1 + ":" + list_tiles[i].Item2 + ":" + list_tiles[i].Item3;
            }
            File.WriteAllLines(location_name + "massiveT.ob", lines);
            return (list_tiles, lines);
        }
        #endregion
        private static ((string, string, string)[] list_tiles, string[] list_colors) fonc()
        {
            // Посмотри код питона там понятней
            /*  
                Color myRgbColor = new Color();
                myRgbColor = Color.FromArgb(65280);
                Console.WriteLine(myRgbColor);
                Console.WriteLine(myRgbColor.ToArgb());
             
            string[] fileT = File.ReadAllLines(location_name + "massiveT.ob");
            string[] list_colors = File.ReadAllLines(location_name + "massiveC.ob");
            (string, string, string)[] list_tiles = new (string, string, string)[fileT.Length];
            try
            {
                
                for (int i = 0; i < fileT.Length; i++)
                {
                    string[] tileTuple = fileT[i].Split(':');
                    list_tiles[i].Item1 = tileTuple[0];
                    list_tiles[i].Item2 = tileTuple[1];
                    list_tiles[i].Item3 = tileTuple[2];
                }
            } catch 
            {
                Environment.Exit(98);
                // Argb color
            }
            return (list_tiles, list_colors);
        }
        private static (string, string, string) Find(Color color, (string, string, string)[] massivT, string[] massivC)
        {
            return massivT[Array.IndexOf(massivC, color.ToArgb())];
        }

        private static (string, string, string) tileNDpaint(int fline, (string, string, string, string)[] massiv)
        {
            (string, string, string, string) line = massiv[fline];
            string tileORpaint = line.Item1;
            string tile = line.Item2;
            string paint = line.Item3;
            return (tileORpaint, tile, paint);

        }

        private static (int,int,int) continueorrestart(int w, int h)
        {
            // pooop = От пользователя
            // poop = str(input("Do you want to continue with the art?(y or n)"))
            string poop = "N";
            if (poop != "n" || poop != "N")
            {
                return WndH(w, h);
            }
            else
            {
                return (0, 0, 0);
            }


        }

        private static (int,int,int) WndH(int w, int h)
        {
            string[] backup_file = File.ReadAllLines(location_name + "backup.txt");
            int touka = Convert.ToInt32(backup_file[0]);
            touka = (touka - 3);
            double k = touka / h;
            int wstart = Convert.ToInt32(Math.Floor(k));
            int hstart = touka % h;
            return (wstart, hstart, touka);
        }

        private static ((string, string, string)[] list_tiles, string[] list_colors) continueor()
        {
            //poop = str(input("Use old-art palette? - Y or N"))
            string poop = "N";
            if (poop == "n" || poop == "N")
            {
                return func();
            }
            else
            {
                return fonc();
            }
                
        }

        public static void g()
        {
            string location = locationX;
            string Fname = location_name;
            
            string[] sprites = File.ReadAllLines(location + "exceptions.txt");
            string[] cowTilePaintHex = File.ReadAllLines(location + "tiles.txt");
            string[] files = new string[cowTilePaintHex.Length];
            (string, string, string, string)[] tilesNpaint = new (string, string, string, string)[cowTilePaintHex.Length];
            for (int i = 0; i < cowTilePaintHex.Length; i++)
            {
                // i = 1:478:0:6C2223
                /*
                 * tilesNpaint:
                 * (1,478,0,6c2223)
                 
                string[] line = cowTilePaintHex[i].Split(':');
                
                if (sprites.Contains(line[1]) && Convert.ToInt32(line[0]) == 1)
                { }
                else
                {
                    tilesNpaint[i].Item1 = line[0];
                    tilesNpaint[i].Item2 = line[1];
                    tilesNpaint[i].Item3 = line[2];
                    tilesNpaint[i].Item4 = line[3];
                    files[i] = line[3];
                }

                int h = imgX.Size.Height;
                int w = imgX.Size.Width;

                int wstart;
                int hstart;
                int tyolka;
                
                (wstart, hstart, tyolka) = continueorrestart(w, h);
                ((string, string, string)[] list_tiles, string[] list_colors) = continueor();
                //string[] my_file = File.ReadAllLines(Fname.Substring(0, Fname.Length - 7));
                File.WriteAllText(Fname.Substring(0, Fname.Length - 7), "1" + ":" + w.ToString() + ":" + h.ToString() + Environment.NewLine);

            }

        }   

    }
}
*/
#endregion



