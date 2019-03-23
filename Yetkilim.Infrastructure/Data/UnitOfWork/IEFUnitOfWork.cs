// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.UnitOfWork.IEFUnitOfWork
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.UnitOfWork;
using Yetkilim.Infrastructure.Data.Repository;

namespace Yetkilim.Infrastructure.Data.UnitOfWork
{
  public interface IEFUnitOfWork : IUnitOfWork, IDisposable
  {
    EFRepository<TEntity> EntityRepository<TEntity>() where TEntity : class, IEntity;

    int ExecuteSqlCommand(string sql, params object[] parameters);

    Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

    int SaveChanges();

    Task<int> SaveChangesAsync();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}
