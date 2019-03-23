// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Logging.LogManager
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using Microsoft.Extensions.Logging;
using System;

namespace Yetkilim.Global.Logging
{
  public class LogManager
  {
    public static ILoggerFactory LoggerFactory { get; set; } = (ILoggerFactory) null;

    public static ILogger<T> GetLogger<T>()
    {
      return LogManager.LoggerFactory.CreateLogger<T>();
    }

    public static ILogger GetLogger(Type type)
    {
      return LogManager.LoggerFactory.CreateLogger(type);
    }
  }
}
