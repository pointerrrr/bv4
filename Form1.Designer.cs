namespace INFOIBV
{
    partial class INFOIBV
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
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageFileName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveButton = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.comboBoxTask = new System.Windows.Forms.ComboBox();
            this.numericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.numericUpDownEdgeThreshold = new System.Windows.Forms.NumericUpDown();
            this.buttonSetAsImage = new System.Windows.Forms.Button();
            this.labelEdgeThreshold = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownThresholdEdgeDetection = new System.Windows.Forms.NumericUpDown();
            this.labelEdge = new System.Windows.Forms.Label();
            this.Neighbour = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEdgeThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdEdgeDetection)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(12, 10);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(98, 23);
            this.LoadImageButton.TabIndex = 0;
            this.LoadImageButton.Text = "Load image...";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // openImageDialog
            // 
            this.openImageDialog.Filter = "Bitmap files (*.bmp;*.gif;*.jpg;*.png;*.tiff;*.jpeg)|*.bmp;*.gif;*.jpg;*.png;*.ti" +
    "ff;*.jpeg";
            this.openImageDialog.InitialDirectory = "..\\..\\images";
            // 
            // imageFileName
            // 
            this.imageFileName.Location = new System.Drawing.Point(116, 12);
            this.imageFileName.Name = "imageFileName";
            this.imageFileName.ReadOnly = true;
            this.imageFileName.Size = new System.Drawing.Size(240, 20);
            this.imageFileName.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(489, 11);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(74, 23);
            this.ApplyButton.TabIndex = 3;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "Bitmap file (*.bmp)|*.bmp";
            this.saveImageDialog.InitialDirectory = "..\\..\\images";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(948, 11);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(95, 23);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save as BMP...";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(530, 76);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(512, 512);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(569, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(276, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // comboBoxTask
            // 
            this.comboBoxTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTask.FormattingEnabled = true;
            this.comboBoxTask.Items.AddRange(new object[] {
            "Threshold",
            "Contrast Boost",
            "Edge Detection",
            "Opening",
            "Closing",
            "Line Detection",
            "Region Detection",
            "Object Detection",
            "Edge sharpening",
            "Invert"});
            this.comboBoxTask.Location = new System.Drawing.Point(362, 12);
            this.comboBoxTask.Name = "comboBoxTask";
            this.comboBoxTask.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTask.TabIndex = 7;
            // 
            // numericUpDownThreshold
            // 
            this.numericUpDownThreshold.Location = new System.Drawing.Point(948, 41);
            this.numericUpDownThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownThreshold.TabIndex = 8;
            this.numericUpDownThreshold.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(855, 43);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(86, 13);
            this.labelThreshold.TabIndex = 9;
            this.labelThreshold.Text = "Image Threshold";
            // 
            // numericUpDownEdgeThreshold
            // 
            this.numericUpDownEdgeThreshold.Location = new System.Drawing.Point(729, 40);
            this.numericUpDownEdgeThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownEdgeThreshold.Name = "numericUpDownEdgeThreshold";
            this.numericUpDownEdgeThreshold.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownEdgeThreshold.TabIndex = 10;
            this.numericUpDownEdgeThreshold.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonSetAsImage
            // 
            this.buttonSetAsImage.Location = new System.Drawing.Point(851, 11);
            this.buttonSetAsImage.Name = "buttonSetAsImage";
            this.buttonSetAsImage.Size = new System.Drawing.Size(91, 23);
            this.buttonSetAsImage.TabIndex = 11;
            this.buttonSetAsImage.Text = "Set as Image";
            this.buttonSetAsImage.UseVisualStyleBackColor = true;
            this.buttonSetAsImage.Click += new System.EventHandler(this.ButtonSetAsImage_Click);
            // 
            // labelEdgeThreshold
            // 
            this.labelEdgeThreshold.AutoSize = true;
            this.labelEdgeThreshold.Location = new System.Drawing.Point(641, 43);
            this.labelEdgeThreshold.Name = "labelEdgeThreshold";
            this.labelEdgeThreshold.Size = new System.Drawing.Size(82, 13);
            this.labelEdgeThreshold.TabIndex = 12;
            this.labelEdgeThreshold.Text = "Edge Threshold";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Location = new System.Drawing.Point(530, 623);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(512, 512);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox5.TabIndex = 14;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(12, 623);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(512, 512);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox4.TabIndex = 13;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(1063, 76);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(512, 512);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 15;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Location = new System.Drawing.Point(1066, 623);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(512, 512);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox6.TabIndex = 16;
            this.pictureBox6.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Input Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(536, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 607);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(536, 607);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1060, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1060, 607);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 22;
            // 
            // numericUpDownThresholdEdgeDetection
            // 
            this.numericUpDownThresholdEdgeDetection.Location = new System.Drawing.Point(515, 41);
            this.numericUpDownThresholdEdgeDetection.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownThresholdEdgeDetection.Name = "numericUpDownThresholdEdgeDetection";
            this.numericUpDownThresholdEdgeDetection.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownThresholdEdgeDetection.TabIndex = 23;
            this.numericUpDownThresholdEdgeDetection.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // labelEdge
            // 
            this.labelEdge.AutoSize = true;
            this.labelEdge.Location = new System.Drawing.Point(369, 43);
            this.labelEdge.Name = "labelEdge";
            this.labelEdge.Size = new System.Drawing.Size(131, 13);
            this.labelEdge.TabIndex = 24;
            this.labelEdge.Text = "Edge Detection Threshold";
            // 
            // Neighbour
            // 
            this.Neighbour.AutoSize = true;
            this.Neighbour.Location = new System.Drawing.Point(234, 44);
            this.Neighbour.Name = "Neighbour";
            this.Neighbour.Size = new System.Drawing.Size(108, 17);
            this.Neighbour.TabIndex = 25;
            this.Neighbour.Text = "4 Neighbourhood";
            this.Neighbour.UseVisualStyleBackColor = true;
            // 
            // INFOIBV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1940, 1061);
            this.Controls.Add(this.Neighbour);
            this.Controls.Add(this.labelEdge);
            this.Controls.Add(this.numericUpDownThresholdEdgeDetection);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.labelEdgeThreshold);
            this.Controls.Add(this.buttonSetAsImage);
            this.Controls.Add(this.numericUpDownEdgeThreshold);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.numericUpDownThreshold);
            this.Controls.Add(this.comboBoxTask);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.imageFileName);
            this.Controls.Add(this.LoadImageButton);
            this.Location = new System.Drawing.Point(10, 10);
            this.Name = "INFOIBV";
            this.ShowIcon = false;
            this.Text = "INFOIBV";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEdgeThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdEdgeDetection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.TextBox imageFileName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox comboBoxTask;
        private System.Windows.Forms.NumericUpDown numericUpDownThreshold;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.NumericUpDown numericUpDownEdgeThreshold;
        private System.Windows.Forms.Button buttonSetAsImage;
        private System.Windows.Forms.Label labelEdgeThreshold;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownThresholdEdgeDetection;
        private System.Windows.Forms.Label labelEdge;
        private System.Windows.Forms.CheckBox Neighbour;
    }
}

