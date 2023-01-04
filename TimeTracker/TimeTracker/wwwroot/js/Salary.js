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
                            return setDateTimeFormat(data, 'DD-MM-yyyy');
                        },
                    },
                    {
                        "data": "toDate", width: 120, "render": function (toDate) {
                            if (toDate == null) {
                                return 'Till Today..';
                            } else {
                                return setDateTimeFormat(toDate, 'DD-MM-yyyy');
                            }
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
                            if ((roleId == 1 || roleId == 3) && (row.toDate == null)) {
                                return '<a href="/salary/update/' + row.id + '" class="justify-content-center"><i class="fas fa-edit text-success"></i>';
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


function bindSRDataTable() {
    datatable = $('#tblSalaryReport')
        .DataTable(
            {
                "sAjaxSource": "/Salary/LoadSalaryReport",
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
                }, "fnServerParams": function (aoData) {
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
                        "data": "salaryMonth", width: 120, "render": function (data) {
                            return setDateTimeFormat(data, 'MMMM-yyyy');
                        },
                    },
                    {
                        "data": "salaryDate", width: 120, "render": function (data) {
                            return setDateTimeFormat(data, 'DD-MM-yyyy');
                        },
                    },
                    { "data": "basicSalary", width: 120 },
                    { "data": "payableAmount", width: 120 },
                    { "data": "workingDays", width: 120 },
                ],
                processing: true,
            });
}

$('#cmbUser').change(function () {
    $('#tblSalaryReport').DataTable().draw();
});

$('#UserId, #SalaryMonth').change(function () {
    var id = $('#UserId').val();
    var month = $('#SalaryMonth').val();
    if (id > 0) {
        $.ajax({
            url: '/Salary/SalaryAmount/',
            type: 'GET',
            data: { id: id, month:month },
            success: function (result) {
                $('#BasicSalary').val(result.salaryAmount);
                $('#PayableAmount').val(result.payableSalaryAmount);
                $('#WorkingDays').val(result.presentDay);
            },
            error: function (result) {
                alert("Followup not Found!");
            },
        })
    }
});
