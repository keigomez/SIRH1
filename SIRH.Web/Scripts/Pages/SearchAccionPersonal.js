$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#FecRigeDesde").datepicker(config);
    $("#FecRigeHasta").datepicker(config);
    $("#FecVenceDesde").datepicker(config);
    $("#FecVenceHasta").datepicker(config);

    $("#FecRigeDesde").val("");
    $("#FecRigeHasta").val("");
    $("#FecVenceDesde").val("");
    $("#FecVenceHasta").val("");
});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $('#preloader').removeAttr("hidden");
    $('#preloader').show();
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    //$('#preloader').hide();
    $("#btnBuscar").css("display", "block");
}

function SuccessSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}


function BeginEditar() {
    $("#btnEditar").hide();
    $("#preloader").show();
}

function CompleteEditar() {
    $("#preloader").hide();
    $("#btnEditar").show();
}

function ExportarAPdf() {
    $('form#thisForm').submit();
}

function ObtenerDetalle(numAccion) {
    $('#target').load("/AccionPersonal/Details?numAccion=" + numAccion);
    $("#detalle-historico").appendTo("body");
    $('#detalle-historico').modal('show');
    return false;
}

function ObtenerAnular(numAccion) {
    $('#target').load("/AccionPersonal/Edit?numAccion=" + numAccion);
    $("#detalle-historico").appendTo("body");
    $('#detalle-historico').modal('show');
    return false;
}

function ObtenerAnularDetalle(numAccion) {
    $('#target').load("/AccionPersonal/Details?numAccion=" + numAccion + "&accion=modificar';");
    $("#detalle-historico").appendTo("body");
    $('#detalle-historico').modal('show');
    return false;
}

function CargarDetalle(numAccion) {
    alert(numAccion);
    $.ajax({
        type: "Get",
        url: '@Url.Action("Details", "AccionPersonal")',
        data: { numAccion: numAccion },
        success: function (data) {
            alert('OK');
            $('#Preview').html(data);
            $('#modalDetalle').modal('show');
        }
    })
    alert('Exit');
    return false;
}


function ObtenerDetalleIncapacidad(id) {
    $('#target').load("/RegistroIncapacidad/Details?codigo=" + id);
    $("#detalle-historico").appendTo("body");
    $('#detalle-historico').modal('show');
    return false;
}