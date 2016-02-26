using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DotNetNuke.Instrumentation;
using DotNetNuke.Modules.dnnsimplearticle.Components;
using DotNetNuke.Modules.dnnsimplearticle.ViewModels;
using DotNetNuke.Security;
using DotNetNuke.Web.Services;

namespace DotNetNuke.Modules.dnnsimplearticle.services
{

    public class DnnSimpleArticleController : DnnController
    {
        [DnnAuthorize(AllowAnonymous = true)]
        public ActionResult GetArticles(int moduleId)
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
                                         Body = Server.HtmlDecode(a.Body),
                                         CreatedByUser = a.CreatedByUser,
                                         CreatedByUserId = a.CreatedByUserId,
                                         CreatedOnDate = a.CreatedOnDate,
                                         Description = Server.HtmlDecode(a.Description),
                                         LastModifiedByUser = a.LastUpdatedByUser,
                                         LastModifiedByUserId = a.LastModifiedByUserId,
                                         LastModifiedOnDate = a.LastModifiedOnDate,
                                         ModuleId = a.ModuleId,
                                         Title = a.Title,
                                         url = Common.Globals.NavigateURL(a.TabID,"", "&aid=" + a.ArticleId)
                                     };
                    cleanArticles.Add(newArt);
                }

                var articleViewModels = new ArticleViewModels
                                            {
                                                Articles = cleanArticles
                                            };
                return Json(articleViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                DnnLog.Error(exc);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [DnnAuthorize(AllowAnonymous = true)]
        public ActionResult GetAllArticles(int portalId, bool sortAsc)
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
                        Body = Server.HtmlDecode(a.Body),
                        CreatedByUser = a.CreatedByUser,
                        CreatedByUserId = a.CreatedByUserId,
                        CreatedOnDate = a.CreatedOnDate,
                        Description = Server.HtmlDecode(a.Description),
                        LastModifiedByUser = a.LastUpdatedByUser,
                        LastModifiedByUserId = a.LastModifiedByUserId,
                        LastModifiedOnDate = a.LastModifiedOnDate,
                        ModuleId = a.ModuleId,
                        Title = a.Title,
                        url = Common.Globals.NavigateURL(a.TabID, "", "&aid=" + a.ArticleId)
                    };
                    cleanArticles.Add(newArt);
                }

                var articleViewModels = new ArticleViewModels
                {
                    Articles = cleanArticles
                };
                return Json(articleViewModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                DnnLog.Error(exc);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [DnnAuthorize(AllowAnonymous = true)]
        public ActionResult GetArticle(int articleId)
        {
            try
            {
                var a = ArticleController.GetArticle(articleId);

                var newArt = new ArticleViewModel
                {
                    ArticleId = a.ArticleId,
                    Body = Server.HtmlDecode(a.Body),
                    CreatedByUser = a.CreatedByUser,
                    CreatedByUserId = a.CreatedByUserId,
                    CreatedOnDate = a.CreatedOnDate,
                    Description = Server.HtmlDecode(a.Description),
                    LastModifiedByUser = a.LastUpdatedByUser,
                    LastModifiedByUserId = a.LastModifiedByUserId,
                    LastModifiedOnDate = a.LastModifiedOnDate,
                    ModuleId = a.ModuleId,
                    Title = a.Title,
                    url = Common.Globals.NavigateURL(a.TabID, "", "&aid=" + a.ArticleId)
                };

                return Json(newArt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                DnnLog.Error(exc);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }

}