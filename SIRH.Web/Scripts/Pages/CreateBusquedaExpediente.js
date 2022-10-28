$(document).ready(function () {
    $('#preloaderBuscar').css("display", "none");
});

function BeginBusquedaExpediente() {
    $("#btnBuscar").css("display", "none");
    $("#preloaderBuscar").css("display", "block");
}


function CompleteBusquedaExpediente() {
    $('#preloaderBuscar').css("display", "none");
    $("#btnBuscar").css("display", "block");
}

function SuccesBusquedaExpediente() {

}