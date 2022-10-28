$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    //$('#FechaDesde').datepicker(config);

    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaDesde").val();
    //$('#FechaHasta').datepicker(config2);

    $('#FechaDesde').val("");
    $('#FechaHasta').val("");
});

var error = false;

function successData() {
    $('#progressbar').hide();
    $("#btnBusca").show();
    if (!$("#target").hasClass("error")) {
        $('#target').show();
    }
}

function beginData() {
    if ($('#codpuesto').val().length < 1 &&
        $('#codcedula').val().length < 1 &&
        $('#FechaDesde').val().length < 1 &&
        $('#FechaHasta').val().length < 1 &&
        $('#motivoSeleccionado').val().length < 1
        ) {
        error = true;
        $("#target").addClass("error")
        $('#error').removeAttr("hidden");
        $('#error').show();
        $('#error').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target').hide();
    }
    else {
        error = false;
        $('#error').hide();
        if ($("#target").hasClass("error")) {
            $('#target').removeClass("error");
        }
        $('#progressbar').removeAttr("hidden");
        $('#progressbar').show();
        $("#btnBusca").hide();
    }
}