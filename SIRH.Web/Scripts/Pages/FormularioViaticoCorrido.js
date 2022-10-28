const formatter = new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC'
});

function BeginCargarFuncionario() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");

    $('#preloader').show();

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#FechaPago").datepicker(config);
    $('#FechaPago').val('');
}

function CompleteCargarFuncionario() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}

function SuccessCargarFuncionario() {
    if (($("#EstadoN").val() == "Propiedad")) {
        $('#fechaDos').attr("hidden", "hidden");
        $('#fechaUno').removeAttr("hidden");
       
    } else if (($("#EstadoN").val() == "Interino")) {
        $('#fechaDos').removeAttr("hidden");
        $('#fechaUno').attr("hidden", "hidden");
    }
 
    $("#lblPagoRealizado").hide();

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    //const formatter = new Intl.NumberFormat('es-CR', {
    //    style: 'currency',
    //    currency: 'CRC'
    //});

    try{
        var parts = $("#fechal").val().split('-');
        var today = new Date(parts[0], parts[1] - 1, parts[2]);
    } catch (e) {
    }
    //try {
    //    $("#montoreal").on('keyup', function () {
    //        var value = $(this).val();
    //        value = ((parseFloat(value) * 0.3) / 100)*100;
    //        $("#MontoResultado").val(value);
    //    }).keyup();
    //} catch (e) { }
   
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

        var value = $("#montoreal").val();

        if (($(this).val() == 2)) {
            $('#TCabinas').removeAttr("hidden");
            $('#Dcontrato').removeAttr("hidden");
        }
        if (($(this).val() == 1)) {
            $('#TCabinas').attr("hidden", "hidden");
            $('#Dcontrato').attr("hidden", "hidden");

            value -= ((parseFloat(value) * 0.3) / 100).toFixed(2) * 100;
        }
        if (($(this).val() == 0)) {
            $('#TCabinas').attr("hidden", "hidden");
            $('#Dcontrato').attr("hidden", "hidden");

            value -= ((parseFloat(value) * 0.3) / 100).toFixed(2) * 100;
        }

        value = parseFloat(value.toString().replace(/\,/g, '.'));
        $("#MontoResultado").val(value);
        $("#montoTotal").val(formatter.format(value));
    });
   

 
    $("#FechaFinAlter").change(function () {
        var NuevaFecha = $(this).val().replace(/^(\d{4})-(\d{2})-(\d{2})$/g, '$3/$2/$1');
        $('#FechaFinAlternativo').val(NuevaFecha);
    });
    
   try{
        //$("#FechaVence").change(function () {
            
        //    var fecha1 = $("#FechaRige").datepicker("getDate"); 
        //    var fecha2 = $(this).datepicker('getDate')
        //    var diasDif = fecha2.getTime() - fecha1.getTime();
        //    var dias = Math.round(diasDif / (1000 * 60 * 60 * 24));
        //    $("#dias").val(dias);
        //}).keyup();
    
    } catch (e) { }
    try {

        if ($("#dias").val() != "") {
            ActualizarMontoDeduccion();
        }
        
    } catch (e) { }
    try {
    
        $("#dias").on('keyup', function () {
            ActualizarMontoDeduccion();
        });
    } catch (e) { }
   /* $("#incs").change(function () {
        alert("hola");
        $('#thisform').submit();
        
    });*/
    
    var montoVC = $('#Monto').val();
    $('#montoDeducViatico').val(montoVC);
    $('#montoDeducVC').val(formatter.format(montoVC));
    $('#montobajar').val((montoVC / 26).toFixed(2));
    $('#montoDeducbajar').val(formatter.format(montoVC / 26));
    

    $("#SMotivos").change(function () {
        $('#motivo').val("");

        if (($(this).val() == 1)) {
            $('#motivo').attr("disabled", "disabled");
            //$('#inc').attr("Hidden", "Hidden");
            //$('#motivo').removeAttr("disabled");
            $('#inc').removeAttr("hidden");
            $('#motivo').val("Incapacidad");
        }
        if (($(this).val() == 2)) {
            $('#motivo').attr("disabled", "disabled");
            $('#inc').attr("Hidden", "Hidden");
            $('#motivo').val("Vacaciones");
        }
        if (($(this).val() == 3)) {
            $('#motivo').attr("disabled", "disabled");
            $('#inc').attr("Hidden", "Hidden");
            $('#motivo').val("Permiso");
        }
        if (($(this).val() == 4)) {
            $('#motivo').removeAttr("disabled");
            $('#inc').attr("Hidden", "Hidden");
        }
    });
    $("#DropD").change(function () {
        var temp = $(this).val();
        $('#IdD').val(temp);
        
    });
    //$("#FechaInicio").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FechaFin").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FechaFin").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FechaInicio").datepicker("option", "maxDate", selectedDate);
    //    }
    //});

    // --------------------------------------------------------------------------------
    // ------------------------------- PopUps -----------------------------------------
    // --------------------------------------------------------------------------------

    //$("#fechaFacturacionPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha'
    //});

    //$("#fechaContratoIPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#fechaContratoFPopUp").datepicker("option", "minDate", selectedDate);
    //    }
    //});

    //$("#fechaContratoFPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#fechaContratoIPopUp").datepicker("option", "maxDate", selectedDate);
    //    }
    //});

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
        clearMotivoPopUp(true);
        $("#modal3").appendTo("body");
        $('#modal3').modal('show')
        $("#codigoFacturaPopUp").attr('disabled', false);
        $('#EditarFacturaPopUp').hide();
        $('#AgregarFacturaPopUp').show();
    });

    $('#btnAgregarContrato').unbind('click');
    $('#btnAgregarContrato').click(function () {
        clearContratoPopUp();
        $('#agregar-contrato').dialog('open');
        $("#modal2").appendTo("body");
        $('#modal2').modal('show')
        $("#codigoContratoPopUp").attr('disabled', false);
        $('#EditarContratoPopUp').hide();
        $('#AgregarContratoPopUp').show();
    });

    $('#CacelarCalculoPopUp').unbind('click');
    $('#CacelarCalculoPopUp').click(function () {
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

    $('#AgregarContratoPopUp').unbind('click');
    $('#AgregarContratoPopUp').click(agregarContrato);

    $('#AgregarMotivoPopUp').unbind('click');
    $('#AgregarMotivoPopUp').click(agregarMotivo);

    $('#btnGuardar').unbind('click');
    $('#btnGuardar').click(function () {
        $('#Conetedor').attr("Hidden", "Hidden");
    });
    $('#btnGuardar2').unbind('click');
    $('#btnGuardar2').click(function () {
        $('#Conetedor2').attr("Hidden", "Hidden");
    });

    $("#btnAgregarMotivo").hide();
    $("#btnGuardar2").hide();
};

function ActualizarMontoDeduccion() {
    //const formatter = new Intl.NumberFormat('es-CR', {
    //    style: 'currency',
    //    currency: 'CRC'
    //});

    var valueDias = $("#dias").val();
    var valueMonto = $('#Monto').val();
    var total = 0;
    total = (parseFloat(valueMonto) / 26) * valueDias;
    //$("#montorebajar").val(total.toFixed(2));
    //$("#montoDeducRebajar").val(formatter.format(total));
    $("#total").val(total.toFixed(2));
    $("#montoDeducTotal").val(formatter.format(total));
}


function calcularDias()
{
    var start = $("#FechaRige").val();
    var end = $("#FechaVence").val();
    var aFecha1 = start.split('/');
    var aFecha2 = end.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);

    diff = new Date(fFecha2 - fFecha1)
    days = (diff / (1000 * 60 * 60 * 24)) + 1;
    if (days < 0)
        days = 0;
    $("#dias").val(days);
    ActualizarMontoDeduccion();
}

function ObtenerDiasRebajo(tipo) {
    var fechaPago = $('#mesDeduccion').val(); // $('#FechaPago').val();
    var cedula = $('#NumCedula').val();
    var monto = $('#Monto').val();

    const formatter = new Intl.NumberFormat('es-CR', {
        style: 'currency',
        currency: 'CRC'
    });

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $.ajax({
        type: "post",
        url: "/ViaticoCorrido/GetDiasRebajar",
        data: { cedula: cedula, fecha: fechaPago, monViatico: monto, tipo: tipo },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                //$('#PorSubsidio').val(data.porc);

                var FecDiaPago = 0;
                var MonPago = 0;
                var Motivo = "";
                var MtoTotalPago = monto;
                var MtoTotalRebaja = 0;

                if (data.agregar)
                    $("#btnAgregarMotivo").attr("Hidden", "Hidden");
                else
                    $("#btnAgregarMotivo").attr("Hidden", "Hidden");


                $("#tablaDias").html('');
                $("#tablaDias").append('<tr><th class="text-left">Día</th><th class="text-left">Motivo</th><th class="text-right">Monto</th></tr>');

                for (var i = 0; i < data.lista.length; i++) {
                    FecDiaPago = data.lista[i].FecDiaPago;
                    MonPago = data.lista[i].MonPago;
                    MtoTotalRebaja += data.lista[i].MonPago;
                    MtoTotalPago -= data.lista[i].MonPago;
                    Motivo = data.lista[i].TipoDetalleDTO.DescripcionTipo; 
                    $("#tablaDias").append("<tr><td align='left'> " + FecDiaPago + "</td><td align='left'>" + Motivo + "</td><td align='right'>" + formatter.format(MonPago) + "</td></tr>");
                }
                $("#tablaDias").append("<tr><td align='left'>Días Rebaja:  " + i + "</td><td align='left'>Total</td><td align='right'>" + formatter.format(MtoTotalRebaja) + "</td></tr>");
                $("#tablaDias").append('<tr><th class="text-left"></th><th class="text-left">Total a Pagar</th><th class="text-right">' + formatter.format(MtoTotalPago) + '</th></tr>');
                $("#MtoTotalPago").val(formatter.format(MtoTotalPago));
            }
            else {
                //alert(data.mensaje);
            }
        },
        error: function (err) {
            //alert("Error");
        }
    });
}


function ObtenerDetalleDeduccion() {
    var fechaPago = $('#mesDeduccion').val();
    var id = $('#idViatico').val();

    $('#comment').val('');
    $("#tablaMotivosDetalle").empty();
    $("#btnAgregarMotivo").hide();
    $("#btnGuardar2").hide();
    $("#lblPagoRealizado").hide();
    
    const formatter = new Intl.NumberFormat('es-CR', {
        style: 'currency',
        currency: 'CRC'
    });

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $.ajax({
        type: "post",
        url: "/ViaticoCorrido/GetDetalleDeduccion",
        data: { idViatico: id, fecha: fechaPago },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {

                $('#comment').val(data.observaciones);

                //$('#PorSubsidio').val(data.porc);
                var Motivo = "";
                var FecRige = 0;
                var FecVence = 0;
                var NumDias = 0;       
                var MtoBajar = 0;
                var MtoCabinas = 0;
                var TotalBajar = 0;
                var NumSolicitud = 0;
                
                if (data.agregar) { // Mostrar botón de agregar Motivo
                    $("#btnAgregarMotivo").show();
                    $("#btnGuardar2").show();
                }
                else {
                    $("#lblPagoRealizado").show();
                }

                for (var i = 0; i < data.lista.length; i++) {
                    Motivo = data.lista[i].DesMotivoDTO;
                    FecRige = data.lista[i].FecRigeDTO;
                    FecVence = data.lista[i].FecVenceDTO;
                    NumDias = data.lista[i].NumNoDiaDTO;
                    MtoBajar = data.lista[i].MontMontoBajarDTO;
                    MtoCabinas = data.lista[i].MontMontoRebajarDTO;
                    TotalBajar = data.lista[i].TotRebajarDTO;
                    NumSolicitud = data.lista[i].NumSolicitudAccionPDTO;
                    $("#tablaMotivosDetalle").append("<tr><td align='left'> " + Motivo +
                                                    "</td><td align='left'>" + FecRige +
                                                    "</td><td align='left'>" + FecVence +
                                                    "</td><td align='left'>" + NumDias +
                                                    "</td><td align='right'>" + formatter.format(MtoBajar) +
                                                    "</td><td align='right'>" + formatter.format(MtoCabinas) +
                                                    "</td><td align='right'>" + formatter.format(TotalBajar) +
                                                    "</td><td align='right'>" + NumSolicitud + "</td></tr>");
                }
            }
            else {
                //alert(data.mensaje);
            }
        },
        error: function (err) {
            //alert("Error");
        }
    });
}



// --------------------------------------------------------------------------------
// ------------------------------- Tablas Dinamicas -------------------------------
// --------------------------------------------------------------------------------

function td(elem) {
    return "<td>" + elem + "</td>";
};

function botones(id, modo) {
    //return '<input class="btn btn-secondary" onclick="cargaEditar' + modo + '(\'' + id + '\')" type="button" value="Editar" />' +
    //'<input class="btn btn-secondary" onClick="eliminar' + modo + '(\'' + id + '\')" type="button" value="Eliminar" />';

    return '<input class="btn btn-secondary" onClick="eliminar' + modo + '(\'' + id + '\')" type="button" value="Eliminar" />';
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

function clearFacturaPopUp(cerrar) {
    $("#fechaFacturacionPopUp").val("");
    $("#codigoFacturaPopUp").val("");
    $("#emisorFacturaPopUp").val("");
    $("#montoFacturaPopUp").val("");
    $("#conceptoFacturaPopUp").val("");
    $("#ConceptoFactura_validationMessage").text("");
    $("#FechaFacturacion_validationMessage").text("");
    $("#CodigoFactura_validationMessage").text("");
    $("#EmisorFactura_validationMessage").text("");
    $("#MontoFactura_validationMessage").text("");
    if (cerrar)
        $('#modal1').modal('hide');
};
function clearMotivoPopUp(cerrar) {
    $("#motivo").val();
    $("#FechaRige").val();
    $("#FechaVence").val();
    $("#dias").val();
    $("#montobajar").val();
    //$("#montoDeducbajar").val('');
    $("#montorebajar").val();
    $("#total").val();
    $("#solicitud").val("0");

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
    //if (validarFactura())
    //    return;
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
    var id = "";
    var motivo = $("#motivo").val();
    var fecrige = $("#FechaRige").val();
    var fecvence = $("#FechaVence").val();
    var nodias = $("#dias").val();
    var montobajar = $("#montobajar").val();
    var montorebajar = $("#montorebajar").val();
    var montorebajarCabina = 0;
    var totalbajar = $("#total").val();
    var solicitante = $("#solicitud").val();
    if (nodias > 0) {
        if (totalbajar != "" && totalbajar != "0.00") {
            if (motivo != "") {
                id = motivo + fecrige.replace('/', '').replace('/', '') + fecvence.replace('/', '').replace('/', '');
                var existe = $("#tablaMotivos").find('#' + id + '');
                if (existe.length == 0) {
                    var values = [motivo, fecrige, fecvence, nodias, montobajar, montorebajarCabina, totalbajar, solicitante, botones(id, "Motivo")];
                    appendTable("tablaMotivos", id, values);
                    clearMotivoPopUp(true);
                }
            }
            else {
                alert("Debe indicar el Detalle");
            }
        }
        else {
            clearMotivoPopUp(true);
        }
    }
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

function update() {
    var cadena = $('#incs').text();
    $('#FechaRige').val(cadena.split(" ")[1]);
    $('#FechaVence').val(cadena.split(" ")[3]);
    var idIncapacidad = cadena.split(" ")[5];
    $('#motivo').val("Incapacidad");
    $('#dias').val($('#incs').val());
    var valueDias = $('#incs').val();
    var valueMonto = $('#Monto').val();
    var total = 0;
    total = (parseFloat(valueMonto) / 26) * valueDias;
    $("#montorebajar").val(parseFloat(total).toFixed(2));
    $("#total").val(total.toFixed(2));
    //$("#montoDeducRebajar").val(formatter.format(total));
    $("#montoDeducTotal").val(formatter.format(total));
    $("#solicitud").val(idIncapacidad);
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


function ObtenerCantones(idProvincia) {
    $list = $("#DropC");

    $.ajax({
        type: "post",
        url: "/ViaticoCorrido/GetCantones",
        data: { idProvincia: idProvincia },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].IdEntidad + '"> ' + data.listado[i].NomCanton + ' </option>');
                }
                ObtenerDistritos(data.listado[0].IdEntidad);
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error");
        }
    });
}

function ObtenerDistritos(idCanton) {
    $list = $("#DropD");

    $.ajax({
        type: "post",
        url: "/ViaticoCorrido/GetDistritos",
        data: { idCanton: idCanton },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].IdEntidad + '"> ' + data.listado[i].NomDistrito + ' </option>');
                }
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error");
        }
    });
}


function ObtenerDistritos(idCanton) {
    $list = $("#DropD");

    $.ajax({
        type: "post",
        url: "/ViaticoCorrido/GetDistritos",
        data: { idCanton: idCanton },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].IdEntidad + '"> ' + data.listado[i].NomDistrito + ' </option>');
                }
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error");
        }
    });
}

// --------------------------------------------------------------------------------
// ------------------------------- Guardado -----------------------------------------
// --------------------------------------------------------------------------------


function BeginGuardarDesarraigo() {
    $('#btnGuardar').hide();
    //$("#progressbarGuardar").progressbar({
    //    value: 100
    //});

    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarDesarraigo() {
    $('#preloader1').hide();
    $('#btnGuardar').show();
    //$("#FechaInicio").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FechaFin").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FechaFin").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FechaInicio").datepicker("option", "maxDate", selectedDate);
    //    }
    //});

    //$("#fechaFacturacionPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha'
    //});

    //$("#fechaContratoIPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#fechaContratoFPopUp").datepicker("option", "minDate", selectedDate);
    //    }
    //});

    //$("#fechaContratoFPopUp").datepicker({
    //    defaultDate: "+1d",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#fechaContratoIPopUp").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

function SuccessGuardarDesarraigo() {
    SuccessCargarFuncionario();
}