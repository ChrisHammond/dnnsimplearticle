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
using DotNetNuke.Entities.Modules;
using Christoc.Modules.dnnsimplearticle.Components;

namespace Christoc.Modules.dnnsimplearticle
{

    public class dnnsimplearticleModuleBase : PortalModuleBase
    {
        //with this base class you can provide any custom properties and methods that all your controls can access here, you can also access all the DNN 
        // methods and properties available off of portalmodulebase such as TabId, UserId, UserInfo, etc.

        public int ArticleId
        {
            get
            {
                var qs = Request.QueryString["aid"];
                if (qs != null)
                    return Convert.ToInt32(qs);
                return -1;
            }
        }

        public int PageNumber
        {
            get
            {
                var qs = Request.QueryString["p"];
                if (qs != null)
                    return Convert.ToInt32(qs);
                return 0;
            }
        }

        public int PageSize
        {
            get
            {
                if(Settings.Contains("PageSize"))
                    return Convert.ToInt32(Settings["PageSize"]);
                return 10;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId,"PageSize",value.ToString());
            }
        }

        public bool ShowCategories
        {
            get
            {
                if (Settings.Contains("ShowCategories"))
                {
                    return Convert.ToBoolean(Settings["ShowCategories"]);
                }
                return true;
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ShowCategories", value.ToString());
            }

        }

        public bool EnableRichDescriptions
        {
            get
            {
                if (Settings.Contains("EnableRichDescriptions"))
                {
                    return Convert.ToBoolean(Settings["EnableRichDescriptions"]);
                }
                return false;
            }

            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EnableRichDescriptions", value.ToString());
            }

        }


        public string DisplayType
        {
            get
            {
                if (Settings.Contains("DisplayType"))
                    return Settings["DisplayType"].ToString();
                return string.Empty;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "DisplayType", value.ToString());
            }
        }


        public string GetArticleLink(string articleId)
        {
            return ArticleController.GetArticleLink(TabId, Convert.ToInt32(articleId));
        }
    }

}
