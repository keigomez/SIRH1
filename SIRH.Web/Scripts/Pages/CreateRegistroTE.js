var error = false;
var validando = false;
var fileDiv = null;
$(document).ready(function () {
    window.addEventListener("submit", function (e) {
        let form = e.target;
        if (form.getAttribute("enctype") === "multipart/form-data") {
            e.preventDefault();
            BeginValidacion();
            sendFormDetalle(e, form, e.submitter.defaultValue);
        } else if (e.submitter.id === "btnRegistroDoble") {
            e.preventDefault();
            BeginValidacionDoble();
            sendFormDetalleDoble(e, form, e.submitter.defaultValue);
        }
        
    }, true)
});

function sendFormDetalle(e, form, value) {
    e.stopImmediatePropagation();
    var xhr = new XMLHttpRequest();
    xhr.open(form.method, form.action);
    let formData = new FormData(form);
    formData.append("submit", value);
    xhr.send(formData);

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (form.dataset.ajaxUpdate) {
                if (!xhr.responseURL.includes("Saved")) {
                    var updateTarget = $('#dvResultadoBusqueda');
                    if (updateTarget) {
                        let filerow = document.getElementById('filaArchivo');
                        let filediv = filerow.children[1];
                        updateTarget.html(xhr.response);
                        filerow = document.getElementById('filaArchivo');
                        SuccessValidacion();
                        CompleteValidacion();
                        $('form')[1].reset();
                        filerow.replaceChild(filediv, filerow.children[1]);
                        readURL(document.getElementById("file"));
                    }
                } else {
                    window.location.replace(xhr.responseURL);
                }
            } else {
                CompleteValidacion();
            }
        }
    };
}
function sendFormDetalleDoble(e, form, value) {
    e.stopImmediatePropagation();
    var xhr = new XMLHttpRequest();
    xhr.open(form.method, form.action);
    let formData = new FormData(form);
    formData.append("submit", value);
    xhr.send(formData);

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            if (form.dataset.ajaxUpdate) {
                if (!xhr.responseURL.includes("Saved")) {
                    var updateTarget = $('#dvResultadoBusqueda');
                    if (updateTarget) {
                        updateTarget.html(xhr.response);
                        SuccessValidacionDoble();
                        CompleteValidacion();
                        $('form')[1].reset();
                    }
                } else {
                    window.location.replace(xhr.responseURL);
                }
            } else {
                CompleteValidacion();
            }
        }
    };
}
function BeginBusqueda() {
    $('#btnBuscar').hide();
    //$('#btnFueraPeriodo').hide();
    $('#preloader').show();
}

function CompleteBusqueda() {
    $('#preloader').hide();
    $('#btnBuscar').show();
    //$('#btnFueraPeriodo').show();
}

function Calculando() {
    if ($('#btnCalculo').val() == "Calcular") {
        validando = true;
    }
    else {
        validando = false;
        $('#btnCalculo').val('Calcular');
        $('#btnRegistro').attr('disabled', true);
    }
}function CalculandoDoble() {
    if ($('#btnCalculo').val() == "Calcular") {
        validando = true;
    }
    else {
        validando = false;
        $('#btnCalculo').val('Calcular');
        $('#btnRegistroDoble').attr('disabled', true);
    }
}
function BeginValidacionDoble() {
    $('#btn_group').hide();
    $('#btn_group2').hide();
    $('#preloaderRegistro').show();
    $('.deshabilitarCampo').attr('disabled', false);
    $(".datepicker").datepicker("destroy");
}
function BeginValidacion() {
    $('#btn_group').hide();
    $('#preloaderRegistro').show();
    $('.deshabilitarCampo').attr('disabled', false);
}
function CompleteValidacionDoble() {
    $('#preloaderRegistro').hide();
    $('#btn_group').show();
    $('#btn_group2').show();
}
function CompleteValidacion() {
    $('#preloaderRegistro').hide();
    $('#btn_group').show();
}
function ActualizarCampos() {
    let total = $('#TotalPagar');
    if (total && total.val() > 0) {
        validando = true;
        SuccessValidacion();
        $('#btnRegistro').attr('disabled', true);
    } else {
        CambiarDetalle();
    }
}
function SuccessValidacion() {
    if (validando) {
        $('#btnCalculo').val('Modificar');
        $('#btnRegistro').attr('disabled', false);
        $('.hora_minuto').prop('readonly', true);
        $('.jornada:not(:checked)').attr('disabled', true);
        $('.deshabilitarCampo').attr('disabled', true);
        $('#MesActualPago').prop('readonly', true);
        validando = false;
    }
    CambiarDetalle();
}
function SuccessValidacionDoble() {
    if (validando) {
        $('#btnCalculo').val('Modificar');
        $('#btnRegistroDoble').attr('disabled', false);
        $('.hora_minuto').prop('readonly', true);
        $('.jornada:not(:checked)').attr('disabled', true);
        $('#MesActualPago').prop('readonly', true);
        validando = false;
    }
}
function CambiarDetalle() {
    let select = $("#ClaseActual")[0];
    let ocupacion = $('#ocupacion').val();
    let claseReal = $("#claseReal").val();
    if (typeof ocupacion !== 'undefined' && ocupacion !== null && typeof select !== 'undefined' && select !== null && claseReal !== 'undefined' && claseReal !== null) {
        if (ocupacion.toUpperCase().startsWith("GUARDA") || claseReal.toUpperCase().startsWith("OFIC.SEGUR.SERV.CIVIL") || select.options[select.selectedIndex].text.toUpperCase().startsWith("OFIC.SEGUR.SERV.CIVIL")) {
            $('tbody tr td:nth-child(5),thead tr th:nth-child(5)').show();
            $('tbody tr td:nth-child(6),thead tr th:nth-child(6)').show();
            $('tfoot tr td:nth-child(1)').attr('colspan', 6);
            $('.horaGuarda').show();
            $('#horaDiurna').text("Monto por hora diurna: ");
        }
        else {
            $('tbody tr td:nth-child(5),thead tr th:nth-child(5)').hide();
            $('tbody tr td:nth-child(6),thead tr th:nth-child(6)').hide();
            $('tfoot tr td:nth-child(1)').attr('colspan', 4);
            $('.horaGuarda').hide();
            $('#horaDiurna').text("Monto por hora (ordinaria): ");
        }
    }
}

function clearForm() {
    $('form')[1].reset();
}

function readURL(input) {
    if (input.files) {
        if (input.files[0]) {
            $('#pdf_preview').show();
            $('#pdf_preview')[0].href = window.URL.createObjectURL(new Blob([input.files[0]], { "type": "application/pdf" }));
        } else {
            $('#pdf_preview').hide();
            $('#pdf_preview')[0].href = "#";
        }
    }
}