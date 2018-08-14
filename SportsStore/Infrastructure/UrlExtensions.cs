using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public static class UrlExtensions
    {

        /*
         * Purpose of function:
         * 
         * Extends  the HttpRequest Class with a function called PathAndQuery
         * 
         * Operates on the HttpRequest class, which .NET uses to describe an HTTP
         * request. This method generates a URL that the browser will be returned to
         * right after the cart has been updated, taking into account the query string
         * if there is one.
         * 
         * 
         * 
         */
        public static string PathAndQuery (this HttpRequest request)
        {

            if(request.QueryString.HasValue)
            {
                return ($"{request.Path}{request.QueryString}");
            }
            else
            {
                return (request.Path.ToString());
            }
        }
    }
}
