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
using Android.Support.V7.Widget;
using CEI.Droid.Adapter;
using System.Threading.Tasks;
using CEI.Services;
using CEI.Droid.Services;

namespace CEI.Droid.Pages
{
    public class DetailPage : BasePage
    {
        private TextView overview;
        private ImageView poster;
        private TextView originalTitle;
        private TextView releaseDate;
        private RatingBar rating;
        private TextView votes;
        private Button play;
        private Button favorite;
        private DetailViewModel viewModel;

        private RecyclerView similar;
        private ItemAdapter similarAdapter;
        private LinearLayoutManager similarManager;

        private bool uiBusy = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            viewModel = Locator.GetNewInstance<DetailViewModel>();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Initialize(Locator.Get<INavigationService>().GetCurrentItem());
            Task.Run(async () =>
            {
                await viewModel.GetSimilar().ConfigureAwait(false);
                await viewModel.GetVideos().ConfigureAwait(false);
            });
            Activity.InvalidateOptionsMenu();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Activity == null) return;
            if (!IsResumed) return;
            switch (e.PropertyName)
            {
                case "SimilarMovies":
                    similarAdapter.AddItems(viewModel.SimilarMovies[viewModel.SimilarPage]);
                    break;
                case "IsFavorite":
                    if (favorite != null)
                    {
                        favorite.Text = viewModel.FavoriteButtonText;
                    }
                    break;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.detailpage, container, false);


            poster = view.FindViewById<ImageView>(Resource.Id.poster);
            overview = view.FindViewById<TextView>(Resource.Id.overview);
            originalTitle = view.FindViewById<TextView>(Resource.Id.originalTitle);
            releaseDate = view.FindViewById<TextView>(Resource.Id.releaseDate);
            rating = view.FindViewById<RatingBar>(Resource.Id.ratingBar);
            votes = view.FindViewById<TextView>(Resource.Id.votes);
            play = view.FindViewById<Button>(Resource.Id.play);
            favorite = view.FindViewById<Button>(Resource.Id.favorite);
            similar = view.FindViewById<RecyclerView>(Resource.Id.similar);

            Bind();
            return view;
        }

        private void Bind()
        {
            overview.Text = viewModel.Overview;
            originalTitle.Text = viewModel.OriginalTitle;
            releaseDate.Text = viewModel.ReleaseDate;
            rating.Rating = viewModel.Rating;
            votes.Text = viewModel.Votes;
            favorite.Text = viewModel.FavoriteButtonText;
            similarManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            similar.SetLayoutManager(similarManager);
            similarAdapter = new ItemAdapter(viewModel.GetCachedItems());
            similar.SetAdapter(similarAdapter);
            var similarScrollListener = new ItemAdapterScollListener(similarManager);
            similarScrollListener.LoadMore += async delegate ()
            {
                if (!viewModel.CanLoadMoreSimilar()) return;
                await viewModel.GetSimilar().ConfigureAwait(false);
            };
            similar.AddOnScrollListener(similarScrollListener);

            favorite.Click += delegate
            {
                //quick and dirty, need to use canexecute on command
                if (uiBusy) return;
                uiBusy = true;
                viewModel.ToggleFavorite();
                uiBusy = false;
            };

            play.Click += delegate
            {
                if (uiBusy) return;
                uiBusy = true;
                viewModel.PlayVideo();
                uiBusy = false;
            };

            Task.Run(async () =>
            {
                var url = await Locator.Get<IApiService>().GetImageUrl(viewModel.PosterPath).ConfigureAwait(false);
                var bitmap = Locator.Get<IImageService>().LoadImage(url);
                Locator.Get<IUIDispatcher>().RunOnUiThread(() =>
                {
                    poster.SetImageBitmap(bitmap);
                    bitmap.Dispose();
                    bitmap = null;
                });
            });
        }

        public override PageType GetPageType()
        {
            return PageType.Detail;
        }
    }
}