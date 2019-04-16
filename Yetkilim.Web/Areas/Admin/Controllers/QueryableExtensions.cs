using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yetkilim.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Some useful extension methods for <see cref="T:System.Linq.IQueryable`1" />.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return query.Skip(skipCount).Take(maxResultCount);
        }

        ///// <summary>
        ///// Used for paging with an <see cref="T:Abp.Application.Services.Dto.IPagedResultRequest" /> object.
        ///// </summary>
        ///// <param name="query">Queryable to apply paging</param>
        ///// <param name="pagedResultRequest">An object implements <see cref="T:Abp.Application.Services.Dto.IPagedResultRequest" /> interface</param>
        //public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        //{
        //    return PageBy(query, pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        //}

        /// <summary>
        /// Filters a <see cref="T:System.Linq.IQueryable`1" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition)
            {
                return query;
            }
            return query.Where(predicate);
        }

        /// <summary>
        /// Filters a <see cref="T:System.Linq.IQueryable`1" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            if (!condition)
            {
                return query;
            }
            return query.Where(predicate);
        }
    }
}
