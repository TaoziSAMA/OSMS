using System.Web;
using System.Web.Mvc;
using Oil.AppCode;

namespace Oil
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new LoginFilter() { IsCheck = true });
        }
    }
}
