using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    /* Purpose of extension methods:
     * 
     * Session state feature in ASP.NET Core stores only int, string, and byte [] values.
     * If I want to store a Cart object, I need to define an extension methods to the ISession interface,
     * which provides access to the session state data to serialize Cart objects into JSON and
     * convert them back when needed.
     * 
     */


    public static class SessionExtensions
    {

        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T> (this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }

    }
}
