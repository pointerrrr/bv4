// Beeldverwerking assignment 2
// Mike Knoop (5853915)
// Gideon Ogilvie (5936373)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace INFOIBV
{
    public partial class INFOIBV : Form
    {
        private Bitmap InputBitmap;
        private Bitmap InputBitmap2;
        private Bitmap OutputImage;
        private Color[,] InputImage;
        private Color[,] InputImage2;

        Color[,] Image;
        Color[,] Image2;

        int threads = 1;

        int kernelDimensionSize;

        Color[,] tempImage;

        int counter = -1;

        public INFOIBV()
        {
            InitializeComponent();
            comboBoxFunctionality.SelectedIndex = 11;
            comboBox1.SelectedIndex = 2;
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)             // Open File Dialog
            {
                string file = openImageDialog.FileName;                     // Get the file name
                imageFileName.Text = file;                                  // Show file name
                if (InputBitmap != null) InputBitmap.Dispose();               // Reset image
                InputBitmap = new Bitmap(file);                              // Create new Bitmap from file

                pictureBox1.Image = (Image)InputBitmap;                 // Display input image
                Image = new Color[InputBitmap.Size.Width, InputBitmap.Size.Height]; // Create array to speed-up operations (Bitmap functions are very slow)
                InputImage = new Color[InputBitmap.Size.Width, InputBitmap.Size.Height];
                // Copy input Bitmap to array   
                // Setup progress bar
                progressBar.Visible = true;
                progressBar.Minimum = 1;
                progressBar.Maximum = InputBitmap.Size.Width * InputBitmap.Size.Height;
                progressBar.Value = 1;
                progressBar.Step = 1;
                for (int x = 0; x < InputBitmap.Size.Width; x++)
                {
                    for (int y = 0; y < InputBitmap.Size.Height; y++)
                    {
                        Image[x, y] = InputBitmap.GetPixel(x, y);                // Set pixel color in array at (x,y)
                        InputImage[x, y] = InputBitmap.GetPixel(x, y);
                        progressBar.PerformStep();
                    }
                }
                progressBar.Visible = false;
                checkBoxGray.Checked = checkGrayscale(Image);
            }
        }

        private void LoadImage2Button_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)             // Open File Dialog
            {
                string file = openImageDialog.FileName;                     // Get the file name
                imageFileName2.Text = file;                                  // Show file name
                if (InputBitmap2 != null) InputBitmap2.Dispose();               // Reset image
                InputBitmap2 = new Bitmap(file);                              // Create new Bitmap from file
                //if (InputBitmap.Size.Height <= 0 || InputBitmap.Size.Width <= 0 ||
                //    InputBitmap.Size.Height > 512 || InputBitmap.Size.Width > 512) // Dimension check
                //    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                //else
                pictureBox2.Image = (Image)InputBitmap2;                 // Display input image
                Image2 = new Color[InputBitmap2.Size.Width, InputBitmap2.Size.Height]; // Create array to speed-up operations (Bitmap functions are very slow)
                InputImage2 = new Color[InputBitmap2.Size.Width, InputBitmap2.Size.Height];
                // Copy input Bitmap to array   
                // Setup progress bar
                progressBar.Visible = true;
                progressBar.Minimum = 1;
                progressBar.Maximum = InputBitmap2.Size.Width * InputBitmap2.Size.Height;
                progressBar.Value = 1;
                progressBar.Step = 1;
                for (int x = 0; x < InputBitmap2.Size.Width; x++)
                {
                    for (int y = 0; y < InputBitmap2.Size.Height; y++)
                    {
                        Image2[x, y] = InputBitmap2.GetPixel(x, y);                // Set pixel color in array at (x,y)
                        InputImage2[x, y] = InputBitmap2.GetPixel(x, y);
                        progressBar.PerformStep();
                    }
                }
                progressBar.Visible = false;
            }
        }

        private bool checkGrayscale(Color[,] Image)
        {
            bool binary = true;
            for (int x = 0; x < Image.GetLength(0); x++)
            {
                for (int y = 0; y < Image.GetLength(1); y++)
                {
                    Color pixel = Image[x, y];
                    //not grayscale
                    if (pixel.R != pixel.G || pixel.R != pixel.B)
                        return false;
                    //check for binary if not black or white
                    else if ((pixel.R != 0 || pixel.G != 0 || pixel.B != 0) && (pixel.R != 255 || pixel.G != 255 || pixel.B != 255))
                        binary = false;
                }
            }

            return !binary;
        }

        private bool checkBinary(Color[,] Image)
        {
            for (int x = 0; x < Image.GetLength(0); x++)
            {
                for (int y = 0; y < Image.GetLength(1); y++)
                {
                    Color pixel = Image[x, y];
                    //check for binary if not black or white
                    if (!((pixel.R == 0 && pixel.G == 0 && pixel.B == 0) || (pixel.R == 255 && pixel.G == 255 && pixel.B == 255)))
                        return false;
                }
            }
            return true;

        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            Image = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            if (InputBitmap == null) return;                                 // Get out if no input image
            if (OutputImage != null) OutputImage.Dispose();                 // Reset output image


            // Setup progress bar
            progressBar.Visible = true;
            progressBar.Minimum = 1;
            progressBar.Maximum = InputBitmap.Size.Width * InputBitmap.Size.Height;
            progressBar.Value = 1;
            progressBar.Step = 1;


            kernelDimensionSize = (int)numericUpDownDSize.Value;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    threads = 1;
                    break;
                case 1:
                    threads = 4;
                    break;
                case 2:
                    threads = 16;
                    break;
                case 3:
                    threads = 64;
                    break;
            }
            Thread[] t = new Thread[threads];
            switch (comboBoxFunctionality.SelectedIndex)
            {
                case 0:
                    // Invert colors
                    for (int i = 0; i < threads; i++)
                    {
                        t[i] = new Thread(startInvertThreads);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Start(i);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Join();
                    }

                    break;
                case 1:
                    if (checkBoxGray.Checked || checkBinary(InputImage))
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startErosion);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                    }
                    else
                        MessageBox.Show("input image must be a binary or grayscale image");
                    // Erosion
                    break;
                case 2:
                    // Dilation
                    if (checkBoxGray.Checked || checkBinary(InputImage))
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startDilation);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                    }
                    else
                        MessageBox.Show("input image must be a binary or grayscale image");
                    break;
                case 3:
                    // Opening
                    if (checkBoxGray.Checked || checkBinary(InputImage))
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startErosion);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                        tempImage = new Color[InputBitmap.Size.Width, InputBitmap.Size.Height];


                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startDilationOpening);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                        Image = tempImage;
                    }
                    else
                        MessageBox.Show("input image must be a binary or grayscale image");
                    break;


                case 4:
                    // Closing
                    if (checkBoxGray.Checked || checkBinary(InputImage))
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startDilation);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }

                        tempImage = new Color[InputBitmap.Size.Width, InputBitmap.Size.Height];

                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startErosionClosing);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                        Image = tempImage;
                    }
                    else
                        MessageBox.Show("input image must be a binary or grayscale image");

                    break;
                case 5:
                    // Complement
                    for (int i = 0; i < threads; i++)
                    {
                        t[i] = new Thread(startComplement);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Start(i);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Join();
                    }
                    break;
                case 6:
                    // And
                    if (InputImage.GetLength(0) != InputImage2.GetLength(0) || InputImage.GetLength(1) != InputImage2.GetLength(1))
                    {
                        MessageBox.Show("Images are not the same size");
                    }
                    else if (!(checkBinary(InputImage) && checkBinary(InputImage2)))
                    {
                        MessageBox.Show("The images should both be binary");
                    }
                    else
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startAnd);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                    }
                    break;
                case 7:
                    // Or
                    if (InputImage.GetLength(0) != InputImage2.GetLength(0) || InputImage.GetLength(1) != InputImage2.GetLength(1))
                    {
                        MessageBox.Show("Images are not the same size");
                    }
                    else if (!(checkBinary(InputImage) && checkBinary(InputImage2)))
                    {
                        MessageBox.Show("The images should both be binary");
                    }
                    else
                    {
                        for (int i = 0; i < threads; i++)
                        {
                            t[i] = new Thread(startOr);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Start(i);
                        }
                        for (int i = 0; i < threads; i++)
                        {
                            t[i].Join();
                        }
                    }
                    break;
                case 8:
                    // Value counting
                    counting(InputImage);
                    break;
                case 9:
                    // Boundary trace
                    if (checkBoxGray.Checked)
                        MessageBox.Show("Input image must be a binary image");
                    else
                    {
                        int[,] mask = new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
                        TraceBoundaries(this.Image, this.InputImage, mask);
                    }
                    break;
                case 10:
                    for (int i = 0; i < threads; i++)
                    {
                        t[i] = new Thread(startThresholdingThreads);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Start(i);
                    }
                    for (int i = 0; i < threads; i++)
                    {
                        t[i].Join();
                    }
                    break;
                case 11:
                    //if(!checkBoxGray.Checked && checkBinary(InputImage))
                    //    houghTransform(InputImage);
                    //else
                    //    MessageBox.Show("Input image must be a binary image");
                    if (minAng.Value > maxAng.Value || maxAng.Value > angSteps.Value)
                    {
                        switch (counter)
                        {
                            case -1:
                                MessageBox.Show("Wrong input value for min or max angle", "Wrong input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            case 0:
                                MessageBox.Show("NO");
                                break;
                            case 1:
                                MessageBox.Show("NO!!!!");
                                break;
                            case 2:
                                MessageBox.Show("I SAID NO!!!");
                                break;
                            case 3:
                                MessageBox.Show("Why u do this?");
                                break;
                            case 4:
                                MessageBox.Show("Fuck this");
                                Environment.Exit(-1);
                                break;
                        }
                        counter++;
                    }
                    else
                    {
                        houghTransform(InputImage);
                        counter = 0;
                    }
                    break;
                case 12:
                    var accumulatorArray = houghTransform(InputImage);
                    HoughThreshold(accumulatorArray);
                    break;
                case 13:
                    accumulatorArray = houghTransform(InputImage);
                    var thresholdedArray = HoughThreshold(houghTransform(InputImage));
                    HoughLineDetection(thresholdedArray, InputImage);
                    break;
                case 14:
                    HoughCircleDetection(InputImage, Image);
                    break;
            }


            progressBar.Value = 1;
            OutputImage = new Bitmap(Image.GetLength(0), Image.GetLength(1)); // Create new output image
            // Copy array to output Bitmap
            for (int x = 0; x < Image.GetLength(0); x++)
            {
                for (int y = 0; y < Image.GetLength(1); y++)
                {
                    if (Image[x, y].R == 0 && Image[x, y].G == 0 && Image[x, y].B == 0)
                        OutputImage.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else
                        OutputImage.SetPixel(x, y, Image[x, y]);               // Set the pixel color at coordinate (x,y)
                    progressBar.PerformStep();
                }
            }

            pictureBox2.Image = (Image)OutputImage;                         // Display output image
            progressBar.Visible = false;                                    // Hide progress bar
        }

        private void HoughLineDetection(int[,] accumulatorArray, Color[,] inputImage)
        {
            int xMid = inputImage.GetLength(0) / 2;
            int yMid = inputImage.GetLength(1) / 2;
            int nAng = (int)angSteps.Value;
            double deltaAng = Math.PI / nAng;
            int nRad = 256;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));

            Image = new Color[inputImage.GetLength(0), inputImage.GetLength(1)];


            int intensityThreshold;
            if (checkBinary(InputImage))
                intensityThreshold = 0;
            else
                intensityThreshold = (int)this.intensityThreshold.Value;

            var locations = new List<Tuple<int, int>>();

            for(int x = 0; x < accumulatorArray.GetLength(0); x++)
            {
                for(int y = 0; y < accumulatorArray.GetLength(1); y++)
                {
                    if (accumulatorArray[x, y] != 0)
                        locations.Add(new Tuple<int, int>(x, y));
                }
            }

            for (int i = 0; i < locations.Count; i++)
            {
                var point = locations[i];
                double radians = (point.Item1 / (double)nRad) * Math.PI;
                double length = ((nAng / 2d) - point.Item2) / (double)nAng * rMax * 2d;

                double aanliggend = Math.Cos(radians) * length;
                double overstaand = Math.Sqrt(length * length - aanliggend * aanliggend) * (length < 0 ? -1 : 1);
                double slope1 = aanliggend / overstaand;
                double slope2 = overstaand / aanliggend;
                double b1 = (yMid - overstaand) - slope1 * aanliggend;
                double b2 = (xMid - aanliggend) - (slope2) * overstaand;
                //if (double.IsInfinity(b1))
                //    b1 = aanliggend;

                var lines = HoughLineDetection(InputImage, new Point(point.Item1, point.Item2), intensityThreshold, (int)LineLength.Value, (int)LineGap.Value);

                if (Math.Abs(slope1) >= 1d)
                {
                    for (int j = 0; j < InputImage.GetLength(1); j++)
                    {
                        int x = (int)(slope2 * (yMid - j) + b2); ;
                        if (x >= 0 && x < InputImage.GetLength(0))
                        {
                            foreach (var line in lines)
                            {
                                int startX = line.Item1.X;
                                int stopX = line.Item2.X;
                                int startY = line.Item1.Y;
                                int stopY = line.Item2.Y;
                                if (((x >= startX && x <= stopX) || (x <= startX && x >= stopX)) && j >= startY && j <= stopY)
                                    Image[x, j] = Color.Red;
                            }
                        }
                    }
                }
                else if (Math.Abs(slope1) < 1d)
                {
                    for (int j = 0; j < InputImage.GetLength(0); j++)
                    {
                        int y = (int)(slope1 * (xMid - j) + b1);
                        if (y >= 0 && y < InputImage.GetLength(1))
                        {
                            foreach (var line in lines)
                            {
                                int startX = line.Item1.X;
                                int stopX = line.Item2.X;
                                int startY = line.Item1.Y;
                                int stopY = line.Item2.Y;
                                if (j >= startX && j <= stopX && ((y >= startY && y <= stopY) || (y >= stopY && y <= startY)))
                                    Image[j, y] = Color.Red;
                            }
                        }
                    }
                }
            }

            
            if (checkBoxCrossings.Checked)
            {
                var crossings = FindCrossings(Image, locations);
                foreach (var crossing in crossings)
                    DrawCircle(Image, crossing.Item1, crossing.Item2, Color.Cyan, 5);
            }
            
            for (int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    if (Image[x, y] != Color.Red && Image[x, y] != Color.Cyan)
                        Image[x, y] = InputImage[x, y];
                }
            }
            
        }

        private int[,] HoughThreshold(int[,] accumulatorArray)
        {
            int nAng = (int)angSteps.Value;
            int nRad = 256;
            int max = 0;
            
            int[,] temp = new int[nAng, nRad];
            for (int x = (int)minAng.Value; x < (int)maxAng.Value; x++)
            {
                for (int y = 0; y < nRad; y++)
                {
                    bool smaller = false;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int xCoord = x + i - 1;
                            int yCoord = y + j - 1;

                            if (xCoord < (int)minAng.Value || xCoord >= (int)maxAng.Value || yCoord < 0 || yCoord >= nRad)
                            {
                                continue;
                            }
                            if (accumulatorArray[x, y] < accumulatorArray[xCoord, yCoord])
                                smaller = true;

                        }
                    }
                    if (smaller)
                        temp[x, y] = 0;
                    else
                    {
                        if (accumulatorArray[x, y] > max)
                            max = accumulatorArray[x, y];
                        temp[x, y] = accumulatorArray[x, y];
                    }

                }
            }

            int threshold = (int)numericUpDownThreshold.Value;

            for (int x = (int)minAng.Value; x < (int)maxAng.Value; x++)
            {
                for (int y = 0; y < nRad; y++)
                {
                    int value = (int)(temp[x, y] / (double)max * 255d);
                    if (value >= threshold)
                        Image[x, y] = Color.FromArgb(value, value, value);
                    else
                    {
                        temp[x, y] = 0;
                        Image[x, y] = Color.Black;
                    }
                }
            }
            return temp;
        }

        // adapted from: https://en.wikipedia.org/wiki/Circle_Hough_Transform
        private void HoughCircleDetection(Color[,] inputImage, Color[,] image)
        {
            int minRadius = (int)numericUpDownMinCircleR.Value;
            int maxRadius = (int)numericUpDownMaxCircleR.Value;
            int theta = 360;
            int[,,] accumulatorArray = new int[inputImage.GetLength(0),inputImage.GetLength(1),(maxRadius-minRadius)];
            int[,,] temp = new int[inputImage.GetLength(0), inputImage.GetLength(1), (maxRadius - minRadius)];
            int max = 0;
            bool binary = checkBinary(InputImage);

            for (int x = 0; x < inputImage.GetLength(0); x++)
            {
                for (int y = 0; y < inputImage.GetLength(1); y++)
                {
                    if (inputImage[x, y].R <= intensityThreshold.Value)
                    {
                        for (int r = 0; r < maxRadius - minRadius; r++)
                        {
                            for (int t = 0; t < theta; t++)
                            {
                                int a = (int)(x - r * Math.Cos(t * Math.PI / 180d));
                                int b = (int)(y - r * Math.Sin(t * Math.PI / 180d));
                                if (a >= 0 && a < inputImage.GetLength(0) && b >= 0 && b < inputImage.GetLength(1))
                                {
                                    if (binary)
                                        accumulatorArray[a, b,r]++;
                                    else
                                        accumulatorArray[a,b,r] += 255 - inputImage[x, y].R;
                                    if (accumulatorArray[a, b, r] > max)
                                        max = accumulatorArray[a, b, r];
                                }

                            }
                        }
                    }
                }
            }

            for(int r = 0; r < maxRadius - minRadius; r++)
            {
                for(int x = 0; x < inputImage.GetLength(0); x++)
                {
                    for(int y = 0; y < inputImage.GetLength(1); y++)
                    {
                        bool smaller = false;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int xCoord = x + i - 1;
                                    int yCoord = y + j - 1;
                                    int zCoord = r + k - 1;

                                    if (xCoord < 0 || xCoord >= inputImage.GetLength(0) || yCoord < 0 || yCoord >= inputImage.GetLength(1) || zCoord < 0 || zCoord >= maxRadius- minRadius)
                                    {
                                        continue;
                                    }
                                    if (accumulatorArray[x, y, r] < accumulatorArray[xCoord, yCoord, zCoord])
                                        smaller = true;
                                }

                            }
                        }
                        if (smaller)
                            temp[x, y,r] = 0;
                        else
                            temp[x, y,r] = accumulatorArray[x, y,r];
                    }
                }
            }

            List<Tuple<int, int,int>> locations = new List<Tuple<int, int,int>>();

            for (int r = 0; r < maxRadius - minRadius; r++)
            {
                for (int x = 0; x < inputImage.GetLength(0); x++)
                {
                    for (int y = 0; y < inputImage.GetLength(1); y++)
                    {
                        if (temp[x, y, r] > (int)(max * ((double)numericUpDownThreshold.Value / 255d)))
                            locations.Add(new Tuple<int, int, int>(x,y,r));
                    }
                }
            }

            for(int i = 0; i < locations.Count; i++)
            {
                int r = locations[i].Item3;
                int x = locations[i].Item1;
                int y = locations[i].Item2;
                if (r >= minRadius)
                {
                    //image[x, y] = Color.Blue;
                    DrawCircle(Image, x, y, Color.Blue, r);
                }
                
            }

            for(int i = 0; i < Image.GetLength(0); i++)
            {
                for(int j = 0; j < Image.GetLength(1); j++)
                {
                    if (Image[i, j] != Color.Blue)
                        Image[i, j] = InputImage[i, j];
                }
            }
        }

        private int[,] houghTransform(Color[,] inputImage)
        {
            int xMid = inputImage.GetLength(0) / 2;
            int yMid = inputImage.GetLength(1) / 2;
            int nAng = (int)angSteps.Value;
            double deltaAng = Math.PI / nAng;
            int nRad = 256;
            int cRad = nRad / 2;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));
            double deltaRad = (2d * rMax) / nRad;
            bool binary = checkBinary(InputImage);
            int[,] accumulatorArray = new int[nAng, nRad];
            int max = 0;


            for (int x = 0; x < inputImage.GetLength(0); x++)
            {
                for (int y = 0; y < inputImage.GetLength(1); y++)
                {
                    if (inputImage[x, y].R <= (int)this.intensityThreshold.Value)
                    {
                        double u = x - xMid;
                        double v = y - yMid;
                        for (int i = 0; i < nAng; i++)
                        {
                            double theta = deltaAng * i;
                            int ir = cRad + (int)Math.Round(((u * Math.Cos(theta)) + (v * Math.Sin(theta))) / deltaRad);
                            if (ir >= 0 && ir < nRad)
                            {
                                if (binary)
                                    accumulatorArray[i, ir]++;
                                else
                                    accumulatorArray[i, ir] += 255 - inputImage[x, y].R;
                                if (accumulatorArray[i, ir] > max)
                                    max = accumulatorArray[i, ir];
                            }
                        }
                    }
                }
            }

            Image = new Color[nAng, nRad];
            for (int x = 0; x < nAng; x++)
            {
                for (int y = 0; y < nRad; y++)
                {
                    int intensity = (int)((accumulatorArray[x, y] / (double)max) * 255d);
                    Image[x, y] = Color.FromArgb(intensity, intensity, intensity);
                }
            }


            return accumulatorArray;
        }

        private List<Tuple<int,int>> FindCrossings(Color[,] image, List<Tuple<int, int>> locations)
        {
            var crossings = new List<Tuple<int, int>>();
            for (int i = 0; i < locations.Count; i++)
            {
                for (int j = i + 1; j < locations.Count; j++)
                {
                    int xMid = InputImage.GetLength(0) / 2;
                    int yMid = InputImage.GetLength(1) / 2;
                    double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));

                    var point1 = new Tuple<int, int>(locations[i].Item1, locations[i].Item2);
                    double radians1 = point1.Item1 / 256d * Math.PI;
                    double length1 = ((256d / 2d) - point1.Item2) / 256d * rMax * 2d;

                    double aanliggend1 = Math.Cos(radians1) * length1;
                    double overstaand1 = Math.Sqrt(length1 * length1 - aanliggend1 * aanliggend1) * (length1 < 0 ? -1 : 1);
                    double slope1 = aanliggend1 / overstaand1;
                    double b1 = (yMid - overstaand1) - slope1 * aanliggend1;

                    var point2 = new Tuple<int, int>(locations[j].Item1, locations[j].Item2);
                    double radians2 = point2.Item1 / 256d * Math.PI;
                    double length2 = ((256d / 2d) - point2.Item2) / 256d * rMax * 2d;

                    double aanliggend2 = Math.Cos(radians2) * length2;
                    double overstaand2 = Math.Sqrt(length2 * length2 - aanliggend2 * aanliggend2) * (length2 < 0 ? -1 : 1);
                    double slope2 = aanliggend2 / overstaand2;
                    double b2 = (yMid - overstaand2) - slope2 * aanliggend2;

                    if (Math.Abs(slope1 - slope2) < 0.00001d || (double.IsInfinity(slope1) && double.IsInfinity(slope2)))
                        continue;

                    double intersectX = (b1 - b2) / (slope2 - slope1);
                    double intersectY = intersectX * slope1 + b1;

                    if (double.IsInfinity(slope1))
                    {
                        intersectX = length1;
                        intersectY = intersectX * slope2 + b2;
                    }
                    if (double.IsInfinity(slope2))
                    {
                        intersectX = length2;
                        intersectY = intersectX * slope1 + b1;
                    }

                    int roundedX = (int)intersectX;
                    int roundedY = (int)intersectY;

                    roundedX *= -1;
                    roundedX += xMid;

                    if (roundedX >= 0 && roundedX < InputImage.GetLength(0) && roundedY >= 0 && roundedY < InputImage.GetLength(1))
                    {
                        crossings.Add(new Tuple<int, int>(roundedX, roundedY));
                    }
                }
            }
            return crossings;
        }

        private void DrawCircle(Color[,] image, int X, int Y, Color color, int r)
        {
            for(int i = 0; i < 2 * r + 1; i++)
            {
                for(int j = 0; j < 2 * r + 1; j++)
                {
                    double relX = i - r;
                    double relY = j - r;
                    if(Math.Abs(relX * relX + relY * relY - r * r) < r)
                    {
                        int newX = (int)(relX + X);
                        int newY = (int)(relY + Y);
                        if(newX >= 0 && newX < image.GetLength(0) && newY >= 0 && newY < image.GetLength(1))
                            image[(int)(relX + X), (int)(relY + Y)] = color;
                    }
                }
            }
        }

        private List<Tuple<Point, Point>> HoughLineDetection(Color[,] InputImage, Point rTheta, int minimumThreshold, int minLength, int maxGap)
        {
            var result = new List<Tuple<Point, Point>>();
            int xMid = InputImage.GetLength(0) / 2;
            int yMid = InputImage.GetLength(1) / 2;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));
            var point = new Tuple<int,int>(rTheta.X, rTheta.Y);
            double radians = point.Item1 / 256d * Math.PI;
            double length = ((256d / 2d) - point.Item2) / 256d * rMax * 2d;

            double aanliggend = Math.Cos(radians) * length;
            double overstaand = Math.Sqrt(length * length - aanliggend * aanliggend) * (length < 0 ? -1 : 1);
            double slope1 = aanliggend / overstaand;
            double slope2 = overstaand / aanliggend;
            double b1 = (yMid - overstaand) - slope1 * aanliggend;
            double b2 = (xMid - aanliggend) - (slope2) * overstaand;

            int curLength = 0;
            int curGap = 0;
            bool onLine = false;
            Point startPoint = new Point(0,0);

            if (Math.Abs(slope1) >= 1d)
            {
                for (int j = 0; j < InputImage.GetLength(1); j++)
                {
                    int x = (int)(slope2 * (yMid - j) + b2);
                    if (x >= 0 && x < InputImage.GetLength(0))
                    {
                        if (InputImage[x, j].R <= minimumThreshold)
                        {
                            if(!onLine)
                            {
                                onLine = true;
                                startPoint = new Point(x, j);
                                curLength = 0;
                                curGap = 0;
                            }
                            curLength++;
                            //curLength += curGap;
                            curGap = 0;
                        }
                        else
                        {
                            curGap++;
                            curLength++;
                            if (curGap > maxGap)
                            {
                                // TODO: lijnsegment controleren
                                if (curLength - curGap >= minLength)
                                {

                                    int oldX = (int)(slope2 * (yMid - j-curGap) + b2);
                                    result.Add(new Tuple<Point, Point>(startPoint, new Point(oldX, j-curGap)));
                                }
                                curGap = 0;
                                curLength = 0;
                                onLine = false;
                            }
                        }
                    }
                    else if(onLine && curLength - curGap >= minLength)
                    {
                        int oldX = (int)(slope2 * (yMid - j + 1 - curGap) + b2);
                        result.Add(new Tuple<Point, Point>(startPoint, new Point(oldX, j + 1 - curGap)));
                        onLine = false;
                    }
                }
                if(onLine && curLength - curGap >= minLength)
                {
                    int j = InputImage.GetLength(1)-1-curGap;
                    int x = (int)(slope2 * (yMid - j) + b2);
                    result.Add(new Tuple<Point, Point>(startPoint, new Point(x,j)));
                }
            }
            else if (Math.Abs(slope1) < 1d)
            {
                for (int j = 0; j < InputImage.GetLength(0); j++)
                {
                    int y = (int)(slope1 * (xMid - j) + b1);
                    if (y >= 0 && y < InputImage.GetLength(1))
                    {
                        if (InputImage[j, y].R <= minimumThreshold)
                        {
                            if (!onLine)
                            {
                                onLine = true;
                                startPoint = new Point(j, y);
                                curLength = 0;
                                curGap = 0;
                            }
                            curLength++;
                            //curLength += curGap;
                            curGap = 0;
                        }
                        else
                        {
                            curGap++;
                            curLength++;
                            if (curGap > maxGap)
                            {
                                // TODO: lijnsegment controleren
                                if (curLength - curGap >= minLength)
                                {
                                    int oldY = (int)(slope1 * (xMid - j - curGap) + b1);
                                    result.Add(new Tuple<Point, Point>(startPoint, new Point(j - curGap, oldY)));
                                }
                                curGap = 0;
                                curLength = 0;
                                onLine = false;
                            }
                        }
                    }
                    else if (onLine && curLength - curGap >= minLength)
                    {
                        int oldY = (int)(slope1 * (xMid - j + 1- curGap) + b1);
                        result.Add(new Tuple<Point, Point>(startPoint, new Point(j + 1 - curGap, oldY)));
                        onLine = false;
                    }
                }
                if (onLine && curLength - curGap >= minLength)
                {
                    int j = InputImage.GetLength(0)-1-curGap;
                    int y = (int)(slope1 * (xMid - j) + b1);
                    result.Add(new Tuple<Point, Point>(startPoint, new Point(j, y)));
                }            
            }
            return result;
        }

        private void startThresholdingThreads(object mt)
        {
            int threshholdValue = (int)numericUpDownThreshold.Value;
            ApplyThresholding((int)mt, InputImage, Image, threshholdValue);
        }

        private void ApplyThresholding(int mt, Color[,] InputImage, Color[,] Image, int threshholdValue)
        {
            int sqr = (int)Math.Sqrt(threads);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    Color pixelColor = InputImage[x, y];
                    Color updatedColor = Color.Black;
                    if (pixelColor.R >= threshholdValue || pixelColor.G >= threshholdValue || pixelColor.B >= threshholdValue)
                        updatedColor = Color.White;
                    Image[x, y] = updatedColor;
                }
            }
        }

        private void TraceBoundaries(Color[,] image, Color[,] inputImage, int[,] mask)
        {
            for(int x = 0; x < inputImage.GetLength(0); x++)
            {
                for(int y = 0; y < inputImage.GetLength(1); y++)
                {
                    if (inputImage[x, y].R == 0 && InputImage[x, y].G == 0 && InputImage[x, y].B == 0)
                    {
                        bool boundary = false;
                        for (int i = 0; i < mask.GetLength(0); i++)
                        {
                            for (int j = 0; j < mask.GetLength(1); j++)
                            {
                                int newx = x + i - mask.GetLength(0) / 2;
                                int newy = y + j - mask.GetLength(1) / 2;
                                if (newx < 0 || newx >= inputImage.GetLength(0) || newy < 0 || newy >= inputImage.GetLength(1))
                                {
                                    continue;
                                }
                                if (mask[i, j] == 1 && inputImage[newx, newy].R == 255 && inputImage[newx,newy].G == 255 && inputImage[newx,newy].B == 255)
                                {
                                    boundary = true;
                                    goto AfterMask;
                                }
                            }
                        }
                    AfterMask:
                        image[x, y] = boundary ? Color.Red : inputImage[x, y];
                    }
                    else
                        image[x, y] = inputImage[x, y];
                }
            }
            
        }

        private void startErosionClosing(object mt)
        {
            bool square = radioButtonSquare.Checked;
            int?[,] kernel = CreateStructure(kernelDimensionSize, square);
            Erode((int)mt, Image, tempImage, kernel);
        }

        private void startDilationOpening(object mt)
        {
            bool square = radioButtonSquare.Checked;
            int?[,] kernel = CreateStructure(kernelDimensionSize, square);
            Dilate((int)mt, Image, tempImage, kernel);
        }

        private void startDilation(object mt)
        {
            bool square = radioButtonSquare.Checked;
            int?[,] kernel = CreateStructure(kernelDimensionSize, square);
            Dilate((int)mt, InputImage, Image, kernel);
        }

        private void startErosion(object mt)
        {
            bool square = radioButtonSquare.Checked;
            int?[,] kernel = CreateStructure(kernelDimensionSize, square);
            Erode((int)mt, InputImage, Image, kernel);
        }

        private void startComplement(object mt)
        {
            InvertColors((int)mt, InputImage, Image);
        }

        private void startAnd(object mt)
        {
            And((int)mt, InputImage, InputImage2, Image);
        }
        private void startOr(object mt)
        {
            Or((int)mt, InputImage, InputImage2, Image);

        }

        private void startInvertThreads(object mt)
        {
            InvertColors((int)mt, InputImage, Image);
        }

        private void And(int mt, Color[,] InputImage, Color[,] InputImage2, Color[,] Image)
        {
            for(int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    Image[x, y] = InputImage[x, y].R + InputImage2[x, y].R == 255 * 2 ? Color.White : Color.Black;
                }
            }
        }

        private void Or(int mt, Color[,] inputImage, Color[,] inputImage2, Color[,] image)
        {
            for (int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    Image[x, y] = InputImage[x, y].R + InputImage2[x, y].R >= 255 ? Color.White : Color.Black;
                }
            }
        }

        private void InvertColors(int mt, Color[,] InputImage, Color[,] Image)
        {
            int sqr = (int)Math.Sqrt(threads);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;

            //==========================================================================================
            // example: create a negative image
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    Color pixelColor = InputImage[x, y];                         // Get the pixel color at coordinate (x,y)
                    Color updatedColor = Color.FromArgb(255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B); // Negative image
                    Image[x, y] = updatedColor;                             // Set the new pixel color at coordinate (x,y)
                }
            }
            //==========================================================================================
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputImage == null) return;                                // Get out if no output image
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
                OutputImage.Save(saveImageDialog.FileName);                 // Save the output image
        }

        private static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        // We have opted for the choiche that a plus shape is only the centre column and row
        public int?[,] CreateStructure(int size, bool rectangle)
        {
            Nullable<int>[,] result = new int?[size, size];
            if (rectangle)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        if (checkBoxGray.Checked)
                            result[x, y] = (int?)numericUpDownThreshold.Value;
                        else
                            result[x, y] = 1;
                    }
                }
            }
            else
            {
                if (checkBoxGray.Checked)
                {
                    if (size % 2 == 0)
                    {
                        // Two collumns are the middle ones
                        int middle1 = size / 2;
                        int middle2 = middle1 + 1;
                        for (int x = 0; x < size; x++)
                        {
                            for (int y = 0; y < size; y++)
                            {
                                if (x == middle1 || y == middle1 || x == middle2 || y == middle2)
                                    result[x, y] = (int)numericUpDownThreshold.Value;
                                else
                                    result[x, y] = null;
                            }
                        }
                    }
                    else
                    {
                        // Only one collumn is the midle one
                        int middle = size / 2;
                        for (int x = 0; x < size; x++)
                        {
                            for (int y = 0; y < size; y++)
                            {
                                if (x == middle || y == middle)
                                    result[x, y] = (int)numericUpDownThreshold.Value;
                                else
                                    result[x, y] = null;
                            }
                        }
                    }
                }
                else
                {
                    if (size % 2 == 0)
                    {
                        // Two collumns are the middle ones
                        int middle1 = size / 2;
                        int middle2 = middle1 + 1;
                        for (int x = 0; x < size; x++)
                        {
                            for (int y = 0; y < size; y++)
                            {
                                if (x == middle1 || y == middle1 || x == middle2 || y == middle2)
                                    result[x, y] = 1;
                                else
                                    result[x, y] = 0;
                            }
                        }
                    }
                    else
                    {
                        // Only one collumn is the midle one
                        int middle = size / 2;
                        for (int x = 0; x < size; x++)
                        {
                            for (int y = 0; y < size; y++)
                            {
                                if (x == middle || y == middle)
                                    result[x, y] = 1;
                                else
                                    result[x, y] = 0;
                            }
                        }
                    }
                }
                
            }
            return result;
        }

        public void Dilate(int mt, Color[,] InputImage, Color[,] Image, int?[,] kernel)
        {
            int sqr = (int)Math.Sqrt(threads);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;

            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    if (checkBoxGray.Checked)
                    {
                        int maxR = 0;
                        int maxG = 0;
                        int maxB = 0;
                        for (int i = 0; i < kernel.GetLength(0); i++)
                        {
                            for (int j = 0; j < kernel.GetLength(1); j++)
                            {
                                int xCoord = x + i - (kernel.GetLength(0) - 1) / 2;
                                int yCoord = y + j - (kernel.GetLength(1) - 1) / 2;

                                if (kernel[i, j] == null)
                                    continue;

                                if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                                {
                                    continue;
                                }
  
                                Color color = InputImage[xCoord, yCoord];
                                if (color.R + kernel[i,j] > maxR)
                                    maxR = color.R + (int)kernel[i, j];
                                if (color.G + kernel[i, j] > maxG)
                                    maxG = color.G + (int)kernel[i, j];
                                if (color.B + kernel[i, j] > maxB)
                                    maxB = color.B + (int)kernel[i, j];
                            }
                        }
                        maxR = Clamp(maxR,0,255);
                        maxG = Clamp(maxG, 0, 255);
                        maxB = Clamp(maxB, 0, 255);
                        Color newColor = Color.FromArgb(maxR, maxG, maxB);
                        Image[x, y] = newColor;
                    }
                    else
                    {
                        Color color = InputImage[x, y];
                        if (color.R == 255)
                        {
                            for (int i = 0; i < kernel.GetLength(0); i++)
                            {
                                for (int j = 0; j < kernel.GetLength(1); j++)
                                {
                                    int xCoord = x + i - (kernel.GetLength(0) - 1) / 2;
                                    int yCoord = y + j - (kernel.GetLength(1) - 1) / 2;
                                    if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1) || kernel[i,j] != 1)
                                    {
                                        continue;
                                    }
                                    if (kernel[i, j] == 1)
                                        Image[xCoord, yCoord] = Color.FromArgb(255, 255, 255);

                                }
                            }
                        }
                    }
                }
            }
        }

        public void Erode(int mt, Color[,] InputImage, Color[,] Image, int?[,] kernel)
        {
            int sqr = (int)Math.Sqrt(threads);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;

            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    if (checkBoxGray.Checked)
                    {
                        int minR = 255;
                        int minG = 255;
                        int minB = 255;
                        for (int i = 0; i < kernel.GetLength(0); i++)
                        {
                            for (int j = 0; j < kernel.GetLength(1); j++)
                            {
                                int xCoord = x + i - (kernel.GetLength(0) - 1) / 2;
                                int yCoord = y + j - (kernel.GetLength(1) - 1) / 2;
                                if (kernel[i, j] == null)
                                    continue;
                                if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                                {
                                    continue;
                                }
                                
                                Color color = InputImage[xCoord, yCoord];
                                if (color.R - kernel[i,j] < minR)
                                    minR = color.R - (int)kernel[i, j];
                                if (color.G - kernel[i, j] < minG)
                                    minG = color.G - (int)kernel[i, j];
                                if (color.B - kernel[i, j] < minB)
                                    minB = color.B - (int)kernel[i, j];
                            }
                        }
                        minR = Clamp(minR, 0, 255);
                        minG = Clamp(minG, 0, 255);
                        minB = Clamp(minB, 0, 255);
                        Color newColor = Color.FromArgb(minR, minG, minB);
                        Image[x, y] = newColor;
                    }
                    else
                    {
                        Color color = InputImage[x, y];
                        //Foreground
                        if (color.R == 255)
                        {
                            bool contains = true;
                            for (int i = 0; i < kernel.GetLength(0); i++)
                            {
                                for (int j = 0; j < kernel.GetLength(1); j++)
                                {
                                    int xCoord = x + i - (kernel.GetLength(0) - 1) / 2;
                                    int yCoord = y + j - (kernel.GetLength(1) - 1) / 2;
                                    if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                                    {
                                        continue;
                                    }
                                    if (kernel[i, j] == 1 && InputImage[xCoord, yCoord].R != 255)
                                        contains = false;
                                }
                            }
                            //structure element is not fully contained
                            if (!contains)
                                Image[x, y] = Color.FromArgb(0, 0, 0);
                            else
                                Image[x, y] = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
        }

        private void counting(Color[,] InputImage)
        {
            if (checkBoxGray.Checked)
            {
                int maxValue = 0;
                int maxLocation = 0;
                int distinct = 0;
                Image = new Color[256, 256];
                int[] histogram = new int[256];
                for (int x = 0; x < InputImage.GetLength(0); x++)
                {
                    for (int y = 0; y < InputImage.GetLength(1); y++)
                    {
                        Color pixelColor = InputImage[x, y];
                        histogram[pixelColor.R]++;
                        if (histogram[pixelColor.R] > maxValue)
                        {
                            maxValue = histogram[pixelColor.R];
                        }
                        if(histogram[pixelColor.R] == 1)
                            distinct++;
                    }
                }

                //textBox1.Text = distinct.ToString();
                //textBoxNonBGPixels.Text = histogram[3].ToString();
                double div = maxValue / 255d;

                for(int x = 0; x < 256; x++)
                {
                    histogram[x] = (int)(histogram[x] / div);
                    for (int y = 0; y < histogram[x]; y++)
                    {
                        Image[x, 255 - y] = Color.FromArgb(255,255,255);
                    }
                }
                threads = 1;
                tempImage = new Color[256, 256];
                InvertColors(0, Image, tempImage);
                Image = tempImage;
            }
            else
            {
                Image = new Color[256, 461];
                int[] histogramR = new int[256];
                int[] histogramG = new int[256];
                int[] histogramB = new int[256];
                int maxValueR = 0;
                int maxValueG = 0;
                int maxValueB = 0;
                // Get Histograms
                for (int x = 0; x < InputImage.GetLength(0); x++)
                {
                    for (int y = 0; y < InputImage.GetLength(1); y++)
                    {
                        Color pixelColor = InputImage[x, y];
                        histogramR[pixelColor.R]++;
                        histogramG[pixelColor.G]++;
                        histogramB[pixelColor.B]++;
                        if (histogramR[pixelColor.R] > maxValueR)
                        {
                            maxValueR = histogramR[pixelColor.R];
                        }
                        if (histogramG[pixelColor.G] > maxValueG)
                        {
                            maxValueG = histogramG[pixelColor.G];
                        }
                        if (histogramB[pixelColor.B] > maxValueB)
                        {
                            maxValueB = histogramB[pixelColor.B];
                        }
                    }
                }

                double divR = maxValueR / 150d;
                double divG = maxValueG / 150d;
                double divB = maxValueB / 150d;

                for (int x = 0; x < 256; x++)
                {
                    histogramR[x] = (int)(histogramR[x] / divR);
                    for (int y = 0; y < histogramR[x]; y++)
                    {
                        Image[x, 150 - y] = Color.Red;
                    }
                    histogramG[x] = (int)(histogramG[x] / divG);
                    for (int y = 0; y < histogramG[x]; y++)
                    {
                        Image[x, 310 - y] = Color.Green;
                    }
                    histogramB[x] = (int)(histogramB[x] / divB);
                    for (int y = 0; y < histogramB[x]; y++)
                    {
                        Image[x, 460 - y] = Color.Blue;
                    }
                }

            }
        }

        private void ButtonSetAsImage_Click(object sender, EventArgs e)
        {
            if (InputBitmap != null) InputBitmap.Dispose();               // Reset image
            InputBitmap = OutputImage.Clone() as Bitmap;
            pictureBox1.Image = (Image)InputBitmap;                 // Display input image

            progressBar.Visible = true;
            progressBar.Minimum = 1;
            progressBar.Maximum = InputBitmap.Size.Width * InputBitmap.Size.Height;
            progressBar.Value = 1;
            progressBar.Step = 1;
            for (int x = 0; x < InputBitmap.Size.Width; x++)
            {
                for (int y = 0; y < InputBitmap.Size.Height; y++)
                {
                    Image[x, y] = InputBitmap.GetPixel(x, y);                // Set pixel color in array at (x,y)
                    InputImage[x, y] = InputBitmap.GetPixel(x, y);
                    //progressBar.PerformStep();
                }
            }
            progressBar.Visible = false;
            checkBoxGray.Checked = checkGrayscale(Image);
        }
    
    }

    public class ExactColor
    {
        public double R, G, B;

        public ExactColor(double r, double g, double b)
        {
            R = r; G = g; B = b;
        }
    }
}
