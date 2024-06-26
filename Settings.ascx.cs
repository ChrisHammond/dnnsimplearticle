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
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;


namespace Christoc.Modules.dnnsimplearticle
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : dnnsimplearticleSettingsBase
    {

        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    //Check for existing settings and use those on this page
                    //Settings["SettingName"]
                    txtPageSize.Text = PageSize.ToString();
                    chkShowCategories.Checked = ShowCategories;
                    chkFullArticleRss.Checked = FullArticleRss;
                    chkCleanRss.Checked = CleanRss;
                    //todo: disabled right now
                    //chkRichDescriptions.Checked = EnableRichDescriptions;
                    if(DisplayType!=string.Empty)
                        ddlDisplayType.Items.FindByValue(DisplayType).Selected=true;
                    ClientAPI.AddButtonConfirm(lnkDeleteAll, Localization.GetString("ConfirmDelete", LocalResourceFile));

                    txtCSVOut.Text = ArticleController.CsvExport(PortalId);


                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                PageSize = Convert.ToInt32(txtPageSize.Text);
                ShowCategories = Convert.ToBoolean(chkShowCategories.Checked);
                //EnableRichDescriptions = Convert.ToBoolean(chkRichDescriptions.Checked);
                FullArticleRss = Convert.ToBoolean(chkFullArticleRss.Checked);
                CleanRss = Convert.ToBoolean(chkCleanRss.Checked);

                DisplayType = ddlDisplayType.SelectedValue;
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        protected void lnkDeleteAll_Click(object sender, EventArgs e)
        {
            ArticleController.DeleteArticles(ModuleId);
        }

        protected void lnkRemoveSearchIndex_Click(object sender, EventArgs e)
        {
            //TODO: replace this
            //DotNetNuke.Data.DataProvider.Instance().DeleteSearchItems(ModuleId);
            throw new Exception("Not implemented");
            
        }
    }

}

