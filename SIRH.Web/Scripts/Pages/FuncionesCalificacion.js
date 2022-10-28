$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecRige').datepicker(config);
    $('#FecVence').datepicker(config);
    $('#FecRigeReglaTec').datepicker(config);
    $('#FecVenceReglaTec').datepicker(config);

    $("#CargarC").attr('hidden', 'hidden');
    $("#ListaClase1").attr('hidden', 'hidden');
    $("#ListaClase2").attr('hidden', 'hidden');
    $("#ListaClase3").attr('hidden', 'hidden');
    $("#ListaClase4").attr('hidden', 'hidden');
    $("#ListaClase5").attr('hidden', 'hidden');


    $("#Fpregunta").change(function () {
        if (($(this).val())) {
            $('#CargarC').removeAttr("hidden");
            $('#CargarC').removeAttr("disabled");
        } else {
            $('#CargarC').attr('disabled', 'disabled');
            $('#CargarC').attr('hidden', 'hidden');
        }

    });

    $('#ced').change(function () {
        //alert("el input cambio");
        if ($(this).val() == "") { //si el input es cero
            $('#Cargar').attr('disabled', 'disabled');
        }
        else { // si tiene un valor diferente a cero
            $('#Cargar').removeAttr("disabled");
        }
    });

    $('button[id=Cargar]').click(function () {
        $(this).attr('hidden', 'hidden');
        $('#preloader').removeAttr("hidden");
        
    });
    $('button[id=CargarC]').click(function () {
        $(this).attr('hidden', 'hidden');
        $('#preloader').removeAttr("hidden");

    });

    $('button[id=btnGuardar]').click(function () {
        $(this).attr('hidden', 'hidden');
        $('#preloader').removeAttr("hidden");

    });

    $('#cedula').change(function () {
        //alert("el input cambio");
        if ($(this).val()) { //si el input es cero 
            $('#lbl_formulario').removeAttr('hidden');
            $('#Fpregunta').removeAttr('hidden');
        } else {
            $('#Fpregunta').attr('hidden', 'hidden');
            $('#lbl_formulario').attr('hidden', 'hidden');
            $('#CargarC').attr('disabled', 'disabled');
            $('#CargarC').attr('hidden', 'hidden');
        }
    });

  

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

function check_text(input) {
    if (input.validity.patternMismatch) {
        input.setCustomValidity("Debe ingresar 10 dígitos y Solo números");
    }
    else {
        input.setCustomValidity("");
    }
}

function MostrarDatos(dato) {
    $('#detalle').html('');

    if (dato != "") {
        $('#lbl_formulario').removeAttr('hidden');
        $('#Fpregunta').removeAttr('hidden');
    } else {
        $('#Fpregunta').attr('hidden', 'hidden');
        $('#lbl_formulario').attr('hidden', 'hidden');
        $('#CargarC').attr('disabled', 'disabled');
        $('#CargarC').attr('hidden', 'hidden');
    }

    $("#cedula").val(dato);
}

function MostrarMensaje(mensaje) {
    $('#idMensaje').val(mensaje);
    $('#mostrar-mensaje').appendTo('body');
    $('#mostrar-mensaje').modal('show');
}

function NotaFinal() {
    var notaF = document.getElementById('notafinal');
    var nota = 0;
    var error = document.getElementById('error');
    var botoGuardar = document.getElementById('btnGuardarCalificacion');
    var notaFL = document.getElementById('notafinalletra');
    var notaNum = document.getElementById('notanumero');
    var notaI = parseFloat(document.getElementById('notaI').value.toString().replace(/\,/g, '.'));
    var notaII = parseFloat(document.getElementById('notaII').value.toString().replace(/\,/g, '.'));
    var notaIII = parseFloat(document.getElementById('notaIII').value.toString().replace(/\,/g, '.'));
    var notaIV = parseFloat(document.getElementById('notaIV').value.toString().replace(/\,/g, '.'));
    var notaV = parseFloat(document.getElementById('notaV').value.toString().replace(/\,/g, '.'));
    var notaVI = parseFloat(document.getElementById('notaVI').value.toString().replace(/\,/g, '.'));
    var notaVII = parseFloat(document.getElementById('notaVII').value.toString().replace(/\,/g, '.'));
    var notaVIII = parseFloat(document.getElementById('notaVIII').value.toString().replace(/\,/g, '.'));
    var notaIX = parseFloat(document.getElementById('notaIX').value.toString().replace(/\,/g, '.'));
    var notaX = parseFloat(document.getElementById('notaX').value.toString().replace(/\,/g, '.'));
    var suma = 0.0;
    if (notaI != 0 && notaII != 0 && notaIII != 0 && notaIV != 0 && notaV != 0
        && notaVI != 0 && notaVII != 0 && notaVIII != 0 && notaIX != 0 && notaX != 0) {
        suma = notaI + notaII + notaIII + notaIV + notaV + notaVI + notaVII + notaVIII + notaIX + notaX;
        if (suma >= 95 && suma <= 100) {
            notaFL.value = "Excelente";
            notaNum.value = '1';
        } else if (suma >= 85 && suma < 95) {
            notaFL.value = "Muy Bueno";
            notaNum.value = '2';
        } else if (suma >= 75 && suma < 85) {
            notaFL.value = "Bueno";
            notaNum.value = '3';
        } else if (suma < 75) {
            notaFL.value = "Regular";
            notaNum.value = '4';
        } else if (suma == 0) {
            notaFL.value = "Deficiente";
            notaNum.value = '5';
        }
        notaF.value = suma;
        botoGuardar.hidden = false;
        error.hidden = true;
    } else {
        notaF.value = "N/D";
        notaFL.value = "N/D";
        error.hidden = false;
        botoGuardar.hidden = true;
    }
}

function MostrarListaClaseFormulario() {
    $('#detalle').html('');

    var id = $("#Fpregunta").val();
    
    $("#CargarC").attr('hidden', 'hidden');
    $("#ListaClase1").attr('hidden', 'hidden');
    $("#ListaClase2").attr('hidden', 'hidden');
    $("#ListaClase3").attr('hidden', 'hidden');
    $("#ListaClase4").attr('hidden', 'hidden');
    $("#ListaClase5").attr('hidden', 'hidden');

    if (id > 0) {
        var nombre = "#ListaClase" + id;
        $(nombre).removeAttr('hidden');
        $("#CargarC").removeAttr('hidden');
    }
}

function BeginGuardarCalificacion() {
    $("#btnGuardarCalificacion").css("display", "none");
    $("#preloaderCalificacion").css("display", "block");
}

function CompleteGuardarCalificacion() {
    $("#preloaderCalificacion").css("display", "none");
    $("#btnGuardarCalificacion").css("display", "block");
}


//$('#dialog-jefe').click(function () {
//    $("#buscar-jefe").appendTo("body");
//    $('#buscar-jefe').modal('show');
//    return false;
//});
$("#dialog-jefe").button({
    label: 'Buscar por Jefatura',
    text: false
})
$("#clean-jefe").button({
    label: 'Limpiar búsqueda',
    text: false
}).click(function () {
    $('#codjefe').val('');
})


$(".jefatura").click(function () {
    $("#buscar-jefe").appendTo("body");
    $('#buscar-jefe').modal('show');
    return false;
});



$("#btnValidarDatos").click(function () {
    //$('#datosFuncionario').attr('hidden', 'hidden');
    $("#datosEvaluacion").removeAttr("hidden");
});

function CargarDato(periodo, funcionario) {
    $('#codPeriodo').val(periodo);
    $('#codFuncionario').val(funcionario);
    $("#buscar-jefe").appendTo("body");
    $('#buscar-jefe').modal('show');
    return false;
}


function CargarDatoHistorico(funcionario) {
    $('#cedFuncionario').val(funcionario);
    $('#periodo').val('');
    $('#nota').val('');
    $('#justificacion').val('');
    $('#error-jefe').html('');
    $('#target-jefe').html('');
    $("#agregar-historico").appendTo("body");
    $('#agregar-historico').modal('show');
    return false;
}

function VerDocumento(codRegla) {
    $('#target').load("/Calificacion/DetallesReglaTecnica?id=" + codRegla);
    return false;
}

function ActualizarJefe(idJefe, nomJefe) {
    $('#buscar-jefe').modal('hide');
    var periodo = $('#codPeriodo').val();
    var funcionario = $('#codFuncionario').val();
    $.ajax({
        type: "post",
        url: "/Calificacion/AsignarJefatura",
        data: { idPeriodo: periodo, idFuncionario: funcionario, idJefatura: idJefe },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $('#target').load('/Calificacion/_Historial');
            }
            else {
                //alert(data.mensaje);
            }
        },
        error: function (err) {
            //alert("Error");
        }
    });
    //$('#cod-jefe').val(idJefe + '' + nomJefe);
    return false;
}




function ActualizarDirector(idJefe, strCorreo) {
    $('#buscar-jefe').modal('hide');
    var id = $('#codFuncionario').val();
    $.ajax({
        type: "post",
        url: "/Calificacion/AsignarDirector",
        data: { idRegla: id, idJefatura: idJefe, correo: strCorreo },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $('#target').load("/Calificacion/ReglaTecnica");
            }
            else {
                //alert(data.mensaje);
            }
        },
        error: function (err) {
            //alert("Error");
        }
    });
    //$('#cod-jefe').val(idJefe + '' + nomJefe);
    return false;
}