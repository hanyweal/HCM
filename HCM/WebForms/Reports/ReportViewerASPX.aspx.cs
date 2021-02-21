using HCMBLL;
using HCMBLL.Enums;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;

namespace HCM
{
    public partial class ReportViewerASPX : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string ID = Request.QueryString["ID"].ToString();
                string Type = Request.QueryString["type"].ToString();
                ReportServicesMethod RSM = new ReportServicesMethod();
                string strFileName = string.Empty;

                if (!Page.IsPostBack)
                {
                    ReportParameter EmployeeName = new ReportParameter("EmployeeName", Session["EmployeeName"].ToString());
                    if (Type.Equals(BusinessSubCategoriesEnum.Assignings.ToString()))
                    {
                        ReportParameter AssigningID = new ReportParameter("AssigningID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "Assigning", AssigningID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.Delegations.ToString()))
                    {
                        ReportParameter DelegationID = new ReportParameter("DelegationID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "Delegation", DelegationID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.PassengerOrders.ToString()))
                    {
                        ReportParameter PassengerOrderID = new ReportParameter("PassengerOrderID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "PassengerOrder", PassengerOrderID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.Lenders.ToString()))
                    {
                        ReportParameter LenderID = new ReportParameter("LenderID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "Lender", LenderID, EmployeeName);
                    }
                    else if (Type.Equals("EmployeeScholarship"))
                    {
                        ReportParameter ScholarshipID = new ReportParameter("ScholarshipID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "Scholarship", ScholarshipID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.OverTimes.ToString()))
                    {
                        ReportParameter OverTimeID = new ReportParameter("OverTimeID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "OverTime", OverTimeID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.Transfers.ToString()))
                    {
                        ReportParameter TransferID = new ReportParameter("TransferID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "Transfer", TransferID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.GovernmentFunds.ToString()))
                    {
                        ReportParameter GovernmentFundID = new ReportParameter("GovernmentFundID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "GovernmentFunds", GovernmentFundID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.GovernmentFundsDeactivation.ToString()))
                    {
                        ReportParameter GovernmentFundID = new ReportParameter("GovernmentFundID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "GovernmentFundsDeactivation", GovernmentFundID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.EmployeesAllowances.ToString()))
                    {
                        ReportParameter EmployeeAllowanceID = new ReportParameter("EmployeeAllowanceID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "EmployeeAllowance", EmployeeAllowanceID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.InternshipScholarships.ToString()))
                    {
                        ReportParameter InternshipScholarshipID = new ReportParameter("InternshipScholarshipID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "InternshipScholarship", InternshipScholarshipID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.Vacations.ToString()))
                    {
                        VacationsDetailsBLL VacationDetail = new VacationsDetailsBLL().GetVacationsDetailsByVacationDetailID(int.Parse(ID));
                        ReportParameter VacationDetailID = new ReportParameter("VacationDetailID", ID);
                        ReportParameter VacationID = new ReportParameter("VacationID", VacationDetail.Vacation.VacationID.ToString());
                        if (VacationDetail.VacationAction.VacationActionID == (int)VacationsActionsEnum.Add)
                        {
                            strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "VacationDetailCreation", VacationDetailID);
                        }
                        //else if (VacationDetail.Vacation.VacationType == VacationsTypesEnum.Normal)
                        //    strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "NormalVacation", VacationDetailID, EmployeeName);
                        else
                        {
                            strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "VacationDetailAction", VacationDetailID);
                        }
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.Scholarships.ToString()))
                    {
                        ScholarshipsDetailsBLL ScholarshipDetails = new ScholarshipsDetailsBLL().GetScholarshipsDetailsByScholarshipDetailID(int.Parse(ID));
                        ReportParameter ScholarshipDetailID = new ReportParameter("ScholarshipDetailID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "ScholarshipByScholarshipDetailID", ScholarshipDetailID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.VacationsByEndOfService.ToString()))
                    {
                        string TotalAvailableVacationBalance = "", TotalConsumedBalance = "", TotalRemainingBalance = "", TotalRemainingAvailableVacationBalance = "";
                        GetVacationBalances(ID, out TotalAvailableVacationBalance, out TotalConsumedBalance, out TotalRemainingBalance, out TotalRemainingAvailableVacationBalance);
                        ReportParameter EndOfServiceID = new ReportParameter("EndOfServiceID", ID);
                        ReportParameter PRTotalAvailableVacationBalance = new ReportParameter("TotalAvailableVacationBalance", TotalAvailableVacationBalance);
                        ReportParameter PRTotalConsumedBalance = new ReportParameter("TotalConsumedBalance", TotalConsumedBalance);
                        ReportParameter PRTotalRemainingBalance = new ReportParameter("TotalRemainingBalance", TotalRemainingBalance);
                        ReportParameter PRTotalRemainingAvailableVacationBalance = new ReportParameter("TotalRemainingAvailableVacationBalance", TotalRemainingAvailableVacationBalance);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "NormalVacationsByEOS", EndOfServiceID, PRTotalAvailableVacationBalance, PRTotalConsumedBalance, PRTotalRemainingBalance, PRTotalRemainingAvailableVacationBalance);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.EmployeeCareerHistoryByEndOfService.ToString()))
                    {
                        ReportParameter EndOfServiceID = new ReportParameter("EndOfServiceID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "EmployeeCareerHistoryByEOS", EndOfServiceID);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.StopWork.ToString()))
                    {
                        ReportParameter StopWorkID = new ReportParameter("StopWorkID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "StopWork", StopWorkID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.InsteadDeportations.ToString()))
                    {
                        ReportParameter InsteadDeportationID = new ReportParameter("InsteadDeportationID", ID);
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "InsteadDeportation", InsteadDeportationID, EmployeeName);
                    }
                    else if (Type.Equals(BusinessSubCategoriesEnum.ChangeLogs.ToString()))
                    {
                        ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", ID);
                        ReportParameter BusinssSubCategoryID = new ReportParameter("BusinssSubCategoryID", Request.QueryString["BusinssSubCategoryID"].ToString());
                        ReportParameter StartDate = new ReportParameter("StartDate", Globals.Calendar.UmAlquraToGreg(Request.QueryString["StartDate"].ToString()));
                        ReportParameter EndDate = new ReportParameter("EndDate", Globals.Calendar.UmAlquraToGreg(Request.QueryString["EndDate"].ToString()));
                        strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "ChangeLogs", EmployeeCodeID, BusinssSubCategoryID, StartDate, EndDate);
                    }
                    //else if (Type.Equals(BusinessSubCategoriesEnum.SickVacationsPaidAmount.ToString()))
                    //{     
                    //    strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, "SickVacationsPaidAmount");
                    //}

                    if (!string.IsNullOrEmpty(strFileName))
                        Response.Redirect(strFileName, false);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public void GetVacationBalances(string EndOfServiceID, out string TotalDeservedBalance, out string TotalConsumedBalance, out string TotalRemainingBalance, out string TotalDeservedRemainingBalance)
        {
            int endOfServiceID = int.Parse(EndOfServiceID);
            EndOfServicesBLL EndOfServices = new EndOfServicesBLL().GetByEndOfServiceID(endOfServiceID);
            //--======== Culculate Noraml vacation .
            var Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
            Vacation.VacationStartDate = Convert.ToDateTime(EndOfServices.EndOfServiceDate.ToString(ConfigurationManager.AppSettings["DateFormat"]), new CultureInfo("ar-SA")); //EndOfServices.EndOfServiceDate;
            int UmAlQuraYear, UmAlQuraMonth, UmAlQuraDay;
            UmAlQuraYear = Globals.Calendar.GetUmAlQuraYear(EndOfServices.EndOfServiceDate);
            UmAlQuraMonth = Globals.Calendar.GetUmAlQuraMonth(EndOfServices.EndOfServiceDate);
            UmAlQuraDay = Globals.Calendar.GetUmAlQuraDay(EndOfServices.EndOfServiceDate);
            ((NormalVacationsBLL)Vacation).GetVacationBalances(EndOfServices.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, UmAlQuraYear, UmAlQuraMonth, UmAlQuraDay);

            //--======== Culculate EndOfService Vacation .
            int TotalEndOfServiceVacationsConsumedBalance = 0;
            List<EndOfServicesVacationsBLL> EndOfServicesVacationsList = new EndOfServicesVacationsBLL().GetByEndOfServiceID(endOfServiceID);

            foreach (EndOfServicesVacationsBLL EndOfServiceVacation in EndOfServicesVacationsList)
            {
                if (EndOfServiceVacation.VacationType.VacationTypeID == (int)VacationsTypesEnum.Normal)
                    TotalEndOfServiceVacationsConsumedBalance += EndOfServiceVacation.VacationPeriod;
            }
            int MaxNormalCompensation = new EndOfServicesVacationsBLL().MaxNormalCompensation;

            TotalDeservedBalance = (Vacation as NormalVacationsBLL).TotalDeservedBalance.ToString();
            TotalConsumedBalance = ((Vacation as NormalVacationsBLL).TotalConsumedBalance + TotalEndOfServiceVacationsConsumedBalance).ToString();
            TotalRemainingBalance = ((Vacation as NormalVacationsBLL).TotalRemainingBalance - TotalEndOfServiceVacationsConsumedBalance).ToString();
            TotalDeservedRemainingBalance = int.Parse(TotalRemainingBalance) >= MaxNormalCompensation ? MaxNormalCompensation.ToString() : TotalRemainingBalance;
        }
    }
}