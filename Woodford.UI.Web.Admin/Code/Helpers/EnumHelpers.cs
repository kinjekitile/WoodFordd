using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.UI.Web.Admin.Code.Helpers {
    public static class EnumHelpers {
        public static string GetDescription(this Enum value) {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null) {
                FieldInfo field = type.GetField(name);
                if (field != null) {
                    EnumFriendlyNameAttribute attr =
                                 Attribute.GetCustomAttribute(field,
                                     typeof(EnumFriendlyNameAttribute)) as EnumFriendlyNameAttribute;
                    if (attr != null) {
                        return attr.FriendlyName;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}