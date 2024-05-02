using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Integration
{
    public class Program
    {
        static SQLiteConnection connection;
        static SQLiteCommand command;

        static void Main(string[] args)
        {
            string relativePath = @"C:\Users\11\Desktop\repos\SoundPad_WPF\SoundPad_DB.db";
            var parentDir = Path.GetDirectoryName(AppContext.BaseDirectory);
            string tmp = parentDir.Remove(parentDir.Length - 10, 10);
            string absolutePath = Path.Combine(tmp, relativePath);
            string connectionString = string.Format("Data Source = {0};Version=3; FailIfMissing=False", absolutePath);

            try
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connected!");
                command = new SQLiteCommand(connection)
                {
                    CommandText = "SELECT * FROM \"Sound_DB\";"
                };
                Console.WriteLine("Результат запроса:");
                DataTable data = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(data);
                Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
                foreach (DataRow row in data.Rows)
                {
                    Console.WriteLine($"id = {row.Field<Int64>("id")} name = {row.Field<string>("name")} group = {row.Field<Int64>("group")}");
                }

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
            }
            Console.Read();
            
        }
    }
}
