using Microsoft.Win32;
using SoundPad.Core;
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
        }
        int y;
        SoundStuff soundStuff = new SoundStuff();
        public Key SoundKey;
        private bool keyNeeded;
        public MediaPlayer player = new MediaPlayer();

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
                    soundStuff.Key = SoundKey;
                    btn.Content = Convert.ToString(soundStuff.Key);
                    
                }
            }
            keyNeeded = true;
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var children = SoundPack.Children.OfType<UIElement>().ToList();
            if (keyNeeded)
            {
                SoundKey = e.Key;

                keyNeeded = false;
            }
            if (!keyNeeded)
            {
                foreach (SoundStuff soundStuff in children)
                {
                    if (soundStuff.Key == e.Key)
                    {
                        Uri MediaSource = new Uri(soundStuff.Link);
                        player.Open(MediaSource);
                        player.Play();
                    }
                }
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
                    SoundPack.Children.Remove(soundStuff);
                    player.Stop();
                }
            }
            soundStuff = new SoundStuff();
        }
        
        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            Sound_DataBase dataBase = new Sound_DataBase();
            foreach (SoundStuff soundStuff in SoundPack.Children)
            {
                dataBase.Save_Sound(soundStuff.Link, Convert.ToString(soundStuff.Key), soundStuff.MyID);
            }
        }
    }
}
