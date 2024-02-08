using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        private void BtnFile_Click(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                BtnSaveTags.Visibility = Visibility.Hidden;
                OpenCmdExecuted(sender, e);
            }
            catch (FileFormatException ex) {
                Console.WriteLine(ex.Message.ToString());
            }

        }
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nowPlaying != null)
                {
                    CC.Content = nowPlaying;
                    BtnSaveTags.Visibility = Visibility.Hidden;
                    Set_NowPlayingTags();
                    PlayPanel.Visibility = Visibility.Visible;
                }
            } catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (editTags != null)
            {
                Player.Stop();
                PlayPanel.Visibility = Visibility.Hidden;
                isPlaying = false;
                Player.Source = null;//after this edit, set it back
                BtnSaveTags.Visibility = Visibility.Visible;
                CC.Content = editTags;
            }
            if(filePath != null)
            {
                Player.Source = new Uri(filePath);
            }
        }

        private void BtnSaveTags_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player.Source = null;
                Get_Tags(sender, e);
                mp3Title = editTags.Title.TagValue.Text.ToString();
                album = editTags.Album.TagValue.Text.ToString();
                year = editTags.Year.TagValue.Text.ToString();
                if (mp3Title.Length > 0 || album.Length > 0 || year != null)
                {
                    tfile.Tag.Title = mp3Title;
                    tfile.Tag.Album = album;
                    tfile.Tag.Year = UInt32.Parse(year);
                    tfile.Save();
                }
                Set_EditTags();
                Player.Source = new Uri(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }


        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuEdit_Click(object sender, RoutedEventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        //get and set tags for the controls
        private void Get_Tags(object sender, RoutedEventArgs e)
        {
            tfile = TagLib.File.Create(MediaOpenDialog.FileName);      
            mp3Title = tfile.Tag.Title.ToString();
            album = tfile.Tag.Album.ToString();
            year = tfile.Tag.Year.ToString();
        }

        private void Set_NowPlayingTags()
        {
            nowPlaying.Title.TagHeader.Content = "Title";
            nowPlaying.Album.TagHeader.Content = "Album";
            nowPlaying.Year.TagHeader.Content = "Year";
            nowPlaying.Title.TagValue.Content = mp3Title;
            nowPlaying.Album.TagValue.Content = album;
            nowPlaying.Year.TagValue.Content = year;
        }
        private void Set_EditTags()
        {
            editTags.Title.TagHeader.Content = "Title";
            editTags.Album.TagHeader.Content = "Album";
            editTags.Year.TagHeader.Content = "Year";
            editTags.Title.TagValue.Text = mp3Title;
            editTags.Album.TagValue.Text = album;
            editTags.Year.TagValue.Text = year;
            tfile.Dispose();
        }

        //CommandBinding

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BtnSaveTags.Visibility = Visibility.Hidden;
            MediaOpenDialog.Multiselect = false;
            bool? success = MediaOpenDialog.ShowDialog();
            if (success == true)
            {
                filePath = MediaOpenDialog.FileName;
                fileName = MediaOpenDialog.SafeFileName;
                nowPlaying = new NowPlaying();
                editTags = new EditTags();
                Get_Tags(sender, e);
                Set_NowPlayingTags();
                Set_EditTags();
                CC.Content = nowPlaying;
                PlayPanel.Visibility = Visibility.Visible;
                Player.Source = new Uri(filePath);
                PlayMedia(sender, e);
                isPlaying = true;

            }
        }

        private void CanPlayMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Player != null) && (Player.Source != null);
        }

        private void PlayMedia(object sender, ExecutedRoutedEventArgs e)
        {
            BtnPlay_Click(sender, e);
            BtnSaveTags.Visibility= Visibility.Hidden;
            Player.Play();
            isPlaying = true;
        }

        private void CanPauseMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = isPlaying;
            e.CanExecute = true;
        }

        private void PauseMedia(object sender, ExecutedRoutedEventArgs e)
        {
            if (isPlaying)
            {
              Player.Pause();
            }

            isPlaying = false;
        }

        private void CanStopMedia(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void StopMedia(object sender, ExecutedRoutedEventArgs e)
        {
            Player.Stop();
            //isPlaying = false;
        }


        //progressBar setting

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




    }
}
