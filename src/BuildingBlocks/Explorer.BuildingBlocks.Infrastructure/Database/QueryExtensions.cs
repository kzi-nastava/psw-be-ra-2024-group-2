using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Infrastructure.Database;

public static class QueryExtensions
{
    public static IQueryable<T> IncludeRelatedEntities<T>(this IQueryable<T> query) where T : class
    {
        var entityType = typeof(T);

        var properties = entityType.GetProperties();

        foreach (var property in properties)
        {
            if (!ReflectionHelper.IsPrimitive(property.PropertyType))
            {
                query = query.Include(property.Name).AsSplitQuery();
            }
        }

        return query;
    }
}