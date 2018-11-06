﻿using System;
using System.IO;
using Android.App;
using Android.Content;
using SQLite;
using Plugin.Settings;

namespace ReLearn
{
    static class DataBase
    {
        const string Statistics_DB = "database_statistics.db3"; 
        const string English_DB = "database_words.db3"; 
        const string Flags_DB = "database_image.db3";

        public readonly static SQLiteConnection Languages = Connect(English_DB);
        public readonly static SQLiteConnection Images = Connect(Flags_DB);
        public readonly static SQLiteConnection Statistics = Connect(Statistics_DB);

        public static TableNamesLanguage TableNameLanguage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameLanguages.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameLanguages.ToString(), TableNamesLanguage.Popular_Words.ToString());
                    Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameLanguages.ToString(), null), out TableNamesLanguage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameLanguages.ToString(), value.ToString());
            }
        }

        public static TableNamesImage TableNameImage
        {
            get
            {
                if (String.IsNullOrEmpty(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameImage.ToString(), null)))
                    CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameImage.ToString(), TableNamesImage.Flags.ToString());
                Enum.TryParse(CrossSettings.Current.GetValueOrDefault(DBSettings.DictionaryNameImage.ToString(), null), out TableNamesImage name);
                return name;
            }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(DBSettings.DictionaryNameImage.ToString(), value.ToString());
            }
        }

        static SQLiteConnection Connect(string nameDB)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nameDB);
            return new SQLiteConnection(databasePath);
        }

        static void InstallDB(string FileName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, FileName);
            if (!File.Exists(path))
            {
                Context context = Application.Context;
                using (var dbAssetStream = context.Assets.Open($"Database/{FileName}"))
                {
                    using (var dbFileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        var buffer = new byte[1024];
                        int length;
                        while ((length = dbAssetStream.Read(buffer, 0, buffer.Length)) > 0)
                            dbFileStream.Write(buffer, 0, length);
                        dbFileStream.Flush();
                    }
                }
            }
        }

        public static void InstallDatabaseFromAssets()
        {
            InstallDB(Statistics_DB);
            InstallDB(English_DB);
            InstallDB(Flags_DB); 
        }
    }
}