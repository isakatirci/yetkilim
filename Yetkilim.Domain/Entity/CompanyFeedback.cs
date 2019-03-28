using System;
using System.Collections.Generic;
using System.Text;

namespace Yetkilim.Domain.Entity
{
    public class CompanyFeedback : TrackableEntity<int>
    {
        public int FeedbackId { get; set; }
        public int TypeId { get; set; }
    }
}
