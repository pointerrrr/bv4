// Beeldverwerking assignment 2
// Mike Knoop (5853915)
// Gideon Ogilvie (5936373)
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
            this.applyButton = new System.Windows.Forms.Button();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.comboBoxFunctionality = new System.Windows.Forms.ComboBox();
            this.buttonSetAsImage = new System.Windows.Forms.Button();
            this.numericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.numericUpDownDSize = new System.Windows.Forms.NumericUpDown();
            this.labelDSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.LoadImage2Button = new System.Windows.Forms.Button();
            this.imageFileName2 = new System.Windows.Forms.TextBox();
            this.radioButtonSquare = new System.Windows.Forms.RadioButton();
            this.radioButtonPlus = new System.Windows.Forms.RadioButton();
            this.checkBoxGray = new System.Windows.Forms.CheckBox();
            this.LineGap = new System.Windows.Forms.NumericUpDown();
            this.LineLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.intensityThreshold = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.minAng = new System.Windows.Forms.NumericUpDown();
            this.maxAng = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.angSteps = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxCrossings = new System.Windows.Forms.CheckBox();
            this.numericUpDownMinCircleR = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaxCircleR = new System.Windows.Forms.NumericUpDown();
            this.labelMinCircleR = new System.Windows.Forms.Label();
            this.labelMaxCircleR = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intensityThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAng)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAng)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinCircleR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxCircleR)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(13, 11);
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
            this.imageFileName.Location = new System.Drawing.Point(117, 13);
            this.imageFileName.Name = "imageFileName";
            this.imageFileName.ReadOnly = true;
            this.imageFileName.Size = new System.Drawing.Size(225, 20);
            this.imageFileName.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 123);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(467, 11);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(103, 23);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "Bitmap file (*.bmp)|*.bmp";
            this.saveImageDialog.InitialDirectory = "..\\..\\images";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(948, 11);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(95, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save as BMP...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(531, 123);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(512, 512);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(576, 13);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(276, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // comboBoxFunctionality
            // 
            this.comboBoxFunctionality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFunctionality.FormattingEnabled = true;
            this.comboBoxFunctionality.Items.AddRange(new object[] {
            "Invert",
            "Erosion",
            "Dilation",
            "Opening",
            "Closing",
            "Complement",
            "And",
            "Or",
            "Value Counting",
            "Boundary Trace",
            "Threshold",
            "Hough transformation",
            "Hough Threshold",
            "Line Detetection",
            "Circle Detection"});
            this.comboBoxFunctionality.Location = new System.Drawing.Point(348, 12);
            this.comboBoxFunctionality.Name = "comboBoxFunctionality";
            this.comboBoxFunctionality.Size = new System.Drawing.Size(113, 21);
            this.comboBoxFunctionality.TabIndex = 7;
            // 
            // buttonSetAsImage
            // 
            this.buttonSetAsImage.Location = new System.Drawing.Point(858, 11);
            this.buttonSetAsImage.Name = "buttonSetAsImage";
            this.buttonSetAsImage.Size = new System.Drawing.Size(84, 23);
            this.buttonSetAsImage.TabIndex = 8;
            this.buttonSetAsImage.Text = "Set as image";
            this.buttonSetAsImage.UseVisualStyleBackColor = true;
            this.buttonSetAsImage.Click += new System.EventHandler(this.ButtonSetAsImage_Click);
            // 
            // numericUpDownThreshold
            // 
            this.numericUpDownThreshold.Location = new System.Drawing.Point(974, 41);
            this.numericUpDownThreshold.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownThreshold.TabIndex = 9;
            this.numericUpDownThreshold.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(837, 44);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(82, 13);
            this.labelThreshold.TabIndex = 10;
            this.labelThreshold.Text = "Group threshold";
            // 
            // numericUpDownDSize
            // 
            this.numericUpDownDSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownDSize.Location = new System.Drawing.Point(647, 42);
            this.numericUpDownDSize.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.numericUpDownDSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDSize.Name = "numericUpDownDSize";
            this.numericUpDownDSize.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownDSize.TabIndex = 11;
            this.numericUpDownDSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelDSize
            // 
            this.labelDSize.AutoSize = true;
            this.labelDSize.Location = new System.Drawing.Point(566, 44);
            this.labelDSize.Name = "labelDSize";
            this.labelDSize.Size = new System.Drawing.Size(80, 13);
            this.labelDSize.TabIndex = 12;
            this.labelDSize.Text = "Dimension size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(727, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Threads:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1",
            "4",
            "16",
            "64"});
            this.comboBox1.Location = new System.Drawing.Point(782, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(49, 21);
            this.comboBox1.TabIndex = 19;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(13, 609);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(512, 512);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 20;
            this.pictureBox3.TabStop = false;
            // 
            // LoadImage2Button
            // 
            this.LoadImage2Button.Location = new System.Drawing.Point(13, 42);
            this.LoadImage2Button.Name = "LoadImage2Button";
            this.LoadImage2Button.Size = new System.Drawing.Size(98, 23);
            this.LoadImage2Button.TabIndex = 21;
            this.LoadImage2Button.Text = "Load image...";
            this.LoadImage2Button.UseVisualStyleBackColor = true;
            this.LoadImage2Button.Click += new System.EventHandler(this.LoadImage2Button_Click);
            // 
            // imageFileName2
            // 
            this.imageFileName2.Location = new System.Drawing.Point(117, 44);
            this.imageFileName2.Name = "imageFileName2";
            this.imageFileName2.ReadOnly = true;
            this.imageFileName2.Size = new System.Drawing.Size(225, 20);
            this.imageFileName2.TabIndex = 22;
            // 
            // radioButtonSquare
            // 
            this.radioButtonSquare.AutoSize = true;
            this.radioButtonSquare.Checked = true;
            this.radioButtonSquare.Location = new System.Drawing.Point(348, 41);
            this.radioButtonSquare.Name = "radioButtonSquare";
            this.radioButtonSquare.Size = new System.Drawing.Size(59, 17);
            this.radioButtonSquare.TabIndex = 23;
            this.radioButtonSquare.TabStop = true;
            this.radioButtonSquare.Text = "Square";
            this.radioButtonSquare.UseVisualStyleBackColor = true;
            // 
            // radioButtonPlus
            // 
            this.radioButtonPlus.AutoSize = true;
            this.radioButtonPlus.Location = new System.Drawing.Point(413, 41);
            this.radioButtonPlus.Name = "radioButtonPlus";
            this.radioButtonPlus.Size = new System.Drawing.Size(45, 17);
            this.radioButtonPlus.TabIndex = 24;
            this.radioButtonPlus.Text = "Plus";
            this.radioButtonPlus.UseVisualStyleBackColor = true;
            // 
            // checkBoxGray
            // 
            this.checkBoxGray.AutoSize = true;
            this.checkBoxGray.Checked = true;
            this.checkBoxGray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGray.Enabled = false;
            this.checkBoxGray.Location = new System.Drawing.Point(457, 43);
            this.checkBoxGray.Name = "checkBoxGray";
            this.checkBoxGray.Size = new System.Drawing.Size(113, 17);
            this.checkBoxGray.TabIndex = 25;
            this.checkBoxGray.Text = "GrayScale Image?";
            this.checkBoxGray.UseVisualStyleBackColor = true;
            // 
            // LineGap
            // 
            this.LineGap.Location = new System.Drawing.Point(974, 65);
            this.LineGap.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.LineGap.Name = "LineGap";
            this.LineGap.Size = new System.Drawing.Size(78, 20);
            this.LineGap.TabIndex = 26;
            this.LineGap.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // LineLength
            // 
            this.LineLength.Location = new System.Drawing.Point(817, 66);
            this.LineLength.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.LineLength.Name = "LineLength";
            this.LineLength.Size = new System.Drawing.Size(78, 20);
            this.LineLength.TabIndex = 27;
            this.LineLength.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(901, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Max line gap";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(696, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Minimum Length of line";
            // 
            // intensityThreshold
            // 
            this.intensityThreshold.Location = new System.Drawing.Point(612, 65);
            this.intensityThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.intensityThreshold.Name = "intensityThreshold";
            this.intensityThreshold.Size = new System.Drawing.Size(78, 20);
            this.intensityThreshold.TabIndex = 30;
            this.intensityThreshold.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(491, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Grayscale threshold";
            // 
            // minAng
            // 
            this.minAng.Location = new System.Drawing.Point(407, 64);
            this.minAng.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minAng.Name = "minAng";
            this.minAng.Size = new System.Drawing.Size(78, 20);
            this.minAng.TabIndex = 32;
            // 
            // maxAng
            // 
            this.maxAng.Location = new System.Drawing.Point(252, 64);
            this.maxAng.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.maxAng.Name = "maxAng";
            this.maxAng.Size = new System.Drawing.Size(78, 20);
            this.maxAng.TabIndex = 33;
            this.maxAng.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(345, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Min Ang";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Max Ang";
            // 
            // angSteps
            // 
            this.angSteps.Location = new System.Drawing.Point(113, 65);
            this.angSteps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.angSteps.Name = "angSteps";
            this.angSteps.Size = new System.Drawing.Size(78, 20);
            this.angSteps.TabIndex = 36;
            this.angSteps.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Ang steps";
            // 
            // checkBoxCrossings
            // 
            this.checkBoxCrossings.AutoSize = true;
            this.checkBoxCrossings.Location = new System.Drawing.Point(13, 92);
            this.checkBoxCrossings.Name = "checkBoxCrossings";
            this.checkBoxCrossings.Size = new System.Drawing.Size(112, 17);
            this.checkBoxCrossings.TabIndex = 38;
            this.checkBoxCrossings.Text = "Detect Crossings?";
            this.checkBoxCrossings.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMinCircleR
            // 
            this.numericUpDownMinCircleR.Location = new System.Drawing.Point(612, 92);
            this.numericUpDownMinCircleR.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMinCircleR.Name = "numericUpDownMinCircleR";
            this.numericUpDownMinCircleR.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownMinCircleR.TabIndex = 39;
            this.numericUpDownMinCircleR.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownMaxCircleR
            // 
            this.numericUpDownMaxCircleR.Location = new System.Drawing.Point(817, 92);
            this.numericUpDownMaxCircleR.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMaxCircleR.Name = "numericUpDownMaxCircleR";
            this.numericUpDownMaxCircleR.Size = new System.Drawing.Size(78, 20);
            this.numericUpDownMaxCircleR.TabIndex = 40;
            this.numericUpDownMaxCircleR.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // labelMinCircleR
            // 
            this.labelMinCircleR.AutoSize = true;
            this.labelMinCircleR.Location = new System.Drawing.Point(499, 96);
            this.labelMinCircleR.Name = "labelMinCircleR";
            this.labelMinCircleR.Size = new System.Drawing.Size(107, 13);
            this.labelMinCircleR.TabIndex = 41;
            this.labelMinCircleR.Text = "Minimum circle radius";
            // 
            // labelMaxCircleR
            // 
            this.labelMaxCircleR.AutoSize = true;
            this.labelMaxCircleR.Location = new System.Drawing.Point(704, 96);
            this.labelMaxCircleR.Name = "labelMaxCircleR";
            this.labelMaxCircleR.Size = new System.Drawing.Size(110, 13);
            this.labelMaxCircleR.TabIndex = 42;
            this.labelMaxCircleR.Text = "Maximum circle radius";
            // 
            // INFOIBV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 857);
            this.Controls.Add(this.labelMaxCircleR);
            this.Controls.Add(this.labelMinCircleR);
            this.Controls.Add(this.numericUpDownMaxCircleR);
            this.Controls.Add(this.numericUpDownMinCircleR);
            this.Controls.Add(this.checkBoxCrossings);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.angSteps);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.maxAng);
            this.Controls.Add(this.minAng);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.intensityThreshold);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LineLength);
            this.Controls.Add(this.LineGap);
            this.Controls.Add(this.checkBoxGray);
            this.Controls.Add(this.radioButtonPlus);
            this.Controls.Add(this.radioButtonSquare);
            this.Controls.Add(this.imageFileName2);
            this.Controls.Add(this.LoadImage2Button);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelDSize);
            this.Controls.Add(this.numericUpDownDSize);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.numericUpDownThreshold);
            this.Controls.Add(this.buttonSetAsImage);
            this.Controls.Add(this.comboBoxFunctionality);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.applyButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intensityThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAng)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAng)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinCircleR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxCircleR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.TextBox imageFileName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox comboBoxFunctionality;
        private System.Windows.Forms.Button buttonSetAsImage;
        private System.Windows.Forms.NumericUpDown numericUpDownThreshold;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.NumericUpDown numericUpDownDSize;
        private System.Windows.Forms.Label labelDSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button LoadImage2Button;
        private System.Windows.Forms.TextBox imageFileName2;
        private System.Windows.Forms.RadioButton radioButtonSquare;
        private System.Windows.Forms.RadioButton radioButtonPlus;
        private System.Windows.Forms.CheckBox checkBoxGray;
        private System.Windows.Forms.NumericUpDown LineGap;
        private System.Windows.Forms.NumericUpDown LineLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown intensityThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown minAng;
        private System.Windows.Forms.NumericUpDown maxAng;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown angSteps;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxCrossings;
        private System.Windows.Forms.NumericUpDown numericUpDownMinCircleR;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxCircleR;
        private System.Windows.Forms.Label labelMinCircleR;
        private System.Windows.Forms.Label labelMaxCircleR;
    }
}

