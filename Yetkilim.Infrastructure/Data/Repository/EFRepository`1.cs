// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Repository.EFRepository`1
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Repository.Base;

namespace Yetkilim.Infrastructure.Data.Repository
{
    public class EFRepository<TEntity> : IRepository<TEntity>, IReadOnlyRepository<TEntity>
      where TEntity : class, IEntity
    {
        public EFRepository(DbContext context)
        {
            DbContext dbContext = context;
            if (dbContext == null)
                throw new ArgumentNullException(nameof(context));
            this.Context = dbContext;
            this.DbSet = context.Set<TEntity>();
        }

        protected DbContext Context { get; }

        protected Microsoft.EntityFrameworkCore.DbSet<TEntity> DbSet { get; }

        public virtual IQueryable<TEntity> GetQueryable(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            IQueryable<TEntity> source = this.DbSet.AsQueryable<TEntity>();
            if (filter != null)
                source = source.Where<TEntity>(filter);
            foreach (Expression<Func<TEntity, object>> navigationPropertyPath in include)
                source = (IQueryable<TEntity>)source.Include<TEntity, object>(navigationPropertyPath);
            return source;
        }

        public virtual IEnumerable<TEntity> GetAll(
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            return (IEnumerable<TEntity>)this.GetQueryable((Expression<Func<TEntity, bool>>)null, orderBy, include).ToList<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            List<TEntity> listAsync = await this.GetQueryable((Expression<Func<TEntity, bool>>)null, orderBy, include).ToListAsync<TEntity>(new CancellationToken());
            return (IEnumerable<TEntity>)listAsync;
        }

        public virtual IEnumerable<TEntity> Get(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            return (IEnumerable<TEntity>)this.GetQueryable(filter, orderBy, include).ToList<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            List<TEntity> listAsync = await this.GetQueryable(filter, orderBy, include).ToListAsync<TEntity>(new CancellationToken());
            return (IEnumerable<TEntity>)listAsync;
        }

        public virtual TEntity GetOne(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            return this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null, include).SingleOrDefault<TEntity>();
        }

        public virtual async Task<TEntity> GetOneAsync(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            TEntity entity = await this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null, include).SingleOrDefaultAsync<TEntity>(new CancellationToken());
            return entity;
        }

        public virtual TEntity GetFirst(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            return this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null, include).FirstOrDefault<TEntity>();
        }

        public virtual async Task<TEntity> GetFirstAsync(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] include)
        {
            TEntity entity = await this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null, include).FirstOrDefaultAsync<TEntity>(new CancellationToken());
            return entity;
        }

        public virtual TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            TEntity async = await this.DbSet.FindAsync(id);
            return async;
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null).Count<TEntity>();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            int num = await this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null).CountAsync<TEntity>(new CancellationToken());
            return num;
        }

        public virtual bool GetExists(Expression<Func<TEntity, bool>> filter = null)
        {
            return this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null).Any<TEntity>();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            bool flag = await this.GetQueryable(filter, (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)null).AnyAsync<TEntity>(new CancellationToken());
            return flag;
        }

        public virtual TEntity Create(TEntity entity, string createdBy = null)
        {
            this.DbSet.Add(entity);
            return entity;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, string createdBy = null)
        {
            EntityEntry<TEntity> res = await this.DbSet.AddAsync(entity, new CancellationToken());
            return res.Entity;
        }

        public virtual void CreateRange(IEnumerable<TEntity> entitites, string createdBy = null)
        {
            this.DbSet.AddRange(entitites);
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entitites, string createdBy = null)
        {
            await this.DbSet.AddRangeAsync(entitites, new CancellationToken());
        }

        public virtual TEntity Update(TEntity entity, string modifiedBy = null)
        {
            this.DbSet.Attach(entity);
            this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual TEntity Delete(object id)
        {
            return this.Delete(this.Context.Set<TEntity>().Find(id));
        }

        public virtual TEntity Delete(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.DbSet<TEntity> dbSet = this.Context.Set<TEntity>();
            if (this.Context.Entry<TEntity>(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
            return entity;
        }

        public virtual void DeleteRange(IEnumerable<object> ids)
        {
            Microsoft.EntityFrameworkCore.DbSet<TEntity> dbSet = this.Context.Set<TEntity>();
            foreach (object id in ids)
            {
                TEntity entity = dbSet.Find(id);
                dbSet.Remove(entity);
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            Microsoft.EntityFrameworkCore.DbSet<TEntity> dbSet = this.Context.Set<TEntity>();
            foreach (TEntity entity in entities)
            {
                if (this.Context.Entry<TEntity>(entity).State == EntityState.Detached)
                    dbSet.Attach(entity);
                dbSet.Remove(entity);
            }
        }
    }
}
