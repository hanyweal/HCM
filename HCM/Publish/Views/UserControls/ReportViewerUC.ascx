<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerUC.ascx.cs" Inherits="HCM.Views.UserControls.ReportViewerUC" %>


<asp:ScriptManager ID="ScriptManager" runat="server">
</asp:ScriptManager>
<rsweb:ReportViewer id="ReportViewerControl" runat="server" font-names="Verdana"
    font-size="8pt" interactivedeviceinfos="(Collection)" processingmode="Remote"
    waitmessagefont-names="Verdana" waitmessagefont-size="14pt" zoommode="Percent"
    zoompercent="100" height="600px" width="" pagecountmode="Actual" showparameterprompts="false">
    <ServerReport ReportPath="/HCM/"></ServerReport>
</rsweb:ReportViewer> 