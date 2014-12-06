using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Loveii;

namespace System.Web.Mvc
{
    /// <summary>
    /// HTML扩展类
    /// </summary>
    public static class HtmlExtension
    {
        public static string Pagination<T>(this HtmlHelper helper, PageResult<T> pageResult, string urlLink, string urlPrefix)
        {
            StringBuilder html = new StringBuilder();

            if (string.IsNullOrEmpty(urlLink))
                return html.ToString();
            //总页数
            int maxPageNums = (pageResult.TotalCount == 0 || pageResult.PageSize == 0) ? 0 : ((pageResult.TotalCount - 1) / pageResult.PageSize) + 1;
            if (maxPageNums < 2)
                return html.ToString();

            // urlLink += urlLink.LastIndexOf("?") > 0 ? "&" : "?";
 
            int prevBound = pageResult.PageIndex - (9 / 2);
            int nextBound = pageResult.PageIndex + (9 / 2);

            if (prevBound <= 0)
            {
                prevBound = 1;
                nextBound = 9;
            }

            if (nextBound > maxPageNums)
            {
                nextBound = maxPageNums;
                prevBound = maxPageNums - 8;
            }

            if (prevBound <= 0)
                prevBound = 1;

            int prevPage = pageResult.PageIndex == 1 ? pageResult.PageIndex : pageResult.PageIndex - 1;
            int nextPage = pageResult.PageIndex < maxPageNums ? pageResult.PageIndex + 1 : pageResult.PageIndex;



            //上一页
            if (prevPage > 0 && pageResult.PageIndex > 1)
            { 
                html.AppendFormat("<a href=\"{0}\" >上一页</a>", urlLink + "/" + prevPage + urlPrefix); 
            }

            if (maxPageNums == 1)
            {
                html.Append("<a class=\"current\">1</a>");
            }
            else
            {
                for (int i = prevBound; i <= nextBound; i++)
                {
                    if (pageResult.PageIndex == i)
                    {
                        html.AppendFormat("<a class=\"current\">{0}</a>", i);
                    }
                    else if (i <= maxPageNums)
                    {
                        html.AppendFormat("<a href=\"{0}\" >{1}</a>", urlLink + "/" + i + urlPrefix, i);
                    }
                }
            }

            //下一页
            if (pageResult.PageIndex < maxPageNums)
            {
                html.AppendFormat("<a href=\"{0}\" >下一页</a>", urlLink + "/" + nextPage + urlPrefix);
            }
            return html.ToString();
        }

        public static string Pagination<T>(this HtmlHelper helper, PageResult<T> pageResult, string urlLink)
        {
            StringBuilder html = new StringBuilder();

            if (string.IsNullOrEmpty(urlLink))
                return html.ToString();
            //总页数
            int maxPageNums = (pageResult.TotalCount == 0 || pageResult.PageSize == 0) ? 0 : ((pageResult.TotalCount - 1) / pageResult.PageSize) + 1;
            if (maxPageNums < 2)
                return html.ToString();

            urlLink += urlLink.LastIndexOf("?") > 0 ? "&" : "?";

            int prevBound = pageResult.PageIndex - (9 / 2);
            int nextBound = pageResult.PageIndex + (9 / 2);

            if (prevBound <= 0)
            {
                prevBound = 1;
                nextBound = 9;
            }

            if (nextBound > maxPageNums)
            {
                nextBound = maxPageNums;
                prevBound = maxPageNums - 8;
            }

            if (prevBound <= 0)
                prevBound = 1;

            int prevPage = pageResult.PageIndex == 1 ? pageResult.PageIndex : pageResult.PageIndex - 1;
            int nextPage = pageResult.PageIndex < maxPageNums ? pageResult.PageIndex + 1 : pageResult.PageIndex;



            //上一页
            if (prevPage > 0 && pageResult.PageIndex > 1)
            {
                html.AppendFormat("<a href=\"{0}\" >上一页</a>", urlLink + "page=" + prevPage );
            }

            if (maxPageNums == 1)
            {
                html.Append("<a class=\"current\">1</a>");
            }
            else
            {
                for (int i = prevBound; i <= nextBound; i++)
                {
                    if (pageResult.PageIndex == i)
                    {
                        html.AppendFormat("<a class=\"current\">{0}</a>", i);
                    }
                    else if (i <= maxPageNums)
                    {
                        html.AppendFormat("<a href=\"{0}\" >{1}</a>", urlLink + "page=" + i, i);
                    }
                }
            }

            //下一页
            if (pageResult.PageIndex < maxPageNums)
            {
                html.AppendFormat("<a href=\"{0}\" >下一页</a>", urlLink + "page=" + nextPage);
            }
            return html.ToString();
        }
    }
}
