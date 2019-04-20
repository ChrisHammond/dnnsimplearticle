//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2019
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
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Modules;
using Christoc.Modules.dnnsimplearticle.Components;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.UI.WebControls;
using Globals = DotNetNuke.Common.Globals;

namespace Christoc.Modules.dnnsimplearticle.Controls
{
    ///<summary>
    /// A simple control that binds a list of articles
    ///</summary>
    public partial class ArticleList : dnnsimplearticleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                rptArticleList.DataSource = ArticleController.GetArticles(ModuleId, PageSize, PageNumber);
                rptArticleList.DataBind();
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
                        additionalParameters.Add(queryString.GetKey(i) + "=" + queryString[i]);
                    }
                }

                link.NavigateUrl = Globals.NavigateURL(tabId, string.Empty, additionalParameters.ToArray());
            }
            else
            {
                link.Visible = false;
            }
        }

        protected void RptArticleListOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //configure the Tags
            var articleTags = e.Item.FindControl("tagsControl") as Tags;
            var lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;
            var lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;
            var pnlAdminControls = e.Item.FindControl("pnlAdminControls") as Panel;
            var pnlOtherControls = e.Item.FindControl("pnlOtherControls") as Panel;
            
            var curArticle = (Article)e.Item.DataItem;

            if (articleTags != null && ShowCategories)
            {
                articleTags.ShowCategories = true;
                articleTags.ShowTags = false;
                var mc = new ModuleController();
                
                //look to see if the Content List module, for displaying tag results, is found

                var mi = mc.GetModuleByDefinition(PortalId, "Content List");
                if(mi!=null)
                {
                    articleTags.NavigateUrlFormatString = Globals.NavigateURL(mi.TabID, String.Empty, "Tag={0}");
                    articleTags.ContentItem = Util.GetContentController().GetContentItem(curArticle.ContentItemId);
                }
                articleTags.DataBind();
                if (pnlOtherControls != null) pnlOtherControls.Visible = true;
            }
            else
            {
                if (pnlOtherControls != null) pnlOtherControls.Visible = false;
            }
            if (articleTags!=null && articleTags.ContentItem!=null)
            {
                if(articleTags.ContentItem.Terms.Count<1)
                    if (pnlOtherControls != null) pnlOtherControls.Visible = false;
            }
            else {
                if (pnlOtherControls != null) pnlOtherControls.Visible = false;
            }

            if (IsEditable && lnkDelete != null && lnkEdit != null)
            {
                if (pnlAdminControls != null) pnlAdminControls.Visible = true;
                lnkDelete.Visible = lnkDelete.Enabled = lnkEdit.Visible = lnkEdit.Enabled = true;
                ClientAPI.AddButtonConfirm(lnkDelete, Localization.GetString("ConfirmDelete", LocalResourceFile));
                lnkDelete.CommandArgument = curArticle.ArticleId.ToString();
                lnkEdit.CommandArgument = curArticle.ArticleId.ToString();
            }
            else
            {
                if (pnlAdminControls != null) pnlAdminControls.Visible = false;
            }

            //handle paging list


            if (curArticle.TotalRecords > PageSize)
                BuildPageList(ArticleController.GetArticles(ModuleId, 1, 1)[0].TotalRecords);

        }
        public void RptArticleListOnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                ArticleController.DeleteArticle(Convert.ToInt32(e.CommandArgument));
            }
            if(e.CommandName=="Edit")
            {
                Response.Redirect(EditUrl(string.Empty, string.Empty, "Edit", "aid=" + e.CommandArgument));
            }

            Response.Redirect(Globals.NavigateURL());
        }

    }
}