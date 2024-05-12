using SoundPad_WPF.Hotkeys;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


namespace SoundPad_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DB = new AppContext();
            HotkeysManager.SetupSystemHook();
            Closing += MainWindow_Closing;
        }
        int y;
        private Size formOriginalSize;
        private Rectangle recMP;
        SoundStuff soundStuff = new SoundStuff();
        Key SoundKey;
        private bool keyNeeded;
        AppContext DB;
        public MediaPlayer player = new MediaPlayer();
        int CurrentID;
        Button CurrentBtn = new Button();
        public void Add_to_list()
        {
            y = 78 + SoundStuff.ID * 75;
            int link_y = 102 + SoundStuff.ID * 75;
            int sound_ID = SoundStuff.ID;
            System.Windows.Controls.Label sound_link = new System.Windows.Controls.Label();
            sound_link.VerticalAlignment = VerticalAlignment.Top;
            sound_link.HorizontalAlignment = HorizontalAlignment.Left;
            sound_link.Width = 80;
            sound_link.Height = 50;
            sound_link.Content = $"Sound link№{SoundStuff.ID + 1}";
            soundStuff.Children.Add(sound_link);
            Button add_sound_btn = new Button();
            add_sound_btn.Background = Brushes.Green;
            add_sound_btn.VerticalAlignment = VerticalAlignment.Top;
            add_sound_btn.HorizontalAlignment = HorizontalAlignment.Left;
            add_sound_btn.Width = 50;
            add_sound_btn.Height = 50;
            add_sound_btn.Margin = new Thickness(80, 0, 0, 0);
            add_sound_btn.Content = "Add Sound";
            add_sound_btn.Click += add_sound_btn_Click;
            soundStuff.Children.Add(add_sound_btn);
            Button set_key_btn = new Button() {};
            set_key_btn.Background = Brushes.Green;
            set_key_btn.VerticalAlignment = VerticalAlignment.Top;
            set_key_btn.HorizontalAlignment = HorizontalAlignment.Left;
            set_key_btn.Width = 50;
            set_key_btn.Height = 50;
            set_key_btn.Margin = new Thickness(140, 0, 0, 0);
            set_key_btn.Content = "Set Key";
            set_key_btn.Click += set_key_btn_Click;
            soundStuff.Children.Add(set_key_btn);
            Button start_btn = new Button();
            start_btn.Background = Brushes.Yellow;
            start_btn.VerticalAlignment = VerticalAlignment.Top;
            start_btn.HorizontalAlignment = HorizontalAlignment.Left;
            start_btn.Width = 50;
            start_btn.Height = 50;
            start_btn.Margin = new Thickness(200, 0, 0, 0);
            start_btn.Content = "Start";
            start_btn.Click += start_btn_Click;
            soundStuff.Children.Add(start_btn);
            Button stop_btn = new Button();
            stop_btn.Background = Brushes.Yellow;
            stop_btn.VerticalAlignment = VerticalAlignment.Top;
            stop_btn.HorizontalAlignment = HorizontalAlignment.Left;
            stop_btn.Width = 50;
            stop_btn.Height = 50;
            stop_btn.Margin = new Thickness(260, 0, 0, 0);
            stop_btn.Content = "Stop";
            stop_btn.Click += stop_btn_Click;
            soundStuff.Children.Add(stop_btn);
            Button delete_btn = new Button();
            delete_btn.Background = Brushes.Red;
            delete_btn.VerticalAlignment = VerticalAlignment.Top;
            delete_btn.HorizontalAlignment = HorizontalAlignment.Left;
            delete_btn.Width = 50;
            delete_btn.Height = 50;
            delete_btn.Margin = new Thickness(320, 0, 0, 0);
            delete_btn.Content = "Delete";
            delete_btn.Click += delete_btn_Click;
            soundStuff.Children.Add(delete_btn);
            soundStuff.Width = 427;
            soundStuff.Height = 54;
            SoundPack.Children.Add(soundStuff);
            soundStuff.IDUp();
            soundStuff = new SoundStuff();
        }
        private void New_Sound_Btn_Click(object sender, RoutedEventArgs e)
        {
            Add_to_list();
        }
        private void start_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    if (soundStuff.Link != "")
                    {
                        Uri MediaSource = new Uri(soundStuff.Link);
                        player.Open(MediaSource);
                        player.Play();
                    }
                }
            }
        }

        private void stop_btn_Click(Object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    if (soundStuff.Link != null)
                    {
                        player.Stop();
                    }
                }
            }
        }

        private void add_sound_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    OpenFileDialog opf = new OpenFileDialog();
                    if (opf.ShowDialog() == true)
                    {
                        soundStuff.Link = opf.FileName;
                    }
                }
            }
        }
        private void set_key_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    CurrentID = soundStuff.MyID;
                    CurrentBtn = btn;
                    //KeysDown(soundStuff);
                }
            }
            keyNeeded = true;
        }
        private void KeysDown(object sender, KeyEventArgs e)
        {
            if (keyNeeded)
            {
                WaveOut waveOut = new WaveOut();
                IWavePlayer wavePlayer = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);

                SoundKey = e.Key;
                
                HotkeysManager.AddHotkey(ModifierKeys.None, e.Key, () =>
                {
                    var children = SoundPack.Children.OfType<UIElement>().ToList();
                    foreach (SoundStuff soundStuff in children)
                    {
                        if (soundStuff.Link != "")
                        {
                            AudioFileReader audioFileReader = new AudioFileReader(soundStuff.Link);
                            waveOut.Init(audioFileReader);
                            waveOut.Play();
                        }
                    }
                });
                var child = SoundPack.Children.OfType<UIElement>().ToList();
                foreach (SoundStuff soundStuff in child)
                {
                    if (soundStuff.MyID == CurrentID)
                    {
                        soundStuff.Key = e.Key;
                    }
                }
                CurrentBtn.Content = Convert.ToString(e.Key);
                keyNeeded = false;
                //KeysDown(soundStuff);
            }
        }
        /*
        private void KeysDown(SoundStuff sound)
        {
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            if (!keyNeeded)
            {
                SoundKey = sound.Key;

                
            }
            /*
            if (!keyNeeded)
            {
                foreach (SoundStuff soundStuff in children)
                {
                    if (soundStuff.Key == sound.Key)
                    {
                        Uri MediaSource = new Uri(soundStuff.Link);
                        player.Open(MediaSource);
                        player.Play();
                    }
                }
            }
           
        }
    */

        private void delete_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    SoundPack.Children.Remove(soundStuff);
                    player.Stop();
                }
            }
            soundStuff = new SoundStuff();
        }
        
        

        private void Save_Sound(string Sound_Link, string Sound_Key, int Sound_ID)
        {
            SoundDB sound = new SoundDB(Sound_Link, Sound_Key, Sound_ID);
            DB.SoundDBs.Add(sound);
            DB.SaveChanges();

            label1.Content = "Saved!";
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                Save_Sound(soundStuff.Link, Convert.ToString(soundStuff.Key), soundStuff.MyID);
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeysManager.ShutdownSystemHook();
        }
    }
}
