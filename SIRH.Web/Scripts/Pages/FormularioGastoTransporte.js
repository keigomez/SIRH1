function BeginCargarFuncionario() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");

    $('#preloader').show();
}

function CompleteCargarFuncionario() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}

function SuccessCargarFuncionario() {
    if (($("#EstadoN").val() == "Propiedad")) {
        $('#fechaDos').attr("hidden", "hidden");
        $('#fechaUno').removeAttr("hidden");
       
    } else if (($("#EstadoN").val() == "Nombramiento interino")) {
        $('#fechaDos').removeAttr("hidden");
        $('#fechaUno').attr("hidden", "hidden");
    }
 
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    try {
        var parts = $("#fechal").val().split('-');
        var today = new Date(parts[0], parts[1] - 1, parts[2]);
    } catch (e) {
    }
    try {
        $("#montoreal").on('keyup', function () {
            var value = $(this).val();
            value = ((parseFloat(value) * 0.3) / 100)*100;
            $("#MontoResultado").val(value);
        }).keyup();
    } catch (e) { }
   
    $('#FechaFinMax').datepicker({
        uiLibrary: 'bootstrap4',
        maxDate: today
    });
    $('#FechaInicio').datepicker(config);
    $('#FechaFin').datepicker(config);
    $('#FechInicio').datepicker(config);
    $('#FechFin').datepicker(config);
    $("#fechaICalculoPopUp").datepicker(config);
    $("#fechaFCalculoFPopUp").datepicker(config);
    $("#fechaFacturacionPopUp").datepicker(config);
    $("#FechaRige").datepicker(config);
    $("#FechaVence").datepicker(config);
    $("#fechaContratoIPopUp").datepicker(config);
    $("#fechaContratoFPopUp").datepicker(config);
    $("#FCabinas").change(function () {

        if (($(this).val() == 2)) {

            $('#TCabinas').removeAttr("hidden");
            $('#Dcontrato').removeAttr("hidden");
            
        }
        if (($(this).val() == 1)) {

            $('#TCabinas').attr("hidden", "hidden");
            $('#Dcontrato').attr("hidden", "hidden");
        }
        if (($(this).val() == 0)) {

            $('#TCabinas').attr("hidden", "hidden");
            $('#Dcontrato').attr("hidden", "hidden");
        }
    });
   
    
 
    $("#FechaFinAlter").change(function () {
        var NuevaFecha = $(this).val().replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$3/$2/$1');
        $('#FechaFinAlternativo').val(NuevaFecha);
    });
    
   /* try{
        $("#FechaVence").change(function () {
            
            //var fecha1 = $("#FechaRige").datepicker("getDate"); 
            var fecha2 = $(this).datepicker('getDate')
            var diasDif = fecha2.getTime() - fecha1.getTime();
            var dias = Math.round(diasDif / (1000 * 60 * 60 * 24));
            $("#dias").val(dias);
        }).keyup();
    
    } catch (e) { }*/
    try {
    
        $("#dias").on('keyup', function () {
            var valueDias = $(this).val();
            var valueMonto = $('#Monto').val();
            var total = 0;
            total = (parseFloat(valueMonto) / 22) * valueDias;
            $("#montorebajar").val(total.toFixed(3));
            $("#total").val(total.toFixed(3));
        });
    } catch (e) { }
    $("#incs").change(function () {
        $('#thisformtwo').submit();
    });
    
    $('#montobajar').val($('#Monto').val());
    
    $("#SMotivos").change(function () {

        if (($(this).val() == 1)) {
            $('#motivo').removeAttr("disabled");
            $('#inc').removeAttr("hidden");
        }
        if (($(this).val() == 2)) {

            $('#motivo').attr("disabled", "disabled");
            $('#inc').attr("Hidden", "Hidden");
        }
        if (($(this).val() == 3)) {
            $('#inc').attr("disabled", "disabled");
            $('#motivo').attr("Hidden", "Hidden");
        }
        if (($(this).val() == 4)) {
            $('#inc').attr("Hidden", "Hidden");
            $('#motivo').removeAttr("disabled");
        }
    });
    $("#DropD").change(function () {
        var temp = $(this).val();
        $('#IdD').val(temp);
        
    });
    // --------------------------------------------------------------------------------
    // ------------------------------- PopUps -----------------------------------------
    // --------------------------------------------------------------------------------
    $('#btnCalcularRetroactivo').unbind('click');
    $('#btnCalcularRetroactivo').click(function () {
        clearCalcularRetroactivoPopUp();

        $("#modal").appendTo("body");
        $('#modal').modal('show')
        //$('#buscar-retroactivo').dialog('open');
    });

    $('#btnAgregarFactura').unbind('click');
    $('#btnAgregarFactura').click(function () {
        clearFacturaPopUp();
        $('#agregar-factura').dialog('open');
        $("#modal1").appendTo("body");
        $('#modal1').modal('show')
        $("#codigoFacturaPopUp").attr('disabled', false);
        $('#EditarFacturaPopUp').hide();
        $('#AgregarFacturaPopUp').show();
    });
    $('#btnAgregarMotivo').unbind('click');
    $('#btnAgregarMotivo').click(function () {
        clearMotivoPopUp();
        $("#modal3").appendTo("body");
        $('#modal3').modal('show')
        $("#codigoFacturaPopUp").attr('disabled', false);
        $('#EditarFacturaPopUp').hide();
        $('#AgregarFacturaPopUp').show();
    });

    $('#btnAgregarRutas').unbind('click');
    $('#btnAgregarRutas').click(function () {
        clearContratoPopUp();
        $('#agregar-rutas').dialog('open');
        $("#modal2").appendTo("body");
        $('#modal2').modal('show')
        $("#codigoRutaPopUp").attr('disabled', false);
        $('#EditarRutaPopUp').hide();
        $('#AgregarRutaPopUp').show();
    });

    $('#CacelarRutaPopUp').unbind('click');
    $('#CacelarRutaPopUp').click(function () {
        clearCalcularRetroactivoPopUp(true);
    });


    $('#CacelarFacturaPopUp').unbind('click');
    $('#CacelarFacturaPopUp').click(function () {
        clearFacturaPopUp(true);
    });

    $('#CacelarContratoPopUp').unbind('click');
    $('#CacelarContratoPopUp').click(function () {
        clearContratoPopUp(true);
    });

    $('#CalcularPopUp').unbind('click');
    $('#CalcularPopUp').click(calcularRetroactivo);

    $('#AgregarFacturaPopUp').unbind('click');
    $('#AgregarFacturaPopUp').click(agregarFactura);

    $('#AgregarRutaPopUp').unbind('click');
    $('#AgregarRutaPopUp').click(agregarRuta);

    $('#AgregarMotivoPopUp').unbind('click');
    $('#AgregarMotivoPopUp').click(agregarMotivo);

    $('#btnGuardar').unbind('click');
    $('#btnGuardar').click(function () {
        $('#Conetedor3').attr("Hidden", "Hidden");
    });
    $('#btnGuardar2').unbind('click');
    $('#btnGuardar2').click(function () {
        $('#Contenedor4').attr("Hidden", "Hidden");
    });
};

// --------------------------------------------------------------------------------
// ------------------------------- Tablas Dinamicas -------------------------------
// --------------------------------------------------------------------------------

function td(elem) {
    return "<td>" + elem + "</td>";
};

function botones(id, modo) {
    return '<input class="btn btn-secondary" onclick="cargaEditar' + modo + '(\'' + id + '\')" type="button" value="Editar" />' +
    '<input class="btn btn-secondary" onClick="eliminar' + modo + '(\'' + id + '\')" type="button" value="Eliminar" />';
    alert(id);
};


function appendTable(table, codigo, values, dataValues) {
  
    var tds = values.reduce(function (anterior, actual) { return anterior + td(actual) }, "");
    $("#" + table + " > tbody:last").append('<tr id="' + codigo + '">' + tds + '</tr>');
    
    var suma = 0;
    $("#tablaRutas tr").find('td:eq(2)').each(function () {
        valor = $(this).html();
        suma += parseInt(valor);
    });
    $('#totalT').val(suma);
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

function validarCalculoRetroactivo() {
    var res = false;
    var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
    res = validation(["Vacio", "Format"], "fechaICalculoPopUp", date_regex, "FechaICalculo_validationMessage",
    "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.") || res;
    res = validation(["Vacio", "Format"], "fechaFCalculoFPopUp", date_regex, "FechaFCalculo_validationMessage",
    "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.") || res;
    res = validation("Vacio", "Carta_NumeroCarta", null, "Carta_NumeroCarta_validationMessage",
    "El número de la carta no puede ser vacio.") || res;
    return res;
}

function clearCalcularRetroactivoPopUp(cerrar) {
    $("#fechaICalculoPopUp").val("");
    $("#fechaFCalculoFPopUp").val("");
    $("#Carta_NumeroCarta").val("");
    $("#FechaICalculo_validationMessage").text("");
    $("#FechaFCalculo_validationMessage").text("");
    $("#Carta_NumeroCarta_validationMessage").text("");
    $("#MensajeCalcularRetroactivo").text("");
    if (cerrar)
        $('#buscar-retroactivo').dialog('close');
}

function clearFacturaPopUp(cerrar) {
    $("#fechaFacturacionPopUp").val("");
    $("#codigoFacturaPopUp").val("");
    $("#emisorFacturaPopUp").val("");
    $("#montoFacturaPopUp").val("");
    $("#conceptoFacturaPopUp").val("");
    $("#FechaFacturacion_validationMessage").text("");
    $("#CodigoFactura_validationMessage").text("");
    $("#EmisorFactura_validationMessage").text("");
    $("#MontoFactura_validationMessage").text("");
    $("#ConceptoFactura_validationMessage").text("");
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
        $('#modal2').modal('hide');
};
function clearMotivoPopUp(cerrar) {
    $("#motivo").val();
    $("#FechaRige").val();
    $("#FechaVence").val();
    $("#dias").val();
    $("#montobajar").val();
    $("#montorebajar").val();
    $("#total").val();
    $("#solicitud").val();

    $("#DesMotivoDTO_validationMessage").text("");
    $("#FecRigeDTO_validationMessage").text("");
    $("#FecVenceDTO_validationMessage").text("");
    $("#NumNoDiaDTO_validationMessage").text("");
    $("#MontMontoBajarDTO_validationMessage").text("");
    $("#MontMontoRebajarDTO_validationMessage").text("");
    $("#TotRebajarDTO_validationMessage").text("");
    $("#NumSolicitudAccionPDTO_validationMessage").text("");
    if (cerrar)
        $('#modal3').modal('hide');
}
function clearRutaPopUp(cerrar) {
    $("#NoRuta").val();
    $("#Fraccionamiento").val();
    $("#tarifas").val();
 
    $("#NomRutaDTO_validationMessage").text("");
    $("#NomFraccionamientoDTO_validationMessage").text("");
    $("#MontTarifa_validationMessage").text("");
    if (cerrar)
        $('#modal2').modal('hide');
}

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

    $("#modal2").appendTo("body");
    $('#modal2').modal('show')
    //$('#agregar-contrato').dialog('open');
}
function EditarMotivoPopUp(id) {
    if (validarFactura())
        return;
    var fila = $("#tablaMotivos #" + id).find("td");
    fila[1].innerHTML = ($("#motivo").val());
    fila[2].innerHTML = ($("#FechaRige").val());
    fila[3].innerHTML = ($("#FechaVence").val());
    fila[4].innerHTML = ($("#dias").val());
    fila[5].innerHTML = ($("#montobajar").val());
    fila[6].innerHTML = ($("#montorebajar").val());
    fila[7].innerHTML = ($("#total").val());
    fila[8].innerHTML = ($("#solicitud").val());
    clearMotivoPopUp(true);
}
function cargaEditarRutas(id) {
    clearRutaPopUp();
    var fila = $("#tablaRutas #" + id).find("td");
    $("#codigoRutaPopUp").val($(fila[0]).text());
    $("#codigoRutaPopUp").attr('disabled', true);
    $("#montoRutaPopUp").val($(fila[1]).text());
    $("#fechaRutaIPopUp").val($(fila[2]).text());
    
    $('#EditarRutaPopUp').unbind('click');
    $('#EditarRutaPopUp').click(function () { EditarRutasPopUp(id); });
    $('#EditarRutaPopUp').show();
    $('#AgregarRutaPopUp').hide();

    $("#modal2").appendTo("body");
    $('#modal2').modal('show')
    //$('#agregar-contrato').dialog('open');
}


function EditarRutasPopUp(id) {
    var fila = $("#tablaRutas #" + id).find("td");
    fila[0].innerHTML = ($("#NoRuta").val());
    fila[1].innerHTML = ($("#Fraccionamiento").val());
    fila[2].innerHTML = ($("#tarifas").val());
    clearRutaPopUp(true);
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

function eliminarRutas(id) {

    $('#tablaRutas #' + id).remove();
}

function eliminarMotivo(id) {
    $('#tablaMotivos #' + id).remove();
}

function agregarFactura() {
    if (validarFactura())
        return;
    var fecha = $("#fechaFacturacionPopUp").val();
    var codigo = $("#codigoFacturaPopUp").val();
    var emisor = $("#emisorFacturaPopUp").val();
    var monto = $("#montoFacturaPopUp").val();
    var concepto = $("#conceptoFacturaPopUp").val();
    var values = [codigo, monto, fecha, emisor,concepto, botones(codigo, "Factura")];
    appendTable("tablaFacturas", codigo, values);
    clearFacturaPopUp(true);
}
function agregarMotivo() {
    var motivo = $("#motivo").val();
    var fecrige = $("#FechaRige").val();
    var fecvence = $("#FechaVence").val();
    var nodias = $("#dias").val();
    var montobajar = $("#montobajar").val();
    var montorebajar = $("#montorebajar").val();
    var totalbajar = $("#total").val();
    var solicitante = $("#solicitud").val();
    var values = [motivo, fecrige, fecvence, nodias, montobajar, montorebajar, totalbajar, solicitante, botones(motivo, "Motivo")];
    appendTable("tablaMotivos", motivo, values);
    clearMotivoPopUp(true);
}
function agregarRuta() {
    var NRuta = $("#NoRuta").val();
    var Frac = $("#Fraccionamiento").val();
    var Tarifa = $("#tarifas").val();
    var values = [NRuta, Frac, Tarifa, botones(NRuta, "Rutas")];
    appendTable("tablaRutas", NRuta, values);
    clearRutaPopUp(true);
}

function agregarContrato() {
    if (validarContrato())
        return;
    var fechaI = $("#fechaContratoIPopUp").val();
    var fechaF = $("#fechaContratoFPopUp").val();
    var codigo = $("#codigoContratoPopUp").val();
    var emisor = $("#emisorContratoPopUp").val();
    var monto = $("#montoContratoPopUp").val();
    var values = [codigo, monto, fechaI, fechaF, emisor,botones(codigo, "Contrato")];
    appendTable("tablaContratos", codigo, values);
    clearContratoPopUp(true);
}
function agregarRutas() {
    
    $("#RutasData").html("");
    $("#tablaRutas > tbody > tr").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#RutasData").append("<input type=\"hidden\" name=\"Rutas[" + index + "].NomRutaDTO\" value=\"" + values[0] + "\" />");
        $("#RutasData").append("<input type=\"hidden\" name=\"Rutas[" + index + "].NomFraccionamientoDTO\" value=\"" + values[1] + "\" />");
        $("#RutasData").append("<input type=\"hidden\" name=\"Rutas[" + index + "].MontTarifa\" value=\"" + values[2] + "\" />");
    });
}

function agregarFacturasContratos() {
    $("#facturasData").html("");
    $("#tablaFacturas > tbody > tr").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].CodigoFactura\" value=\"" + values[0] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].MontoFactura\" value=\"" + values[1] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].FechaFacturacion\" value=\"" + values[2] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].Emisor\" value=\"" + values[3] + "\" />");
        $("#facturasData").append("<input type=\"hidden\" name=\"Facturas[" + index + "].ObsConcepto\" value=\"" + values[4] + "\" />");
    });
    $("#contratosData").html("");
    $("#tablaContratos > tbody > tr").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].CodigoContratoArrendamiento\" value=\"" + values[0] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].MontoContrato\" value=\"" + values[1] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].FechaInicio\" value=\"" + values[2] + "\" />");
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].FechaFin\" value=\"" + values[3] + "\" />")
        $("#contratosData").append("<input type=\"hidden\" name=\"ContratosArrendamiento[" + index + "].EmisorContrato\" value=\"" + values[4] + "\" />");
    });
}

function agregarMotivos() {

    $("#MotivosData").html("");
    $("#tablaMotivos > tbody > tr").each(function (index, elem) {
        var values = Array.prototype.slice.call(elem.cells).map(function (e) { return $(e).text(); });
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].DesMotivoDTO\" value=\"" + values[0] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].FecRigeDTO\" value=\"" + values[1] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].FecVenceDTO\" value=\"" + values[2] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].NumNoDiaDTO\" value=\"" + values[3] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].MontMontoBajarDTO\" value=\"" + values[4] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].MontMontoRebajarDTO\" value=\"" + values[5] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].TotRebajarDTO\" value=\"" + values[6] + "\" />");
        $("#MotivosData").append("<input type=\"hidden\" name=\"DetalleD[" + index + "].NumSolicitudAccionPDTO\" value=\"" + values[7] + "\" />");

    });
}
function update() {
    var cadena = $('#incs').text();
    $('#FechaRige').val(cadena.split(" ")[1]);
    $('#FechaVence').val(cadena.split(" ")[3]);
    $('#motivo').val("Incapacidad");
    $('#dias').val($('#incs').val());
    var valueDias = $('#incs').val();
    var valueMonto = $('#Monto').val();
    var total = 0;
    total = (parseFloat(valueMonto) / 22) * valueDias;
    $("#montorebajar").val(total.toFixed(3));
    $("#total").val(total.toFixed(3));
}
function calcularRetroactivo() {
    if (validarCalculoRetroactivo())
        return;
    $.ajax({
        type: "POST",
        url: "/Desarraigo/CalcularRetroactivo",
        data: $("#CalcularRetroactivo").serialize(),
        success: function (respuesta) {
            if (!isNaN(respuesta)) {
                $("#Desarraigo_MontoDesarraigo").val(respuesta * 0.3);
                clearCalcularRetroactivoPopUp(true);
            }
            else $("#MensajeCalcularRetroactivo").text(respuesta);
        }
    });


    clearContratoPopUp(true);
}


// --------------------------------------------------------------------------------
// ------------------------------- Guardado -----------------------------------------
// --------------------------------------------------------------------------------


function BeginGuardarDesarraigo() {
    $('#btnGuardar').hide();
;

    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarDesarraigo() {
    $('#preloader1').hide();
    $('#btnGuardar').show();
    
}

function SuccessGuardarDesarraigo() {
    SuccessCargarFuncionario();
}