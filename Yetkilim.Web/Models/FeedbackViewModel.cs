using System;
using System.Runtime.CompilerServices;

namespace Yetkilim.Web.Models
{
	public class FeedbackViewModel
	{
		public string FormData
		{
			get;
			set;
		}

		public int FormId
		{
			get;
			set;
		}

		public string FormMessage
		{
			get;
			set;
		}

		public bool IsLogged
		{
			get;
			set;
		}

		public FeedbackViewModel()
		{
		}
	}


    public class FeedbacksViewModel
    {
        /// <summary>
        ///     JSON Form
        /// </summary>
        public string FormData { get; set; }

        public string FormMessage { get; set; }
        public int FormId { get; set; }
    }

    public class FeedbacksViewModelBase
    {
        public string Error { get; set; }
        public string FormId { get; set; }
    }

    public class FeedbackViewModel0 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel1 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel2 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel3 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel4 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel5 : FeedbacksViewModelBase
    {

    }

    public class FeedbackViewModel6 : FeedbacksViewModelBase
    {

    }
    public class FeedbackViewModel7 : FeedbacksViewModelBase
    {

    }
}