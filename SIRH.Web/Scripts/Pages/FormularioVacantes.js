var error = false;
var errorFecha = false;
var todayDate = new Date().getDate();
var minDate;
var maxDate;

$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy',
        todayHighlight: true
    }

    //$('#FechaMovimiento').datepicker(config).change(function (dateText)
    //{
    //    var fecha = $('#FechaMovimiento').val();
    //    var array = fecha.split("/");
    //    var dia = array[0];
    //    var mes = array[1];
    //    var ano = array[2];
    //    minDate = new Date(Number(ano), Number(mes)-1, Number(dia));
    //    OnChangeStart();
    //});

    //$('#FechaMovimiento').val('');
    $('#ExplicacionTXT').val('');

    //$('#FecVencimiento').datepicker(config).change(function (dateText){
    //    var fecha = $('#FecVencimiento').val();
    //    var array = fecha.split("/");
    //    var dia = array[0];
    //    var mes = array[1];
    //    var ano = array[2];
    //    maxDate = new Date(Number(ano), Number(mes) - 1, Number(dia));
    //    OnChangeEnd();
    //});

    //$('#FecVencimiento').val('');

    $("#divFechaVencimiento").attr("hidden", "hidden");
});

function checkMotivo(elem) {
    if (elem.selectedIndex == 5 || elem.selectedIndex == 13 || elem.selectedIndex == 14 || elem.selectedIndex == 17 || elem.selectedIndex == 20 || elem.selectedIndex == 21 || elem.selectedIndex == 25) {
        $('#divFechaVencimiento').attr('hidden', false);
    }
    else {
        $("#divFechaVencimiento").attr("hidden", "hidden");
    }
}

function OnChangeStart()
{
    if (maxDate != 'undefined') {
        if (maxDate < minDate) {
            errorFecha = true;
        }
    }
}

function OnChangeEnd() {
    if (minDate != 'undefined') {
        if (minDate > maxDate) {
            errorFecha = true;
        }
    }
}

function Begin() {
    $("#btnGuardar").hide();
    $("#progressbarGuardar").removeAttr("hidden");
    $("#progressbarGuardar").show();


    $('#error').attr("hidden", "hidden");
    if ($('#codpuesto').val().length < 1 &&
	            $('#codclase').val().length < 1 &&
	            $('#codespecialidad').val().length < 1 &&
	            $('#codocupacion').val().length < 1) {
        $('#error').removeAttr("hidden");
        error = true;
        $('#error').show();
        $('#error').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target').hide();
        return false;
    }
    else {
        error = false;
        $('#error').hide();
        $("#progressbar").removeAttr("hidden");
        $('#progressbar').show();

        $("#btnBusca").hide();
    }
}

function Success() {
    // Animate
    $('#progressbar').hide();
    $("#btnBusca").show();
    //if (!error) {
    //    $('#target').show();
    //    error = false;
    //}

    //var config2 = {
    //    locale: 'es-es',
    //    uiLibrary: 'bootstrap4',
    //    format: 'dd/mm/yyyy'
    //}

    //$('#FechaMovimiento').val('');
    //$('#FechaMovimiento').datepicker(config2);
}

function Complete() {
    $("#btnGuardar").show();
    $("#progressbarGuardar").hide();
}


function beginData() {
    //$('#dialog-form').remove();
    $('#error').attr("hidden", "hidden");
    if ($('#cedula').val().length < 1 &&
        $('#codpuesto').val().length < 1) {
        error = true;
        $('#error').show();
        $('#error').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target').hide();
        return false;
    }
    else {
        error = false;
        $('#error').hide();
        $("#progressbar").removeAttr("hidden");
        $('#progressbar').show();

        $("#btnBusca").hide();
    }
}

function onBeginMovimiento()
{
}

function onCompleteMovimiento()
{ }

function onSuccessMovimiento()
{ }

function BeginGuardarNombramiento()
{
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteGuardarNombramiento()
{
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function successData()
{
    $('#progressbar').hide();
    $("#btnBusca").show();
    if (!error) {
        $('#target').show();
        error = false;
    }

    //if (!error) {
    //    $('#target').show();
    //    $('#accordion').accordion({
    //        autoHeight: false,
    //        navigation: true
    //    });
    //    $('#edit').button(
    //    {
    //        text: false,
    //        icons:
    //        {
    //            primary: 'ui-icon-pencil'
    //        }
    //    });
    //    $('#dialog-form').dialog(
    //    {
    //        autoOpen: false,
    //        modal: true,
    //        height: 350,
    //        width: 450,
    //        close: function () {
    //            $(".datepicker").datepicker('hide');
    //        }
    //    });
    //    $('#edit').click(function () {
    //        $('#dialog-form').dialog(
    //        {
    //            autoOpen: false,
    //            modal: true,
    //            height: 350,
    //            width: 450,
    //            close: function () {
    //                $(".datepicker").datepicker('hide');
    //            }
    //        });
    //        $('#dialog-form').dialog('open');
    //        $('#successRegistro').hide();
    //        return false;
    //    });
    //    $(".datepicker").datepicker(
    //    {
    //        showOn: 'button',
    //        buttonImage: '../Content/Images/calendar.gif',
    //        buttonImageOnly: true,
    //        buttonText: 'Seleccionar fecha'

    //    });

    //}
}

    function validarVacante() {
        var textoError = "";
        
        $('#Oficio').val($("#MotivoVacante option:selected").text());
    
        if (($("#MotivoVacante").prop('selectedIndex') == 5 || $("#MotivoVacante").prop('selectedIndex') == 13 || $("#MotivoVacante").prop('selectedIndex') == 14
            || $("#MotivoVacante").prop('selectedIndex') == 17 || $("#MotivoVacante").prop('selectedIndex') == 20 || $("#MotivoVacante").prop('selectedIndex') == 21 || $("#MotivoVacante").prop('selectedIndex') == 25)) {
            if (!($('#FecVencimiento').val() != "")) {
                $('#errorRegistro').show();
                textoError = "Debe digitar la fecha de vencimiento para el movimiento de puesto.<br />"
                $('#errorRegistro').html(textoError);
                $('#successRegistro').hide();
            } else {
                if ($('#NumeroOficio').val().length >= 1 && $('#FechaMovimiento').val() != "") {
                    var fecha1 = $('#FechaMovimiento').val();
                    var array = fecha1.split("/");
                    //var Date = new Date();
                    var dia = array[0];
                    var mes = array[1];
                    var ano = array[2];
                    var fecha1Final = new Date(Number(ano), Number(mes) - 1, Number(dia));
                    var fecha2 = $('#FecVencimiento').val();
                    var array1 = fecha2.split("/");
                    var dia1 = array1[0];
                    var mes1 = array1[1];
                    var ano1 = array1[2];
                    var fecha2Final = new Date(Number(ano1), Number(mes1) - 1, Number(dia1));
                    var diff = new Date(fecha2Final - fecha1Final);
                    var days = diff / 1000 / 60 / 60 / 24;
                    if (days < 0) {
                        $('#errorRegistro').show();
                        textoError += "La fecha de movimiento no puede ser mayor a la fecha de vencimiento.";
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    else {
                        if (days < 30 && ($("#MotivoVacante").prop('selectedIndex') == 5 || $("#MotivoVacante").prop('selectedIndex') == 13)) {
                            $('#errorRegistro').show();
                            textoError = "Los permisos menores a 30 días deben cargarse en la sección de Deducciones del SIRH, en esta sección solo se pueden cargar aquellos que superen ese tiempo.<br />"
                            $('#errorRegistro').html(textoError);
                            $('#successRegistro').hide();
                            //$('#successRegistro').show();
                            //$('#errorRegistro').hide();
                            //$('#NotaVacante').html("Este puesto NO se registrará como Vacante");
                            //BloquearForm();
                        }
                            //aquí debería haber un puta else
                        else {
                            $('#successRegistro').show();
                            $('#errorRegistro').hide();
                            $('#NotaVacante').html("Este puesto se registrará como Vacante");
                            BloquearForm();
                        }
                    }
                }
                else {
                    $('#errorRegistro').show();
                    if ($('#NumeroOficio').val().length < 1) {
                        textoError = "Debe digitar el código del oficio que respalda esta operación.<br />"
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    if (($('#FechaMovimiento').val() == "")) {
                        textoError += "Debe digitar una fecha de movimiento válida para este registro."
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    if (errorFecha) {
                        textoError += "La fecha de movimiento no puede ser mayor a la fecha de vencimiento.";
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                }
            }
        }
        else {
            if ($('#NumeroOficio').val().length >= 1 && $('#FechaMovimiento').val() != "") {
                $('#successRegistro').show();
                $('#NotaVacante').html("Este puesto se registrará como Vacante");
                BloquearForm();
            }
            else {
                $('#errorRegistro').show();
                if ($('#NumeroOficio').val().length < 1) {
                    textoError = "Debe digitar el código del oficio que respalda esta operación.<br />"
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
                if (($('#FechaMovimiento').val() == "")) {
                    textoError += "Debe digitar una fecha de movimiento válida para este registro."
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
                if (errorFecha) {
                    textoError += "La fecha de movimiento no puede ser mayor a la fecha de vencimiento y viceversa.";
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
            }
        }
    }

    function validarNombramiento() {
        var textoError = "";

        $('#TextoMotivo').val($("#MotivoVacante option:selected").text());

        if (($("#MotivoVacante").prop('selectedIndex') == 2 || $("#MotivoVacante").prop('selectedIndex') == 9
            || $("#MotivoVacante").prop('selectedIndex') == 4
            || $("#MotivoVacante").prop('selectedIndex') == 18 || $("#MotivoVacante").prop('selectedIndex') == 23
            || $("#MotivoVacante").prop('selectedIndex') == 26)) {
            if (!($('#FecVencimiento').val() != "")) {
                $('#errorRegistro').show();
                textoError = "Debe digitar la fecha de vencimiento para el movimiento de puesto.<br />"
                $('#errorRegistro').html(textoError);
                $('#successRegistro').hide();
            } else {
                if ($('#NumeroOficio').val().length >= 1 && $('#FechaMovimiento').val() != "") {
                    var fecha1 = $('#FechaMovimiento').val();
                    var array = fecha1.split("/");
                    var dia = array[0];
                    var mes = array[1];
                    var ano = array[2];
                    var fecha1Final = new Date(Number(ano), Number(mes) - 1, Number(dia));
                    var fecha2 = $('#FecVencimiento').val();
                    var array1 = fecha2.split("/");
                    var dia1 = array1[0];
                    var mes1 = array1[1];
                    var ano1 = array1[2];
                    var fecha2Final = new Date(Number(ano1), Number(mes1) - 1, Number(dia1));
                    var diff = new Date(fecha2Final - fecha1Final);
                    var days = diff / 1000 / 60 / 60 / 24;
                    if (days < 0) {
                        $('#errorRegistro').show();
                        textoError += "La fecha de movimiento no puede ser mayor a la fecha de vencimiento.";
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    else {
                        if ((days < 30 && $("#MotivoVacante").prop('selectedIndex') == 5) || $("#MotivoVacante").prop('selectedIndex') == 13) {
                            $('#successRegistro').show();
                            $('#errorRegistro').hide();
                            //$('#NotaVacante').html("Este puesto NO se registrará como Vacante");
                            BloquearFormNombramiento();
                        }
                            //aquí debería haber un puta else
                        else {
                            $('#successRegistro').show();
                            $('#errorRegistro').hide();
                            //$('#NotaVacante').html("Este puesto se registrará como Vacante");
                            BloquearFormNombramiento();
                        }
                    }
                }
                else {
                    $('#errorRegistro').show();
                    if ($('#NumeroOficio').val().length < 1) {
                        textoError = "Debe digitar el código del oficio que respalda esta operación.<br />"
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    if (($('#FechaMovimiento').val() == "")) {
                        textoError += "Debe digitar una fecha de rige válida para este registro."
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                    if (errorFecha) {
                        textoError += "La fecha de rige no puede ser mayor a la fecha de vencimiento.";
                        $('#errorRegistro').html(textoError);
                        $('#successRegistro').hide();
                    }
                }
            }
        }
        else {
            if ($('#NumeroOficio').val().length >= 1 && $('#FechaMovimiento').val() != "") {
                $('#errorRegistro').hide();
                $('#successRegistro').show();
                //$('#NotaVacante').html("Este puesto se registrará como Vacante");
                BloquearFormNombramiento();
            }
            else {
                $('#errorRegistro').show();
                if ($('#NumeroOficio').val().length < 1) {
                    textoError = "Debe digitar el código del oficio que respalda esta operación.<br />"
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
                if (($('#FechaMovimiento').val() == "")) {
                    textoError += "Debe digitar una fecha de rige válida para este registro."
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
                if (errorFecha) {
                    textoError += "La fecha de rige no puede ser mayor a la fecha de vencimiento y viceversa.";
                    $('#errorRegistro').html(textoError);
                    $('#successRegistro').hide();
                }
            }
        }
    }


    function BloquearForm()
    {
        $("#NumeroOficio").prop("readonly", true);
        $("#FecMovimiento").attr("disabled", "disabled");
        $("#FecMovimiento").datepicker('disable');
        $("#setMotivo").val($("#MotivoVacante").val());
        $("#MotivoVacante").prop("disabled", true);
        $("#MovimientoPuesto_Explicacion").prop("readonly", true);
        $(".input-group-append").children().prop('disabled', true);
    }

    function BloquearFormNombramiento() {
        $("#NumeroOficio").prop("readonly", true);
        $("#FecMovimiento").attr("disabled", "disabled");
        $("#FecMovimiento").datepicker('disable');
        $("#setMotivo").val($("#MotivoVacante").val());
        $("#MotivoVacante").prop("disabled", true);
        $("#Explicacion").prop("readonly", true);
        $(".input-group-append").children().prop('disabled', true);
    }

    function LimpiarForm() {
        $('#successRegistro').hide();
        $('#errorRegistro').hide();
        $("#NumeroOficio").prop("readonly", false);
        $("#MovimientoPuesto_Explicacion").prop("readonly", false);
        $(".input-group-append").children().prop('disabled', false);
        $("#MotivoVacante").prop("disabled", false);
    }

    function LimpiarFormNombramiento() {
        $('#successRegistro').hide();
        $('#errorRegistro').hide();
        $("#NumeroOficio").prop("readonly", false);
        $("#Explicacion").prop("readonly", false);
        $(".input-group-append").children().prop('disabled', false);
        $("#MotivoVacante").prop("disabled", false);
    }

    function checkNombramiento(elem) {
        if (elem.selectedIndex == 2 || elem.selectedIndex == 9 || elem.selectedIndex == 4
            || elem.selectedIndex == 14 || elem.selectedIndex == 16 || elem.selectedIndex == 18 || elem.selectedIndex == 21 || elem.selectedIndex == 22
            || elem.selectedIndex == 25) {
            $('#divFechaVencimiento').attr('hidden', false);
        }
        else {
            $("#divFechaVencimiento").attr("hidden", "hidden");
        }
    }

    function beginCreateNombramiento()
    {
        $("#progressbar").removeAttr("hidden");
        $('#progressbar').show();
        $("#btnBusca").hide();
    }

    function successCreateNombramiento()
    {
        //Ver si es necesario
    }

    function completeCreateNombramiento()
    {
        $('#progressbar').hide();
        $("#btnBusca").show();
    }

    function ValidarCheck(elem)
    {
        var button = document.getElementById("btnCalcularRetroactivo");
        if (button.disabled) {
            button.disabled = false;
            document.querySelector('#btnCalcularRetroactivo').innerHTML = '<i class="fa fa-calculator"></i> Recalcular Salario';
        }
        switch (elem.id) {
            case "chkBonificacion":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idBonificacion").hidden = false;
                }
                else {
                    document.getElementById("idBonificacion").hidden = true;
                    $('#idBonificacion').val("0");
                }
                break;
            case "chkCurso":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idCurso").hidden = false;
                }
                else {
                    document.getElementById("idCurso").hidden = true;
                    $('#idCurso').val("0");
                }
                break;
            case "chkConsulta":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idConsulta").hidden = false;
                }
                else {
                    document.getElementById("idConsulta").hidden = true;
                    $('#idConsulta').val("0");
                }
                break;
            case "chkDisponibilidad":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idDisponibilidad").hidden = false;
                }
                else {
                    document.getElementById("idDisponibilidad").hidden = true;
                    $('#idDisponibilidad').val("0");
                }
                break;
            case "chkNumGrado":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idNumGrado").hidden = false;
                }
                else {
                    document.getElementById("idNumGrado").hidden = true;
                    $('#idNumGrado').val("0");
                }
                break;
            case "chkProhibicion":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idProhibicion").hidden = false;
                }
                else {
                    document.getElementById("idProhibicion").hidden = true;
                    $('#idProhibicion').val("0");
                }
                break;
            case "chkQuinquenio":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idQuinquenio").hidden = false;
                }
                else {
                    document.getElementById("idQuinquenio").hidden = true;
                    $('#idQuinquenio').val("0");
                }
                break;
            case "chkRecargo":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idRecargo").hidden = false;
                }
                else {
                    document.getElementById("idRecargo").hidden = true;
                    $('#idRecargo').val("0");
                }
                break;
            case "chkRiesgo":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idRiesgo").hidden = false;
                }
                else {
                    document.getElementById("idRiesgo").hidden = true;
                    $('#idRiesgo').val("0");
                }
                break;
            case "chkOtros":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idOtros").hidden = false;
                }
                else {
                    document.getElementById("idOtros").hidden = true;
                    $('#idOtros').val("0");
                }
                break;
            case "chkDedicacion":
                if (document.getElementById(elem.id).checked) {
                    document.getElementById("idDedicacion").hidden = false;
                }
                else {
                    document.getElementById("idDedicacion").hidden = true;
                    $('#idDedicacion').val("0");
                }
                break;
            default:
                break;
        }
    }

    var specialKeys = new Array();
    //specialKeys.push(8); //Backspace
    var entra = 0;
    var total = 0;
    var porcentajeAnterior = 0;

    function IsNumericMonto(e, texto, campo) {
        if (texto != "") {
            texto = "-" + texto;
        }
        if ($('#' + campo).val() == "" && (e.keyCode == 46 || e.keyCode == 48)) {
            ret = false;
        }
        else {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 46 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            if (ret == false) {
                $('#error' + texto).show();
                $('#error' + texto).html("El " + campo + " sólo puede contener valores numéricos");
            }
            else {
                $('#error' + texto).hide();
            }
        }
        return ret;
    }

    var totalInicial = 0;
    var entradas = 0;

    function recalcularSalario()
    {
        entradas++;
        if (entradas < 2) {
            totalInicial = parseFloat($('#idTotal').val());
        }
        document.getElementById('btnCalcularRetroactivo').disabled = true;
        document.querySelector('#btnCalcularRetroactivo').innerHTML = '<i class="fa fa-calculator"></i> Salario sin variaciones';
        var total = totalInicial;
        if($('#idBonificacion').val() != "" && parseFloat($('#idBonificacion').val()) != "NaN")
        {
            var porcentaje = ((parseFloat($('#idBonificacion').val()) / 100));
            $('#DetalleAccion_PorBonificacion').val(porcentaje);
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idCurso').val() != "" && parseFloat($('#idCurso').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idCurso').val()) / 100));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idConsulta').val() != "" && parseFloat($('#idConsulta').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idConsulta').val()) / 100));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idDisponibilidad').val() != "" && parseFloat($('#idDisponibilidad').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idDisponibilidad').val()) / 100));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idNumGrado').val() != "" && parseFloat($('#idNumGrado').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idNumGrado').val() * 2273)));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje);
            $('#idMtoGradoGrupo').val(porcentaje);
            $('#idTotal').val(total);
        }
        if ($('#idProhibicion').length) {
            if ($('#idProhibicion').val() != "" && parseFloat($('#idProhibicion').val()) != "NaN") {
                var porcentaje = ((parseFloat($('#idProhibicion').val()) / 100));
                var base = parseFloat($('#idSalarioBase').val());
                total = total + (porcentaje * base);
                $('#idProhibicionDedicacion').val((porcentaje * base).toFixed(2));
                $('#idTotal').val(total.toFixed(2));
            }
        }
        if ($('#idDedicacion').length) {
            if ($('#idDedicacion').val() != "" && parseFloat($('#idDedicacion').val()) != "NaN") {
                var porcentaje = ((parseFloat($('#idDedicacion').val()) / 100));
                var base = parseFloat($('#idSalarioBase').val());
                total = total + (porcentaje * base);
                $('#idProhibicionDedicacion').val((porcentaje * base).toFixed(2));
                $('#idTotal').val(total.toFixed(2));
            }
        }
        if ($('#idQuinquenio').val() != "" && parseFloat($('#idQuinquenio').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idQuinquenio').val()) / 100));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idRecargo').val() != "" && parseFloat($('#idRecargo').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idRecargo').val())));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje);
            $('#idTotal').val(total);
        }
        if ($('#idRiesgo').val() != "" && parseFloat($('#idRiesgo').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idRiesgo').val()) / 100));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje * base);
            $('#idTotal').val(total);
        }
        if ($('#idOtros').val() != "" && parseFloat($('#idOtros').val()) != "NaN") {
            var porcentaje = ((parseFloat($('#idOtros').val())));
            var base = parseFloat($('#idSalarioBase').val());
            total = total + (porcentaje);
            $('#idTotal').val(total);
        }
    }

    function isFloat(n){
        return Number(n) === n && n % 1 !== 0;
    }