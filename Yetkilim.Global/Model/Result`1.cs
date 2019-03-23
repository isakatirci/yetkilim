// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Model.Result`1
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using System;
using System.Collections.Generic;

namespace Yetkilim.Global.Model
{
    public class Result<T> : Result
    {
        public new T Data
        {
            get;
            protected set;
        }

        public Result()
        {
        }

        public Result(T data)
        {
            base.IsSuccess = true;
            Data = data;
        }

        public virtual Result<T> Success(T dataVal, string message = null)
        {
            Data = dataVal;
            if (message != null)
            {
                base.Messages = new string[1]
                {
                message
                };
            }
            base.IsSuccess = true;
            return this;
        }

        public new virtual Result<T> Success(string message = null)
        {
            if (message != null)
            {
                base.Messages = new string[1]
                {
                message
                };
            }
            base.IsSuccess = true;
            return this;
        }

        public new virtual Result<T> Fail(string message = null, Exception ex = null, string resultCode = null)
        {
            if (message != null)
            {
                base.Messages = new string[1]
                {
                message
                };
            }
            if (ex != null)
            {
                base.Exception = ex;
            }
            if (resultCode != null)
            {
                base.ResultCode = resultCode;
            }
            base.IsSuccess = false;
            return this;
        }

        public new virtual Result<T> Fail(IEnumerable<string> messages = null, Exception ex = null, string resultCode = null)
        {
            if (messages != null)
            {
                base.Messages = messages;
            }
            if (ex != null)
            {
                base.Exception = ex;
            }
            if (resultCode != null)
            {
                base.ResultCode = resultCode;
            }
            base.IsSuccess = false;
            return this;
        }
    }
}
