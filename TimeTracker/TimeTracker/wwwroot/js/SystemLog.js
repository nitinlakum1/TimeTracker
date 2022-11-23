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
                    { "data": "description" },
                    { "data": "logType" },
                    {
                        "data": "logTime", "render": function (data) {
                            return setDateTimeFormat(data);
                        }
                    },
                    { "data": "id", "bSortable": false },
                ],
                columnDefs: [
                    {
                        targets: 2,
                        render: function (data, type, row) {
                            debugger
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
                                return '<a onclick="deleteUser(' + row.id + ')"><i class="fas fa-trash text-danger" style="cursor:pointer; width: 20px !important;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

function deleteUser(id) {
    $("#comformdelete").show();
    $("#deleteId").val(id);
}

function conformDelete() {
    var id = $("#deleteId").val();
    if (id > 0) {
        $.ajax({
            url: '/SystemLog/DeleteSystemLog/',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                setStatusMsg(result);
                Close();
                $('#tblUsers').DataTable().ajax.reload();
            },
            error: function (result) {
                alert("User not Delete!");
            },
        })
    }
}

function Close() {
    $("#deleteId").val(0);
    $("#comformdelete").hide();
}