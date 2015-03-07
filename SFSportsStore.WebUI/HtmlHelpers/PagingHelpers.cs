using System;
using System.Text;
using System.Web.Mvc;
using SFSportsStore.WebUI.Models;

namespace SFSportsStore.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {

        //HTML paging helper - HTML helper ext: takes paging info and delegate (int as param returns string) for url building.
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper, PagingInfo pagingInfo, Func<int, string> urlBuildDelg)
        {
            //Init stringbuilder - to build result string
            StringBuilder resultString = new StringBuilder();

            //For each page in paging info
            for(int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                //Create a new anchor tag
                TagBuilder htmlTag = new TagBuilder("a");
                //Add the href attr to the attrs of the html tag - using the passed func (delg) to build the value - takes int as param and returns a string
                htmlTag.MergeAttribute("href", urlBuildDelg(i));
                //If the current page in pagingInfo matches the current iteration of pages in paging info
                if (i == pagingInfo.CurrentPage)
                {
                    htmlTag.AddCssClass("btn-primary selected");
                }
                htmlTag.AddCssClass("btn btn-default");
                //Set the page numer display
                htmlTag.InnerHtml = i.ToString();
                //Append the current tag to the result string
                resultString.Append(htmlTag.ToString());
            }
            //Create mvc html string passing the stringbuilder converted to string
            return MvcHtmlString.Create(resultString.ToString());
        }
    }
}
