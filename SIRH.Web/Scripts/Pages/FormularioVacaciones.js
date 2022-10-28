
function BeginCargarVacaciones() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}
function CompleteCargarVacaciones() {
    $('#preloader').hide();
    $('#btnBuscar').show();

}
function SuccessCargarVacaciones() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FechaRige").datepicker(config);
    $("#FechaVence").datepicker(config);
}
function BeginGuardarVacaciones() {
    $('#btnGuardar').hide();
    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarVacaciones() {
    $('#preloader1').hide();
    $('#btnGuardar').show();

}

function SuccessGuardarVacaciones() {
    SuccessCargarVacaciones();
}