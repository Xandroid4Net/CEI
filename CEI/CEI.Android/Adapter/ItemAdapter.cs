using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CEI.Droid.Services;
using CEI.IOC;
using CEI.Models;
using CEI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CEI.Droid.Adapter
{
    public class ItemAdapter : RecyclerView.Adapter
    {
        private IApiService api;
        private IImageService image;
        private List<Item> items;
        public ItemAdapter(List<Item> movies)
        {
            items = movies;
            api = Locator.Get<IApiService>();
            image = Locator.Get<IImageService>();
        }
        public override int ItemCount => items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            var vh = holder as ItemViewHolder;
            Task.Run(async () =>
            {
                var url = await api.GetImageUrl(items[position].Poster_path).ConfigureAwait(false);
                await image.LoadImage(url, vh.Image).ConfigureAwait(false);
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
        }
    }

    public class ItemViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }

        public ItemViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.poster);
        }
    }
}