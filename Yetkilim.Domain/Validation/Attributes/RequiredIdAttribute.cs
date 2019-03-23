// Decompiled with JetBrains decompiler
// Type: Yetkilim.Domain.Validation.Attributes.RequiredIdAttribute
// Assembly: Yetkilim.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9451A12F-2E26-461F-B515-9B3493CE1118
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Domain.dll

using System.ComponentModel.DataAnnotations;

namespace Yetkilim.Domain.Validation.Attributes
{
    public class RequiredIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
          object value,
          ValidationContext validationContext)
        {
            int result;
            int.TryParse(value.ToString(), out result);
            if (result > 0)
                return ValidationResult.Success;
            return new ValidationResult(this.GetErrorMessage(validationContext));
        }

        private string GetErrorMessage(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
                return this.ErrorMessage;
            return validationContext.DisplayName + " değeri set edilmeli!";
        }
    }
}
