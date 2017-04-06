using CEI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEI.Services.Navigation
{
    public delegate void NavigateEventHandler(IPage page, bool clearpageStack);

    public interface INavigationService
    {
        void Navigate(PageType page, bool clearpageStack, Item item = null);

        void NavigateBack();

        Item GetCurrentItem();

        PageType GetCurrentPage();

        PageType GetPageAtIndex(int index);

        void Exit();

        int GetPageStackCount();

        IPage GetPage(PageType type);
    }
    public abstract class AbstractNavigationService : INavigationService
    {
        public event NavigateEventHandler GotoPage;

        private Stack<PageType> NavigationStack = new Stack<PageType>();
        private Stack<Item> ItemStack = new Stack<Item>();

        public abstract IPage GetPage(PageType type);
        public abstract void Exit();

        public virtual void Navigate(PageType page, bool clearpageStack, Item item = null)
        {
            if (clearpageStack)
                NavigationStack.Clear();
            NavigationStack.Push(page);
            if (page == PageType.Detail)
            {
                ItemStack.Push(item);
            }
            if (GotoPage != null)
            {
                GotoPage(GetPage(page), clearpageStack);
            }
        }

        public virtual void NavigateBack()
        {
            PageType currentPage = NavigationStack.Pop();

            if (currentPage == PageType.Detail && ItemStack.Count > 0)
            {
                ItemStack.Pop();
            }
            if (GotoPage != null)
            {
                GotoPage(GetPage(NavigationStack.Peek()), false);
            }
        }

        public virtual PageType GetCurrentPage()
        {
            PageType type;
            if (NavigationStack.Count == 0)
            {
                type = PageType.Browse;
            }
            else
            {
                type = NavigationStack.Peek();
            }
            return type;
        }

        public virtual PageType GetPageAtIndex(int index)
        {
            return NavigationStack.ElementAt(index);
        }

        public int GetPageStackCount()
        {
            return NavigationStack.Count;
        }

        public Item GetCurrentItem()
        {
            return ItemStack.Peek();
        }
    }
}
