using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ABB.Application.Common.Helpers
{
    public static class DynamicHelper
    {
        public static bool PropertyExist(dynamic dynVar, string propertyName)
        {
            Type typeOfDynamic = dynVar.GetType();
            return typeOfDynamic.GetProperties().Any(p => p.Name.Equals(propertyName));
        }

        public static dynamic GetValue(dynamic dynVar, string propertyName)
        {
            return dynVar.GetType().GetProperty(propertyName).GetValue(dynVar, null);
        }

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }
}