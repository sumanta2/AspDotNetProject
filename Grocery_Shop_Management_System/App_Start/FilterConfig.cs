using System.Web;
using System.Web.Mvc;

namespace Grocery_Shop_Management_System
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
