﻿Simple text :- Can be simply typed → Text
Italic text :- Can be typed between pair of Underscores → _text_
Bold text :- Can be typed between a pair of two Asterisk → **text**
Strikethrough :- Can be typed between a pair of two Tilde → ~~text~~

## Project: MP3 Player - used XAML/C# to create a WPF application 

Max size 1 → # Heading 1
Size 2 → ##Heading 2
Size 3 → ###Heading 3
Size 4 → ####Heading 4
Size 5→ #####Heading 5
Min Size 6 → ######Heading 6

### Discription
(-Just add a dash first and then write a text.Need a space follow the sign)
- Allows users to select and play an MP3 audio file(If you add four spaces or use a tab key, you will create an indented list.)
![image](./images/File-Menu.png)

- On application start, the user can open a file dialog, browse to an MP3 audio file, and open it. 
![image](./images/Open-File-dialog.png)

- When an MP3 has been opened, the user can use either a menu option or toolbar buttons to control the music’s playback state (**Play**, **Pause** or **Stop**)

![image](./images/Media-Menu.png)

![image](./images/Now-Playing-screen.png)

- After a song is selected, song **metadata** (such as Title, Artist, Album, Year) shall be displayed in a “Now Playing” screen. 

- Once a song has been selected, the user should be able to **toggle** the display between the “Now Playing” screen and a tag-editing screen.

![image](./images/Tag-Editor-screen.png)

### Precondition
- Install TagLibSharp in NuGet Manager 
- Taglib is a library for reading and editing audio metadata, commonly known as tags.

### This project includes the following technical specs:
-   WPF application using XAML and C#.
-   Use CommandBindings for the media and application controls (ApplicationCommands (ex. Copy, Paste, Close, Redo, Save),MediaCommands (ex. Play, Stop, Pause, IncreaseVolume, NextTrack))
-   Use at least one User Control (Suggested use: Now Playing and/or Tag Editor screens, now playing)
-   Use at least three layout managers to create an intuitive and flexible user interface.
	- StackPanel, Grid,DockPanel
-   At minimum, the app should be able to read and write the following tag data: Song Title, Album, Year. Other tags can be used as desired.
-   Implement reasonable exception handling to avoid program crashes.
-   Note: The timer and progress slider shown in the screenshots are nice features to include, but optional.

[Reference article :what-is-readme-md-file](https://www.geeksforgeeks.org/what-is-readme-md-file/)

[Reference tutorial :open-file-dialog](https://www.youtube.com/watch?v=Ks9bzPSx7Vs)
[Reference tutorial :wpf-audio-player](https://www.youtube.com/watch?v=B0MXTryR5Cw)
[Reference tutorial :Media Element In WPF](https://www.c-sharpcorner.com/UploadFile/dpatra/media-element-in-wpf/)
[Reference tutorial :Media Player with File Metadata](https://www.youtube.com/watch?v=jARmuKSRsio)
[Reference stackoverflow:Media Player with File Metadata](https://stackoverflow.com/questions/50437358/c-sharp-taglib-set-album-cover-for-mp3)
[Reference tutorial : WPF | Sidebar, multi Window Navigation](https://www.youtube.com/watch?v=FACL4eTZ8uA&t=844s)
[Reference tutorial: How-to: Creating a complete Audio/Video player](https://wpf-tutorial.com/hu/101/audio-video/how-to-creating-a-complete-audio-video-player/)
### Key words:
- Windows Presentation Foundation (WPF) is a UI-rendering framework
- XAML – Extensible Application Markup Language
- 

### Application developing steps:
#### UI 
- Main window, menu, select file and open (title can have an icon)
- Progress bar, play, pause, stop
- toggle side bar
- spend 30 minutes figure out why border doesn't match stackpanel size
#### Backend function
- open file dialogg(disable buttons if no file selected, if stop, need to select file again)
- play, pause, stop function
- Thumb.DragCompleted="ProgressSlider_DragCompleted" not working
