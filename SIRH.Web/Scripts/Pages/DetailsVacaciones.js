function abrirActualizarSaldo() {
    $("#modal-saldo").appendTo("body");
    $('#modal-saldo').modal('show');
    return true;
}

function aprobar() {

}

function ActualizarSaldoConfirmado() {
    if ($('#txtNuevoSaldo').val() != '') {
        if ($('#diasDerecho').val() >= $('#txtNuevoSaldo').val()) {
            $("#error").attr("hidden", true);
            $('#modal-saldo').modal('hide');
            Begin('btnSaldo', 'preloader');
        }
        else {
            $("#error").attr("hidden", false);
            $('#error').html("El saldo no puede ser mayor a los días a derecho para este periodo");
        }
    }
    else
    {
        $("#error").attr("hidden", false);
        $('#error').html("Debe digitar el valor del saldo a actualizar");
    }
}

function Begin(btnId, loaderId) {
    $("#" + btnId).hide();
    $("#" + loaderId).show();
}

function CompleteActualizarSaldo(btnId, loaderId) {
    $("#" + btnId).show();
    $("#" + loaderId).hide();
}

function abrirActualizarDerecho() {
    $("#modal-derecho").appendTo("body");
    $('#modal-derecho').modal('show');
    return true;
}

function ActualizarDerechoConfirmado() {
    if ($('#txtNuevoDerecho').val() != '') {
        if ($('#saldo').val() <= $('#txtNuevoDerecho').val()) {
            $("#error").attr("hidden", true);
            $('#modal-derecho').modal('hide');
            Begin('btnDerecho', 'preloader');
        }
        else {
            $("#error").attr("hidden", false);
            $('#error').html("El saldo no puede ser mayor a los días a derecho para este periodo");
        }
    }
    else {
        $("#error").attr("hidden", false);
        $('#error').html("Debe digitar la cantidad de días a derecho a actualizar");
    }
}

function abrirAnularPeriodo() {
    $("#modal-anular").appendTo("body");
    $('#modal-anular').modal('show');
    //$("#modal-anular #bookId").val(a);
    return true;
}

function AnularPeriodoConfirmado() {

}

function CompleteAnularPeriodo(btnId, loaderId) {
    $("#" + btnId).show();
    $("#" + loaderId).hide();
}

function CompleteTrasladarRegistro(btnId, loaderId) {
    $("#" + btnId).show();
    $("#" + loaderId).hide();
    $('#modal-trasladar').modal('close');
}

function abrirTrasladarRegistro(idRegistro, numDocumento, dias) {
    $("#modal-trasladar").appendTo("body");
    $('#modal-trasladar').modal('show');
    $("#modal-trasladar #idRegistro").val(idRegistro);
    $("#modal-trasladar #numDocumento").text(numDocumento);
    $("#modal-trasladar #mensajeDias").text(dias);
    $("#modal-trasladar #dias").val(dias);

    return true;
}

function TrasladarRegistroConfirmado() {
    if (!$("input[name='periodo']").is(':checked')) {
        $("#modal-trasladar #error").show();
        $("#modal-trasladar #error").text("Debe seleccionar un periodo y la cantidad de días a trasladar para continuar.");
        return false;
    }
    else {
        var radio = $('input[name="periodo"]:checked').attr('id');
        var textbox = "diasTraslado" + radio.substring(7,radio.length);
        if ($("#" + textbox).val() != "")
        {
            return true;
        }
        else {
            $("#modal-trasladar #error").show();
            $("#modal-trasladar #error").text("Debe seleccionar un periodo y la cantidad de días a trasladar para continuar.");
            return false;
        }
    }
}

function radioClick(a)
{
    $('input[id^="diasTraslado"]').val("");
    $('input[id^="diasTraslado"]').attr("readonly", false);
    $("#diasTraslado" + a).attr("readonly", false);
}

function controlNumber(i) {
    $("#diasTraslado" + i).keypress(function (evt) {
        evt.preventDefault();
    });
    var saldo = $("#Saldo" + i).val();
    var dias = $("#modal-trasladar #dias").val();
    var cambio = $("#diasTraslado" + i).val();

    $("#modal-trasladar #diasTraslado" + i).attr('max', dias);

    //if (parseFloat(dias) > parseFloat(saldo)) {
    //    if (parseFloat(cambio) > parseFloat(saldo)) {
    //        $("#diasTraslado" + i).keypress(function (evt) {
    //            evt.preventDefault();
    //        });
    //        $("#diasTraslado" + i).val(saldo);
    //    }
    //}
    //else {
    //    if (parseFloat(cambio) > parseFloat(dias)) {
    //        $("#diasTraslado" + i).keypress(function (evt) {
    //            evt.preventDefault();
    //        });
    //        $("#diasTraslado" + i).val(dias);
    //    }
    //}
}

function abrirReintegrarRegistro(idRegistro, numDocumento, dias, inicio, fin, fuente) {
    $("#modal-reintegrar").appendTo("body");
    $('#modal-reintegrar').modal('show');
    $("#modal-reintegrar #idRegistro").val(idRegistro);
    $("#modal-reintegrar #fuente").val(fuente);
    $("#modal-reintegrar #numDocumento").text(numDocumento);
    $("#modal-reintegrar #mensajeDias").text(dias);
    $("#modal-reintegrar #dias").text(dias);
    $("#modal-reintegrar #inicio").text(inicio);
    $("#modal-reintegrar #fin").text(fin);
    $("#modal-reintegrar #fechaInicioOriginal").val(inicio);
    $("#modal-reintegrar #fechaFinOriginal").val(fin);
    $("#modal-reintegrar #diasReintegro").attr('max', dias);

    $('#modal-reintegrar #fecInicio').val('');
    $('#modal-reintegrar #fecInicio').datepicker(
    {
        language: "es",
        autoclose: true
    });

    $('#modal-reintegrar #fecFin').val('');
    $('#modal-reintegrar #fecFin').datepicker(
    {
        language: "es",
        autoclose: true
    });
    return true;
}

function ReintegrarRegistroConfirmado() {
    if ($('#modal-reintegrar #fecInicio').val() == "") {
        $("#modal-reintegrar #error").show();
        $("#modal-reintegrar #error").text("Debe seleccionar la fecha de inicio.");
        return false;
    }

    if ($('#modal-reintegrar #fecFin').val() == "") {
        $("#modal-reintegrar #error").show();
        $("#modal-reintegrar #error").text("Debe seleccionar la fecha de finalizacion.");
        return false;
    }

    if ($('#modal-reintegrar #diasReintegro').val() == "" || $('#modal-reintegrar #diasReintegro').val() == "") {
        $("#modal-reintegrar #error").show();
        $("#modal-reintegrar #error").text("Debe seleccionar la cantidad de dias a reintegrar.");
        return false;
    }

    if ($('#modal-reintegrar #docReintegro').val() == "") {
        $("#modal-reintegrar #error").show();
        $("#modal-reintegrar #error").text("Debe indicar el número de documento de reintegro.");
        return false;
    }

    if ($('#modal-reintegrar #obsReintegro').val() == "") {
        $("#modal-reintegrar #error").show();
        $("#modal-reintegrar #error").text("Debe indicar el motivo del reintegro en el campo de observaciones.");
        return false;
    }

    $("#modal-reintegrar #error").hide();
    return true;
}

function CompleteReintegrarRegistro(btnId, loaderId) {
    $("#" + btnId).show();
    $("#" + loaderId).hide();
}