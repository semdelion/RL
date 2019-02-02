﻿using Android.Content.Res;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using ReLearn.Droid.Services;

namespace ReLearn.Droid.Views.Menu
{
    public abstract class BaseFragment : MvxFragment
    {
        protected Toolbar _toolbar;
        protected abstract int FragmentId { get; }
        protected abstract int Toolbar { get; }

        protected MvxActionBarDrawerToggle _drawerToggle;

		public MvxAppCompatActivity ParentActivity { get => (MvxAppCompatActivity)Activity; }

        protected BaseFragment() => RetainInstance = true;
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = this.BindingInflate(FragmentId, null);

			_toolbar = view.FindViewById<Toolbar>(Toolbar);
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
            return view;
		}

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
                _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
                _drawerToggle.SyncState();
        }
    }

    public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}