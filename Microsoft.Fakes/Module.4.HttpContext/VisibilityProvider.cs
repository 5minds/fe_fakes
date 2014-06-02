namespace Module._4.HttpContext
{
    using HTTPContext = System.Web.HttpContext;

    public class VisibilityProvider
    {
        public bool IsVisible(Node node)
        {
            var isVisible = false;

            if (null != HTTPContext.Current && null != HTTPContext.Current.User)
            {
                isVisible = HTTPContext.Current.User.IsInRole(node.Role);
            }

            return isVisible;
        }
    }

    public struct Node
    {
        public string Path { get; set; }
        public string Role { get; set; }
    }
}