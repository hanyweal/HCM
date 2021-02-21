using HCM.Classes.Helpers;
using System;

namespace HCM.WebForms.Reports
{
    public partial class FileDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileName = Request.QueryString["fileName"].ToString();
            if (!string.IsNullOrEmpty(fileName))
            {
                ExcelHelper.DownLoadExcel(fileName);
            }
        }
    }
}