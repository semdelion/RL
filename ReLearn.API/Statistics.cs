﻿using Plugin.Settings;
using ReLearn.API.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReLearn.API
{
    public static class Statistics
    {
        public static int Count
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.Count}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.Count}{DataBase.TableName}", value);
        }
        public static int True
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.True}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.True}{DataBase.TableName}", value);
        }
        public static int False
        {
            get => CrossSettings.Current.GetValueOrDefault($"{DBSettings.False}{DataBase.TableName}", 0);
            set => CrossSettings.Current.AddOrUpdateValue($"{DBSettings.False}{DataBase.TableName}", value);
        }

        public static float GetAverageNumberLearn(List<DBStatistics> Database_NL_and_D)
        {
            if (Database_NL_and_D.Count == 0)
                return Settings.StandardNumberOfRepeats;
            float avg_numberLearn_stat = (float)Database_NL_and_D.Sum(
                r => r.NumberLearn > Settings.StandardNumberOfRepeats ?
                Settings.StandardNumberOfRepeats : r.NumberLearn) / (float)Database_NL_and_D.Count;
            return avg_numberLearn_stat;
        }

        public static async Task Add(List<DatabaseWords> WordDatabase, int CurrentWordNumber, int answer)
        {
            WordDatabase[CurrentWordNumber].NumberLearn += answer;

            int value = WordDatabase[CurrentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats ?
                        Settings.MaxNumberOfRepeats : WordDatabase[CurrentWordNumber].NumberLearn < 0 ? 
                        0 : WordDatabase[CurrentWordNumber].NumberLearn;

            await DatabaseWords.Update(WordDatabase[CurrentWordNumber].Word, value);
        }
        public static async Task Add(List<DatabaseImages> ImagesDatabase, int CurrentWordNumber, int answer)
        {
            ImagesDatabase[CurrentWordNumber].NumberLearn += answer;

            int value = ImagesDatabase[CurrentWordNumber].NumberLearn > Settings.MaxNumberOfRepeats ?
                        Settings.MaxNumberOfRepeats : ImagesDatabase[CurrentWordNumber].NumberLearn < 0 ?
                        0 : ImagesDatabase[CurrentWordNumber].NumberLearn;
            await DatabaseImages.Update(ImagesDatabase[CurrentWordNumber].Image_name , value);
        }

        public static void Delete() => Count = True = False = 0;
    }
}
