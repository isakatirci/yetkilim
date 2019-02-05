using System;
using System.Collections.Generic;

namespace Yetkilim.Web.Models
{
    public class MyFeedbacksViewModel
    {
        public List<MyFeedbackViewItem> Feedbacks { get; set; }

        public string FormMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class MyFeedbackViewItem
    {
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}