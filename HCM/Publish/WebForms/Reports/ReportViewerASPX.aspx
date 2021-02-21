<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerASPX.aspx.cs" Inherits="HCM.ReportViewerASPX" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/Global.css" rel="stylesheet" />

    <%--<script type="text/javascript">
        $(document).ready(function () {

            if (true){  //($.browser.chrome) {
                try {
                    var ControlName = 'ReportViewerControl_ReportViewerControl';
                    var innerTbody = '<tbody><tr><td><input type="image" style="border-width: 0px; padding: 2px; height: 16px; width: 16px;" alt="Print" src="/Reserved.ReportViewerWebControl.axd?OpType=Resource&amp;Version=9.0.30729.1&amp;Name=Microsoft.Reporting.WebForms.Icons.Print.gif" title="Print"></td></tr></tbody>';
                    var innerTable = '<table title="Print" onclick="PrintFunc(\'' + ControlName + '\'); return false;" id="' + ControlName + '_print" style="border: 1px solid rgb(236, 233, 216); background-color: rgb(236, 233, 216); cursor: default;">'  + innerTbody + '</table>'
                    var outerDiv = '<div style="display: inline; font-size: 8pt; height: 30px;" class=" "><table cellspacing="0" cellpadding="0" style="display: inline;"><tbody><tr><td height="28px">' + innerTable + '</td></tr></tbody></table></div>';

                    //alert($("#" + ControlName));

                    $("#" + ControlName + "_ctl05" + " > div").append(outerDiv);

                }
                catch (e) { alert(e); }
            }
        });

        function PrintFunc(ControlName) {
            //setTimeout('ReportFrame' + ControlName + '.print();', 100);
            
            var strFrameName = ("printer-" + (new Date().getTime()));
            var jFrame = $("<iframe name='" + strFrameName + "'>");
            jFrame
            .css("width", "1000px")
            .css("height", "1000px")
            //.css("position", "absolute")
            //.css("left", "-2000")
            .appendTo($("body:first"));

             

            var objFrame = window.frames[strFrameName];
            var objDoc = objFrame.document;
            var jStyleDiv = $("<div>").append($("style").clone());

            //console.log('jStyleDiv', jStyleDiv);
             

            objDoc.open();
            objDoc.write($("head").html());

            alert('4');

            console.log('aaa ', $("head").html())

            objDoc.write($("#VisibleReportContentReportViewerControl_ReportViewerControl_ctl09").html());
            objDoc.close();
            objFrame.print();

            setTimeout(function () { jFrame.remove(); }, (60*1000));
        }
    </script>--%>

    <%-- <script type="text/javascript">
        <%:System.Web.Optimization.Styles.Render("~/Content/css")%>
        <%:System.Web.Optimization.Scripts.Render("~/bundles/jquery")%>
        <%:System.Web.Optimization.Scripts.Render("~/bundles/bootstrap")%>
        <%:System.Web.Optimization.Scripts.Render("~/bundles/modernizr")%>
        <%:System.Web.Optimization.Scripts.Render("~/bundles/javascript")%>
        <%:System.Web.Optimization.Scripts.Render("~/bundles/jqueryval")%>
    </script>--%>
</head>
<body>
  <%--  <div id="divLoader" class="overlay" style="display: none; text-align: center;">
        <span class="glyphicon glyphicon-refresh glyphicon-refresh-animate loader"></span>
    </div>--%>
    <form id="form1" runat="server">
        <uc:ReportViewer ID="ReportViewerControl" runat="server" />
    </form>
</body>
</html>

