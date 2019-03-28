using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yetkilim.Domain.Entity;
using Yetkilim.Global.Context;
using Yetkilim.Infrastructure.Data.UnitOfWork;

namespace Yetkilim.Business.Services
{
    public interface ICompanyFeedbackService
    {
        IQueryable<CompanyFeedback> GetCompanyFeedbackQueryable();
    }
    public class CompanyFeedbackService : ServiceBase, ICompanyFeedbackService
    {
        private readonly IYetkilimUnitOfWork _unitOfWork;


        public CompanyFeedbackService(IYetkilimUnitOfWork unitOfWork, IGlobalContext globalContext)
            : base(globalContext)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<CompanyFeedback> GetCompanyFeedbackQueryable()
        {
            return _unitOfWork.EntityRepository<CompanyFeedback>().GetQueryable(null, null);
        }
    }

  
}
