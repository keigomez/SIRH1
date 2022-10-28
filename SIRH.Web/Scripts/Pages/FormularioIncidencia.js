// MÉTODOS PARA CONTROLAR EL GUARDADO
function BeginGuardarIncidencia() {
    $('#btnAgregar').css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteGuardarIncidencia() {
    $('#preloader').css("display", "none");
    $("#btnAgregar").css("display", "block");
}

function SuccessGuardarIncidencia() {
    CompleteGuardarIncidencia();
    ShowPopUp();
}

// MÉTODO PARA CONTROLAR LA IMAGEN
$(document).ready(function () {
    $('#ImagenError').on('change', function () {
        var fileName = $(this).val();
        fileName = fileName.replace("C:\\fakepath\\", "");
        $(this).next('.custom-file-label').html(fileName);
        $('#Imagen').val(fileName);
    });
    $('#ErrorComun').on('change', ShowErrorArea);
    ShowErrorArea();
    $('#form').submit(BeginGuardarIncidencia);
    CompleteGuardarIncidencia();
});

function sendForm(e, form) {
    e.stopImmediatePropagation();
    var xhr = new XMLHttpRequest();
    xhr.open(form.method, form.action);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (form.dataset.ajaxUpdate) {
                var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                if (updateTarget) {
                    updateTarget.innerHTML = xhr.responseText;
                }
            }
        }
    };
    xhr.send(new FormData(form));
}

// Controlar PopUp
function ShowPopUp() {
    $('#ErrorComun').find('option:eq(0)').prop('selected', true);
    $('#Incidencia_Error').val('');
    $("#detail-incidencia").appendTo("body");
    $('#detail-incidencia').modal('show');
}

// Controlar el campo de texto de error
function ShowErrorArea(e) {
    if ($('#ErrorComun :selected').text() != 'El sistema muestra un error desconocido') {
        $('#Errorlbl').css("display", "none");
        $('#Error').css("display", "none");
    }
    else {
        $('#Errorlbl').css("display", "block");
        $('#Error').css("display", "block");
    }
}