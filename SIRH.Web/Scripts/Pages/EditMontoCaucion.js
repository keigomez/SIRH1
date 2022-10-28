$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FechaInactiva').datepicker(config);

    $('#FechaInactiva').val("");
});

function BeginEditarMontoCaucion() {
    $("#btnGuardar").hide();
    $("#preloader").show();
}

function CompleteEditarMontoCaucion() {
    $("#preloader").hide();
    $("#btnGuardar").show();
}