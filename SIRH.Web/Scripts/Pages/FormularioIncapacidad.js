var error = false;
var validando = false;

// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

function BeginCargarFuncionario() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteCargarFuncionario() {
    //$("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function SuccessCargarFuncionario() 
{
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecRige').datepicker(config);
    $('#FecVence').datepicker(config);

    //$('#FecRige').datepicker().on('changeDate', function (e) {
    //    var minDate = new Date(e.date.valueOf());
    //    $('#FecVence').datepicker('setStartDate', e);
    //});

    //$('#FecVence').datepicker().on('changeDate', function (e) {
    //    var maxDate = new Date(e.date.valueOf());
    //    $('#FecRige').datepicker('setEndDate', e);
    //});

    $("#filaNumCaso").hide();
    //$("#FecRige").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FecVence").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVence").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FecRige").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

// METODOS PARA CONTROLAR EL GUARDADO

function BeginGuardarIncapacidad() {
    $('#btnGuardar').css("display", "none");
    $("#preloaderGuardar").css("display", "block");
}

function CompleteGuardarIncapacidad() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecRige').datepicker(config);
    $('#FecVence').datepicker(config);


    //$('#FecRige').datepicker().on('changeDate', function (e) {
    //    var minDate = new Date(e.date.valueOf());
    //    $('#FecVence').datepicker('setStartDate', e);
    //});

    //$('#FecVence').datepicker().on('changeDate', function (e) {
    //    var maxDate = new Date(e.date.valueOf());
    //    $('#FecRige').datepicker('setEndDate', e);
    //});

    //$('#progressbarGuardar').hide();
    //$('#btnGuardar').show();
    //$("#FecRige").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FecVence").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVence").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FecRige").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

function SuccessGuardarIncapacidad() {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FecRige').datepicker(config);
    $('#FecVence').datepicker(config);


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
    //    onSelect: function(selectedDate) {
    //        $("#FecVence").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVence").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //        $("#FecRige").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
    
}


function ObtenerPorc() {
    var id = $("#Entidad").val();
    var idTipo = $("#idTipo").val();
    var fechaRige = $('#FecRige').val();
    var fechaVence = $('#FecVence').val();
    var montoSalario = $('#MontoSalarioBruto').val();
    var diasOrigen = $('#DiasOrigen').val();

    $("#btnGuardarProrroga").css("display", "none");

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
        url: "/RegistroIncapacidad/GetPorcentajes",
        data: { idTipo: idTipo, idEntidad: id, diasOrigen: diasOrigen, fecRige: fechaRige, fecVence: fechaVence, montoSalario: montoSalario },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                //$('#PorSubsidio').val(data.porc);

                var NumDias = 0;
                var FecRige = 0;
                var PorSubsidio = 0;
                var MtoSubsidio = 0;
                var MtoTotalSubsidio = 0;
                var PorRebaja = 0;
                var MtoRebaja = 0;
                var MtoTotalRebaja = 0;

                $("#tabla").html('');
                $("#tabla").append('<tr><th class="text-right">#</th><th class="text-right">Día</th><th class="text-right">Por. Subsidio</th><th class="text-right">Mto Subsidio</th><th class="text-right">Por. Rebajo</th><th class="text-right">Mto Rebajo</th></tr>');

                for (var i = 0; i < data.porc.length; i++) {
                    NumDias = data.porc[i].NumDia;
                    FecRige = data.porc[i].FecRige;
                    PorSubsidio = data.porc[i].PorSubsidio.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, '');
                    MtoSubsidio = formatter.format(data.porc[i].MtoSubsidio);
                    MtoTotalSubsidio += data.porc[i].MtoSubsidio;
                    PorRebaja = data.porc[i].PorRebaja.toLocaleString('es-CR', { minimumFractionDigits: 2 }).replace(/\./g, '');
                    MtoRebaja = formatter.format(data.porc[i].MtoRebaja);
                    MtoTotalRebaja += data.porc[i].MtoRebaja;
                    $("#tabla").append("<tr><td align='right'> " + NumDias  + "</td><td align='right'>" + FecRige + "</td><td align='right'>" + PorSubsidio + "</td><td align='right'>" + MtoSubsidio + "</td><td align='right'>" + PorRebaja + "</td><td align='right'>" + MtoRebaja + " </td></tr>");
                    $("#btnGuardarProrroga").css("display", "block");
                }
                $("#MtoTotalSubsidio").val(formatter.format(MtoTotalSubsidio));
                $("#MtoTotalRebaja").val(formatter.format(MtoTotalRebaja));
                $("#TotalDias").val(NumDias);
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


function ObtenerEntidadMedica(idTipo) {
    LimpiarDetalle();
    $("#idTipo").val(idTipo);
    $.ajax({
        type: "post",
        url: "/RegistroIncapacidad/GetEntidad",
        data: { idTipo: idTipo },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $("#Entidad").val(data.idEntidad);
                if (data.idEntidad == 1) { //CCSS
                    ObtenerPorc(idTipo);
                    $("#filaNumCaso").hide();
                    $("#CodNumeroCaso").val('');
                }
                else { // INS
                    $("#filaNumCaso").show();
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

function LimpiarDetalle() {
    $("#detalle").html('');
    var fechaRige = $('#FecRige').val();
    var fechaVence = $('#FecVence').val();

    //if (fechaRige > fechaVence)
    //    $('#FecVence').val(fechaRige);

}