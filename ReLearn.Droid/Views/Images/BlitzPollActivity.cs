﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Animation;
using Android.Util;
using Android.Views;
using Android.Widget;
using Calligraphy;
using MvvmCross.Droid.Support.V7.AppCompat;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Timers;
using ReLearn.API;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BlitzPollActivity : MvxAppCompatActivity<BlitzPollViewModel>
    {
        System.Timers.Timer timer;
        BitmapDrawable _backgroundWord;
        LinearLayout ViewPrev;
        LinearLayout ViewCurrent;
        List<DBImages> ImageDatabase { get; set; }
        bool answer;
        int CurrentWordNumber;
        string TitleCount { set => FindViewById<TextView>(Resource.Id.BlitzPoll_toolbar_textview_images).Text = value; }
        int Time = Settings.TimeToBlitz * 10;
        int True = 0, 
            False = 0;

        TextView GetTextView()
        {
            LinearLayout.LayoutParams param1 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
            {
                BottomMargin    = AdditionalFunctions.DpToPX(20),
                RightMargin     = AdditionalFunctions.DpToPX(10),
                LeftMargin      = AdditionalFunctions.DpToPX(10)
            };
            int randIndex = (CurrentWordNumber + new Random(unchecked((int)(DateTime.Now.Ticks))).Next(1, ImageDatabase.Count)) % ImageDatabase.Count;
            var textView = new TextView(this)
            {
                TextSize = 25,
                LayoutParameters = param1,
                Text = $"{(ImageDatabase[answer ? CurrentWordNumber : randIndex]).ImageName}",
                Gravity = GravityFlags.CenterHorizontal
            };
            textView.SetTextColor(Colors.White);
            return textView;
        }

        ImageView GetImage()
        {
            LinearLayout.LayoutParams param1 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, AdditionalFunctions.DpToPX(200))
            {
                TopMargin    = AdditionalFunctions.DpToPX(15),
                BottomMargin = AdditionalFunctions.DpToPX(20),
                RightMargin  = AdditionalFunctions.DpToPX(10),
                LeftMargin   = AdditionalFunctions.DpToPX(10)
            };
            var ImageView = new ImageView(this) {LayoutParameters = param1};
            using (Bitmap bitmap = BitmapFactory.DecodeStream(Application.Context.Assets.Open($"Image{DataBase.TableName}/{ImageDatabase[CurrentWordNumber].Image_name}.png")))
            using (var bitmapRounded = BitmapHandler.GetRoundedCornerBitmap(bitmap, AdditionalFunctions.DpToPX(5)))
                ImageView.SetImageBitmap(bitmapRounded);
            return ImageView;
        }

        LinearLayout GetLayout()
        {
            CurrentWordNumber = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(ImageDatabase.Count);
            answer = new Random(unchecked((int)(DateTime.Now.Ticks))).Next(2) == 1 ? true : false;
            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, AdditionalFunctions.DpToPX(300))
            {
                TopMargin       = AdditionalFunctions.DpToPX(160),
                BottomMargin    = AdditionalFunctions.DpToPX(10),
                RightMargin     = AdditionalFunctions.DpToPX(10),
                LeftMargin      = AdditionalFunctions.DpToPX(10)
            };
            var linearLayout = new LinearLayout(this)
            {
                Orientation     = Orientation.Vertical,
                LayoutParameters= param,
            };
            linearLayout.Background = _backgroundWord;
            linearLayout.AddView(GetImage());
            linearLayout.AddView(GetTextView());
            return linearLayout;
        }
        void RunAnimation(float distance)
        {
            FlingAnimation flingAnimation = new FlingAnimation(ViewCurrent, DynamicAnimation.TranslationX);
            flingAnimation.SetStartVelocity(distance);
            flingAnimation.SetFriction(2);
            flingAnimation.Start();
        }

        void Answer(bool UserAnswer)
        {
            if (!(answer ^ UserAnswer))
                True++;
            else
                False++;
            if (ViewPrev!=null)
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).RemoveView(ViewPrev);
            ViewCurrent.Background = GetDrawable(!(answer ^ UserAnswer) ? Resource.Drawable.viewTrue : Resource.Drawable.viewFalse);
            RunAnimation((UserAnswer ? 1 : -1) * AdditionalFunctions.DpToPX(5000));
            ViewPrev = ViewCurrent;
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 0);
            Statistics.Add(ImageDatabase, CurrentWordNumber, !(answer ^ UserAnswer) ? -1 : 1);
            TitleCount = $"{GetString(Resource.String.Repeated)} {True + False + 1 }";
        }

        [Java.Interop.Export("Button_Images_No_Click")]
        public void Button_Images_No_Click(View v) => Answer(false);
        
        [Java.Interop.Export("Button_Images_Yes_Click")]
        public void Button_Images_Yes_Click(View v) => Answer(true);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImagesBlitzPollActivity);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarImagesBlitzPoll);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true); // отображаем кнопку домой

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

            _backgroundWord = new BitmapDrawable(Resources, Background.GetBackgroung(
                displayMetrics.WidthPixels - AdditionalFunctions.DpToPX(20),
                AdditionalFunctions.DpToPX(300)
                ));
            var _backgroundTimer = new BitmapDrawable(Resources, Background.GetBackgroung(
                displayMetrics.WidthPixels - AdditionalFunctions.DpToPX(200),
                AdditionalFunctions.DpToPX(50)
                ));

            //float startX = 0;
            //float webViewWidth = 0;

            //FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).Touch += (sender, e) => {
            //    if (e.Event.Action == Android.Views.MotionEventActions.Down)
            //    {
            //        webViewWidth = FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).Width;
            //        startX = e.Event.GetX();
            //    }
            //    else if (e.Event.Action == Android.Views.MotionEventActions.Pointer1Up || e.Event.Action == Android.Views.MotionEventActions.PointerUp || e.Event.Action == Android.Views.MotionEventActions.Up)
            //    {
            //        float movement = e.Event.GetX() - startX;
            //        float offset = webViewWidth / 2;

            //        if (Math.Abs(movement) > offset)
            //        {
            //            if (movement < 0)
            //            {
            //                Answer(false);

            //            }
            //            else
            //            {
            //                Answer(true);

            //            }
            //        }
            //    }
                   
            //    e.Handled = false;
            //};

            FindViewById<TextView>(Resource.Id.textView_Timer_Images).Background = _backgroundTimer;

            ImageDatabase = DBImages.GetDataNotLearned;
            if (ImageDatabase.Count == 0)
            {
                Toast.MakeText(this, GetString(Resource.String.RepeatedAllImages), ToastLength.Short).Show();
                Finish();
                return;
            }
            ViewCurrent = GetLayout();
            FindViewById<RelativeLayout>(Resource.Id.RelativeLayoutImagesBlitzPoll).AddView(ViewCurrent, 1);
            TitleCount = $"{GetString(Resource.String.Repeated)} {1}";
            TimerStart();
        }

        void TimerStart()
        {
            timer = new System.Timers.Timer{ Interval = 100, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Cancel()
        {
            timer.Dispose();
            timer = null;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                if (Time > 0)
                {
                    Time--;
                    string sec = $"{Time % 10}0";
                    FindViewById<TextView>(Resource.Id.textView_Timer_Images).Text = $"{Time / 10}:{sec}";

                    if (Time == 50)
                        FindViewById<TextView>(Resource.Id.textView_Timer_Images).SetTextColor(Colors.Red);
                }
                else
                {
                    DBStatistics.Insert(True, False, DataBase.TableName.ToString());
                    ViewModel.ToStatistic.Execute();
                    Cancel();
                    Finish();
                }
            });
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Cancel();
            Finish();
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Cancel();
            Finish();
        }
    }
}