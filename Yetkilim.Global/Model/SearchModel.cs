// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Model.SearchModel
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using System;

namespace Yetkilim.Global.Model
{
  public class SearchModel
  {
    public string SearchText { get; set; }

    public int PageSize { get; set; } = 20;

    public int Page { get; set; } = 1;

    public int PageIndex { get; private set; }

    public int Skip
    {
      get
      {
        return this.PageSize * this.PageIndex;
      }
    }

    public void FixPageDefinations()
    {
      if (this.PageSize < 0)
        this.PageSize = 20;
      if (this.Page < 1)
        this.Page = 1;
      this.PageIndex = Math.Max(0, this.Page - 1);
    }
  }
}
