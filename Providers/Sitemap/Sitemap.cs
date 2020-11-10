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
using DotNetNuke.Entities.Portals;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Services.Sitemap;

namespace Christoc.Modules.dnnsimplearticle.Providers.Sitemap
{
    public class Sitemap : SitemapProvider
    {

        public override List<SitemapUrl> GetUrls(int portalId, PortalSettings ps, string version)
        {
            var listOfUrls = new List<SitemapUrl>();

            foreach (Article ai in ArticleController.GetAllArticles(portalId))
            {
                
                var pageUrl = new SitemapUrl
                              {
                                  Url =
                                      ArticleController.GetArticleLink(ai.TabID, ai.ArticleId),
                                  Priority = (float)0.9,
                                  LastModified = ai.LastModifiedOnDate,
                                  ChangeFrequency = SitemapChangeFrequency.Daily
                              };
                listOfUrls.Add(pageUrl);

            }
            return listOfUrls;
        }
    }
}
