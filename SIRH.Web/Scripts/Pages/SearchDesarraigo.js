$(document).ready(function () {

    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    $("#FechaInicioDesarraigoI").datepicker(config);
    var config2 = Object.assign({}, config);
    config2["minDate"] = () => $("#FechaInicioDesarraigoI").val();
    $("#FechaFinalDesarraigoI").datepicker(config2);

    $("#FechaInicioDesarraigoF").datepicker(config);
    var config3 = Object.assign({}, config);
    config3["minDate"] = () => $("#FechaInicioDesarraigoF").val();
    $("#FechaFinalDesarraigoF").datepicker(config3);

    $("#FechaInicioDesarraigoI").val("");
    $("#FechaInicioDesarraigoF").val("");
    $("#FechaFinalDesarraigoI").val("");
    $("#FechaFinalDesarraigoF").val("");

});

function BeginSearch() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}

function CompleteSearch() {
    $('#preloader').hide();
    $('#btnBuscar').show();
}

function CleanSearch() {
    $('form').get(0).reset();
    $("#FechaInicioDesarraigoI").val("");
    $("#FechaInicioDesarraigoF").val("");
    $("#FechaFinalDesarraigoI").val("");
    $("#FechaFinalDesarraigoF").val("");
    ObtenerCantones(0);
}

function ExportarAPdf() {
    $('form#thisForm').submit();
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
                $list.append('<option value=0> Cantón </option>');
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
                $list.append('<option value=0> Distrito </option>');
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
