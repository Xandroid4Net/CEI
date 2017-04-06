using CEI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Services
{
    public interface IFavoriteService
    {
        void AddToFavorites(Item item);
        void RemoveFromFavorites(Item item);
        bool IsFavorite(Item item);
        List<int> GetFavorites();
    }
    public abstract class AbstractFavoriteService : IFavoriteService
    {
        //If api supports storing favorites server side add that logic here.

        public virtual void AddToFavorites(Item item)
        { }

        public abstract List<int> GetFavorites();

        public abstract bool IsFavorite(Item item);

        public virtual void RemoveFromFavorites(Item item)
        { }

        protected List<int> JsonToListOfFavorites(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<int>>(json);
            }
            return new List<int>();
        }

        protected string ListOfFavoritesToString(List<int> favorites)
        {
            return JsonConvert.SerializeObject(favorites);
        }
    }
}
