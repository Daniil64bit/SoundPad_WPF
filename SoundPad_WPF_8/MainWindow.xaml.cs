using Microsoft.Win32;
using NAudio.Wave;
using SoundPad_WPF_8.HotKeys;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SO;
using System.Data.SQLite;
using System.Reflection.Emit;
using System.Collections.Generic;
using NAudio.CoreAudioApi;


namespace SoundPad_WPF_8
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Sound_Count = SoundDataBase.Get_Count();
            var result = SoundDataBase.Get_Data();
            for (int i = 1; i <= Sound_Count; i++)
            {
                New_Sound(result[0], result[1]);
                SoundStuff.Upload_HotKey(result[1], result[0]);
            }
            HotkeysManager.SetupSystemHook();
            Closing += MainWindow_Closing;
        }
        SoundStuff soundStuff = new SoundStuff();
        Key SoundKey = Key.NoName;
        private bool keyNeeded;
        public MediaPlayer player = new MediaPlayer();
        int CurrentID;
        Button CurrentBtn = new Button();
        int Sound_Count = 0;
        BrushConverter bc = new BrushConverter();
        public void New_Sound(List<string> linkData = null, List<string> keyData = null)
        {
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
            Button set_key_btn = new Button() { };
            set_key_btn.Background = Brushes.Green;
            set_key_btn.VerticalAlignment = VerticalAlignment.Top;
            set_key_btn.HorizontalAlignment = HorizontalAlignment.Left;
            set_key_btn.Width = 50;
            set_key_btn.Height = 50;
            set_key_btn.Margin = new Thickness(140, 0, 0, 0);
            if (keyData != null)
            {
                set_key_btn.Content = keyData[0];
            }
            else
            {
                set_key_btn.Content = "Set Key";
            }
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
            var border = new Border();
            soundStuff.Children.Add(delete_btn);
            soundStuff.Width = 427;
            soundStuff.Height = 54;
            soundStuff.Margin = new Thickness(0, 1, 0, 0);
            soundStuff.Background = (Brush)bc.ConvertFrom("#fcb65b");
            border.BorderBrush = (Brush)bc.ConvertFrom("#a294ff");
            border.BorderThickness = new Thickness(2);
            soundStuff.Children.Add(border);
            if (linkData != null)
            {
                soundStuff.Link = linkData[0];
            }
            if (keyData != null)
            {
                soundStuff.Key = ConvertFromString(keyData[0]);
            }
            SoundPack.Children.Add(soundStuff);
            soundStuff.IDUp();
            soundStuff = new SoundStuff();
        }
        public static Key ConvertFromString(string keystr)
        {
            if (keystr != "Set Key")
            {
                return (Key)Enum.Parse(typeof(Key), keystr);
            }
        return Key.None;
        }
        private void New_Sound_Btn_Click(object sender, RoutedEventArgs e)
        {
            New_Sound();
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
                    CurrentID = soundStuff.MyID;
                    string OldLink = soundStuff.Link;
                    OpenFileDialog opf = new OpenFileDialog();
                    if (opf.ShowDialog() == true)
                    {
                        soundStuff.Link = opf.FileName;
                        SoundStuff.Delete_HotKey(soundStuff.Key);
                        foreach (string link in SoundStuff.HotKeyLinks)
                        {
                            if (link == OldLink)
                            {
                                int index = SoundStuff.HotKeyLinks.IndexOf(OldLink);
                                SoundKey = SoundStuff.HotKeys[index];
                                SoundStuff.HotKeys.RemoveAt(index);
                                SoundStuff.HotKeyLinks.RemoveAt(index);
                                break;
                            }
                        }
                        if (soundStuff.MyID == CurrentID)
                        {
                            SoundStuff.New_HotKey(SoundKey, soundStuff.Link);
                        }
                    }
                }
                SoundKey = Key.NoName;
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
                SoundKey = e.Key;
                string constLink = Convert.ToString(CurrentBtn.Content);
                Key OldKey = ConvertFromString(constLink);
                var children = SoundPack.Children.OfType<UIElement>().ToList();
                foreach (SoundStuff soundStuff in children)
                {
                    if (SoundStuff.HotKeys != null && SoundStuff.HotKeys.Contains(OldKey))
                    {
                        SoundStuff.Delete_HotKey(OldKey);
                        foreach (Key key in SoundStuff.HotKeys)
                        {
                            if(key == OldKey)
                            {
                                int index = SoundStuff.HotKeys.IndexOf(OldKey);
                                SoundStuff.HotKeys.RemoveAt(index);
                                SoundStuff.HotKeyLinks.RemoveAt(index);
                                break;
                            }
                        }
                    }
                    else if (SoundStuff.HotKeys.Contains(Key.NoName))
                    {
                        SoundStuff.Delete_HotKey(Key.NoName);
                        foreach (Key key in SoundStuff.HotKeys)
                        {
                            if (key == Key.NoName)
                            {
                                int index = SoundStuff.HotKeys.IndexOf(Key.NoName);
                                SoundStuff.HotKeys.RemoveAt(index);
                                SoundStuff.HotKeyLinks.RemoveAt(index);
                                break;
                            }
                        }
                    }

                    if (soundStuff.MyID == CurrentID)
                    {
                        constLink = soundStuff.Link;
                        SoundStuff.New_HotKey(SoundKey, constLink);
                    }
                }

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
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                if (soundStuff.Children.Contains(btn))
                {
                    if(SoundStuff.HotKeyLinks.Contains(soundStuff.Link) && SoundStuff.HotKeys.Contains(soundStuff.Key))
                    {
                        int index = SoundStuff.HotKeys.IndexOf(soundStuff.Key);
                        SoundStuff.Delete_HotKey(soundStuff.Key);
                        SoundStuff.HotKeys.RemoveAt(index);
                        SoundStuff.HotKeyLinks.RemoveAt(index);
                    }
                    SoundPack.Children.Remove(soundStuff);
                    player.Stop();
                }
            }
            soundStuff = new SoundStuff();
        }
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            SoundDataBase.Delete_Sound();
            Sound_Count = 0;
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            foreach (SoundStuff soundStuff in children)
            {
                Sound_Count++;
                SoundDataBase.Save_Sound(soundStuff.Link, Convert.ToString(soundStuff.Key), soundStuff.MyID);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeysManager.ShutdownSystemHook();
            SoundDataBase.Update_Count(Sound_Count);
        }

        private void Sounds_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(ContentPanel.Visibility == Visibility.Visible)
            {
                ContentPanel.Visibility = Visibility.Hidden;
                New_Sound_Btn.Visibility = Visibility.Hidden;
            }
            else
            {
                ContentPanel.Visibility = Visibility.Visible;
                New_Sound_Btn.Visibility = Visibility.Visible;
            }
        }
    }
}
