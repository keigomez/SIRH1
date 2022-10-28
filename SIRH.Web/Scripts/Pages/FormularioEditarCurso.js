$(document).ready(function () {
    window.addEventListener("submit", function (e) {
        var form = e.target;
        if (form.getAttribute("enctype") === "multipart/form-data") {
            BeginEditarCurso();
            if (document.getElementById("CursoCapacitacion_DescripcionCapacitacion") != null) {
                if ($("#Resolucion").val() != ""
                        || $("#ImagenTitulo").val() != "") {
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
                else {
                    CompleteEditarCurso();
                    return;
                }
            }
            else {
                if ($("#Resolucion").val() != ""
                        || $("#ImagenTitulo").val() != "") {
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
                else {
                    CompleteEditarCurso();
                    return;
                }
            }
        }
    }, true);

    $('#ImagenTitulo').on('change', function () {
        var fileName = $(this).val();
        fileName = fileName.replace("C:\\fakepath\\", "");
        $(this).next('.custom-file-label').html(fileName);
    })
});

function BeginEditarCurso() {
    $('#btnEditar').hide();
    $('#preloader').show();
}

function CompleteEditarCurso() {
    $('#preloader').hide();
    $('#btnEditar').show();
}