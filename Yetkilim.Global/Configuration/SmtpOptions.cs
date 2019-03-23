// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Configuration.SmtpOptions
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

namespace Yetkilim.Global.Configuration
{
  public class SmtpOptions
  {
    public SmtpOptions()
    {
      this.EnableSsl = true;
      this.SenderName = "Yetkilim";
    }

    public string Host { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public bool EnableSsl { get; set; }

    public string SenderName { get; set; }
  }
}
