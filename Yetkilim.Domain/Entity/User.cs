// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.User
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Domain.Entity
{
  public class User : TrackableEntity<int>
  {
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(255)]
    public string Email { get; set; }

    public string Phone { get; set; }

    public bool IsExternal { get; set; }

    public string Password { get; set; }

    public bool IsDeleted { get; set; }

    public int? ExternalUserId { get; set; }

    public virtual ExternalUser ExternalUser { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; }
  }
}
