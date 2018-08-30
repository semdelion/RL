﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace ReLearn
{
    [Activity(Label = "Repeat 1/20", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Repeat : Activity
    {
        void Random_Button(Button B1, Button B2, Button B3, Button B4, List<Database_Flags> dataBase, int i)   //загружаем варианты ответа в текст кнопок
        {
            Additional_functions.Random_4_numbers(i, dataBase.Count, out List<int> random_numbers);
            B1.Text = Repeat_work.Word_det(dataBase[random_numbers[0]]);
            B2.Text = Repeat_work.Word_det(dataBase[random_numbers[1]]);
            B3.Text = Repeat_work.Word_det(dataBase[random_numbers[2]]);
            B4.Text = Repeat_work.Word_det(dataBase[random_numbers[3]]);
        }

        void Function_Next_Test(Button B1, Button B2, Button B3, Button B4, Button BNext, ImageView imageView, List<Database_Flags> dataBase, int rand_word, int i_rand) //new test
        {

            var his = Application.Context.Assets.Open("ImageFlags/" + dataBase[rand_word].Image_name+ ".png");
            Bitmap bitmap = BitmapFactory.DecodeStream(his);
            imageView.SetImageBitmap(bitmap);

           // imageView.SetImageResource(dataBase[rand_word].Image_name);
            GUI.Button_ebabled(BNext);
            switch (i_rand)
            {// задаём рандоммную кнопку                            
                case 0:
                    {
                        Random_Button(B1, B2, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 1:
                    {
                        Random_Button(B2, B1, B3, B4, dataBase, rand_word);
                        break;
                    }
                case 2:
                    {
                        Random_Button(B3, B1, B2, B4, dataBase, rand_word);
                        break;
                    }
                case 3:
                    {
                        Random_Button(B4, B1, B2, B3, dataBase, rand_word);
                        break;
                    }
            }
        }

        void Answer(Button B1, Button B2, Button B3, Button B4, Button BNext, List<Database_Flags> dataBase, List<Statistics_learn> Stats, int rand_word) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            GUI.Button_enable(B1, B2, B3, B4, BNext);
            if (B1.Text == Repeat_work.Word_det(dataBase[rand_word]))
            {
                Repeat_work.Delete_Repeat(Stats, Convert.ToString(dataBase[rand_word].Image_name), rand_word, dataBase[rand_word].NumberLearn -= Magic_constants.true_answer);
                Statistics_learn.AnswerTrue++;
                GUI.Button_true(B1);
            }
            else
            {
                Repeat_work.Delete_Repeat(Stats, Convert.ToString(dataBase[rand_word].Image_name), rand_word, dataBase[rand_word].NumberLearn += Magic_constants.false_answer);
                Statistics_learn.AnswerFalse++;
                GUI.Button_false(B1);
                if (B2.Text == Repeat_work.Word_det(dataBase[rand_word]))
                    GUI.Button_true(B2);
                else if (B3.Text == Repeat_work.Word_det(dataBase[rand_word]))
                    GUI.Button_true(B3);
                else
                    GUI.Button_true(B4);
            }
        }

        public void Update_Database(List<Statistics_learn> listdataBase) // изменение у бвзы данных элемента NumberLearn
        {
            var database = DataBase.Connect(Database_Name.Flags_DB);
            database.CreateTable<Database_Flags>();
            int month = DateTime.Today.Month;
            for (int i = 0; i < listdataBase.Count; i++)
            {
                database.Query<Database_Flags>("UPDATE Database_Flags SET DateRecurrence = " + month + " WHERE Image_name = ?", listdataBase[i].Word);
                database.Query<Database_Flags>("UPDATE Database_Flags SET NumberLearn = " + listdataBase[i].Learn + " WHERE Image_name = ?", listdataBase[i].Word);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Repeat);
            GUI.Button_default(Flags.button_flags_repeat);
            var toolbarMain = FindViewById<Toolbar>(Resource.Id.toolbarFlagsRepeat);
            SetActionBar(toolbarMain);
            ActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            Statistics_learn.AnswerFalse = 0;
            Statistics_learn.AnswerTrue = 0;
            int rand_word = 0, i_rand = 0, count = 0;
            List<Statistics_learn> Stats = new List<Statistics_learn>();

            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView_Flags_repeat); // проверить
            Button button1 = FindViewById<Button>(Resource.Id.button_F_choice1);
            Button button2 = FindViewById<Button>(Resource.Id.button_F_choice2);
            Button button3 = FindViewById<Button>(Resource.Id.button_F_choice3);
            Button button4 = FindViewById<Button>(Resource.Id.button_F_choice4);
            Button button_next = FindViewById<Button>(Resource.Id.button_F_Next);
            button1.Touch += GUI.Button_Click;
            button2.Touch += GUI.Button_Click;
            button3.Touch += GUI.Button_Click;
            button4.Touch += GUI.Button_Click;
            button_next.Touch += GUI.Button_Click;
            
            try
            {
                var db = DataBase.Connect(Database_Name.Flags_DB);
                db.CreateTable<Database_Flags>(); //
                List<Database_Flags> dataBase = new List<Database_Flags>();
                var table = db.Table<Database_Flags>();
                foreach (var word in table)
                { // создание БД в виде  List<DatabaseOfFlags>
                    Database_Flags w = new Database_Flags();
                    if (word.NumberLearn != 0) //add all flags with 'NumberLearn' > 0
                    {
                        w.Add(word.Image_name, word.Name_flag_en, word.Name_flag_ru, word.NumberLearn, word.DateRecurrence);
                        dataBase.Add(w);
                    }
                }
                Random rnd = new Random(unchecked((int)(DateTime.Now.Ticks)));
                rand_word = rnd.Next(dataBase.Count);
                i_rand = rnd.Next(4);                                                                                                //рандом для 4 кнопок
                Function_Next_Test(button1, button2, button3, button4, button_next, imageView, dataBase, rand_word, i_rand);
                button1.Click += (s, e) => { Answer(button1, button2, button3, button4, button_next, dataBase, Stats, rand_word); }; //лямбда оператор для подсветки ответа // true ? green:red
                button2.Click += (s, e) => { Answer(button2, button1, button3, button4, button_next, dataBase, Stats, rand_word); };
                button3.Click += (s, e) => { Answer(button3, button1, button2, button4, button_next, dataBase, Stats, rand_word); };
                button4.Click += (s, e) => { Answer(button4, button1, button2, button3, button_next, dataBase, Stats, rand_word); };
                button_next.Click += (s, e) =>
                {
                    button_next.Enabled = false;
                    if (count < Magic_constants.repeat_count - 1)
                    {
                        i_rand = rnd.Next(4);
                        rand_word = rnd.Next(dataBase.Count);
                        Function_Next_Test(button1, button2, button3, button4, button_next, imageView, dataBase, rand_word, i_rand);
                        GUI.Button_Refresh(button1, button2, button3, button4, button_next);
                        count++;
                        ActionBar.Title = Convert.ToString("Repeat " + (count + 1) + "/" + Magic_constants.repeat_count); // ПЕРЕДЕЛАЙ Костыль счётчик 
                    }
                    else
                    {
                        Repeat_work.Add_Statistics(Statistics_learn.AnswerTrue, Statistics_learn.AnswerFalse);
                        Update_Database(Stats);
                        Intent intent_flags_stat = new Intent(this, typeof(Flags_Stats));
                        StartActivity(intent_flags_stat);
                        this.Finish();
                    }
                };
            }
            catch{
                Toast.MakeText(this, "Error : can't connect to database of flags", ToastLength.Long).Show();
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            this.Finish();
            return true;
        }
    }
}