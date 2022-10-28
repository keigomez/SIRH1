function BeginSalario() {
    $("#btnAgregar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteSalario() {
    $("#preloader").css("display", "none");
    $("#btnAgregar").css("display", "block");
}