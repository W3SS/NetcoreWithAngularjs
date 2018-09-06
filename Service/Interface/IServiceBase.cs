using Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IServiceBase<T> : IServiceBase where T : Entity
    {
        /// <summary>
        /// The AddOrUpdate
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> AddOrUpdate(T entity);

        /// <summary>
        /// The DeleteById
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        Task<int> DeleteById(string id);

        /// <summary>
        /// The DeleteByObject
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        Task<int> DeleteByObject(T entity);

        /// <summary>
        /// The DeleteMore
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        Task<int> DeleteMore(Expression<Func<T, bool>> spec);

        /// <summary>
        /// The BlukInsert
        /// </summary>
        /// <param name="entities">The entities<see cref="List{T}"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        Task<int> BlukInsert(List<T> entities);

        /// <summary>
        /// The FindOne
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> FindOne(string id);

        /// <summary>
        /// The FindOne
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> FindOne(Expression<Func<T, bool>> spec);

        /// <summary>
        /// The FindMore
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{List{T}}"/></returns>
        Task<List<T>> FindMore(Expression<Func<T, bool>> spec);

        /// <summary>
        /// The FindPaging
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <param name="pageNumber">The pageNumber<see cref="int?"/></param>
        /// <param name="pageSize">The pageSize<see cref="int?"/></param>
        /// <returns>The <see cref="Task{PagingResult{T}}"/></returns>
     //   Task<PagingResult<T>> FindPaging(Expression<Func<T, bool>> spec, int? pageNumber, int? pageSize);

        /// <summary>
        /// The BlukUpdate
        /// </summary>
        /// <param name="entities">The entities<see cref="List{T}"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> BlukUpdate(List<T> entities);
    }

    /// <summary>
    /// Defines the <see cref="IServiceBase" />
    /// </summary>
    public interface IServiceBase : IDisposable
    {
    }
}
