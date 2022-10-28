$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecEmisionDesde').datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FecEmisionDesde").val();
    $('#FecEmisionHasta').datepicker(config2);

    $('#FecEmisionDesde').val("");
    $('#FecEmisionHasta').val("");
});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}