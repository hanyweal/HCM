using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class PromotionRecordsByPromotionPeriod : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter PromotionPeriodID = new ReportParameter("PromotionPeriodID", Request.QueryString["PromotionPeriodID"] != "" ? Request.QueryString["PromotionPeriodID"].ToString() : null);

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "PromotionRecordsCountByPromotionPeriod";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, PromotionPeriodID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
