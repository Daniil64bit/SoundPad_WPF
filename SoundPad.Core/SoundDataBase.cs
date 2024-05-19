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


//namespace SoundPad_WPF_8.Sound_DataBase
namespace SO
{
    public class SoundDataBase
    {
        
        public void _Init<T>(T item) { }
        public static int Get_Count()
        {
            SQLiteConnection connection;
            try
            {
                int Count = 0;
                string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string tmp = parentDir.Remove(parentDir.Length - 10, 10);
                string absolutePath = Path.Combine(tmp, relativePath);
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
                string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string tmp = parentDir.Remove(parentDir.Length - 10, 10);
                string absolutePath = Path.Combine(tmp, relativePath);
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
                List<List<string>> Sounds = new List<List<string>>();
                string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string tmp = parentDir.Remove(parentDir.Length - 10, 10);
                string absolutePath = Path.Combine(tmp, relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);

                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"SELECT * FROM Sound_DB";
                string soundLink = null, soundKey = null;
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            soundLink = rdr.GetString(0);
                            LinkList.Add(soundLink);
                            soundKey = rdr.GetString(1);
                            KeyList.Add(soundKey);
                        }
                    }
                }
                Sounds.Add(LinkList);
                Sounds.Add(KeyList);
                return Sounds;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
            return null;
        }
        public static void Save_Sound(string sound_link, string sound_key, int sound_id)
        {
            SQLiteConnection connection;
            try
            {
                string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string tmp = parentDir.Remove(parentDir.Length - 10, 10);
                string absolutePath = Path.Combine(tmp, relativePath);
                string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);

                connection = new SQLiteConnection(connectionString);
                connection.Open();
                string sql = $"INSERT INTO \"Sound_DB\" (Sound_Link, Sound_Key, Sound_ID) VALUES(\"{sound_link}\", \"{sound_key}\", \"{sound_id}\");";
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
                string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
                var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
                string tmp = parentDir.Remove(parentDir.Length - 10, 10);
                string absolutePath = Path.Combine(tmp, relativePath);
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
