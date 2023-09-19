namespace PixelArt
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tile_path = new System.Windows.Forms.TextBox();
            this.photo_path = new System.Windows.Forms.TextBox();
            this.extile_path = new System.Windows.Forms.TextBox();
            this.OutputPath = new System.Windows.Forms.TextBox();
            this.ArtName = new System.Windows.Forms.TextBox();
            this.progressbar_label = new System.Windows.Forms.Label();
            this.Tiles_TB = new System.Windows.Forms.Label();
            this.photo_TB = new System.Windows.Forms.Label();
            this.extile_TB = new System.Windows.Forms.Label();
            this.Output_Directory_TB = new System.Windows.Forms.Label();
            this.ArtName_TB = new System.Windows.Forms.Label();
            this.ImageGen_Button = new System.Windows.Forms.Button();
            this.ImageVisual_Button = new System.Windows.Forms.Button();
            this.button_name = new System.Windows.Forms.Button();
            this.output_button = new System.Windows.Forms.Button();
            this.tile_button = new System.Windows.Forms.Button();
            this.photo_button = new System.Windows.Forms.Button();
            this.extile_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.torch_TB = new System.Windows.Forms.Label();
            this.torch_path = new System.Windows.Forms.TextBox();
            this.torch_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tile_path
            // 
            this.tile_path.Location = new System.Drawing.Point(12, 237);
            this.tile_path.MaxLength = 1000;
            this.tile_path.Name = "tile_path";
            this.tile_path.Size = new System.Drawing.Size(277, 20);
            this.tile_path.TabIndex = 5;
            this.tile_path.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.tile_path.Leave += new System.EventHandler(this.tile_path_TextBoxUI);
            // 
            // photo_path
            // 
            this.photo_path.Location = new System.Drawing.Point(12, 276);
            this.photo_path.MaxLength = 1000;
            this.photo_path.Name = "photo_path";
            this.photo_path.Size = new System.Drawing.Size(277, 20);
            this.photo_path.TabIndex = 6;
            this.photo_path.TextChanged += new System.EventHandler(this.photo_path_TextChanged);
            this.photo_path.Leave += new System.EventHandler(this.photo_path_TextBoxUI);
            // 
            // extile_path
            // 
            this.extile_path.Location = new System.Drawing.Point(12, 315);
            this.extile_path.MaxLength = 1000;
            this.extile_path.Name = "extile_path";
            this.extile_path.Size = new System.Drawing.Size(277, 20);
            this.extile_path.TabIndex = 7;
            this.extile_path.TextChanged += new System.EventHandler(this.extile_path_TextChanged);
            this.extile_path.Leave += new System.EventHandler(this.extile_path_TextBoxUI);
            // 
            // OutputPath
            // 
            this.OutputPath.Location = new System.Drawing.Point(12, 198);
            this.OutputPath.MaxLength = 1000;
            this.OutputPath.Name = "OutputPath";
            this.OutputPath.Size = new System.Drawing.Size(277, 20);
            this.OutputPath.TabIndex = 15;
            this.OutputPath.Leave += new System.EventHandler(this.Output_path_TextBoxUI);
            // 
            // ArtName
            // 
            this.ArtName.BackColor = System.Drawing.Color.White;
            this.ArtName.Location = new System.Drawing.Point(12, 159);
            this.ArtName.MaxLength = 1000;
            this.ArtName.Name = "ArtName";
            this.ArtName.Size = new System.Drawing.Size(277, 20);
            this.ArtName.TabIndex = 17;
            this.ArtName.Leave += new System.EventHandler(this.Art_name_TextBoxUI);
            // 
            // progressbar_label
            // 
            this.progressbar_label.AutoSize = true;
            this.progressbar_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressbar_label.Location = new System.Drawing.Point(385, 7);
            this.progressbar_label.Name = "progressbar_label";
            this.progressbar_label.Size = new System.Drawing.Size(0, 24);
            this.progressbar_label.TabIndex = 9;
            // 
            // Tiles_TB
            // 
            this.Tiles_TB.AutoSize = true;
            this.Tiles_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Tiles_TB.Location = new System.Drawing.Point(12, 221);
            this.Tiles_TB.Name = "Tiles_TB";
            this.Tiles_TB.Size = new System.Drawing.Size(127, 13);
            this.Tiles_TB.TabIndex = 12;
            this.Tiles_TB.Text = "Введите путь до тайлов";
            this.Tiles_TB.Click += new System.EventHandler(this.label4_Click);
            // 
            // photo_TB
            // 
            this.photo_TB.AutoSize = true;
            this.photo_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.photo_TB.Location = new System.Drawing.Point(12, 260);
            this.photo_TB.Name = "photo_TB";
            this.photo_TB.Size = new System.Drawing.Size(154, 13);
            this.photo_TB.TabIndex = 13;
            this.photo_TB.Text = "Введите путь до фотографии";
            this.photo_TB.Click += new System.EventHandler(this.photo_TB_Click);
            // 
            // extile_TB
            // 
            this.extile_TB.AutoSize = true;
            this.extile_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.extile_TB.Location = new System.Drawing.Point(12, 299);
            this.extile_TB.Name = "extile_TB";
            this.extile_TB.Size = new System.Drawing.Size(236, 13);
            this.extile_TB.TabIndex = 14;
            this.extile_TB.Text = "Введите путь до обработанного ранее файла";
            this.extile_TB.Click += new System.EventHandler(this.extile_TB_Click);
            // 
            // Output_Directory_TB
            // 
            this.Output_Directory_TB.AutoSize = true;
            this.Output_Directory_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Output_Directory_TB.Location = new System.Drawing.Point(12, 182);
            this.Output_Directory_TB.Name = "Output_Directory_TB";
            this.Output_Directory_TB.Size = new System.Drawing.Size(206, 13);
            this.Output_Directory_TB.TabIndex = 16;
            this.Output_Directory_TB.Text = "Введите путь до  выходной директории";
            this.Output_Directory_TB.Click += new System.EventHandler(this.label2_Click);
            // 
            // ArtName_TB
            // 
            this.ArtName_TB.AutoSize = true;
            this.ArtName_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ArtName_TB.Location = new System.Drawing.Point(12, 143);
            this.ArtName_TB.Name = "ArtName_TB";
            this.ArtName_TB.Size = new System.Drawing.Size(168, 13);
            this.ArtName_TB.TabIndex = 18;
            this.ArtName_TB.Text = "Желаемое название искусства";
            // 
            // ImageGen_Button
            // 
            this.ImageGen_Button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ImageGen_Button.Location = new System.Drawing.Point(385, 380);
            this.ImageGen_Button.Name = "ImageGen_Button";
            this.ImageGen_Button.Size = new System.Drawing.Size(138, 58);
            this.ImageGen_Button.TabIndex = 0;
            this.ImageGen_Button.Text = "Генерировать\r\nизображение";
            this.ImageGen_Button.UseVisualStyleBackColor = true;
            this.ImageGen_Button.Click += new System.EventHandler(this.ImageGen_Click);
            // 
            // ImageVisual_Button
            // 
            this.ImageVisual_Button.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ImageVisual_Button.Location = new System.Drawing.Point(588, 380);
            this.ImageVisual_Button.Name = "ImageVisual_Button";
            this.ImageVisual_Button.Size = new System.Drawing.Size(138, 58);
            this.ImageVisual_Button.TabIndex = 3;
            this.ImageVisual_Button.Text = "Визуализировать\r\nизображение";
            this.ImageVisual_Button.UseVisualStyleBackColor = true;
            this.ImageVisual_Button.Click += new System.EventHandler(this.ImageVisualize_Click);
            // 
            // button_name
            // 
            this.button_name.Location = new System.Drawing.Point(289, 159);
            this.button_name.Name = "button_name";
            this.button_name.Size = new System.Drawing.Size(62, 20);
            this.button_name.TabIndex = 19;
            this.button_name.Text = "OK";
            this.button_name.UseVisualStyleBackColor = true;
            this.button_name.Click += new System.EventHandler(this.ButtonName_Click);
            // 
            // output_button
            // 
            this.output_button.Location = new System.Drawing.Point(289, 198);
            this.output_button.Name = "output_button";
            this.output_button.Size = new System.Drawing.Size(62, 20);
            this.output_button.TabIndex = 20;
            this.output_button.Text = "Browse";
            this.output_button.UseVisualStyleBackColor = true;
            this.output_button.Click += new System.EventHandler(this.OutputDirectoryButton_Click);
            // 
            // tile_button
            // 
            this.tile_button.Location = new System.Drawing.Point(289, 237);
            this.tile_button.Name = "tile_button";
            this.tile_button.Size = new System.Drawing.Size(62, 20);
            this.tile_button.TabIndex = 21;
            this.tile_button.Text = "Browse";
            this.tile_button.UseVisualStyleBackColor = true;
            this.tile_button.Click += new System.EventHandler(this.tile_button_Click);
            // 
            // photo_button
            // 
            this.photo_button.Location = new System.Drawing.Point(289, 275);
            this.photo_button.Name = "photo_button";
            this.photo_button.Size = new System.Drawing.Size(62, 20);
            this.photo_button.TabIndex = 22;
            this.photo_button.Text = "Browse";
            this.photo_button.UseVisualStyleBackColor = true;
            this.photo_button.Click += new System.EventHandler(this.photo_button_Click);
            // 
            // extile_button
            // 
            this.extile_button.Location = new System.Drawing.Point(289, 315);
            this.extile_button.Name = "extile_button";
            this.extile_button.Size = new System.Drawing.Size(62, 20);
            this.extile_button.TabIndex = 23;
            this.extile_button.Text = "Browse";
            this.extile_button.UseVisualStyleBackColor = true;
            this.extile_button.Click += new System.EventHandler(this.extile_button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(385, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(340, 26);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(385, 67);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(341, 307);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // torch_TB
            // 
            this.torch_TB.AutoSize = true;
            this.torch_TB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.torch_TB.Location = new System.Drawing.Point(12, 338);
            this.torch_TB.Name = "torch_TB";
            this.torch_TB.Size = new System.Drawing.Size(188, 13);
            this.torch_TB.TabIndex = 25;
            this.torch_TB.Text = "Введите путь до файла с факелами";
            // 
            // torch_path
            // 
            this.torch_path.Location = new System.Drawing.Point(12, 354);
            this.torch_path.MaxLength = 1000;
            this.torch_path.Name = "torch_path";
            this.torch_path.Size = new System.Drawing.Size(277, 20);
            this.torch_path.TabIndex = 24;
            this.torch_path.Leave += new System.EventHandler(this.torch_path_TextBoxUI);
            // 
            // torch_button
            // 
            this.torch_button.Location = new System.Drawing.Point(289, 354);
            this.torch_button.Name = "torch_button";
            this.torch_button.Size = new System.Drawing.Size(62, 20);
            this.torch_button.TabIndex = 26;
            this.torch_button.Text = "Browse";
            this.torch_button.UseVisualStyleBackColor = true;
            this.torch_button.Click += new System.EventHandler(this.torch_button_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(747, 450);
            this.Controls.Add(this.torch_button);
            this.Controls.Add(this.torch_TB);
            this.Controls.Add(this.torch_path);
            this.Controls.Add(this.extile_button);
            this.Controls.Add(this.photo_button);
            this.Controls.Add(this.tile_button);
            this.Controls.Add(this.output_button);
            this.Controls.Add(this.button_name);
            this.Controls.Add(this.ArtName_TB);
            this.Controls.Add(this.ArtName);
            this.Controls.Add(this.Output_Directory_TB);
            this.Controls.Add(this.OutputPath);
            this.Controls.Add(this.extile_TB);
            this.Controls.Add(this.photo_TB);
            this.Controls.Add(this.Tiles_TB);
            this.Controls.Add(this.progressbar_label);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.extile_path);
            this.Controls.Add(this.photo_path);
            this.Controls.Add(this.tile_path);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ImageVisual_Button);
            this.Controls.Add(this.ImageGen_Button);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ImageGen_Button;
        private System.Windows.Forms.Button ImageVisual_Button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tile_path;
        private System.Windows.Forms.TextBox photo_path;
        private System.Windows.Forms.TextBox extile_path;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label progressbar_label;
        private System.Windows.Forms.Label Tiles_TB;
        private System.Windows.Forms.Label photo_TB;
        private System.Windows.Forms.Label extile_TB;
        private System.Windows.Forms.Label Output_Directory_TB;
        private System.Windows.Forms.TextBox OutputPath;
        private System.Windows.Forms.Label ArtName_TB;
        private System.Windows.Forms.TextBox ArtName;
        private System.Windows.Forms.Button button_name;
        private System.Windows.Forms.Button output_button;
        private System.Windows.Forms.Button tile_button;
        private System.Windows.Forms.Button photo_button;
        private System.Windows.Forms.Button extile_button;
        private System.Windows.Forms.Label torch_TB;
        private System.Windows.Forms.TextBox torch_path;
        private System.Windows.Forms.Button torch_button;
    }
}