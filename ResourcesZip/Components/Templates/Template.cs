//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2019
// by Christoc.com
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

namespace Christoc.Modules.dnnsimplearticle.Components.Templates
{
    ///<summary>
    /// The Article object for use in our article module
    ///</summary>
    public class Template : ContentItem
    {
        ///<summary>
        /// Id is the identifier for our teplates, it matches up to the ID column in the database
        ///</summary>
        public int Id { get; set; }
        ///<summary>
        /// Template Name
        ///</summary>
        public string TemplateName{ get; set; }
        ///<summary>
        /// A template content
        ///</summary>
        public string TemplateContent { get; set; }
        ///<summary>
        /// The Template Type Id
        ///</summary>
        public int TemplateTypeId { get; set; }

        public int CreatedByUserId { get; set; }
        ///<summary>
        /// An integer for the user id of the user who last updated the template
        ///</summary>
        public int LastModifiedByUserId { get; set; }
        ///<summary>
        /// The date the template was created
        ///</summary>
        public new DateTime CreatedOnDate { get; set; }
        ///<summary>
        /// The date the template was updated
        ///</summary>
        public new DateTime LastModifiedOnDate { get; set; }
        ///<summary>
        /// The ModuleId of where the template was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }
        ///<summary>
        /// The portal where the template resides
        ///</summary>
        public int PortalId { get; set; }

        //Read Only Props
        ///<summary>
        /// The username of the user who created the template
        ///</summary>
        public string CreatedByUser
        {
            get
            {
                return CreatedByUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedByUserId).DisplayName : Null.NullString;
            }
        }

        ///<summary>
        /// The username of the user who last updated the template
        ///</summary>
        public string LastUpdatedByUser
        {
            get
            {
                return LastModifiedByUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastModifiedByUserId).Username : Null.NullString;
            }
        }

        ///<summary>
        /// Save the template
        ///</summary>
        ///<param name="tabId"></param>
        //public void Save(int tabId)
        //{
        //    Id = TemplateController.SaveTemplate(this, tabId);
        //}

        #region IHydratable Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public override void Fill(System.Data.IDataReader dr)
        {
            //Call the base classes fill method to populate base class properties
            //base.FillInternal(dr);

            Id = Null.SetNullInteger(dr["Id"]);
            //ModuleId = Null.SetNullInteger(dr["ModuleId"]);
            TemplateName = Null.SetNullString(dr["TemplateName"]);
            TemplateContent = Null.SetNullString(dr["TemplateContent"]);
            //PortalId = Null.SetNullInteger(dr["PortalId"]);
            CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastModifiedByUserId = Null.SetNullInteger(dr["LastModifiedByUserId"]);
            CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
            LastModifiedOnDate = Null.SetNullDateTime(dr["LastModifiedOnDate"]);
        }

        /// <summary>
        /// Gets and sets the Key ID
        /// </summary>
        /// <returns>An Integer</returns>
        public override int KeyID
        {
            get { return Id; }
            set { Id = value; }
        }

        #endregion
    }
}
