using Microsoft.Win32;
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

namespace MP3Player_Xiaoxia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP3 Source Files | *.mp3";
            fileDialog.Title = "Pick a MP3 file";
            fileDialog.Multiselect = false;
            bool? success = fileDialog.ShowDialog();
            if(success == true) 
            {
                string path = fileDialog.FileName;
                fileName = fileDialog.SafeFileName;
                LblTest.Content = fileName;
                Player.Source = new Uri(path);
                BtnPlay.IsEnabled = true;
            }
            else
            {
                //didn't pick a file
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
        }



        //Play and pause funcion


    }
}
