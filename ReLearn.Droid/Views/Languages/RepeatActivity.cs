﻿using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.Helpers;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Helpers;
using ReLearn.Droid.Services;
using ReLearn.Droid.Views.Facade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReLearn.Droid.Languages
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class RepeatActivity : MvxAppCompatActivityRepeat<RepeatViewModel>
    {
        protected override void RandomButton(params Button[] buttons)   //загружаем варианты ответа в текст кнопок
        {
            RandomNumbers.RandomFourNumbers(ViewModel.CurrentNumber, ViewModel.Database.Count, out List<int> random_numbers);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Text = ViewModel.Database[random_numbers[i]].TranslationWord;
        }

        protected override void NextTest()
        {
            ViewModel.Word = ViewModel.Database[ViewModel.CurrentNumber].Word;
            ViewModel.Text = $"{ ViewModel.Database[ViewModel.CurrentNumber].Word}" +
                $"{(ViewModel.Database[ViewModel.CurrentNumber].Transcription == null ? "" : $"\n{ ViewModel.Database[ViewModel.CurrentNumber].Transcription}")}";
            const int four = 4;
            int first = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(four);
            List<int> randomNumbers = new List<int> { first, 0, 0, 0 };
            for (int i = 1; i < four; i++)
                randomNumbers[i] = (first + i) % four; 
           RandomButton(Buttons[randomNumbers[0]], Buttons[randomNumbers[1]], Buttons[randomNumbers[2]], Buttons[randomNumbers[3]]);        
        }

        protected override async Task Answer(params Button[] buttons) // подсвечиваем правильный ответ, если мы ошиблись подсвечиваем неправвильный и паравильный 
        {
            API.Statistics.Count++;
            ButtonEnable(false);
            if (buttons[0].Text == ViewModel.Database[ViewModel.CurrentNumber].TranslationWord)
            {
                await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, -Settings.TrueAnswer);
                API.Statistics.True++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_true);
            }
            else
            {
                await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, Settings.FalseAnswer);
                API.Statistics.False++;
                buttons[0].Background = GetDrawable(Resource.Drawable.button_false);
                int index = Buttons.FindIndex(s => s.Text == ViewModel.Database[ViewModel.CurrentNumber].TranslationWord);
                Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
            }
        }

        protected override async Task Unknown()
        {
            API.Statistics.Count++;
            API.Statistics.False++;
            await API.Statistics.Add(ViewModel.Database, ViewModel.CurrentNumber, Settings.NeutralAnswer);
            int index =  Buttons.FindIndex(s => s.Text == ViewModel.Database[ViewModel.CurrentNumber].TranslationWord);
            Buttons[index].Background = GetDrawable(Resource.Drawable.button_true);
        }

        [Java.Interop.Export("Button_Speak_Languages_Click")]
        public void Button_Speak_Languages_Click(View v) => ViewModel.TextToSpeech.Speak(ViewModel.Word);

        [Java.Interop.Export("Button_Languages_1_Click")]
        public async void Button_Languages_1_Click(View v) => await Answer(Buttons[0], Buttons[1], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Languages_2_Click")] 
        public async void Button_Languages_2_Click(View v) => await Answer(Buttons[1], Buttons[0], Buttons[2], Buttons[3]);

        [Java.Interop.Export("Button_Languages_3_Click")] 
        public async void Button_Languages_3_Click(View v) => await Answer(Buttons[2], Buttons[0], Buttons[1], Buttons[3]);

        [Java.Interop.Export("Button_Languages_4_Click")] 
        public async void Button_Languages_4_Click(View v) => await Answer(Buttons[3], Buttons[0], Buttons[1], Buttons[2]);

        [Java.Interop.Export("Button_Languages_Next_Click")]
        public async void Button_Languages_Next_Click(View v)
        {
            ButtonNext.button.Enabled = false;
            if (ButtonNext.State == StateButton.Unknown)
            {
                ButtonNext.State = StateButton.Next;
                ButtonEnable(false);
                await Unknown();
            }
            else
            {
                if (API.Statistics.Count < Settings.NumberOfRepeatsLanguage)
                {
                    ViewModel.CurrentNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ViewModel.Database.Count);
                    NextTest();
                    ButtonEnable(true);
                    ViewModel.TitleCount = $"{GetString(Resource.String.Repeated)} {API.Statistics.Count + 1}/{Settings.NumberOfRepeatsLanguage}";
                }
                else
                {
                    await DBStatistics.Insert(API.Statistics.True, API.Statistics.False, $"{DataBase.TableName}");
                    API.Statistics.Delete();
                    ViewModel.ToStatistic.Execute();
                    Finish();
                }
            }
            ButtonNext.button.Enabled = true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_languages_repeat);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_languages_repeat);
           
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            using (var background = BitmapHelper.GetBackgroung(Resources, _displayWidth - PixelConverter.DpToPX(70), PixelConverter.DpToPX(190)))
                FindViewById<TextView>(Resource.Id.textView_Eng_Word).Background = background;

            Buttons = new List<Button>{
                FindViewById<Button>(Resource.Id.button_Languages_choice1),
                FindViewById<Button>(Resource.Id.button_Languages_choice2),
                FindViewById<Button>(Resource.Id.button_Languages_choice3),
                FindViewById<Button>(Resource.Id.button_Languages_choice4),
            };

            ButtonNext = new ButtonNext
            {
                button = FindViewById<Button>(Resource.Id.button_Languages_Next),
                State = StateButton.Next
            };
            Button_Languages_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
    }
}