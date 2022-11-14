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