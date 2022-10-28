function BeginCargarFuncionario() {
    $('#preloader').show();
    $("#btnBuscar").hide();
}

function CompleteCargarFuncionario() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}


function BeginGuardarPuntos() {
    $('#btnGuardar').hide();
    if ($('#preloaderGuardar').has("hidden"))
        $('#preloaderGuardar').removeAttr("hidden");
    $('#preloaderGuardar').show();
}

function CompleteGuardarPuntos() {
    $('#preloaderGuardar').hide();
    $('#btnGuardar').show();
}