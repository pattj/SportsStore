using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    public class SessionCart:Cart
    {
        /*
         * Inherits the Cart class, override the functions so that they'll call the base implementation
         * version (Add,Remove,Clear), update the cart, and store the updated state of the cart 
         * into the session (from the ISession interface)
         * 
         * 
         * 
         */ 

        public static Cart GetCart(IServiceProvider services)
        {
            /*
             * To get a hold of the ISession object, we have to first  obtain an instance
             * of the IHttpContextAccessor service, which provides us an access to an
             * HttpContext object ,in turn, provides us with the ISession.
             * 
             */ 
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart")?? new SessionCart(); //if null, create a new sessioncart
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }

    }
}
