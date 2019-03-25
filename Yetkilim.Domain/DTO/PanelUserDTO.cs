// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.PanelUserDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Domain.DTO
{
    public class PanelUserDTO
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Surname
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public UserRole Role
        {
            get;
            set;
        }

        public int CompanyId
        {
            get;
            set;
        }

        public int? PlaceId
        {
            get;
            set;
        }

        public CompanyInfoDTO Company
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public string PlaceName
        {
            get;
            set;
        }
    }

}
