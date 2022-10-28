$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy',   
    }

    $('#Periodo').datepicker(config);

    $('#Periodo').val("");
});

function BeginSearch() {
    $("#btnSubmit").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnSubmit").css("display", "block");
}