var error = false;
var validando = false;

// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

function BeginCargarFuncionario() {
    $('#btnBuscar').hide();
    //$("#progressbar").progressbar({
    //    value: 100
    //});
    $('#progressbar').removeAttr("hidden");
    $('#progressbar').show();
}

function CompleteCargarFuncionario() {
    $('#progressbar').hide();
    $('#btnBuscar').show();
    //$('.datepicker').datepicker({
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha'
    //});
}

function SuccessCargarFuncionario() 
{
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FecRige").datepicker(config);
    $("#FecVence").datepicker(config);
    $("#FecRigeIntegra").datepicker(config);
    $("#FecVenceIntegra").datepicker(config);

    $("#FecRige").val("");
    $("#FecVence").val("");
    $("#FecRigeIntegra").val("");
    $("#FecVenceIntegra").val("");

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

    //$("#FecRigeIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecVenceIntegra").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVenceIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecRigeIntegra").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

// METODOS PARA CONTROLAR EL GUARDADO

function BeginGuardarBorrador() {
    $('#btnGuardar').hide();
    //$("#progressbarGuardar").progressbar({
    //    value: 100
    //});
    $("#progressbarGuardar").removeAttr("hidden");
    $('#progressbarGuardar').show();
}

function CompleteGuardarBorrador() {
    $('#progressbarGuardar').hide();
    $('#btnGuardar').show();
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FecRige").datepicker(config);
    $("#FecVence").datepicker(config);
    $("#FecRigeIntegra").datepicker(config);
    $("#FecVenceIntegra").datepicker(config);

    $("#FecRige").val("");
    $("#FecVence").val("");
    $("#FecRigeIntegra").val("");
    $("#FecVenceIntegra").val("");
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
    //$("#FecRigeIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecVenceIntegra").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVenceIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecRigeIntegra").datepicker("option", "maxDate", selectedDate);
    //    }
    //});
}

function SuccessGuardarBorrador() {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FecRige").datepicker(config);
    $("#FecVence").datepicker(config);
    $("#FecRigeIntegra").datepicker(config);
    $("#FecVenceIntegra").datepicker(config);

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

    //$("#FecRigeIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecVenceIntegra").datepicker("option", "minDate", selectedDate);
    //    }
    //});
    //$("#FecVenceIntegra").datepicker({
    //    defaultDate: "+1w",
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha',
    //    onSelect: function(selectedDate) {
    //    $("#FecRigeIntegra").datepicker("option", "maxDate", selectedDate);
    //    }
    //});

}
    
function ActualizarCategoria(idCat) {
    $.ajax({
        type: "post",
        url: "/BorradorAccionPersonal/GetCategoria",
        data: { idCategoria: idCat },
        datatype: "json",
        traditional: true,
        success: function(data) {
            if(data.success) {
                $("#MtoSalBase").val(data.salario);
                $("#MtoAnual").val(data.aumento);
            }
            else {
                alert(data.mensaje);
            }
            ActualizarMontos();
        },
        error: function(err) {
            alert("Error");
        }
    });
}

function ActualizarPorcentaje(porc) {
      var valor = parseFloat(porc);
      $("#PorProh").val(valor); 
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
    var monAnual = 0;
    var monRecargo = 0;
    var monProh = 0;
    var monOtro = 0;
    var monPto = 0;
    
    var numAnual = 0;
    var porProh = 0;
    var numPtos = 0;
    var numPtoCarrera = 0;
    
    monSalBase = $("#MtoSalBase").val();
    monSalBase = parseFloat(monSalBase.toString().replace(/\,/g,'.'));
    
    monAnual = $("#MtoAnual").val();
    monAnual = parseFloat(monAnual.toString().replace(/\,/g,'.'));
       
    monRecargo = $("#MtoRecargo").val();
    monRecargo = parseFloat(monRecargo.toString().replace(/\,/g,'.'));
    
    monOtro = $("#MtoOtros").val(); 
    monOtro = parseFloat(monOtro.toString().replace(/\,/g,'.'));
    
    numPtos = $("#NumPto").val();
    numPtos = parseFloat(numPtos.toString().replace(/\,/g,'.'));
    
    numPtoCarrera = $("#NumPtoCarrera").val();
    numPtoCarrera = parseFloat(numPtoCarrera.toString().replace(/\,/g,'.'));
    
    porProh = $("#PorProh").val(); 
    
    if (porProh == null|| porProh == undefined) { 
        porProh = $('#PorList').val();
    }
         
    porProh = parseFloat(porProh.toString().replace(/\,/g,'.'));
    
    numAnual = $("#NumAnualidad").val();

    monAnual = monAnual * numAnual;
    monProh = monSalBase * porProh /100;
    monPto = numPtos * numPtoCarrera;
    
    total = monSalBase + monAnual + monRecargo + monPto + monProh + monOtro;
    
    $("#MtoProh").val(monProh.toLocaleString('es-CR', {minimumFractionDigits: 2}).replace(/\./g,''));
    $("#MtoAnuales").val(monAnual.toLocaleString('es-CR', {minimumFractionDigits: 2}).replace(/\./g,''));
    $("#MtoPtos").val(monPto.toLocaleString('es-CR', {minimumFractionDigits: 2}).replace(/\./g,''));
    $("#total").val(formatter.format(total.toString()));
}