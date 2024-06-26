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

using System.Data;
using System;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;


namespace Christoc.Modules.dnnsimplearticle.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class for the data access layer
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

        #region Shared/Static Methods

        private static DataProvider _provider;

        // return the provider
        ///<summary>
        /// Gets an instance of the dataprovider object for use
        ///</summary>
        ///<returns></returns>
        public static DataProvider Instance()
        {
            if (_provider == null)
            {
                const string assembly = "Christoc.Modules.dnnsimplearticle.Data.SqlDataprovider,dnnsimplearticle";
                Type objectType = Type.GetType(assembly, true, true);

                if (objectType != null)
                {
                    _provider = (DataProvider)Activator.CreateInstance(objectType);
                    DataCache.SetCache(objectType.FullName, _provider);
                }
            }

            return _provider;
        }

        ///<summary>
        /// Deals with getting the connection for our data provider
        ///</summary>
        ///<returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            var objProvider = ((Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider]);
            string connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection {ConnectionString = connectionString};
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract methods

        //public abstract IDataReader GetItems(int userId, int portalId);

        //public abstract IDataReader GetItem(int itemId);        

        ///<summary>
        /// Abstract for getting a single article
        ///</summary>
        ///<param name="articleId"></param>
        ///<returns></returns>
        public abstract IDataReader GetArticle(int articleId);
        ///<summary>
        /// Abstract for getting a list of articles
        ///</summary>
        ///<param name="moduleId"></param>
        ///<param name="pageSize"></param>
        ///<param name="pageNumber"></param>
        ///<returns></returns>
        public abstract IDataReader GetArticles(int moduleId, int pageSize, int pageNumber);

        ///<summary>
        /// Abstract for getting a list of all articles for a portalid
        ///</summary>
        ///<param name="portalId"></param>
        ///<returns></returns>
        public abstract IDataReader GetAllArticles(int portalId);

        ///<summary>
        /// Abstract for getting a list of all articles for a portalid with a sort flag passed
        ///</summary>
        ///<param name="portalId"></param>
        ///<param name="sortAsc"></param>
        ///<returns></returns>
        public abstract IDataReader GetAllArticles(int portalId, bool sortAsc);

        ///<summary>
        /// The add article astract
        ///</summary>
        ///<param name="a"></param>
        ///<returns></returns>
        public abstract int AddArticle(Article a);
        ///<summary>
        /// Updating an article
        ///</summary>
        ///<param name="a"></param>
        public abstract void UpdateArticle(Article a);
        ///<summary>
        /// Delete an article
        ///</summary>
        ///<param name="articleId"></param>
        public abstract void DeleteArticle(int articleId);
        ///<summary>delete all articles for a module id
        ///</summary>
        ///<param name="moduleId"></param>
        public abstract void DeleteArticles(int moduleId);


        ///<summary>
        /// Abstract for getting a single template
        ///</summary>
        ///<param name="templateId"></param>
        ///<returns></returns>
        public abstract IDataReader GetTemplate(int templateId);


        internal abstract void GetArticleUrls(int portalid, out FriendlyUrlInfoCol urls);
        internal abstract void GetArticleUrl(int portalId, int itemId, string itemType, out FriendlyUrlInfo url);


        #endregion

    }

}