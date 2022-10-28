$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}