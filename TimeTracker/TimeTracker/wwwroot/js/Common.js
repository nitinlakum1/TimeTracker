function setStatusMsg(result) {
    $('#divAlertMsg').css('display', 'block');
    scrollUp();
    if (result.isSuccess) {
        $('#divAlertMsg').removeClass('alert-danger');
        $('#divAlertMsg').addClass('alert-success');
        if (result.path != '' && result.path != undefined) {
            setTimeout(function () {
                location.href = result.path;
            }, 2000);
        }
    } else {
        $('#divAlertMsg').removeClass('alert-success');
        $('#divAlertMsg').addClass('alert-danger');
    }
    $('#spnAlertMsg').text('');
    $('#spnAlertMsg').text(result.message);
}

function scrollDown() {
    $('html, body').animate({
        scrollTop: $(".scrollDown").offset().top
    }, 800);
};

function scrollUp() {
    $('html, body').animate({
        scrollTop: $(".scrollUp").offset().top
    }, 800);
};

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