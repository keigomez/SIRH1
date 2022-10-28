

$(document).ready(function () {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#FechaInicioVacaciones").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioVacaciones").val();
    $("#FechaFinalVacaciones").datepicker(config2);


    $("#FechaInicioVacaciones").val("");
    $("#FechaFinalVacaciones").val("");


});
function CompleteInfo() {
    $('#preloader').hide();
    $('#btnBuscar').show();

}
function BeginCargarInfo() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}
function CleanSearch() {
    $('form').get(0).reset();
    $("#FechaInicioVacaciones").val("");
    $("#FechaFinalVacaciones").val("");
    ObtenerCantones(0);
}