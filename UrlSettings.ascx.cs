//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2024
// by Christoc.com
//

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;
using System.Collections.Generic;

namespace Christoc.Modules.dnnsimplearticle
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class UrlSettings : dnnsimplearticleSettingsBase, IExtensionUrlProviderSettingsControl
    {


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LocalResourceFile = "~/DesktopModules/dnnsimplearticle/App_LocalResources/UrlSettings.ascx.resx";
        }

        //public void LoadSettings()
        void IExtensionUrlProviderSettingsControl.LoadSettings()
        {
            if (!IsPostBack)
            {
                if (Provider != null)
                {
                    chkHideArticlePagePath.Checked = GetSafeBool(Constants.HideArticlePathPageSettingName, false);
                    lblHideArticlePagePath.Text = Localization.GetString("HideArticlePagePath.Text", LocalResourceFile);
                    cboArticlePage.SelectedPage = GetSafeTab(Constants.ArticlePageTabIdSettingName, null);
                    lblArticlePage.Text = Localization.GetString("ArticlePageTabId", LocalResourceFile);
                    txtDateFormat.Text = GetSafeString(Constants.UrlDateFormatSettingName, "");
                    lblDateFormat.Text = Localization.GetString("DateFormat", LocalResourceFile);
                }
                else
                {
                    throw new ArgumentNullException("ExtensionUrlProviderInfo is null on LoadSettings()");
                }
            }

        }

        public ExtensionUrlProviderInfo Provider { get; set; }

        //public Dictionary<string, string> SaveSettings()
        Dictionary<string, string> IExtensionUrlProviderSettingsControl.SaveSettings()
        {
            var settings = new Dictionary<string, string>();
            
            settings.Add(Constants.HideArticlePathPageSettingName, chkHideArticlePagePath.Checked.ToString());

            if (cboArticlePage.SelectedPage != null)
            {
                settings.Add(Constants.ArticlePageTabIdSettingName, cboArticlePage.SelectedPage.TabID.ToString());
            }

            //txtDateFormat
            //if (string.IsNullOrEmpty(txtDateFormat.Text) == false)
            //{
            settings.Add(Constants.UrlDateFormatSettingName, txtDateFormat.Text);
            //}

            return settings;
        }

        #region private helper methods

        private string GetSafeString(string settingName, string defaultValue)
        {
            string raw = defaultValue;
            if (Provider != null && Provider.Settings != null && Provider.Settings.ContainsKey(settingName))
            {
                raw = Provider.Settings[settingName];
            }
            return raw;
        }

        private TabInfo GetSafeTab(string settingName, TabInfo defaultValue)
        {
            TabInfo result = defaultValue;
            int tabId = GetSafeInt(settingName, -1);
            if (tabId > 0)
            {
                TabController tc = new TabController();
                result = tc.GetTab(tabId, this.PortalId, false);

            }
            return result;
        }

        private int GetSafeInt(string settingName, int defaultValue)
        {
            int result = defaultValue;
            string raw = null;
            if (Provider != null && Provider.Settings != null && Provider.Settings.ContainsKey(settingName))
            {
                raw = Provider.Settings[settingName];
            }
            if (string.IsNullOrEmpty(raw) == false) int.TryParse(raw, out result);
            return result;
        }

        private bool GetSafeBool(string settingName, bool defaultValue)
        {
            bool result = defaultValue;
            string raw = null;
            if (Provider != null && Provider.Settings != null && Provider.Settings.ContainsKey(settingName))
            {
                raw = Provider.Settings[settingName];
            }
            if (string.IsNullOrEmpty(raw) == false) bool.TryParse(raw, out result);
            return result;
        }


        #endregion
    }

}