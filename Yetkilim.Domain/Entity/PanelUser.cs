// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.PanelUser
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System.ComponentModel.DataAnnotations;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Domain.Entity
{
    public class PanelUser : TrackableEntity<int>
    {
        [StringLength(100)]
        public string Name
        {
            get;
            set;
        }

        [StringLength(100)]
        public string Surname
        {
            get;
            set;
        }

        [StringLength(255)]
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

        public bool IsDeleted
        {
            get;
            set;
        }

        public string ResetCode
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

        public Company Company
        {
            get;
            set;
        }

        public int? PlaceId
        {
            get;
            set;
        }

        public Place Place
        {
            get;
            set;
        }
    }
}
