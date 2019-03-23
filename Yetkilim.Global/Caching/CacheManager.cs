// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Caching.CacheManager
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using Microsoft.Extensions.Caching.Memory;

namespace Yetkilim.Global.Caching
{
  public class CacheManager
  {
    public static IMemoryCache Cache;

    public CacheManager(IMemoryCache cache)
    {
      CacheManager.Cache = cache;
    }
  }
}
