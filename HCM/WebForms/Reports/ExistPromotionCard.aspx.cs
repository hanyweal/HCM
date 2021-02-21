using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Web.UI;

namespace HCM.WebForms.Reports
{
    public partial class ExistPromotionCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string PromotionCardPrintingID = Request.QueryString["PromotionCardPrintingID"].ToString();
       
            if (!Page.IsPostBack)
            {
                string PDFFolder = ConfigurationManager.AppSettings["PDFFolder"];
                string ShardPDFFolderForPromotionCard = ConfigurationManager.AppSettings["ShardPDFFolderForPromotionCard"];
 
                string PdfFilePath = Server.MapPath(String.Format("{0}{1}.{2}", PDFFolder, PromotionCardPrintingID, "pdf"));
                if(System.IO.File.Exists(PdfFilePath))
                {
                    System.IO.File.Delete(PdfFilePath);
                }
                System.IO.File.Copy( string.Format("{0}{1}.{2}", ShardPDFFolderForPromotionCard, PromotionCardPrintingID, "pdf"), PdfFilePath);
                Response.Redirect(string.Format("{0}{1}.{2}", PDFFolder, PromotionCardPrintingID, "pdf"));
            }

        }
    }
}