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


        private void Colorizer()
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
                            previewImageBitMap.SetPixel(xCoordinate, yCoordinate, System.Drawing.Color.FromArgb(oldPixelColor.R, oldPixelColor.G, 0));
                        }
                    }

                    UpdateImagePreview(previewImageBitMap);
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

        private void UpdateImagePreview(Bitmap previewImageBitMap)
        {
            newImagePreview.Source = ConvertBitMapToImageSource(previewImageBitMap);
        }

        private void SaveImageCopy(object sender, RoutedEventArgs e)
        {

        }
    }
}
