$(document).ready(function () {  
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    //$('#FecEmisionDesde').datepicker(config);
    //var config2 = Object.assign({}, config);
    //config2["minDate"] = () => $("#FecEmisionDesde").val();
    //$('#FecEmisionHasta').datepicker(config2);

    $('#FecVenceDesde').datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FecVenceDesde").val();
    $('#FecVenceHasta').datepicker(config3);

    $('#FecBitacoraDesde').datepicker(config);
    $('#FecBitacoraHasta').datepicker(config);

    $('#FecEmisionDesde').val("");
    $('#FecEmisionHasta').val("");
    $('#FecVenceDesde').val("");
    $('#FecVenceHasta').val("");
    $('#FecBitacoraDesde').val("");
    $('#FecBitacoraHasta').val("");
});

function BeginSearch() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function BeginSearchNotificacion() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearchNotificacion() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}