function bindDataTable() {
    datatable = $('#tblUsers')
        .dataTable(
            {
                "sAjaxSource": "/User/LoadUser",
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
                    { "data": "username" },
                    { "data": "fullName" },
                    { "data": "email" },
                    { "data": "contactNo", "render": $.fn.dataTable.render.number('', '', '', '+91 ') },
                    { "data": "id", "bSortable": false },
                ],
                columnDefs: [
                    {
                        targets: 4,
                        render: function (data, type, row) {
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
                                return '<a href="/user/update/' + row.id + '" style="margin-right: 10px;" class="justify-content-center"><i class="fas fa-edit text-success"></i></a><a onclick="deleteUser(' + row.id + ')"><i class="fas fa-trash text-danger" style="cursor:pointer;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

function deleteUser(id) {
    debugger
    $('#comformdelete').modal('show');
    $("#deleteId").val(id);
}

function conformDelete() {
    var id = $("#deleteId").val();
    if (id > 0) {
        $.ajax({
            url: '/User/DeleteUser/',
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
    $("#comformdelete").modal('hide');
}

function deleteProfilePic() {
    var id = $("#deleteProfileId").val();
    if (id > 0) {
        $.ajax({
            url: '/User/DeleteProfilePic/',
            type: 'POST',
            data: {
                id: id,
                url: $("#deleteProfileUrl").val(),
            },
            success: function (result) {
                setStatusMsg(result);
                Close();
            },
            error: function (result) {
                alert("Profile not Delete!");
            },
        })
    }
}

$('#btnUpdateTimeTracker').click(function () {
    $.ajax({
        url: '/User/CheckUpdate/',
        type: 'POST',
        success: function (result) {
            setStatusMsg(result);
        },
        error: function (result) {
            alert("Profile not Delete!");
        },
    })
});