using CEC_SwiftHR.Models.CustomAttributes;
using System.Web;
using System.Web.Mvc;

namespace CEC_SwiftHR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MultiLanguage());
        }
    }
}
