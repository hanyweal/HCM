using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class HolidaysAttendanceViewModel : BaseViewModel
    {
        public HolidaysAttendanceViewModel()
        {

        }
        public int HolidayAttendanceID { get; set; }

        //[CustomDisplay("MaturityYearBalanceText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public HolidaysSettingsBLL HolidaySetting { get; set; }

        #region HolidaySetting
        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int MaturityYearID { get; set; }

        [CustomDisplay("HolidayTypeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int HolidayTypeID { get; set; }
        public HolidaysSettingsBLL HolidaySetting { get; set; }
        #endregion

        #region Organization
        [CustomDisplay("WantedAddedOrganizationText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int OrganizationID { get; set; }

        public OrganizationsStructuresBLL Organization { get; set; }
        #endregion

        public HolidaysAttendanceDetailsBLL HolidayAttendanceDetailRequest { get; set; }

    }
}