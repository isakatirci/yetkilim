// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Repository.Base.IReadOnlyRepository`1
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yetkilim.Domain.Entity;

namespace Yetkilim.Domain.Repository.Base
{
  public interface IReadOnlyRepository<TEntity> where TEntity : class, IEntity
  {
    IEnumerable<TEntity> GetAll(
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    Task<IEnumerable<TEntity>> GetAllAsync(
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    IEnumerable<TEntity> Get(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    Task<IEnumerable<TEntity>> GetAsync(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    TEntity GetOne(
      Expression<Func<TEntity, bool>> filter = null,
      params Expression<Func<TEntity, object>>[] include);

    Task<TEntity> GetOneAsync(
      Expression<Func<TEntity, bool>> filter = null,
      params Expression<Func<TEntity, object>>[] include);

    TEntity GetFirst(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    Task<TEntity> GetFirstAsync(
      Expression<Func<TEntity, bool>> filter = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
      params Expression<Func<TEntity, object>>[] include);

    TEntity GetById(object id);

    Task<TEntity> GetByIdAsync(object id);

    int GetCount(Expression<Func<TEntity, bool>> filter = null);

    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

    bool GetExists(Expression<Func<TEntity, bool>> filter = null);

    Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);
  }
}
