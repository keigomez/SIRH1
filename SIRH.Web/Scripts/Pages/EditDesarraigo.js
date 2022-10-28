//var error = false;
//var validando = false;

// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

$(document).ready(function () {
    //$('#agregar-factura').dialog({
    //    autoOpen: false,
    //    modal: true,
    //    height: 420,
    //    width: 550
    //});

    //$("#agregar-contrato").dialog({
    //    autoOpen: false,
    //    modal: true,
    //    height: 440,
    //    width: 560
    //});

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#fechaFacturacionPopUp").datepicker(config);

    $("#fechaContratoIPopUp").datepicker(config);

    $("#fechaContratoFPopUp").datepicker(config);


    $('#btnAgregarFactura').unbind('click');
    $('#btnAgregarFactura').click(function () {
        clearFacturaPopUp();
        //$('#agregar-factura').dialog('open');
        $("#modal1").appendTo("body");
        $('#modal1').modal('show')
        $("#codigoFacturaPopUp").attr('disabled', false);
        $('#EditarFacturaPopUp').hide();
        $('#AgregarFacturaPopUp').show();
    });

    $('#btnAgregarContrato').unbind('click');
    $('#btnAgregarContrato').click(function () {
        clearContratoPopUp();
        //$('#agregar-contrato').dialog('open');
        $("#modal2").appendTo("body");
        $('#modal2').modal('show')
        $("#codigoContratoPopUp").attr('disabled', false);
        $('#EditarContratoPopUp').hide();
        $('#AgregarContratoPopUp').show();
    });

    $('#CacelarFacturaPopUp').unbind('click');
    $('#CacelarFacturaPopUp').click(function () {
        clearFacturaPopUp(true);
    });

    $('#CacelarContratoPopUp').unbind('click');
    $('#CacelarContratoPopUp').click(function () {
        clearContratoPopUp(true);
    });

    $('#AgregarFacturaPopUp').unbind('click');
    $('#AgregarFacturaPopUp').click(agregarFactura);

    $('#AgregarContratoPopUp').unbind('click');
    $('#AgregarContratoPopUp').click(agregarContrato);

    $('#gruposBtn').show();
});


// --------------------------------------------------------------------------------
// ------------------------------- Tablas Dinamicas -------------------------------
// --------------------------------------------------------------------------------

function td(elem) {
    return "<td>" + elem + "</td>";
};

function botones(id, modo) {
    return '<input class="btn btn-outline-warning" onclick="cargaEditar' + modo + '(\'' + id + '\')" type="button" value="Editar" />' +
    '<input class="btn btn-outline-danger" onClick="eliminar' + modo + '(\'' + id + '\')" type="button" value="Eliminar" />';
};


function appendTable(table, codigo, values, dataValues) {
    var tds = values.reduce(function (anterior, actual) { return anterior + td(actual) }, "");
    $("#" + table + " > tbody:last").append('<tr id="' + codigo + '">' + tds + '</tr>');
};

// --------------------------------------------------------------------------------
// ------------------------------- PopUps -----------------------------------------
// --------------------------------------------------------------------------------

function validation(mode, id, param, content, mess) {
    var pintar = function (C, E) {
        E ? $("#" + C).attr('class', 'validation-summary-errors') :
            $("#" + C).attr('class', 'field-validation-valid');
    };
    var mostrar = function (C, M, E) {
        E ? $("#" + C).text(M) : $("#" + C).text("");
    };
    var master = function (C, M, E) {
        if (C) {
            pintar(C, E);
            if (M) mostrar(C, M, E);
        }
        return E;
    };
    var valor = $("#" + id).val();
    mode = mode instanceof Array ? mode : Array(mode);
    mess = mess instanceof Array ? mess : Array(mess);
    var i = 0;
    for (var i = 0; i < mode.length; i++) {
        switch (mode[i]) {
            case "Vacio":
                if (master(content, mess[i] == undefined ? mess[0] : mess[i], valor == "" || valor == null))
                    return true;
                break;
            case "TamMax":
                if (master(content, mess[i] == undefined ? mess[0] : mess[i], valor.length > param))
                    return true;
                break;
            case "NoNum":
                if (master(content, mess[i] == undefined ? mess[0] : mess[i], isNaN(valor)))
                    return true;
                break;
            case "Format":
                if (master(content, mess[i] == undefined ? mess[0] : mess[i], !param.test(valor)))
                    return true;
                break;
        }
    }
}

function validarFactura() {
    var res = false;
    var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
    res = validation(["Vacio", "Format"], "fechaFacturacionPopUp", date_regex, "FechaFacturacion_validationMessage",
    "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.") || res;
    res = validation(["Vacio", "TamMax"], "codigoFacturaPopUp", 50, "CodigoFactura_validationMessage",
    ["El código no puede ser vacio.", "El código no puede tener más de 50 carácteres"]) || res;
    res = validation(["Vacio", "TamMax"], "emisorFacturaPopUp", 50, "EmisorFactura_validationMessage",
    ["El emisor no puede ser vacio.", "El emisor no puede tener más de 50 carácteres"]) || res;
    res = validation(["Vacio", "NoNum", "Format"], "montoFacturaPopUp", /^\d{1,10}(\.\d{1,2})?$/, "MontoFactura_validationMessage",
    ["El monto no puede ser vacio.", "El monto tiene que ser un número.", "El monto no puede ser mayor a 999999999.99 o tener más de dos decimales."]) || res;
    return res;
}

function validarContrato() {
    var res = false;
    var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
    res = validation(["Vacio", "Format"], "fechaContratoIPopUp", date_regex, "FechaInicioContrato_validationMessage",
    "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.") || res;
    res = validation(["Vacio", "Format"], "fechaContratoFPopUp", date_regex, "FechaFinalContrato_validationMessage",
    "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.") || res;
    res = validation(["Vacio", "TamMax"], "codigoContratoPopUp", 50, "CodigoContrato_validationMessage",
    ["El código no puede ser vacio.", "El código no puede tener más de 50 carácteres"]) || res;
    res = validation(["Vacio", "TamMax"], "emisorContratoPopUp", 50, "EmisorContrato_validationMessage",
    ["El emisor no puede ser vacio.", "El emisor no puede tener más de 50 carácteres"]) || res;
    res = validation(["Vacio", "NoNum", "Format"], "montoContratoPopUp", /^\d{1,10}(\.\d{1,2})?$/, "MontoContrato_validationMessage",
    ["El monto no puede ser vacio.", "El monto tiene que ser un número.", "El monto no puede ser mayor a 999999999.99 o tener más de dos decimales."]) || res;
    return res;
}

function clearFacturaPopUp(cerrar) {
    $("#fechaFacturacionPopUp").val("");
    $("#codigoFacturaPopUp").val("");
    $("#emisorFacturaPopUp").val("");
    $("#montoFacturaPopUp").val("");
    $("#FechaFacturacion_validationMessage").text("");
    $("#CodigoFactura_validationMessage").text("");
    $("#EmisorFactura_validationMessage").text("");
    $("#MontoFactura_validationMessage").text("");
    if (cerrar)
        $('#agregar-factura').dialog('close');
};

function clearContratoPopUp(cerrar) {
    $("#fechaContratoIPopUp").val("");
    $("#fechaContratoFPopUp").val("");
    $("#codigoContratoPopUp").val("");
    $("#emisorContratoPopUp").val("");
    $("#montoContratoPopUp").val("");
    $("#FechaInicioContrato_validationMessage").text("");
    $("#FechaFinalContrato_validationMessage").text("");
    $("#CodigoContrato_validationMessage").text("");
    $("#EmisorContrato_validationMessage").text("");
    $("#MontoContrato_validationMessage").text("");
    if (cerrar)
        $('#agregar-contrato').dialog('close');
};

function cargaEditarFactura(id) {
    clearFacturaPopUp();
    var fila = $("#tablaFacturas #" + id).find("td");
    $("#codigoFacturaPopUp").val($(fila[0]).text());
    $("#codigoFacturaPopUp").attr('disabled', true);
    $("#montoFacturaPopUp").val($(fila[1]).text());
    $("#fechaFacturacionPopUp").val($(fila[2]).text());
    $("#emisorFacturaPopUp").val($(fila[3]).text());
    $('#EditarFacturaPopUp').unbind('click');
    $('#EditarFacturaPopUp').click(function () { EditarFacturaPopUp(id); });
    $('#EditarFacturaPopUp').show();
    $('#AgregarFacturaPopUp').hide();
    //$('#agregar-factura').dialog('open');
    $("#modal1").appendTo("body");
    $('#modal1').modal('show')
}

function cargaEditarContrato(id) {
    clearContratoPopUp();
    var fila = $("#tablaContratos #" + id).find("td");
    $("#codigoContratoPopUp").val($(fila[0]).text());
    $("#codigoContratoPopUp").attr('disabled', true);
    $("#montoContratoPopUp").val($(fila[1]).text());
    $("#fechaContratoIPopUp").val($(fila[2]).text());
    $("#fechaContratoFPopUp").val($(fila[3]).text());
    $("#emisorContratoPopUp").val($(fila[4]).text());
    $('#EditarContratoPopUp').unbind('click');
    $('#EditarContratoPopUp').click(function () { EditarContratoPopUp(id); });
    $('#EditarContratoPopUp').show();
    $('#AgregarContratoPopUp').hide();
    //$('#agregar-contrato').dialog('open');
    $("#modal2").appendTo("body");
    $('#modal2').modal('show')
}

function EditarFacturaPopUp(id) {
    if (validarFactura())
        return;
    var fila = $("#tablaFacturas #" + id).find("td");
    fila[1].innerHTML = ($("#montoFacturaPopUp").val());
    fila[2].innerHTML = ($("#fechaFacturacionPopUp").val());
    fila[3].innerHTML = ($("#emisorFacturaPopUp").val());
    clearFacturaPopUp(true);
}

function EditarContratoPopUp(id) {
    if (validarContrato())
        return;
    var fila = $("#tablaContratos #" + id).find("td");
    fila[1].innerHTML = ($("#montoContratoPopUp").val());
    fila[2].innerHTML = ($("#fechaContratoIPopUp").val());
    fila[3].innerHTML = ($("#fechaContratoFPopUp").val());
    fila[4].innerHTML = ($("#emisorContratoPopUp").val());
    clearContratoPopUp(true);
}

function eliminarFactura(id) {
    $('#tablaFacturas #' + id).remove();
}

function eliminarContrato(id) {
    $('#tablaContratos #' + id).remove();
}

function agregarFactura() {
    if (validarFactura())
        return;
    var fecha = $("#fechaFacturacionPopUp").val();
    var codigo = $("#codigoFacturaPopUp").val();
    var emisor = $("#emisorFacturaPopUp").val();
    var monto = $("#montoFacturaPopUp").val();
    var values = [codigo, monto, fecha, emisor, botones(codigo, "Factura")];
    appendTable("tablaFacturas", codigo, values);
    clearFacturaPopUp(true);
}

function agregarContrato() {
    if (validarContrato())
        return;
    var fechaI = $("#fechaContratoIPopUp").val();
    var fechaF = $("#fechaContratoFPopUp").val();
    var codigo = $("#codigoContratoPopUp").val();
    var emisor = $("#emisorContratoPopUp").val();
    var monto = $("#montoContratoPopUp").val();
    var values = [codigo, monto, fechaI, fechaF, emisor, botones(codigo, "Contrato")];
    appendTable("tablaContratos", codigo, values);
    clearContratoPopUp(true);
}

function agregarFacturasContratos() {
    $("#facturasData").html("");
    $("#tablaFacturas > tbody > tr").not(".omitir").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].CodigoFactura\" value=\"" + values[0] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].MontoFactura\" value=\"" + values[1] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].FechaFacturacion\" value=\"" + values[2] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].Emisor\" value=\"" + values[3] + "\" />");
    });
    $("#contratosData").html("");
    $("#tablaContratos > tbody > tr").not(".omitir").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].CodigoContratoArrendamiento\" value=\"" + values[0] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].MontoContrato\" value=\"" + values[1] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].FechaInicio\" value=\"" + values[2] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].FechaFin\" value=\"" + values[3] + "\" />")
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].EmisorContrato\" value=\"" + values[4] + "\" />");
    });
}

// --------------------------------------------------------------------------------
// ------------------------------- Guardado -----------------------------------------
// --------------------------------------------------------------------------------


function BeginGuardarDesarraigo() {
    $('#gruposBtn').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}

function CompleteGuardarDesarraigo() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }


    $("#fechaFacturacionPopUp").datepicker(config);

    $("#fechaContratoIPopUp").datepicker(config);

    $("#fechaContratoFPopUp").datepicker(config);

    $('#gruposBtn').show();
    
    $('#preloader').hide();
}