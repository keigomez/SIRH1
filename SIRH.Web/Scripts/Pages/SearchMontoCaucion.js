$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FechaInicio').datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicio").val();
    $('#FechaFin').datepicker(config2);

    $('#FechaInicio').val("");
    $('#FechaFin').val("");
    $("#MontoInicio").val("");
    $("#MontoFinal").val("");

});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}