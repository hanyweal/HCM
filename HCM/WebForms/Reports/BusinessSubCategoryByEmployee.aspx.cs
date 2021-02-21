using HCMBLL;
using HCMBLL.Enums;
using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class BusinessSubCategoryByEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", Request.QueryString["ID"].ToString());
            string Type = Request.QueryString["Type"].ToString();
            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                if (Type.Equals(BusinessSubCategoriesEnum.Assignings.ToString()))
                    ReportName = "AssigningsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.Delegations.ToString()))
                    ReportName = "DelegationsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.Lenders.ToString()))
                    ReportName = "LendersByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.OverTimes.ToString()))
                    ReportName = "OverTimesByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.PassengerOrders.ToString()))
                    ReportName = "PassengerOrdersByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.EmployeesAllowances.ToString()))
                    ReportName = "EmployeesAllowancesByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.GovernmentFunds.ToString()))
                    ReportName = "GovernmentFundsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.InternshipScholarships.ToString()))
                    ReportName = "InternshipScholarshipsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.Vacations.ToString()))
                    ReportName = "VacationsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.Scholarships.ToString()))
                    ReportName = "ScholarshipByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.EmployeesQualifications.ToString()))
                    ReportName = "EmployeesQualificationsByEmployee";

                else if (Type.Equals(BusinessSubCategoriesEnum.EmployeesExperiencesDetails.ToString()))
                {
                    ReportName = "EmployeesExperiencesDetailsByEmployee";                                       
                }
            }

            if (!Page.IsPostBack)
            {
                // Associated with Task 298: Showing the total experiences days of employee in printing template
                if (Type.Equals(BusinessSubCategoriesEnum.EmployeesExperiencesDetails.ToString()))
                {
                    int years, months, days;
                    int employeeCodeID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                    new EmployeeExperiencesWithDetailsBLL().GetTotalEmployeeExperienceInYMD(employeeCodeID, out years, out months, out days);

                    ReportParameter ParamYears = new ReportParameter("years", years.ToString());
                    ReportParameter ParamMonths = new ReportParameter("months", months.ToString());
                    ReportParameter ParamDays = new ReportParameter("days", days.ToString());
                    strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, EmployeeCodeID, 
                                                        ParamYears, ParamMonths, ParamDays);
                }
                else
                {
                    strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, EmployeeCodeID);
                }

                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }
        }
    }
}