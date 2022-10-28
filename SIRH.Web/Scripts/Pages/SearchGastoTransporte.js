$(function () {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
   
    $("#FechaInicioGastoTransporteI").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioGastoTransporteI").val();
    $("#FechaFinalGastoTransporteI").datepicker(config2);

    $("#FechaInicioGastoTransporteF").datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FechaInicioGastoTransporteF").val();
    $("#FechaFinalGastoTransporteF").datepicker(config3);

    $("#FechaInicioGastoTransporteI").val("");
    $("#FechaInicioGastoTransporteF").val("");
    $("#FechaFinalGastoTransporteI").val("");
    $("#FechaFinalGastoTransporteF").val("");

});

function BeginSearch() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}

function CompleteSearch() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}

function CleanSearch() {
    $('form').get(0).reset();
    $("#FechaInicioGastoTransporteI").val("");
    $("#FechaInicioGastoTransporteF").val("");
    $("#FechaFinalGastoTransporteI").val("");
    $("#FechaFinalGastoTransporteF").val("");
}

function ExportarAPdf() {
    $('form#thisForm').submit();
}