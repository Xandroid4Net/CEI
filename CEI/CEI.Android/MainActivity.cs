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
using JavaObject = Java.Lang.Object;
using CEI.IOC.Bus;

namespace CEI.Droid
{
    [Activity(Label = "CEI.Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        public bool isPaused = false;
        private Toolbar toolbar;
        private FrameLayout navigationFrame;
        private INavigationService navigationService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Locator.Register<IUIDispatcher>(new UIDispatcher(this));
            SetContentView(Resource.Layout.Main);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            navigationFrame = FindViewById<FrameLayout>(Resource.Id.navigator);

            var service = Locator.Get<INavigationService>(false) as NavigationService;
            service.GotoPage += GoToPage;
            navigationService = service;
            navigationService.Navigate(PageType.Browse, true);

            EventBus.Subscribe(this.GetType(), (obj) =>
            {
                if (isPaused) return;
                if (obj is PlayVideoEvent)
                {
                    PlayVideo(obj as PlayVideoEvent);
                    return;
                }

                if (obj is Exception)
                {
                    Locator.Get<IUIDispatcher>().RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, (obj as Exception).Message, ToastLength.Short).Show();
                    });
                    return;
                }
            });
        }

        private void PlayVideo(PlayVideoEvent ev)
        {
            Intent appIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("vnd.youtube:" + ev.Key));
            Intent webIntent = new Intent(Intent.ActionView,
                        Android.Net.Uri.Parse("http://www.youtube.com/watch?v=" + ev.Key));
            try
            {
                StartActivity(appIntent);
            }
            catch (ActivityNotFoundException ex)
            {
                StartActivity(webIntent);
            }
        }

        public override void OnBackPressed()
        {
            if (navigationService.GetCurrentPage() == PageType.Browse)
            {
                base.OnBackPressed();
                return;
            }
            navigationService.NavigateBack();
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
                    FragmentManager.BeginTransaction().Replace(Resource.Id.navigator, page.As<DetailPage>()).Commit();
                    break;
                default:
                    break;
            }


        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            if (navigationService.GetCurrentPage() == PageType.Browse)
            {
                MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            }
            else
            {
                MenuInflater.Inflate(Resource.Menu.menu_detail, menu);
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.close:
                    OnBackPressed();
                    return true;
                case Resource.Id.search:
                    Toast.MakeText(this, "Search not implemented", ToastLength.Short).Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}


