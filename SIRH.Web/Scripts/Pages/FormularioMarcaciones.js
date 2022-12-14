$(document).ready(function () {
    $("#btnPopup").click(function () {
        $("#modal").appendTo("body");
        $('#modal').modal('show')
        return false;
    });

});
/*Funciones Ajax */
function BeginGuardar() {
    $('#btnGuardar').hide();
    $("#preloader1").removeAttr("hidden");
    $('#preloader1').show();
}

function CompleteGuardar() {
    $('#btnGuardar').show();
    $('#preloader1').hide();

}

function SuccessGuardar() {
    $('#btnGuardar').show();
    $('#preloader1').hide();

}

/*Agregar un dispositivo a la tabla de dispositivos asignados*/
function CargarDato(descripcion, ubicacion, cedula, nombre, apellidoPaterno, apellidoMaterno, id) {

    var tamanioTabla = $('#dispositivos >tbody >tr').length;
    var aux = 1;

    if (tamanioTabla > 2) {
        for (var i = 2; i < tamanioTabla; i++) {
            if (document.getElementById('dispositivos').rows[i].cells[0].innerHTML.trim() === descripcion) {
                $("#dialogConf").dialog({
                    title: "Atención",
                    modal: true,
                    buttons: {
                        "Aceptar": function () {
                            //$(this).dialog("close");
                            $('#modal').modal('hide');
                        }
                    }
                });
                aux = 0
            }
        }
    }
    if (aux == 1) {
        var index = (parseInt(tamanioTabla)) - 2;
        var hidden = "<input type='hidden' name='Model.ListaDispositivos[" + index + "].IdEntidad' value='" + id + "' />";
        var hidden2 = "<input type='hidden' name='Model.FuncionarioAux[" + index + "].Cedula' value='" + cedula + "' />" +
	"<input type='hidden' name='Model.FuncionarioAux[" + index + "].Nombre' value='" + nombre + "' />" +
	"<input type='hidden' name='Model.FuncionarioAux[" + index + "].PrimerApellido' value='" + apellidoPaterno + "' />" +
	"<input type='hidden' name='Model.FuncionarioAux[" + index + "].SegundoApellido' value='" + apellidoMaterno + "' />";

        var html =
	"<tr>" +
	"<td> " + descripcion + "</td>" +
	"<td> " + ubicacion + hidden + " " + hidden2 + "</td>" +
	"</tr>";

        $('#dispositivos').append(html);
    }
    return false;
}
