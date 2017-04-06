using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Zit.Web.Libs.UI
{
    public class HtmlComponentFactory
    {
        readonly HtmlHelper helper;
        public HtmlComponentFactory(HtmlHelper helper)
        {
            this.helper = helper;
        }

        public MvcHtmlString DatePicker(string id = null)
        {
            if (id == null) id = "id";
            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add(new KeyValuePair<string,string>("type","text"));
            tag.Attributes.Add(new KeyValuePair<string, string>("id", id));
            tag.Attributes.Add(new KeyValuePair<string, string>("onload", "(function(e){alert('test');})(event);"));
            //var script = string.Format(@"<script>$(function() {{$(""#{0}"").datepicker();}});</script>", id);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }
    }

    public class HtmlComponentFactory<TModel> : HtmlComponentFactory
    {
        readonly HtmlHelper<TModel> helper;

        public HtmlComponentFactory(HtmlHelper<TModel> helper)
            :base(helper)
        {
            this.helper = helper;
        }
    }

    public static class HtmlHelperExt
    {
        public static HtmlComponentFactory<TModel> UI<TModel>(this HtmlHelper<TModel> helper)
        {
            return new HtmlComponentFactory<TModel>(helper);
        }
        public static HtmlComponentFactory UI(this HtmlHelper helper)
        {
            return new HtmlComponentFactory(helper);
        }
    }
}
