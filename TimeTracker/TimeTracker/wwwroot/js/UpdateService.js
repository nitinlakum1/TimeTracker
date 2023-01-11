function bindDataTable() {
    datatable = $('#tblUpdateService')
        .dataTable(
            {
                "sAjaxSource": "/UpdateService/LoadUpdateService",
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
                "aoColumns": [
                    { "data": "name" },
                    { "data": "version", width: 100 },
                    {
                        "data": "createdOn", width: 100, "render": function (data) {
                            return setDateTimeFormat(data, 'DD, MMM-yyyy');
                        },
                    },
                    { "data": "id", "bSortable": false, width: 70 },
                ],
                columnDefs: [
                    {
                        targets: 3,
                        render: function (data, type, row) {
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
                                return '<a onclick="deleteUpdateService(' + row.id + ')"><i class="fas fa-trash text-danger" style="cursor:pointer;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

function deleteUpdateService(id) {
    debugger
    $('#comformdelete').modal('show');
    $("#deleteId").val(id);
}

function conformDelete() {
    var id = $("#deleteId").val();
    if (id > 0) {
        $.ajax({
            url: '/UpdateService/deleteUpdateService/',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                setStatusMsg(result);
                Close();
                $('#tblUpdateService').DataTable().ajax.reload();
            },
            error: function (result) {
                alert("Update Service not Delete!");
            },
        })
    }
}

function Close() {
    $("#deleteId").val(0);
    $("#comformdelete").modal('hide');
}