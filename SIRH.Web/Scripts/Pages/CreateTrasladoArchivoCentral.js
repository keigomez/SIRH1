$(document).ready(function () {
    $('#preloaderBuscar').css("display", "none");
});

function SuccesTrasladoArchivoCentral() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    }

    $('#datepickerFechaTraslado').datepicker(config);
    $('#datepickerFechaTraslado').val("");
}

function BeginTrasladoArchivoCentral() {
    $("#btnBuscar").css("display", "none");
    $("#preloaderBuscar").css("display", "block");
}

function CompleteTrasladoArchivoCentral() {
    $("#preloaderBuscar").css("display", "none");
    $("#btnBuscar").css("display", "block");
}



// ajax para el formulario

function BeginTrasladarArchivo() {
    $('#myModal').modal('hide');
}

function CompleteTrasladarArchivo() {

}

function SuccesTrasladarArchivo() {

}
