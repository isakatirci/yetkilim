// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.ITrackableEntity
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;

namespace Yetkilim.Domain.Entity
{
  public interface ITrackableEntity : IEntity
  {
    DateTime CreatedDate { get; set; }

    DateTime? ModifiedDate { get; set; }

    string CreatedBy { get; set; }

    string ModifiedBy { get; set; }
  }
}
