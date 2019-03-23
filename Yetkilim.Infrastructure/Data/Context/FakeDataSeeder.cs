//// Decompiled with JetBrains decompiler
//// Type: Yetkilim.Infrastructure.Data.Context.FakeDataSeeder
//// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
//// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
//// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

//using Bogus;
//using GeoAPI.Geometries;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using Yetkilim.Domain.Entity;
//using Yetkilim.Infrastructure.Data.Model;

//namespace Yetkilim.Infrastructure.Data.Context
//{
//  public static class FakeDataSeeder
//  {
//    public static void Seed(YetkilimDbContext context)
//    {
//      CompanyType[] companyTypeArray = new CompanyType[2];
//      CompanyType companyType1 = new CompanyType();
//      companyType1.Id = 1;
//      companyType1.Code = "CAFE";
//      companyType1.Name = "Cafe";
//      companyTypeArray[0] = companyType1;
//      CompanyType companyType2 = new CompanyType();
//      companyType2.Id = 2;
//      companyType2.Code = "RESTAURANT";
//      companyType2.Name = "Restoran";
//      companyTypeArray[1] = companyType2;
//      CompanyType[] companyTypes = companyTypeArray;
//      if (!context.Companies.Any<Company>())
//      {
//        List<Company> companyList = new Faker<Company>("tr").RuleFor<string>((Expression<Func<Company, M0>>) (o => o.Name), (Func<Faker, M0>) (f => f.get_Company().CompanyName(new int?()))).RuleFor<string>((Expression<Func<Company, M0>>) (o => o.Address), (Func<Faker, M0>) (f => f.get_Address().FullAddress())).RuleFor<string>((Expression<Func<Company, M0>>) (o => o.Image), (Func<Faker, M0>) (f => f.get_Image().LoremPixelUrl("food", 640, 480, false, true))).RuleFor<int>((Expression<Func<Company, M0>>) (o => o.CompanyTypeId), (Func<Faker, M0>) (f => ((Yetkilim.Domain.Entity.Entity<int>) f.PickRandom<CompanyType>((M0[]) companyTypes)).Id)).RuleFor<string>((Expression<Func<Company, M0>>) (o => o.CreatedBy), (Func<Faker, M0>) (f => "fake")).RuleFor<DateTime>((Expression<Func<Company, M0>>) (o => o.CreatedDate), (M0) DateTime.Now).Generate(10, (string) null);
//        context.Companies.AddRange((IEnumerable<Company>) companyList);
//        context.SaveChanges();
//      }
//      if (context.Places.Any<Place>())
//        return;
//      int capacity = 50;
//      List<Company> companies = context.Companies.ToList<Company>();
//      if (companies.Count > 0)
//      {
//        Random random1 = new Random();
//        List<double> coordinatesLat = new List<double>(capacity);
//        for (int index = 0; index < capacity; ++index)
//          coordinatesLat.Add(41.015137 - random1.NextDouble() / 100.0);
//        Random random2 = new Random();
//        List<double> coordinatesLong = new List<double>(capacity);
//        for (int index = 0; index < capacity; ++index)
//          coordinatesLong.Add(29.27953 - random2.NextDouble() / 100.0);
//        List<Place> placeList = new Faker<Place>("tr").RuleFor<string>((Expression<Func<Place, M0>>) (o => o.Name), (Func<Faker, M0>) (f => f.get_Company().CompanyName(new int?()))).RuleFor<string>((Expression<Func<Place, M0>>) (o => o.Address), (Func<Faker, M0>) (f => f.get_Address().FullAddress())).RuleFor<IPoint>((Expression<Func<Place, M0>>) (o => o.Location), (Func<Faker, M0>) (f => (IPoint) new XYPoint((double) f.PickRandom<double>((List<M0>) coordinatesLat), (double) f.PickRandom<double>((List<M0>) coordinatesLong)))).RuleFor<int>((Expression<Func<Place, M0>>) (o => o.CompanyId), (Func<Faker, M0>) (f => ((Yetkilim.Domain.Entity.Entity<int>) f.PickRandom<Company>((List<M0>) companies)).Id)).RuleFor<string>((Expression<Func<Place, M0>>) (o => o.CreatedBy), (Func<Faker, M0>) (f => "fake")).RuleFor<DateTime>((Expression<Func<Place, M0>>) (o => o.CreatedDate), (M0) DateTime.Now).Generate(capacity, (string) null);
//        context.Places.AddRange((IEnumerable<Place>) placeList);
//        context.SaveChanges();
//      }
//    }
//  }
//}
