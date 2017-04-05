using CEI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.ViewModels
{
    public class DetailViewModel : ViewModel
    {
        private Item item;
        public async Task<bool> Initialize(Item item)
        {
            this.item = item;
            return true;
        }

        public string Overview
        {
            get { return item.Overview; }
            set
            {
                if (item.Overview != value)
                {
                    item.Overview = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
