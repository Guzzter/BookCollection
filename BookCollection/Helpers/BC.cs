using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BookCollection.Helpers
{
    public static class BC
    {
        public static MvcHtmlString BoolIcon(bool target, string toolTipTrue = "", string toolTipFalse = "")
        {
            string spanTemplate = "<span class=\"glyphicon glyphicon-{0}\" aria-hidden=\"true\" {1}></span>";
            string toolTipTemplate = " data-toggle=\"tooltip\" data-placement=\"top\" title=\"{0}\"";
            string tooltip = "";

            if (target)
            {
                tooltip = string.IsNullOrEmpty(toolTipTrue) ? "" : string.Format(toolTipTemplate, toolTipTrue);
                return new MvcHtmlString(String.Format(spanTemplate, "ok", tooltip));
            }

            tooltip = string.IsNullOrEmpty(toolTipFalse) ? "" : string.Format(toolTipFalse, toolTipTrue);
            return new MvcHtmlString(String.Format(spanTemplate, "remove", tooltip));
        }

        public static MvcHtmlString IconButton(string action, string iconname, string linktext, int ID = -1)
        {
            object routeVal = (ID == -1 ? null : new { id = ID });
            return  IconButton("", action, iconname, linktext, routeVal);
        }

        public static MvcHtmlString IconButton(string controller, string action, string iconname, string linktext, int ID = -1)
        {
            object routeVal = (ID == -1 ? null : new { id = ID });
            return IconButton(controller, action, iconname, linktext, routeVal);
        }

        public static MvcHtmlString IconButton(string controller, string action, string iconname, string linktext, object routeValues)
        {
            //<a href="@Url.Action("Edit", new { id = item.BookID })" class="btn btn-primary btn-xs">
            //<span class="glyphicon glyphicon-pencil" aria-hidden="true"></span> Edit</a>

            string spanTemplate = "<span class=\"glyphicon glyphicon-{0}\" aria-hidden=\"true\"></span>";
            string iconHtml = string.IsNullOrEmpty(iconname) ? "" : string.Format(spanTemplate, iconname);

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string url = urlHelper.Action(action, controller, routeValues);

            string linkTemplate = "<a href=\"{0}\" class=\"btn btn-primary btn-xs\">{1} {2}</a>";

            return new MvcHtmlString(String.Format(linkTemplate, url, iconHtml, linktext));
        }

        public static MvcHtmlString MaterialBadge(Models.Material ma)
        {
            
            string templ = "<span class =\"badge\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"{0}\">{1}</span>";
            string html = "";
            switch (ma)
            {
                case Models.Material.HardCover:
                    // Use the text block below to separate html elements from code
                    html = string.Format(templ, "Hard cover", "HAC");
                    break;  // Always break each case
                case Models.Material.SoftCover:
                    html = string.Format(templ, "Soft cover", "SOC");
                    break;
                case Models.Material.Deluxe:
                    html = string.Format(templ, "Deluxe edition", "LUX");
                    break;
                case Models.Material.Summary:
                    html = string.Format(templ, "Summary", "SUM");
                    break;
                case Models.Material.Pocket:
                    html = string.Format(templ, "Pocket", "POC");
                    break;
                case Models.Material.SeparateBox:
                    html = string.Format(templ, "Has separate box", "SEP");
                    break;
                default:
                    html = string.Format(templ, "", "?");
                    break;
            }
            return MvcHtmlString.Create(html);
        }

        /*
        public static MvcHtmlString StateDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            Dictionary<string, string> stateList = new Dictionary<string, string>()
        {
            {"AL"," Alabama"},
            {"AK"," Alaska"},
            {"AZ"," Arizona"},
            {"AR"," Arkansas"},
            {"CA"," California"},
            {"CO"," Colorado"},
            {"CT"," Connecticut"},
            {"DE"," Delaware"},
            {"FL"," Florida"},
            {"GA"," Georgia"},
            {"HI"," Hawaii"},
            {"ID"," Idaho"},
            {"IL"," Illinois"},
            {"IN"," Indiana"},
            {"IA"," Iowa"},
            {"KS"," Kansas"},
            {"KY"," Kentucky"},
            {"LA"," Louisiana"},
            {"ME"," Maine"},
            {"MD"," Maryland"},
            {"MA"," Massachusetts"},
            {"MI"," Michigan"},
            {"MN"," Minnesota"},
            {"MS"," Mississippi"},
            {"MO"," Missouri"},
            {"MT"," Montana"},
            {"NE"," Nebraska"},
            {"NV"," Nevada"},
            {"NH"," New Hampshire"},
            {"NJ"," New Jersey"},
            {"NM"," New Mexico"},
            {"NY"," New York"},
            {"NC"," North Carolina"},
            {"ND"," North Dakota"},
            {"OH"," Ohio"},
            {"OK"," Oklahoma"},
            {"OR"," Oregon"},
            {"PA"," Pennsylvania"},
            {"RI"," Rhode Island"},
            {"SC"," South Carolina"},
            {"SD"," South Dakota"},
            {"TN"," Tennessee"},
            {"TX"," Texas"},
            {"UT"," Utah"},
            {"VT"," Vermont"},
            {"VA"," Virginia"},
            {"WA"," Washington"},
            {"WV"," West Virginia"},
            {"WI"," Wisconsin"},
            {"WY"," Wyoming"},
            {"AS"," American Samoa"},
            {"DC"," District of Columbia"},
            {"FM"," Federated States of Micronesia"},
            {"MH"," Marshall Islands"},
            {"MP"," Northern Mariana Islands"},
            {"PW"," Palau"},
            {"PR"," Puerto Rico"},
            {"VI"," Virgin Islands"},
            {"GU"," Guam"}
        };
            return HtmlHelper.DropDownListFor(expression, new SelectList(stateList, "key", "value"));
        }*/
    }
}