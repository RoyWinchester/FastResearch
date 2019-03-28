﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

/// <summary>
/// Sqlite版本一定要是1.1.1以下
/// </summary>
namespace FastResearch.DatabaseManager
{
    public static class UserDataBase
    {
        public static void InitializeDatabase()
        {
            try
            {
                using (SqliteConnection db =
                    new SqliteConnection("Filename=userdata.db"))
                {
                    db.Open();

                    String TablePaperAreaCommand = "CREATE TABLE IF NOT " +
                                "EXISTS PaperAreas(PaperAreaId INTEGER PRIMARY KEY, " +
                                "PaperArea Text)";
                    SqliteCommand createTable = new SqliteCommand(TablePaperAreaCommand, db);
                    createTable.ExecuteReader();
                    String TablePaperCommand = "CREATE TABLE IF NOT " +
                                "EXISTS Papers (PaperId INTEGER PRIMARY KEY, " +"Paper Text, " + "BelongToPaperArea Text, " + "FOREIGN KEY(BelongToPaperArea) REFERENCES PaperAreas(PaperArea)";
                    createTable = new SqliteCommand(TablePaperCommand, db);
                    createTable.ExecuteReader();
                }
            } catch
            {
                Debug.WriteLine("无法打开数据库");
            }
        }
        public static void addPaperArea(string PaperArea)
        {
            try
            {
                using (SqliteConnection db =
                new SqliteConnection("Filename=userdata.db"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO PaperAreas VALUES (NULL, @PaperArea);";
                    insertCommand.Parameters.AddWithValue("@PaperArea", PaperArea);
                    insertCommand.ExecuteReader();
                    db.Close();
                }
            } catch
            {
                Debug.WriteLine("加入不了数据");
            }
            
        }
        public static void addPaper(string paperArea, string paper)
        {
            try
            {
                using (SqliteConnection db =
                new SqliteConnection("Filename=userdata.db"))
                {
                    db.Open();

                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;
                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = "INSERT INTO Paper VALUES (NULL, @Paper, @BelongToPaperArea);";
                    insertCommand.Parameters.AddWithValue("@BelongToPaperArea", paperArea);
                    insertCommand.Parameters.AddWithValue("@Paper", paper);
                    insertCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch
            {
                Debug.WriteLine("加入不了数据");
            }
        }
        public static List<String> getPaperArea()
        {
            List<String> PaperArea = new List<string>();

            try
            {
                using (SqliteConnection db =
                    new SqliteConnection("Filename=userdata.db"))
                {
                    db.Open();
                    SqliteCommand selectCommand = new SqliteCommand
                        ("SELECT PaperArea from PaperAreas", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();

                    while (query.Read())
                    {
                        PaperArea.Add(query.GetString(0));
                        
                    }

                    db.Close();
                }
            } catch
            {
                Debug.WriteLine("读取不了PaperArea类中的内容");
            }

            return PaperArea;
        }
        public static List<String> GetPaperName(String paperArea)
        {
            List<String> PaperName = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=userdata.db"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Paper FROM Papers WHERE BelongToPaperArea='" + paperArea + "'" , db);
            
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    PaperName.Add(query.GetString(0));
                }

                db.Close();
            }

            return PaperName;
        }

    }
}
