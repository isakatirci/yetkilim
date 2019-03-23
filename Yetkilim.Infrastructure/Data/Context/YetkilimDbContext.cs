// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Context.YetkilimDbContext
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Yetkilim.Domain.Entity;

namespace Yetkilim.Infrastructure.Data.Context
{
  public class YetkilimDbContext : DbContext
  {
    private static LoggerFactory _myLoggerFactory;

    public DbSet<Company> Companies { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<FeedbackForm> FeedbackForms { get; set; }

    public DbSet<Place> Places { get; set; }

    public DbSet<Promotion> Promotions { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<CompanyType> CompanyTypes { get; set; }

    public YetkilimDbContext(DbContextOptions<YetkilimDbContext> options)
      : base((DbContextOptions) options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      YetkilimDbContext.BindLogging(optionsBuilder);
    }

    public static Action<SqlServerDbContextOptionsBuilder> SqlOptions
    {
      get
      {
        return (Action<SqlServerDbContextOptionsBuilder>) (builder => SqlServerNetTopologySuiteDbContextOptionsBuilderExtensions.UseNetTopologySuite(builder));
      }
    }

    public static LoggerFactory MyLoggerFactory
    {
      get
      {
        LoggerFactory loggerFactory = YetkilimDbContext._myLoggerFactory;
        if (loggerFactory != null)
          return loggerFactory;
        DebugLoggerProvider[] debugLoggerProviderArray = new DebugLoggerProvider[1]
        {
          new DebugLoggerProvider()
        };
        return YetkilimDbContext._myLoggerFactory = new LoggerFactory((IEnumerable<ILoggerProvider>) debugLoggerProviderArray);
      }
    }

    [Conditional("DEBUG")]
    private static void BindLogging(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLoggerFactory((ILoggerFactory) YetkilimDbContext.MyLoggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Seed();
      foreach (IMutableForeignKey mutableForeignKey in modelBuilder.Model.GetEntityTypes().SelectMany<IMutableEntityType, IMutableForeignKey>((Func<IMutableEntityType, IEnumerable<IMutableForeignKey>>) (e => e.GetForeignKeys())))
        mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
    }
  }
}
