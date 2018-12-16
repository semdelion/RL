﻿using Android.App;
using Android.Graphics;
using Android.OS;

namespace ReLearn.Droid
{
    [Activity(
        Label = "@string/app_name", 
        MainLauncher = true, 
        Theme = "@style/SplashTheme", 
        NoHistory = true)]

    public class SplashScreen : Activity
    {     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AdditionalFunctions.Update_Configuration_Locale(this.Resources);
            base.OnCreate(savedInstanceState);
            FrameStatistics.Plain = Typeface.CreateFromAsset(Assets, Settings.font);
            DataBase.InstallDatabaseFromAssets();
            DataBase.SetupConnection();
            DBWords.СreateTable();
            DBImages.СreateTable();
            DBWords.ADDCOLUMN();
            DBImages.UpdateData();
            DBWords.UpdateData();
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}


