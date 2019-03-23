// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.ParameterEntity
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Domain.Entity
{
  public class ParameterEntity : Yetkilim.Domain.Entity.Entity<int>
  {
    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(20)]
    public string Code { get; set; }
  }
}
