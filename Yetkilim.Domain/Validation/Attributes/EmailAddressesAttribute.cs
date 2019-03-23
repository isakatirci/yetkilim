// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Validation.Attributes.EmailAddressesAttribute
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Domain.Validation.Attributes
{
  public class EmailAddressesAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(
      object value,
      ValidationContext validationContext)
    {
      if (!(value is IEnumerable<string> strings))
        throw new ArgumentException("Attribute not applied");
      EmailAddressAttribute addressAttribute = new EmailAddressAttribute();
      foreach (string invalidEmail in strings)
      {
        if (!addressAttribute.IsValid((object) invalidEmail))
          return new ValidationResult(this.GetErrorMessage(validationContext, invalidEmail));
      }
      return ValidationResult.Success;
    }

    private string GetErrorMessage(ValidationContext validationContext, string invalidEmail)
    {
      if (!string.IsNullOrEmpty(this.ErrorMessage))
        return this.ErrorMessage;
      return validationContext.DisplayName + " değeri valid email adresleri içermeli! (" + invalidEmail + ")";
    }
  }
}
