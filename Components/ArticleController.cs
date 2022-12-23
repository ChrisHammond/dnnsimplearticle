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
using System.Collections.Generic;
using System.Web;
using Christoc.Modules.dnnsimplearticle.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content.Common;

namespace Christoc.Modules.dnnsimplearticle.Components
{
    ///<summary>
    /// ArticleController provides the implementation of methods for our article
    ///</summary>
    public class ArticleController
    {

        ///<summary>
        /// Get an individual article
        ///</summary>
        ///<param name="articleId"></param>
        ///<returns></returns>
        public static Article GetArticle(int articleId)
        {
            return CBO.FillObject<Article>(DataProvider.Instance().GetArticle(articleId));
        }

        ///<summary>
        /// Get a list of articles for a moduleid, 1000 of them
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        public static List<Article> GetArticles(int moduleId)
        {
            return GetArticles(moduleId, 1000, 0);
        }

        ///<summary>
        /// Get a list of articles for a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        ///<returns></returns>
        public static List<Article> GetArticles(int moduleId, int pageSize, int pageNumber)
        {
            //todo: look at caching
            return CBO.FillCollection<Article>(DataProvider.Instance().GetArticles(moduleId, pageSize, pageNumber));
        }

        ///<summary>
        /// Get a list of articles for a portal
        ///</summary>
        ///<param name="portalId"></param>
        ///<returns></returns>
        public static List<Article> GetAllArticles(int portalId)
        {   //todo: look at caching
            return GetAllArticles(portalId, false);
        }

        ///<summary>
        /// Get a list of articles for a portal and sorted (true == sort by ID ASC)
        ///</summary>
        ///<param name="portalId"></param>
        ///<param name="sortAsc"></param>
        ///<returns></returns>
        public static List<Article> GetAllArticles(int portalId, bool sortAsc)
        {   //todo: look at caching
            return CBO.FillCollection<Article>(DataProvider.Instance().GetAllArticles(portalId, sortAsc));
        }

        ///<summary>Save the article, checks if we are creating new, or updating an existing
        ///</summary>
        ///<param name="a"></param>
        ///<param name="tabId"></param>
        ///<returns></returns>
        public static int SaveArticle(Article a, int tabId)
        {
            if (a.ArticleId < 1)
            {
                a.ArticleId = DataProvider.Instance().AddArticle(a);

                var cntTaxonomy = new Taxonomy.Content();
                var objContentItem = cntTaxonomy.CreateContentItem(a, tabId);

                a.ContentItemId = objContentItem.ContentItemId;
                SaveArticle(a, tabId);
            }
            else
            {
                DataProvider.Instance().UpdateArticle(a);
                var cntTaxonomy = new Taxonomy.Content();
                cntTaxonomy.UpdateContentItem(a, tabId);
            }
            return a.ArticleId;
        }



        ///<summary>Delete an article based on ID
        ///</summary>
        ///<param name="articleId"></param>
        public static void DeleteArticle(int articleId)
        {
            //get the article
            var a = GetArticle(articleId);
            var cntTaxonomy = new Taxonomy.Content();
            //delete the content item
            cntTaxonomy.DeleteContentItem(a);
            //delete the article
            DataProvider.Instance().DeleteArticle(articleId);
        }

        ///<summary>Delete all articles based on a moduleid
        ///</summary>
        ///<param name="moduleId"></param>
        public static void DeleteArticles(int moduleId)
        {
            //also need to delete the Content Items
            var cntTaxonomy = new Taxonomy.Content();

            //for each article in this module 
            foreach (Article a in GetArticles(moduleId))
            {
                //delete the content items for these articles
                cntTaxonomy.DeleteContentItem(a);
            }

            DataProvider.Instance().DeleteArticles(moduleId);
        }


        public static string GetArticleLink(int tabId, int articleId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(tabId, String.Empty, "aid=" + articleId);
        }


        public static string CsvExport(int portalId)
        {
            var output = "";
            List<Article> coldnnsimplearticles = ArticleController.GetAllArticles(portalId);
            if (coldnnsimplearticles.Count != 0)
            {
                //Test Title,test-title,"This is the body",2010-06-27 02:30:26.487,html
                
                foreach (Article objArticle in coldnnsimplearticles)
                {
                    var ci = Util.GetContentController().GetContentItem(objArticle.ContentItemId); ;
                    var tagList = string.Empty;

                    if (ci.Terms.Count > 1)
                    {
                        tagList += "[ ";
                        foreach (var t in ci.Terms)
                        {
                            tagList += t.Name + ", ";
                        }

                        tagList += " ]";
                    }


                    output += "\"" + objArticle.Title + "\",";
                    output += GetArticleLink(objArticle.TabID,objArticle.ArticleId) + ",\"";
                    output += HttpUtility.HtmlDecode(objArticle.Body).Replace("\"", "\"\"").Replace(Environment.NewLine, " ") + "\",";
                    output += objArticle.CreatedOnDate.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                    output += "html,";
                    if(objArticle.ImageUrl.IndexOf("?ver=")>0)
                    {
                        output += objArticle.ImageUrl.Substring(0, objArticle.ImageUrl.IndexOf("?ver=")) + ","; //remote ?ver=
                    }
                    else {
                        output += objArticle.ImageUrl + ","; //remote ?ver=
                    }
                    
                    
                    output += "\"" + tagList + "\"$clear$";

                    /*
                     * 
                     *  var mi = mc.GetModuleByDefinition(PortalId, "Content List");
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

                     * 
                     */

                }
            }
            return output;

        }

    }
}
