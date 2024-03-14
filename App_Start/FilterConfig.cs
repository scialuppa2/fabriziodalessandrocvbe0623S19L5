using System.Web;
using System.Web.Mvc;

namespace progetto_settimanaleS19L5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
