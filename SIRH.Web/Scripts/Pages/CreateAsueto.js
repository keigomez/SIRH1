$(document).ready(function () {
    $("#MesSeleccionado").change(function () {
        var mes = this.value;
        if (mes == "Febrero") {
            $('#Dias1').hide();
            $('#Dias2').show();
            $('#Dias3').hide();
        }
        if (mes == "Enero" || mes == "Marzo" || mes == "Mayo" || mes == "Julio" || mes == "Agosto" || mes == "Octubre" || mes == "Diciembre") {
            $('#Dias1').show();
            $('#Dias2').hide();
            $('#Dias3').hide();
        }
        if (mes == "Abril" || mes == "Junio" || mes == "Setiembre" || mes == "Noviembre") {
            $('#Dias1').hide();
            $('#Dias2').hide();
            $('#Dias3').show();
        }
    });
});


function BeginAsueto() {
    $("#btnAgregar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteAsueto() {
    $("#preloader").css("display", "none");
    $("#btnAgregar").css("display", "block");
}
