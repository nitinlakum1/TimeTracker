function setStatusMsg(result) {
    debugger
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

function setDateTimeFormat(data) {
    if (data == null || data == undefined) {
        return "";
    }
    return moment(data).format('DD-MM-yyyy hh:mm A');
}

function setDateFormat(data) {
    if (data == null || data == undefined) {
        return "";
    }
    return moment(data).format('DD-MM-yyyy');
}

function SelectedMenu(mainMenu, subMenu) {
    $('#' + mainMenu).addClass("active");
    $('#' + mainMenu).parent().addClass("menu-is-opening");
    $('#' + mainMenu).next('ul').css('display', 'block')
    $('#' + subMenu).addClass("active");
}