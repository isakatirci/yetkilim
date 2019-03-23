// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.TrackableEntityDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;

namespace Yetkilim.Domain.DTO
{
  public abstract class TrackableEntityDTO
  {
    private DateTime? _createdDate;

    public virtual DateTime? ModifiedDate { get; set; }

    public virtual DateTime CreatedDate
    {
      get
      {
        return this._createdDate ?? DateTime.UtcNow;
      }
      set
      {
        this._createdDate = new DateTime?(value);
      }
    }

    public virtual string CreatedBy { get; set; }

    public virtual string ModifiedBy { get; set; }
  }
}
