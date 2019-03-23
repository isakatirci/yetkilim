// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.FeedbackDetail
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

namespace Yetkilim.Domain.Entity
{
  public class FeedbackDetail : TrackableEntity<int>
  {
    public int EmployeeRate { get; set; }

    public int FlavorRate { get; set; }

    public int PriceRate { get; set; }

    public int CleaningRate { get; set; }

    public int AdviseRate { get; set; }
  }
}
