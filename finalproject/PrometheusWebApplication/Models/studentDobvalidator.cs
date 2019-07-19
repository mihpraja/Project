using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrometheusWebApplication.Models
{
    public class studentDobvalidator : ValidationAttribute
    {
        /// <summary>
        /// Custom validator for Student DOB.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult
IsValid(object value, ValidationContext validationContext)
        {
            var model = (Models.Student)validationContext.ObjectInstance;
            DateTime dateofbirth = Convert.ToDateTime(model.DOB);

            DateTime dt2 = DateTime.Now;



            double year = (dt2 - dateofbirth).Days / 365;





            if (year < 16 || year > 25)
            {
                return new ValidationResult
                    ("age should be between 16 and 20 years");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}