// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Consts
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

namespace Yetkilim.Global
{
  public class Consts
  {
    public class UploadFolders
    {
      public const string BaseFolder = "Uploads";
      public const string AdminBaseFolder = "admin/uploads";
    }

    public class Authentication
    {
      public const string IdentityType = "ClaimIdentity";
    }

    public class UserRoles
    {
      public const string SuperAdmin = "SuperAdmin";
      public const string Admin = "Admin";
      public const string Dealer = "Dealer";
      public const string Member = "Member";
    }

    public class AdminArea
    {
      public const string AuthenticationScheme = "AdminAreaCookies";
      public const string BaseRole = "SuperAdmin,Admin";
    }
  }
}
