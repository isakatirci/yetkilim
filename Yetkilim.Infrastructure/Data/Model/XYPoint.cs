// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Model.XYPoint
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using NetTopologySuite.Geometries;

namespace Yetkilim.Infrastructure.Data.Model
{
    public class XYPoint : Point
    {
        //public XYPoint(double x, double y)
        //{
        //  base.\u002Ector(x, y);
        //  ((Geometry) this).set_SRID(4326);
        //}
        public XYPoint(double x, double y) : base(x, y)
        {
            ((Geometry)this).SRID = 4326;
        }
    }
}
