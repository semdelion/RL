﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
namespace ReLearn
{
    [Activity(Label = "Language", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English : Activity
    {
        public static Button button_english_add;
        public static Button button_english_learn;
        public static Button button_english_repeat;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {       
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English);
            GUI.Button_default(MainActivity.button_english);
            Toolbar toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglish);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            button_english_add = FindViewById<Button>(Resource.Id.button_english_add);
            button_english_learn = FindViewById<Button>(Resource.Id.button_english_learn);
            button_english_repeat = FindViewById<Button>(Resource.Id.button_english_repeat);
            button_english_add.Touch += GUI.Button_Touch;
            button_english_learn.Touch += GUI.Button_Touch;
            button_english_repeat.Touch += GUI.Button_Touch;
            button_english_add.Click += GUI.Button_1_Click;
            button_english_learn.Click += GUI.Button_1_Click;
            button_english_repeat.Click += GUI.Button_1_Click;

            button_english_add.Click += delegate
            {
                Intent intent_english_add = new Intent(this, typeof(English_Add));
                StartActivity(intent_english_add);
            };

            button_english_learn.Click += delegate
            {
                try
                {
                    var database = DataBase.Connect(NameDatabase.English_DB);
                    database.CreateTable<Database_Words>();
                    int search_occurrences = database.Query<Database_Words>("SELECT * FROM " + DataBase.Table_Name).Count;
                    if (search_occurrences != 0)
                    {
                        Intent intent_english_learn = new Intent(this, typeof(English_Learn));
                        StartActivity(intent_english_learn);
                    }
                    else
                        Toast.MakeText(this, "The database is empty", ToastLength.Short).Show();
                }
                catch { Toast.MakeText(this, "Error : can't connect to database", ToastLength.Long).Show(); }
            };

            button_english_repeat.Click += delegate
            {
                try {

                    var database = DataBase.Connect(NameDatabase.English_DB);
                    database.CreateTable<Database_Words>();
                    var search_occurrences = database.Query<Database_Words>("SELECT * FROM  " + DataBase.Table_Name);// поиск вхождения слова в БД
                    var search_numberlearn_null = database.Query<Database_Words>("SELECT * FROM  " + DataBase.Table_Name + " WHERE numberLearn = 0").Count;
                    if (search_occurrences.Count == search_numberlearn_null)
                        Toast.MakeText(this, "You repeated all the words", ToastLength.Short).Show();
                    else if (search_occurrences.Count != 0){
                        Intent intent_english_repeat = new Intent(this, typeof(English_Repeat));
                        StartActivity(intent_english_repeat);
                    }
                    else
                        Toast.MakeText(this, "The database is empty", ToastLength.Short).Show();
                }
                catch { Toast.MakeText(this, "Error : can't connect to database", ToastLength.Long).Show(); }
            };   
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_english, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            
            int id = item.ItemId;
            if (id == Resource.Id.menuDatabase_MyDictionary)
            {
                DataBase.Table_Name = "Database_My_Directly";
                return true;
            }
            if (id == Resource.Id.menuDatabase_PopularWords)
            {
                DataBase.Table_Name = "Database_PopularWords";
                return true;
            }
            if (id == Resource.Id.Stats){
                Intent intent_english_stat = new Intent(this, typeof(English_Stat));
                StartActivity(intent_english_stat);
                return true;
            }
            if(id == Resource.Id.Deleteword){
                Intent intent_english_add = new Intent(this, typeof(English_Delete));
                StartActivity(intent_english_add);
                return true;
            }
            if (id == Android.Resource.Id.Home)
                this.Finish(); 
            return true;
        }
    }
}