namespace Yetkilim.Web.Models
{
    public class FeedbackRequestModel
    {
        public int FormId { get; set; }
        /// <summary>
        ///     JSON Form
        /// </summary>
        public string FormValue { get; set; }

        public bool IsUserShare { get; set; }
        public string Description { get; set; }

        public int? UserId { get; set; }

        public int EmployeeRate { get; set; }
        public int FlavorRate { get; set; }
        public int PriceRate { get; set; }
        public int CleaningRate { get; set; }
        public int AdviseRate { get; set; }

        public string BrowserFp { get; set; }
        public string IpAddress { get; set; }
    }
}