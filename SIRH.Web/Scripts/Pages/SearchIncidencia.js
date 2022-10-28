$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FechaInicioDesde').datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioDesde").val();
    $('#FechaInicioHasta').datepicker(config2);

    $('#FechaFinDesde').datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FechaFinDesde").val();
    $('#FechaFinHasta').datepicker(config3);

    $('#FechaInicioDesde').val("");
    $('#FechaInicioHasta').val("");
    $('#FechaFinDesde').val("");
    $('#FechaFinHasta').val("");    
});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}

// Controlar PopUp Details
function ShowPopUp() {
    $("#detail-incidencia").appendTo("body");
    $('#detail-incidencia').modal('show');
    $("#data-incidencia").css("display", "none");
    $("#preloader1").css("display", "block");
}

function FullPopUp() {
    $("#preloader1").css("display", "none");
    $("#data-incidencia").css("display", "block");
}

function HidePopUp() {
    $("#detail-incidencia .close").click();
}

// Controlar PopUp Edit
function ShowPopUpEdit() {
    $("#preloader1").css("display", "block");
    $("#data-incidencia").css("display", "none");
}

function FullPopUpEdit() {
    $("#preloader1").css("display", "none");
    $("#data-incidencia").css("display", "block");
}

function HidePopUpEdit() {
    $("#edit-incidencia .close").click()
}

function ReShowPopUp() {
    HidePopUp();
}

// Controlar guardar el motivo de rechazo
function BeginGuardar() {
    $('#GuardarIncidenciaPopUp').css("display", "none");
    $('#CancelarIncidenciaPopUp').css("display", "none");
    $("#preloader3").css("display", "block");  
    ShowMotivoArea();
}

function CompleteGuardar() {    
    $('#preloader3').css("display", "none");
    $("#GuardarIncidenciaPopUp").css("display", "block");
    $("#CancelarIncidenciaPopUp").css("display", "block");    
}

function SuccessGuardar() {
    CompleteGuardar();
    ShowMotivoArea();
    FullPopUp();
}

// Controlar el campo de texto de motivo de rechazo
function ShowMotivoArea(e) {
    if ($('#MotivoComun :selected').text() != 'Otro') {
        $('#Motivolbl').css("display", "none");
        $('#Motivo').css("display", "none");
    }
    else {
        $('#Motivolbl').css("display", "block");
        $('#Motivo').css("display", "block");
    }
}