// Decompiled with JetBrains decompiler
// Type: Yetkilim.Global.Helpers.ValidatableObjectHelper
// Assembly: Yetkilim.Global, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64E10379-6F96-4F72-8B19-6E8446119219
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Global.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Global.Helpers
{
  public static class ValidatableObjectHelper
  {
    public static IEnumerable<string> GetValidationMessages(
      IEnumerable<ValidationResult> validationResults)
    {
      Collection<string> collection = new Collection<string>();
      foreach (ValidationResult validationResult in validationResults)
        collection.Add(validationResult.ErrorMessage);
      return (IEnumerable<string>) collection;
    }
  }
}
