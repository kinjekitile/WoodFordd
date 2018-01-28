using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Woodford.UI.Web.Admin.Code.Helpers {
    public static class UrlHelpers {
        public static string ToQueryString(this object request, string separator = ",") {
            if (request == null)
                throw new ArgumentNullException("request");

            var properties = request.GetType().GetProperties()
               .Where(x => x.CanRead)
               .Where(x => x.GetValue(request, null) != null)
               .ToList();


            //// Get all properties on the object
            //var properties = request.GetType().GetProperties()
            //    .Where(x => x.CanRead)
            //    .Where(x => x.GetValue(request, null) != null)
            //    .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            Dictionary<string, object> items = new Dictionary<string, object>();


            foreach (var p in properties) {
                if (p.PropertyType.IsEnum) {
                    items.Add("Filter." + p.Name, (int)p.GetValue(request));
                } else {
                    items.Add("Filter." + p.Name, p.GetValue(request));
                }
            }
            // Get names for all IEnumerable properties (excl. string)
            //var propertyNames = properties
            //    .Where(x => !(x.Value is string) && x.Value is IEnumerable)
            //    .Select(x => x.Key)
            //    .ToList();

            //// Concat all IEnumerable properties into a comma separated string
            //foreach (var key in propertyNames) {
            //    var valueType = properties[key].GetType();
            //    var valueElemType = valueType.IsGenericType
            //                            ? valueType.GetGenericArguments()[0]
            //                            : valueType.GetElementType();
            //    if (valueElemType.IsPrimitive || valueElemType == typeof(string)) {
            //        var enumerable = properties[key] as IEnumerable;
            //        properties[key] = string.Join(separator, enumerable.Cast<object>());
            //    }
            //}

            // Concat all key/value pairs into a string separated by ampersand
            return string.Join("&", items
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }
    }
}