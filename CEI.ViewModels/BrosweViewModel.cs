using CEI.IOC;
using CEI.Models;
using CEI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.ViewModels
{
    public class BrowseViewModel : ViewModel
    {
        public int TopRatedPage { get; private set; } = 0;
        private int totalTopRatedPages = -1;
        public Dictionary<int, List<Item>> TopRatedMovies { get; set; } = new Dictionary<int, List<Item>>();

        public async Task<bool> GetTopRated()
        {
            TopRatedPage += 1;
            var service = Locator.Get<IApiService>();
            var response = await service.GetTopRated(TopRatedPage).ConfigureAwait(false);
            if (response == null) return false;
            TopRatedPage = response.Page;
            if (totalTopRatedPages < 0) totalTopRatedPages = response.Total_pages;
            TopRatedMovies[TopRatedPage] = response.Results.ToList();
            RaisePropertyChanged("TopRatedMovies");
            return true;
        }
    }
}
