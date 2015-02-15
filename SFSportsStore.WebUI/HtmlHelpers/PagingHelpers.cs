using System;
using System.Text;
using System.Web.Mvc;
using SFSportsStore.WebUI.Models;

namespace SFSportsStore.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            //For each page
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                //Create a new a tag
                TagBuilder tag = new TagBuilder("a");
                //Passed page url function - to create the urls pass it value of i
                tag.MergeAttribute("href", pageUrl(i));
                //Display page number
                tag.InnerHtml = i.ToString();
                //If this is the current page
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                //Append each page link to result
                result.Append(tag.ToString());
            }
            //Return HTLM
            return MvcHtmlString.Create(result.ToString());
        }
    }
}