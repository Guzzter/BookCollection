using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookCollection.Helpers
{
    public abstract class CustomWebViewPage<T> : WebViewPage<T>
    {
        public new CustomHtmlHelper<T> Html { get; set; }

        public override void InitHelpers()
        {
            Ajax = new AjaxHelper<T>(ViewContext, this);
            Url = new UrlHelper(ViewContext.RequestContext);

            //Load Custom Html Helper instead of Default
            Html = new CustomHtmlHelper<T>(ViewContext, this);
        }
    }
}