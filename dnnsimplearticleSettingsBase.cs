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

using DotNetNuke.Entities.Modules;
using System;

namespace Christoc.Modules.dnnsimplearticle
{

    public class dnnsimplearticleSettingsBase : ModuleSettingsBase
    {

        public int PageSize
        {
            get
            {
                if (Settings.Contains("PageSize"))
                    return Convert.ToInt32(Settings["PageSize"]);
                return 10;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "PageSize", value.ToString());
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

        public bool CleanRss
        {
            get
            {
                if (Settings.Contains("CleanRss"))
                {
                    return Convert.ToBoolean(Settings["CleanRss"]);
                }
                return false;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "CleanRss", value.ToString());
            }

        }

        public bool FullArticleRss
        {
            get
            {
                if (Settings.Contains("FullArticleRss"))
                {
                    return Convert.ToBoolean(Settings["FullArticleRss"]);
                }
                return false;
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "FullArticleRss", value.ToString());
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


    }

}
