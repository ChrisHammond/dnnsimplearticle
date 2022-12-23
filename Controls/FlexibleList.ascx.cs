//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2023
// by Christoc.com
//
// Originally licensed by
// DotNetNuke - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//


using System;
using DotNetNuke.Entities.Modules;
using Christoc.Modules.dnnsimplearticle.Components;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using Globals = DotNetNuke.Common.Globals;
using Christoc.Modules.dnnsimplearticle.Components.Templates;
using System.Text;
using System.Web;
using System.Web.Security.AntiXss;

namespace Christoc.Modules.dnnsimplearticle.Controls
{
    ///<summary>
    /// A simple control that binds a list of articles
    ///</summary>
    public partial class FlexibleList : dnnsimplearticleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                FlexibleListWrapper.Attributes.Add("class","row template-" + 1);

                //get the template
                var t = TemplateController.GetTemplate(1);
                var sb = new StringBuilder();
                
                //get the articles
                var arts = ArticleController.GetArticles(ModuleId, PageSize, PageNumber);

                var totalRecords = 0;
                //templatize the articles
                foreach(var a in arts)
                {
                    var mi = ModuleController.Instance.GetTabModulesByModule(a.ModuleId);

                    var newT = string.Empty;
                    newT = t.TemplateContent.ToString();
                    newT = newT.Replace("[TITLE]", a.Title);
                    newT = newT.Replace("[ID]", a.ArticleId.ToString());
                    newT = newT.Replace("[IMGURL]", a.ImageUrl);
                    newT = newT.Replace("[CREATEDONDATE]", a.CreatedOnDate.ToString());
                    newT = newT.Replace("[URL]", ArticleController.GetArticleLink(mi[0].TabID, Convert.ToInt32(a.ArticleId)));
                    newT = HttpUtility.HtmlDecode(newT.Replace("[DESCRIPTION]", a.Description));
                    sb.Append(newT);

                    totalRecords = a.TotalRecords;

                }

                if (totalRecords > PageSize)
                    BuildPageList(ArticleController.GetArticles(ModuleId, 1, 1)[0].TotalRecords);


                var l = new Literal();
                l.Text = sb.ToString();
                //TODO: render the articles
                phFlexibleList.Controls.Add(l);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        private void BuildPageList(int totalItems)
        {
            float numberOfPages = totalItems / (float)PageSize;
            int intNumberOfPages = Convert.ToInt32(numberOfPages);
            if (numberOfPages > intNumberOfPages)
            {
                intNumberOfPages++;
            }

            NameValueCollection queryString = Request.QueryString;
            SetPagingLink(queryString, lnkNext, PageNumber + 1 < intNumberOfPages, PageNumber + 1, TabId);
            SetPagingLink(queryString, lnkPrevious, PageNumber - 1 > -1, PageNumber - 1, TabId);
        }

        private static void SetPagingLink(NameValueCollection queryString
            , HyperLink link, bool showLink, int linkedPageId, int tabId)
        {
            if (showLink)
            {
                link.Visible = true;
                queryString = new NameValueCollection(queryString);
                queryString["p"] = linkedPageId.ToString(CultureInfo.InvariantCulture);
                var additionalParameters = new List<string>(queryString.Count);

                for (int i = 0; i < queryString.Count; i++)
                {
                    if (string.Equals(queryString.GetKey(i), "TABID", StringComparison.OrdinalIgnoreCase))
                    {
                        int newTabId;
                        if (int.TryParse(queryString[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out newTabId))
                        {
                            tabId = newTabId;
                        }
                    }
                    else if (!string.Equals(queryString.GetKey(i), "LANGUAGE", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalParameters.Add(AntiXssEncoder.UrlEncode(queryString.GetKey(i)) + "=" + AntiXssEncoder.UrlEncode(queryString[i]));
                    }
                }

                
                link.NavigateUrl = Globals.NavigateURL(tabId, string.Empty, additionalParameters.ToArray());
            }
            else
            {
                link.Visible = false;
            }
        }


    }
}