using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vecto.Core.Entities;

namespace Vecto.Application.Helpers
{
    public static class ReflectionHelper
    {
        public static List<TypeInfo> GetSubtypesInSameAssembly(this Type type)
        {
            return type.Assembly.DefinedTypes.Where(t=>t.IsSubclassOf(type)).ToList();
        }
        
        public static List<string> GetSubtypeNamesInSameAssembly(this Type type)
        {
            return GetSubtypesInSameAssembly(type).Select(s => s.Name).ToList();
        }
    }
}