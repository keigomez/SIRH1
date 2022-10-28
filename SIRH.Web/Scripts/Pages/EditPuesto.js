function SuccessCargarPuesto() {
}

function BeginCargarPuesto() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteCargarPuesto() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
}