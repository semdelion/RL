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
using ReLearn.API.Database;
using ReLearn.Droid.Fragments;

namespace ReLearn.Droid.Views.Statistics
{
    public class TabGeneralFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return new GraphGeneralStatistics(inflater.Inflate(Resource.Layout.statistics_general_fragment, container, false).Context,
                StatisticsFragment.LightColor, StatisticsFragment.DarkColor, StatisticsFragment.StatisticsDB,
                StatisticsFragment.DataTupe, DataBase.TableName.ToString());
        }
    }
}

  

   
