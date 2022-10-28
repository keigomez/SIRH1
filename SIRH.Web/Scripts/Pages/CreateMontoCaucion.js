$(document).ready(function () {

    $("#btnGuardar").click(function () {
        $("#btnGuardar").css("display", "none");
        $("#preloader").css("display", "block");
    });

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    //$('#FechaRige').datepicker(config);
    //$('#FechaInactiva').datepicker(config);

    $("#trFechaInactividad").hide();
    $("#trJustificacionInactividad").hide();

    RegistrarInactividad();

});

function RegistrarInactividad() {
    if ($('#ddlEstado').val() == "Inactivo") {
        $("#trFechaInactividad").show();
        $("#trJustificacionInactividad").show();
    }
    else {
        $("#trFechaInactividad").hide();
        $("#trJustificacionInactividad").hide();
    }
}
