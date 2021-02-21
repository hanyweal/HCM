using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.IO;
using System.Web;


public class ReportServicesMethod
{
    private String GetReportServerUrl()
    {
        //return "http://hany/ReportServer" ;
        //return "http://172.16.0.13/ReportServer";
        return ConfigurationManager.AppSettings["SSRSServer"].ToString();
    }

    private String GetProjectNameInSSRSServer()
    {
        return ConfigurationManager.AppSettings["ProjectNameInSSRSServer"].ToString();
    }

    private String GetPDFFolder()
    {
        return ConfigurationManager.AppSettings["PDFFolder"].ToString();
    }

    private void ReportProperties(ReportViewer RV, String ReportName)
    {
        RV.ProcessingMode = ProcessingMode.Remote;
        RV.ServerReport.ReportServerUrl = new System.Uri(GetReportServerUrl());
        RV.ServerReport.ReportPath = GetProjectNameInSSRSServer() + ReportName;
        RV.ShowParameterPrompts = false;
        RV.ShowPromptAreaButton = false;
    }

    private void ReportProperties(ReportViewer RV, String ReportName, ReportParameter[] parm)
    {
        RV.ProcessingMode = ProcessingMode.Remote;
        RV.ServerReport.ReportServerUrl = new System.Uri(GetReportServerUrl());
        RV.ServerReport.ReportPath = GetProjectNameInSSRSServer() + ReportName;
        RV.ServerReport.SetParameters(parm);
        RV.ShowParameterPrompts = false;
        RV.ShowPromptAreaButton = false;
    }

    public string ReportCalling(ReportViewer RV, String ReportName, params ReportParameter[] parm)
    {
        RV.ProcessingMode = ProcessingMode.Remote;
        RV.ShowCredentialPrompts = false;
        RV.ServerReport.ReportServerUrl = new System.Uri(GetReportServerUrl());
        RV.ServerReport.ReportPath = GetProjectNameInSSRSServer() + ReportName;
        RV.ServerReport.SetParameters(parm);
        RV.ShowParameterPrompts = false;
        RV.ShowPromptAreaButton = false;

        return ExportToPDF(RV, ReportName);
    }

    public void ReportCalling(ReportViewer RV, String GetProjectNameInSSRSServer, String ReportName, params ReportParameter[] parm)
    {
        RV.ProcessingMode = ProcessingMode.Remote;
        RV.ShowCredentialPrompts = false;
        RV.ServerReport.ReportServerUrl = new System.Uri(GetReportServerUrl());
        RV.ServerReport.ReportPath = GetProjectNameInSSRSServer + ReportName;
        RV.ServerReport.SetParameters(parm);
        RV.ShowParameterPrompts = false;
        RV.ShowPromptAreaButton = false;
    }

    private string ExportToPDF(ReportViewer RV, String ReportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;

        ReportParameterInfoCollection pInfo = RV.ServerReport.GetParameters();
        string filenameParams = "";

        byte[] bytes;
        bytes = RV.ServerReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

        filenameParams = ReportName;
        string filename = Path.Combine(GetPDFFolder(), filenameParams + Guid.NewGuid() + ".pdf");
        using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(filename), FileMode.Create))
        {
            fs.Write(bytes, 0, bytes.Length);
        }

        return filename;
    }
}
