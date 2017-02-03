using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageFilterApp
{
    public partial class MainWindow : Window
    {
        public string ImageFilePath { get; set; }
        public string FilePathOfWorkingDirectory
        {
            get { return System.IO.Path.GetDirectoryName(ImageFilePath); }
        }

        public MainWindow()
        {
            InitializeComponent();
        }


        private void ChangeImageColors(ColorModifier newColorInfo)
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
                            System.Drawing.Color newColor = GetNewPixelColor(oldPixelColor, newColorInfo);

                            previewImageBitMap.SetPixel(xCoordinate, yCoordinate, newColor);
                        }
                    }

                    UpdateImagePreview(previewImageBitMap);
                    return previewImageBitMap;
                }
            }
        }

        public BitmapImage ConvertBitMapToImageSource(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void UpdateImagePreview(Bitmap previewImageBitMap)
        {
            newImagePreview.Source = ConvertBitMapToImageSource(previewImageBitMap);
        }


        private void OpenFileBrowserToSelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            GetFilePathOfImage(openFileDialog);
            SetImageFromFile();
        }

        private void GetFilePathOfImage(OpenFileDialog openFileDialog1)
        {
            ImageFilePath = openFileDialog1.FileName;
        }

        private void SetImageFromFile()
        {
            imagePreview.Source = new BitmapImage(new Uri(ImageFilePath));
            newImagePreview.Source = new BitmapImage(new Uri(ImageFilePath));
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

        private void SaveImageCopy(object sender, RoutedEventArgs e)
        {
            string newImageFilePath = ImageFilePath;

            while (File.Exists(newImageFilePath))
            {
                newImageFilePath = FilePathOfWorkingDirectory + @"\" + System.IO.Path.GetFileNameWithoutExtension(newImageFilePath) + "-COPY" + System.IO.Path.GetExtension(newImageFilePath);
            }

            // create a new file based on the bitmap in the right box
            // this is the last step to be "working"
        }

        public void applyToImageButton_Click(object sender, RoutedEventArgs e)
        {
            int red = Convert.ToInt32(sliderRed.Value);
            int green = Convert.ToInt32(sliderGreen.Value);
            int blue = Convert.ToInt32(sliderBlue.Value);

            ColorModifier newColorModifier = new ColorModifier(red, green, blue);
            ChangeImageColors(newColorModifier);
        }



    }
}
