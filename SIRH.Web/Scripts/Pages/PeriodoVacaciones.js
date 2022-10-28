
function BeginCargarPeriodo() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}
function CompleteCargarPeriodo() {
    $('#preloader').hide();
    $('#btnBuscar').show();

}
function SuccessCargarPeriodo() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    //$("#FechaCarga").datepicker(config);
}
function BeginGuardarPeriodo() {
    $('#btnGuardar').hide();
    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarPeriodo() {
    $('#preloader1').hide();
    $('#btnGuardar').show();

}

function SuccessGuardarPeriodo() {
    SuccessCargarReintegro();
}