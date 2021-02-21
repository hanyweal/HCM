using ExceptionNameSpace;
using HCMBLL;
using HCMBLL.Enums;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System;

namespace HCM.Classes.Helpers
{
    public class CommonHelper  : Controller
    {
        public static string DateFormat
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString();
            }
        }

        public JsonResult SetJsonResultWithMaxJsonLength(object DataToConvertToJson)
        {
            var jsonResult = Json(new { data = DataToConvertToJson }, JsonRequestBehavior.AllowGet);
            //int len = jsonResult.MaxJsonLength.HasValue ? jsonResult.MaxJsonLength.Value : 0;
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region Conflict Validation Messages
        public static void ConflictValidationMessage(Result result)
        {
            if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithScholarship.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithScholarshipText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithLender.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithLenderText);
            else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithExternalAssigning.ToString())
                throw new CustomException(Resources.Globalization.ValidationConflictWithExternalAssigningText);
        }

        #endregion
    }
}