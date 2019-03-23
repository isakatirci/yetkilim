// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.UnitOfWork.YetkilimUnitOfWork
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using System;
using Yetkilim.Domain.UnitOfWork;

namespace Yetkilim.Infrastructure.Data.UnitOfWork
{
  public class YetkilimUnitOfWork : EFUnitOfWork, IYetkilimUnitOfWork, IEFUnitOfWork, IUnitOfWork, IDisposable
  {
    public YetkilimUnitOfWork(DbContext context)
      : base(context)
    {
    }
  }
}
