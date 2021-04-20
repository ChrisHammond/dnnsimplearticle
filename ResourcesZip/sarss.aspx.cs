using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Entities.Portals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Christoc.Modules.dnnsimplearticle
{
    public partial class sarss : System.Web.UI.Page
    {
        //Grabbed this logic (MIT LICENSE) from the RSS page I built with Engage Publish YEARS ago. See the Publish code at https://github.com/chrishammond/Engage-Publish

        public int NumberOfItems
        {
            get
            {
                string i = Request.Params["numberOfItems"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                return -1;
            }
        }

        public int PortalId
        {
            get
            {
                string i = Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                return -1;
            }
        }

        public int ModuleId
        {
            get
            {
                string i = Request.Params["moduleId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var ps = new PortalSettings(PortalId);
            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;

            var sw = new StringWriter(CultureInfo.InvariantCulture);
            var wr = new XmlTextWriter(sw);

            wr.WriteStartElement("rss");
            wr.WriteAttributeString("version", "2.0");
            wr.WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
            wr.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
            wr.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            wr.WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");

            wr.WriteStartElement("channel");
            wr.WriteElementString("title", ps.PortalName);

            //Get aliases
            var portalAliases = PortalAliasController.Instance.GetPortalAliasesByPortalId(PortalId);//.GetPortalAliasesByPortalId(portalId);


            var pa = (portalAliases != null && portalAliases.Count<PortalAliasInfo>() > 0 ? portalAliases.ElementAt<PortalAliasInfo>(0) : null);

            if (pa.HTTPAlias.IndexOf("//", StringComparison.OrdinalIgnoreCase) == -1)
            {
                wr.WriteElementString("link", "//" + pa.HTTPAlias);
            }
            else
            {
                wr.WriteElementString("link", pa.HTTPAlias);
            }
            wr.WriteElementString("description", "RSS Feed for " + ps.PortalName);
            wr.WriteElementString("ttl", "120");


            var dt = new DataTable { Locale = CultureInfo.InvariantCulture };

            //dt = ItemId == -1 ? DataProvider.Instance().GetMostRecent(ItemTypeId, NumberOfItems, PortalId) : DataProvider.Instance().GetMostRecentByCategoryId(ItemId, ItemTypeId, NumberOfItems, PortalId);
            var al = ArticleController.GetArticles(ModuleId);


            if (al != null)
            {
                foreach (var a in al)
                {
                    wr.WriteStartElement("item");

                    DateTime startDate = DateTime.MinValue;
                    
                    wr.WriteElementString("title", a.Title);

                    wr.WriteElementString("link", ArticleController.GetArticleLink(a.TabID, a.ArticleId));
                    
                    wr.WriteElementString("description", Server.HtmlDecode(a.Description));
                    
                    wr.WriteElementString("thumbnail", a.ThumbImg);

                    wr.WriteElementString("dc:creator", a.CreatedByUser);
                                        
                    wr.WriteElementString("pubDate", a.CreatedOnDate.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));

                    wr.WriteStartElement("guid");

                    wr.WriteAttributeString("isPermaLink", "true");

                    wr.WriteString(ArticleController.GetArticleLink(a.TabID, a.ArticleId));
                    wr.WriteEndElement();
                    wr.WriteEndElement();
                }
            }

            wr.WriteEndElement();
            wr.WriteEndElement();
            Response.Write(sw.ToString());
            
        }
    }
}