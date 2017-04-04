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

namespace CEI.Droid.Pages
{
    public class BrowsePage : BasePage
    {
        private BrowseViewModel viewModel;
        private RecyclerView topRated;
        private ItemAdapter topRatedAdapter;
        RecyclerView.LayoutManager topRatedManager;

        public override PageType GetPageType()
        {
            return PageType.Browse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.browsePage, container, false);
            viewModel = Locator.GetNewInstance<BrowseViewModel>();

            topRated = view.FindViewById<RecyclerView>(Resource.Id.topRated);
            topRatedManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            topRated.SetLayoutManager(topRatedManager);
            topRatedAdapter = new ItemAdapter(new List<Item>());
            topRated.SetAdapter(topRatedAdapter);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            Task.Run(async () =>
            {
                await viewModel.GetTopRated().ConfigureAwait(false);
            });
            return view;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Activity == null) return;
            if (!IsResumed) return;
            switch (e.PropertyName)
            {
                case "TopRatedMovies":
                    topRatedAdapter.AddItems(viewModel.TopRatedMovies[viewModel.TopRatedPage]);
                    Activity.RunOnUiThread(() =>
                    {
                        topRatedAdapter.NotifyDataSetChanged();
                    });
                    break;
            }
        }
    }
}