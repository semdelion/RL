﻿using Android.Content;
using Calligraphy;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace ReLearn.Droid
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class AboutUs : AppCompatActivity
    {
        [Java.Interop.Export("Button_Support_Project_Click")]
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("http://www.donationalerts.ru/r/semdelionteam"));
            StartActivity(browserIntent);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Font();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.About_us);
            var toolbarMain = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar_About_Us);
            SetSupportActionBar(toolbarMain);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {      
            Finish();
            return base.OnOptionsItemSelected(item);
        }

        protected override void AttachBaseContext(Context newbase) => base.AttachBaseContext(CalligraphyContextWrapper.Wrap(newbase));
    }
}