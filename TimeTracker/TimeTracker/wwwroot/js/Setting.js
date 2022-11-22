function bindDataTable() {
    datatable = $('#tblUsers')
        .dataTable(
            {
                "sAjaxSource": "/Setting/SettingList",
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
                    { "data": "displayName" },
                    { "data": "key" },
                    { "data": "value" },
                    { "data": "isActive", "bSortable": false, width: 80 },
                    { "data": "id", "bSortable": false, width: 70 },
                ],
                columnDefs: [
                    {
                        targets: 3,
                        render: function (data, type, row) {
                            if (row.isActive == '1') {
                                return '<button type="button" class="btn new-btn-success btn-sm" style="padding: 0px 6px 0 6px;width: 62px;">Active</button>';
                            } else {
                                return '<button type="button" class="btn new-btn-danger btn-sm" style="padding: 0px 6px 0 6px;width: 62px;">Inactive</button>';
                            }
                        },
                        className: "text-center",
                    },
                    {
                        targets: 4,
                        render: function (data, type, row) {
                            return '<a href="/setting/update/' + row.id + '" style="margin-right: 10px;" class="justify-content-center"><i class="fas fa-edit text-success"></i></a>';
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
            url: '/Setting/DeleteSetting/',
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