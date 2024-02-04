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
        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += Timer_Tick;
        }
        //update slider value by seconds, if file been loaded, the duration of the file is awailable and the user isn't ragging he slider.
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !isUserDraggingSlider)
            {
                ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = Player.Position.TotalSeconds;
            }
        }

        private void MenuFile_Click(object sender, RoutedEventArgs e)
        {
            BtnFile_Click(sender, e);
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
            if(Player?.Source != null)
            {
                Player.Play();
                isPlaying = true;
            }

        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            isPlaying = false;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            isPlaying = false;
        }

        private void ProgressSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
                isUserDraggingSlider = true;                
        }
        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }
        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblTime.Content = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss");
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        //Play and pause funcion


    }
}
