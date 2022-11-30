function bindDataTable() {
    datatable = $('#tblUsers')
        .dataTable(
            {
                "sAjaxSource": "/Salary/LoadSalary",
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
                    { "data": "salary" },
                    { "data": "fromDate" },
                    { "data": "toDate" },
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
    $("#comformdelete").hide();
}