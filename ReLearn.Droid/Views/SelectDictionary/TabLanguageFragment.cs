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

namespace ReLearn.Droid.Views.SelectDictionary
{
    class TabLanguageFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return null; //new GraphStatistics(inflater.Inflate(Resource.Layout.statistics_main_fragment, container, false).Context,
                                                        //Colors.Orange, Colors.DarkOrange, DataBase.TableNameImage.ToString());
        }
    }
}