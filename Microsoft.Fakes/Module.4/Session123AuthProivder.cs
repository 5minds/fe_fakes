namespace Module._4
{
    using System.Web;

    public class Session123AuthProivder
    {
        public static bool IsAuthorized()
        {
            bool isAuthorized = false;

            if (null != HttpContext.Current && null != HttpContext.Current.Session)
            {
                isAuthorized = (string)HttpContext.Current.Session["auth-token"] == "123";
            }

            return isAuthorized;
        }
    }
}