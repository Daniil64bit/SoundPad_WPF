using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        private Size formOriginalSize;
        private Rectangle recMP;
        SoundStuff soundStuff = new SoundStuff();
        Key SoundKey;
        private bool keyNeeded;

        public void Add_to_list()
        {
            y = 78 + SoundStuff.ID * 75;
            int link_y = 102 + SoundStuff.ID * 75;
            int sound_ID = SoundStuff.ID;
            DesrLabel.Content sound_link = new Label();
            sound_link.Location = new Point(1, 25);
            sound_link.Size = new Size(80, 20);
            sound_link.Text = $"Sound link№{SoundStuff.ID + 1}";
            soundStuff.Controls.Add(sound_link);
            Button add_sound_btn = new Button() { BackColor = Color.Green };
            add_sound_btn.Size = new Size(50, 50);
            add_sound_btn.Location = new Point(198, 4);
            add_sound_btn.Text = "Add Sound";
            add_sound_btn.Click += add_sound_btn_Click;
            soundStuff.Controls.Add(add_sound_btn);
            Button set_key_btn = new Button() { BackColor = Color.Green };
            set_key_btn.Size = new Size(50, 50);
            set_key_btn.Location = new Point(138, 4);
            set_key_btn.Text = "Set Key";
            set_key_btn.Click += set_key_btn_Click;
            soundStuff.Controls.Add(set_key_btn);
            Button start_btn = new Button() { BackColor = Color.Yellow };
            start_btn.Size = new Size(50, 50);
            start_btn.Location = new Point(258, 4);
            start_btn.Text = "Start";
            start_btn.Click += start_btn_Click;
            soundStuff.Controls.Add(start_btn);
            Button stop_btn = new Button() { BackColor = Color.Yellow };
            stop_btn.Size = new Size(50, 50);
            stop_btn.Location = new Point(318, 4);
            stop_btn.Text = "Stop";
            stop_btn.Click += stop_btn_Click;
            soundStuff.Controls.Add(stop_btn);
            Button delete_btn = new Button() { BackColor = Color.Red };
            delete_btn.Size = new Size(50, 50);
            delete_btn.Location = new Point(378, 4);
            delete_btn.Text = "Delete";
            delete_btn.Click += delete_btn_Click;
            soundStuff.Controls.Add(delete_btn);
            soundStuff.Size = new Size(427, 54);
            soundStuff.Location = new Point(361, y);
            SoundPack.Controls.Add(soundStuff);
            soundStuff.IDUp();
            soundStuff = new SoundStuff();
        }

        private void New_Sound_Btn_Click(object sender, EventArgs e)
        {
            Add_to_list();
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (SoundStuff soundStuff in SoundPack.Controls)
            {
                if (soundStuff.Controls.Contains(btn))
                {
                    if (this.soundStuff.Link != null)
                    {
                        Test_Sound.URL = soundStuff.Link;
                        Test_Sound.Ctlcontrols.play();
                    }
                }
            }
        }

        private void stop_btn_Click(Object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (SoundStuff soundStuff in SoundPack.Controls)
            {
                if (soundStuff.Controls.Contains(btn))
                {
                    if (this.soundStuff.Link != null)
                    {
                        Test_Sound.Ctlcontrols.stop();
                    }
                }
            }
        }

        private void add_sound_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (SoundStuff soundStuff in SoundPack.Controls)
            {
                if (soundStuff.Controls.Contains(btn))
                {
                    OpenFileDialog opf = new OpenFileDialog();
                    if (opf.ShowDialog() == DialogResult.OK)
                    {
                        soundStuff.Link = opf.FileName;
                    }
                }
            }
        }
        private void set_key_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (SoundStuff soundStuff in SoundPack.Controls)
            {
                if (soundStuff.Controls.Contains(btn))
                {
                    soundStuff.Key = SoundKey;
                    btn.Text = Convert.ToString(soundStuff.Key);
                }
            }
            keyNeeded = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyNeeded)
            {
                SoundKey = e.KeyCode;

                keyNeeded = false;
            }
            if (!keyNeeded)
            {
                foreach (SoundStuff soundStuff in SoundPack.Controls)
                {
                    if (soundStuff.Key == e.KeyCode)
                    {
                        Test_Sound.URL = soundStuff.Link;
                        Test_Sound.Ctlcontrols.play();
                    }
                }
            }
        }
        private void delete_btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            foreach (SoundStuff soundStuff in SoundPack.Controls)
            {
                if (soundStuff.Controls.Contains(btn))
                {
                    SoundPack.Controls.Remove(soundStuff);
                    Test_Sound.Ctlcontrols.stop();
                }
            }
            soundStuff = new SoundStuff();
        }

        private void DataBaseCall()
        {
            var connection = new SQLiteConnection("E:\\Data Bases\\Test.db");
            connection.Open();
            label1.Text = "Connected!";
        }

        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            formOriginalSize = this.Size;
            recMP = new Rectangle(Test_Sound.Location, Test_Sound.Size);
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            Resize_Control(Test_Sound, recMP);
        }

        private void Resize_Control(Control c, Rectangle r)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(formOriginalSize.Height);
            int newX = (int)(r.X * xRatio);
            int newY = (int)(r.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);

        }

        private void Select_video_Click(object sender, EventArgs e)
        {

        }

        private void Sound_List_Click(object sender, EventArgs e)
        {
            if (Test_Sound.Visible == false)
            {
                Test_Sound.Visible = true;
            }
            else
            {
                Test_Sound.Visible = false;
            }
        }
    }
}
