// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.UserDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;

namespace Yetkilim.Domain.DTO
{
  public class UserDTO
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public bool IsExternal { get; set; }

    public string Password { get; set; }

    public int? ExternalUserId { get; set; }

    public DateTime CreatedDate { get; set; }
  }
}
