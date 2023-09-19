using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelArt
{
    
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Data.Init();
        }

        #region temp
        /*
         *         private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                Data.tiles_path = tile_path.Text;
                tile_path.BackColor = Color.LemonChiffon;
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                Data.photo_path = photo_path.Text;
                photo_path.BackColor = Color.LemonChiffon;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                Data.extile_path = extile_path.Text;
                extile_path.BackColor = Color.LemonChiffon;
            }
        }
         */
        #endregion

        #region TextBoxes
        
        private void Art_name_TextBoxUI(object sender, EventArgs e)
        {
            if (IsStringEqLetter(ArtName.Text)) { Data.art_name = null; ArtName.BackColor = Color.White; return; }
            Data.art_name = ArtName.Text;
            ArtName.BackColor = Color.LemonChiffon;
        }
        private void Output_path_TextBoxUI(object sender, EventArgs e)
        {
            if (IsStringEqLetterv2(OutputPath.Text) || !OutputPath.Text.Contains(@":\") || !Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1]))
            { Data.output_path = null; OutputPath.BackColor = Color.White; return; }
            Data.output_path = OutputPath.Text + "\\";
            OutputPath.BackColor = Color.LemonChiffon;
        }
        private void tile_path_TextBoxUI(object sender, EventArgs e)
        {
            if (!tile_path.Text.Contains(".txt")) { Data.tiles_path = null; tile_path.BackColor = Color.White; return; }
            Data.tiles_path = tile_path.Text;
            tile_path.BackColor = Color.LemonChiffon;
        }
        private void photo_path_TextBoxUI(object sender, EventArgs e)
        {
            if (!photo_path.Text.Contains(".jpg") && !photo_path.Text.Contains(".png")) { Data.photo_path = null; photo_path.BackColor = Color.White; return; }
            Data.photo_path = photo_path.Text;
            photo_path.BackColor = Color.LemonChiffon;
        }
        private void extile_path_TextBoxUI(object sender, EventArgs e)
        {
            if (!extile_path.Text.Contains(".txt")) { Data.extile_path = null; extile_path.BackColor = Color.White; return; }
            Data.extile_path = extile_path.Text;
            extile_path.BackColor = Color.LemonChiffon;
        }
        private void torch_path_TextBoxUI(object sender, EventArgs e)
        {
            if (PathInsecct(torch_path.Text)) { Data.torch_path = null; torch_path.BackColor = Color.White; return; }
            Data.torch_path = torch_path.Text;
            torch_path.BackColor = Color.LemonChiffon;
        }
        #endregion

        #region Tiles Progress Bar
        private void CreateTiles_CoWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += worker_CreateTiles;
            worker.ProgressChanged += backgroundWorker1_ProgressChanged2;
            worker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.RunWorkerAsync();

        }
        //Task<Bitmap> t = Task.Run(() => Tools.CreatePhoto(Data.Photo_tiles_list, Data.list_colors, Data.list_tiles,sender,e));
        //Bitmap result = await t;
        public void worker_CreateTiles(object sender, DoWorkEventArgs e)
        {
            //(sender as BackgroundWorker).ReportProgress(5);
            Data.DataSet();
            Tools.CreateTilesPhoto(sender, e);
            //if (!PhotoVisualise_ChekArg()) { return; }
            CreatePhoto_CoWorker();
        }

        #endregion

        #region Photo Progress Bar
        public void CreatePhoto_CoWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += worker_CreatePhoto;
            worker.ProgressChanged += backgroundWorker1_ProgressChanged;
            worker.RunWorkerCompleted += backgroundWorker_2RunWorkerCompleted;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.RunWorkerAsync();

        }
        //Task<Bitmap> t = Task.Run(() => Tools.CreatePhoto(Data.Photo_tiles_list, Data.list_colors, Data.list_tiles,sender,e));
        //Bitmap result = await t;
        public void worker_CreatePhoto(object sender, DoWorkEventArgs e)
        {
            //(sender as BackgroundWorker).ReportProgress(5);
            Data.DataSet();
            Bitmap result = Tools.CreatePhoto(Data.list_colors, Data.list_tiles, sender, e);
            pictureBox1.Image = result;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //Data.WorkName = null;
            progressbar_label_Manipulation();
        }

        #endregion

        #region BackGroundWorker
        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Update UI with progress value
            backgoundInvoke(e);
            progressbar_label_Manipulation();

        }
        public void backgroundWorker1_ProgressChanged2(object sender, ProgressChangedEventArgs e)
        {
            // Update UI with progress value
            backgoundInvoke(e);
            progressbar_label_Manipulation2();

        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update UI when operation is complete
            // e.Result contains the result of the operation
            if (progressBar1.InvokeRequired) progressBar1.Invoke(new Action<int>((s) => progressBar1.Value = s), 0);
            else progressBar1.Value = 0;
            progressbar_label_Manipulation();
            return;
        }
        public void backgroundWorker_2RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update UI when operation is complete
            // e.Result contains the result of the operation
            if (progressBar1.InvokeRequired) progressBar1.Invoke(new Action<int>((s) => progressBar1.Value = s), 0);
            else progressBar1.Value = 0;
            progressbar_label_Manipulation2();
            if (tile_path.InvokeRequired || photo_path.InvokeRequired || extile_path.InvokeRequired || ArtName.InvokeRequired || OutputPath.InvokeRequired)
            {
                TexBox_SetZero(true);
            }
            else
            {
                TexBox_SetZero(false);
            }
            return;
        }

        #endregion

        #region Independent functions
        private bool PathInsecct(string Text, string ext = "txt")
        {
            if (ext == "txt")
            {
                if (!(!(Text == "") && Text.Contains(".txt") && Text.Contains(@":\"))) { return true; }
                return false;
            }
            if (ext == "png")
            {
                if (!(!(photo_path.Text == "") && (photo_path.Text.Contains(".jpg") || photo_path.Text.Contains(".png")) && photo_path.Text.Contains(@":\"))) { return true; }
                return false;
            }
            return false;
        }
        private void RedHighlight(int func = 0)
        {
            Color color =Color.FromArgb(246, 74, 70);
            if (func == 0)
            {
                
                if (photo_path.BackColor != Color.LemonChiffon || Data.photo_path == "") { photo_path.BackColor = color; } else { photo_path.BackColor = Color.LemonChiffon; }
                if (extile_path.BackColor != Color.LemonChiffon || Data.extile_path == "") { extile_path.BackColor = color; } else { extile_path.BackColor = Color.LemonChiffon; }
                if (tile_path.BackColor != Color.LemonChiffon || Data.tiles_path == "") { tile_path.BackColor = color; }
                else
                {
                    tile_path.BackColor = Color.LemonChiffon;
                }
                if (ArtName.BackColor != Color.LemonChiffon || Data.art_name == "") { ArtName.BackColor = color; }
                else
                {
                    ArtName.BackColor = Color.LemonChiffon;
                }
                if (OutputPath.BackColor != Color.LemonChiffon || Data.output_path == "") { OutputPath.BackColor = color; }
                else
                {
                    OutputPath.BackColor = Color.LemonChiffon;
                }
                return;
            }
            if (func == 1)
            {
                if (photo_path.BackColor != Color.LemonChiffon || Data.photo_path == "") { photo_path.BackColor = color; }
                else
                {
                    photo_path.BackColor = Color.LemonChiffon;
                }
                if (tile_path.BackColor != Color.LemonChiffon || Data.tiles_path == "") { tile_path.BackColor = color; }
                else
                {
                    tile_path.BackColor = Color.LemonChiffon;
                }
                if (ArtName.BackColor != Color.LemonChiffon || Data.art_name == "") { ArtName.BackColor = color; }
                else
                {
                    ArtName.BackColor = Color.LemonChiffon;
                }
                if (OutputPath.BackColor != Color.LemonChiffon || Data.output_path == "") { OutputPath.BackColor = color; }
                else
                {
                    OutputPath.BackColor = Color.LemonChiffon;
                }
                if (!(!(extile_path.Text == "") && (extile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\")))) { extile_path.BackColor = Color.White; }
                
                return;
            }
            if (func == 2)
            {
                if (extile_path.BackColor != Color.LemonChiffon || Data.extile_path == "") { extile_path.BackColor = color; }
                else
                {
                    extile_path.BackColor = Color.LemonChiffon;
                }
                if (tile_path.BackColor != Color.LemonChiffon || Data.tiles_path == "") { tile_path.BackColor = color; }
                else
                {
                    tile_path.BackColor = Color.LemonChiffon;
                }
                if (ArtName.BackColor != Color.LemonChiffon || Data.art_name == "") { ArtName.BackColor = color; }
                else
                {
                    ArtName.BackColor = Color.LemonChiffon;
                }
                if (OutputPath.BackColor != Color.LemonChiffon || Data.output_path == "") { OutputPath.BackColor = color; }
                else
                {
                    OutputPath.BackColor = Color.LemonChiffon;
                }
                if (!(!(photo_path.Text == "") && (photo_path.Text.Contains(".jpg") || photo_path.Text.Contains(".png")) && photo_path.Text.Contains(@":\"))) { photo_path.BackColor = Color.White; }
                return;
            }

        }
        private bool CheckAllData(int func = 0)
        {
            // func == 0 -- ALL
            // func == 1 -- Tiles
            // func == 2 -- Photo
            if (func == 0)
            {
                if ((!IsStringEqLetter(Data.art_name))
                    && (!IsStringEqLetterv2(Data.output_path) && Data.output_path.Contains(@":\") && Char.IsLetterOrDigit(Data.output_path[Data.output_path.Length - 1]))
                    && (!(Data.tiles_path == "") && Data.tiles_path.Contains(".txt") && Data.tiles_path.Contains(@":\"))
                    && (!(Data.photo_path == "") && (Data.photo_path.Contains(".jpg") || Data.photo_path.Contains(".png")) && Data.photo_path.Contains(@":\"))
                    && (!(Data.extile_path == "") && (Data.extile_path.Contains(".txt") && Data.extile_path.Contains(@":\")))) { return true; }
                RedHighlight(func);
                return false;
            }
            else if (func == 1)
            {
                if ((!IsStringEqLetter(Data.art_name))
                    && (!IsStringEqLetterv2(Data.output_path) && Data.output_path.Contains(@":\") && Char.IsLetterOrDigit(Data.output_path[Data.output_path.Length - 1]))
                    && (!(Data.tiles_path == "") && Data.tiles_path.Contains(".txt") && Data.tiles_path.Contains(@":\"))
                    && (!(Data.photo_path == "") && (Data.photo_path.Contains(".jpg") || Data.photo_path.Contains(".png")) && Data.photo_path.Contains(@":\")))
                { return true; }
                RedHighlight(func);
                return false;
            }
            else if (func == 2)
            {
                if ((!IsStringEqLetter(Data.art_name))
                    && (!IsStringEqLetterv2(Data.output_path) && Data.output_path.Contains(@":\") && Char.IsLetterOrDigit(Data.output_path[Data.output_path.Length - 1]))
                    && (!(Data.tiles_path == "") && Data.tiles_path.Contains(".txt") && Data.tiles_path.Contains(@":\"))
                    && (!(Data.extile_path == "") && (Data.extile_path.Contains(".txt") && Data.extile_path.Contains(@":\")))) { return true; }
                RedHighlight(func);
                return false;
            }
            return false;

        }
        private bool AllCheck(int func = 0)
        {
            // func == 0 -- ALL
            // func == 1 -- Tiles
            // func == 2 -- Photo
            if (func == 0)
            {
                if ((!IsStringEqLetter(ArtName.Text))
                    && (!IsStringEqLetterv2(OutputPath.Text) && OutputPath.Text.Contains(@":\") && Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1]))
                    && (!(tile_path.Text == "") && tile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\"))
                    && (!(photo_path.Text == "") && (photo_path.Text.Contains(".jpg") || photo_path.Text.Contains(".png")) && photo_path.Text.Contains(@":\"))
                    && (!(extile_path.Text == "") && (extile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\")))) { return true; }
                RedHighlight(func);
                return false;
            }
            else if (func == 1)
            {
                if ((!IsStringEqLetter(ArtName.Text))
                    && (!IsStringEqLetterv2(OutputPath.Text) && OutputPath.Text.Contains(@":\") && Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1]))
                    && (!(tile_path.Text == "") && tile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\"))
                    && (!(photo_path.Text == "") && (photo_path.Text.Contains(".jpg") || photo_path.Text.Contains(".png")) && photo_path.Text.Contains(@":\")))
                { return true; }
                RedHighlight(func);
                return false; 
                #region ko
                /*
                int i;
                if (!IsStringEqLetter(ArtName.Text))
                    i = 0;
                if (!IsStringEqLetterv2(OutputPath.Text) && OutputPath.Text.Contains(@":\") && Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1])) { i = 1; }
                if (!(tile_path.Text == "") && tile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\")) { i = 2; }
                if (!(photo_path.Text == "") && (photo_path.Text.Contains(".jpg") || photo_path.Text.Contains(".png")) && photo_path.Text.Contains(@":\")) { i = 3; }
                else { return false; } */
                #endregion
            }
            else if (func == 2)
            {
                if ((!IsStringEqLetter(ArtName.Text))
                    && (!IsStringEqLetterv2(OutputPath.Text) && OutputPath.Text.Contains(@":\") && Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1]))
                    && (!(tile_path.Text == "") && tile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\"))
                    && (!(extile_path.Text == "") && (extile_path.Text.Contains(".txt") && tile_path.Text.Contains(@":\")))) { return true; }
                RedHighlight(func);
                return false;
            }
            return false;

        }
        private bool IsStringEqLetterv2(string line)
        {

            foreach (char item in line)
            {

                if (Char.IsLetterOrDigit(item) == false && item.ToString() != "\\" && item != ':') { return true; };
            }
            if (line == String.Empty) { return true; };
            return false;
        }
        private bool IsStringEqLetter(string line)
        {

            foreach (char item in line)
            {
                if (Char.IsLetterOrDigit(item) == false) { return true; };
            }
            if (line == String.Empty) { return true; };
            return false;
        }
        private void progressbar_label_Manipulation2()
        {
            if (Data.WorkName == null)
            {
                // labelInvoke2
                labelInvoke2(false);
                return;
            }
            labelInvoke2(true);
        }
        private void progressbar_label_Manipulation()
        {
            if (Data.WorkName == null)
            {
                // labelInvoke2
                labelInvoke(false);
                return;
            }
            labelInvoke(true);
        }
        private void textBoxInvoke(bool shit)
        {
            if (shit)
            {
                extile_path.Invoke(new Action<String>((s) => extile_path.Text = s), "");
                photo_path.Invoke(new Action<String>((s) => photo_path.Text = s), "");
                ArtName.Invoke(new Action<String>((s) => ArtName.Text = s), "");
                return;
            }
            extile_path.Text = "";
            photo_path.Text = "";
            ArtName.Text = "";
        }
        private void labelInvoke(bool shit)
        {
            string data;
            if (shit != false)
            {
                int percent = (int)Math.Round((double)100 * Data.Now_Stage / Data.Then_Stage); 
                data = Data.WorkName + " - " + percent + "%";
            }
            else
            {
                data = "";
            }
            if (progressbar_label.InvokeRequired)
            {
                progressbar_label.Invoke(new Action<string>((s) => progressbar_label.Text = s), data);
            }
            else
            {
                progressbar_label.Text = data;

            }
        }
        private void labelInvoke2(bool shit)
        {
            string data;
            if (shit != false)
            {
                int percent = Data.Percent2;
                data = Data.WorkName + " - " + percent + "%";
            }
            else
            {
                data = "";
            }
            if (progressbar_label.InvokeRequired)
            {
                progressbar_label.Invoke(new Action<string>((s) => progressbar_label.Text = s), data);
            }
            else
            {
                progressbar_label.Text = data;

            }
        }
        private void backgoundInvoke(ProgressChangedEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<int>((s) => progressBar1.Value = s), e.ProgressPercentage);
            }
            else
            {
                progressBar1.Value = e.ProgressPercentage;
            }
        }
        private void Data_Text_Zero()
        {
            if (extile_path.InvokeRequired || photo_path.InvokeRequired || ArtName.InvokeRequired)
            {
                textBoxInvoke(true);
                return;
            }
            textBoxInvoke(false);
            Data.art_name = "";
            Data.photo_path = "";
            Data.extile_path = "";
            Data.File_Photo_list = null;
            //Data.output_path = "";
        }
        private void TexBox_SetZero(bool shit)
        {
            Data_Text_Zero();
            if (shit == true)
            {
                //tile_path.Invoke(new Action<Color>((s) => tile_path.BackColor = s), Color.White);
                photo_path.Invoke(new Action<Color>((s) => photo_path.BackColor = s), Color.White);
                extile_path.Invoke(new Action<Color>((s) => extile_path.BackColor = s), Color.White);
                ArtName.Invoke(new Action<Color>((s) => ArtName.BackColor = s), Color.White);
                //OutputPath.Invoke(new Action<Color>((s) => OutputPath.BackColor = s), Color.White);
                //torch_path.Invoke(new Action<Color>((s) => OutputPath.BackColor = s), Color.White);

                return;
            }
            //tile_path.BackColor = Color.White;
            photo_path.BackColor = Color.White;
            extile_path.BackColor = Color.White;
            ArtName.BackColor = Color.White;
            //OutputPath.BackColor = Color.White;
            //torch_path.BackColor = Color.White;
        }
        #endregion

        #region Private Trash Do Not Delete

        private void extile_path_TextChanged(object sender, EventArgs e)
        {

        }

        private void extile_TB_Click(object sender, EventArgs e)
        {

        }

        private void photo_path_TextChanged(object sender, EventArgs e)
        {

        }

        private void photo_TB_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void progressBar1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        #endregion


        #region Buttons
        
       
        private void OutputDirectoryButton_Click(object sender, EventArgs e)
        {
            OutputPath.Text = Tools.FolderPath_Dialog();
            if (!IsStringEqLetterv2(OutputPath.Text) && OutputPath.Text.Contains(@":\") && Char.IsLetterOrDigit(OutputPath.Text[OutputPath.Text.Length - 1]))
            {
                Data.output_path = OutputPath.Text + "\\";
                OutputPath.BackColor = Color.LemonChiffon;
            }
            else
            {
                Data.output_path = null;
                OutputPath.BackColor = Color.White;
            }
            return;
        }
        
        private void ButtonName_Click(object sender, EventArgs e)
        {
            
            if (IsStringEqLetter(ArtName.Text)) { Data.art_name = null; ArtName.BackColor = Color.White; return; }
            Data.art_name = ArtName.Text;
            ArtName.BackColor = Color.LemonChiffon;
        }
        private void tile_button_Click(object sender, EventArgs e)
        {
            tile_path.Text = Tools.Filepath_Dialog("tiles file");
            if (PathInsecct(tile_path.Text)) { Data.tiles_path = null; tile_path.BackColor = Color.White; return; }
            Data.tiles_path = tile_path.Text;
            tile_path.BackColor = Color.LemonChiffon;
        }
        private void photo_button_Click(object sender, EventArgs e)
        {
            photo_path.Text = Tools.Filepath_Dialog("photo file");
            if (PathInsecct(photo_path.Text,"png")) { Data.photo_path = null; photo_path.BackColor = Color.White; return; }
            Data.photo_path = photo_path.Text;
            photo_path.BackColor = Color.LemonChiffon;
        }
        private void extile_button_Click(object sender, EventArgs e)
        {
            extile_path.Text = Tools.Filepath_Dialog("extile file");
            if (PathInsecct(extile_path.Text)) { Data.extile_path = null; extile_path.BackColor = Color.White; return; }
            Data.extile_path = extile_path.Text;
            extile_path.BackColor = Color.LemonChiffon;
        }
        private void torch_button_Click(object sender, EventArgs e)
        {
            torch_path.Text = Tools.Filepath_Dialog("extile file");
            if (PathInsecct(torch_path.Text)) { Data.torch_path = null; torch_path.BackColor = Color.White; return; }
            Data.torch_path = torch_path.Text;
            torch_path.BackColor = Color.LemonChiffon;
        }
        // Main Buttons Right
        private void ImageGen_Click(object sender, EventArgs e)
        {
            if (!AllCheck(1) || CheckAllData(1))
            {
                return;
            }
            Data.Percent = 0;
            Data.save_path = Data.output_path + Data.art_name + "\\";
            Tools.CreateDirectory(Data.save_path, 1);
            pictureBox1.Image = null;
            CreateTiles_CoWorker();
        }
        private void ImageVisualize_Click(object sender, EventArgs e)
        {
            if (!AllCheck(2) || CheckAllData(2))
            {
                return;
            }
            Data.Percent = 0;
            Data.save_path = Data.output_path + Data.art_name + "\\";
            Tools.CreateDirectory(Data.save_path, 1);
            pictureBox1.Image = null;
            // update
            CreatePhoto_CoWorker();
            return;
        }

        




        #endregion

        

        
    }
    public class Data
    {
        #region Constants Belongs
        /* 
         * Requirments:
         * 
         * ProgressBar = {
         *              int Now_Stage
         *              int Then_Stage
         *              string WorkName
         *             }
         * MainPage = {
         *              string tiles_path
         *              string photo_path
         *              string extile_path
         *              string art_name
         *              string output_path
         *              string torch_path
         *             }     
         * BehindScene = {
         *              Color[] list_colors
         *              string[] list_tiles
         *              string[] Photo_tiles_list
         *              int Percent2
         *             } 
         * ArtVisualise = {
         *              string x
         *              string y
         *              string xstart
         *              string[] File_Photo_list
         *             }             
         */
        #endregion
        #region Constants

        // List<string> values
        public static List<string> Numlist = new List<string> { "1", "2", "3", "4","5","6","7","8","9","0" };
        // Int values
        public static int Now_Stage,
                          Then_Stage,
                          Percent2,
                          Percent;
        // String values
        public static string tiles_path,
                             photo_path,
                             extile_path,
                             art_name,
                             output_path,
                             WorkName,
                             x,
                             y,
                             xstart,
                             torch_path,
                             save_path;

        // Bool values
        public static bool new_img = true;
        // Color arrays
        public static Color[] list_colors;
        // String arrays
        public static string[] list_tiles,
                               Photo_tiles_list,
                               File_Photo_list;
        #endregion

        #region Functions
        public static void DataSet()
        {
            if (extile_path != String.Empty && extile_path != null)
            {
                Data.Photo_tiles_list = Tools.fileREAD(extile_path);
            }
            if (tiles_path != String.Empty && tiles_path.Contains(".txt"))
            {
                Tools.Enumerating(out Color[] list_colors, out string[] list_tiles);
                Data.list_colors = list_colors;
                Data.list_tiles = list_tiles;


            }
        }
        public static void Init()
        {
            Data.tiles_path = String.Empty;
            Data.photo_path = String.Empty;
            Data.extile_path = String.Empty;
            Data.art_name = String.Empty;
            Data.output_path = String.Empty;
            Data.Percent = 0;
        }




        #endregion
    }
}
