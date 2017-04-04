using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using CEI.IOC;
using CEI.Services.Navigation;
using CEI.Droid.Services;
using CEI.Droid.Pages;
using CEI.Services;

namespace CEI.Droid
{
    [Activity(Label = "CEI.Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        public bool isPaused = false;
        private Toolbar toolbar;
        private FrameLayout navigationFrame;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Locator.Register<IUIDispatcher>(new UIDispatcher(this));
            SetContentView(Resource.Layout.Main);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            navigationFrame = FindViewById<FrameLayout>(Resource.Id.navigator);

            var navigationService = Locator.Get<INavigationService>(false) as NavigationService;
            navigationService.GotoPage += GoToPage;
            Locator.Get<INavigationService>(false).Navigate(PageType.Browse, true);
        }

        protected override void OnPause()
        {
            isPaused = true;
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            isPaused = false;
        }

        private void GoToPage(IPage page, bool clearpageStack)
        {
            if (isPaused) return;

            switch (page.GetPageType())
            {
                case PageType.Browse:
                    FragmentManager.BeginTransaction().Replace(Resource.Id.navigator, page.As<BrowsePage>()).Commit();
                    break;
                case PageType.Detail:
                    //FragmentManager.BeginTransaction().Replace(Resource.Id.navigator, page.As<ChooseTransactionPage>()).Commit();
                    break;
                default:
                    break;
            }


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
    }
}


