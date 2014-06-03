namespace Module._4
{
    using System;
    using System.Web;

    public class CookieGenerator
    {
        public static bool GenerateCookie(string name, string value, DateTime expiry)
        {
            var generated = false;

            if (null != HttpContext.Current)
            {
                var cookie = new HttpCookie(name, value) { Expires = expiry };
                HttpContext.Current.Response.Cookies.Add(cookie);
                generated = true;
            }
                
            return generated;
        }
    }
}