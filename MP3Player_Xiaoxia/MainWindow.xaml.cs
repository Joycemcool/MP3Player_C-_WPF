using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace MP3Player_Xiaoxia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string? fileName;
        private bool isPlaying=false;
        private bool isUserDraggingSlider=false;
                
        private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(0.1) };
        private readonly OpenFileDialog MediaOpenDialog = new()
        {
            Title = "Pick a MP3 file",
            Filter = "Media Files (*.mp3,*.mp4)|*.mp3;*.mp4"
        };
        public MainWindow()
        {
            InitializeComponent();

        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new NowPlaying();
        }
        //update slider value by seconds, if file been loaded, the duration of the file is awailable and the user isn't ragging he slider.
        //private void Timer_Tick(object? sender, EventArgs e)
        //{
        //    if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !isUserDraggingSlider)
        //    {
        //        ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
        //        ProgressSlider.Value = Player.Position.TotalSeconds;
        //    }
        //}

        private void MenuFile_Click(object sender, RoutedEventArgs e)
        {
            BtnFile_Click(sender, e);
        }

        private void BtnFile_Click(object sender, RoutedEventArgs e)
        {

            MediaOpenDialog.Multiselect = false;
            bool? success = MediaOpenDialog.ShowDialog();
            if (success == true)
            {
                string path = MediaOpenDialog.FileName;
                fileName = MediaOpenDialog.SafeFileName;
            }
            else
            {
                //didn't pick a file
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new EditTags();
        }
        //#region Media Controls


    }
}
