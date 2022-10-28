$(document).ready(function () {
    
    if ($('#fechaDesde') && $('#fechaHasta')) {
        var config = {
            locale: 'es-es',
            uiLibrary: 'bootstrap4',
            format: 'dd/mm/yyyy'
        }
        //$('#fechaDesde').datepicker(config);

        var config2 = Object.assign({}, config);
        config2["minDate"] = () => $("#fechaDesde").val();
        //$('#fechaHasta').datepicker(config2);

        $('#fechaDesde').val("");
        $('#fechaHasta').val("");
    }
    if ($('#DetallePuesto_OcupacionReal_DesOcupacionReal') && $('#RegistroTiempoExtra_Clase_DesClase')) {
        let clase = $('#RegistroTiempoExtra_Clase_DesClase').val();
        let ocupacion = $('#DetallePuesto_OcupacionReal_DesOcupacionReal').val();
        if (typeof ocupacion !== 'undefined' && ocupacion !== null && typeof clase !== 'undefined' && clase !== null) {
            if (ocupacion.toUpperCase().startsWith("GUARDIA") || clase.startsWith("OFIC.SEGUR.SERV.CIVIL")) {
                $('tbody tr td:nth-child(5),thead tr th:nth-child(5)').show();
                $('tfoot tr td:nth-child(1)').attr('colspan', 5);
                $('.horaGuarda').show();
                $('#horaDiurna').text("Monto por hora diurna: ");
            }
        }
    }
    $('#pagoDoble').change(function () {
        if ($("#pagoDoble option:selected").text() == "Jornada Doble") {
            $('#idEstado').html("Estado Jornadas Dobles");
        } else {
            $('#idEstado').html("Estado");
        }
    });
});
function resetDate(id) {
    $("#" + id).val("");
}
function onBeginNew() {
    document.getElementById("form_reportar").submit();
}
function Begin(btnId, loaderId) {
    $("#" + btnId).hide();
    $("#" + loaderId).show();
}
function Complete(btnId, loaderId) {
    $("#" + btnId).show();
    //document.getElementById(loaderId).hidden = 'hidden';
    $("#" + loaderId).hide();
}

function Anular() {
    var titulo = $("#anulado").val();
    $("#RegistroTiempoExtra_Estado").val("Anulado");
    $("#RegistroTiempoExtra_EstadoDetalles").val("Anulado");
    $("#EstadoDetalles").val("Anulado");
    $("#btnAnular").attr({ "title": titulo, "disabled": "disabled"});
}
function abrirConfirmarAnular() {
    $("#modal-anular").appendTo("body");
    $('#modal-anular').modal('show');
    return true;
}
function AnularConfirmado() {
    $('#modal-anular').modal('hide');
    Begin('btnAnular', 'preloader');
    //document.getElementById("formAnular").submit();

}

function aprobar() {
    var titulo = $("#aprobado").val();
    $("#RegistroTiempoExtra_Estado").val("Aprobado");
    $("#RegistroTiempoExtra_EstadoDetalles").val("Aprobado");
    $("#EstadoDetalles").val("Anulado");
    $("#btnAprobar").attr({ "title": "Aprobado", "disabled": "disabled" });
    $("#btnRechazar").attr({ "title": "Aprobado", "disabled": "disabled" });
}
function abrirConfirmarAprobar() {
    $("#modal-aprobar").appendTo("body");
    $('#modal-aprobar').modal('show');
    return true;
}
function AprobarConfirmado() {
    $('#modal-aprobar').modal('hide');
    //Begin('btnAprobar', 'preloader');
    //document.getElementById("formAnular").submit();

}

function rechazar() {
    var titulo = $("#rechazado").val();
    $("#RegistroTiempoExtra_Estado").val("Rechazado");
    $("#RegistroTiempoExtra_EstadoDetalles").val("Rechazado");
    $("#EstadoDetalles").val("Rechazado");
    $("#btnAprobar").attr({ "title": "Rechazado", "disabled": "disabled" });
    $("#btnRechazar").attr({ "title": "Rechazado", "disabled": "disabled" });
}
function abrirConfirmarRechazar() {
    $("#modal-rechazar").appendTo("body");
    $('#modal-rechazar').modal('show');
    return true;
}
function RechazarConfirmado() {
    $('#modal-rechazar').modal('hide');
    //Begin('btnRechazado', 'preloader');
    //document.getElementById("formAnular").submit();

}

