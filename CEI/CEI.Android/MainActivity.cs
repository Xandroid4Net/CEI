using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace CEI.Droid
{
	[Activity (Label = "CEI.Android", MainLauncher = true, Icon = "@drawable/icon",Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : AppCompatActivity
    {
        private Toolbar toolbar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Movie Explorer";
            SetSupportActionBar(toolbar);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
    }
}


