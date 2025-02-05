﻿using System.Data;
using System.Data.SQLite;
using System.Globalization;
namespace HabitTracker.BrozDa
{
    internal class DatabaseManager
    {
        private string _connectionString = @"Data Source=habit-tracker.sqlite;Version=3;";
        
        public DatabaseManager()
        {
            
        }
        public void CreateNewTable(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) { 
                connection.Open();
                string sql = $"CREATE TABLE {tableName} (" +
                             $"ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                             $"Date varchar(255), " +
                             $"Glasses varchar(255)" +
                             $");";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool CheckIfTableExists(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = $"SELECT name " +
                             $"FROM sqlite_schema " +
                             $"WHERE type ='table' AND name ='{tableName}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection); 
                
                try
                {
                    SQLiteDataReader output = cmd.ExecuteReader();
                    return output.HasRows;
                }
                catch (Exception ex) {
                    Console.WriteLine("Exception occured in DatabaseManager.DoesTableExist()");
                    Console.WriteLine(ex.ToString());
                }

                connection.Close();
                
            }
            return true;
        }
        public List<string> GetListOfTables()
        {
            List<string> tables = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT name " +
                                   $"FROM sqlite_schema " +
                                   $"WHERE type ='table';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                try
                {
                    SQLiteDataReader output = cmd.ExecuteReader();
                    while (output.Read()) {
                        tables.Add(output.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured in DatabaseManager.DoesTableExist()");
                    Console.WriteLine(ex.ToString());
                }
                connection.Close();
            }
            return tables;
        }
        public List<string> GetTableColumnNames(string tableName)
        {
            List<string> columns = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = $"SELECT name FROM pragma_table_info('{tableName}');";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = command.ExecuteReader();

                while (output.Read())
                {
                    columns.Add(output.GetString(0));
                }
                connection.Close();
            }
            return columns;
        }
        public List<DatabaseRecord> GetTableRecords(string tableName)
        {
            List<DatabaseRecord> records = new List<DatabaseRecord>();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string tableExist = $"SELECT * FROM {tableName};";
                SQLiteCommand cmd = new SQLiteCommand(tableExist, connection);
                SQLiteDataReader output = cmd.ExecuteReader();

                while (output.Read())
                {
                    records.Add(new DatabaseRecord(output.GetInt32(0), output.GetString(1), output.GetString(2)));
                }

                connection.Close();

            }
            return records;
        }
        public DatabaseRecord GetRecord(int ID)
        {
            DatabaseRecord record = new DatabaseRecord();

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM WaterIntake " +
                             $"WHERE ID='{ID}';";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                SQLiteDataReader output = cmd.ExecuteReader();
                while (output.Read())
                {
                    record = new DatabaseRecord(output.GetInt32(0), output.GetString(1), output.GetString(2));

                }

                connection.Close();
            }
            return record;
        }
        public void InsertRecord(DatabaseRecord record, string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString)) 
            {
                connection.Open();
                string sql = $"INSERT INTO WaterIntake (Date, Glasses) " +
                             $"VALUES ('{record.Date}', '{record.Volume}');";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();
            
                connection.Close();
            }
        }
        public void UpdateRecord(DatabaseRecord UpdatedRecord, string table) {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"UPDATE {table} " +
                             $"SET Date='{UpdatedRecord.Date}', Glasses='{UpdatedRecord.Volume}' " +
                             $"WHERE ID={UpdatedRecord.ID};";
                SQLiteCommand cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }

        }


    }
}
