﻿using Acr.UserDialogs;
using Android.OS;
using Android.Runtime;
using Android.Support.Animation;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ReLearn.API.Database;
using ReLearn.Core.ViewModels;
using ReLearn.Core.ViewModels.MainMenu.SelectDictionary;
using ReLearn.Droid.Views.Menu;
using System;
using System.Collections.Generic;

namespace ReLearn.Droid.Views.SelectDictionary
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    [Register("relearn.droid.views.selectdictionary.SelectDictionaryFragment")]
    public class SelectDictionaryFragment : BaseFragment<SelectDictionaryViewModel>
    {
        IProgressDialog Loading;

        public static Dictionaries Dictionaries;

        protected override int FragmentId => Resource.Layout.fragment_menu_select_dictionary;

        protected override int Toolbar => Resource.Id.toolbar_select_dictionary;

        public static void SelectDictionaryClick(object sender, EventArgs e)
        {
            ImageView ImgV = sender as ImageView;
            Dictionaries.Selected(ImgV.Tag.ToString(), DataBase.TableName.ToString());
            Enum.TryParse(ImgV.Tag.ToString(), out TableNames name);
            DataBase.TableName = name;
            var Animation = new SpringAnimation(ImgV, DynamicAnimation.Rotation, 0);
            Animation.Spring.SetStiffness(SpringForce.StiffnessMedium);
            Animation.SetStartVelocity(500);
            Animation.Start();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            Loading = Mvx.IoCProvider.Resolve<IUserDialogs>().Loading();
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Dictionaries = new Dictionaries((int)(Resources.DisplayMetrics.WidthPixels / 3f));
            ViewPager viewPager = view.FindViewById<ViewPager>(Resource.Id.pager);

            var fragments = new List<MvxViewPagerFragmentInfo> {
                    new MvxViewPagerFragmentInfo("", "", typeof(TabLanguageFragment), typeof(DictionaryLanguageViewModel)),
                    new MvxViewPagerFragmentInfo("", "", typeof(TabImageFragment), typeof(DictionaryImageViewModel))
                };
            viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tablayout);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(tabLayout.TabCount - 2).SetIcon(Resource.Drawable.ic_dictionary_languages);
            tabLayout.GetTabAt(tabLayout.TabCount - 1).SetIcon(Resource.Drawable.ic_dictionary_images);
            Loading.Hide();
            return view;
        }
    }
}
