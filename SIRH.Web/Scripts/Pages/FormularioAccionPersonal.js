// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

function BeginCargarFuncionario() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteCargarFuncionario() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function SuccessCargarFuncionario() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecRige').datepicker(config);
    $('#FecVence').datepicker(config);

    $('#FecRigeIntegra').datepicker(config);
    $('#FecVenceIntegra').datepicker(config);

    $('#FecUltRige').datepicker(config);
    $('#FecUltVence').datepicker(config);

    $('#urlDetalle').hide();

    //$('#FecRige').datepicker().on('changeDate', function (e) {
    //    var minDate = new Date(e.date.valueOf());
    //    $('#FecVence').datepicker('setStartDate', e);
    //});

    //$('#FecVence').datepicker().on('changeDate', function (e) {
    //    var maxDate = new Date(e.date.valueOf());
    //    $('#FecRige').datepicker('setEndDate', e);
    //});

    //$("#FecRige").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function (selectedDate) {
    //        $("#FecVence").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVence").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function (selectedDate) {
    //        $("#FecRige").datepicker("option", "maxDate", selectedDate);
    //    }
    //});

    //$("#FecRigeIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function (selectedDate) {
    //        $("#FecVenceIntegra").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVenceIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function (selectedDate) {
    //        $("#FecRigeIntegra").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

// METODOS PARA CONTROLAR EL GUARDADO

function BeginGuardarAccion() {
    $('#btnGuardar').css("display", "none");
    $("#preloaderGuardar").css("display", "block");
}

function CompleteGuardarAccion() {
    $('#preloaderGuardar').css("display", "none");
    $("#btnGuardar").css("display", "block");
    $("#FecRige").datepicker({
        defaultDate: "+1w",
        showOn: 'button',
        buttonImage: '../Content/Images/calendar.gif',
        buttonImageOnly: true,
        buttonText: 'Seleccionar fecha',
        onSelect: function (selectedDate) {
            $("#FecVence").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#FecVence").datepicker({
        defaultDate: "+1w",
        showOn: 'button',
        buttonImage: '../Content/Images/calendar.gif',
        buttonImageOnly: true,
        buttonText: 'Seleccionar fecha',
        onSelect: function (selectedDate) {
            $("#FecRige").datepicker("option", "maxDate", selectedDate);
        }
    });
    $("#FecRigeIntegra").datepicker({
        defaultDate: "+1w",
        showOn: 'button',
        buttonImage: '../Content/Images/calendar.gif',
        buttonImageOnly: true,
        buttonText: 'Seleccionar fecha',
        onSelect: function (selectedDate) {
            $("#FecVenceIntegra").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#FecVenceIntegra").datepicker({
        defaultDate: "+1w",
        showOn: 'button',
        buttonImage: '../Content/Images/calendar.gif',
        buttonImageOnly: true,
        buttonText: 'Seleccionar fecha',
        onSelect: function (selectedDate) {
            $("#FecRigeIntegra").datepicker("option", "maxDate", selectedDate);
        }
    });
}

function SuccessGuardarAccion() {
    SuccessCargarFuncionario();
}


//function () {
//    $("#FecVence").val('');
//}

$('#CleanFecVence').click(function () {
    $("#FecVence").val('');
    $("#FecVenceIntegra").val('');
});

function CleanFecVenceIntegra() {
    $("#FecVence").val('');
    $("#FecVenceIntegra").val('');
}


function ActualizarFechaRige() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    var fecha = $('#FecRige').val();
    $('#FecRigeIntegra').datepicker(config);
    $('#FecRigeIntegra').val(fecha);
}

function ActualizarFechaVence() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    var fechaRige = $('#FecRige').val();
    var fecha = $('#FecVence').val();
    $('#FecVenceIntegra').datepicker(config);
    $('#FecVenceIntegra').val(fecha);

    var numCed = $("#Cedula").val();
    var idTipo = $("#idTipoAccion").val(); 

    $.ajax({
        type: "post",
        url: "/AccionPersonal/GetFechas",
        data: { idTipoAccion: idTipo, numCedula: numCed },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                if (data.fecR != '01/01/0001' && data.fecV.indexOf("01/01/0001") < 0) {

                    if ((fechaRige >= data.fecR) && (fechaRige <= data.fecV)) {
                        alert('Ya tiene una misma acción que coincide con esas fechas. Verifique por favor');
                        $('#FecUltVence').val("");
                        $('#FecRige').val("");
                        $('#FecRigeIntegra').val("");
                        $('#FecVence').val("");
                        $('#FecVenceIntegra').val("");
                    }
                        
                    
                    //$('#FecUltRige').val(data.fecR);
                    //$('#FecRige').val(data.fecRI);
                    //$('#FecRigeIntegra').val(data.fecRI);

                    //if (data.fecV.indexOf("01/01/0001") < 0) {
                    //    $('#FecUltVence').val(data.fecV);
                    //    $('#FecVence').val(data.fecVI);
                    //    $('#FecVenceIntegra').val(data.fecVI);
                    //}
                    //else {
                    //    $('#FecUltVence').val("");
                    //    $('#FecVence').val("");
                    //    $('#FecVenceIntegra').val("");
                    //}
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

function ActualizarCategoria(idCat) {
    $.ajax({
        type: "post",
        url: "/AccionPersonal/GetCategoria",
        data: { idCategoria: idCat },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $("#MtoSalBase").val(data.salario);
                $("#MtoAnual").val(data.aumento);
            }
            else {
                alert(data.mensaje);
            }
            ActualizarMontos();
        },
        error: function (err) {
            alert("Error");
        }
    });
}

function ActualizarClaseCategoria(id) {
    $.ajax({
        type: "post",
        url: "/AccionPersonal/GetClaseCategoria",
        data: { idClase: id },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $("#CategoriaSeleccionada").val(data.categoria).change();
                $("#IndCategoria").val(data.categoria);
                ActualizarCategoria(data.categoria);
            }
            else {
                alert(data.mensaje);
            }
            ActualizarMontos();
        },
        error: function (err) {
            alert("Error");
        }
    });
}

function ActualizarPorcentaje(porc) {
    var valor = parseFloat(porc);
    $("#PorProhibicion").val(valor);
    ActualizarMontos();
}

function ActualizarMontos() {

    const formatter = new Intl.NumberFormat('es-CR', {
        style: 'currency',
        currency: 'CRC'
    });

    // Your total
    var total = 0;
    var monSalBase = 0;
    var monSalCalculo = 0;
    var monAnual = 0;
    var monRecargo = 0;
    var monProh = 0;
    var monOtro = 0;
    var monPto = 0;

    var numAnual = 0;
    var porProh = 0;
    var porDisponibilidad = 0;
    var porRiesgo = 0;
    var porCurso = 0;
    var porQuinquenio = 0;
    var porBonificacion = 0;
    var porConsultaExterna = 0;
    var porPeligrosidad = 0;
    var PorGradoPolicial = 0;
    var PorCarreraPolicial = 0;

    var numPtos = 0;
    var numPtoCarrera = 0;

    monSalBase = $("#MtoSalBase").val();
    monSalBase = parseFloat(monSalBase.toString().replace(/\,/g, '.'));

    monSalCalculo = $("#MtoSalCalculo").val();
    monSalCalculo = parseFloat(monSalCalculo.toString().replace(/\,/g, '.'));

    monAnual = $("#MtoAnual").val();
    monAnual = parseFloat(monAnual.toString().replace(/\,/g, '.'));
    
    porDisponibilidad = $("#PorDisponibilidad").val();
    porDisponibilidad = parseFloat(porDisponibilidad.toString().replace(/\,/g, '.'));

    porRiesgo = $("#PorRiesgo").val();
    porRiesgo = parseFloat(porRiesgo.toString().replace(/\,/g, '.'));

    porCurso = $("#PorCurso").val();
    porCurso = parseFloat(porCurso.toString().replace(/\,/g, '.'));

    porQuinquenio = $("#PorQuinquenio").val();
    porQuinquenio = parseFloat(porQuinquenio.toString().replace(/\,/g, '.'));

    porBonificacion = $("#PorBonificacion").val();
    porBonificacion = parseFloat(porBonificacion.toString().replace(/\,/g, '.'));

    porConsultaExterna = $("#PorConsulta").val();
    porConsultaExterna = parseFloat(porConsultaExterna.toString().replace(/\,/g, '.'));

    porPeligrosidad = $("#PorPeligrosidad").val();
    porPeligrosidad = parseFloat(porPeligrosidad.toString().replace(/\,/g, '.'));

    PorGradoPolicial = $("#PorGradoPolicial").val();
    PorGradoPolicial = parseFloat(PorGradoPolicial.toString().replace(/\,/g, '.'));

    PorCarreraPolicial = $("#PorCarreraPol").val();
    PorCarreraPolicial = parseFloat(PorCarreraPolicial.toString().replace(/\,/g, '.'));
    

    monRecargo = $("#MtoRecargo").val();
    //monRecargo = (monSalBase * (porDisponibilidad + porRiesgo + porCurso)) / 100;
    monRecargo = parseFloat(monRecargo.toString().replace(/\,/g, '.'));

    //monOtro = $("#MtoOtros").val();
    monOtro = (monSalCalculo * (porDisponibilidad + porRiesgo + porCurso + porQuinquenio + porBonificacion + porConsultaExterna + porPeligrosidad + PorGradoPolicial + PorCarreraPolicial)) / 100;
    monOtro = parseFloat(monOtro.toString().replace(/\,/g, '.'));

    numPtos = $("#NumPto").val();
    numPtos = parseFloat(numPtos.toString().replace(/\,/g, '.'));

    numPtoCarrera = $("#NumPtoCarrera").val();
    numPtoCarrera = parseFloat(numPtoCarrera.toString().replace(/\,/g, '.'));

    porProh = $("#PorProhibicion").val();
    if (porProh == null || porProh == undefined) {
        porProh = $('#PorList').val();
    }

    porProh = parseFloat(porProh.toString().replace(/\,/g, '.'));

    numAnual = $("#NumAnualidad").val();

    monAnual = monAnual * numAnual;
    monProh = monSalBase * porProh / 100;
    monPto = numPtos * numPtoCarrera;

    total = monSalBase + monAnual + monRecargo + monPto + monProh + monOtro;

    $("#MtoRecargo").val(monRecargo.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, ''));
    $("#MtoProh").val(monProh.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, ''));
    $("#MtoAnuales").val(monAnual.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, ''));
    $("#MtoPtos").val(monPto.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, '')); 
    //$("#MtoOtros").val(monOtro.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, ''));
    $("#MtoOtros").val(monOtro);
    $("#otros").val(formatter.format(monOtro.toString()));
    $("#total").val(formatter.format(total.toString()));
    $("#MtoTotalNuevo").val(total);
}

function MostrarDatos(tipo) {
    //MostrarDatosSalario(false);
    $("#idTipoAccion").val(tipo);

    $("#filaUltimaAccion").hide();
    $("#filaProrroga").hide();

    $("#filaSeccion").hide();
    $("#filaClase").hide();
    $("#filaCategoria").hide();
    $("#filaPuesto").hide();
    $("#filaQuinquenio").hide();
    $("#filaBonificacion").hide();
    $("#filaConsulta").hide();
    $("#filaPeligrosidad").hide();
    $("#filaGradoPolicial").hide();
    $("#filaCarreraPol").hide();
    $("#filaEspecialidad").hide();
    $("#filaSubespecialidad").hide();
    $("#PorQuinquenioOrig").hide();
    $("#PorDisponibilidadOrig").hide();

    $("#IndPrograma").attr('readonly', true);

    $("#filaDisfrutado").attr('readonly', true);
    $("#NumDisfrutado").attr('readonly', true);
    $("#filaDisfrutado").hide();
    $("#filaAutorizado").hide();

    $("#NumAnualidad").attr('readonly', true);
    $("#MtoRecargo").attr('readonly', true);
    $("#NumPto").attr('readonly', true);

    var porProhOrig = $("#PorProhOriginal").val();
    $("#PorProhibicion").val(porProhOrig);
    $("#PorProhibicion").attr('readonly', true);
    $("#PorProhibicion").attr('hidden', false);
    $("#PorList").attr('hidden', true);
    $("#MtoOtros").attr('readonly', true);
          
    var porOriginal = $("#PorBonificacionOrig").val();
    porOriginal = parseFloat(porOriginal.toString().replace(/\,/g, '.'));
    $("#PorBonificacion").val(porOriginal);

    porOriginal = $("#PorCursoOrig").val();
    $("#PorCurso").val(porOriginal);

    porOriginal = $("#PorConsultaOrig").val();
    $("#PorConsulta").val(porOriginal);

    porOriginal = $("#PorDisponibilidadOrig").val();
    $("#PorDisponibilidad").val(porOriginal);

    porOriginal = $("#PorPeligrosidadOrig").val();
    $("#PorPeligrosidad").val(porOriginal);

    porOriginal = $("#PorGradoPolicialOrig").val();
    $("#PorGradoPolicial").val(porOriginal);

    porOriginal = $("#PorCarreraPolOrig").val();
    $("#PorCarreraPol").val(porOriginal);

    porOriginal = $("#PorQuinquenioOrig").val();
    $("#PorQuinquenio").val(porOriginal);

    porOriginal = $("#PorRiesgoOrig").val();
    $("#PorRiesgo").val(porOriginal);

    $("#MesAum").attr('disabled', true);

    //$('#FecRige').val("");
    //$('#FecVence').val("");
    //$('#FecRigeIntegra').val("");
    //$('#FecVenceIntegra').val("");
    $("#FecUltRige").val("");
    $("#FecUltVence").val("");
    $("#CodigoModulo").val("0");
    $("#CodigoObjetoEntidad").val("0");
    $('#puestoAccion').val("");
    $('#puestoAccion').hide();

    var ced = $("#Cedula").val();
    
      
    ActualizarMontos();

    MostrarDatosSalario(true); //  Se solicitó que se mostrara los datos del salario sin importar el Tipo de Acción
    switch (tipo) {
        case "8": // Prórroga de incapacidad
            //ObtenerFechas(tipo, ced);
            break;
        case "9": // Prórroga de permiso con salario.
        case "10": // Prórroga de permiso sin salario.
            ObtenerProrroga(tipo, ced);
            break;
        case "11": // Prórroga de susp. Temporal.
            //ObtenerFechas(tipo, ced);
            break;
        case "16": // Vacaciones
            //$("#filaDisfrutado").show();
            //$("#filaAutorizado").show();
            break;
        case "23": // Prórroga de nombramiento.
            //ObtenerFechas(tipo, ced);
            break;
        case "30":  // Ascenso en propiedad.
            $("#filaClase").show();
            break;
        case "31":  // Ascenso interino.
            $("#filaClase").show();
            break;
        case "32":  // Reajuste de Aumento Anual
            $("#filaDisfrutado").show();
            $("#filaAutorizado").show();
            $("#NumAnualidad").attr('readonly', false);
            break;
        case "36":  // Cambio de Categoría
            $("#filaCategoria").show();
            $("#indClase").attr('disabled', true);
            $("#CategoriaSeleccionada").attr('disabled', false);
            ActualizarClaseCategoria($("#CodClase").val());
            MostrarDatosSalario(true);
            break;
        case "37":  // Cambio de Código
            $("#filaClase").show();
            MostrarDatosSalario(true);
            break;
        case "38":  // Descenso en propiedad.
            $("#filaClase").show();
            break;
        case "39":  // Descenso interino.
            $("#filaClase").show();
            break;
        case "41":  // Reaj. Aprobación Peligrosidad
            $("#PorPeligrosidad").val("5");
            $("#filaPeligrosidad").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "42":  // Reaj. Eliminación Peligrosidad
            if ($("#PorPeligrosidadOrig").val() == 0) {
                alert("No se puede realizar la acción porque no cuenta con Peligrosidad");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorPeligrosidad").val("0");
            $("#filaPeligrosidad").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "48": // Prórroga de ascen. Interino.
        case "49": // Prórroga de descen. Interino.
            ObtenerProrroga(tipo, ced);
            break;
        case "50": // Prórroga permiso con sueldo
        case "51": // Prórroga permuta interino.
        case "52": // Prórroga de traslado interino.
            //ObtenerFechas(tipo, ced);
            break;
        case "53":  // Reajuste de sobresueldos.
            $("#MtoRecargo").attr('readonly', false);
            $("#MtoRecargo").attr('disabled', false);
            MostrarDatosSalario(true);
            break;z
        case "54": // Reasignación
            $("#filaClase").show();
            $("#filaCategoria").show();
            $("#filaEspecialidad").show();
            $("#filaSubespecialidad").show();
            $("#indClase").attr('disabled', false);
            $("#CategoriaSeleccionada").attr('disabled', true);
            MostrarDatosSalario(true);
        case "55":  // Recargo de funciones.
            $("#filaClase").show();
            $("#filaCategoria").show();
            $("#indClase").attr('disabled', false);
            MostrarDatosSalario(true);
            break;
        case "58":  // Traslado interino.
            $("#filaSeccion").show();
            break;
        case "60":  // Reajuste aprobación prohibición.
            if ($("#PorProhOriginal").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Prohibición");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorList").attr('hidden', false);
            $("#PorProhibicion").attr('readonly', true);
            //$("#PorProhibicion").attr('hidden', true);
            var por = $("#PorList").val();
            $("#PorProhibicion").val(por);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "61":  // Reajuste eliminación prohibición.
            if ($("#PorProhOriginal").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Prohibición");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorProhibicion").val("0");
            $("#PorList").attr('hidden', true);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "62":  // Reajuste aprob. Dedic. Exclusiva.
            if ($("#PorDedOriginal").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Dedicación Exclusiva");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorList").attr('hidden', false);
            $("#PorProhibicion").attr('readonly', true);
            $("#PorProhibicion").attr('hidden', true);
            var por = $("#PorList").val();
            $("#PorProhibicion").val(por);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "63":  // Reaj. Eliminación dedic. Exclusiva.
            if ($("#PorDedOriginal").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Dedicación Exclusiva");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorProhibicion").val("0");
            $("#PorList").attr('hidden', true);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "66":  // Reaj. Aprob. Riesgo policial
            if ($("#PorRiesgoOrig").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Riesgo");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorRiesgo").val("18");
            $("#filaRiesgo").attr('hidden', false);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "67":  // Reaj. Eliminación. Riesgo policial
            if ($("#PorRiesgoOrig").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Riesgo");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorRiesgo").val("0");
            $("#filaRiesgo").attr('hidden', false);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "68":  // Reaj. Aprob. Disponibilidad
            if ($("#PorDisponibilidadOrig").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Disponibilidad");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorDisponibilidad").val("25");
            $("#filaDisponibilidad").attr('hidden', false);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "69":  // Reaj. Eliminac. Disponibilidad
            if ($("#PorDisponibilidadOrig").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Disponibilidad");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorDisponibilidad").val("0");
            $("#filaDisponibilidad").attr('hidden', false);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "74":  // Reaj. Por quinquenio
            var porQuinquenio = $("#PorQuinquenioOrig").val();
            porQuinquenio = parseFloat(porQuinquenio.toString().replace(/\,/g, '.'));
            porQuinquenio = porQuinquenio + 5;
            $("#PorQuinquenio").val(porQuinquenio);
            $("#filaQuinquenio").attr('hidden', false);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "78":  // Prórroga de Dedic. Exclusiva
            $("#filaCategoria").show();
            MostrarDatosSalario(true);
            break;
        case "80": // Regreso al puesto en propiedad.
            //ObtenerFechas(tipo, ced);
            break;
        case "81":  // Reaj. Aprobación Consulta Externa
            if ($("#PorConsultaOrig").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Consulta Externa");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorConsulta").val("22");
            $("#filaConsulta").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "82":  // Reaj. Eliminación Consulta Externa
            if ($("#PorConsultaOrig").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Consulta Externa");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorConsulta").val("0");
            $("#filaConsulta").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "83":  // Reaj. Aprobación Bonificación Adicional
            if ($("#PorBonificacionOrig").val() > 0) {
                alert("No se puede realizar la acción porque YA cuenta con Bonificación Adicional");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorBonificacion").val("17");
            $("#filaBonificacion").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "84":  // Reaj. Eliminación Bonificación Adicional
            if ($("#PorBonificacionOrig").val() == 0) {
                alert("No se puede realizar la acción porque NO cuenta con Bonificación Adicional");
                $("#tipoAP")[0].selectedIndex = 0;
                break;
            }
            $("#PorBonificacion").val("0");
            $("#filaBonificacion").show();
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;
        case "88": // Modificación de fecha de A.A  (Aumento Anual)
            $("#MesAum").attr('disabled', false);
            break;
        case "91": // Cambio de especialidad
            $("#filaCategoria").show();
            $("#filaEspecialidad").show();
            $("#filaSubespecialidad").show();            
            $("#CategoriaSeleccionada").attr('disabled', true);
            break;
        case "94":  // Addendum Dedicación Exclusiva
            $("#PorList").attr('hidden', false);
            $("#PorProhibicion").attr('readonly', true);
            $("#PorProhibicion").attr('hidden', true);
            var por = $("#PorList").val();
            $("#PorProhibicion").val(por);
            ActualizarMontos();
            MostrarDatosSalario(true);
            break;

        default:
            break;
    }
}

function MostrarDatosSalario(estado)
{
    if (estado) {
        $("#filaSalBase").show();
        $("#filaMtoAnual").show();
        $("#filaDisfrutado").show();
        $("#filaAutorizado").show();
        $("#filaMtoAnualidades").show();
        $("#filaMtoRecargo").show();
        $("#filaPunto").show();
        $("#filaNumGradoGrupo").show();
        $("#filaMtoGrado").show();
        $("#filaPor").show();
        $("#filaMtoPor").show();
        $("#filaMtoOtros").show();
        $("#filaTotal").show();
        $("#filaQuinquenio").show();
        $("#filaDisponibilidad").show();
        $("#filaRiesgo").show();
        $("#filaGradoPolicial").show();
        $("#filaCarreraPol").show();
        $("#filaCurso").show();
        $("#filaBonificacion").show();
        $("#filaConsulta").show();
    }
    else {
        $("#filaSalBase").hide();
        $("#filaMtoAnual").hide();
        $("#filaMtoAnualidades").hide();
        $("#filaDisfrutado").hide();
        $("#filaAutorizado").hide();
        $("#filaMtoRecargo").hide();
        $("#filaPunto").hide();
        $("#filaNumGradoGrupo").hide();
        $("#filaMtoGrado").hide();
        $("#filaPor").hide();
        $("#filaMtoPor").hide();
        $("#filaMtoOtros").hide();
        $("#filaTotal").hide();
        $("#filaQuinquenio").hide();
        $("#filaDisponibilidad").hide();
        $("#filaRiesgo").hide();
        $("#filaGradoPolicial").hide();
        $("#filaCarreraPol").hide();
        $("#filaCurso").hide();
        $("#filaBonificacion").hide();
        $("#filaConsulta").hide();
    }
}

function ObtenerFechas(idTipo, numCed) {
    $('#urlDetalle').hide();

    $.ajax({
        type: "post",
        url: "/AccionPersonal/GetFechas",
        data: { idTipoAccion: idTipo, numCedula: numCed },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                if (data.fecR != '01/01/0001') {
                    $('#FecUltRige').val(data.fecR);
                    $('#FecRige').val(data.fecRI);
                    $('#FecRigeIntegra').val(data.fecRI);

                    if (data.fecV.indexOf("01/01/0001") < 0)
                    {
                        $('#FecUltVence').val(data.fecV);
                        $('#FecVence').val(data.fecVI);
                        $('#FecVenceIntegra').val(data.fecVI);
                    }
                    else {
                        $('#FecUltVence').val("");
                        $('#FecVence').val("");
                        $('#FecVenceIntegra').val("");
                    }                 
                    
                    $('#CodigoModulo').val("0");
                    $('#CodigoObjetoEntidad').val(data.codObj);

                    //var newurl = "@Url.Action('Details','AccionPersonal')";
                    //$('#urlDetalle').attr('href', newurl + "?numAccion = " + data.numAcc);

                    if (data.numAcc != null)
                    {
                        var newurl = "Details?numAccion=" + data.numAcc;
                        $('#urlDetalle').attr('href', newurl);
                        $('#urlDetalle').show();
                        $("#filaUltimaAccion").show();
                    }
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

    $(function () {
        //$.ajaxSetup({ cache: false });
        //$("a[data-modal]").on("click", function (e) {
        //    $('#detalle-accion').load(this.href, function () {
        //        $('#detalle-accion').modal({
        //            keyboard: true
        //        }, 'show');
        //    });
        //    return false;
        //});

        $("#lk_Detalle").click(function () {
            $.ajax({
                url: '/AccionPesonal/Details',
                data: { numAccion: 1 }
            }).done(function (msg) {
                $("#showModal").html(msg);
                $("#myModal").modal();
            });

        //$("body").on("click", "a", function () {
        //    $.ajax({
        //        url: '/AccionPesonal/Details',
        //        data: { numAccion: 1 }
        //    }).done(function (msg) {
        //        $("#showModal").html(msg);
        //        $("#myModal").modal();
        //    })
        })

        $('#CleanFecVence').click(function () {
            $('#FecVence').val("");
        });

        $('#CleanFecVenceIntegra').click(function () {
            $('#FecVenceIntegra').val("");
        });
    });
}

function ObtenerProrroga(idTipo, numCed) {
    $('#urlDetalle').hide();
    $('#puestoAccion').hide();

    $.ajax({
        type: "post",
        url: "/AccionPersonal/GetFechas",
        data: { idTipoAccion: idTipo, numCedula: numCed },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $('#CodigoModulo').val("0");
                $('#CodigoObjetoEntidad').val(data.codObj);
                if (data.numAcc != null) {
                    // alert('La acción se va a realizar al puesto ' + data.numAcc);
                    $('#puestoAccion').val('La acción se va a realizar al puesto ' + data.numAcc);
                    $('#puestoAccion').show();
                }
                //if (data.codObj == 0) {
                //    if (data.numAcc != null) {
                //        var newurl = "return ObtenerDetalle('" + data.numAcc + "');";
                //        $('#urlDetalle').attr('onclick', newurl);
                //        $('#urlDetalle').show();
                //        $("#filaUltimaAccion").show();
                //    }
                //}
                //else {
                //    $("#filaProrroga").show();
                //}
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error");
        }
    });

    $(function () {
        //$.ajaxSetup({ cache: false });
        //$("a[data-modal]").on("click", function (e) {
        //    $('#detalle-accion').load(this.href, function () {
        //        $('#detalle-accion').modal({
        //            keyboard: true
        //        }, 'show');
        //    });
        //    return false;
        //});

        $("#lk_Detalle").click(function () {
            $.ajax({
                url: '/AccionPesonal/Details',
                data: { numAccion: 1 }
            }).done(function (msg) {
                $("#showModal").html(msg);
                $("#myModal").modal();
            });

            //$("body").on("click", "a", function () {
            //    $.ajax({
            //        url: '/AccionPesonal/Details',
            //        data: { numAccion: 1 }
            //    }).done(function (msg) {
            //        $("#showModal").html(msg);
            //        $("#myModal").modal();
            //    })
        })

        $('#CleanFecVence').click(function () {
            $('#FecVence').val("");
        });

        $('#CleanFecVenceIntegra').click(function () {
            $('#FecVenceIntegra').val("");
        });
    });
}

function CargarDetalle(numAccion) {
    $.ajax({
        type: "Get",
        url: '@Url.Action("Details", "AccionPersonal")',
        data: { id: currentId },
        success: function (data) {
            $('#Preview').html(data);
            $('#modalDetalle').modal('show');
        }
    })
    
    return false;
}

$(document).ready(function () {
    jQuery("#btnBuscarNombre").on("click", function (event) {
        event.preventDefault();
        $("#dvResultadoBusqueda").html('');
    });
});

function ObtenerDetalleH(id) {
    $('#target').load("/AccionPersonal/DetailsH?codigo=" + id);
    $("#detalle-historico").appendTo("body");
    $('#detalle-historico').modal('show');
    return false;
}