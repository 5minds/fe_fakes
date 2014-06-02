namespace Module._4.HttpContext
{
    using System;
    using System.Web;

    using HTTPContext = System.Web.HttpContext;

    public class CookieGenerator
    {
        public static bool GenerateCookie(string name, string value, DateTime expiry)
        {
            var generated = false;

            if (null != HTTPContext.Current)
            {
                var cookie = new HttpCookie(name, value) { Expires = expiry };
                HTTPContext.Current.Response.Cookies.Add(cookie);
                generated = true;
            }
                
            return generated;
        }
    }
}