
using DotNetNuke.Web.Services;

namespace DotNetNuke.Modules.dnnsimplearticle.services
{
    public class DnnSimpleArticleRouteMapper : IServiceRouteMapper
    {

        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute("DnnSimpleArticle", "{controller}.ashx/{action}",
                                     new[] { "DotNetNuke.Modules.dnnsimplearticle.services" });
        }
    }
}