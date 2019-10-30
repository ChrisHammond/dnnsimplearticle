using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//based off of
//https://github.com/fomenta/DnnUrlProvider

namespace Christoc.Modules.dnnsimplearticle.Components
{
    internal static class UrlController
    {

        /// <summary>
        /// Checks for, and adds to the indexes, a missing item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="tabId"></param>
        /// <param name="portalId"></param>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        /// <param name="messages"></param>
        /// <returns>Valid path if found</returns>
        internal static string CheckForMissingItemId(int itemId, string itemType, int portalId, UrlProvider provider, FriendlyUrlOptions options, ref List<string> messages)
        {
            string path = null;
            FriendlyUrlInfo friendlyUrl = null;
            messages.Add("Item not found Id : " + itemId.ToString() + ", Type : " + itemType + " Checking Item directly");
            Data.DataProvider.Instance().GetArticleUrl(portalId, itemId, itemType, out friendlyUrl);
            if (friendlyUrl != null)
            {
                messages.Add("itemId found : " + itemId.ToString() + " , Type : " + itemType + " - Rebuilding indexes");
                //call and get the path
                path = UrlController.MakeItemFriendlyUrl(friendlyUrl, provider, options);
                //so this entry did exist but wasn't in the index.  Rebuild the index
                UrlController.RebuildIndexes(portalId, provider, options);
            }
            return path;
        }

        /// <summary>
        /// Creates a Friendly Url For the Item
        /// </summary>
        /// <param name="friendlyUrl">Object containing the relevant properties to create a friendly url from</param>
        /// <param name="provider">The active module provider</param>
        /// <param name="options">THe current friendly Url Options</param>
        /// <returns></returns>
        private static string MakeItemFriendlyUrl(FriendlyUrlInfo friendlyUrl, UrlProvider provider, FriendlyUrlOptions options)
        {
            //calls back up the module provider to utilise the CleanNameForUrl method, which creates a safe Url using the current Friendly Url options.
            string friendlyUrlPath = provider.CleanNameForUrl(friendlyUrl.UrlFragment1, options);
            return friendlyUrlPath;
        }
        /// <summary>
        /// Returns a friendly url index from the cache or database.
        /// </summary>
        /// <param name="tabId"></param>
        /// <param name="portalId"></param>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static Hashtable GetFriendlyUrlIndex(int portalId, UrlProvider provider, FriendlyUrlOptions options)
        {
            string furlCacheKey = GetFriendlyUrlIndexKeyForPortal(portalId);
            Hashtable friendlyUrlIndex = DataCache.GetCache<Hashtable>(furlCacheKey);
            if (friendlyUrlIndex == null)
            {
                string qsCacheKey = GetQueryStringIndexCacheKeyForPortal(portalId);
                Hashtable queryStringIndex = null;
                //build index for tab
                BuildUrlIndexes(portalId, provider, options, out friendlyUrlIndex, out queryStringIndex);
                StoreIndexes(friendlyUrlIndex, furlCacheKey, queryStringIndex, qsCacheKey);
            }
            return friendlyUrlIndex;

        }
        /// <summary>
        /// Return the index of all the querystrings that belong to friendly urls for the specific tab.
        /// </summary>
        /// <param name="tabId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        internal static Hashtable GetQueryStringIndex(int portalId, UrlProvider provider, FriendlyUrlOptions options, bool forceRebuild)
        {
            string qsCacheKey = GetQueryStringIndexCacheKeyForPortal(portalId);
            Hashtable queryStringIndex = DataCache.GetCache<Hashtable>(qsCacheKey);
            if (queryStringIndex == null || forceRebuild)
            {
                string furlCacheKey = GetFriendlyUrlIndexKeyForPortal(portalId);
                Hashtable friendlyUrlIndex = null;
                //build index for tab
                BuildUrlIndexes(portalId, provider, options, out friendlyUrlIndex, out queryStringIndex);
                StoreIndexes(friendlyUrlIndex, furlCacheKey, queryStringIndex, qsCacheKey);
            }
            return queryStringIndex;
        }

        private static void BuildUrlIndexes(int portalId, UrlProvider provider, FriendlyUrlOptions options, out Hashtable friendlyUrlIndex, out Hashtable queryStringIndex)
        {
            friendlyUrlIndex = new Hashtable();
            queryStringIndex = new Hashtable();
            //call database procedure to get list of 
            FriendlyUrlInfoCol itemUrls = null;
            Data.DataProvider.Instance().GetArticleUrls(portalId, out itemUrls);
            List<TabInfo> childTabs = null;
            TabController tc = new TabController();
            TabInfo articleParentTab = null;
            string articleParentTabPath = null;
            //get a list of child tabs for this request
            if (provider.ArticlePagePathTabId > 0 && provider.HideArticlePagePath)
            {
                childTabs = DotNetNuke.Entities.Tabs.TabController.GetTabsByParent(provider.ArticlePagePathTabId, portalId);
                articleParentTab = tc.GetTab(provider.ArticlePagePathTabId, portalId, false);
                articleParentTabPath = provider.CleanNameForUrl(articleParentTab.TabName, options);
            }
            if (itemUrls != null)
            {
                //build up the dictionary
                foreach (FriendlyUrlInfo itemUrl in itemUrls)
                {
                    //friendly url index - look up by entryid, find Url

                    string furlKey = itemUrl.ItemKey;
                    string furlValue = MakeItemFriendlyUrl(itemUrl, provider, options);
                    if (friendlyUrlIndex.ContainsKey(furlKey) == false)
                        friendlyUrlIndex.Add(furlKey, furlValue);

                    //querystring index - look up by url, find querystring for the item
                    string qsKey = furlValue.ToLower();//the querystring lookup is the friendly Url value - but converted to lower case

                    string itemQueryString = "&" + itemUrl.QueryStringKey + "=" + itemUrl.ItemId.ToString();//the querystring that is rewritten for the url
                    string qsValue = itemQueryString;

                    //when not including the dnn page path into the friendly Url, then include the tabid in the querystring
                    if (itemUrl.ItemType.ToLower() == "article" && provider.HideArticlePagePath == true && provider.ArticlePagePathTabId > 0)
                        qsValue = "?TabId=" + provider.ArticlePagePathTabId.ToString() + qsValue;

                    if (queryStringIndex.ContainsKey(qsKey) == false)
                        queryStringIndex.Add(qsKey, qsValue);

                    //if the options aren't standard, also add in some other versions that will identify the right entry but will get redirected
                    if (options.PunctuationReplacement != "")
                    {
                        FriendlyUrlOptions altOptions = options.Clone();
                        altOptions.PunctuationReplacement = "";//how the urls look with no replacement
                        string altQsKey = MakeItemFriendlyUrl(itemUrl, provider, altOptions).ToLower();//keys are always lowercase
                        if (altQsKey != qsKey)
                        {
                            if (queryStringIndex.ContainsKey(altQsKey) == false)
                                queryStringIndex.Add(altQsKey, qsValue);
                        }
                    }

                    //now repeat for the child tabs
                    if (childTabs != null && childTabs.Count > 0 && articleParentTab != null)
                    {
                        //derive the path of the child tabs in with the parent tab, and build up a set of urls that include the article name and the child tab name
                        foreach (TabInfo childTab in childTabs)
                        {
                            string childPath = provider.CleanNameForUrl(childTab.TabName, options);//gets the cleaned tab name
                            childPath = furlValue + "/" + provider.EnsureNotLeadingChar("/", childPath);
                            //the child path is the user + the child tab path
                            //add rewriting values
                            string cpQsValue = "?TabId=" + childTab.TabID.ToString() + itemQueryString;
                            string cpQsKey = childPath.ToLower();
                            if (queryStringIndex.ContainsKey(cpQsKey) == false)
                                queryStringIndex.Add(cpQsKey, cpQsValue);
                            //add friendly url paths
                            string cpFurlKey = "t" + childTab.TabID.ToString() + furlKey;
                            string cpFurlValue = childPath;
                            if (friendlyUrlIndex.ContainsKey(cpFurlKey) == false)
                                friendlyUrlIndex.Add(cpFurlKey, cpFurlValue);
                            //add alternatives
                            if (options.PunctuationReplacement != "")
                            {
                                FriendlyUrlOptions altOptions = options.Clone();
                                altOptions.PunctuationReplacement = "";//how the urls look with no replacement
                                string altQsKey = childPath + "/" + MakeItemFriendlyUrl(itemUrl, provider, altOptions).ToLower();//keys are always lowercase
                                if (altQsKey != qsKey)
                                {
                                    if (queryStringIndex.ContainsKey(altQsKey) == false)
                                        queryStringIndex.Add(altQsKey, cpQsValue);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// REbuilds the two indexes and re-stores them into the cache
        /// </summary>
        /// <param name="tabId"></param>
        /// <param name="portalId"></param>
        /// <param name="provider"></param>
        /// <param name="options"></param>
        private static void RebuildIndexes(int portalId, UrlProvider provider, FriendlyUrlOptions options)
        {
            Hashtable queryStringIndex = null;
            Hashtable friendlyUrlIndex = null;
            string qsCacheKey = GetQueryStringIndexCacheKeyForPortal(portalId);
            string furlCacheKey = GetFriendlyUrlIndexKeyForPortal(portalId);
            //build index for tab
            BuildUrlIndexes(portalId, provider, options, out friendlyUrlIndex, out queryStringIndex);
            StoreIndexes(friendlyUrlIndex, furlCacheKey, queryStringIndex, qsCacheKey);
        }
        /// <summary>
        /// Store the two indexes into the cache
        /// </summary>
        /// <param name="friendlyUrlIndex"></param>
        /// <param name="friendlyUrlCacheKey"></param>
        /// <param name="queryStringIndex"></param>
        /// <param name="queryStringCacheKey"></param>
        private static void StoreIndexes(Hashtable friendlyUrlIndex, string friendlyUrlCacheKey, Hashtable queryStringIndex, string queryStringCacheKey)
        {
            TimeSpan expire = new TimeSpan(24, 0, 0);
            DataCache.SetCache(friendlyUrlCacheKey, friendlyUrlIndex, expire);
            DataCache.SetCache(queryStringCacheKey, queryStringIndex, expire);
        }

        /// <summary>
        /// Return the caceh key for a tab index
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static string GetFriendlyUrlIndexKeyForPortal(int portalId)
        {
            return string.Format(Constants.FriendlyUrlIndexKey, portalId);
        }
        private static string GetQueryStringIndexCacheKeyForPortal(int portalId)
        {
            return string.Format(Constants.QueryStringIndexKey, portalId);
        }
    }
}