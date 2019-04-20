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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Christoc.Modules.dnnsimplearticle.Components
{
    ///<summary>
    /// The Article object for use in our article module
    ///</summary>
    public class Article : ContentItem
    {
        ///<summary>
        /// ArticleId is the identifier for our articles, it matches up to the ID column in the database
        ///</summary>
        public int ArticleId { get; set; }
        ///<summary>
        /// Article Title
        ///</summary>
        public string Title{ get; set; }
        ///<summary>
        /// A simple article description
        ///</summary>
        public string Description { get; set; }
        ///<summary>
        /// The body of our article
        ///</summary>
        public string Body { get; set; }

        ///<summary>
        /// Article Thumbnail Image
        ///</summary>
        public string ThumbImg { get; set; }

        ///<summary>
        /// Article Large Image
        ///</summary>
        public string LargeImg { get; set; }

        ///<summary>
        /// An integer for the user id of the user who created the article
        ///</summary>
        public int CreatedByUserId { get; set; }
        ///<summary>
        /// An integer for the user id of the user who last updated the article
        ///</summary>
        public int LastModifiedByUserId { get; set; }
        ///<summary>
        /// The date the article was created
        ///</summary>
        public new DateTime CreatedOnDate { get; set; }
        ///<summary>
        /// The date the article was updated
        ///</summary>
        public new DateTime LastModifiedOnDate { get; set; }
        ///<summary>
        /// The ModuleId of where the article was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }
        ///<summary>
        /// The portal where the article resides
        ///</summary>
        public int PortalId { get; set; }

        ///<summary>
        /// The number of articles, populated when getting multiple articles
        ///</summary>
        public int TotalRecords { get; set; }

        //Read Only Props
        ///<summary>
        /// The username of the user who created the article
        ///</summary>
        public string CreatedByUser
        {
            get
            {
                return CreatedByUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedByUserId).DisplayName : Null.NullString;
            }
        }

        ///<summary>
        /// The username of the user who last updated the article
        ///</summary>
        public string LastUpdatedByUser
        {
            get
            {
                return LastModifiedByUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastModifiedByUserId).Username : Null.NullString;
            }
        }

        ///<summary>
        /// Save the article
        ///</summary>
        ///<param name="tabId"></param>
        public void Save(int tabId)
        {
            ArticleId = ArticleController.SaveArticle(this, tabId);
        }

        #region IHydratable Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public override void Fill(System.Data.IDataReader dr)
        {
            //Call the base classes fill method to populate base class properties
            base.FillInternal(dr);

            ArticleId = Null.SetNullInteger(dr["Id"]);
            ModuleId = Null.SetNullInteger(dr["ModuleId"]);
            Title = Null.SetNullString(dr["Title"]);
            Description = Null.SetNullString(dr["Description"]);
            Body = Null.SetNullString(dr["Body"]);
            //PortalId = Null.SetNullInteger(dr["PortalId"]);
            CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastModifiedByUserId = Null.SetNullInteger(dr["LastModifiedByUserId"]);
            CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
            LastModifiedOnDate = Null.SetNullDateTime(dr["LastModifiedOnDate"]);

            TotalRecords = Null.SetNullInteger(dr["TotalRecords"]);
        }

        /// <summary>
        /// Gets and sets the Key ID
        /// </summary>
        /// <returns>An Integer</returns>
        public override int KeyID
        {
            get { return ArticleId; }
            set { ArticleId = value; }
        }

        #endregion
    }
}
