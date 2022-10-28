$(document).ready(function () {
    $('#notaI').val("0"); $('#notaII').val("0"); $('#notaIII').val("0"); $('#notaIV').val("0"); $('#notaV').val("0");
    $('#notaVI').val("0"); $('#notaVII').val("0"); $('#notaVIII').val("0"); $('#notaIX').val("0"); $('#notaX').val("0");

    $("input[id=btnI]").click(function () {
        if ($(this).val() == 1) {
            $('#notaI').val("4");
        } else if ($(this).val() == 2) {
            $('#notaI').val("6");
        } else if ($(this).val() == 3) {
            $('#notaI').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaI').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaI').val("10");

        }
    });
    $("input[id=btnII]").click(function () {
        if ($(this).val() == 1) {
            $('#notaII').val("4");
        } else if ($(this).val() == 2) {
            $('#notaII').val("6");
        } else if ($(this).val() == 3) {
            $('#notaII').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaII').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaII').val("10");

        }
    });
    $("input[id=btnIII]").click(function () {
        if ($(this).val() == 1) {
            $('#notaIII').val("4");
        } else if ($(this).val() == 2) {
            $('#notaIII').val("6");
        } else if ($(this).val() == 3) {
            $('#notaIII').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaIII').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaIII').val("10");

        }
    });
    $("input[id=btnIV]").click(function () {
        if ($(this).val() == 1) {
            $('#notaIV').val("4");
        } else if ($(this).val() == 2) {
            $('#notaIV').val("6");
        } else if ($(this).val() == 3) {
            $('#notaIV').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaIV').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaIV').val("10");

        }
    });
    $("input[id=btnV]").click(function () {
        if ($(this).val() == 1) {
            $('#notaV').val("4");
        } else if ($(this).val() == 2) {
            $('#notaV').val("6");
        } else if ($(this).val() == 3) {
            $('#notaV').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaV').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaV').val("10");

        }
    });
    $("input[id=btnVI]").click(function () {
        if ($(this).val() == 1) {
            $('#notaVI').val("4");
        } else if ($(this).val() == 2) {
            $('#notaVI').val("6");
        } else if ($(this).val() == 3) {
            $('#notaVI').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaVI').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaVI').val("10");

        }
    });
    $("input[id=btnVII]").click(function () {
        if ($(this).val() == 1) {
            $('#notaVII').val("4");
        } else if ($(this).val() == 2) {
            $('#notaVII').val("6");
        } else if ($(this).val() == 3) {
            $('#notaVII').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaVII').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaVII').val("10");

        }
    });
    $("input[id=btnVIII]").click(function () {
        if ($(this).val() == 1) {
            $('#notaVIII').val("4");
        } else if ($(this).val() == 2) {
            $('#notaVIII').val("6");
        } else if ($(this).val() == 3) {
            $('#notaVIII').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaVIII').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaVIII').val("10");

        }
    });
    $("input[id=btnIX]").click(function () {
        if ($(this).val() == 1) {
            $('#notaIX').val("4");
        } else if ($(this).val() == 2) {
            $('#notaIX').val("6");
        } else if ($(this).val() == 3) {
            $('#notaIX').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaIX').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaIX').val("10");

        }
    });
    $("input[id=btnX]").click(function () {
        if ($(this).val() == 1) {
            $('#notaX').val("4");
        } else if ($(this).val() == 2) {
            $('#notaX').val("6");
        } else if ($(this).val() == 3) {
            $('#notaX').val("7,5");
        } else if ($(this).val() == 4) {
            $('#notaX').val("8,5");
        } else if ($(this).val() == 5) {
            $('#notaX').val("10");

        }
    });

});


var error = false;

function beginData() {
    if ($('#cedula').val().length < 1 &&
        $('#cedulaJefe').val().length < 1 &&
        $('#coddivision').val().length < 1 &&
        $('#coddireccion').val().length < 1 &&
        $('#coddepartamento').val().length < 1 &&
        $('#codseccion').val().length < 1) {
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

function beginDataListado() {
    if ($('#cedula').val().length < 1 &&
        $('#cedulaJefe').val().length < 1) {
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
    $('#toggleE1').bootstrapToggle();
}


