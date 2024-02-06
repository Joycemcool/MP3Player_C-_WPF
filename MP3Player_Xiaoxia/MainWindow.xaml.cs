using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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
        string? filePath;
        private TagLib.File tfile;
        string? mp3Title;
        string? album;
        string? year;
        private bool isPlaying=false;
        private bool isUserDraggingSlider=false;
        private NowPlaying nowPlaying;
        private EditTags editTags;
                
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

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            
            CC.Content = nowPlaying;
            nowPlaying.Title.TagHeader.Content = "title";
            //CC.Content = new NowPlaying();
        }

        private void MenuFile_Click(object sender, RoutedEventArgs e)
        {
            BtnFile_Click(sender, e);
        }

        private void Get_Tags(object sender, RoutedEventArgs e)
        {
            tfile = TagLib.File.Create(MediaOpenDialog.FileName);
            //tfile = TagLib.File.Create(@"C:\Users\joyce\Documents\ComeAlive.mp3");
       
            mp3Title = tfile.Tag.Title.ToString();
        }
        private void BtnFile_Click(object sender, RoutedEventArgs e)
        {

            MediaOpenDialog.Multiselect = false;
            bool? success = MediaOpenDialog.ShowDialog();
            if (success == true)
            {
                filePath = MediaOpenDialog.FileName;
                fileName = MediaOpenDialog.SafeFileName;
                Player.Source = new Uri(filePath);
                Player.Play();
                nowPlaying = new NowPlaying();
                CC.Content = nowPlaying;
                PlayPanel.Visibility = Visibility.Visible;

                Get_Tags(sender, e);
                if(mp3Title != null)
                {
                    nowPlaying.Title.TagValue.Content = mp3Title;
                }



            }
            else
            {
                //didn't pick a file
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            editTags = new EditTags();
            CC.Content = editTags;
            tfile.Dispose( );
        }
        //#region Media Controls

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !isUserDraggingSlider)
            {
                ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = Player.Position.TotalSeconds;
            }
        }

        #region Media Controls
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (Player?.Source != null)
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

        private void Show_Properties(object sender, RoutedEventArgs e)
        {
            var tfile = TagLib.File.Create(MediaOpenDialog.FileName);
            var title = tfile.Tag.Title.ToString();
            var album = tfile.Tag.Album.ToString();
            var year = tfile.Tag.Year.ToString();
            var genre = tfile.Tag.Genres.ToString();


            //Image image = tfile.Tag.Picture;
            //tfile.Save(); 


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



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            
            Player.Source=null;
            mp3Title = editTags.Title.TextBoxMetaValue.Text.ToString();
            tfile.Tag.Title = mp3Title;
            tfile.Save();
        }
    }
}
