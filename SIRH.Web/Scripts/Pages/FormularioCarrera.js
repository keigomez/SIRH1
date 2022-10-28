$(document).ready(function () {
   
    window.addEventListener("submit", function (e) {
        var form = e.target;
        if (form.getAttribute("enctype") === "multipart/form-data") {
            BeginGuardarCarrera();
            if (document.getElementById("CodigoPolicial").value != "0") {
                if (document.getElementById("DescripcionCapacitacion") != null) {
                    if (document.getElementById("DescripcionCapacitacion").value != ''
                        && document.getElementById("EntidadEducativaSeleccionada").value != ''
                        && document.getElementById("ModalidadSeleccionada").value != ''
                        && document.getElementById("FecInicio").value != ''
                        && document.getElementById("FecFinal").value != ''
                        && parseInt(document.getElementById("TotalHoras").value) < '840') {
                        sendForm(e, form);
                    }
                    else {
                        CompleteGuardarCarrera();
                        return;
                    }
                }
                else {
                    if (document.getElementById("CursoGrado").value != ''
                        && document.getElementById("EntidadEducativaSeleccionada").value != ''
                        && document.getElementById("FecEmision").value != ''
                        && document.getElementById("GradoAcademicoSeleccionado").value != ''
                        && document.getElementById("PorcentajeIncentivo").value != '') {
                        sendForm(e, form);
                    }
                    else {
                        CompleteGuardarCarrera();
                        return;
                    }
                }
            }
            else {
                if (document.getElementById("DescripcionCapacitacion") != null) {
                    if (document.getElementById("DescripcionCapacitacion").value != ''
                        && document.getElementById("EntidadEducativaSeleccionada").value != ''
                        && document.getElementById("ModalidadSeleccionada").value != ''
                        && document.getElementById("FecInicio").value != ''
                        && document.getElementById("FecFinal").value != ''
                        && parseInt(document.getElementById("TotalHoras").value) < '840') {
                        sendForm(e, form);
                    }
                    else {
                        CompleteGuardarCarrera();
                        return;
                    }
                }
                else {
                    if (document.getElementById("CursoGrado").value != ''
                        && document.getElementById("EntidadEducativaSeleccionada").value != ''
                        && document.getElementById("FecEmision").value != ''
                        && document.getElementById("GradoAcademicoSeleccionado").value != ''
                        && document.getElementById("PorcentajeIncentivo").value != '') {
                        sendForm(e, form);
                    }
                    else {
                        CompleteGuardarCarrera();
                        return;
                    }
                }
            }
        }
    }, true);

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FechaI").datepicker(config);
    $("#FechaI").val("");
    $("#FechaF").datepicker(config);
    $("#FechaF").val("");
    $("#FechaE").datepicker(config);
    $("#FechaE").val("");
});

function sendForm(e, form) {
    e.stopImmediatePropagation();
    var xhr = new XMLHttpRequest();
    xhr.open(form.method, form.action);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (form.dataset.ajaxUpdate) {
                var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                if (updateTarget) {
                    updateTarget.innerHTML = xhr.responseText;
                }
            }
        }
    };
    xhr.send(new FormData(form));
}

/*VARIABLES PARA CONTROL DE MODALIDAD SELECCIONADA*/
var puntaje = 0;
var porcentaje = 0;

/* METODOS PARA CARGA DEL FORMULARIO DE GUARDADO */
function BeginCargarFuncionario() {
    $('#preloader').show();
    $("#btnBuscar").hide();
}

function CompleteCargarFuncionario() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}

/* METODOS PARA EL CAMBIO DE MODALIDADES Y MANEJO DE CAMPOS EN EL FORMULARIO */

function OnChangeModalidad() {
    $("#warningCapacitacion").css("display", "none");
    if ($("#CodigoPolicial").val() != "0") {
        return OnChangeModalidadPolicial();
    }
    else {
        return OnChangeModalidadProfesional();
    }
}

function OnChangeModalidadPolicial() {
    var totalPuntos = 0;
    $("#TotalHoras").val(0);
    $("#TotalPorcentaje").val(0);
    $("#HorasAcum").val(0);
    var select = $("#ModalidadSeleccionada").val();
    if (select != "1" && select != "2" && select != "10" && select != "11") {
        $(".hide").hide();
        if (select == "3") {
            porcentaje = parseInt($("#PorInstruccion").val()) + parseInt($("#PuntosAdicionales").val());
            $("#Porcentaje").val(porcentaje);
            totalPuntos = parseInt($("#Porcentaje").val());
            if (totalPuntos < 1) {
                $("#TotalPorcentaje").val(20);
                $("#Porcentaje").val(20);
            }
            else {
                $("#TotalPorcentaje").val(0);
                $("#Porcentaje").val(20);
                $("#warningCapacitacion").css("display", "block");
            }
        }
        if (select == "7") {
            porcentaje = parseInt($("#PorCurso").val()) + parseInt($("#PuntosAdicionales").val());
            $("#Porcentaje").val(porcentaje);
            totalPuntos = parseInt($("#Porcentaje").val());
            if (totalPuntos < 1) {
                $("#TotalPorcentaje").val(5);
                $("#Porcentaje").val(5);
            }
            else {
                $("#TotalPorcentaje").val(0);
                $("#Porcentaje").val(5);
                $("#warningCapacitacion").css("display", "block");
            }
        }
        if (select == "6") {
            porcentaje = parseInt($("#PorRiesgo").val()) + parseInt($("#PuntosAdicionales").val());
            $("#Porcentaje").val(porcentaje);
            totalPuntos = parseInt($("#Porcentaje").val());
            if (totalPuntos < 1) {
                $("#TotalPorcentaje").val(18);
                $("#Porcentaje").val(18);
            }
            else {
                $("#TotalPorcentaje").val(0);
                $("#Porcentaje").val(18);
                $("#warningCapacitacion").css("display", "block");
            }
        }
    }
    else {
        $(".hide").show();
        porcentaje = parseInt($("#PtsEspecializada").val()) + parseInt($("#PuntosAdicionales").val());
        $("#Porcentaje").val(porcentaje);
        $("#PorcentajeAnt").val(porcentaje);
        if (select == "1" || select == "10") {
            horas = CalcularHorasAcumuladas(parseInt($("#HrsAprovechamiento").val()), 1);
            $("#HorasAcumAnt").val(horas);
            $("#HorasAcum").val(horas);
        }
        if (select == "2" || select == "11") {
            horas = CalcularHorasAcumuladas(parseInt($("#HrsParticipacion").val()), 2);
            $("#HorasAcumAnt").val(horas);
            $("#HorasAcum").val(horas);
        }
    }
}

function OnChangeModalidadProfesional() {
    var totalPuntos = 0;
    $("#TotalHoras").val(0);
    $("#TotalPuntos").val(0);
    $("#HorasAcum").val(0);
    var select = $("#ModalidadSeleccionada").val();
    if (select != "1" && select != "2" && select != "3" && select != "10" && select != "11") {
        $(".hide").hide();
        if (select == "4") {
            puntaje = parseInt($("#PtsPublicaciones").val()) + parseInt($("#PuntosAdicionales").val());
            $("#Puntaje").val(puntaje);
            totalPuntos = $("#Puntaje").val();
            $("#TotalPuntos").val(1);
            $("#Puntaje").val((parseInt(totalPuntos) + 1));
        }
        if (select == "5") {
            puntaje = parseInt($("#PtsLibros").val()) + parseInt($("#PuntosAdicionales").val());
            $("#Puntaje").val(puntaje);
            totalPuntos = $("#Puntaje").val();
            if ((parseInt(totalPuntos) + 1) <= 20) {
                $("#TotalPuntos").val(1);
                $("#Puntaje").val((parseInt(totalPuntos) + 1));
            }
            else {
                $("#TotalPuntos").val(0);
                $("#Puntaje").val(20);
                $("#warningCapacitacion").css("display", "block");
            }
        }
    }
    else {
        $(".hide").show();
        puntaje = parseInt($("#PtsEspecializada").val()) + parseInt($("#PuntosAdicionales").val());
        $("#Puntaje").val(puntaje);
        $("#PuntajeAnt").val(puntaje);
        let horas = 0;
        if (select == "1" || select == "10") {
            horas = CalcularHorasAcumuladas(parseInt($("#HrsAprovechamiento").val()),1);
            $("#HorasAcumAnt").val(horas);
            $("#HorasAcum").val(horas);
        }
        if (select == "2" || select == "11") {
            horas = CalcularHorasAcumuladas(parseInt($("#HrsParticipacion").val()), 2);
            $("#HorasAcumAnt").val(horas);
            $("#HorasAcum").val(horas);
        }
        if (select == "3") {
            horas = CalcularHorasAcumuladas(parseInt($("#HrsInstruccion").val()), 3);
            $("#HorasAcumAnt").val(horas);
            $("#HorasAcum").val(horas);
        }
    }
}


//Modalidad
//1 -> Aprovechamiento
//2 -> Participación
//3 -> Instrucción
function CalcularHorasAcumuladas(horas, modalidad) {
    let division;
    let precision;
    let respuesta;
    if (modalidad == 1 || modalidad == 10)
    {
        division = horas / 40;
        precision = Math.floor(division);
        respuesta = horas - (precision * 40);
    }
    if (modalidad == 2 || modalidad == 11) {
        division = horas / 80;
        precision = Math.floor(division);
        respuesta = horas - (precision * 80);
    }
    if (modalidad == 3) {
        division = horas / 24;
        precision = Math.floor(division);
        respuesta = horas - (precision * 24);
    }
    return respuesta;    
}

//Modalidad
//1 -> Aprovechamiento
//2 -> Participación
//3 -> Instrucción
function CalcularPuntos(horas, modalidad) {
    var division;
    var respuesta;
    if (modalidad == 1 || modalidad == 10) {
        division = horas / 40;
        respuesta = Math.floor(division);
    }
    if (modalidad == 2 || modalidad == 11) {
        division = horas / 80;
        respuesta = Math.floor(division);
    }
    if (modalidad == 3) {
        division = horas / 24;
        respuesta = Math.floor(division);
    }
    return respuesta;
}

/* METODOS PARA LA ACTUALIZACIÓN DE PUNTOS Y HORAS EN EL FORMULARIO */

function OnChangeTotalHoras() {
    $("#warningCapacitacion").css("display", "none");
    if ($("#CodigoPolicial").val() != "0") {
        return OnChangeTotalHorasPolicial();
    }
    else {
        return OnChangeTotalHorasProfesional();
    }
}

function OnChangeTotalHorasPolicial() {
    var select = $("#ModalidadSeleccionada").val();
    var total = parseInt($("#TotalHoras").val());
    $("#Porcentaje").val(porcentaje);
    var totalPuntos = porcentaje;
    var cantidad = 0;
    var acumuladas = 0;
    var puntos = 0;
    switch (select) {
        case "1":
            if (total >= 12) {
                puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1);
                acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1)
                $("#HorasAcum").val(acumuladas);
                $("#Porcentaje").val(puntos + parseInt($("#PorcentajeAnt").val()));
                $("#TotalPorcentaje").val(puntos);
            } else {
                $("#HorasAcum").val(parseInt($("#HorasAcumAnt").val()));
                $("#TotalPorcentaje").val(parseInt($("#PorcentajeAnt").val()));
            }
            break;
        case "2":
            if (total >= 30) {
                puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2);
                acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2)
                $("#HorasAcum").val(acumuladas);
                $("#Porcentaje").val(puntos + parseInt($("#PorcentajeAnt").val()));
                $("#TotalPorcentaje").val(puntos);
            } else {
                $("#HorasAcum").val(parseInt($("#HorasAcumAnt").val()));
                $("#TotalPorcentaje").val(parseInt($("#PorcentajeAnt").val()));
            }
            break;
        case "10":
            if (total >= 12) {
                puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1);
                acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1)
                $("#HorasAcum").val(acumuladas);
                $("#Porcentaje").val(puntos + parseInt($("#PorcentajeAnt").val()));
                $("#TotalPorcentaje").val(puntos);
            } else {
                $("#HorasAcum").val(parseInt($("#HorasAcumAnt").val()));
                $("#TotalPorcentaje").val(parseInt($("#PorcentajeAnt").val()));
            }
            break;
        case "11":
            if (total >= 30) {
                puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2);
                acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2)
                $("#HorasAcum").val(acumuladas);
                $("#Porcentaje").val(puntos + parseInt($("#PorcentajeAnt").val()));
                $("#TotalPorcentaje").val(puntos);
            } else {
                $("#HorasAcum").val(parseInt($("#HorasAcumAnt").val()));
                $("#TotalPorcentaje").val(parseInt($("#PorcentajeAnt").val()));
            }
            break;
        default:
            break;
    }
}


function OnChangeTotalHorasProfesional() {
    var select = $("#ModalidadSeleccionada").val();
    var total = $("#TotalHoras").val();
    if (total == "") {
        $("#TotalHoras").val(0)
        $("#HorasAcum").val(0)
        $("#TotalPuntos").val(0)
        $("#Puntaje").val(0)
        return;
    }
    $("#Puntaje").val(puntaje);
    var totalPuntos = puntaje;
    var cantidad = 0;
    var acumuladas = 0;
    var puntos = 0;
    switch (select) {
        case "1":
            puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1);
            acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1)
            $("#HorasAcum").val(acumuladas);
            $("#Puntaje").val(puntos + parseInt($("#PuntajeAnt").val()));
            $("#TotalPuntos").val(puntos);
            break;
        case "2":
            puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2);
            acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2)
            $("#HorasAcum").val(acumuladas);
            $("#Puntaje").val(puntos + parseInt($("#PuntajeAnt").val()));
            $("#TotalPuntos").val(puntos);
            break;
        case "3":
            puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 3);
            acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 3)
            $("#HorasAcum").val(acumuladas);
            $("#Puntaje").val(puntos + parseInt($("#PuntajeAnt").val()));
            $("#TotalPuntos").val(puntos);
            break;
        case "10":
            puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1);
            acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 1)
            $("#HorasAcum").val(acumuladas);
            $("#Puntaje").val(puntos + parseInt($("#PuntajeAnt").val()));
            $("#TotalPuntos").val(puntos);
            break;
        case "11":
            puntos = CalcularPuntos(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2);
            acumuladas = CalcularHorasAcumuladas(parseInt(total) + parseInt($("#HorasAcumAnt").val()), 2)
            $("#HorasAcum").val(acumuladas);
            $("#Puntaje").val(puntos + parseInt($("#PuntajeAnt").val()));
            $("#TotalPuntos").val(puntos);
            break;
        default:
            break;
    }
}

function DeterminarPuntosOficial(cantidad, puntos) {
    if ((parseInt(cantidad) + parseInt(puntos)) == 10) {
        return puntos;
    }
    else {
        puntos = puntos - 1;
        if (puntos > 0) {
            return DeterminarPuntosOficial(cantidad, puntos);
        }
        else {
            return 0;
        }
    }
}

function DeterminarPuntos(cantidad, puntos) {
    if ((parseInt(cantidad) + parseInt(puntos)) == 20) {
        return puntos;
    }
    else {
        puntos = puntos - 1;
        if (puntos > 0) {
            return DeterminarPuntos(cantidad, puntos);
        }
        else {
            return 0;
        }
    }
}

/* METODOS PARA CONTROL DEL FORMULARIO DE CURSO DE GRADO*/

function OnChangeGrado() {
    $("#warningGrado").css("display", "none");
    $("#PorcentajeIncentivo").val('');
    var grado = parseInt($("#GradoAcademicoSeleccionado option:selected").val() - 1);
    var ver = $("#GradoAcademicoSeleccionado").val();
    var actual = parseInt($("#CursoActual").val());
    if ($("#CodigoPolicial").val() == 0) {

        grado++;
        switch (grado) {
            case 1:
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(10);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 2:
                if (grado <= actual || (grado - actual) > 1) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(16);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 3:
                if ($("#CursoActual").val() != "3") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(21);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 4:
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(32);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 5:
                if ($("#CursoActual").val() != "5") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(42);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 6: 
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(40);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 7:
                if ($("#CursoActual").val() != "7") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(52);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 8:
                if ($("#CursoActual").val() != "2") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(16);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 9:
                if ($("#CursoActual").val() != "3" || $("#CursoActual").val() != "4") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    if ($("#CursoActual").val() == "3") {
                        $("#PorcentajeIncentivo").val(26);
                        $("#btnGuardar").attr("disabled", false);
                    }
                    else {
                        $("#PorcentajeIncentivo").val(31);
                        $("#btnGuardar").attr("disabled", false);
                    }
                }
                break;
            case 10:
                if ($("#CursoActual").val() != "9" || $("#CursoActual").val() != "10") {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    if ($("#CursoActual").val() == "9") {
                        $("#PorcentajeIncentivo").val(33);
                        $("#btnGuardar").attr("disabled", false);
                    }
                    else {
                        $("#PorcentajeIncentivo").val(38);
                        $("#btnGuardar").attr("disabled", false);
                    }
                }
                break;
            default:
                break;
        }
    }
    else {
        grado = grado + 1;
        switch (grado) {
            case 1:
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(10);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 2:
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(15);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            case 3:
                if (grado <= actual) {
                    $("#warningGrado").css("display", "block");
                    $("#btnGuardar").attr("disabled", true);
                }
                else {
                    $("#PorcentajeIncentivo").val(25);
                    $("#btnGuardar").attr("disabled", false);
                }
                break;
            default:
                break;
        }
    }
}


/* METODOS PARA EL GUARDADO DE DATOS DEL FORMULARIO Y CONTROL DE ERRORES */
function BeginGuardarCarrera() {
    $('#btnGuardar').hide();
    if ($('#preloaderGuardar').has("hidden"))
        $('#preloaderGuardar').removeAttr("hidden");
    $('#preloaderGuardar').show();
}

function CompleteGuardarCarrera() {
    $('#preloaderGuardar').hide();
    $('#btnGuardar').show();
}