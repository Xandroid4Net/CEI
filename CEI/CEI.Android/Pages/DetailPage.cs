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

namespace CEI.Droid.Pages
{
    public class DetailPage : BasePage
    {
        private TextView overview;
        private DetailViewModel viewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            viewModel = Locator.Get<DetailViewModel>();
            var view = inflater.Inflate(Resource.Layout.detailpage, container, false);
            overview = view.FindViewById<TextView>(Resource.Id.overview);
            Bind();
            return view;
        }

        private void Bind()
        {
            overview.Text = viewModel.Overview;
        }
        public override PageType GetPageType()
        {
            return PageType.Detail;
        }
    }
}