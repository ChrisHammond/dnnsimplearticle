using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Christoc.Modules.dnnsimplearticle.Components
{
    internal class FriendlyUrlInfo
    {
        internal int ItemId { get; set; }
        internal string ItemType { get; set; }
        internal string UrlFragment1 { get; set; }
        internal string UrlFragment2 { get; set; }

        internal string ItemKey
        {
            get
            {
                return MakeKey(this.ItemType, this.ItemId);
            }
        }

        internal string QueryStringKey
        {
            get
            {
                return GetQueryStringKey(this.ItemType);
            }
        }

        internal static string GetQueryStringKey(string itemType)
        {
            string result = null;
            switch (itemType.ToLower())
            {
                case "article":
                    result = "aid";
                    break;
            }
            return result;
        }

        internal static string MakeKey(string itemType, int itemId)
        {
            string result = "";
            switch (itemType.ToLower())
            {
                case "article":
                    result = "a" + itemId.ToString();
                    break;

            }
            return result;
        }
    }

    internal class FriendlyUrlInfoCol : List<FriendlyUrlInfo>
    {

    }
}