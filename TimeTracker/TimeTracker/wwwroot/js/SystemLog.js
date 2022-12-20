﻿function bindDataTable() {
    datatable = $('#tblSystemLogs')
        .dataTable(
            {
                "sAjaxSource": "/SystemLog/GetSystemLog",
                "bServerSide": true,
                "bProcessing": true,
                "bSearchable": true,
                "scrollX": true,
                "order": [[0, "DESC"]],
                "paging": true,
                "searching": true,
                "bAutoWidth": false,
                "language": {
                    "emptyTable": "No record found.",
                    "processing":
                        '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
                },
                "fnServerParams": function (aoData) {
                    aoData.push({
                        "name": "filter", "value": JSON.stringify({
                            UserId: $('#cmbUser').val(),
                            FromDate: $('#txtDate').val(),
                            ToDate: $('#txtDate').val(),
                        })
                    });
                    perm = aoData;
                },
                "aoColumns": [
                    { "data": "username", width: 150 },
                    { "data": "logTypeName", width: 130 },
                    { "data": "description" },
                    {
                        "data": "logTime", width: 140, "render": function (data) {
                            return setDateTimeFormat(data);
                        },
                    },
                    { "data": "id", "bSortable": false, width: 70 },
                ],
                columnDefs: [
                    {
                        targets: 4,
                        render: function (data, type, row) {
                            var roleId = $("#roleId").text();
                            if (roleId == 1) {  // Admin = 1 , Employee = 2
                                return '<a onclick="deleteLog(' + row.id + ')"><i class="fas fa-trash text-danger" style="cursor:pointer;"></i></a>';
                            } else {
                                return 'N/A';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmbUser').change(function () {
    $('#tblSystemLogs').DataTable().draw();
});

$('#txtDate').change(function () {
    $('#tblSystemLogs').DataTable().draw();
});

function startTimeLog() {
    var lastTime = $('#spnLastTime').text();
    var hour = parseInt(lastTime.split(':')[0]);
    var min = parseInt(lastTime.split(':')[1]);
    var second = parseInt(lastTime.split(':')[2]);
    if (lastTime != "00:00:00") {
        setInterval(function () {
            second++;
            if (second == 60) {
                min++;
            }
            second = second == 60 ? 0 : second;
            if (min == 60) {
                hour++;
            }
            min = min == 60 ? 0 : min;

            var h = hour.toString().padStart(2, "0");
            var m = min.toString().padStart(2, "0");
            var s = second.toString().padStart(2, "0");
            $('#spnLastTime').text(`${h}:${m}:${s}`);
        }, 1000)
    }
}

/*
 * Monthly report
 */
var totalMonthTime = "";
var totalHours = 0;
var totalMinutes = 0;
function bindMRDataTable() {
    datatable = $('#tblMonthlyReport')
        .dataTable(
            {
                "sAjaxSource": "/SystemLog/GetMonthlyReport",
                "bServerSide": true,
                "bProcessing": true,
                "bSearchable": true,
                "scrollX": true,
                "order": [[0, "DESC"]],
                "paging": false,
                "searching": false,
                "bSortable": false,
                "bAutoWidth": false,
                "bInfo": false,
                "language": {
                    "emptyTable": "No record found.",
                    "processing":
                        '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
                },
                "fnServerParams": function (aoData) {
                    aoData.push({
                        "name": "filter", "value": JSON.stringify({
                            UserId: $('#cmbUserMonth').val(),
                            FromDate: $('#txtDateMonth').val(),
                            ToDate: $('#txtDateMonth').val(),
                        })
                    });
                    perm = aoData;
                },
                "aoColumns": [
                    {
                        "data": "date", width: 170, "bSortable": false, "render": function (data) {
                            return setDateDayFormat(data);
                        }
                    },
                    { "data": "username", "bSortable": false, width: 170 },
                    {
                        "data": "startingTime", width: 170, "bSortable": false, "render": function (data) {
                            return setDateTimeFormat(data);
                        },
                    },
                    {
                        "data": "closingTime", width: 170, "bSortable": false, "render": function (data) {
                            return setDateTimeFormat(data);
                        },
                    },
                    { "data": "totalTime", width: 100, "bSortable": false },
                ],
                "drawCallback": function (settings) {
                    $('#spnTotalMonthTime').text(totalMonthTime);
                },
                columnDefs: [
                    {
                        targets: 4,
                        render: function (data, type, row) {
                            if (row.totalTime == '00:00') {
                                return '<span class="new-btn-danger" style="padding: 3px 20px 4px 20px; border-radius:2px;">00:00</span>';
                            } else {
                                totalHours += parseInt(row.totalTime.split(':')[0]);
                                totalMinutes += parseInt(row.totalTime.split(':')[1]);
                                if (totalMinutes > 59) {
                                    totalHours++;
                                    totalMinutes = totalMinutes - 60;
                                }
                                var Hours = ("0" + totalHours).slice(-2);
                                var Minutes = ("0" + totalMinutes).slice(-2);
                                totalMonthTime = `${Hours}:${Minutes}`;
                                return row.totalTime;
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmbUserMonth').change(function () {
    totalHours = 0;
    totalMinutes = 0;
    $('#tblMonthlyReport').DataTable().clear().draw();
});

$('#txtDateMonth').change(function () {
    totalHours = 0;
    totalMinutes = 0;
    $('#tblMonthlyReport').DataTable().clear().draw();
});

function deleteLog(id) {
    debugger
    $('#comformdelete').modal('show');
    $("#deleteId").val(id);
}

function conformDelete() {
    var id = $("#deleteId").val();
    if (id > 0) {
        $.ajax({
            url: '/SystemLog/DeleteLog/',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                setStatusMsg(result);
                Close();
                $('#tblSystemLogs').DataTable().ajax.reload();
            },
            error: function (result) {
                alert("Log not Delete!");
            },
        })
    }
}

function Close() {
    $("#deleteId").val(0);
    $("#comformdelete").modal('hide');
}