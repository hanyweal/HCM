using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class CommonHelper
    {
        public static Result IsNoConflictWithOtherProcess(int EmployeeCodeID, DateTime StartDate, DateTime EndDate, params BusinessSubCategoriesEnum[] ExcludeBusinessSubCategories)
        {
            Result result = null;

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.InternshipScholarships) == false)
            {
                #region Validaion for conflict with InternshipScholarship
                List<BaseInternshipScholarshipsBLL> BaseInternshipScholarshipBLLList = new BaseInternshipScholarshipsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (BaseInternshipScholarshipBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.OverTimes) == false)
            {
                #region Validaion for conflict with Overtime
                List<OverTimesBLL> OverTimesBLLList = new OverTimesBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                OverTimesBLL nn = new OverTimesBLL();
                if (OverTimesBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString();
                    result.Entity = nn;
                    return result;
                }

                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Delegations) == false)
            {
                #region Validaion for conflict with Delegation
                List<BaseDelegationsBLL> DelegationsBLLList = new BaseDelegationsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (DelegationsBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString();
                    return result;
                }

                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Vacations) == false)
            {
                #region Validaion for conflict with vacation
                List<BaseVacationsBLL> VacationsBLLList = new BaseVacationsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (VacationsBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.StopWork) == false)
            {
                #region Validaion for conflict with StopWorks
                List<StopWorksBLL> StopWorksBLLList = new StopWorksBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (StopWorksBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Scholarships) == false)
            {
                #region Validaion for conflict with Scholarship
                List<BaseScholarshipsBLL> BaseScholarshipsBLLList = new BaseScholarshipsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (BaseScholarshipsBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithScholarship.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Lenders) == false)
            {
                #region Validaion for conflict with Lenders
                List<LendersBLL> LendersBLLList = new LendersBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (LendersBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithLender.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Assignings) == false)
            {
                #region Validaion for conflict with External Assigning
                List<ExternalAssigningBLL> ExternalAssigningBLLList = new ExternalAssigningBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (ExternalAssigningBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithExternalAssigning.ToString();
                    return result;
                }
                #endregion
            }

            if (ExcludeBusinessSubCategories.Contains(BusinessSubCategoriesEnum.Teachers) == false)
            {
                #region Validaion for conflict with Teachers
                //List<TeachersBLL> TeachersBLLList = new TeachersBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                //if (TeachersBLLList.Count() != 0)
                //{
                //    result = new Result();
                //    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                //    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithTeacher.ToString();
                //    return result;
                //}
                #endregion
            }

            return result;
        }

        public static Result IsValidToCompleteEmployeesPlacement()
        {
            try
            {
                Result result;
                DateTime LastDayOfEmployeesPlacement = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["LastDayOfEmployeesPlacement"].ToString()).Date;
                if (LastDayOfEmployeesPlacement < DateTime.Now.Date)
                {
                    result = new Result();
                    //result.Entity = LastDayOfEmployeesPlacement;
                    result.EnumType = typeof(OrganizationStructureValidationEnum);
                    result.EnumMember = OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString();
                    return result;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static double CalculateEndOfServiceCompensation(double TotalSalary, double YearsCount)
        {
            // in case of end of service during first 5 years : 15 days cost from total salary per year * Years count
            double Compensation = 0;
            if (YearsCount > 5)
            {
                Compensation = (5 * TotalSalary) * .50;
                Compensation += ((YearsCount - 5) * TotalSalary);
            }
            if (YearsCount <= 5)
            {
                Compensation = (YearsCount * TotalSalary) * .50;
            }

            return Compensation;
        }


        //internal Result GetExceptionMessage(out Result result, HttpResponseMessage httpResponseMessage)
        //{

        //    if (httpResponseMessage.StatusCode == HttpStatusCodeAddition.BusinessValidation)
        //    {
        //        result = httpResponseMessage.Content.ReadAsAsync<Result>().Result;
        //        return result;
        //    }
        //    else
        //    {
        //        string ErrorMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
        //        throw new CustomException(ErrorMessage);
        //    }
        //}
    }
}