function bindDataTable() {
    datatable = $('#tblSalary')
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
                    aoData.push({
                        "name": "filter", "value": JSON.stringify({
                            UserId: $('#cmbUser').val(),
                        })
                    });
                    perm = aoData;
                },
                "aoColumns": [
                    { "data": "username" },
                    { "data": "salary", width: 120, "render": $.fn.dataTable.render.number(',', '.', 0, '₹ ') },
                    {
                        "data": "fromDate", width: 120, "render": function (data) {
                            return setDateFormat(data);
                        },
                    },
                    {
                        "data": "toDate", width: 120, "render": function (data) {
                            return setDateFormat(data);
                        },
                    },
                    { "data": "id", "bSortable": false, width: 70 },
                ],
                columnDefs: [
                    {
                        targets: 4,
                        render: function (data, type, row) {
                            // Admin = 1 , Employee = 2 , HR = 3
                            var roleId = $("#roleId").text();
                            if (roleId == 1 || roleId == 3) {
                                return '<a href="/salary/update/' + row.id + '" style="margin-right: 10px;" class="justify-content-center"><i class="fas fa-edit text-success"></i>';
                            } else {
                                return "N/A";
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmbUser').change(function () {
    $('#tblSalary').DataTable().draw();
});