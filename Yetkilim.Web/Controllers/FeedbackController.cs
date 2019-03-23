using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private bool myEquals(string value, string other)
        {
            return string.Equals(value, other, StringComparison.InvariantCultureIgnoreCase);
        }

        //class ComparerPropertyInfo : IComparer<PropertyInfo>
        //{
        //    public int Compare(PropertyInfo x, PropertyInfo y)
        //    {
        //        return string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
        //    }
        //}
        private string myTableMaker<T>(T[] list)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //Array.Sort(properties, new ComparerPropertyInfo());
            var table = "<table class=\"responsive-table striped highlight display nowrap\" style=\"width:100%\" id=\"feedbacktable\"><thead><tr>";
            for (int j = 0; j < properties.Length; j++)
            {
                table += "<td>" + properties[j].Name + "</td>";
            }
            table += "</tr></thead><tbody>";
            for (int i = 0; i < list.Length; i++)
            {
                table += "<tr>";
                for (int j = 0; j < properties.Length; j++)
                {
                    table += "<td>" + properties[j].GetValue(list[i], null) + "</td>";
                }
                table += "</tr>";
            }
            table += "</tbody></table>";

            return table;
        }

        public IActionResult FeedbackIndex(string feedbackid)
        {
            if (string.IsNullOrWhiteSpace(feedbackid))
            {
                return RedirectToAction(actionName: "Index");
            }

            var table = string.Empty;

            using (yetkilimDBContext db = new yetkilimDBContext())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                db.ChangeTracker.AutoDetectChangesEnabled = false;
                //db.ProxyCreationEnabled = false;            
                if (myEquals(feedbackid, "0"))
                {
                    table = myTableMaker(db.Feedback0.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "1"))
                {
                    table = myTableMaker(db.Feedback1.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "2"))
                {
                    table = myTableMaker(db.Feedback2.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "3"))
                {
                    table = myTableMaker(db.Feedback3.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "4"))
                {
                    table = myTableMaker(db.Feedback4.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "5"))
                {
                    table = myTableMaker(db.Feedback5.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "6"))
                {
                    table = myTableMaker(db.Feedback6.AsNoTracking().ToArray());
                }
                else if (myEquals(feedbackid, "7"))
                {
                    table = myTableMaker(db.Feedback7.AsNoTracking().ToArray());
                }
            }

            ViewBag.Table = table;

            return View();       
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



        public IActionResult Feedback0(Feedback0 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback0.Add(feedback);                    
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback1(Feedback1 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback1.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback2(Feedback2 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback2.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback3(Feedback3 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);   
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback3.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback4(Feedback4 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);  
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback4.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback5(Feedback5 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);  
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback5.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback6(Feedback6 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture); 
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback6.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }
        public IActionResult Feedback7(Feedback7 feedback)
        {
            try
            {
                //id = (int)Decimal.Parse(feedbackid, NumberStyles.Currency, CultureInfo.InvariantCulture);
                using (yetkilimDBContext db = new yetkilimDBContext())
                {
                    feedback.CreatedDate = DateTime.Now;
                    db.Feedback7.Add(feedback);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return new EmptyResult();
        }


    }
}