var error = false;
var validando = false;
$(document).ready(function () {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    $("#FechaI").datepicker(config);
    $("#FechaI").val("");

    $("#FechaT").datepicker(config);
    $("#FechaT").val("");
    $("#FechaC").datepicker(config);
    $("#FechaC").val("");
    $("#FechaR").datepicker(config);
    $("#FechaR").val("");
    $("#FechaF").datepicker(config);
    $("#FechaF").val("");


})
// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

function BeginCargarFuncionario() {
    $('#btnBuscar').hide();
    $('#preloader').removeAttr("hidden");
    $('#preloader').show();
}

function CompleteCargarFuncionario() {
    $('#preloader').hide();
    $('#btnBuscar').show();
    //$('.datepicker').datepicker({
    //    showOn: 'button',
    //    buttonImage: '../Content/Images/calendar.gif',
    //    buttonImageOnly: true,
    //    buttonText: 'Seleccionar fecha'
    //});
}

function SuccessCargarFuncionario() {
    if (updateData) {
        $('.nav-tabs a[href="#tabs-3"]').tab('show');
        updateData = false;
    }
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }
    //Detallecontratacion
    $("#datepickerIngreso").datepicker(config);
}

// METODOS PARA CONTROLAR EL GUARDADO

function BeginGuardarCandidato() {
    $('#btnGuardar').hide();
    $('#preloader1').removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardarCandidato() {
    $('#preloader1').hide();
    $('#btnGuardar').show();
}

function SuccessGuardarCandidato() {
}

var updateData = false;

function update() {
    updateData = true;
    $('#thisForm').submit();

}

function ObtenerCantones(idProvincia) {
    $list = $("#DropCantones");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetCantones",
        data: { idProvincia: idProvincia },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomCanton + '"> ' + data.listado[i].NomCanton + ' </option>');
                }

                ObtenerDistritos(data.listado[0].IdEntidad);
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los cantones");
        }
    });
}

function ObtenerDistritos(nombreCanton) {
    $list = $("#DropDistritos");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetDistritos",
        data: { nombreCanton: nombreCanton },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomDistrito + '"> ' + data.listado[i].NomDistrito + ' </option>');
                }
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los distritos");
        }
    });
}

function ObtenerCantonesContrato(idProvincia) {
    $list = $("#DropCantonescontrato");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetCantones",
        data: { idProvincia: idProvincia },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomCanton + '"> ' + data.listado[i].NomCanton + ' </option>');
                }

                ObtenerDistritos(data.listado[0].IdEntidad);
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los cantones");
        }
    });
}

function ObtenerDistritosContrato(nombreCanton) {
    $list = $("#DropDistritoscontrato");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetDistritos",
        data: { nombreCanton: nombreCanton },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomDistrito + '"> ' + data.listado[i].NomDistrito + ' </option>');
                }
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los distritos");
        }
    });
}

function ObtenerCantonesTrabajo(idProvincia) {
    $list = $("#DropCantonestrabajo");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetCantones",
        data: { idProvincia: idProvincia },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomCanton + '"> ' + data.listado[i].NomCanton + ' </option>');
                }

                ObtenerDistritos(data.listado[0].IdEntidad);
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los cantones");
        }
    });
}

function ObtenerDistritosTrabajo(nombreCanton) {
    $list = $("#DropDistritostrabajo");
    $.ajax({
        type: "post",
        url: "/Desarraigo/GetDistritos",
        data: { nombreCanton: nombreCanton },
        datatype: "json",
        traditional: true,
        success: function (data) {
            if (data.success) {
                $list.empty();
                $list.append('<option value=0> Seleccionar... </option>');
                for (var i = 0; i < data.listado.length; i++) {
                    $list.append('<option value="' + data.listado[i].NomDistrito + '"> ' + data.listado[i].NomDistrito + ' </option>');
                }
            }
            else {
                alert(data.mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar los distritos");
        }
    });
}


