var error = false;

function beginData() {
    if ($('#cedula').val().length < 1 &&
        $('#nombre').val().length < 1 &&
        $('#apellido1').val().length < 1 &&
        $('#apellido2').val().length < 1 &&
        $('#codestado').val().length < 1 &&
        $('#codnivel').val().length < 1 &&
        $('#codpuesto').val().length < 1 &&
        $('#codclase').val().length < 1 &&
        $('#codespecialidad').val().length < 1 &&
        $('#coddivision').val().length < 1 &&
        $('#coddireccion').val().length < 1 &&
        $('#coddepartamento').val().length < 1 &&
        $('#codseccion').val().length < 1 &&
        $('#codpresupuesto').val().length < 1 &&
        $('#codscostos').val().length < 1) {
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
        $('#preloader').removeAttr("hidden");
        $('#preloader').show();
        $("#btnBusca").hide();
    }
}

function successData() {
    // Animate
    $('#preloader').hide();
    $("#btnBusca").show();
    if (!$("#target").hasClass("error")) {
        $('#target').show();
    }
}