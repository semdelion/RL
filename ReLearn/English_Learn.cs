﻿using System;
using System.IO;
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

namespace ReLearn
{
    [Activity(Label = "Learn", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class English_Learn : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //setting layout
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.English_Learn);

            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarEnglishLearn);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой
            Window.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.backgroundEnFl));


            Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(127, 0, 0, 0));

            GUI.button_default(English.button_english_learn);
            //this.ActionBar.SetBackgroundDrawable(GetDrawable(Resource.Drawable.BackgroundActionBar));
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            TextView textView_learn_en = FindViewById<TextView>(Resource.Id.textView_learn_en);
            TextView textView_learn_ru = FindViewById<TextView>(Resource.Id.textView_learn_ru);
            Button button_learn_en_ru = FindViewById<Button>(Resource.Id.button_learn_en_ru);
            button_learn_en_ru.Touch += GUI.TouchAdd;

            try
            {
                var db = DataBase.Connect(NameDatabase.English_DB);
                db.CreateTable<Database>();
                List<DatabaseOfWords> dataBase = new List<DatabaseOfWords>();
                var table = db.Table<Database>();
                foreach (var word in table)
                { // создание БД в виде  List<DatabaseOfWords>
                    DatabaseOfWords w = new DatabaseOfWords();
                    w.Add(word.enWords, word.ruWords, word.numberLearn, word.dateRepeat);
                    dataBase.Add(w);
                }

                int rand_word = 0;
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count);
                textView_learn_en.Text = dataBase[rand_word].enWords;
                textView_learn_ru.Text = dataBase[rand_word].ruWords;

                button_learn_en_ru.Click += (s, e) =>
                {                  
                    rand_word = rnd.Next(dataBase.Count);
                    textView_learn_en.Text = dataBase[rand_word].enWords;
                    textView_learn_ru.Text = dataBase[rand_word].ruWords;
                };
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of Language in Learn", ToastLength.Long).Show();
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
    
}