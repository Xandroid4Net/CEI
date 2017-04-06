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
using CEI.Services.Navigation;
using CEI.ViewModels;
using CEI.IOC;
using System.Threading.Tasks;
using Android.Support.V7.Widget;
using CEI.Droid.Adapter;
using CEI.Models;
using Android.Support.V4.Widget;
using CEI.Services;

namespace CEI.Droid.Pages
{
    public class BrowsePage : BasePage
    {
        private BrowseViewModel viewModel;
        private RecyclerView topRated;
        private ItemAdapter topRatedAdapter;
        private LinearLayoutManager topRatedManager;

        private RecyclerView popular;
        private ItemAdapter popularAdapter;
        private LinearLayoutManager popularManager;

        private RecyclerView nowPlaying;
        private ItemAdapter nowPlayingAdapter;
        private LinearLayoutManager nowPlayingManager;

        private SwipeRefreshLayout swipeRefresh;

        public override PageType GetPageType()
        {
            return PageType.Browse;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            viewModel = Locator.Get<BrowseViewModel>();
            if (viewModel == null)
            {
                viewModel = Locator.GetNewInstance<BrowseViewModel>();
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
                Task.Run(async () =>
                {
                    await viewModel.GetTopRated().ConfigureAwait(false);
                    await viewModel.GetPopular().ConfigureAwait(false);
                    await viewModel.GetNowPlaying().ConfigureAwait(false);
                });
            }

            Activity.InvalidateOptionsMenu();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.browsePage, container, false);



            topRated = view.FindViewById<RecyclerView>(Resource.Id.topRated);
            InitTopRatedRecyclerView();

            popular = view.FindViewById<RecyclerView>(Resource.Id.popular);
            InitPopularRecyclerView();


            nowPlaying = view.FindViewById<RecyclerView>(Resource.Id.nowPlaying);
            InitNowPlayingRecyclerView();

            swipeRefresh = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefresh);
            swipeRefresh.SetOnRefreshListener(new SwipeRefreshListener(async () =>
            {
                await viewModel.Refresh(() =>
                {
                    Locator.Get<IUIDispatcher>().RunOnUiThread(() =>
                    {
                        swipeRefresh.Refreshing = false;
                        topRated.ScrollToPosition(0);
                        popular.ScrollToPosition(0);
                        nowPlaying.ScrollToPosition(0);
                    });
                });
            }));


            return view;
        }

        private void InitTopRatedRecyclerView()
        {
            topRatedManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            topRated.SetLayoutManager(topRatedManager);
            topRatedAdapter = new ItemAdapter(viewModel.GetCachedTopRated());
            topRated.SetAdapter(topRatedAdapter);
            var topRatedScrollListener = new ItemAdapterScollListener(topRatedManager);
            topRatedScrollListener.LoadMore += async delegate ()
            {
                if (!viewModel.CanLoadMoreTopRated()) return;
                await viewModel.GetTopRated().ConfigureAwait(false);
            };
            topRated.AddOnScrollListener(topRatedScrollListener);
        }

        private void InitPopularRecyclerView()
        {
            popularManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            popular.SetLayoutManager(popularManager);
            popularAdapter = new ItemAdapter(viewModel.GetCachedPopular());
            popular.SetAdapter(popularAdapter);
            var popularScrollListener = new ItemAdapterScollListener(popularManager);
            popularScrollListener.LoadMore += async delegate ()
            {
                if (!viewModel.CanLoadMorePopular()) return;
                await viewModel.GetPopular().ConfigureAwait(false);
            };
            popular.AddOnScrollListener(popularScrollListener);
        }

        private void InitNowPlayingRecyclerView()
        {
            nowPlayingManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            nowPlaying.SetLayoutManager(nowPlayingManager);
            nowPlayingAdapter = new ItemAdapter(viewModel.GetCachedNowPlaying());
            nowPlaying.SetAdapter(nowPlayingAdapter);
            var nowPlayingScrollListener = new ItemAdapterScollListener(nowPlayingManager);
            nowPlayingScrollListener.LoadMore += async delegate ()
            {
                if (!viewModel.CanLoadMoreNowPlaying()) return;
                await viewModel.GetNowPlaying().ConfigureAwait(false);
            };
            nowPlaying.AddOnScrollListener(nowPlayingScrollListener);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Activity == null) return;
            if (!IsResumed) return;
            switch (e.PropertyName)
            {
                case "TopRatedMovies":
                    topRatedAdapter.AddItems(viewModel.TopRatedMovies[viewModel.TopRatedPage]);
                    break;
                case "PopularMovies":
                    popularAdapter.AddItems(viewModel.PopularMovies[viewModel.PopularPage]);
                    break;
                case "NowPlayingMovies":
                    nowPlayingAdapter.AddItems(viewModel.NowPlayingMovies[viewModel.NowPlayingPage]);
                    break;
            }
        }
    }

    public class SwipeRefreshListener : Java.Lang.Object, SwipeRefreshLayout.IOnRefreshListener
    {
        private readonly Action refresh;
        public SwipeRefreshListener(Action action)
        {
            refresh = action;
        }
        public void OnRefresh()
        {
            refresh.Invoke();
        }
    }
}