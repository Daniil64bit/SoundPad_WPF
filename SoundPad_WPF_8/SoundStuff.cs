using SoundPad_WPF_8.HotKeys;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NAudio.CoreAudioApi;
using System.Windows.Documents;


namespace SoundPad_WPF_8
{

    public class SoundStuff : Grid
    {
        private int myID;
        private static int _ID = 0;
        private string _link = "C:\\Users\\Danill64bit\\Documents\\GitHub\\SoundPad_WPF\\Sounds\\silent.wav";
        private Key key;
        public static List<string> HotKeyLinks = new List<string>();
        public static List<Key> HotKeys = new List<Key>();
        public static int ID { get => _ID; set => _ID = value; }
        public int MyID { get => myID; set => myID = value; }
        public string Link { get => _link; set => _link = value; }
        public Key Key { get => key; set => key = value; }
        public static MediaPlayer player = new MediaPlayer();

        public SoundStuff()
        {

        }
        public void IDUp()
        {
            myID = _ID;
            _ID++;
        }
        public static void New_HotKey(Key HotKey, string HotKeyLink = "")
        {
            WaveOut waveOut = new WaveOut() {DeviceNumber = 0 };
            IWavePlayer wavePlayer = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);
            HotkeysManager.AddHotkey(ModifierKeys.None, HotKey, () =>
            {
                AudioFileReader audioFileReader = new AudioFileReader(HotKeyLink);
                audioFileReader.Volume = 0.5f;
                waveOut.Init(audioFileReader);
                Uri MediaSource = new Uri(HotKeyLink);
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                    //player.Stop();
                }
                else if (waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    player.Open(MediaSource);
                    waveOut.Play();
                    //player.Play();
                }
                
            });
            HotKeyLinks.Add(HotKeyLink);
            HotKeys.Add(HotKey);
        }
        public static void Upload_HotKey(List<string> HotKeyData, List<string> HotKeyLinkData)
        {
            Key HotKey = Key.NoName;
            if (HotKeyData != null)
            {
                HotKey = MainWindow.ConvertFromString(HotKeyData[0]);
                HotKeyData.RemoveAt(0);
            }
            string HotKeyLink = "C:\\Users\\Danill64bit\\Documents\\GitHub\\SoundPad_WPF\\Sounds\\silent.wav";
            if (HotKeyLinkData != null)
            {
                HotKeyLink = HotKeyLinkData[0];
                HotKeyLinkData.RemoveAt(0);
            }
            WaveOut waveOut = new WaveOut() { DeviceNumber = 0 };
            IWavePlayer wavePlayer = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);
            HotkeysManager.AddHotkey(ModifierKeys.None, HotKey, () =>
            {
                PlaybackState playback = waveOut.PlaybackState;
                AudioFileReader audioFileReader = new AudioFileReader(HotKeyLink);
                waveOut.Init(audioFileReader);
                audioFileReader.Volume = 0.5f;
                Uri MediaSource = new Uri(HotKeyLink);
                if (playback == PlaybackState.Playing)
                {
                    waveOut.Stop();
                    //player.Stop();
                }
                else if (playback == PlaybackState.Stopped)
                {
                    player.Open(MediaSource);
                    waveOut.Play();
                    //player.Play();
                }
            });
            HotKeyLinks.Add(HotKeyLink);
            HotKeys.Add(HotKey);
        }
        public static void Delete_HotKey(Key HotKey)
        {
            HotkeysManager.RemoveHotkey(ModifierKeys.None, HotKey);
        }
    }
}