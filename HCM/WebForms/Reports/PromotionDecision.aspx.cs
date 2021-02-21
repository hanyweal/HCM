using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class PromotionDecision : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            int PromotionRecordID = 0;
            int? CurrentEmployeeCareerHistoryID = null;
            ReportParameter RpPromotionRecordID = new ReportParameter("PromotionRecordID");
            ReportParameter RpCurrentEmployeeCareerHistoryID = new ReportParameter("CurrentEmployeeCareerHistoryID");

            if (Request.QueryString["PromotionRecordID"] != "")
            {
                PromotionRecordID = int.Parse(Request.QueryString["PromotionRecordID"].ToString());
                RpPromotionRecordID.Values.Add(PromotionRecordID.ToString());
            }
            if (Request.QueryString["EmployeeCarrerHistoryID"] != "")
            {
                CurrentEmployeeCareerHistoryID = int.Parse(Request.QueryString["EmployeeCarrerHistoryID"].ToString());
                RpCurrentEmployeeCareerHistoryID.Values.Add(CurrentEmployeeCareerHistoryID.Value.ToString());
            }



            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "PromotionsDecisions";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, RpPromotionRecordID, RpCurrentEmployeeCareerHistoryID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}