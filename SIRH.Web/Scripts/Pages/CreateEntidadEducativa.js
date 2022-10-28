function BeginSearch() {
    $("#btnGuardar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSearch() {
    $("#preloader").css("display", "none");
    $("#btnGuardar").css("display", "block");
}