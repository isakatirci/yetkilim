// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Context.ModelBuilderExtensions
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using System;
using Yetkilim.Domain.Entity;
using Yetkilim.Domain.Enums;

namespace Yetkilim.Infrastructure.Data.Context
{
  public static class ModelBuilderExtensions
  {
    public static void Seed(this ModelBuilder modelBuilder)
    {
      CompanyType[] companyTypeArray1 = new CompanyType[2];
      CompanyType companyType1 = new CompanyType();
      companyType1.Id = 1;
      companyType1.Code = "CAFE";
      companyType1.Name = "Cafe";
      companyTypeArray1[0] = companyType1;
      CompanyType companyType2 = new CompanyType();
      companyType2.Id = 2;
      companyType2.Code = "RESTAURANT";
      companyType2.Name = "Restoran";
      companyTypeArray1[1] = companyType2;
      CompanyType[] companyTypeArray2 = companyTypeArray1;
      modelBuilder.Entity<CompanyType>().HasData(companyTypeArray2);
      Company company1 = new Company();
      company1.Id = 1;
      company1.Name = "Yetkilim A.Ş";
      company1.CompanyTypeId = 1;
      company1.CreatedDate = DateTime.Now;
      company1.CreatedBy = nameof (Seed);
      Company company2 = company1;
      modelBuilder.Entity<Company>().HasData(new Company[1]
      {
        company2
      });
      PanelUser[] panelUserArray1 = new PanelUser[1];
      PanelUser panelUser = new PanelUser();
      panelUser.Id = 1;
      panelUser.Name = "Super Admin";
      panelUser.CompanyId = 1;
      panelUser.Email = "super@yetkilim.com";
      panelUser.Password = "4BD37BEC2EAD95835D4B10BBF86A5649";
      panelUser.Role = UserRole.SuperAdmin;
      panelUser.CreatedDate = DateTime.Now;
      panelUserArray1[0] = panelUser;
      PanelUser[] panelUserArray2 = panelUserArray1;
      modelBuilder.Entity<PanelUser>().HasData(panelUserArray2);
      FeedbackForm feedbackForm1 = new FeedbackForm();
      feedbackForm1.Id = 1;
      feedbackForm1.CompanyId = 1;
      feedbackForm1.CreatedDate = DateTime.Now;
      FeedbackForm feedbackForm2 = feedbackForm1;
      modelBuilder.Entity<FeedbackForm>().HasData(new FeedbackForm[1]
      {
        feedbackForm2
      });
    }
  }
}
