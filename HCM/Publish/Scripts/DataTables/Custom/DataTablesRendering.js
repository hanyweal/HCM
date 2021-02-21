function GetRowsCount(tblID) {
    var table = $(tblID).DataTable();
    return table.data().count();
}

var RenderCheckBox = function (data) {
    var IsChecked = data == true ? "checked" : "";
    return '<input type="checkbox" disabled="disabled" class="checkbox" ' +
        IsChecked + ' />';
}

var RenderDateTime = function (data) {
    if (data == null)
        return null;

    //moment.locale('ar-sa');
    return moment(data).format("YYYY-MM-DD h:mm:ss a");

    //var r = /\/Date\(([0-9]+)\)\//gi
    //var matches = data.match(r);
    //if (matches == null)
    //    return '1/1/1950';
    //var result = matches.toString().substring(6, 19);
    //var epochMilliseconds = result.replace(
    ///^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
    //'$1');
    //var b = new Date(parseInt(epochMilliseconds));
    //var c = new Date(b.toString());
    //var curr_date = c.getDate();
    //var curr_month = c.getMonth() + 1;
    //var curr_year = c.getFullYear();
    //var curr_h = c.getHours();
    //var curr_m = c.getMinutes();
    //var curr_s = c.getSeconds();
    //var curr_offset = c.getTimezoneOffset() / 60
    //var d = curr_month.toString() + '/' + curr_date + '/' + curr_year + " " + curr_h + ':' + curr_m + ':' + curr_s;
    //return d;
}

var RenderDate = function (data) {
    if (data == null)
        return null;

    moment.locale('en-UK');
    return moment(data).format("iYYYY-iMM-iDD");
    //return data;
    //var r = /\/Date\(([0-9]+)\)\//gi
    //var matches = data.match(r);
    //if (matches == null)
    //    return '1/1/1950';
    //var result = matches.toString().substring(6, 19);
    //var epochMilliseconds = result.replace(
    ///^\/Date\(([0-9]+)([+-][0-9]{4})?\)\/$/,
    //'$1');
    //var b = new Date(parseInt(epochMilliseconds));
    //var c = new Date(b.toString());
    //var curr_date = c.getDate();
    //var curr_month = c.getMonth() + 1;
    //var curr_year = c.getFullYear(); 
    //var curr_offset = c.getTimezoneOffset() / 60
    //var d = curr_month.toString() + '/' + curr_date + '/' + curr_year;
    //return d;
}

var RenderHyperLink = function (data, type, row) {
    //Link = "Users/Create/";
    //return '<a href="'+ Link + data + '">تعديل</a>'
    return '<a href="#"><img src="/Content/Images/Edit.gif" alt="bottle" class="thumbnails" /></a>'
}

$(document).ready(function () {
    //default settings to all jquery datatable
    $.extend(true, $.fn.dataTable.defaults, {
        //"searching": false,
        //"ordering": false,
        "initComplete": function (settings, json) {
            ShowTooltip();
        },
        "drawCallback": function (settings, json) {
            ShowTooltip();
        },
        "lengthChange": false,
        "language": {
            "url": "/Scripts/DataTables/Custom/JqueryDataTablesLang.json",
        }
        //,"ajax": {
        //    global: false // set to false to prevent the global handlers like [ajaxStart, ajaxStop] from being triggered.       
        //}
        //"pagingType": "full_numbers"
    });
});