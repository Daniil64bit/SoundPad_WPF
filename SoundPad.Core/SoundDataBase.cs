using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using SoundPad_WPF_8;
using System.Windows;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;


//namespace SoundPad_WPF_8.Sound_DataBase
namespace Sound_DataBase
{
    public class SoundDataBase
    {
        public static int SoundNumber = 0;
        public static int ID = 0;
        public static string GetSilentSoundPath()
        {
            string relativePath = @"..\..\..\SoundPad.Core\SoundResource\silent.wav";
            string absolutePath = Path.GetFullPath(relativePath);
            return absolutePath;
        }
        public static List<string> GetDefault()
        {
            string relativePath = @"..\..\..\SoundResource";
            string absolutePath = Path.GetFullPath(relativePath);

            try
            {
                return Directory.GetFiles(absolutePath).Select(x => absolutePath + "\\" + x).ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }

        }
        public static int Get_Count()
        {
            SQLiteConnection connection;
            try
            {
                int Count = 0;
                string relativePath = @"..\..\..\SoundPad_DB.db";
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"SELECT Sound_Count FROM \"Sound_Count_DB\";";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Count = rdr.GetInt32(0);
                        }
                    }
                }
                return Count;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
            return 0;
        }
        public static void Update_Count(int count)
        {
            SQLiteConnection connection;
            try
            {
                string relativePath = @"..\..\..\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"DELETE FROM \"Sound_Count_DB\";";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                sql = $"INSERT INTO \"Sound_Count_DB\" (Sound_Count) VALUES(\"{count}\");";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
        }
        public static List<List<string>> Get_Data()
        {
            SQLiteConnection connection;
            try
            {
                List<string> LinkList = new List<string>();
                List<string> KeyList = new List<string>();
                List<string> SoundID = new List<string>();
                List<List<string>> Sounds = new List<List<string>>();
                string relativePath = @"..\..\..\SoundPad_DB.db";
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"SELECT * FROM Sound_DB";
                string soundLink = null, soundKey = null, soundID = null;
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            soundLink = rdr.GetString(0);
                            LinkList.Add(soundLink);
                            soundKey = rdr.GetString(2);
                            KeyList.Add(soundKey);
                            soundID = Convert.ToString(rdr.GetInt32(3));
                            SoundID.Add(soundID);
                        }
                    }
                }
                Sounds.Add(LinkList);
                Sounds.Add(KeyList);
                Sounds.Add(SoundID);
                return Sounds;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
            return null;
        }
        public static void Create_TempSound(List<string> SoundID)
        {
            SQLiteConnection connection;
            try
            {
                SoundNumber++;
                string relativePath = @"..\..\..\SoundPad_DB.db";
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string tempData;
                string sql = $"SELECT Sound_Data FROM Sound_DB WHERE Sound_ID = {Convert.ToInt32(SoundID[ID])}";
                string outputFilePath = @"..\..\..\TempSounds\Temp_Sound_" + Convert.ToString(SoundNumber) + ".mp3";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            tempData = rdr.GetString(0);
                            byte[] byteArray = Convert.FromBase64String(tempData);
                            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                            {
                                fs.Write(byteArray, 0, byteArray.Length);
                            }
                        }
                    }
                }
                ID++;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
        }
        public static void Save_Sound(string sound_link, string sound_key, int sound_id)
        {
            SQLiteConnection connection;
            try
            {
                string relativePath = @"..\..\..\SoundPad_DB.db";
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);
                byte[] soundData;
                using (FileStream fs = new FileStream(sound_link, FileMode.Open, FileAccess.Read))
                {
                    soundData = File.ReadAllBytes(sound_link);
                }
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"INSERT INTO \"Sound_DB\" (Sound_Name, Sound_Data, Sound_Key, Sound_ID) VALUES(\"{sound_link}\", \"{Convert.ToBase64String(soundData)}\", \"{sound_key}\", \"{sound_id}\");";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
        }
        public static void Delete_Sound()
        {
            SQLiteConnection connection;
            try
            {
                string relativePath = @"..\..\..\SoundPad_DB.db";
                string absolutePath = Path.GetFullPath(relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);

                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"DELETE FROM \"Sound_DB\";";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
        }
    }
    public static class Test
    {
        public static bool one = true;
    }
}
