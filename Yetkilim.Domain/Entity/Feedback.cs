// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.Feedback
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

namespace Yetkilim.Domain.Entity
{
    public class Feedback : TrackableEntity<int>
    {
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

        public int LikeRate
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

        public bool IsDeleted
        {
            get;
            set;
        }

        public int FormId
        {
            get;
            set;
        }

        public virtual FeedbackForm Form
        {
            get;
            set;
        }

        public int? DetailId
        {
            get;
            set;
        }

        public virtual FeedbackDetail Detail
        {
            get;
            set;
        }

        public int? UserId
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public int PlaceId
        {
            get;
            set;
        }

        public virtual Place Place
        {
            get;
            set;
        }
    }
}
