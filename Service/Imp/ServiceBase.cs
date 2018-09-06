using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain;
using entityframework;

using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace Service.Imp
{


    public class ServiceBase<T> : IServiceBase<T> where T : Entity
    {
        /// <summary>
        /// Defines the lazyConnection
        /// </summary>
        private readonly Lazy<UcDbContext> lazyConnection = new Lazy<UcDbContext>();

        /// <summary>
        /// The AddOrUpdate
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        public Task<T> AddOrUpdate(T entity)
        {
            return entity.UniqueId == null || string.IsNullOrEmpty(entity.UniqueId.ToString())
                ? Add(entity)
                : Update(entity);
        }

        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        protected virtual async Task<T> Add(T entity)
        {
            entity.UniqueId = Guid.NewGuid().ToString();
            ;
            Db.Entry(entity).State = EntityState.Added;
            var result = await Db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// The Update
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        protected virtual async Task<T> Update(T entity)
        {
            var entry = Db.Entry(entity);
            entry.State = EntityState.Modified;
            await Db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// The DeleteById
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        public async Task<int> DeleteById(string id)
        {
            return await DeleteMore(x => x.UniqueId == id);
        }

        /// <summary>
        /// The DeleteByObject
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        public async Task<int> DeleteByObject(T entity)
        {
            return await DeleteMore(x => x.UniqueId == entity.UniqueId);
        }

        /// <summary>
        /// The DeleteMore
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        public async Task<int> DeleteMore(Expression<Func<T, bool>> spec)
        {
            var result = await Db.Set<T>().Where(spec).ToListAsync();
            if (result != null)
            {
                Db.Set<T>().RemoveRange(result);
                var rowChanges = await Db.SaveChangesAsync();
                return rowChanges;
            }

            return 0;
        }

        /// <summary>
        /// The FindMore
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{List{T}}"/></returns>
        public async Task<List<T>> FindMore(Expression<Func<T, bool>> spec)
        {
            return (await Queryable.Where(spec).ToListAsync());
        }

        /// <summary>
        /// The FindOne
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        public async Task<T> FindOne(Expression<Func<T, bool>> spec)
        {
            return await Queryable.FirstOrDefaultAsync(spec);
        }

        /// <summary>
        /// The FindOne
        /// </summary>
        /// <param name="id">The id<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        public async Task<T> FindOne(string id)
        {
            return await FindOne(x => x.UniqueId == id);
        }

        /// <summary>
        /// The FindPaging
        /// </summary>
        /// <param name="spec">The spec<see cref="Expression{Func{T, bool}}"/></param>
        /// <param name="pageNumber">The pageNumber<see cref="int?"/></param>
        /// <param name="pageSize">The pageSize<see cref="int?"/></param>
        /// <returns>The <see cref="Task{PagingResult{T}}"/></returns>
        //public async Task<PagingResult<T>> FindPaging(Expression<Func<T, bool>> spec, int? pageNumber, int? pageSize)
        //{
        //    var index = !pageNumber.HasValue || pageNumber <= 0 ? 0 : pageNumber.Value - 1;
        //    var size = !pageSize.HasValue ? int.MaxValue : (pageSize <= 0 ? 15 : pageSize.Value);

        //    var q = Queryable.Where(spec);

        //    return new PagingResult<T>
        //    {
        //        Data = await q.Skip(index * size).Take(size).ToListAsync(),
        //        TotalCount = q.Count(),
        //        PageNumber = index,
        //        PageSize = size
        //    };
        //}

        /// <summary>
        /// The BlukInsert
        /// </summary>
        /// <param name="entities">The entities<see cref="List{T}"/></param>
        /// <returns>The <see cref="Task{int}"/></returns>
        public async Task<int> BlukInsert(List<T> entities)
        {
            var rowChanges = 0;
            if (entities != null)
            {
                Db.Set<T>().AddRange(entities);
                rowChanges = await Db.SaveChangesAsync();
            }


            return rowChanges;
        }

        /// <summary>
        /// The BlukUpdate
        /// </summary>
        /// <param name="entities">The entities<see cref="List{T}"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> BlukUpdate(List<T> entities)
        {
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    var entry = Db.Entry(item);
                    entry.State = EntityState.Modified;
                }

                await Db.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Gets the Db
        /// </summary>
        protected UcDbContext Db
        {
            get { return lazyConnection.Value; }
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        void IDisposable.Dispose()
        {
            //服务关闭时、关闭db connection，这样可避免保持过多的数据连接
            if (lazyConnection != null && lazyConnection.IsValueCreated)
            {
                lazyConnection.Value.Dispose();
            }
        }


        protected IQueryable<T> Queryable
        {
            get
            {


                IQueryable<T> q = Db.Set<T>().AsNoTracking();
                //if (typeof(IDomain).IsAssignableFrom(typeof(T)) && ServiceContext != null && !string.IsNullOrEmpty(ServiceContext.Domain) && !string.IsNullOrEmpty(ServiceContext.SiteId))
                //{
                //    q = q.Where(BuildExpression<T>());
                //}

                return q;
            }
        }
    }
}

