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

using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;

namespace Christoc.Modules.dnnsimplearticle.Components.Taxonomy
{
    ///<summary>
    /// The content class used for creating and maintaining content items
    ///</summary>
    public class Content
    {

        private const string ContentTypeName = "SimpleArticle";

        /// <summary>
        /// This should only run after the Article exists in the data store. 
        /// </summary>
        /// <returns>The newly created ContentItemID from the data store.</returns>
        public ContentItem CreateContentItem(Article objArticle, int tabId)
        {
            var typeController = new ContentTypeController();
            var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == ContentTypeName select t);
            int contentTypeId;

            if (colContentTypes.Count() > 0)
            {
                var contentType = colContentTypes.Single();
                contentTypeId = contentType == null ? CreateContentType() : contentType.ContentTypeId;
            }
            else
            {
                contentTypeId = CreateContentType();
            }

            var objContent = new ContentItem
            {
                Content = objArticle.Title + " " + HttpUtility.HtmlDecode(objArticle.Description),
                ContentTypeId = contentTypeId,
                Indexed = false,
                ContentKey = "aid=" + objArticle.ArticleId,
                ModuleID = objArticle.ModuleId,
                TabID = tabId
            };

            objContent.ContentItemId = Util.GetContentController().AddContentItem(objContent);

            // Add Terms
            var cntTerm = new Terms();
            cntTerm.ManageArticleTerms(objArticle, objContent);

            return objContent;
        }

        /// <summary>
        /// This is used to update the content in the ContentItems table. Should be called when an Article is updated.
        /// </summary>
        public void UpdateContentItem(Article objArticle, int tabId)
        {
            var origArticle = ArticleController.GetArticle(objArticle.ArticleId);
            var objContent = Util.GetContentController().GetContentItem(objArticle.ContentItemId);

            if (objContent == null) return;
            objContent.Content = objArticle.Title + " " + HttpUtility.HtmlDecode(objArticle.Description);
            objContent.TabID = tabId;

            
            var CC = Util.GetContentController();
            var md = CC.GetMetaData(objContent.ContentItemId);

            //remove first
            objContent.Metadata.Remove("SimpleArticleLargeImg");
            //save
            objContent.Metadata.Add("SimpleArticleLargeImg", objArticle.LargeImg);

            //TODO: removed 7/19/2012 because metadata is useless
            //delete only works if there is only one description in metadata
            //Util.GetContentController().DeleteMetaData(objContent, "Description", origArticle.Metadata["Description"]);
            //Util.GetContentController().AddMetaData(objContent, "Description", objArticle.Description);

            Util.GetContentController().UpdateContentItem(objContent);

            // Update Terms
            var cntTerm = new Terms();
            cntTerm.ManageArticleTerms(objArticle, objContent);
        }

        /// <summary>
        /// This removes a content item associated with an article from the data store. Should run every time an article is deleted.
        /// </summary>
        /// <param name="objArticle">The Content Item we wish to remove from the data store.</param>
        public void DeleteContentItem(Article objArticle)
        {
            if (objArticle.ContentItemId <= Null.NullInteger) return;
            var objContent = Util.GetContentController().GetContentItem(objArticle.ContentItemId);
            if (objContent == null) return;

            // remove any metadata/terms associated first (perhaps we should just rely on ContentItem cascade delete here?)
            var cntTerms = new Terms();
            cntTerms.RemoveArticleTerms(objArticle);

            Util.GetContentController().DeleteContentItem(objContent);
        }

        #region Private Methods

        /// <summary>
        /// Creates a Content Type (for taxonomy) in the data store.
        /// </summary>
        /// <returns>The primary key value of the new ContentType.</returns>
        private static int CreateContentType()
        {
            var typeController = new ContentTypeController();
            var objContentType = new ContentType { ContentType = ContentTypeName };

            return typeController.AddContentType(objContentType);
        }

        #endregion

    }
}


