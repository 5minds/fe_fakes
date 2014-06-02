namespace Module._4.HttpContext
{
    using HTTPContext = System.Web.HttpContext;

    public class Session123AuthProivder
    {
        public static bool IsAuthorized()
        {
            bool isAuthorized = false;

            if (null != HTTPContext.Current && null != HTTPContext.Current.Session)
            {
                isAuthorized = (string)HTTPContext.Current.Session["auth-token"] == "123";
            }

            return isAuthorized;
        }
    }
}