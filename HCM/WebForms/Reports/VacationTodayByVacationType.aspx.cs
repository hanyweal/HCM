using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HCM.WebForms.Reports
{
    public partial class VacationsTodayByVacationType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportParameter VacationTypeID = new ReportParameter("VacationTypeID", Request.QueryString["VacationTypeID"] != "" ? Request.QueryString["VacationTypeID"].ToString() : null);
            ReportParameter StartDate = new ReportParameter("StartDate", Request.QueryString["StartDate"].ToString());
            ReportParameter EndDate = new ReportParameter("EndDate", Request.QueryString["EndDate"].ToString());

            string ReportName = string.Empty;
            string strFileName = string.Empty;
            ReportServicesMethod RSM = new ReportServicesMethod();
            if (!Page.IsPostBack)
            {
                ReportName = "VacationsTodayByVacationType";
                strFileName = RSM.ReportCalling(ReportViewerControl.RViewer, ReportName, VacationTypeID, StartDate, EndDate);
                if (!string.IsNullOrEmpty(strFileName))
                    Response.Redirect(strFileName);
            }

        }
    }
}
