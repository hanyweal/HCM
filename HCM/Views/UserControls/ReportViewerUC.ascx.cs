using Microsoft.Reporting.WebForms;

namespace HCM.Views.UserControls
{
    public partial class ReportViewerUC : System.Web.UI.UserControl
    {
        public ReportViewer RViewer
        {
            get
            {
                return ReportViewerControl;
            }
        }

        public bool ShowParameterPrompts
        {
            get
            {
                return ReportViewerControl.ShowParameterPrompts;
            }
            set
            {
                ReportViewerControl.ShowParameterPrompts = value;
            }
        }
    }
}