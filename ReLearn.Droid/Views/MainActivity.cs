﻿using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.Core.ViewModels;
using ReLearn.Droid.Fragments;
using ReLearn.Droid.Services;

namespace ReLearn.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Locale)]
    public class MainActivity : MvxAppCompatActivity<MainViewModel>, INavigationActivity, Android.Support.V4.App.FragmentManager.IOnBackStackChangedListener
    {
        public DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_activity);
            UserDialogs.Init(this);
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarMain);
            // SetSupportActionBar(toolbar);
            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            this.SupportFragmentManager.AddOnBackStackChangedListener(this);
           // SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, new HomeFragment(), "Home").Commit();
            


            //this.SupportFragmentManager.FindFragmentById(Resource.Layout.main_content);
            ViewModel?.ShowMenu();
            //DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            //drawer.AddDrawerListener(toggle);
            //toggle.SyncState();

            //NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //navigationView.SetNavigationItemSelectedListener(this);
        }


        [Java.Interop.Export("Button_Support_Project_Click")] //TODO 
        public void Button_Support_Project_Click(View v)
        {
            Intent browserIntent = new Intent(Intent.ActionView);
            browserIntent.SetData(Android.Net.Uri.Parse("http://www.donationalerts.ru/r/semdelionteam"));
            StartActivity(browserIntent);
        }

        //[Java.Interop.Export("Button_Send_Click")]
        //public void Button_Send_Click(View v)
        //{
        //    if (ViewModel.Message == "" || ViewModel.Message == null)
        //        Toast.MakeText(this, GetString(Resource.String.Enter_word), ToastLength.Short).Show();//TODO android Intent;
        //    else
        //    {
        //        var email = new Intent(Intent.ActionSend);
        //        email.PutExtra(Intent.ExtraEmail, new string[] { "SemdelionTeam@gmail.com" });
        //        email.PutExtra(Intent.ExtraSubject, "Hello, SemdelionTeam!");
        //        email.PutExtra(Intent.ExtraText, Fragment.Message);
        //        email.SetType("message/rfc822");
        //        try
        //        {
        //            StartActivity(email);
        //            ViewModel.Message = "";
        //        }
        //        catch
        //        {
        //            //Toast.MakeText(this, "There are no email applications installed.", ToastLength.Long).Show();
        //        }
        //    }
        //}

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
                base.OnBackPressed();
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null) return;

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void OnBackStackChanged()
        {
           // SupportFragmentManager.RemoveOnBackStackChangedListener();
        }


        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.settings, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Resource.Id.MenuSelectDictionary:
        //            ViewModel.ToSelectDictionary.Execute();
        //            return true;
        //        default:
        //            return base.OnOptionsItemSelected(item);
        //    }
        //}

        //public bool OnNavigationItemSelected(IMenuItem menuItem)
        //{
        //    switch (menuItem.ItemId)
        //    {
        //        case Resource.Id.statistics:
        //            ViewModel.ToStatistic.Execute();
        //            break;
        //        case Resource.Id.view_dictionary:
        //            ViewModel.ToViewDictionary.Execute();
        //            break;
        //        case Resource.Id.add_word:
        //            ViewModel.ToAddition.Execute();
        //            break;
        //        case Resource.Id.about_us:
        //            ViewModel.ToAboutUs.Execute();
        //            break;
        //        case Resource.Id.feedback:
        //            ViewModel.ToFeedback.Execute();
        //            break;
        //        case Resource.Id.settings_menu:
        //            ViewModel.ToSettingsMenu.Execute();
        //            Finish();
        //            break; 
        //    }
        //    DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
        //    drawer.CloseDrawer(GravityCompat.Start);
        //    return true;
        //}
    }
}
