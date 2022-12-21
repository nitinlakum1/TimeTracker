function bindDataTable() {
    datatable = $('#tblResource')
        .dataTable(
            {
                "sAjaxSource": "/Resource/GetResourcesList",
                "bServerSide": true,
                "bProcessing": true,
                "bSearchable": true,
                "ordering": false,
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
                            Designation: $('#cmbDesignation').val(),
                            City: $('#cmbCity').val(),
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
                    { "data": "statusName" },
                    { "data": "degree" },
                    { "data": "birthDate" },
                    { "data": "city" },
                    { "data": "workStartDate" },
                    //{ "data": "companyExperiences" },
                    { "data": "preferenceId" },
                    { "data": "preferenceId" },
                ],
                columnDefs: [
                    {
                        targets: 6,
                        "createdCell": function (td, cellData, rowData, row, col) {
                            switch (cellData) {
                                case "Call Not Attend":
                                    $(td).addClass('callNotAttend-status');
                                    break;
                                case "Not Interested":
                                    $(td).addClass('notInterested-status');
                                    break;
                                case "In Process":
                                    $(td).addClass('inProcess-status');
                                    break;
                                case "Interview":
                                    $(td).addClass('interview-status');
                                    break;
                                case "Selected":
                                    $(td).addClass('selected-status');
                                    break;
                                case "Rejected":
                                    $(td).addClass('rejected-status');
                                    break;
                                case "Confirmed":
                                    $(td).addClass('confirmed-status');
                                    break;
                            }
                        },
                        className: "text-center",
                    },
                    {
                        targets: 12,
                        render: function (data, type, row) {
                            {
                                return '<a onclick="openFollowupModel(\'' + row.preferenceId + '\')"><i class="fa-regular fa-square-plus font-color"  style="cursor:pointer; font-size: 16px;"></i></a><a onclick="openDetailsModel(\'' + row.preferenceId + '\')"><i class="fa-solid fa-eye font-color"  style="cursor:pointer; font-size: 16px; margin-left:10px;"></i></a>';
                            }
                        },
                        className: "text-center",
                    }
                ],
                processing: true,
            });
}

$('#cmdColumn').on('change', function (e) {
    e.preventDefault();
    var dt = $('#tblResource').DataTable();
    dt.columns().visible(true);
    dt.columns($(this).val()).visible(false);
});

$('#cmbExperience').change(function () {
    $('#tblResource').DataTable().draw();
});

$('#cmbDesignation').keyup(function () {
    $('#tblResource').DataTable().draw();
});

$('#cmbCity').keyup(function () {
    $('#tblResource').DataTable().draw();
});

function openFollowupModel(preferenceId) {
    $('#modalFollowup').modal('show');
    $("#hdnPreferenceId").val(preferenceId);
}

function openDetailsModel(preferenceId) {
    if (preferenceId > 0) {
        $.ajax({
            url: '/Resource/GetFollowupList/',
            type: 'GET',
            data: { id: preferenceId },
            success: function (result) {
                $('#loadFollowup').html(result);
                $('#modalFollowupList').modal('show');
            },
            error: function (result) {
                alert("Followup not Found!");
            },
        })
    }
}

$('#frmFollowup').submit(function (e) {
    e.preventDefault();
    if ($('#frmFollowup').valid()) {
        var id = $("#hdnPreferenceId").val();
        if (id > 0) {
            $.ajax({
                url: '/Resource/AddFollowup/',
                type: 'POST',
                data: $('#frmFollowup').serialize(),
                success: function (result) {
                    $('#frmFollowup')[0].reset();
                    $("#hdnPreferenceId").val('');
                    $('#modalFollowup').modal('hide');
                    setStatusMsg(result);
                    $('#tblResource').DataTable().ajax.reload();
                },
                error: function (result) {
                    alert("Followup not Added!");
                },
            })
        }
    }
});
