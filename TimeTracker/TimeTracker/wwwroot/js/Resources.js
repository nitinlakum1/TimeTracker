function bindDataTable() {
    datatable = $('#tblUsers')
        .dataTable(
            {
                "sAjaxSource": "/Resource/GetResourcesList",
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
                            Experience: $('#cmbExperience').val(),
                        })
                    });
                    perm = aoData;
                },
                "aoColumns": [
                    { "data": "name" },
                    { "data": "gender" },
                    { "data": "mobile" },
                    { "data": "email" },
                    { "data": "workYears" },
                    { "data": "designation" },
                    { "data": "degree" },
                    { "data": "birthDate" },
                    { "data": "workStartDate" },
                    { "data": "companyExperiences" },
                    { "data": "city" },
                    { "data": "Remarks" },
                ],
                columnDefs: [
                    {
                        targets: 11,
                        render: function (data, type, row) {
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
                                return '<a data-toggle="modal" data-target="#basicModal""><i class="fas fa-trash text-danger" style="cursor:pointer;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmbExperience').change(function () {
    $('#tblUsers').DataTable().draw();
});

function deleteUser() {

}