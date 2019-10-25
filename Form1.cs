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

        // Prewitt operators ///////////////////////////////////////////////////////
        readonly double[,] prewittXKernel = new double[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        readonly double[,] prewittYKernel = new double[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
        readonly double prewittScalar = 1d / 6;
        ////////////////////////////////////////////////////////////////////////////

        public INFOIBV()
        {
            InitializeComponent();
            comboBoxTask.SelectedIndex = 0;
        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
           if (openImageDialog.ShowDialog() == DialogResult.OK)             // Open File Dialog
            {
                string file = openImageDialog.FileName;                     // Get the file name
                imageFileName.Text = file;                                  // Show file name
                if (InputImage != null) InputImage.Dispose();               // Reset image
                InputImage = new Bitmap(file);                              // Create new Bitmap from file
                if (InputImage.Size.Height <= 0 || InputImage.Size.Width <= 0 ||
                    InputImage.Size.Height > 512 || InputImage.Size.Width > 512) // Dimension check
                    MessageBox.Show("Error in image dimensions (have to be > 0 and <= 512)");
                else
                    pictureBox1.Image = (Image) InputImage;                 // Display input image
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
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

            switch(comboBoxTask.SelectedIndex)
            {
                case 0:
                    // Thresholding
                    Image = ApplyThresholding(Image, 200);
                    break;
                case 1:
                    // Contrast Boost
                    var histo = ContrastHistogram(Image);
                    Image = contrastAdjustment(Image);
                    break;
                case 2:
                    // Edge Detection
                    Image = ApplyEdgeDetection(Image, prewittXKernel, prewittYKernel, prewittScalar);
                    Image = InvertImage(Image);
                    break;
                case 3:
                    // Opening (erosion -> dilation)
                    int kernelDimensionSize = 3;
                    int?[,] kernel = CreateStructure(kernelDimensionSize);
                    var erode = Erode(Image, kernel);
                    Image = Dilate(erode, kernel);
                    break;
                case 4:
                    // Closing (dilation -> erosion)
                    kernelDimensionSize = 3;
                    kernel = CreateStructure(kernelDimensionSize);
                    var dilate = Dilate(Image, kernel);
                    Image = Erode(dilate, kernel);
                    break;
                case 5:
                    // Line Detection

                    break;
                case 6:
                    // Region Detection
                    Image = RegionDetection(Image);
                    break;
                case 7:
                    // Object Detection

                    break;
                default:
                    return;
            }

            // Copy array to output Bitmap
            for (int x = 0; x < InputImage.Size.Width; x++)
            {
                for (int y = 0; y < InputImage.Size.Height; y++)
                {
                    OutputImage.SetPixel(x, y, Image[x, y]);               // Set the pixel color at coordinate (x,y)
                }
            }
            
            pictureBox2.Image = (Image)OutputImage;                         // Display output image
            progressBar.Visible = false;                                    // Hide progress bar
        }

        private Color[,] InvertImage(Color[,] InputImage)
        {
            var res = new Color[InputImage.GetLength(0), InputImage.GetLength(1)];

            for(int i = 0; i < InputImage.GetLength(0); i++)
            {
                for (int j = 0; j < InputImage.GetLength(1); j++)
                {
                    var startColor = InputImage[i, j];
                    res[i, j] = Color.FromArgb(255 - startColor.R, 255 - startColor.G, 255 - startColor.B);
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
                    Color pixelColor = InputImage[x, y];
                    Color updatedColor = Color.Black;
                    if (pixelColor.R >= threshholdValue || pixelColor.G >= threshholdValue || pixelColor.B >= threshholdValue)
                        updatedColor = Color.White;
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

        private Color[,] contrastAdjustment(Color[,] InputImage, int mt = 0)
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

                    res[x, y] = Color.FromArgb((int)Math.Sqrt(redX + redY), (int)Math.Sqrt(greenX + greenY), (int)Math.Sqrt(blueX + blueY));
                }
            }
            return res;
        }

        public int?[,] CreateStructure(int size, int? value = 3, bool grayscale = true)
        {
            var result = new int?[size, size];
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

        private Color[,] RegionDetection(Color[,] image)
        {
            throw new NotImplementedException();
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

    }
}
