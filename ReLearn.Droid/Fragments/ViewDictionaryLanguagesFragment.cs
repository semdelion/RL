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
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Plugin.Settings;
using ReLearn.API;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.Languages;
using ReLearn.Droid.Resources;
using ReLearn.Droid.Services;
using ReLearn.Droid.Views;

namespace ReLearn.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("relearn.droid.fragments.ViewDictionaryLanguagesFragment")]
    public class ViewDictionaryLanguagesFragment : BaseFragment<ViewDictionaryViewModel>
    {
        protected override int FragmentId => Resource.Layout.languages_view_dictionary_activity;

        protected override int Toolbar => Resource.Id.toolbarLanguagesDelete;
        ListView DictionaryWords { get; set; }
        List<DBWords> WordDatabase = DBWords.GetData;
        public static bool HideStudied
        {
            get => CrossSettings.Current.GetValueOrDefault(DBSettings.HideStudied.ToString(), true);
            set => CrossSettings.Current.AddOrUpdateValue(DBSettings.HideStudied.ToString(), value);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(FragmentId, null);

            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Toolbar);
            if (_toolbar != null)
            {
                ParentActivity.SetSupportActionBar(_toolbar);
                ParentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                               // host Activity
                    (ParentActivity as INavigationActivity).DrawerLayout,  // DrawerLayout object
                    _toolbar,                               // nav drawer icon to replace 'Up' caret
                    Resource.String.navigation_drawer_open,
                    Resource.String.navigation_drawer_close
                );
                _drawerToggle.DrawerOpened += (object sender, ActionBarDrawerEventArgs e) => (Activity as MainActivity)?.HideSoftKeyboard();
                (ParentActivity as INavigationActivity).DrawerLayout.AddDrawerListener(_drawerToggle);
            }
            DictionaryWords = view.FindViewById<ListView>(Resource.Id.listView_dictionary);
            WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
            DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
            return view;
        }

       
            
        

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
            var _searchView = menu.FindItem(Resource.Id.action_search).ActionView.JavaCast<Android.Support.V7.Widget.SearchView>();
            menu.FindItem(Resource.Id.HideStudied).SetChecked(HideStudied);

            _searchView.QueryTextChange += (sender, e) =>
            {
                if (e.NewText == "")
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                else
                {
                    List<DBWords> FD = new List<DBWords>();
                    foreach (var word in WordDatabase)
                        if (word.Word.Substring(0, ((e.NewText.Length > word.Word.Length) ? 0 : e.NewText.Length)) == e.NewText)
                            FD.Add(word);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, FD);
                }
            };

            DictionaryWords.ItemClick += (s, args) =>
            {
                var word = DictionaryWords.Adapter.GetItem(args.Position);
                DBWords words = new DBWords();
                words = WordDatabase[WordDatabase.FindIndex(obj => obj.Word == word.ToString())];
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(ParentActivity);

                alert.SetTitle("");
                alert.SetMessage($"To delete : {word.ToString()}? ");
                alert.SetPositiveButton("Cancel", delegate { alert.Dispose(); });
                alert.SetNeutralButton("ок", delegate
                {
                    WordDatabase.Remove(words);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    DBWords.Delete(word.ToString());
                    Toast.MakeText(ParentActivity, GetString(Resource.String.Word_Delete), ToastLength.Short).Show();

                });
                alert.Show();
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.increase:
                    WordDatabase.Sort((x, y) => x.NumberLearn.CompareTo(y.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.decrease:
                    WordDatabase.Sort((x, y) => y.NumberLearn.CompareTo(x.NumberLearn));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.ABC:
                    WordDatabase.Sort((x, y) => x.Word.CompareTo(y.Word));
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                case Resource.Id.HideStudied:
                    HideStudied = !HideStudied;
                    item.SetChecked(HideStudied);
                    DictionaryWords.Adapter = new CustomAdapterWord(ParentActivity, HideStudied ? WordDatabase.FindAll(obj => obj.NumberLearn != 0) : WordDatabase);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}