using System;
using System.Collections.Generic;
using System.Text;

namespace Yetkilim.Domain.DTO
{
    public class PromotionDTO : TrackableEntityDTO
    {
        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string UsageCode
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public DateTime DueDate
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public PlaceInfoDTO Place
        {
            get;
            set;
        }

        public int PlaceId
        {
            get;
            set;
        }

        public UserInfoDTO User
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }
    }
}
