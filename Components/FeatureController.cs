//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2024
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
using System.Globalization;
using System.Web;
using System.Xml;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Search;
using DotNetNuke.Services.Search.Entities;

//using System.Xml;

namespace Christoc.Modules.dnnsimplearticle.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for dnnsimplearticle
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : ModuleSearchBase, IPortable
    {

        #region Public Methods

        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int moduleId)
        //{
        //    string strXml = "";

        //    List<Article> coldnnsimplearticles = ArticleController.GetArticles(moduleId);
        //    if (coldnnsimplearticles.Count != 0)
        //    {
        //        strXml += "<articles>";

        //        foreach (Article objArticle in coldnnsimplearticles)
        //        {
        //            strXml += "<article>";
        //            strXml += "<title>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objArticle.Title) + "</title>";
        //            strXml += "<description>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objArticle.Description) + "</description>";
        //            strXml += "<body>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objArticle.Body) + "</body>";
        //            strXml += "</article>";
        //        }
        //        strXml += "</articles>";
        //    }

        //    return strXml;
        //}

        /* CSV export for use in Jekyll */
        public string ExportModule(int moduleId)
        {
            string strXml = "";

            List<Article> coldnnsimplearticles = ArticleController.GetArticles(moduleId);
            if (coldnnsimplearticles.Count != 0)
            {
                //Test Title,test-title,"This is the body",2010-06-27 02:30:26.487,html

                foreach (Article objArticle in coldnnsimplearticles)
                {
                    strXml += objArticle.Title + ",";
                    strXml += objArticle.PermaLink + ",\"";
                    strXml += HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(objArticle.Body)).Replace("\"","\"\"").Replace(Environment.NewLine, " ") + "\",";
                    strXml += objArticle.CreatedOnDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK") + ",";
                    strXml += "html,";
                    strXml += objArticle.ImageUrl + "\n";                    

                }

            }

            return strXml;
        }



        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="moduleId">The Id of the module to be imported</param>
        /// <param name="content">The content to be imported</param>
        /// <param name="version">The version of the module to be imported</param>
        /// <param name="userId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            var mc = new ModuleController();
            var mi = mc.GetModule(moduleId);

            XmlNode xmldnnsimplearticles = DotNetNuke.Common.Globals.GetContent(content, "articles");


            if (xmldnnsimplearticles != null)
                // ReSharper disable PossibleNullReferenceException
                foreach (XmlNode xmldnnsimplearticle in xmldnnsimplearticles.SelectNodes("article"))
                // ReSharper restore PossibleNullReferenceException
                {
                    if (xmldnnsimplearticle != null)
                    {
                        var objdnnsimplearticle = new Article
                                                      {
                                                          ModuleId = moduleId,
                                                          Title = xmldnnsimplearticle.SelectSingleNode("title").InnerText,
                                                          Description =
                                                              xmldnnsimplearticle.SelectSingleNode("description").InnerText,
                                                          Body = xmldnnsimplearticle.SelectSingleNode("body").InnerText,
                                                          CreatedByUserId = userId,
                                                          CreatedOnDate = DateTime.Now,
                                                          LastModifiedByUserId = userId,
                                                          LastModifiedOnDate = DateTime.Now
                                                      };

                        objdnnsimplearticle.Save(mi.TabID);
                    }
                }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="modInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            var searchItemCollection = new SearchItemInfoCollection();

            List<Article> colArticles = ArticleController.GetArticles(modInfo.ModuleID);

            foreach (Article objArticle in colArticles)
            {

                string rssDescription;
                if (modInfo.ModuleSettings.Contains("FullArticleText"))
                {
                    rssDescription = Convert.ToBoolean(modInfo.ModuleSettings["FullArticleText"]) ? objArticle.Body : objArticle.Description;
                }
                else
                {
                    rssDescription = objArticle.Description;
                }

                var rssContent = HttpUtility.HtmlDecode(rssDescription);
                if (modInfo.ModuleSettings.Contains("CleanRss"))
                {
                    if (Convert.ToBoolean(modInfo.ModuleSettings["CleanRss"]))
                    {
                        rssContent = DotNetNuke.Common.Utilities.HtmlUtils.StripTags(rssContent, false);
                    }
                }

                var searchItem = new SearchItemInfo(objArticle.Title, rssContent, objArticle.CreatedByUserID, objArticle.CreatedOnDate, modInfo.ModuleID, objArticle.ArticleId.ToString(CultureInfo.InvariantCulture),DotNetNuke.Common.Utilities.HtmlUtils.StripTags(HttpUtility.HtmlDecode(objArticle.Body), false), "aid=" + objArticle.ArticleId);
                searchItemCollection.Add(searchItem);
            }

            return searchItemCollection;
        }

        public override IList<SearchDocument> GetModifiedSearchDocuments(ModuleInfo moduleInfo, DateTime beginDate)
        {
            var searchDocuments = new List<SearchDocument>();
            List<Article> colArticles = ArticleController.GetArticles(moduleInfo.ModuleID);

            foreach (Article objArticle in colArticles)
            {
                if (objArticle.LastModifiedOnDate.ToUniversalTime() <= beginDate.ToUniversalTime() ||
                    objArticle.LastModifiedOnDate.ToUniversalTime() >= DateTime.UtcNow)
                    continue;

                var content = string.Format("{0} {1} {2}", objArticle.Title, objArticle.Description, objArticle.Body);

                var searchDocumnet = new SearchDocument
                {
                    UniqueKey = string.Format("Articles:{0}:{1}", moduleInfo.ModuleID, objArticle.ArticleId),  // any unique identifier to be able to query for your individual record
                    PortalId = moduleInfo.PortalID,  // the PortalID
                    TabId = moduleInfo.TabID, // the TabID
                    AuthorUserId = objArticle.LastModifiedByUserId, // the person who created the content
                    Title = objArticle.Title,  // the title of the content, but should be the module title
                    Description = objArticle.Description,  // the description or summary of the content
                    Body = content,  // the long form of your content
                    ModifiedTimeUtc = objArticle.LastModifiedOnDate.ToUniversalTime(),  // a time stamp for the search results page
                    CultureCode = moduleInfo.CultureCode, // the current culture code
                    IsActive = true, // allows you to remove the item from the search index (great for soft deletes)
                    Url = ArticleController.GetArticleLink(moduleInfo.TabID,objArticle.ArticleId)
                };

                searchDocuments.Add(searchDocumnet);
            }

            return searchDocuments;
        }

        #endregion

    }

}
