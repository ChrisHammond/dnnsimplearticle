//
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
using System.Web.UI;
using DotNetNuke.Framework;
using DotNetNuke.Modules.dnnsimplearticle.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using Globals = DotNetNuke.Common.Globals;

namespace DotNetNuke.Modules.dnnsimplearticle.Controls
{
    ///<summary>
    /// A simple control to display an individual article
    ///</summary>
    public partial class ArticleView : dnnsimplearticleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (ArticleId > 0)
                    {
                        var curArticle = ArticleController.GetArticle(ArticleId);
                        //display article info on the view control
                        if (curArticle != null)
                        {
                            plArticleTitle.Controls.Add(new LiteralControl(curArticle.Title));
                            plArticleBody.Controls.Add(new LiteralControl(Server.HtmlDecode(curArticle.Body)));

                            //display categories in the TagsControl

                            tagsControl.ShowCategories = true;
                            tagsControl.ShowTags = false;

                            var mc = new ModuleController();

                            //look to see if the Content List module, for displaying tag results, is found

                            var mi = mc.GetModuleByDefinition(PortalId, "Content List");
                            if (mi != null)
                            {
                                tagsControl.NavigateUrlFormatString = Globals.NavigateURL(mi.TabID, String.Empty, "Tag={0}");
                                tagsControl.ContentItem =
                                    Util.GetContentController().GetContentItem(curArticle.ContentItemId);
                            }

                            tagsControl.DataBind();

                            if (tagsControl.ContentItem == null || tagsControl.ContentItem.Terms.Count < 1)
                            {
                                ArticleTags.Visible = false;
                            }

                            //change the page title, description, and add categories to keywords

                            var tp = (CDefault)Page;
                            tp.Title = curArticle.Title;
                            //we need to strip HTML from the description                    
                            tp.Description =
                                Common.Utilities.HtmlUtils.StripTags(Server.HtmlDecode(curArticle.Description),
                                                                                false);

                            var listOfTerms = curArticle.Terms.ToDelimittedString(",");
                            tp.KeyWords += "," + listOfTerms;

                            if (!IsEditable)
                            {
                                ArticleAdmin.Visible = false;
                            }
                            else
                            {
                                ClientAPI.AddButtonConfirm(lnkDelete, Localization.GetString("ConfirmDelete", LocalResourceFile));
                            }

                        }
                        else
                        {
                            //no article found
                            ArticleTags.Visible = ArticleAdmin.Visible = false;
                            plArticleTitle.Controls.Add(new LiteralControl(Localization.GetString("noArticle.Text",LocalResourceFile)));
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            if (IsEditable)
            {
                Response.Redirect(EditUrl(string.Empty, string.Empty, "Edit", "aid=" + ArticleId));
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (IsEditable)
            {
                ArticleController.DeleteArticle(ArticleId);
                Response.Redirect(Globals.NavigateURL());
            }
        }
    }
}