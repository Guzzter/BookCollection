using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookCollection.Helpers
{
    //http://stackoverflow.com/questions/15952240/how-do-you-override-html-actionlink

    public class CustomHtmlHelper<T> : HtmlHelper<T>
    {
        public CustomHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) :
            base(viewContext, viewDataContainer)
        {
        }

        //Instance methods will always be called instead of extension methods when both exist with the same signature...

        public MvcHtmlString ActionLink(string linkText, string actionName)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, object routeValues)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName)
        {
            return ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return ActionLink(linkText, actionName, null, routeValues, new RouteValueDictionary());
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return ActionLink(linkText, actionName, null, routeValues, htmlAttributes);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string sortIconHtml = "";
            routeValues = SetSortParam(routeValues, out sortIconHtml);
            string link = GenerateLink(ViewContext.RequestContext, RouteCollection, linkText, null, actionName, controllerName, routeValues, htmlAttributes);

            return MvcHtmlString.Create(sortIconHtml + link);
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, new RouteValueDictionary(routeValues), AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string sortIconHtml = "";
            routeValues = SetSortParam(routeValues, out sortIconHtml);
            string link = GenerateLink(ViewContext.RequestContext, RouteCollection, linkText, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);

            return MvcHtmlString.Create(sortIconHtml + link);
        }

        private const string spanTemplate = "<span class=\"glyphicon glyphicon-{0}\" aria-hidden=\"true\"></span>";
        public RouteValueDictionary SetSortParam(RouteValueDictionary rvd, out string sortPrefix)
        {
            sortPrefix = "";
            // Is sortable link?
            if (rvd != null && rvd.Keys.Contains("sortKey") && !string.IsNullOrWhiteSpace(rvd["sortKey"] as string) )
            {
                // prevent double keys
                if (rvd.ContainsKey("sortOrder"))
                {
                    rvd.Remove("sortOrder");
                }

                string sortKey = rvd["sortKey"] as string;
                // Has user specified a sort?
                if (rvd.Keys.Contains("currentSort") && !string.IsNullOrWhiteSpace(rvd["currentSort"] as string))
                {
                    string[] currentSort = (rvd["currentSort"] as string).Split(new[] { '_' });
                    if (currentSort.Length == 2)
                    {
                        // Is specified current sort matching the sort key for this link?
                        if (sortKey.Equals(currentSort[0], StringComparison.InvariantCultureIgnoreCase))
                        {
                            
                            if (currentSort[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // set new sort direction and display icon ascending
                                sortPrefix = string.Format(spanTemplate, "sort-by-attributes-alt");
                                rvd.Add("sortOrder", sortKey + "_asc");
                            }
                            else
                            {
                                // set new sort direction and display icon descending
                                sortPrefix = string.Format(spanTemplate, "sort-by-attributes");
                                rvd.Add("sortOrder", sortKey + "_desc");
                            }
                        }
                        else
                        {
                            rvd.Add("sortOrder", sortKey + "_asc");
                        }
                    }
                }
                else
                {
                    // No used sort, set default first sort for this sort key
                    rvd.Add("sortOrder", sortKey + "_asc");
                }
            }
            return rvd;
        }
    }
}