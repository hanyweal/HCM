using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class AfterRatificationPromotionCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string PromotionCardPrintingID = Request.QueryString["PromotionCardPrintingID"].ToString();
            ReportParameter PromotionPeriodID = new ReportParameter("PromotionPeriodID", Request.QueryString["PromotionPeriodID"].ToString());
            ReportParameter EmployeeCodeID = new ReportParameter("EmployeeCodeID", Request.QueryString["EmployeeCodeID"].ToString());
            ReportParameter IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllownces = new ReportParameter("IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllownces", Request.QueryString["IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllownces"].ToString());

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "AfterRatificationPromotionCard";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, EmployeeCodeID, PromotionPeriodID, IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllownces);
                if (!string.IsNullOrEmpty(strFileName))
                {
                    string ShardPDFFolderForPromotionCard = ConfigurationManager.AppSettings["ShardPDFFolderForPromotionCard"];
                    string PdfFilePath = Server.MapPath(strFileName);
                    System.IO.File.Copy(PdfFilePath, string.Format("{0}{1}.{2}", ShardPDFFolderForPromotionCard, PromotionCardPrintingID, "pdf"));
                }
            }

        }
    }
}