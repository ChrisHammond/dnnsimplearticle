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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;

namespace Christoc.Modules.dnnsimplearticle
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Viewdnnsimplearticle class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : dnnsimplearticleModuleBase, IActionable
    {
        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var controlToLoad = "Controls/ArticleList.ascx";
                
                if (DisplayType == "FlexibleList")
                {
                    controlToLoad = "Controls/FlexibleList.ascx";
                }
                //if we're passing in an article, use that instead
                if (ArticleId > 0)
                {
                    controlToLoad = "Controls/ArticleView.ascx";
                }

                var mbl = (dnnsimplearticleModuleBase)LoadControl(controlToLoad);
                mbl.ModuleConfiguration = ModuleConfiguration;
                mbl.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phViewControl.Controls.Add(mbl);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Optional Interfaces

        ///<summary>
        /// Implementing IActionable
        ///</summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection();
                //if we have an existing article we can edit it, otherwise we only create the ADD link
                if (ArticleId > 0)
                {
                    actions = new ModuleActionCollection
                                  {
                                      {
                                          GetNextActionID(), Localization.GetString("EditArticle", LocalResourceFile),
                                          "", "", "", EditUrl(string.Empty, string.Empty, "Edit", "aid=" + ArticleId),
                                          false, SecurityAccessLevel.Edit, true, false
                                          }
                                  };
                }

                actions.Add(GetNextActionID(), Localization.GetString("AddArticle", LocalResourceFile),
                                      "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false);


                return actions;
            }
        }
        #endregion
    }
}
