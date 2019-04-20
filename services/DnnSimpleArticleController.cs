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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DotNetNuke.Instrumentation;
using Christoc.Modules.dnnsimplearticle.Components;
using Christoc.Modules.dnnsimplearticle.ViewModels;
using DotNetNuke.Web.Api;

namespace Christoc.Modules.dnnsimplearticle.services
{

    public class DnnSimpleArticleController : DnnApiController
    {
        [DnnAuthorize()]
        public HttpResponseMessage GetArticles()
        {
            return GetArticles(ActiveModule.ModuleID);
        }

        //[DnnAuthorize(AllowAnonymous = true)]
        public HttpResponseMessage GetArticles(int moduleId)
        {
            try
            {
                //todo: get the latest X articles?
                var articles = ArticleController.GetArticles(moduleId);

                //because of the circular reference when cerealizing the taxonomy within content items we have to build out our article view models manually.
                var cleanArticles = new List<ArticleViewModel>();
                foreach (Article a in articles)
                {
                    var newArt = new ArticleViewModel
                                     {
                                         ArticleId = a.ArticleId,
                                         Body = WebUtility.HtmlDecode(a.Body),
                                         CreatedByUser = a.CreatedByUser,
                                         CreatedByUserId = a.CreatedByUserId,
                                         CreatedOnDate = a.CreatedOnDate,
                                         Description = WebUtility.HtmlDecode(a.Description),
                                         LastModifiedByUser = a.LastUpdatedByUser,
                                         LastModifiedByUserId = a.LastModifiedByUserId,
                                         LastModifiedOnDate = a.LastModifiedOnDate,
                                         ModuleId = a.ModuleId,
                                         Title = a.Title,
                                         url = DotNetNuke.Common.Globals.NavigateURL(a.TabID, "", "&aid=" + a.ArticleId)
                                     };
                    cleanArticles.Add(newArt);
                }

                var articleViewModels = new ArticleViewModels
                                            {
                                                Articles = cleanArticles
                                            };

                return Request.CreateResponse(HttpStatusCode.OK, articleViewModels, new MediaTypeHeaderValue("text/json"));
                //to force JSON as the result 
                //return Request.CreateResponse(HttpStatusCode.OK, articleViewModels, new MediaTypeHeaderValue("text/json"));
                
            }
            catch (Exception exc)
            {
                DnnLog.Error(exc); //todo: obsolete
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error in request"); //todo: probably should localize that?
            }
        }

        //[DnnAuthorize(AllowAnonymous = true)]
        public HttpResponseMessage GetAllArticles(bool sortAsc)
        {
            return GetAllArticles(ActiveModule.PortalID, sortAsc);
        }


        //[DnnAuthorize(AllowAnonymous = true)]
        
        public HttpResponseMessage GetAllArticles(int portalId, bool sortAsc)
        {
            try
            {
                //todo: get the latest X articles?
                var articles = ArticleController.GetAllArticles(portalId, sortAsc);

                //because of the circular reference when cerealizing the taxonomy within content items we have to build out our article view models manually.
                var cleanArticles = new List<ArticleViewModel>();
                foreach (Article a in articles)
                {
                    var newArt = new ArticleViewModel
                    {
                        ArticleId = a.ArticleId,
                        Body = WebUtility.HtmlDecode(a.Body),
                        CreatedByUser = a.CreatedByUser,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedOnDate = a.CreatedOnDate,
                        Description = WebUtility.HtmlDecode(a.Description),
                        LastModifiedByUser = a.LastUpdatedByUser,
                        LastModifiedByUserId = a.LastModifiedByUserId,
                        LastModifiedOnDate = a.LastModifiedOnDate,
                        ModuleId = a.ModuleId,
                        Title = a.Title,
                        url = DotNetNuke.Common.Globals.NavigateURL(a.TabID, "", "&aid=" + a.ArticleId)
                    };
                    cleanArticles.Add(newArt);
                }

                var articleViewModels = new ArticleViewModels
                {
                    Articles = cleanArticles
                };

                return Request.CreateResponse(HttpStatusCode.OK, articles);

            }
            catch (Exception exc)
            {
                DnnLog.Error(exc); //todo: obsolete
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error in request"); //todo: probably should localize that?
            }
        }

        //[DnnAuthorize(AllowAnonymous = true)]
        public HttpResponseMessage GetArticle(int articleId)
        {
            try
            {
                var a = ArticleController.GetArticle(articleId);

                var newArt = new ArticleViewModel
                {
                    ArticleId = a.ArticleId,
                    Body = WebUtility.HtmlDecode(a.Body),
                    CreatedByUser = a.CreatedByUser,
                    CreatedByUserId = a.CreatedByUserId,
                    CreatedOnDate = a.CreatedOnDate,
                    Description = WebUtility.HtmlDecode(a.Description),
                    LastModifiedByUser = a.LastUpdatedByUser,
                    LastModifiedByUserId = a.LastModifiedByUserId,
                    LastModifiedOnDate = a.LastModifiedOnDate,
                    ModuleId = a.ModuleId,
                    Title = a.Title,
                    url = DotNetNuke.Common.Globals.NavigateURL(a.TabID, "", "&aid=" + a.ArticleId)
                };

                return Request.CreateResponse(HttpStatusCode.OK, newArt);

            }
            catch (Exception exc)
            {
                DnnLog.Error(exc); //todo: obsolete
                return Request.CreateResponse(HttpStatusCode.BadRequest, "error in request"); //todo: probably should localize that?

            }
        }

    }

}