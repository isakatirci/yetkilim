﻿// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.CompanyDetailDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

namespace Yetkilim.Domain.DTO
{
    public class CompanyDetailDTO
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }
        public string Demo { get; set; }

        public int CompanyTypeId { get; set; }

        public ParameterDTO CompanyType { get; set; }
    }
}
