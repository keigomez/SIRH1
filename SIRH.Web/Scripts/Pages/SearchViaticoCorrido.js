$(function () {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
   
    $("#FechaInicioViaticoCorridoI").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioViaticoCorridoI").val();
    $("#FechaFinalViaticoCorridoI").datepicker(config2);

    $("#FechaInicioViaticoCorridoF").datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FechaInicioViaticoCorridoF").val();
    $("#FechaFinalViaticoCorridoF").datepicker(config3);

    $("#FechaInicioViaticoCorridoI").val("");
    $("#FechaInicioViaticoCorridoF").val("");
    $("#FechaFinalViaticoCorridoI").val("");
    $("#FechaFinalViaticoCorridoF").val("");

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
    $("#FechaInicioViaticoCorridoI").val("");
    $("#FechaInicioViaticoCorridoF").val("");
    $("#FechaFinalViaticoCorridoI").val("");
    $("#FechaFinalViaticoCorridoF").val("");
}

function ExportarAPdf() {
    $('form#thisForm').submit();
}