﻿// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.PlaceDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

namespace Yetkilim.Domain.DTO
{
  public class PlaceDTO
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int CompanyId { get; set; }

    public CompanyInfoDTO Company { get; set; }
  }
}
