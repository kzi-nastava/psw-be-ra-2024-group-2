using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Infrastructure.Database;

public static class ReflectionHelper
{
    public static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive || type.IsValueType || type == typeof(string);
    }
}