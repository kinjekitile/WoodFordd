using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.ApplicationServices.Utilities {
    public static class Helpers {
        public static string GetEnumDescription(this Enum value) {
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
    }
}
