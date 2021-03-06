﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageFilterApp
{
    public partial class MainWindow : Window
    {
        public string ImageFilePath { get; set; }
        public Bitmap ModifiedBitmap { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }


        public void ChangeImageColorsAndUpdateImage(ColorModifier newColorInfo)
        {
            if (ImageFilePath != null)
            {
                byte[] imageInBytes = File.ReadAllBytes(ImageFilePath);

                using (MemoryStream ms = new MemoryStream(imageInBytes))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                    {
                        Bitmap previewImageBitMap = new Bitmap(img);

                        for (int xCoordinate = 0; xCoordinate < previewImageBitMap.Width; xCoordinate++)
                        {
                            for (int yCoordinate = 0; yCoordinate < previewImageBitMap.Height; yCoordinate++)
                            {
                                System.Drawing.Color oldPixelColor = previewImageBitMap.GetPixel(xCoordinate, yCoordinate);
                                System.Drawing.Color newPixelColor = GetNewPixelColor(oldPixelColor, newColorInfo);

                                previewImageBitMap.SetPixel(xCoordinate, yCoordinate, newPixelColor);
                            }
                        }

                        UpdateImagePreview(previewImageBitMap);
                        ModifiedBitmap = previewImageBitMap;
                    }
                }
            }

        }

        public BitmapImage ConvertBitmapToImageSource(Bitmap sourceBitmap)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)sourceBitmap).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public void UpdateImagePreview(Bitmap previewImageBitMap)
        {
            newImagePreview.Source = ConvertBitmapToImageSource(previewImageBitMap);
        }

        public System.Drawing.Color GetNewPixelColor(System.Drawing.Color oldColor, ColorModifier colorModifier)
        {
            int newRed = Convert.ToInt32(oldColor.R + (colorModifier.Red * 2.5));
            int newGreen = Convert.ToInt32(oldColor.G + (colorModifier.Green * 2.5)); ;
            int newBlue = Convert.ToInt32(oldColor.B + (colorModifier.Blue * 2.5)); ; ;

            if (newRed > 255)
                newRed = 255;
            if (newRed < 0)
                newRed = 0;
            if (newGreen > 255)
                newGreen = 255;
            if (newGreen < 0)
                newGreen = 0;
            if (newBlue > 255)
                newBlue = 255;
            if (newBlue < 0)
                newBlue = 0;

            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(newRed, newGreen, newBlue);

            return newColor;

        }

        public void applyToImageButton_Click(object sender, RoutedEventArgs e)
        {
            int red = Convert.ToInt32(sliderRed.Value);
            int green = Convert.ToInt32(sliderGreen.Value);
            int blue = Convert.ToInt32(sliderBlue.Value);

            ColorModifier newColorModifier = new ColorModifier(red, green, blue);
            ChangeImageColorsAndUpdateImage(newColorModifier);
        }


        public void OpenFileBrowserAndSetImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SetFilePathOfImage(openFileDialog);
                SetImagesFromFile();
            }
        }

        public void SetFilePathOfImage(OpenFileDialog openFileDialog1)
        {
            ImageFilePath = openFileDialog1.FileName;
        }

        public void SetImagesFromFile()
        {
            imagePreview.Source = new BitmapImage(new Uri(ImageFilePath));
            newImagePreview.Source = new BitmapImage(new Uri(ImageFilePath));
        }

        public void SaveImageCopy(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "jpg";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                if (ModifiedBitmap != null)
                {
                    ModifiedBitmap.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                }
            }
        }
    }
}
