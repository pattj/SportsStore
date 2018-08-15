using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repo)
        {
            repository = repo;
        }


        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }


        /*
         * AddToCart and RemoveFromCart action method note:
         * 
         * The parameter names that are used (productID and returnUrl) match the input elements 
         * in the HTML forms created in the ProductSummary.cshtml view. This allows MVC to 
         * associate incoming form POST variables with those parameters, meaning there is no
         * need to process the form myself (basically model binding).
         * 
         */



        public RedirectToActionResult AddToCart (int productId, string returnUrl)
        {
          Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            //Ask browser to request a URL that will call the Index action of this controller
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId,
        string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        /*
         * HttpContext property is provided through the Controller base class 
         * and returns an an HttpContext object that provides context data about the 
         * request that has been received and the response that is being prepared.
         * 
         * HttpContext.Session property returns a Session object that implements ISession interface, 
         * which allows us to call the SetJson method, which accepts the arguments that specify a key and on 
         * object that will be added to the session state. SetJson, which is an extension method, serializes the
         * object and adds ito the session state, using the underlying functionlity provided by the ISession interface
         * 
         * To get the object back, use GetJson, where you specify the object type and key.
         * 
         */ 

        private Cart GetCart()
        {
            //Giving users their own cart and have it be persistent between requests
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            //Adding a cart to the session state in the controller
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}