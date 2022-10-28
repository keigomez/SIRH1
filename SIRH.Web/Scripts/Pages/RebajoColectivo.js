
var progressBar;
$(document).ready(function () {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#FechaInicioVacaciones").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioVacaciones").val();
    $("#FechaFinalVacaciones").datepicker(config2);


    $("#FechaInicioVacaciones").val("");
    $("#FechaFinalVacaciones").val("");
    $("#barClass").hide();
});
function CompleteInfo() {
    $('#btnBuscar').show();
    clearInterval(progressBar);
    $("#barClass").hide();
    $("#dvResultadoBusqueda").show();
}
function BeginCargarInfo() {
    $('#btnBuscar').hide();
    $("#barClass").show();
    $("#dvResultadoBusqueda").hide();
    $("#bar").css({ width: "1%" });
    $("#label").html("1%");
    $("#bar").attr('aria-valuenow', "1");
    progressBar = setInterval("startUpdatingProgressIndicator()", 3000);
}
function CleanSearch() {
    $('form').get(0).reset();
    $("#FechaInicioVacaciones").val("");
    $("#FechaFinalVacaciones").val("");
}

function startUpdatingProgressIndicator() {
    $.ajax({
        type: "POST",
        async: "true", cache: "false",
        url: "/Vacaciones/GetUploadProgress",
        data: $("#uploadProgresses").serialize(),
        success: function (respuesta) {
            $("#bar").css({ width: respuesta + "%" });
            $("#label").html(respuesta + "%");
            $("#bar").attr('aria-valuenow', respuesta);
            if (respuesta == "100" || respuesta == "-1") {
                stopProgress;
            }
        }
    });
}
function stopProgress() {
    $("#barClass").hide();
    clearInterval(progressBar);
    $("#bar").css({ width: "0%" });
    $("#label").html("0%");
    $("#bar").attr('aria-valuenow', 0);
}

function BeginCargarAnular() {
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}