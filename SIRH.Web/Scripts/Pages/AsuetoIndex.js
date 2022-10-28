function BeginAsueto() {
    $("#btnBuscarDiasAsueto").css("display", "none");
    $("#preloaderDiasAsueto").css("display", "block");
}

function SuccessAsueto() {
    $("#preloaderDiasAsueto").css("display", "none");
    $("#btnBuscarDiasAsueto").css("display", "block");
}