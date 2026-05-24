using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Services.Helpers.CustomValidations
{
    public class ValidatingFileTypeAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly int _maxFileSizeMb = 5;
        private readonly bool _isRequiredForNew;

        public ValidatingFileTypeAttribute(bool isRequiredForNew = true)
        {
            _isRequiredForNew = isRequiredForNew;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            // Checking if fille is null
            if (file == null || file.Length == 0)
            {
                //cehcking if it is of Productupdate request or Productadd request means the admin has upadate the image or not and also checking if it is adding product (this can happen if the admin does not select the image then we have to show erro message)

                if (IsNewProduct(validationContext) && _isRequiredForNew)
                {
                    return new ValidationResult("Product image is required for new products");
                }
                return ValidationResult.Success;
            }

            // if file is not null means we are validating it (it can be of time adding a product or updating a product)
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult("Only .jpg, .jpeg, .png types are allowed");
            }

            if (file.Length > _maxFileSizeMb * 1024 * 1024)
            {
                return new ValidationResult($"File size must be less than {_maxFileSizeMb} MB");
            }

            return ValidationResult.Success;
        }

        private bool IsNewProduct(ValidationContext context)
        {
            //Checking if productid is exist or not and getting that productid value by reflection
            var idProperty = context.ObjectType.GetProperty("ProductId");
            if (idProperty != null)
            {
                //if idproperty is not equal to null it means that product id exist so it should return false (because id is not null and neither idValue.Equals(0) is true so it will return false) means it is of updating any product but the admin does not want to update the product image. 
                
                var idValue = idProperty.GetValue(context.ObjectInstance);
                return idValue == null || idValue.Equals(0);
            }
            return true; // If no Id property value exists then returning true bcz it means that product does not exist so it happens when adimn is inserting new product so for that we have to validate Image type 
        }
    }
}

