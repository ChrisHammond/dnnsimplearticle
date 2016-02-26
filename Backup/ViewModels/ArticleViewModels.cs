using System.Collections.Generic;

namespace DotNetNuke.Modules.dnnsimplearticle.ViewModels
{
    public class ArticleViewModels
    {
        //todo: removed total articles for now
        //public int TotalArticles { get; set; }
        public IList<ArticleViewModel> Articles { get; set; }
    }
}