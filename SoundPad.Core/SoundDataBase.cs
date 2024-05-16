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


//namespace SoundPad_WPF_8.Sound_DataBase
namespace SO
{
    public class SoundDataBase
    {
        
        public void _Init<T>(T item) { }
        public static string[] Get_Data()
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
                string sql = $"SELECT * FROM Sound_Data";
                string soundLink = null, soundKey = null;
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            soundLink = rdr.GetString(1);
                            soundKey = rdr.GetString(2);
                            //cmd.ExecuteNonQuery();

                        }
                    }
                }
                return new string[] { soundLink, soundKey };
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
    }
    public static class Test
    {
        public static bool one = true;

    }
}
