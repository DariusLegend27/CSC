using System.Web;
using System.Web.Mvc;

namespace Extra_Task_Google_Drive
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
