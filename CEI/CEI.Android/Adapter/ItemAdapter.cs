using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CEI.Droid.Services;
using CEI.IOC;
using CEI.Models;
using CEI.Services;
using CEI.Services.Navigation;
using CEI.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JavaObject = Java.Lang.Object;

namespace CEI.Droid.Adapter
{
    public class ItemAdapter : RecyclerView.Adapter
    {
        private IApiService api;
        private IImageService image;
        private IUIDispatcher dispatcher;
        private INavigationService navigator;
        private List<Item> items;
        public ItemAdapter(List<Item> movies)
        {
            items = movies;
            api = Locator.Get<IApiService>();
            image = Locator.Get<IImageService>();
            dispatcher = Locator.Get<IUIDispatcher>();
            navigator = Locator.Get<INavigationService>();
        }
        public override int ItemCount => items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            var vh = holder as ItemViewHolder;
            vh.Root.SetOnClickListener(null);
            vh.Root.SetOnClickListener(new ItemOnClick(async () =>
            {
                var detailVM = Locator.GetNewInstance<DetailViewModel>();
                await detailVM.Initialize(items[position]);
                navigator.Navigate(PageType.Detail, false);
            }));
            Task.Run(async () =>
            {
                var url = await api.GetImageUrl(items[position].Poster_path).ConfigureAwait(false);
                var bitmap = image.LoadImage(url);
                dispatcher.RunOnUiThread(() =>
                {
                    vh.Image.SetImageBitmap(bitmap);
                    bitmap.Dispose();
                    bitmap = null;
                });
            });
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.itemview, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            ItemViewHolder vh = new ItemViewHolder(itemView);
            return vh;
        }

        public void AddItems(List<Item> movies)
        {
            items.AddRange(movies);
            dispatcher.RunOnUiThread(() =>
            {
                NotifyDataSetChanged();
            });
        }
    }

    public class ItemViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public LinearLayout Root { get; set; }

        public ItemViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.poster);
            Root = itemView.FindViewById<LinearLayout>(Resource.Id.root);
        }
    }


    public class ItemOnClick : JavaObject, View.IOnClickListener
    {
        public readonly Action Callback;
        public ItemOnClick(Action action)
        {
            Callback = action;
        }
        public void OnClick(View v)
        {
            Callback.Invoke();
        }
    }

    public class ItemAdapterScollListener : RecyclerView.OnScrollListener
    {
        private readonly LinearLayoutManager manager;

        public delegate void LoadMoreEvent();
        public event LoadMoreEvent LoadMore;

        public ItemAdapterScollListener(LinearLayoutManager lManager)
        {
            manager = lManager;

        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisiblesItems = manager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount)
            {
                LoadMore();
            }
        }

    }
}