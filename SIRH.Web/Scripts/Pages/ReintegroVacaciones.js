
function BeginCargarReintegro() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}
function CompleteCargarReintegro() {
    $('#preloader').hide();
    $('#btnBuscar').show();

}
function SuccessCargarReintegro() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FechaRige").datepicker(config);
    $("#FechaVence").datepicker(config);
}
function BeginGuardarReintegro() {
    $('#btnGuardar').hide();
    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarReintegro() {
    $('#preloader1').hide();
    $('#btnGuardar').show();

}

function SuccessGuardarReintegro() {
    SuccessCargarReintegro();
}