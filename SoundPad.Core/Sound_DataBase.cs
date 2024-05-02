using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SoundPad.Core
{
    public class Sound_DataBase
    {
        public void Save_Sound(string sound_link, string sound_key, int sound_id)
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
                string sql = $"INSERT INTO \"Sound_Data\" (SoundLink, SoundKey, SoundID) VALUES(\"{sound_link}\", \"{sound_key}\", \"{sound_id}\") ON CONFLICT;";
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
}
