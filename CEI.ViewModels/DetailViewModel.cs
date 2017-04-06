using CEI.IOC;
using CEI.IOC.Bus;
using CEI.Models;
using CEI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.ViewModels
{
    public class DetailViewModel : ViewModel
    {
        public Item Item { get; private set; }
        private VideoResponse videos;
        public void Initialize(Item item)
        {
            Item = item;
            IsFavorite = Locator.Get<IFavoriteService>().IsFavorite(item);
        }

        public string Overview
        {
            get { return Item.Overview; }
            //set
            //{
            //    if (item.Overview != value)
            //    {
            //        item.Overview = value;
            //        RaisePropertyChanged();
            //    }
            //}
        }

        public string OriginalTitle
        {
            get { return Item.Original_title; }
        }

        public string PosterPath
        {
            get { return Item.Poster_path; }
        }

        public string ReleaseDate
        {
            get { return string.Format("Release Date: {0}", Item.Release_date.HasValue ? Item.Release_date.Value.ToString("MM/dd/yyyy") : ""); }
        }

        public string Votes
        {
            get { return string.Format("(from {0} votes)", Item.Vote_count); }
        }

        public float Rating
        {
            get { return (float)Math.Round(Item.Vote_average / 2, 1); }
        }

        private bool isFavorite = false;
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                if (isFavorite != value)
                {
                    isFavorite = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FavoriteButtonText
        {
            get
            {
                if (IsFavorite)
                {
                    return "Remove Favorite";
                }
                else
                {
                    return "Add To Favorites";
                }
            }
        }

        public int SimilarPage { get; private set; } = 0;
        private int totalSimilarPages = -1;
        public Dictionary<int, List<Item>> SimilarMovies { get; set; } = new Dictionary<int, List<Item>>();

        public bool CanLoadMoreSimilar()
        {
            return SimilarPage < totalSimilarPages;
        }

        public async Task<bool> GetSimilar()
        {
            SimilarPage += 1;
            var service = Locator.Get<IApiService>();
            var response = await service.GetSimilar(SimilarPage, Item.Id).ConfigureAwait(false);
            if (response == null) return false;
            SimilarPage = response.Page;
            if (totalSimilarPages < 0) totalSimilarPages = response.Total_pages;
            SimilarMovies[SimilarPage] = response.Results.ToList();
            RaisePropertyChanged("SimilarMovies");
            return true;
        }

        public async Task<bool> GetVideos()
        {
            var service = Locator.Get<IApiService>();
            videos = await service.GetVideos(Item.Id).ConfigureAwait(false);
            return true;
        }

        public List<Item> GetCachedItems()
        {
            List<Item> ret = new List<Item>();
            foreach (var kvp in SimilarMovies)
            {
                ret.AddRange(kvp.Value);
            }
            return ret;
        }

        public void ToggleFavorite()
        {
            var service = Locator.Get<IFavoriteService>();
            if (IsFavorite)
            {
                service.RemoveFromFavorites(Item);
                IsFavorite = false;
                return;
            }
            service.AddToFavorites(Item);
            IsFavorite = true;
        }

        public void PlayVideo()
        {
            if (videos != null && videos.Results.Count() > 1)
            {
                foreach (var result in videos.Results)
                {
                    if (result.Site.ToLower().Equals("youtube", StringComparison.CurrentCultureIgnoreCase))
                    {
                        EventBus.Publish(new PlayVideoEvent(result.Key));
                        return;
                    }
                }
            }
            EventBus.PublishError(new Exception("No playable youtube links found"));
        }


    }
}
