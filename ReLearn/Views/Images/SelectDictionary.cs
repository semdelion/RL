﻿using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.Animation;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Calligraphy;

namespace ReLearn.Droid.Images
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class SelectDictionary: AppCompatActivity
    {
        Dictionaries Dictionaries { get; set; }

        private void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Dictionaries.Selected(ImgV.Tag.ToString(), DataBase.TableNameImage.ToString());
            Enum.TryParse(ImgV.Tag.ToString(), out TableNamesImage name);
            DataBase.TableNameImage = name;
            var Animation = new SpringAnimation(ImgV, DynamicAnimation.Rotation, 0);
            Animation.Spring.SetStiffness(SpringForce.StiffnessMedium);
            Animation.SetStartVelocity(500);
            Animation.Start();
        }

        void CreateViewForDictionary(string NameDictionarn, int ImageId, bool flag, bool separate)
        {
            var width = Resources.DisplayMetrics.WidthPixels / 100f;
            var DB = DBStatistics.GetImages(NameDictionarn);
            int count = DB.Count;
            LinearLayout DictionarylinearLayout = new LinearLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent)
            };
            Dictionaries.DictionariesBitmap.Add(Dictionaries.CreateBitmapWithStats(BitmapFactory.DecodeResource(Resources, ImageId), DB, Colors.Orange, Colors.DarkOrange));
            ImageView ImageDictionary = new ImageView(this) { Tag = NameDictionarn.ToString() };
            ImageDictionary.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent)
            {
                Gravity = flag ? GravityFlags.Left : GravityFlags.Right
            };
            ImageDictionary.SetPadding((int)(5 * width), 0, (int)(5 * width), 0);
            ImageDictionary.SetImageBitmap(Dictionaries.DictionariesBitmap[Dictionaries.DictionariesBitmap.Count - 1]);
            ImageDictionary.Click += SelectDictionaryClick;

            TextView Name = new TextView(this)
            {
                Text = AdditionalFunctions.GetResourceString(NameDictionarn, this.Resources),
                TextSize = 20//(int)(3 * width)
            };
            TextView CountWords = new TextView(this)
            {
                Text = $"{GetString(Resource.String.DatatypeImages)} {count}, {GetString(Resource.String.StudiedAt)} " +
                $"{(int)(100 - Statistics.GetAverageNumberLearn(DB) * 100f / Settings.StandardNumberOfRepeats)}%",
                TextSize = 14//(int)(2.1f * width)
            
            };
            TextView Description = new TextView(this)
            {
                Text = AdditionalFunctions.GetResourceString($"{NameDictionarn}Description", this.Resources),
                TextSize = 11//(int)(1.7f * width)
            };
            LinearLayout TextlinearLayout = new LinearLayout(this)
            {
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent),
                Orientation = Android.Widget.Orientation.Vertical
            };

            Name.LayoutParameters = CountWords.LayoutParameters = Description.LayoutParameters =
                new LinearLayout.LayoutParams(52 * Resources.DisplayMetrics.WidthPixels / 100, LinearLayout.LayoutParams.WrapContent);
            Name.SetTextColor(Colors.White);
            CountWords.SetTextColor(Colors.HintWhite);
            Description.SetTextColor(Colors.HintWhite);
            TextlinearLayout.AddView(Name);
            TextlinearLayout.AddView(CountWords);
            TextlinearLayout.AddView(Description);
            TextlinearLayout.SetPadding(flag ? 0 : (int)(5 * width), 0, !flag ? 0 : (int)(5 * width), 0);

            DictionarylinearLayout.AddView(flag == true ? (View)ImageDictionary : TextlinearLayout);
            DictionarylinearLayout.AddView(flag == false ? (View)ImageDictionary : TextlinearLayout);

            FindViewById<LinearLayout>(Resource.Id.FlagsSelectDictionary).AddView(DictionarylinearLayout);
            if (separate)
            {
                View SeparateView = new View(this)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, (int)(width / 2f))
                        {TopMargin = (int)(2 * width),BottomMargin = (int)(2 * width)},
                    Background = GetDrawable(Resource.Drawable.separator)
                };
                FindViewById<LinearLayout>(Resource.Id.FlagsSelectDictionary).AddView(SeparateView);
            }
            Dictionaries.DictionariesView.Add(ImageDictionary);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDictionary_Images);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarFlagsSelectDictionary);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Dictionaries = new Dictionaries((int)(Resources.DisplayMetrics.WidthPixels / 3f));
            CreateViewForDictionary(TableNamesImage.Flags.ToString(), Resource.Drawable.FlagDictionary, true,true);
            CreateViewForDictionary(TableNamesImage.Films.ToString(), Resource.Drawable.FilmDictionary, false,false);
            Dictionaries.Selected(DataBase.TableNameImage.ToString(), DataBase.TableNameImage.ToString());
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return base.OnOptionsItemSelected(item);
        }
        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}