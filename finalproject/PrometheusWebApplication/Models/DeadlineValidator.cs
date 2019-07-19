using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrometheusWebApplication.Models
{
    public class DeadlineValidator: ValidationAttribute
    {
        /// <summary>
        /// Custom validator for deadline.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult
        IsValid(object value, ValidationContext validationContext)
        {
            var model = (Models.Homework)validationContext.ObjectInstance;
            DateTime deadlinetime = Convert.ToDateTime(model.Deadline);
            

            if (deadlinetime <= DateTime.Now)
            {
                return new ValidationResult
                    ("date of submission should be given atleast one day time");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}