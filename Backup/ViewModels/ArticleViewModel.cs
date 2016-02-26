using System;
using System.Collections.Generic;

namespace DotNetNuke.Modules.dnnsimplearticle.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string CreatedByUser { get; set; }
        public int CreatedByUserId { get; set; }
        public string LastModifiedByUser { get; set; }
        public int LastModifiedByUserId { get; set; }
        public new DateTime CreatedOnDate { get; set; }
        public new DateTime LastModifiedOnDate { get; set; }
        public int ModuleId { get; set; }
        public int PortalId { get; set; }
        public string url { get; set; }
        
        //public IList<NotificationActionViewModel> Actions { get; set; }

    }
}