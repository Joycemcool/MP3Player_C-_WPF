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
    /// Interaction logic for EditTags.xaml
    /// </summary>
    public partial class EditTags : UserControl
    {

        string? fileName;
        private bool isPlaying = false;
        private bool isUserDraggingSlider = false;
        private MediaElement Player;
        public EditTags()
        {
            InitializeComponent();
        }


       private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(0.1) };
        private readonly OpenFileDialog MediaOpenDialog = new()
        {
            Title = "Pick a MP3 file",
            Filter = "Media Files (*.mp3,*.mp4)|*.mp3;*.mp4"
        };

        //update slider value by seconds, if file been loaded, the duration of the file is awailable and the user isn't ragging he slider.

    }
}
