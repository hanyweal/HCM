using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class PromotionRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int PromotionRecordID = 0;
            ReportParameter RpPromotionRecordID = new ReportParameter("PromotionRecordID");

            if (Request.QueryString["PromotionRecordID"] != "")
            {
                PromotionRecordID = int.Parse(Request.QueryString["PromotionRecordID"].ToString());
                RpPromotionRecordID.Values.Add(PromotionRecordID.ToString());
            }

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "PromotionRecord";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, RpPromotionRecordID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}