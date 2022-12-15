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
                    { "data": "preferenceId" },
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
                    { "data": "preferenceId" },
                ],
                columnDefs: [
                    {
                        targets: 12,
                        render: function (data, type, row) {
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
                                return '<a onclick="findId(\'' + row.preferenceId + '\')" data-toggle="modal" data-target="#basicModal""><i class="fa-regular fa-square-plus"  style="cursor:pointer; font-size: 22px;"></i></a><a><i class="fa-solid fa-eye"  style="cursor:pointer; font-size: 22px; margin-left:10px;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

function findId(preferenceId) {
    $("#PreferenceId").val(preferenceId);
}

//function conformDelete() {
//    var id = $("#deleteId").val();
//    if (id > 0) {
//        $.ajax({
//            url: '/Resource/AddRemarks/',
//            type: 'POST',
//        })
//    }
//}