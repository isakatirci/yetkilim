// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.FeedbackDetailDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;

namespace Yetkilim.Domain.DTO
{
    public class FeedbackDetailDTO
    {
        public int Id
        {
            get;
            set;
        }

        public bool IsAnon
        {
            get;
            set;
        }

        public UserDTO User
        {
            get;
            set;
        }

        public bool IsUserShare
        {
            get;
            set;
        }

        public string DeskCode
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public FeedbackDetailInfoDTO Info
        {
            get;
            set;
        }

        public int PlcId
        {
            get;
            set;
        }

        public string Place
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }
    }

}
