function bindDataTable() {
    datatable = $('#tblUsers')
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
                },
                "aoColumns": [
                    { "data": "username", width: 150 },
                    { "data": "logTypeName", width: 130 },
                    { "data": "description" },
                    {
                        "data": "logTime", width: 140, "render": function (data) {
                            return setDateTimeFormat(data);
                        }
                    },
                    { "data": "id", "bSortable": false, width: 70 },
                ],
                processing: true,
            });
}

function startTimeLog() {
    var lastTime = $('#spnLastTime').text();
    var hour = parseInt(lastTime.split(':')[0]);
    var min = parseInt(lastTime.split(':')[1]);
    var second = parseInt(lastTime.split(':')[2]);

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