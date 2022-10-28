$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    }
    $('#datepickerFechaInicioPrestamoExpediente').datepicker(config);
    $('#datepickerFechaFinPrestamoExpediente').datepicker(config);

    $('#datepickerFechaInicioPrestamoExpediente').val("");
    $('#datepickerFechaFinPrestamoExpediente').val("");
    $('#preloaderBuscar').css("display", "none");
});


function UpdateBusquedaPrestamo() {

}

function SuccesBusquedaPrestamo() {

}

function CompleteBusquedaPrestamo() {
    $('#preloaderBuscar').css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function BeginBusquedaPrestamo() {
    $("#btnBuscar").css("display", "none");
    $("#preloaderBuscar").css("display", "block");
}

function OnChangeHabilitarCampoFechas() {
    if ($('#FiltroFecha').is(':checked')) {
        $("#datoABuscar").val("Sin Valor");
        $("#datoABuscar").attr('readonly', true);
    }
}

function OnChangeDeshabilitarCampoFechas() {
    $('#datepickerFechaInicioPrestamoExpediente').val("");
    $('#datepickerFechaFinPrestamoExpediente').val("");
    $("#datoABuscar").val("");
    $("#datoABuscar").attr('readonly', false);
}