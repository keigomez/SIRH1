$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    //$('#FechaInicio').datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicio").val();
    //$('#FechaFinal').datepicker(config2);

    $('#FechaInicio').val("");
    $('#FechaFinal').val("");
});

function BeginHistorico() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteHistorico() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}