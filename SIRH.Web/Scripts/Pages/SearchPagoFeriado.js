$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $('#FechaTramiteDesde').datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaTramiteDesde").val();
    $('#FechaTramitenHasta').datepicker(config2);

    $('#FechaTramiteDesde').val("");
    $('#FechaTramitenHasta').val("");
});

function BeginSearch() {
    $('#btnBuscar').hide();
    $('#preloader').show();
}

function CompleteSearch() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}