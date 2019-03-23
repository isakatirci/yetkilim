// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.ServiceBase
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Yetkilim.Global.Caching;
using Yetkilim.Global.Context;
using Yetkilim.Global.Logging;

namespace Yetkilim.Business
{
  public class ServiceBase
  {
    protected IGlobalContext GlobalContext;

    protected ServiceBase(IGlobalContext globalContext)
    {
      this.GlobalContext = globalContext;
    }

    protected ILogger Logger
    {
      get
      {
        return LogManager.GetLogger(this.GetType());
      }
    }

    protected IMemoryCache Cache
    {
      get
      {
        return CacheManager.Cache;
      }
    }
  }
}
