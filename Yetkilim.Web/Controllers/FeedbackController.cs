using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yetkilim.Web.Models.Ef;

namespace Yetkilim.Web.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index(string feedbackid)
        {
            if (!string.IsNullOrWhiteSpace(feedbackid))
            {
                return View("~/Views/Feedback/Feedback" + feedbackid + ".cshtml");
            }

            return View();

            //using (yetkilimDBContext db = new yetkilimDBContext())
            //{
            //    db.FeedbackForms.Add(new FeedbackForms
            //    {
            //        CreatedDate = DateTime.Now,
            //        CompanyId = 1
            //    });
            //    db.SaveChanges();
            //}
        }


        //AdviseRate: 4
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //CleaningRate: 4
        //Description: "İletmek istedikleriniz↵"
        //DoktorUzmanligi: 2
        //EmployeeRate: 3
        //FormId: ""
        //IsUserShare: false
        //UzmanCesidi: 3



        //AdviseRate: 3
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //CleaningRate: 3
        //Description: "İletmek istedikleriniz↵"
        //EmployeeRate: 5
        //FormId: ""
        //IsUserShare: false
        //PriceRate: 3
        //ReyonGorevlisi: "Reyon Görevlisi"
        //UrunCesidi: 1


        //AdviseRate: 3
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //Description: "İletmek istedikleriniz↵"
        //EmployeeRate: 3
        //EtkinlikAdi: "Etkinlik Adı"
        //FormId: ""
        //IsUserShare: false
        //MekanYeterliligi: 3
        //PlanaUyum: 3


        //AdviseRate: 3
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //CleaningRate: 4
        //Description: "İletmek istedikleriniz↵"
        //EmployeeRate: 4
        //FormId: ""
        //Ikram: w.fn.init []
        //IsUserShare: false
        //Rotar: 4
        //SafeDrive: 4
        //SeatNo: "Koltuk Numarası"



        //AdviseRate: 3
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //Description: "İletmek istedikleriniz↵"
        //EmployeeRate: 2
        //FormId: ""
        //IsUserShare: false
        //WCCleaningRate: w.fn.init []


        //AdviseRate: 4
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //CleaningRate: 4
        //Description: "İletmek istedikleriniz↵"
        //EmployeeSkill: w.fn.init []
        //FormId: ""
        //IsUserShare: false
        //PriceRate: 2


        //AdviseRate: 4
        //BrowserFp: "0a16f3c7f08de3b63c301b64779dcc09"
        //CleaningRate: 3
        //Description: "İletmek istedikleriniz↵"
        //DeskCode: "Masa Kodu"
        //EmployeeRate: 4
        //FlavorRate: 3
        //FormId: ""
        //IsUserShare: false
        //PriceRate: 3



        public IActionResult Feedback(string feedbackid)
        {
            if (!string.IsNullOrWhiteSpace(feedbackid))
            {
                return Content("");            }

            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);
                switch (feedbackid)
                {
                    case "0":
                        break;
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("");
        }


    }
}