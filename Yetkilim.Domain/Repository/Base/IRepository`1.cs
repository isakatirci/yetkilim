// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Repository.Base.IRepository`1
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using Yetkilim.Domain.Entity;

namespace Yetkilim.Domain.Repository.Base
{
  public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IEntity
  {
    TEntity Create(TEntity entity, string createdBy = null);

    TEntity Update(TEntity entity, string modifiedBy = null);

    TEntity Delete(object id);

    TEntity Delete(TEntity entity);
  }
}
