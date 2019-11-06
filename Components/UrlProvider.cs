using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;

namespace Christoc.Modules.dnnsimplearticle.Components
{
    public class UrlProvider : DotNetNuke.Entities.Urls.ExtensionUrlProvider
    {
        public override bool AlwaysUsesDnnPagePath(int portalId)
        {
            return true; //TODO: decide if we want to include the page in the URL, for now, yes
        }

        public override string ChangeFriendlyUrl(TabInfo tab, string friendlyUrlPath, FriendlyUrlOptions options, string cultureCode, ref string endingPageName, out bool useDnnPagePath, ref List<string> messages)
        {
            //set default values for out parameters
            useDnnPagePath = true;
            if (messages == null) messages = new List<string>();
            //check if we want to try and modify this Url
            //first check to see if this Url is an 'edit' Url - something that loads a module-specific page.
            //we don't want to mess with these, because they're always permissions based Urls and thus
            //no need to be friendly
            if (string.IsNullOrEmpty(friendlyUrlPath) == false && Regex.IsMatch(friendlyUrlPath, @"(^|/)(mid|moduleId)/\d+/?", RegexOptions.IgnoreCase) == false)
            {
                Hashtable friendlyUrlIndex = null; //the friendly url index is the lookup we use
                //try and match incoming friendly url path to what we would expect from the module
                Regex articleUrlRegex = new Regex(@"(?<l>/)?aid/(?<aid>\d+)", RegexOptions.IgnoreCase);
                Match articleUrlMatch = articleUrlRegex.Match(friendlyUrlPath);
                if (articleUrlMatch.Success)
                {

                    //this is a article Url we want to try and modify
                    string rawId = articleUrlMatch.Groups["aid"].Value;
                    int aId = 0;
                    if (int.TryParse(rawId, out aId))
                    {
                        //we have obtained the articleId out of the Url
                        //get the friendlyUrlIndex (it comes from the database via the cache)
                        friendlyUrlIndex = UrlController.GetFriendlyUrlIndex(tab.PortalID, this, options);
                        if (friendlyUrlIndex != null)
                        {
                            //item urls are indexed with i + itemId ("i5") - this is so we could mix and match entities if necessary
                            string furlkey = FriendlyUrlInfo.MakeKey("article", aId);  //create the lookup key for the friendly url index
                            string path = null;
                            //check for a child pages / article ID in the index first
                            if (ArticlePagePathTabId > -1 && tab.ParentId == ArticlePagePathTabId)
                            {
                                string cpFurlKey = "t" + tab.TabID.ToString() + furlkey;
                                path = (string)friendlyUrlIndex[cpFurlKey];//check if in the index for a child page
                            }
                            if (path == null) //now check for a match in the index 
                                path = (string)friendlyUrlIndex[furlkey];//check if in the index
                            if (path == null)
                            {
                                //don't normally expect to have a no-match with a friendly url path when an itemId was in the Url.
                                //could be a new item that has been created and isn't in the index
                                //do a direct call and find out if it's there
                                path = UrlController.CheckForMissingItemId(aId, "article", tab.PortalID, this, options, ref messages);
                            }
                            if (path != null) //got a valid path
                            {
                                //url found in the index for this entry.  So replace the matched part of the path with the friendly url
                                if (articleUrlMatch.Groups["l"].Success) //if the path had a leading /, then make sure to add that onto the replacement
                                    path = base.EnsureLeadingChar("/", path);

                                /* finish it all off */
                                messages.Add("Article Friendly Url Replacing : " + friendlyUrlPath + " in Path : " + path);

                                //this is the point where the Url is modified!
                                //replace the path in the path - which leaves any other parts of a path intact.
                                friendlyUrlPath = articleUrlRegex.Replace(friendlyUrlPath, path);//replace the part in the friendly Url path with it's replacement.

                                //check if this tab is the one specified to not use a path
                                if ((ArticlePagePathTabId == tab.TabID || ArticlePagePathTabId == tab.ParentId) && HideArticlePagePath)
                                    useDnnPagePath = false;//make this Url relative from the site root

                                //set back to default.aspx so that DNN Url Rewriter removes it - just in case it wasn't standard
                                endingPageName = DotNetNuke.Common.Globals.glbDefaultPage;
                            }
                        }
                    }
                }
            }
            return friendlyUrlPath;
        }

        public override bool CheckForRedirect(int tabId, int portalid, string httpAlias, Uri requestUri, NameValueCollection queryStringCol, FriendlyUrlOptions options, out string redirectLocation, ref List<string> messages)
        {
            bool doRedirect = false;
            redirectLocation = "";//set blank location
            //compare to known pattern of old Urls
            string path = requestUri.AbsoluteUri;
            if (string.IsNullOrEmpty(path) == false && Regex.IsMatch(path, @"(^|/)(mid|moduleId)/\d+/?", RegexOptions.IgnoreCase) == false)
            {
                //could be in old /aid/xx format - if so, we want to redirect it
                Regex pathRegex = new Regex(@"/aid/(?<aid>\d+)", RegexOptions.IgnoreCase);
                Match pathMatch = pathRegex.Match(path);
                if (pathMatch.Success)
                {
                    string articleIdRaw = pathMatch.Groups["aid"].Value;
                    int articleId;
                    if (int.TryParse(articleIdRaw, out articleId))
                    {
                        //ok, valid item Id found
                        //get the valid Url for this item
                        Hashtable friendlyUrlIndex = UrlController.GetFriendlyUrlIndex(portalid, this, options);
                        //look up the friendly url index using the item key
                        string furlKey = FriendlyUrlInfo.MakeKey("article", articleId);
                        if (friendlyUrlIndex != null)
                        {
                            string friendlyUrl = null;
                            TabController tc = new TabController();
                            TabInfo tab = tc.GetTab(tabId, portalid, false);
                            if (tab != null && tab.ParentId == ArticlePagePathTabId)
                            {
                                //this is the child tab of the article tab
                                string cpFurlKey = "t" + tabId.ToString() + furlKey;
                                friendlyUrl = (string)friendlyUrlIndex[cpFurlKey];
                            }
                            if (friendlyUrl == null)
                                friendlyUrl = (string)friendlyUrlIndex[furlKey];
                            if (friendlyUrl != null)
                            {
                                //ok, we're going to replace this in the Url
                                if (HideArticlePagePath == false)
                                {
                                    friendlyUrl = base.EnsureLeadingChar("/", friendlyUrl);
                                    string result = pathRegex.Replace(path, friendlyUrl);
                                    doRedirect = true;
                                    redirectLocation = result;
                                }
                                else
                                {
                                    DotNetNuke.Entities.Portals.PortalAliasInfo pa = DotNetNuke.Entities.Portals.PortalAliasController.GetPortalAliasInfo(httpAlias);
                                    if (pa != null)
                                    {
                                        DotNetNuke.Entities.Portals.PortalSettings ps = new DotNetNuke.Entities.Portals.PortalSettings(tabId, pa);
                                        redirectLocation = DotNetNuke.Common.Globals.NavigateURL(tabId, ps, "", "aid=" + articleId);
                                        doRedirect = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return doRedirect;
        }

        public override string TransformFriendlyUrlToQueryString(string[] urlParms, int tabId, int portalId, FriendlyUrlOptions options, string cultureCode, PortalAliasInfo portalAlias, ref List<string> messages, out int status, out string location)
        {
            //initialise results and output variables
            string result = ""; status = 200; //OK 
            location = null; //no redirect location
            if (messages == null) messages = new List<string>();

            Hashtable queryStringIndex = null;
            string path = string.Join("/", urlParms);
            bool found = false;
            bool siteRootMatch = false;
            if (string.IsNullOrEmpty(path) == false && Regex.IsMatch(path, @"(^|/)(mid|moduleId)/\d+/?", RegexOptions.IgnoreCase) == false)
            {
                if (urlParms.Length > 0)
                {
                    //tabid == -1 when no dnn page path is in the Url.  This means the DNN url rewriter can't determine the DNN page based on the Url.
                    //In this case, it is up to this provider to identify the correct tabId that matches the Url.  Failure to do so will result in the incorrect tab being loaded when the page is rendered. 
                    if (tabId == -1)
                    {
                        siteRootMatch = true;
                    }
                    queryStringIndex = UrlController.GetQueryStringIndex(portalId, this, options, false);
                    List<string> keepParms = new List<string>();

                    //TODO: look at making this work for date format/path, will need to figure out how to regex match then pull the date formated content out, this might fix the URL issues for not including the page name
                    string lookupPath = path;
                    //if (string.IsNullOrEmpty(UrlPath) == false && lookupPath.StartsWith(UrlPath))
                    //    lookupPath = lookupPath.Substring(UrlPath.Length);

                    //lookupPath = Regex.Replace(lookupPath, "\\b(?<year>\\d{2,4})/(?<month>\\d{1,2})\\b", UrlDateFormat);
                    //if (lookupPath.Contains(UrlDateFormat))
                    //    lookupPath = lookupPath.Substring(UrlDateFormat.Length);

                    for (int i = urlParms.GetUpperBound(0); i >= 0; i--)
                    {
                        //check for existence of this value in the querystring index
                        string urlParm = urlParms[i];
                        string qsKey = lookupPath.ToLower();
                        string qs = (string)queryStringIndex[qsKey];

                        if (qs == null)
                        {
                            qs = (string)queryStringIndex[path.ToLower()];
                        }

                        if (qs != null)
                        {
                            //found a querystring match
                            found = true;
                            messages.Add("Item Matched in Friendly Url Provider.  Url : " + lookupPath + " Path : " + path);
                            result += qs;
                            break;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(urlParm) == false)
                            {
                                //not found - remove last url parm from lookup path
                                if (lookupPath.Length <= urlParm.Length)
                                    lookupPath = "";
                                else
                                    lookupPath = lookupPath.Remove(lookupPath.Length - (urlParm.Length + 1));
                                keepParms.Insert(0, urlParm);//add from front always
                            }
                        }
                    }
                    if (found)
                    {
                        //put on any remainder of the path that wasn't to do with the friendly Url
                        string remainder = base.CreateQueryStringFromParameters(keepParms.ToArray(), -1);
                        //put it all together for the final rewrite string
                        result += remainder;
                    }
                }
            }
            return result;
        }

        private string GetSafeString(string settingName, string defaultValue)
        {
            string raw = defaultValue;
            if (ProviderConfig.Settings.ContainsKey(settingName))
                raw = this.ProviderConfig.Settings[settingName];
            return raw;
        }

        private bool GetSafeBool(string settingName, bool defaultValue)
        {
            bool result = defaultValue;
            string raw = null;
            if (ProviderConfig.Settings.ContainsKey(settingName))
                raw = this.ProviderConfig.Settings[settingName];
            if (string.IsNullOrEmpty(raw) == false) bool.TryParse(raw, out result);
            return result;
        }

        private int GetSafeInt(string settingName, int defaultValue)
        {
            int result = defaultValue;
            string raw = null;
            if (ProviderConfig.Settings.ContainsKey(settingName))
                raw = this.ProviderConfig.Settings[settingName];
            if (string.IsNullOrEmpty(raw) == false) int.TryParse(raw, out result);
            return result;
        }

        public override Dictionary<string, string> GetProviderPortalSettings()
        {
            return null;
        }


        #region Internal Properties
        internal new string CleanNameForUrl(string title, FriendlyUrlOptions options)
        {
            bool replaced = false;
            //the base module Url PRovider contains a Url cleaning routine, which will remove illegal 
            //and unwanted characters from a string, using the specific friendly url options
            return base.CleanNameForUrl(title, options, out replaced);
        }
        internal int ArticlePagePathTabId
        {
            get
            {
                int articlePageTabId = GetSafeInt(Constants.ArticlePageTabIdSettingName, -1);
                return articlePageTabId;
            }
        }
        internal bool HideArticlePagePath
        {
            get
            {
                bool hideArticlePagePath = GetSafeBool(Constants.HideArticlePathPageSettingName, false);
                return hideArticlePagePath;
            }
        }

        internal string UrlDateFormat
        {
            get
            {
                string urlDateFormat = GetSafeString(Constants.UrlDateFormatSettingName, "");
                return urlDateFormat;

            }
        }
        #endregion
    }
}