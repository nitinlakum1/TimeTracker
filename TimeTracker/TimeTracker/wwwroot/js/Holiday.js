function bindDataTable() {
    datatable = $('#tblHoliday')
        .dataTable(
            {
                "sAjaxSource": "/Holiday/LoadHoliday",
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
                    { "data": "name" },
                    {
                        "data": "date", width: 140, "render": function (data) {
                            return setDateFormat(data);
                        },
                        },
                    { "data": "id", "bSortable": false, width: 140},
                ],
                columnDefs: [
                    {
                        targets: 2,
                        render: function (data, type, row) {
                            var roleId = $("#roleId").text();
                            if (roleId == 1) { // Admin = 1 , Employee = 2
                                return '<a href="/holiday/update/' + row.id + '" style="margin-right: 10px;" class="justify-content-center"><i class="fas fa-edit text-success"></i></a><a onclick="deleteUser(' + row.id + ')"><i class="fas fa-trash text-danger" style="cursor:pointer; margin-left:8px;"></i></a>';
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

function deleteUser(id) {
    $("#comformdelete").modal('show');
    $("#deleteId").val(id);
}

function conformDelete() {
    
    var id = $("#deleteId").val();
    if (id > 0) {
        $.ajax({
            url: '/Holiday/DeleteHoliday/',
            type: 'POST',
            data: { id: id },
            success: function (result) {
                setStatusMsg(result);
                Close();
                $('#tblHoliday').DataTable().ajax.reload();
            },
            error: function (result) {
                alert("User not Delete!");
            },
        })
    }
}

function Close() {
    $("#deleteId").val(0);
    $("#comformdelete").modal('hide');
}