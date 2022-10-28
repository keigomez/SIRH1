$(document).ready(function () {
    

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FecRigeDesde").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FecRigeDesde").val();
    $("#FecRigeHasta").datepicker(config2);

    $("#FecVenceDesde").datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FecVenceDesde").val();
    $("#FecVenceHasta").datepicker(config3);

    $("#FecRigeDesde").val("");
    $("#FecRigeHasta").val("");
    $("#FecVenceDesde").val("");
    $("#FecVenceHasta").val("");
});

function BeginSearch() {
    $('#btnBuscar').hide();
    $("#progressbar").removeAttr("hidden")
    $('#progressbar').show();
}

function CompleteSearch() {
    $('#progressbar').hide();
    $('#btnBuscar').show();
}

function ExportarAPdf() {
    $('form#thisForm').submit();
}