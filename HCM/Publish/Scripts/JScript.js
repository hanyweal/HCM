$(document).ready(function () {

    $(document).on('ajaxStart', function () {
        $("#divLoader").show();
    }).on('ajaxStop', function () {
        $("#divLoader").hide();
    }).on('ajaxError', function () {
        $("#divLoader").hide();
    });
    SelectNodeByControllerName();

    SetSelectedHijri();

    ShowTooltip();
});

// For DataTable - to Remove Extra Columns From URL when use "serverSide": true,
function RemoveExtraColumnsFromURL(data) {
    for (var i = 0, len = data.columns.length; i < len; i++) {
        if (!data.columns[i].search.value) delete data.columns[i].search;
        if (data.columns[i].searchable === true) delete data.columns[i].searchable;
        if (data.columns[i].orderable === true) delete data.columns[i].orderable;
        if (data.columns[i].data === data.columns[i].name) delete data.columns[i].name;
    }
    delete data.search.regex;
}

function GetVacationEndDate(txtVacationStartDate, txtVacationPeriod, txtVacationEndDateOut, txtWorkDateOut) {
    var VacationStartDate = $(txtVacationStartDate);
    var VacationPeriod = $(txtVacationPeriod);
    var VacationEndDate = $(txtVacationEndDateOut);
    var WorkDate = $(txtWorkDateOut);

    if (VacationStartDate.val() != '' && VacationPeriod.val() != '') {
        $.ajax({
            type: "POST"
           , url: "/Vacations/GetVacationEndDate/" + VacationPeriod.val() + "/" + VacationStartDate.val()
           , success: function (d) {
               VacationEndDate.val(d.data);

               // get GetWorkDateAfterVacation
               $.ajax({
                   type: "POST"
            , url: "/Vacations/GetWorkDateAfterVacation/" + VacationEndDate.val()
            , success: function (d) {
                WorkDate.val(d.data);
            }
               , error: function (xhr, status, error) {
                   //AddingFailure();
               }
               });

           }
                , error: function (xhr, status, error) {
                    //AddingFailure();
                }
        });
    }
    else
        VacationEndDate.val('');
}

function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}

function ResetEmployeeData() {
    $("#hdnFldEmployeeCodeID").val('');
    $("#txtEmployeeCodeNo").val('');
    $("#txtEmployeeNameAr").val('');
    $("#txtEmployeeIDNo").val('');
    $("#txtEmployeeJobNo").val('');
    $("#txtEmployeeJobName").val('');
    $("#txtEmployeeOrganizationName").val('');
    $("#txtEmployeeRankName").val('');
    $("#txtEmployeeCurrentQualification").val('');
}

function formatDate(data) {
    if (data == null)
        return null;
    var r = /\/Date\(([0-9]+)\)\//gi
    var matches = data.match(r);
    if (matches == null)
        return '1/1/1950';
    var result = matches.toString().substring(6, 19);
    var epochMilliseconds = result.replace(
    /^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
    '$1');
    var b = new Date(parseInt(epochMilliseconds));
    var c = new Date(b.toString());
    var curr_date = c.getDate();
    var curr_month = c.getMonth() + 1;
    var curr_year = c.getFullYear();
    var curr_h = c.getHours();
    var curr_m = c.getMinutes();
    var curr_s = c.getSeconds();
    var curr_offset = c.getTimezoneOffset() / 60
    var d = curr_month.toString() + '/' + curr_date + '/' + curr_year;
    return d;
}

function SuccessAlert(Msg) {
    $.toast({
        text: Msg,
        heading: 'رسالة تأكيد',
        icon: 'success',
        position: 'mid-center',
        textAlign: 'center',
        sticky: false
        //stack: false
    });
}

function FailureAlert(Msg) {
    $.toast({
        text: Msg,
        heading: 'رسالة خطأ',
        icon: 'error',
        position: 'mid-center',
        // stack: false,
        sticky: false
    });
}

function WarningAlert(Msg) {
    $.toast({
        text: Msg,
        heading: 'رسالة تنبيه',
        icon: 'warning',
        position: 'mid-center',
        sticky: false
    });
}

function GetEmployeeData(EmployeeCodeID) {
    $.ajax({
        type: "POST"
        , dataType: "json"
        , contentType: "application/json; charset=utf-8"
        , url: "/Employees/GetByEmployeeCodeID"
        , data: "{ 'id' : '" + EmployeeCodeID + "'}"
        , success: function (d) {
            //alert(d.data.EmployeeCurrentJob.EmployeeCareerHistoryID);
            //debugger;
            DestroyModal("#divEmployeesModal");
            $("#hdnFldEmployeeCodeID").val(d.data.EmployeeCodeID);
            $("#txtEmployeeCodeNo").val(d.data.EmployeeCodeNo);
            $("#txtEmployeeNameAr").val(d.data.Employee.EmployeeNameAr);
            $("#txtEmployeeIDNo").val(d.data.Employee.EmployeeIDNo);
            $("#txtEmployeeJobNo").val(d.data.EmployeeCurrentJob != null ? d.data.EmployeeCurrentJob.OrganizationJob.JobNo : null);
            $("#txtEmployeeJobName").val(d.data.EmployeeCurrentJob != null ? d.data.EmployeeCurrentJob.OrganizationJob.Job.JobName : null);
            $("#txtEmployeeOrganizationName").val(d.data.EmployeeCurrentJob != null ? d.data.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName : null);
            $("#txtEmployeeRankName").val(d.data.EmployeeCurrentJob != null ? d.data.EmployeeCurrentJob.OrganizationJob.Rank.RankName : null);
            $("#hdnFldEmployeeCareerHistoryID").val(d.data.EmployeeCurrentJob != null ? d.data.EmployeeCurrentJob.EmployeeCareerHistoryID : null);
            
        }
        , error: function (xhr, status, error) {
            alert(error.message);
        }
    });
}

function SetSelectedHijri() {
    var DateValueField = '.TxtHijriPicker';
    jQuery(DateValueField).calendarsPicker({
        calendar: jQuery.calendars.instance('ummalqura', 'ar'),
        onSelect: customRange,
        commandsAsDateFormat: true,
        dateFormat: 'yyyy-mm-dd'
    });
    $(DateValueField).prop('readonly', true);
    $(DateValueField).css('background-color', 'inherit');
}

function RenderDate(data) {
    if (data == null)
        return null;

    moment.locale('en-UK');
    return moment(data).format("iYYYY-iMM-iDD");
}

function customRange(dates) {

}

function ShowModal(ModelID) {
    $(ModelID).modal({
        cache: false,
        show: true,
        Keyboard: true,
        backdrop: "static"
    });
}

function ShowTooltip() {
    $('[title]').each(function () {
        $(this).tooltip();
    })
}

function DestroyModal(ModelID) {
    $(ModelID).modal('hide');
    $(ModelID).on('hidden.bs.modal', function () {
        $(this).data('bs.modal', null);
        DataTableFilterClear();
    });
}

function SelectNodeByControllerName() {
    var url = window.location.href;
    var urlSplited = url.split('/');
    var ControllerName = urlSplited[3];
    if (urlSplited[4] && urlSplited[4].toLowerCase() != "create" && urlSplited[4].toLowerCase() != "edit" && urlSplited[4].toLowerCase() != "details")
        ControllerName += "/" + urlSplited[4];
    var $tree = $('#tree');
    $tree.treeview('expandAll');
    var nodeIdStr = $('li a[href^="/' + ControllerName + '"]').parent().attr('data-nodeid');
    $tree.treeview('collapseAll');

    if (nodeIdStr === undefined || nodeIdStr === "")
        return;
    var nodeId = parseInt(nodeIdStr);
    $tree.treeview('selectNode', nodeId);
    ExpandParent('tree', $tree.treeview('getNode', nodeId));
}

function ExpandParent(treeName, node) {

    var $tree = $('#' + treeName);
    if (node.parentId === undefined) return;
    var parentId = node.parentId;
    $tree.treeview('expandNode', parentId);
    var parentNode = $tree.treeview('getNode', parentId);
    if (parentNode)
        ExpandParent(treeName, parentNode);
}

function DataTableFilterClear() {
    var tables = $('.dataTable').DataTable();
    for (var i = 0; i < tables.context.length; i++) {
        tables.table(i).search('').columns().search('').draw();
    }
}




//function GetEndDate(StartDate, Period) {
//    var StartDateValue = new Date($("#" + StartDate).val());
//    var PeriodValue = parseInt("0" + $("#" + Period).val(), 10) - 1;
//    var EndDateValue;

//    //console.log(StartDateValue);
//    ////console.log(PeriodValue);
//    //moment.locale('en-UK');
//    //console.log(moment(StartDateValue).format('idd/iMM/iYYYY'));

//    if (!isNaN(StartDateValue.getTime())) {
//        StartDateValue.setDate(StartDateValue.getDate() + PeriodValue);
//        EndDateValue = $.datepicker.formatDate('yy/mm/dd', StartDateValue);
//        return EndDateValue;
//    }
//    else {
//        return "Error";
//    }
//}






