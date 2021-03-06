﻿// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Entity.Promotion
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;

namespace Yetkilim.Domain.Entity
{
    public class Promotion : TrackableEntity<int>
    {
        public string Title
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string UsageCode
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public DateTime DueDate
        {
            get;
            set;
        }

        public virtual Place Place
        {
            get;
            set;
        }

        public int PlaceId
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }
    }
}
