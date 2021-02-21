using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class UmAlquraCalViewModel : IValidatableObject
    {
        //[CustomDisplay("RankText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Day { get; set; }

        //[CustomDisplay("MonthText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Month { get; set; }

        //[DataType(DataType.Date)]
        //[CustomDisplay("ReadingText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Year { get; set; }

        //[CustomDisplay("ReadingText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? FullDate
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Year) || !string.IsNullOrEmpty(this.Month) || !string.IsNullOrEmpty(this.Day))
                    return new DateTime(int.Parse(this.Year), int.Parse(this.Month), int.Parse(this.Day));
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Day = Globals.Calendar.GetUmAlQuraDay(value.Value).ToString().Length > 1 ? Globals.Calendar.GetUmAlQuraDay(value.Value).ToString() : "0" + Globals.Calendar.GetUmAlQuraDay(value.Value).ToString();
                    this.Month = Globals.Calendar.GetUmAlQuraMonth(value.Value).ToString().Length > 1 ? Globals.Calendar.GetUmAlQuraMonth(value.Value).ToString() : "0" + Globals.Calendar.GetUmAlQuraMonth(value.Value).ToString();
                    this.Year = Globals.Calendar.GetUmAlQuraYear(value.Value).ToString();
                }
            }
        }

        public bool IsRequired { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsRequired)
            {
                if (!string.IsNullOrEmpty(this.Year) || !string.IsNullOrEmpty(this.Month) || !string.IsNullOrEmpty(this.Day))
                    yield return new ValidationResult("لابد من ادخال التاريخ بشكل كامل");
            }
        }
    }
}