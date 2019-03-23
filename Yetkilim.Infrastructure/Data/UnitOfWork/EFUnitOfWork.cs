// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.UnitOfWork.EFUnitOfWork
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.UnitOfWork;
using Yetkilim.Infrastructure.Data.Repository;

namespace Yetkilim.Infrastructure.Data.UnitOfWork
{
  public abstract class EFUnitOfWork : IEFUnitOfWork, IUnitOfWork, IDisposable
  {
    protected DbContext Context;
    protected IDbContextTransaction Transaction;
    private bool _disposed;

    protected EFUnitOfWork(DbContext context)
    {
      DbContext dbContext = context;
      if (dbContext == null)
        throw new ArgumentNullException(nameof (context));
      this.Context = dbContext;
      this.Context = context;
    }

    public EFRepository<TEntity> EntityRepository<TEntity>() where TEntity : class, IEntity
    {
      return new EFRepository<TEntity>(this.Context);
    }

    public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
    {
      return this.Context.Database.ExecuteSqlCommand((RawSqlString) sql, parameters);
    }

    public virtual async Task<int> ExecuteSqlCommandAsync(
      string sql,
      params object[] parameters)
    {
      int num = await this.Context.Database.ExecuteSqlCommandAsync((RawSqlString) sql, parameters);
      return num;
    }

    public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
      this.Transaction = this.Context.Database.BeginTransaction(isolationLevel);
    }

    public virtual bool Commit()
    {
      this.Transaction.Commit();
      return true;
    }

    public virtual void Rollback()
    {
      this.Transaction.Rollback();
    }

    public virtual int SaveChanges()
    {
      return this.Context.SaveChanges();
    }

    public virtual Task<int> SaveChangesAsync()
    {
      return this.Context.SaveChangesAsync(new CancellationToken());
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      return this.Context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this._disposed && disposing)
      {
        this.Context?.Dispose();
        this.Context = (DbContext) null;
        this.Transaction?.Dispose();
        this.Transaction = (IDbContextTransaction) null;
      }
      this._disposed = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
