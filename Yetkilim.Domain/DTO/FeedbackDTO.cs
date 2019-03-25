// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.DTO.FeedbackDTO
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

namespace Yetkilim.Domain.DTO
{
    public class FeedbackDTO : TrackableEntityDTO
    {
        public int Id
        {
            get;
            set;
        }

        public int FormId
        {
            get;
            set;
        }

        public int LikeRate
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

        public bool IsUserShare
        {
            get;
            set;
        }

        public string FormValue
        {
            get;
            set;
        }

        public string BrowserFp
        {
            get;
            set;
        }

        public string IpAddress
        {
            get;
            set;
        }

        public PlaceInfoDTO Place
        {
            get;
            set;
        }

        public FeedbackDetailInfoDTO Detail
        {
            get;
            set;
        }

        public int? UserId
        {
            get;
            set;
        }

        public UserInfoDTO User
        {
            get;
            set;
        }

        public int PlaceId
        {
            get;
            set;
        }
    }
}
