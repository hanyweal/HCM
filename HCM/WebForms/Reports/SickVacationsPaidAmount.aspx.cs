using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class SickVacationsPaidAmount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "SickVacationsPaidAmount";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
