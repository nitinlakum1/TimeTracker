//function setStatusMsg(result) {
//    $('#divAlertMsg').css('display', 'block');
//    scrollUp();
//    if (result.isSuccess) {
//        $('#divAlertMsg').removeClass('alert-danger');
//        $('#divAlertMsg').addClass('alert-success');
//        if (result.path != '' && result.path != undefined) {
//            setTimeout(function () {
//                location.href = result.path;
//            }, 2000);
//        }
//    } else {
//        $('#divAlertMsg').removeClass('alert-success');
//        $('#divAlertMsg').addClass('alert-danger');
//    }
//    $('#spnAlertMsg').text('');
//    $('#spnAlertMsg').text(result.message);
//}

var Toast;
$(function () {
    Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        customClass: 'swal-toast-position',
        top: '100px',
        showConfirmButton: false,
        timer: 3000
    });
});

function setStatusMsg(result) {
    Toast.fire({
        icon: result.isSuccess ? 'success' : 'error',
        title: result.message
    })
}

/*
 dddd - Wednesday
 DD - 31
 MM - 12
 MMM - Dec
 MMMM - December
 yyyy - 2022
 YY - 22
 HH - 23
 hh - 12
 mm - 59
 A - AM/PM
 */
function setDateTimeFormat(data, format) {
    if (data == null || data == undefined) {
        return "";
    }
    return moment(data).format(format);
}

function SelectedMenu(mainMenu, subMenu) {
    $('#' + mainMenu).addClass("active");
    $('#' + mainMenu).parent().addClass("menu-is-opening");
    $('#' + mainMenu).next('ul').css('display', 'block')
    $('#' + subMenu).addClass("active");
}

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});

//Initialize Select2 Elements
$('.select2').select2()

//Initialize Select2 Elements
$('.select2bs4').select2({
    theme: 'bootstrap4'
})

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

function todayDate() {
    var d = new Date();
    var todayDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    return todayDate;
};

$(document).ready(function () {
    setProfilePic();
});

function setProfilePic() {
    $.ajax({
        url: '/User/GetProfilePic/',
        type: 'GET',
        success: function (result) {
            if (result == '') {
                $('#imgProfilePic').attr('src', '/images/userprofilepic.jpg');
            } else {
                $('#imgProfilePic').attr('src', result);
            }
        }
    })
}