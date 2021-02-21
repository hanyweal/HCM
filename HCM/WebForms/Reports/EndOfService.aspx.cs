using Microsoft.Reporting.WebForms;
using System;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class EndOfService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int EndOfServiceID = 0;
            ReportParameter RpEndOfServiceID = new ReportParameter("EndOfServiceID");

            if (Request.QueryString["EndOfServiceID"] != "")
            {
                EndOfServiceID = int.Parse(Request.QueryString["EndOfServiceID"].ToString());
                RpEndOfServiceID.Values.Add(EndOfServiceID.ToString());
            }

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "EndOfService";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, RpEndOfServiceID);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}