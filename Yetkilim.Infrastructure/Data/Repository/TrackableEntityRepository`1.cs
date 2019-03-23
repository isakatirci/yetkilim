// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Repository.TrackableEntityRepository`1
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using System;
using Yetkilim.Domain.Entity;

namespace Yetkilim.Infrastructure.Data.Repository
{
  public class TrackableEntityRepository<TEntity> : EFRepository<TEntity> where TEntity : class, ITrackableEntity
  {
    protected TrackableEntityRepository(DbContext context)
      : base(context)
    {
    }

    public override TEntity Create(TEntity entity, string createdBy = null)
    {
      entity.CreatedDate = DateTime.UtcNow;
      entity.CreatedBy = createdBy;
      return this.DbSet.Add(entity).Entity;
    }

    public override TEntity Update(TEntity entity, string modifiedBy = null)
    {
      entity.ModifiedDate = new DateTime?(DateTime.UtcNow);
      entity.ModifiedBy = modifiedBy;
      this.DbSet.Attach(entity);
      this.Context.Entry<TEntity>(entity).State = EntityState.Modified;
      return entity;
    }
  }
}
