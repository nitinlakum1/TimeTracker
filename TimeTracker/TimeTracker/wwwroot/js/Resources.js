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
                    { "data": "designationName" },
                    { "data": "degree" },
                    { "data": "birthDate" },
                    { "data": "cityName" },
                    { "data": "workStartDate" },
                    //{ "data": "companyExperiences" },
                    { "data": "preferenceId" },
                ],
                columnDefs: [
                    {
                        targets: 11,
                        render: function (data, type, row) {
                            if (row.username == 'Dev' || row.username == 'Admin') {
                                return "";
                            } else {
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
    $.each($(this).val(), function (index, val) {
        var column = $('#tblResource').DataTable().column(val);
        column.visible(!column.visible());
    });
});

$('#cmbExperience').change(function () {
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
                alert("User not Delete!");
            },
        })
    }
}

function submitFollowup() {
    var id = $("#hdnPreferenceId").val();
    if (id > 0) {
        $.ajax({
            url: '/Resource/AddRemarks/',
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
                alert("User not Delete!");
            },
        })
    }
}