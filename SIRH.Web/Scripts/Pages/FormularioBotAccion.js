// MÉTODOS PARA CONTROLAR EL GUARDADO
function Begin() {
    $('#btnAgregar').css("display", "none");
    $("#preloader").css("display", "block");
}

function Complete() {
    $('#preloader').css("display", "none");
    $("#btnAgregar").css("display", "block");
}

function Success() {
    Complete();
    ShowPopUp();
}

function ClearFields() {
    $('#DesAccion').val("");
    $('#URLVideo').val("");
    $("#ModuloSeleccionado").val($("#ModuloSeleccionado option:first").val());
}

// Controlar PopUp
function ShowPopUp() {
    $("#detail-accion").appendTo("body");
    $('#detail-accion').modal('show');
}

$(document).ready(function () {
    $('#form').submit(Begin);
    Complete();
});