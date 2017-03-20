using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.Web;

namespace Zit.Web.Libs
{
    public class ZitModelBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime) ||
                    propertyDescriptor.PropertyType == typeof(DateTime?))
            {
                ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                DateTime outDate;
                try
                {
                    if (DateTime.TryParse(valueResult.AttemptedValue, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out outDate))
                    {
                        return outDate;
                    }
                }
                catch { }
            }
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        protected override bool OnPropertyValidating(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (value is string && (controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)))
            {
                if (controllerContext.Controller.ValidateRequest && bindingContext.PropertyMetadata[propertyDescriptor.Name].RequestValidationEnabled)
                {
                    int index;
                    if (CrossSiteScriptingValidation.IsDangerousString(value.ToString(), out index))
                    {
                        throw new HttpRequestValidationException("Dangerous Input Detected");
                    }
                }
            }
            return base.OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}
