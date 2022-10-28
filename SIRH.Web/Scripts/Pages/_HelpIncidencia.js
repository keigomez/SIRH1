$(document).ready(function () {
    $(".help").prepend("<i class='fa fa-question-circle-o'></i>");
});

// Controlar PopUp Help
function ShowPopUp() {
    $("#detail-help").appendTo("body");
    $('#detail-help').modal('show');
    $("#data-help").css("display", "none");
    $("#preloader1").css("display", "block");
}

function FullPopUp() {
    $("#preloader1").css("display", "none");
    $("#data-help").css("display", "block");
}

function HidePopUp() {
    $("#detail-help .close").click();
}