using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SportsStore.Models;

namespace SportsStore.Components
{
    /* View component that renders the navigation menu and integrates it into 
     * the app by invoking the compnent from the shared layout
     * 
     */
    public class NavigationMenuViewComponent : ViewComponent
    {
        //Implementation of this interface is determined in Startup.cs
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        //Called when component is usd in a Razor view, result is inserted into HTMTL sent 
        //to the browser.
        public IViewComponentResult Invoke()
        {
            //LINQ is used to select and order set of categories in the repository and 
            //pass them as the argument to the View method, which renders the default Razor partial view.
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products.Select(x => x.Category).Distinct().
                OrderBy(x => x));
        }
    }
}
