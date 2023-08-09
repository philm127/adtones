using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EFMVC.Web.Extension
{
    public static class CheckboxEx
    {
        public static MvcHtmlString CheckBoxListForTest<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TProperty>>> expression, MultiSelectList allOptions, object htmlAttributes = null)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression<TModel, IEnumerable<TProperty>>(expression, htmlHelper.ViewData);

            // Derive property name for checkbox name
            string propertyName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(modelMetadata.PropertyName);

            // Get currently select values from the ViewData model
            IEnumerable<TProperty> list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            // Convert selected value list to a List<string> for easy manipulation
            IList<string> selectedValues = new List<string>();

            if (list != null)
            {
                selectedValues = new List<TProperty>(list).ConvertAll<string>(delegate (TProperty i) { return i.ToString(); });
            }

            // Create div
            TagBuilder divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            // Add checkboxes
            foreach (SelectListItem item in allOptions)
            {
                divTag.InnerHtml += string.Format(
                                                  "<div><input type=\"checkbox\" class=\"i-checks\"  name=\"{0}\" id=\"{1}_{2}\" " +
                                                  "value=\"{2}\" {3} /><label for=\"{1}_{2}\">{4}</label></div>",
                                                  propertyName,
                                                  TagBuilder.CreateSanitizedId(propertyName),
                                                  item.Value,
                                                  selectedValues.Contains(item.Value) ? "checked=\"checked\"" : string.Empty,
                                                  item.Text);
            }

            return MvcHtmlString.Create(divTag.ToString());
        }
    }
}