$(document).ready(function () {
  
        var config = {
            locale: 'es-es',
            uiLibrary: 'bootstrap4',
            format: 'dd/mm/yyyy'
        }

        $("#FechaI").datepicker(config);
        $('#FechaF').datepicker(config);
        $("#FechaI").val("");
        $('#FechaF').val("");

        $('input[type=radio][name=TipoReporte]').change(function () {
            switch (this.value) {
                case 'MC':
                case 'CF':
                    $("#DepartamentoReporte").hide();
                    $("#FechaReporte").show();
                    $("#FuncionarioReporte").show();
                    $("#SinMarcasReporte").show();
                    break;
                case 'CD':
                    $("#DepartamentoReporte").show();
                    $("#FechaReporte").show();
                    $("#FuncionarioReporte").hide();
                    $("#SinMarcasReporte").show();
                    break;
                case 'CDP':
                    $("#DepartamentoReporte").show();
                    $("#FechaReporte").hide();
                    $("#FuncionarioReporte").hide();
                    $("#SinMarcasReporte").hide();
                    break;
            }
        });

        $("#DepartamentoReporte").hide();
        $("#FechaReporte").show();
        $("#FuncionarioReporte").show();
        $("#SinMarcasReporte").show();

});


//$(function () {
//    $("#FechaI").datepicker({
//        defaultDate: null,
//        showOn: 'button',
//        buttonImage: '../Content/Images/calendar.gif',
//        buttonImageOnly: true,
//        changeMonth: true,
//        changeYear: true,
//        buttonText: 'Seleccionar fecha',
//        onSelect: function (selectedDate) {
//            $("#FechaF").datepicker("option", "minDate", selectedDate);
//        }
//    });

//    $("#FechaF").datepicker({
//        defaultDate: "+0d",
//        showOn: 'button',
//        buttonImage: '../Content/Images/calendar.gif',
//        buttonImageOnly: true,
//        changeMonth: true,
//        changeYear: true,
//        buttonText: 'Seleccionar fecha',
//        onSelect: function (selectedDate) {
//            $("#FechaI").datepicker("option", "maxDate", selectedDate);
//        }
//    });

//    $('.datepicker').val("");

//    $('input[type=radio][name=TipoReporte]').change(function () {
//        switch (this.value) {
//            case 'MC':
//            case 'CF':
//                $("#DepartamentoReporte").hide();
//                $("#FechaReporte").show();
//                $("#FuncionarioReporte").show();
//                $("#SinMarcasReporte").show();
//                break;
//            case 'CD':
//                $("#DepartamentoReporte").show();
//                $("#FechaReporte").show();
//                $("#FuncionarioReporte").hide();
//                $("#SinMarcasReporte").show();
//                break;
//            case 'CDP':
//                $("#DepartamentoReporte").show();
//                $("#FechaReporte").hide();
//                $("#FuncionarioReporte").hide();
//                $("#SinMarcasReporte").hide();
//                break;
//        }
//    });

//    $("#DepartamentoReporte").hide();
//    $("#FechaReporte").show();
//    $("#FuncionarioReporte").show();
//    $("#SinMarcasReporte").show();

//})

function validation() {
    var type = $('input[name="TipoReporte"]:checked').val();
    var C = "#respuestas";
    $(C).text("");

    var pintar = function (C, E) {
        //$(C).attr('class', 'validation-summary-errors');
        $(C).removeAttr("hidden");
        $(C).text($(C).text() + E);
        return true;
    };

    var valiFechaI = function () {
        var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
        var valor = $("#FechaI").val();
        var res = false;
        if (!date_regex.test(valor))
            res = pintar(C, "La fecha no puede ser vacia o tener un formato distinto de dd/mm/yyyy.\n");
        else
            if (parseInt(valor.split("/")[2]) <= 1936)
                res = pintar(C, "El año tiene que ser valido.\n");
        return res;
    }

    var valiCedula = function () {
        var valor = $("#Funcionario_Cedula").val();
        if (valor == undefined || valor == null || valor.length == 0)
            return pintar(C, "La cédula es requerida.\n");
    }

    var valiDepartamento = function () {
        var valor = $("#DepartamentosSeleccion").val();
        if (valor == undefined || valor == null || valor == "")
            return pintar(C, "El departamento es requerida.\n");
    }

    switch (type) {
        case "MC":
        case "CF": return valiFechaI() || valiCedula(); break;
        case "CD": return valiFechaI() || valiDepartamento(); break;
        case "CDP": return valiDepartamento(); break;
        default:
            return pintar(C, "El tipo del reporte es obligatorio.\n");
            break;
    }
}

function dataToPDF() {
    if (validation())
        return;
    $('form#thisForm').submit();
}
