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
using System.Data;
using System.Data.SqlClient;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Christoc.Modules.dnnsimplearticle.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "DNNSimpleArticle_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        ///<summary>
        /// Our concrete data provider implementation
        ///</summary>
        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            var objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        ///<summary>Connection string
        ///</summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        ///<summary>The path for the provider
        ///</summary>
        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        ///<summary>
        /// The object qualifier used on the database, found in the DNN web.config file
        ///</summary>
        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        ///<summary>
        /// The database owner setting from the web.config file
        ///</summary>
        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion


        #region Public Methods

        public override IDataReader GetTemplate(int templateId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetTemplate", new SqlParameter("@TemplateId", templateId));
        }



        public override IDataReader GetArticle(int articleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetArticle", new SqlParameter("@ArticleId", articleId));
        }


        public override IDataReader GetArticles(int moduleId, int pageSize, int pageNumber)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetArticles", new SqlParameter("@ModuleId", moduleId)
                , new SqlParameter("@PageSize", pageSize)
                , new SqlParameter("@PageIndex", pageNumber)
                );
        }

        public override IDataReader GetAllArticles(int portalId)
        {
            return GetAllArticles(portalId, false);
        }

        public override IDataReader GetAllArticles(int portalId, bool sortAsc)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetAllArticles", new SqlParameter("@PortalId", portalId), new SqlParameter("@sortAsc", sortAsc));
        }

        

        public override int AddArticle(Article a)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spAddArticle"
                , new SqlParameter("@ModuleId", a.ModuleId)
                , new SqlParameter("@Title", a.Title)
                , new SqlParameter("@Description", a.Description)
                , new SqlParameter("@Body", a.Body)
                , new SqlParameter("@CreatedOnDate", a.CreatedOnDate)
                , new SqlParameter("@CreatedByUserId", a.CreatedByUserId)
                , new SqlParameter("@LastModifiedOnDate", a.LastModifiedOnDate)
                , new SqlParameter("@LastModifiedByUserId", a.LastModifiedByUserId)
                ));
        }
        public override void DeleteArticle(int articleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spDeleteArticle", new SqlParameter("@ArticleId", articleId));
        }

        public override void DeleteArticles(int moduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spDeleteArticles", new SqlParameter("@ModuleId", moduleId));
        }

        public override void UpdateArticle(Article a)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "spUpdateArticle"
                , new SqlParameter("@ArticleId", a.ArticleId)
                , new SqlParameter("@ModuleId", a.ModuleId)
               , new SqlParameter("@Title", a.Title)
               , new SqlParameter("@Description", a.Description)
               , new SqlParameter("@Body", a.Body)
               , new SqlParameter("@LastModifiedOnDate", a.LastModifiedOnDate)
               , new SqlParameter("@LastModifiedByUserId", a.LastModifiedByUserId)
               , new SqlParameter("@ContentItemId", a.ContentItemId)
               );
        }



        #endregion

        #region URLS
        internal override void GetArticleUrls(int portalId, out FriendlyUrlInfoCol urls)
        {
            urls = new FriendlyUrlInfoCol();
            string sp = NamePrefix + "spGetArticleUrls";
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@portalId", portalId);
            SqlDataReader rdr = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, sp, parms);
            while (rdr.Read())
            {
                FriendlyUrlInfo url = new FriendlyUrlInfo();
                if (!Convert.IsDBNull(rdr["PermaLink"]))
                    url.UrlFragment1 = (string)rdr["PermaLink"];
                if (!Convert.IsDBNull(rdr["CreatedOnDate"]))
                    url.UrlFragment2 = ((DateTime)rdr["CreatedOnDate"]).ToString();
                url.ItemType = "Article";
                if (!Convert.IsDBNull(rdr["Id"]))
                    url.ItemId = (int)rdr["Id"];
                //add to collection
                urls.Add(url);
            }

            rdr.Close();
            rdr.Dispose();
        }
        internal override void GetArticleUrl(int portalId, int itemId, string itemType, out FriendlyUrlInfo url)
        {
            url = null;
            string sp = NamePrefix + "spGetArticleUrl";
            SqlParameter[] parms = new SqlParameter[3];
            parms[0] = new SqlParameter("@portalId", portalId);
            parms[1] = new SqlParameter("@articleId", itemId);
            //parms[2] = new SqlParameter("@itemType", itemType);
            SqlDataReader rdr = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, sp, parms);
            if (rdr.Read())
            {
                url = new FriendlyUrlInfo();
                if (!Convert.IsDBNull(rdr["PermaLink"]))
                    url.UrlFragment1 = (string)rdr["PermaLink"];
                if (!Convert.IsDBNull(rdr["CreatedOnDate"]))
                    url.UrlFragment2 = ((DateTime)rdr["CreatedOnDate"]).ToString();
                url.ItemType = "Article";
                if (!Convert.IsDBNull(rdr["Id"]))
                    url.ItemId = (int)rdr["Id"];
            }

            rdr.Close();
            rdr.Dispose();
        }
        #endregion

    }

}