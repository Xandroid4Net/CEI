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

        public int PopularPage { get; private set; } = 0;
        private int totalPopularPages = -1;
        public Dictionary<int, List<Item>> PopularMovies { get; set; } = new Dictionary<int, List<Item>>();

        public int NowPlayingPage { get; private set; } = 0;
        private int totalNowPlayingPages = -1;
        public Dictionary<int, List<Item>> NowPlayingMovies { get; set; } = new Dictionary<int, List<Item>>();

        public bool CanLoadMoreTopRated()
        {
            return TopRatedPage <= totalTopRatedPages;
        }

        public bool CanLoadMorePopular()
        {
            return PopularPage <= totalPopularPages;
        }

        public bool CanLoadMoreNowPlaying()
        {
            return NowPlayingPage <= totalNowPlayingPages;
        }

        public List<Item> GetCachedTopRated()
        {
            return GetCachedItems(TopRatedMovies);
        }

        public List<Item> GetCachedPopular()
        {
            return GetCachedItems(PopularMovies);
        }

        public List<Item> GetCachedNowPlaying()
        {
            return GetCachedItems(NowPlayingMovies);
        }

        private List<Item> GetCachedItems(Dictionary<int, List<Item>> dictionary)
        {
            List<Item> ret = new List<Item>();
            foreach (var kvp in dictionary)
            {
                ret.AddRange(kvp.Value);
            }
            return ret;
        }

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

        public async Task<bool> GetPopular()
        {
            PopularPage += 1;
            var service = Locator.Get<IApiService>();
            var response = await service.GetPopular(PopularPage).ConfigureAwait(false);
            if (response == null) return false;
            PopularPage = response.Page;
            if (totalPopularPages < 0) totalPopularPages = response.Total_pages;
            PopularMovies[PopularPage] = response.Results.ToList();
            RaisePropertyChanged("PopularMovies");
            return true;
        }

        public async Task<bool> GetNowPlaying()
        {
            NowPlayingPage += 1;
            var service = Locator.Get<IApiService>();
            var response = await service.GetNowPlaying(NowPlayingPage).ConfigureAwait(false);
            if (response == null) return false;
            NowPlayingPage = response.Page;
            if (totalNowPlayingPages < 0) totalNowPlayingPages = response.Total_pages;
            NowPlayingMovies[NowPlayingPage] = response.Results.ToList();
            RaisePropertyChanged("NowPlayingMovies");
            return true;
        }
    }
}
