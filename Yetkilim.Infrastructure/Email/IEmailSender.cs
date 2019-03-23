// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Email.IEmailSender
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using Yetkilim.Global.Model;

namespace Yetkilim.Infrastructure.Email
{
  public interface IEmailSender
  {
    Task<Result> Send(IEnumerable<string> toAddresses, string subject, string body);
  }
}
