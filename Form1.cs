using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace INFOIBV
{
    public partial class INFOIBV : Form
    {
        private Bitmap InputImage;
        private Bitmap OutputImage;

        private int maxR = 255, maxG = 255, maxB = 255, minR = 0, minG = 0, minB = 0;

        List<detectableObject> detect;

        // Prewitt operators ///////////////////////////////////////////////////////
        readonly double[,] prewittXKernel = new double[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        readonly double[,] prewittYKernel = new double[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
        readonly double prewittScalar = 1d / 6;
        ////////////////////////////////////////////////////////////////////////////

        int maxRegions;

        public INFOIBV()
        {
            InitializeComponent();
            comboBoxTask.SelectedIndex = 0;
            // objects
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)             // Open File Dialog
            {
                string file = openImageDialog.FileName;                     // Get the file name
                imageFileName.Text = file;                                  // Show file name
                if (InputImage != null) InputImage.Dispose();               // Reset image
                InputImage = new Bitmap(file);                              // Create new Bitmap from file
                //if (InputImage.Size.Height <= 0 || InputImage.Size.Width <= 0 ||
                //    InputImage.Size.Height > 512 || InputImage.Size.Width > 512) // Dimension check
                //    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                //else
                pictureBox1.Image = (Image)InputImage;                 // Display input image
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (InputImage == null) return;                                 // Get out if no input image
            if (OutputImage != null) OutputImage.Dispose();                 // Reset output image
            OutputImage = new Bitmap(InputImage.Size.Width, InputImage.Size.Height); // Create new output image
            Color[,] Image = new Color[InputImage.Size.Width, InputImage.Size.Height]; // Create array to speed-up operations (Bitmap functions are very slow)

            // Setup progress bar
            progressBar.Visible = true;
            progressBar.Minimum = 1;
            progressBar.Maximum = InputImage.Size.Width * InputImage.Size.Height;
            progressBar.Value = 1;
            progressBar.Step = 1;

            // Copy input Bitmap to array            
            for (int x = 0; x < InputImage.Size.Width; x++)
            {
                for (int y = 0; y < InputImage.Size.Height; y++)
                {
                    Image[x, y] = InputImage.GetPixel(x, y);                // Set pixel color in array at (x,y)
                }
            }

            

            var histo = ContrastHistogram(Image);
            int kernelDimensionSize = 3;

            Func<int[,]> PreProcessing = () =>
            {
                //Image = ContrastAdjustment(Image);
                var binaryImage = ApplyThresholding(Image, (int)numericUpDownThreshold.Value);
                var binaryImage2 = InvertBinary(binaryImage);
                print(binaryImage2, pictureBox3, label3, "Binary Iamge:");
                var edgeImage = ApplyEdgeDetection(Image, prewittXKernel, prewittYKernel, prewittScalar);
                print(edgeImage, pictureBox4, label4, "Edge detection:");
                double[,] kernel3 = CreateGaussianKernel(3, (double)2);
                var zooi2 = ApplyLinearFilter(edgeImage, kernel3);
                var edgeImage7 = ApplyEdgeSharpening(edgeImage, zooi2, (double)3);
                print(edgeImage7, pictureBox5, label5, "Edge Sharpening");
                var edgeImage2 = ApplyThresholding(edgeImage7, (int)numericUpDownThresholdEdgeDetection.Value);
                var edgeImage3 = InvertImage(edgeImage2);
                var kernel = CreateStructure(kernelDimensionSize, 3, true);
                var edgeImage4 = Erode(edgeImage3, kernel);
                var edgeImage5 = Dilate(edgeImage4, kernel);
                var edgeImage6 = ApplyThresholding(edgeImage5, (int)numericUpDownThreshold.Value);
                print(edgeImage6, pictureBox6, label6, "Thresholded Edge");
                var regions = RegionDetection(binaryImage2, edgeImage6);                
                return regions;
            };

            switch (comboBoxTask.SelectedIndex)
            {
                case 0:
                    // Thresholding
                    Image = ApplyThresholding(Image, (int)numericUpDownThreshold.Value);
                    break;
                case 1:
                    // Contrast Boost

                    Image = ContrastAdjustment(Image);
                    break;
                case 2:
                    // Edge Detection
                    Image = ApplyEdgeDetection(Image, prewittXKernel, prewittYKernel, prewittScalar);
                    Image = InvertImage(Image);
                    break;
                case 3:
                    // Opening (erosion -> dilation)
                    int?[,] kernel = CreateStructure(kernelDimensionSize);
                    var erode = Erode(Image, kernel);
                    Image = Dilate(erode, kernel);
                    break;
                case 4:
                    // Closing (dilation -> erosion)

                    kernel = CreateStructure(kernelDimensionSize);
                    var dilate = Dilate(Image, kernel);
                    Image = Erode(dilate, kernel);
                    break;
                case 5:
                    // Line Detection
                    var accumulatorArray = houghTransform(Image);
                    var thresholdedArray = HoughThreshold(accumulatorArray);
                    Image = HoughLineDetection(thresholdedArray, Image);
                    break;
                case 6:
                    // Region Detection
                    var regions = PreProcessing();
                    Image = DrawRegions(Image, regions);
                    break;
                case 7:
                    // Object Detection
                    var areas = PreProcessing();
                    var areaInfos = GetAreas(areas);

                    var newDict = new Dictionary<int, AreaInfo>();
                    int max = 0;
                    int? id = null;

                    foreach(var info in areaInfos)
                    {
                        if (info.Value.Area > 100)
                        {
                            newDict.Add(info.Key, info.Value);
                            if (info.Value.Area > max && info.Key != 1)
                            {
                                id = info.Key;
                                max = info.Value.Area;
                            }
                        }

                    }
                    if(id != null)
                    {
                        string text = "\n";
                        text += "CX: " + newDict[(int)id].CentroidX.ToString() + "\n";
                        text += "CY: " + newDict[(int)id].CentroidY.ToString() + "\n";
                        text += "Circ: " + newDict[(int)id].Circularity.ToString() + "\n";
                        text += "Comp: " + newDict[(int)id].Compactness.ToString() + "\n";
                        text += "MAXX: " + newDict[(int)id].Max.X.ToString() + "\n";
                        text += "MAXY: " + newDict[(int)id].Max.Y.ToString() + "\n";
                        text += "MINX: " + newDict[(int)id].Min.X.ToString() + "\n";
                        text += "MINY: " + newDict[(int)id].Min.X.ToString() + "\n";
                        File.AppendAllText("file.txt", text);
                    }
                    //Image = drawDict(Image, newDict);
                     break;
                    //goto case 6;
                case 8:
                    //edge sharpening
                    double[,] kernel2 = CreateGaussianKernel(3, (double)2);
                    var zooi = ApplyLinearFilter(Image, kernel2);
                    Image = ApplyEdgeSharpening(Image, zooi, (double)3);
                    break;
                case 9:
                    Image = InvertImage(Image);
                    break;
                default:
                    return;
            }
            print(Image, pictureBox2, label2, "Output Image");
            progressBar.Visible = false;
        }

        //private Color[,] drawDict(Color[,] image, Dictionary<int, AreaInfo> newDict)
        //{
        //    Color[,] res = new Color[image.GetLength(0), image.GetLength(1)];
        //    Random rnd = new Random(1337);
        //    Color[] colors = new Color[maxRegions];
        //    for (int i = 0; i < maxRegions; i++)
        //    {
        //        colors[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        //    }
        //    for (int y = 0; y < image.GetLength(1); y++)
        //    {
        //        for (int x = 0; x < image.GetLength(0); x++)
        //        {
        //            if (regions[x, y] != 0)
        //            {
        //                res[x, y] = colors[regions[x, y] - 1];
        //            }
        //            else
        //                res[x, y] = Color.White;
        //        }
        //    }
        //    return res;
        //}

        private Dictionary<int,AreaInfo> GetAreas(int[,] regions)
        {
            var res = new Dictionary<int,AreaInfo>();



            for(int i = 0; i < regions.GetLength(0); i++)
            {
                for(int j = 0; j < regions.GetLength(1); j++)
                {
                    int currentRegion = regions[i, j];
                    if (currentRegion == 0)
                        continue;

                    if(res.ContainsKey(currentRegion))
                    {
                        var curArea = res[currentRegion];
                        curArea.Pixels.Add(new Point(i, j));
                        if (i < curArea.Min.X)
                            curArea.Min.X = i;
                        if (i > curArea.Max.X)
                            curArea.Max.X = i;
                        if (j < curArea.Min.Y)
                            curArea.Min.Y = j;
                        if (j > curArea.Max.Y)
                            curArea.Max.Y = j;

                        bool perimiter = false;

                        for(int x = -1; x < 2; x++)
                        {
                            for(int y = -1; y < 2; y++)
                            {
                                if ((x == -1 || x == 1) && y != 0 && Neighbour.Checked || (x == 0 && y == 0))
                                    continue;
                                int absX = i + x;
                                int absY = j + y;
                                if (absX < 0 || absX >= regions.GetLength(0) || absY < 0 || absY >= regions.GetLength(1))
                                {
                                    perimiter = true;
                                    continue;
                                }
                                perimiter |= regions[absX, absY] == 0;
                            }
                        }
                        if (perimiter)
                            curArea.Perimeter++;
                    }
                    else
                    {
                        res.Add(currentRegion, new AreaInfo(currentRegion, new Point(i,j), new Point(i,j), 1, new List<Point> { new Point(i, j) }));
                    }
                }
            }

            return res;
        }

        void print(Color[,] Input, PictureBox output, Label textLabel, String text)
        {
            Bitmap pic = new Bitmap(Input.GetLength(0), Input.GetLength(1));
            // Copy array to output Bitmap
            for (int x = 0; x < InputImage.Size.Width; x++)
            {
                for (int y = 0; y < InputImage.Size.Height; y++)
                {
                    pic.SetPixel(x, y, Input[x, y]);               // Set the pixel color at coordinate (x,y)
                    OutputImage.SetPixel(x, y, Input[x, y]);
                }
            }
            output.Image = (Image)pic;                         // Display output image
            textLabel.Text = text;

        }
        private void ButtonSetAsImage_Click(object sender, EventArgs e)
        {
            InputImage = (Bitmap)OutputImage.Clone();

            pictureBox1.Image = (Image)InputImage;
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (OutputImage == null) return;                                // Get out if no output image
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
                OutputImage.Save(saveImageDialog.FileName);                 // Save the output image
        }

        private Color[,] ApplyLinearFilter(Color[,] InputImage, double[,] kernel, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
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
                    ExactColor updatedColor = new ExactColor(0, 0, 0);
                    double missed = 0; // for compensating pixels outside of the image
                    for (int i = 0; i < kernel.GetLength(0); i++)
                        for (int j = 0; j < kernel.GetLength(1); j++)
                        {
                            int xCoord = x + i - (kernel.GetLength(0) - 1) / 2;
                            int yCoord = y + j - (kernel.GetLength(1) - 1) / 2;
                            if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                            {
                                missed += kernel[i, j];
                                continue;
                            }

                            Color pixelColor = InputImage[xCoord, yCoord];
                            updatedColor = new ExactColor(updatedColor.R + pixelColor.R * kernel[i, j], updatedColor.G + pixelColor.G * kernel[i, j], updatedColor.B + pixelColor.B * kernel[i, j]);
                        }


                    res[x, y] = Color.FromArgb((int)(updatedColor.R * (1 / (1 - missed))), (int)(updatedColor.G * (1 / (1 - missed))), (int)(updatedColor.B * (1 / (1 - missed))));                             // Set the new pixel color at coordinate (x,y)                           // Increment progress bar
                }
            }
            return res;
        }

        public static double[,] CreateGaussianKernel(int dimensionSize, double standardDeviation)
        {
            double[,] kernel = new double[dimensionSize, dimensionSize];
            double sum = 0;
            int offset = (dimensionSize - 1) / 2;
            double distance;
            for (int xRaw = 0; xRaw < dimensionSize; xRaw++)
                for (int yRaw = 0; yRaw < dimensionSize; yRaw++)
                {
                    int x = xRaw - offset;
                    int y = yRaw - offset;
                    distance = ((x * x) + (y * y)) / (2 * standardDeviation * standardDeviation);
                    double value = 1d / (2 * Math.PI * standardDeviation * standardDeviation) * Math.Exp(-distance);
                    kernel[x + offset, y + offset] = value;
                    sum += value;
                }

            // normalize the matrix to have a sum of 1
            for (int x = 0; x < dimensionSize; x++)
            {
                for (int y = 0; y < dimensionSize; y++)
                {
                    kernel[x, y] = kernel[x, y] / sum;
                }
            }
            return kernel;
        }

        private Color[,] DrawRegions(Color[,] image, int[,] regions)
        {
            Color[,] res = new Color[image.GetLength(0), image.GetLength(1)];
            Random rnd = new Random(1337);
            Color[] colors = new Color[maxRegions];
            for (int i = 0; i < maxRegions; i++)
            {
                colors[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    if (regions[x, y] != 0)
                    {
                        res[x, y] = colors[regions[x, y] - 1];
                    }
                    else
                        res[x, y] = Color.White;
                }
            }
            return res;
        }

        private Color[,] InvertImage(Color[,] InputImage)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];

            for (int i = 0; i < InputImage.GetLength(0); i++)
            {
                for (int j = 0; j < InputImage.GetLength(1); j++)
                {
                    var startColor = InputImage[i, j];
                    res[i, j] = Color.FromArgb(255 - startColor.R, 255 - startColor.G, 255 - startColor.B);
                }
            }
            return res;
        }

        private Color[,] InvertBinary(Color[,] Input)
        {
            var res = new Color[Input.GetLength(0), Input.GetLength(1)];

            for (int i = 0; i < Input.GetLength(0); i++)
            {
                for (int j = 0; j < Input.GetLength(1); j++)
                {
                    var startColor = Input[i, j];
                    if (startColor.R == 255)
                        res[i, j] = Color.Black;
                    else
                        res[i, j] = Color.White;
                }
            }
            return res;
        }

        private Color[,] ApplyThresholding(Color[,] InputImage, int threshholdValue, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    if (x == 144 && y == 182)
                    { int a = 0; }
                    Color pixelColor = InputImage[x, y];
                    Color updatedColor = Color.FromArgb(0, 0, 0);
                    if (pixelColor.R >= threshholdValue || pixelColor.G >= threshholdValue || pixelColor.B >= threshholdValue)
                        updatedColor = Color.FromArgb(255, 255, 255);
                    res[x, y] = updatedColor;
                }
            }
            return res;
        }

        private int[] ContrastHistogram(Color[,] InputImage)
        {
            var res = new int[256];
            float qLow = 0.05f, qHigh = 0.05f;
            int[] histoR = new int[256], histoG = new int[256], histoB = new int[256];
            for (int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    Color pixelColor = InputImage[x, y];
                    histoR[pixelColor.R]++;
                    histoG[pixelColor.G]++;
                    histoB[pixelColor.B]++;
                    if (pixelColor.R < minR)
                        minR = pixelColor.R;
                    if (pixelColor.G < minG)
                        minG = pixelColor.G;
                    if (pixelColor.B < minB)
                        minB = pixelColor.B;
                    if (pixelColor.R > maxR)
                        maxR = pixelColor.R;
                    if (pixelColor.G > maxG)
                        maxG = pixelColor.G;
                    if (pixelColor.B > maxB)
                        maxB = pixelColor.B;
                }
            }
            int pixelCount = InputImage.GetLength(0) * InputImage.GetLength(1);
            int remLow = (int)(pixelCount * qLow);
            int counter = 0;
            while (remLow > 0)
            {
                int amount = histoR[counter];
                remLow -= amount;
                counter++;
            }
            minR = counter;
            remLow = (int)(pixelCount * qLow);
            counter = 0;
            while (remLow > 0)
            {
                int amount = histoG[counter];
                remLow -= amount;
                counter++;
            }
            minG = counter;
            remLow = (int)(pixelCount * qLow);
            counter = 0;
            while (remLow > 0)
            {
                int amount = histoB[counter];
                remLow -= amount;
                counter++;
            }
            minB = counter;

            int remHigh = (int)(pixelCount * qHigh);
            counter = 255;
            while (remHigh > 0)
            {
                int amount = histoR[counter];
                remHigh -= amount;
                counter--;
            }
            maxR = counter;
            remHigh = (int)(pixelCount * qHigh);
            counter = 255;
            while (remHigh > 0)
            {
                int amount = histoG[counter];
                remHigh -= amount;
                counter--;
            }
            maxG = counter;
            remHigh = (int)(pixelCount * qHigh);
            counter = 255;
            while (remHigh > 0)
            {
                int amount = histoB[counter];
                remHigh -= amount;
                counter--;
            }
            maxB = counter;
            return res;
        }

        private Color[,] ContrastAdjustment(Color[,] InputImage, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;
            if (!(maxR == minR || maxG == minG || maxB == minB))
            {
                for (int x = fromX; x < toX; x++)
                {
                    for (int y = fromY; y < toY; y++)
                    {
                        Color pixelColor = InputImage[x, y];
                        int updatedR, updatedG, updatedB;

                        if (pixelColor.R <= minR)
                            updatedR = 0;
                        else if (pixelColor.R >= maxR)
                            updatedR = 255;
                        else
                        {
                            updatedR = (int)((pixelColor.R - minR) * (255d / (maxR - minR)));
                        }

                        if (pixelColor.G <= minG)
                            updatedG = 0;
                        else if (pixelColor.G >= maxG)
                            updatedG = 255;
                        else
                        {
                            updatedG = (int)((pixelColor.G - minG) * (255d / (maxG - minG)));
                        }

                        if (pixelColor.B <= minB)
                            updatedB = 0;
                        else if (pixelColor.B >= maxB)
                            updatedB = 255;
                        else
                        {
                            updatedB = (int)((pixelColor.B - minB) * (255d / (maxB - minB)));
                        }

                        updatedR = Clamp(updatedR, 0, 255);
                        updatedG = Clamp(updatedG, 0, 255);
                        updatedB = Clamp(updatedB, 0, 255);

                        Color updatedColor = Color.FromArgb(updatedR, updatedG, updatedB);
                        res[x, y] = updatedColor;
                    }
                }
            }
            return res;
        }

        private Color[,] ApplyEdgeDetection(Color[,] InputImage, double[,] xKernel, double[,] yKernel, double scalar, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    double redX = 0, greenX = 0, blueX = 0, redY = 0, greenY = 0, blueY = 0;
                    for (int i = 0; i < xKernel.GetLength(0); i++)
                    {
                        for (int j = 0; j < xKernel.GetLength(1); j++)
                        {
                            int xCoord = x + i - (xKernel.GetLength(0) - 1) / 2;
                            int yCoord = y + j - (xKernel.GetLength(1) - 1) / 2;
                            if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                            {
                                continue;
                            }

                            Color pixelColor = InputImage[xCoord, yCoord];
                            redX += pixelColor.R * xKernel[i, j];
                            greenX += pixelColor.G * xKernel[i, j];
                            blueX += pixelColor.B * xKernel[i, j];
                        }
                    }
                    for (int i = 0; i < yKernel.GetLength(0); i++)
                    {
                        for (int j = 0; j < yKernel.GetLength(1); j++)
                        {
                            int xCoord = x + i - (yKernel.GetLength(0) - 1) / 2;
                            int yCoord = y + j - (yKernel.GetLength(1) - 1) / 2;
                            if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1))
                            {
                                continue;
                            }

                            Color pixelColor = InputImage[xCoord, yCoord];
                            redY += pixelColor.R * yKernel[i, j];
                            greenY += pixelColor.G * yKernel[i, j];
                            blueY += pixelColor.B * yKernel[i, j];
                        }
                    }
                    redX *= scalar;
                    greenX *= scalar;
                    blueX *= scalar;
                    redY *= scalar;
                    greenY *= scalar;
                    blueY *= scalar;
                    redX *= redX;
                    greenX *= greenX;
                    blueX *= blueX;
                    redY *= redY;
                    greenY *= greenY;
                    blueY *= blueY;

                    //res[x, y] = Color.FromArgb((int)Math.Sqrt(redX + redY), (int)Math.Sqrt(greenX + greenY), (int)Math.Sqrt(blueX + blueY));
                    int colorR = (int)Math.Sqrt(redX + redY);
                    int colorG = (int)Math.Sqrt(greenX + greenY);
                    int colorB = (int)Math.Sqrt(blueX + blueY);
                    int color = Math.Max(colorR, Math.Max(colorG, colorB));
                    res[x, y] = Color.FromArgb(color, color, color);
                }
            }
            return res;
        }

        public int?[,] CreateStructure(int size, int? value = 3, bool plus = false, bool grayscale = true)
        {
            var result = new int?[size, size];
            if (plus)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int x = i - size / 2;
                        int y = j - size / 2;

                        if (x == 0 || y == 0)
                        {
                            if (grayscale)
                                result[i, j] = value;
                            else
                                result[i, j] = 1;
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        if (grayscale)
                            result[x, y] = value;
                        else
                            result[x, y] = 1;
                    }
                }
            }
            return result;
        }


        public Color[,] Dilate(Color[,] InputImage, int?[,] kernel, bool grayscale = true, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;

            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    if (grayscale)
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
                                if (color.R + kernel[i, j] > maxR)
                                    maxR = color.R + (int)kernel[i, j];
                                if (color.G + kernel[i, j] > maxG)
                                    maxG = color.G + (int)kernel[i, j];
                                if (color.B + kernel[i, j] > maxB)
                                    maxB = color.B + (int)kernel[i, j];
                            }
                        }
                        maxR = Clamp(maxR, 0, 255);
                        maxG = Clamp(maxG, 0, 255);
                        maxB = Clamp(maxB, 0, 255);
                        Color newColor = Color.FromArgb(maxR, maxG, maxB);
                        res[x, y] = newColor;
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
                                    if (xCoord < 0 || xCoord >= InputImage.GetLength(0) || yCoord < 0 || yCoord >= InputImage.GetLength(1) || kernel[i, j] != 1)
                                    {
                                        continue;
                                    }
                                    if (kernel[i, j] == 1)
                                        res[xCoord, yCoord] = Color.FromArgb(255, 255, 255);

                                }
                            }
                        }
                    }
                }
            }
            return res;
        }

        public Color[,] Erode(Color[,] InputImage, int?[,] kernel, bool grayscale = true, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;

            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    if (grayscale)
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
                                if (color.R - kernel[i, j] < minR)
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
                        res[x, y] = newColor;
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
                                res[x, y] = Color.FromArgb(0, 0, 0);
                            else
                                res[x, y] = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
            return res;
        }


        private int[,] RegionDetection(Color[,] BinaryImage, Color[,] EdgeImage)
        {
            var res = new int[BinaryImage.GetLength(0), BinaryImage.GetLength(1)];
            int counter = 1;
            var collisions = new Dictionary<int, int>();
            for (int y = 0; y < BinaryImage.GetLength(1); y++)
            {
                for (int x = 0; x < BinaryImage.GetLength(0); x++)
                {
                    if (EdgeImage[x, y].R == 0)
                        continue;
                    int hasNeighbor = 0;
                    List<int> spottedRegions = new List<int>();
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 1; j++)
                        {
                            if (((i == -1 || i == 1) && j != 0) && Neighbour.Checked)
                                continue;
                            if (x + i >= 0 && x + i < BinaryImage.GetLength(0) && y + j >= 0 && y + j < BinaryImage.GetLength(1))
                            {
                                if (res[x + i, y + j] != 0)
                                {
                                    if (hasNeighbor == 0)
                                        hasNeighbor = res[x + i, y + j];
                                    else if (hasNeighbor != res[x + i, y + j])
                                    {
                                        // Do something res[x + i, y + j] < hasNeighbor 
                                        // Colision
                                        int test1 = res[x + i, y + j];
                                        int test2 = hasNeighbor;

                                        if (res[x + i, y + j] < hasNeighbor)
                                        {
                                            if (collisions[hasNeighbor] > res[x + i, y + j] || collisions[hasNeighbor] == -1)
                                                collisions[hasNeighbor] = res[x + i, y + j];
                                            // Do something special
                                            hasNeighbor = res[x + i, y + j];
                                        }
                                        else
                                        {
                                            if (collisions[res[x + i, y + j]] > hasNeighbor || collisions[res[x + i, y + j]] == -1)
                                                collisions[res[x + i, y + j]] = hasNeighbor;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (hasNeighbor == 0)
                    {
                        res[x, y] = counter;
                        collisions.Add(counter, -1);
                        counter++;
                        maxRegions++;
                    }
                    else
                        res[x, y] = hasNeighbor;
                }
            }

            for (int y = 0; y < BinaryImage.GetLength(1); y++)
            {
                for (int x = 0; x < BinaryImage.GetLength(0); x++)
                {
                    if (res[x, y] != 0)
                    {
                        int debug = res[x, y];
                        while (collisions[res[x, y]] != -1)
                        {
                            int debug2 = collisions[res[x, y]];
                            res[x, y] = collisions[res[x, y]];
                        }
                    }
                }
            }

            return res;

            // In case of desperation:

            //bool change = true;
            //while(change)
            //{
            //    change = false;
            //    for (int y = 0; y < BinaryImage.GetLength(0); y++)
            //    {
            //        for (int x = 0; x < BinaryImage.GetLength(1); x++)
            //        {
            //            if(res[x,y] != 0)
            //            {
            //                int low = res[x, y];
            //                for (int i = -1; i < 2; i++)
            //                {
            //                    for (int j = -1; j < 2; j++)
            //                    {
            //                        if (x + i >= 0 && x + i < BinaryImage.GetLength(0) && y + j >= 0 && y + j < BinaryImage.GetLength(1))
            //                        {
            //                            if (res[x + i, y + j] < low && res[x + i, y + j] != 0)
            //                                low = res[x + i, y + j];
            //                        }
            //                    }
            //                }

            //                if(low != res[x, y])
            //                {
            //                    res[x, y] = low;
            //                    change = true;
            //                }
            //            }
            //        }
            //    }
            //}

            return res;
        }

        private int[,] houghTransform(Color[,] InputImage)
        {
            int xMid = InputImage.GetLength(0) / 2;
            int yMid = InputImage.GetLength(1) / 2;
            int nAng = (int)numericUpDownnAng.Value;
            double deltaAng = Math.PI / nAng;
            int nRad = (int)numericUpDownnRad.Value;
            int cRad = nRad / 2;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));
            double deltaRad = (2d * rMax) / nRad;
            bool binary = checkBinary(InputImage);
            int[,] accumulatorArray = new int[nAng, nRad];
            int max = 0;


            for (int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    if (InputImage[x, y].R <= (int)this.numericUpDownThreshold.Value)
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
                                    accumulatorArray[i, ir] += 255 - InputImage[x, y].R;
                                if (accumulatorArray[i, ir] > max)
                                    max = accumulatorArray[i, ir];
                            }
                        }
                    }
                }
            }
            /*
            Image = new Color[nAng, nRad];
            for (int x = 0; x < nAng; x++)
            {
                for (int y = 0; y < nRad; y++)
                {
                    int intensity = (int)((accumulatorArray[x, y] / (double)max) * 255d);
                    Image[x, y] = Color.FromArgb(intensity, intensity, intensity);
                }
            }*/


            return accumulatorArray;
        }

        private int[,] HoughThreshold(int[,] accumulatorArray)
        {
            int nAng = (int)numericUpDownnAng.Value;
            int nRad = (int)numericUpDownnRad.Value;
            int max = 0;

            int[,] temp = new int[nAng, nRad];
            for (int x = 0; x < nAng; x++)
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

                            if (xCoord < 0 || xCoord >= nAng || yCoord < 0 || yCoord >= nRad)
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


            for (int x = 0; x < nAng; x++)
            {
                for (int y = 0; y < nRad; y++)
                {
                    int value = (int)(temp[x, y] / (double)max * 255d);
                    if (value >= threshold)
                        ;//Image[x, y] = Color.FromArgb(value, value, value);
                    else
                    {
                        temp[x, y] = 0;
                        //Image[x, y] = Color.Black;
                    }
                }
            }
            return temp;
        }

        private Color[,] HoughLineDetection(int[,] accumulatorArray, Color[,] InputImage)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int xMid = InputImage.GetLength(0) / 2;
            int yMid = InputImage.GetLength(1) / 2;
            int nAng = (int)numericUpDownnAng.Value;
            double deltaAng = Math.PI / nAng;
            int nRad = (int)numericUpDownnRad.Value;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));

            int intensityThreshold;
            if (checkBinary(InputImage))
                intensityThreshold = 0;
            else
                intensityThreshold = (int)this.numericUpDownThreshold.Value;

            var locations = new List<Tuple<int, int>>();

            for (int x = 0; x < accumulatorArray.GetLength(0); x++)
            {
                for (int y = 0; y < accumulatorArray.GetLength(1); y++)
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
                                    res[x, j] = Color.Red;
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
                                    res[j, y] = Color.Red;
                            }
                        }
                    }
                }
            }


            if (checkBoxCrossings.Checked)
            {
                var crossings = FindCrossings(res, locations);
                foreach (var crossing in crossings)
                    DrawCircle(res, crossing.Item1, crossing.Item2, Color.Cyan, 5);
            }

            for (int x = 0; x < InputImage.GetLength(0); x++)
            {
                for (int y = 0; y < InputImage.GetLength(1); y++)
                {
                    if (res[x, y] != Color.Red && res[x, y] != Color.Cyan)
                        res[x, y] = InputImage[x, y];
                }
            }
            return res;
        }

        private List<Tuple<Point, Point>> HoughLineDetection(Color[,] InputImage, Point rTheta, int minimumThreshold, int minLength, int maxGap)
        {
            double nAng = (double)numericUpDownnAng.Value;
            double nRad = (double)numericUpDownnRad.Value;
            var result = new List<Tuple<Point, Point>>();
            int xMid = InputImage.GetLength(0) / 2;
            int yMid = InputImage.GetLength(1) / 2;
            double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));
            var point = new Tuple<int, int>(rTheta.X, rTheta.Y);
            double radians = point.Item1 / nRad * Math.PI;
            double length = ((nAng / 2d) - point.Item2) / nAng * rMax * 2d;

            double aanliggend = Math.Cos(radians) * length;
            double overstaand = Math.Sqrt(length * length - aanliggend * aanliggend) * (length < 0 ? -1 : 1);
            double slope1 = aanliggend / overstaand;
            double slope2 = overstaand / aanliggend;
            double b1 = (yMid - overstaand) - slope1 * aanliggend;
            double b2 = (xMid - aanliggend) - (slope2) * overstaand;

            int curLength = 0;
            int curGap = 0;
            bool onLine = false;
            Point startPoint = new Point(0, 0);

            if (Math.Abs(slope1) >= 1d)
            {
                for (int j = 0; j < InputImage.GetLength(1); j++)
                {
                    int x = (int)(slope2 * (yMid - j) + b2);
                    if (x >= 0 && x < InputImage.GetLength(0))
                    {
                        if (InputImage[x, j].R <= minimumThreshold)
                        {
                            if (!onLine)
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

                                    int oldX = (int)(slope2 * (yMid - j - curGap) + b2);
                                    result.Add(new Tuple<Point, Point>(startPoint, new Point(oldX, j - curGap)));
                                }
                                curGap = 0;
                                curLength = 0;
                                onLine = false;
                            }
                        }
                    }
                    else if (onLine && curLength - curGap >= minLength)
                    {
                        int oldX = (int)(slope2 * (yMid - j + 1 - curGap) + b2);
                        result.Add(new Tuple<Point, Point>(startPoint, new Point(oldX, j + 1 - curGap)));
                        onLine = false;
                    }
                }
                if (onLine && curLength - curGap >= minLength)
                {
                    int j = InputImage.GetLength(1) - 1 - curGap;
                    int x = (int)(slope2 * (yMid - j) + b2);
                    result.Add(new Tuple<Point, Point>(startPoint, new Point(x, j)));
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
                        int oldY = (int)(slope1 * (xMid - j + 1 - curGap) + b1);
                        result.Add(new Tuple<Point, Point>(startPoint, new Point(j + 1 - curGap, oldY)));
                        onLine = false;
                    }
                }
                if (onLine && curLength - curGap >= minLength)
                {
                    int j = InputImage.GetLength(0) - 1 - curGap;
                    int y = (int)(slope1 * (xMid - j) + b1);
                    result.Add(new Tuple<Point, Point>(startPoint, new Point(j, y)));
                }
            }
            return result;
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
        private void DrawCircle(Color[,] image, int X, int Y, Color color, int r)
        {
            for (int i = 0; i < 2 * r + 1; i++)
            {
                for (int j = 0; j < 2 * r + 1; j++)
                {
                    double relX = i - r;
                    double relY = j - r;
                    if (Math.Abs(relX * relX + relY * relY - r * r) < r)
                    {
                        int newX = (int)(relX + X);
                        int newY = (int)(relY + Y);
                        if (newX >= 0 && newX < image.GetLength(0) && newY >= 0 && newY < image.GetLength(1))
                            image[(int)(relX + X), (int)(relY + Y)] = color;
                    }
                }
            }
        }

        private List<Tuple<int, int>> FindCrossings(Color[,] InputImage, List<Tuple<int, int>> locations)
        {
            double nAng = (double)numericUpDownnAng.Value;
            double nRad = (double)numericUpDownnRad.Value;
            var crossings = new List<Tuple<int, int>>();
            for (int i = 0; i < locations.Count; i++)
            {
                for (int j = i + 1; j < locations.Count; j++)
                {
                    int xMid = InputImage.GetLength(0) / 2;
                    int yMid = InputImage.GetLength(1) / 2;
                    double rMax = Math.Sqrt((xMid * xMid) + (yMid * yMid));

                    var point1 = new Tuple<int, int>(locations[i].Item1, locations[i].Item2);
                    double radians1 = point1.Item1 / nRad * Math.PI;
                    double length1 = ((nAng / 2d) - point1.Item2) / nAng * rMax * 2d;

                    double aanliggend1 = Math.Cos(radians1) * length1;
                    double overstaand1 = Math.Sqrt(length1 * length1 - aanliggend1 * aanliggend1) * (length1 < 0 ? -1 : 1);
                    double slope1 = aanliggend1 / overstaand1;
                    double b1 = (yMid - overstaand1) - slope1 * aanliggend1;

                    var point2 = new Tuple<int, int>(locations[j].Item1, locations[j].Item2);
                    double radians2 = point2.Item1 / nRad * Math.PI;
                    double length2 = ((nAng / 2d) - point2.Item2) / nAng * rMax * 2d;

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

        private static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }


        private Color[,] ApplyEdgeSharpening(Color[,] InputImage, Color[,] blurImage, double scalar, int mt = 0)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];
            int sqr = (int)Math.Sqrt(1);
            int fromX = (mt % sqr) * InputImage.GetLength(0) / sqr;
            int toX = ((mt % sqr) + 1) * InputImage.GetLength(0) / sqr;
            int fromY = (int)(mt / sqr) * InputImage.GetLength(1) / sqr;
            int toY = ((int)(mt / sqr) + 1) * InputImage.GetLength(1) / sqr;
            for (int x = fromX; x < toX; x++)
            {
                for (int y = fromY; y < toY; y++)
                {
                    int blurR = blurImage[x, y].R;
                    int blurG = blurImage[x, y].G; ;
                    int blurB = blurImage[x, y].B; ;

                    Color normal = InputImage[x, y];
                    int normalR = normal.R;
                    int normalG = normal.G;
                    int normalB = normal.B;

                    int newR = Clamp((int)(((1 + scalar) * normalR) - (scalar * blurR)), 0, 255);
                    int newG = Clamp((int)(((1 + scalar) * normalG) - (scalar * blurG)), 0, 255);
                    int newB = Clamp((int)(((1 + scalar) * normalB) - (scalar * blurB)), 0, 255);

                    res[x, y] = Color.FromArgb(newR, newG, newB);
                }
            }
            return res;
        }
    }

    public class AreaInfo
    {
        public int AreaNumber;
        public Point Min, Max;
        
        public int Perimeter;
        public List<Point> Pixels;

        private int? centroidX;
        private int? centroidY;

        public int Area
        {
            get
            {
                return Pixels.Count;
            }
        }

        public double Compactness
        {
            get
            {
                return Area / (double)(Perimeter * Perimeter);
            }
        }

        public double Circularity
        {
            get
            {
                return 4 * Math.PI * (Area / (double)( Perimeter * Perimeter));
            }
        }
        public int CentroidX {
            get
            {
                if (centroidX == null)
                    centroidX = (int)(Pixels.Select(p => p.X - Min.X).Sum() * (1d / Area));
                return (int)centroidX;
            }
        }
        public int CentroidY
        {
            get
            {
                if (centroidY == null)
                    centroidY = (int)(Pixels.Select(p => p.Y - Min.Y).Sum() * (1d / Area));
                return (int)centroidY;
            }
        }

        public AreaInfo(int areaNumber, Point min, Point max, int perimeter, List<Point> pixels)
        {
            AreaNumber = areaNumber;
            Min = min;
            Max = max;
            Perimeter = perimeter;
            Pixels = pixels;
        }
    }

    public class detectableObject
    {
        String name;
        double Circularity, Compactness, ratio, xCentroid, yCentroid;

        public detectableObject(double Circle, double Compact, double ratio, double xCentroid, double yCentroid)
        {
            this.Circularity = Circle;
            this.Compactness = Compact;
            this.ratio = ratio;
            this.xCentroid = xCentroid;
            this.yCentroid = yCentroid;
        }

        public bool sameObject(AreaInfo input)
        {
            double areaCircularity = input.Circularity;
            double areaCompactness = input.Compactness;

            double areaRatio = (input.Max.X-input.Min.X)/(input.Max.Y-input.Min.Y);
            if (areaRatio < 1)
                areaRatio = 1d / areaRatio;
            double areaXCentroid = input.CentroidX / (input.Max.X - input.Min.X);
            double areaYCentroid = input.CentroidY/ (input.Max.Y - input.Min.Y);

            double error = 0.03d;
            //Check if same object?
            if ((Math.Abs(areaCircularity - this.Circularity) < error) && (Math.Abs(areaCompactness - this.Compactness) < error) && (Math.Abs(areaRatio - ratio) < error) && (Math.Abs(areaXCentroid - xCentroid) < error) && (Math.Abs(areaYCentroid - yCentroid) < error))
                return true;

            return false;
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
