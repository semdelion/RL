﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.ViewModels;
using ReLearn.Droid.Helpers;

namespace ReLearn.Droid.Views.Facade
{
    public enum StateButton
    {
        Next,
        Unknown
    }

    public class ButtonNext
    {
        public StateButton State { get; set; }
        public Button button = null;
    }

    public abstract class MvxAppCompatActivityRepeat<ViewModel> : MvxAppCompatActivity<ViewModel> where ViewModel : class, IMvxViewModel
    {
        protected readonly float _displayWidth = Application.Context.Resources.DisplayMetrics.WidthPixels;

        protected List<Button> Buttons { get; set; }
        protected ButtonNext ButtonNext { get; set; }

        protected virtual void ButtonEnable(bool state)
        {
            foreach (var button in Buttons) button.Enabled = state;
            if (state)
            {
                ButtonNext.State = StateButton.Unknown;
                ButtonNext.button.Text = GetString(Resource.String.Unknown);
                foreach (var button in Buttons) button.Background = GetDrawable(Resource.Drawable.button_style_standard);
            }
            else
            {
                ButtonNext.State = StateButton.Next;
                ButtonNext.button.Text = GetString(Resource.String.Next);
            }
        }

        protected abstract Task Answer(params Button[] buttons);

        protected abstract void RandomButton(params Button[] buttons);

        protected abstract void NextTest();

        protected abstract Task Unknown();
    }
}