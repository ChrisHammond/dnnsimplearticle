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
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Modules;
using Christoc.Modules.dnnsimplearticle.Components;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.UI.WebControls;
using Globals = DotNetNuke.Common.Globals;
using Christoc.Modules.dnnsimplearticle.Components.Templates;
using System.Text;

namespace Christoc.Modules.dnnsimplearticle.Controls
{
    ///<summary>
    /// A simple control that binds a list of articles
    ///</summary>
    public partial class FlexibleList : dnnsimplearticleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                FlexibleListWrapper.Attributes.Add("class","row template-" + 1);


                //TODO: get the template
                var t = TemplateController.GetTemplate(1);
                var sb = new StringBuilder();
                
                //get the articles
                var arts = ArticleController.GetArticles(ModuleId, PageSize, PageNumber);

                //templatize the articles
                foreach(var a in arts)
                {
                    var newT = string.Empty;
                    newT = t.TemplateContent.ToString();
                    newT = newT.Replace("[TITLE]", a.Title);
                    newT = newT.Replace("[ID]", a.ArticleId.ToString());
                    newT = newT.Replace("[IMGURL]", a.ImageUrl);
                    newT = newT.Replace("[CREATEDONDATE]", a.CreatedOnDate.ToString());
                    newT = newT.Replace("[URL]", ArticleController.GetArticleLink(TabId, Convert.ToInt32(a.ArticleId)));
                    sb.Append(newT);
                }

                var l = new Literal();
                l.Text = sb.ToString();
                //TODO: render the articles
                phFlexibleList.Controls.Add(l);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

       
        

    }
}