namespace Module._4
{
    using System.Web;

    public class VisibilityProvider
    {
        public bool IsVisible(Node node)
        {
            var isVisible = false;

            if (null != HttpContext.Current && null != HttpContext.Current.User)
            {
                isVisible = HttpContext.Current.User.IsInRole(node.Role);
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