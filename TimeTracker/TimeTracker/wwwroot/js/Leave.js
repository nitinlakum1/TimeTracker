function bindDataTable() {
    datatable = $('#tblLeave')
        .dataTable(
            {
                "sAjaxSource": "/Leave/LoadLeave",
                "bServerSide": true,
                "bProcessing": true,
                "bSearchable": true,
                "scrollX": true,
                "order": [[0, "DESC"]],
                "ordering": false,
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
                        })
                    });
                    perm = aoData;
                },
                "aoColumns": [
                    { "data": "username" },
                    {
                        "data": "leaveFromDate", "render": function (data) {
                            return setDateTimeFormat(data, 'DD, MMM-yyyy');
                        },
                    },
                    {
                        "data": "leaveToDate", "render": function (data) {
                            return setDateTimeFormat(data, 'DD, MMM-yyyy');
                        },
                    },
                    {
                        "data": "applyDate", "render": function (data) {
                            return setDateTimeFormat(data, 'DD, MMM-yyyy');
                        },
                    },
                    { "data": "leaveStatusName" },
                    {
                        "data": "isPaid", width: 65, "render": function (isPaid) {
                            return (isPaid == true ? 'Yes' : 'No')
                        },
                    },
                    { "data": "id", width: 70 },
                ],

                columnDefs: [
                    {
                        targets: 6,
                        render: function (data, type, row) {
                            var roleId = $("#roleId").text();
                            var style = row.leaveStatusName == "Apply" ? 'cursor:pointer;' : 'cursor:no-drop; color:grey !important;';
                            var leaveId = row.leaveStatusName == "Apply" ? `onclick="statusChange('${leaveId}')"` : '';

                            var btn1 = `<a ${leaveId} style="margin-right: 10px; ${style}" class="justify-content-center"><i class="fas fa-edit text-success" style="${style}"></i></a>`;

                            // Admin = 1
                            return (roleId == 1 ? btn1 : '') + `<a onclick="openLeaveModel('${row.id}')"><i class="fa-solid fa-eye font-color"  style="cursor:pointer; font-size: 16px;"></i></a>`;
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmbUser').change(function () {
    $('#tblLeave').DataTable().draw();
});

function statusChange(id) {
    $("#comformChange").modal('show');
    $("#statusId").val(id);
}

$('#btnApproved , #btnDeclined').click(function () {
    debugger
    var btnId = $(this).val();
    var id = $("#statusId").val();
    if (id > 0) {
        $.ajax({
            url: '/Leave/ChangeStatus/',
            type: 'POST',
            data: { id: id, btnId: btnId },
            success: function (result) {
                setStatusMsg(result);
                $("#comformChange").modal('hide');
                $('#tblLeave').DataTable().ajax.reload();
            },
            error: function (result) {
                alert("Status not Change!");
            },
        })
    }
});

function openLeaveModel(id) {
    if (id > 0) {
        $.ajax({
            url: '/Leave/GetLeaveDetail/',
            type: 'GET',
            data: { id: id },
            success: function (result) {
                $('#loadLeaveDetail').html(result);
                $('#modalLeaveDetail').modal('show');
            },
            error: function (result) {
                alert("Leave Detail not Found!");
            },
        })
    }
}