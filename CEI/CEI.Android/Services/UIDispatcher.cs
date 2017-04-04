using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CEI.Services;

namespace CEI.Droid.Services
{
    public class UIDispatcher : IUIDispatcher
    {
        private MainActivity context;
        public UIDispatcher(MainActivity activity)
        {
            context = activity;
        }

        public void RunOnUiThread(Action action)
        {
            if (context.isPaused) return;
            if (context == null) return;
            context.RunOnUiThread(action);
        }
    }
}