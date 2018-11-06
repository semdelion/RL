﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V7.App;
using System.Collections.Generic;
using Plugin.Settings;

namespace ReLearn
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class Flags_Learn : AppCompatActivity
    {
        int Count { get; set; }
        List<DBImages> ImagesDatabase { get; set; }

        Bitmap ImageViewBox
        {
            set{ FindViewById<ImageView>(Resource.Id.imageView_Flags_learn).SetImageBitmap(value);}
        }

        string ImageName
        {
            get { return FindViewById<TextView>(Resource.Id.textView_flag_learn).Text; }
            set { FindViewById<TextView>(Resource.Id.textView_flag_learn).Text = value; }
        }
        

        [Java.Interop.Export("Button_Images_Learn_NotRepeat_Click")]
        public void Button_Languages_Learn_NotRepeat_Click(View v)
        {
            DBImages.UpdateLearningNotRepeat(ImageName);
            Button_Flags_Learn_Next_Click(null);
        }

        [Java.Interop.Export("Button_Flags_Learn_Next_Click")]
        public void Button_Flags_Learn_Next_Click(View v)
        {
            if (Count < ImagesDatabase.Count)
            {
                DBImages.UpdateLearningNext(ImagesDatabase[Count].Image_name);
                try
                {
                    ImageViewBox = BitmapFactory.DecodeStream(
                        Application.Context.Assets.Open(
                            $"ImageFlags/{ImagesDatabase[Count].Image_name}.png"));
                }
                catch(Exception ex)
                {
                    Toast.MakeText(this, ex.Message , ToastLength.Short).Show();
                }
                ImageName = AdditionalFunctions.NameOfTheFlag(ImagesDatabase[Count++]);
            }
            else
                Toast.MakeText(this, GetString(Resource.String.DictionaryOver), ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Flags_Learn);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsLearn);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            ImagesDatabase = DBImages.GetDataNotLearned;
            if (ImagesDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllImages), ToastLength.Short).Show();
                Finish();
                return;
            }
            Button_Flags_Learn_Next_Click(null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) // button home
        {
            Finish();
            return true;
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(Calligraphy.CalligraphyContextWrapper.Wrap(newbase));
    }
}