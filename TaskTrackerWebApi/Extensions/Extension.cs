using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskTrackerWebApi.Controllers;

namespace TaskTrackerWebApi.Extensions
{
    public static class Extension
    {
        public enum OrderTypes { Ascending, Descending }
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, OrderTypes orderType)
        {
            if (orderType == OrderTypes.Ascending)
                return source.OrderBy(keySelector);
            else
                return source.OrderByDescending(keySelector);
        }
    }
}
