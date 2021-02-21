using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HCM.WebForms.Reports
{
    public partial class ReportOrphanAssignments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "AssigningOrphanRecords";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
