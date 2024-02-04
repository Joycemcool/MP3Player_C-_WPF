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

            timer.Tick += Timer_Tick;
            timer.Start();
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

            MediaOpenDialog.Multiselect = false;
            bool? success = MediaOpenDialog.ShowDialog();
            if(success == true) 
            {
                string path = MediaOpenDialog.FileName;
                fileName = MediaOpenDialog.SafeFileName;
                LblTest.Content = fileName;
                Player.Source = new Uri(path);
                BtnPlay.IsEnabled = true;
                Player.Play();
                Show_Properties(sender, e);
            }
            else
            {
                //didn't pick a file
            }
        }
        #region Media Controls
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(Player?.Source != null)
            {
                Player.Play();
                isPlaying = true;
                //need a default value for label
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
        //why this method doesn't work
        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }
        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblTime.Content = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss");
       
        }

        private void ProgressSlider_DragCompleted_1(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        //Play and pause funcion
        #endregion
        #region Properties
        private void Show_Properties(object sender, RoutedEventArgs e)
        {
            //string MP3_Title = 
            var tfile = TagLib.File.Create(MediaOpenDialog.FileName);
            StringBuilder sb = new();
            var tags = tfile.Tag.Title.ToString();
            
            sb.AppendLine(tags);    

            sb.AppendLine("Duration: " + tfile.Properties.Duration.ToString(@"hh\:mm\:ss"));

            if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Audio))
            {
                sb.AppendLine("Audio bitrate: " + tfile.Properties.AudioBitrate);
                sb.AppendLine("Audio sample rate: " + tfile.Properties.AudioSampleRate);
                sb.AppendLine("Audio channels: " + (tfile.Properties.AudioChannels == 1 ? "Mono" : "Stereo"));
            }

            if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Video))
            {
                sb.AppendLine($"Video resolution: {tfile.Properties.VideoWidth} x {tfile.Properties.VideoHeight}");
            }
            TextBlockTags.Text = sb.ToString();
        }

        private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaOpenDialog.FileName != "")
            {
                var tfile = TagLib.File.Create(MediaOpenDialog.FileName);
                StringBuilder sb = new();

                sb.AppendLine("Duration: " + tfile.Properties.Duration.ToString(@"hh\:mm\:ss"));

                if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Audio))
                {
                    sb.AppendLine("Audio bitrate: " + tfile.Properties.AudioBitrate);
                    sb.AppendLine("Audio sample rate: " + tfile.Properties.AudioSampleRate);
                    sb.AppendLine("Audio channels: " + (tfile.Properties.AudioChannels == 1 ? "Mono" : "Stereo"));
                }

                if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Video))
                {
                    sb.AppendLine($"Video resolution: {tfile.Properties.VideoWidth} x {tfile.Properties.VideoHeight}");
                }

                MessageBox.Show(sb.ToString(), "Properties");
            }
        }

        #endregion

    }
}
