﻿using System;
using System.IO;
using SQLite;
using Plugin.Settings;

namespace ReLearn.API.Database
{

    public enum TableNames
    {
        Flags,
        Films,
        ///////////////////////////////////
        My_Directly,
        Home,
        Education,
        Popular_Words,
        ThreeFormsOfVerb,
        ComputerScience,
        Nature
    }

    public static class DataBase
    {
        const string _statistics  = "database_statistics.db3"; 
        const string _english     = "database_words.db3"; 
        const string _flags       = "database_image.db3";

        public static SQLiteConnection Languages;
        public static SQLiteConnection Images;
        public static SQLiteConnection Statistics;

        public static TableNames TableName
        {
            get
            {
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryName.ToString(), TableNames.Popular_Words.ToString()), out TableNames name);
                return name;
            }
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryName.ToString(), value.ToString());
        }

        public static void SetupConnection()
        {
            Languages = Connect(_english);
            Images = Connect(_flags);
            Statistics = Connect(_statistics);
        }

        //public static void InstallDatabaseFromAssets()
        //{
        //    InstallDB(_statistics);
        //    InstallDB(_english);
        //    InstallDB(_flags); 
        //}

        //static void InstallDB(string FileName)
        //{
        //    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    var path = Path.Combine(documentsPath, FileName);
        //    if (!File.Exists(path))
        //    {
        //        Context context = Application.Context;
        //        using (var dbAssetStream = context.Assets.Open($"Database/{FileName}"))
        //        {
        //            using (var dbFileStream = new FileStream(path, FileMode.OpenOrCreate))
        //            {
        //                var buffer = new byte[1024];
        //                int length;
        //                while ((length = dbAssetStream.Read(buffer, 0, buffer.Length)) > 0)
        //                    dbFileStream.Write(buffer, 0, length);
        //                dbFileStream.Flush();
        //            }
        //        }
        //    }
        //}

        static SQLiteConnection Connect(string nameDB) => new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDB));
    }
}