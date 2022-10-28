function BeginBuscar() {
    $('#preloader').show();
    $("#btnBuscar").hide();
    $("#btnAgregar").hide();
    $("#btnRemover").hide();
    $("#btnGenerar").hide();
}

function CompleteBuscar() {
    $('#preloader').hide();
    $('#btnBuscar').show();
    $('#btnAgregar').show();
    $('#btnGenerar').show();
}


