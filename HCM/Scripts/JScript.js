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
    SetSelectedGreg();

    ShowTooltip();
    insertSelectionOptionToDDL();
    AllowNumericWithDecimal();
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

function AddCustomColumnFilter(dataTableName, func) {

    // Setup - add a text input to each footer cell
    $('#' + dataTableName + ' thead tr').clone(true).appendTo('#' + dataTableName + ' thead');

    $('#' + dataTableName + ' thead tr:eq(1) th.no-search').each(function (i) {
        $(this).html('');
    });

    $('#' + dataTableName + ' thead tr:eq(1) th.search').each(function (i) {
        var title = $(this).text();
        var id = $(this).attr('id');

        $(this).html('<input id="txtSearch' + id + '" type="text" class="form-control" placeholder="' + title + '" />');

        $('input', this).on('change', function () {
            if (typeof func !== 'undefined' && $.isFunction(window[func])) {
                window[func]();
            }
        });
    });
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
                console.log(d.data);
               VacationEndDate.val(d.data);
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
    $("#txtEmployeeNameEn").val('');
    $("#txtEmployeeIDNo").val('');
    $("#txtEmployeeJobNo").val('');
    $("#txtEmployeeJobName").val('');
    $("#txtEmployeeOrganizationName").val('');
    $("#txtEmployeeRankName").val('');
    $("#txtEmployeeCurrentQualification").val('');
    $("#hdnFldEmployeeGenderID").val('');
    $("#txtHiringDate").val('');
    $("#txtEmployeeBirthDate").val('');
    $("#txtCurrentJobJoinDate").val('');
    $("#txtEmployeeCurrentOrganization").val('');
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
    var d = curr_year + '-' + curr_month.toString() + '-' + curr_date;
    return d;
}

function RenderDate(data) {
    if (data == null)
        return null;

    moment.locale('en-UK');
    return moment(data).format("iYYYY-iMM-iDD");
}

function RenderDateF(data, formatD) {
    if (data == null)
        return null;

    moment.locale('en-UK');
    return moment(data).format(formatD || "iYYYY-iMM-iDD");
}

function isWeekEnd(data) {
    date = moment(data);
    return (date.isoWeekday() == 5 || date.isoWeekday() == 6) ? true : false;
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

function SetSelectedGreg() {
    var DateValueField = '.TxtGregPicker';
    jQuery(DateValueField).calendarsPicker({
        calendar: jQuery.calendars.instance('gregorian', 'en'),
        onSelect: customRange,
        commandsAsDateFormat: true,
        dateFormat: 'yyyy-mm-dd'
    });
    $(DateValueField).prop('readonly', true);
    $(DateValueField).css('background-color', 'inherit');
}

//function RenderDate(data) {
//    if (data == null)
//        return null;

//    moment.locale('en-UK');
//    return moment(data).format("iYYYY-iMM-iDD");
//}

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

function insertSelectionOptionToDDL(name) {
    //console.log('name', name);
    var newItem = $('<option />').val(0).html('-- أختر --');
    if (name) {
        $("#" + name).prepend(newItem);
        $("#" + name).val(0);
    }
    else {
        $(".addSelectOption").prepend(newItem);
        $(".addSelectOption").val(0);
    }
}

function insertSelectionOptionToDDL(name, value) {

    if (value == -1) value = '';

    var newItem = $('<option />').val(value).html('-- أختر --');
    if (name) {
        $("#" + name).prepend(newItem);
        $("#" + name).val(value);
    }
    else {
        $(".addSelectOption").prepend(newItem);
        $(".addSelectOption").val(value);
    }
}

function AllowNumericWithDecimal() {
    $(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });


    $(".allownumericwithoutdecimal").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

function AcceptArabicKeyPress(e) {
    var StrCheck = 'ابتثجحخدرزسشصضطظعغفقكلمنهويىةؤءئ ';
    var WhichCode = e.which ? e.which : e.keyCode;

    if (WhichCode == 13 || WhichCode == 8 || WhichCode == 9) {
        return true;
    }
    key = String.fromCharCode(WhichCode);
    if (StrCheck.indexOf(key) == -1)
        return false;

    return true;
}

function AcceptEnglishKeyPress(e) {
    var StrCheck = 'ابتثجحخدرزسشصضطظعغفقكلمنهويىةؤءئ ';
    var WhichCode = e.which ? e.which : e.keyCode;

    if (WhichCode == 13 || WhichCode == 8 || WhichCode == 9) {
        return true;
    }
    key = String.fromCharCode(WhichCode);
    if (StrCheck.indexOf(key) == -1)
        return false;

    return true;
}

function AcceptDigitalKeyPress(e) {
    var StrCheck = '1234567890';
    var WhichCode = e.which ? e.which : e.keyCode;

    if (WhichCode == 13 || WhichCode == 8 || WhichCode == 9) {
        return true;
    }
    key = String.fromCharCode(WhichCode);
    if (StrCheck.indexOf(key) == -1)
        return false;

    return true;
}

function AcceptArabicKeyPressWithSpicalCharacters(e) {

    var StrCheck = '!@#$%^&*()_-*/?\,.1234567890ابتثجحخدرزسشصضطظعغفقكلمنهويىةؤءئ ';
    var WhichCode = e.which ? e.which : e.keyCode;

    if (WhichCode == 13 || WhichCode == 8 || WhichCode == 9) {
        return true;
    }
    key = String.fromCharCode(WhichCode);
    if (StrCheck.indexOf(key) == -1)
        return false;

    return true;
}

function AcceptEnglishKeyPressWithSpicalCharacters(e) {

    var StrCheck = '!@#$%^&*()_-*/?\,.0123456789acbdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ';
    var WhichCode = e.which ? e.which : e.keyCode;

    if (WhichCode == 13 || WhichCode == 8 || WhichCode == 9) {
        return true;
    }
    key = String.fromCharCode(WhichCode);
    if (StrCheck.indexOf(key) == -1)
        return false;

    return true;
}

function ScrollToDiv(name) {
    $('html, body').animate({
        scrollTop: $('#' + name).offset().top
    }, 1000);
}