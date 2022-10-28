$(document).ready(function () {

});


function OnChangeSolicitanteEsFuncionario() {


    if (document.getElementById("SolicitanteEsFuncionario").checked) {
        var text = $('#CampoSolicitanteText').val();
        $('#CampoSolicitanteEsFuncionarioText').val(text);
    } else {
        $('#CampoSolicitanteEsFuncionarioText').val("");
    }
}

function FormatoValidoParaDate(fechaSeleccionada) {

    // realizamos el substring de la fecha obtenida por el datepicker
    var dd = fechaSeleccionada.substring(0, 2);
    var mm = fechaSeleccionada.substring(3, 5);
    var yyyy = fechaSeleccionada.substring(6, 10);

    //convertimos el string recibido en un Date
    var fechaCaducidad = new Date(mm + '/' + dd + '/' + yyyy);     
    fechaCaducidad.setDate(fechaCaducidad.getDate() + 14);

    //Aquí determina si los meses son menores a 10, agrega un 0 antes ej: 13/6/2019 pasa a 13/06/2019.
    if (fechaCaducidad.getMonth() < 9) { 
        var temp = '0' + (fechaCaducidad.getMonth() + 1).toString();
        return fechaCaducidad.getDate() + "/" + temp + "/" + fechaCaducidad.getFullYear();
    } else {
        return fechaCaducidad.getDate() + "/" + (fechaCaducidad.getMonth() + 1) + "/" + fechaCaducidad.getFullYear();
    }
}

function UpdateBoletaPrestamo() {

}

function SuccesBoletaPrestamo() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        todayHighlight: true,
        format: 'dd/mm/yyyy'
    }

    $('#datepickerFechaPrestamo').datepicker(config);
    $('#datepickerFechaPrestamo').change(function () {

        var currentDate = document.getElementById("datepickerFechaPrestamo");
        $('#FechaDevolucion').val(FormatoValidoParaDate(currentDate.value));

    });
    $('#datepickerFechaPrestamo').val("");
    $('#FechaDevolucion').val("");

    if (document.getElementById("TipoUsuario").checked) {
        $('#Institucion').attr('readonly', false);
    } else {
        $('#Institucion').attr('readonly', true);
    }
}

function CompleteBoletaPrestamo() {
    $('#preloaderBuscar').css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function BeginBoletaPrestamo() {
    $("#btnBuscar").css("display", "none");
    $("#preloaderBuscar").css("display", "block");
}

function BeginGuardarBoleta() {
    $("#btnGuardarBoleta").css("display", "none");
    $("#preloaderGuardarBoleta").css("display", "block");
}

function CompleteGuardarBoleta() {
    $("#btnGuardarBoleta").css("display", "block");
    $("#preloaderGuardarBoleta").css("display", "none");
}

function SuccessGuardarBoleta() {
    $("#btnGuardarBoleta").css("display", "block");
    $("#preloaderGuardarBoleta").css("display", "none");
}