using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Model
{
    public static class PageExtension
    {
        public static IQueryable<T> GetPageList<T>(this IQueryable<T> query, PageParameter parameters)
        {
            parameters.RecordCount = query.Count();
            if (!String.IsNullOrEmpty(parameters.OrderBy))
            {
                //支持多字段排序
                string[] orderBys = parameters.OrderBy.Split(',');
                for (int i = 0; i < orderBys.Length; i++)
                {
                    string orderBy = orderBys[i];
                    bool isAsc = orderBy.StartsWith("-") ? false : (orderBy.StartsWith("+") ? true : parameters.IsAsc);
                    orderBy = orderBy.TrimStart('-').TrimStart('+');

                    query = query.DataSorting(orderBy, isAsc, i == 0 ? true : false);
                }
            }
            if (parameters.PageSize < 1)
                return query;

            if (parameters.PageIndex > 1)
                query = query.Skip((parameters.PageIndex - 1) * parameters.PageSize).Take(parameters.PageSize);
            return query.Take(parameters.PageSize);
        }

        public static IQueryable<T> DataSorting<T>(this IQueryable<T> source, string orderBy, bool IsASC, bool isFirst)
        {
            var sortingDir = (isFirst ? "Order" : "Then") + (IsASC ? "By" : "ByDescending");

            ParameterExpression param = Expression.Parameter(typeof(T), orderBy);

            PropertyInfo pi = typeof(T).GetProperty(orderBy);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            var lambda = Expression.Lambda(Expression.Property(param, orderBy), param);

            var expr = Expression.Call(typeof(Queryable), sortingDir, types, source.AsQueryable().Expression, Expression.Quote(lambda));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }

    }
}
