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

        private void UpdateImagePreview()
        {

        }

    }
}
