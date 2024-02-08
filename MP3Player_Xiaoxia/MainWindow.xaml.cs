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
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //switch nowPlaying and edit window
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
 
            CC.Content = nowPlaying;
            nowPlaying.Title.TagHeader.Content = "Title";
            nowPlaying.Album.TagHeader.Content = "Album";
            nowPlaying.Year.TagHeader.Content = "Year";
            nowPlaying.Title.TagValue.Content = mp3Title;
            nowPlaying.Album.TagValue.Content = album;
            nowPlaying.Year.TagValue.Content = year;
        }


        private void Get_Tags(object sender, RoutedEventArgs e)
        {
            tfile = TagLib.File.Create(MediaOpenDialog.FileName);
      
            mp3Title = tfile.Tag.Title.ToString();
            album = tfile.Tag.Album.ToString();
            year = tfile.Tag.Year.ToString();
        }

        //CommandBinding

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MediaOpenDialog.Multiselect = false;
            bool? success = MediaOpenDialog.ShowDialog();
            if (success == true)
            {
                filePath = MediaOpenDialog.FileName;
                fileName = MediaOpenDialog.SafeFileName;
                nowPlaying = new NowPlaying();
                CC.Content = nowPlaying;
                PlayPanel.Visibility = Visibility.Visible;

                Get_Tags(sender, e);
                nowPlaying.Title.TagValue.Content = mp3Title;
                //if (mp3Title != null)
                //{
                //    nowPlaying.Title.TagValue.Content = mp3Title;
                //}

                Player.Source = new Uri(filePath);

                PlayMedia(sender, e);

            }
        }

        private void CanPlayMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Player != null) && (Player.Source != null) ;
        }

        private void PlayMedia(object sender, ExecutedRoutedEventArgs e)
        {
            Player.Play();
            isPlaying = true;
            nowPlaying.Title.TagHeader.Content = "Title";
            nowPlaying.Album.TagHeader.Content = "Album";
            nowPlaying.Year.TagHeader.Content = "Year";
            nowPlaying.Title.TagValue.Content = mp3Title;
            nowPlaying.Album.TagValue.Content = album;
            nowPlaying.Year.TagValue.Content = year;
        }

        private void CanPauseMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void PauseMedia(object sender, ExecutedRoutedEventArgs e)
        {
            Player.Pause();
            //isPlaying= false;
        }

        private void CanStopMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void StopMedia(object sender, ExecutedRoutedEventArgs e)
        {
            Player.Stop();
            //isPlaying = false;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            Player.Source = null;
            editTags = new EditTags();
            BtnSaveTags.Visibility = Visibility.Visible;
            CC.Content = editTags;
            editTags.Title.TagHeader.Content = "Title";
            editTags.Album.TagHeader.Content = "Album";
            editTags.Year.TagHeader.Content = "Year";
            editTags.Title.TagValue.Text = mp3Title;
            editTags.Album.TagValue.Text = album;
            editTags.Year.TagValue.Text = year;
            tfile.Dispose();          
        }
        //#region Media Controls

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !isUserDraggingSlider)
            {
                ProgressSlider.Minimum= 0;
                ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = Player.Position.TotalSeconds;
            }
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
            LblTime.Content = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss"); //number upside down

        }

        private void ProgressSlider_DragCompleted_1(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mp3Title = editTags.Title.TagValue.Text.ToString();
            if(mp3Title.Length > 0)
            {
                tfile.Tag.Title = mp3Title;
                tfile.Save();
            }

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
}
