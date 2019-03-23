// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Model.Result
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yetkilim.Global.Helpers;

namespace Yetkilim.Global.Model
{
  public class Result
  {
    public bool IsSuccess { get; protected set; }

    public string ResultCode { get; protected set; }

    public IEnumerable<string> Messages { get; protected set; }

    public string FormMessage
    {
      get
      {
        if (this.Messages == null)
          return string.Empty;
        return string.Join("<br/>", this.Messages);
      }
    }

    public Exception Exception { get; protected set; }

    public static Result<TData> Data<TData>(TData data)
    {
      return new Result<TData>(data);
    }

    public static Result ValidationFail(IEnumerable<ValidationResult> errors)
    {
      return new Result().SetValidationFail(errors);
    }

    protected Result SetValidationFail(IEnumerable<ValidationResult> errors)
    {
      this.IsSuccess = false;
      this.Messages = ValidatableObjectHelper.GetValidationMessages(errors);
      return this;
    }

    public static Result Success(string message = null)
    {
      return new Result().SetSuccess(message);
    }

    protected virtual Result SetSuccess(string message = null)
    {
      if (message != null)
        this.Messages = (IEnumerable<string>) new string[1]
        {
          message
        };
      this.IsSuccess = true;
      return this;
    }

    public static Result Fail(string message = null, Exception ex = null, string resultCode = null)
    {
      return new Result().SetFail(message, ex, resultCode);
    }

    public static Result Fail(IEnumerable<string> messages = null, Exception ex = null, string resultCode = null)
    {
      return new Result()
      {
        IsSuccess = false,
        Messages = messages,
        Exception = ex,
        ResultCode = resultCode
      };
    }

    protected virtual Result SetFail(string message = null, Exception ex = null, string resultCode = null)
    {
      if (message != null)
        this.Messages = (IEnumerable<string>) new string[1]
        {
          message
        };
      if (ex != null)
        this.Exception = ex;
      if (resultCode != null)
        this.ResultCode = resultCode;
      this.IsSuccess = false;
      return this;
    }
  }
}
