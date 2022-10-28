
var errorDivision = false;
var errorCostos = false;
var errorDireccion = false;
var errorDepartamento = false;
var errorSeccion = false;
var errorPresupuesto = false;
var errorUbicacion = false;

$(document).ready(function () {
    /****************************************************************************************
                                    CODIGOS DIVISIÓN
    ****************************************************************************************/
    $('#dialog-division').click(function () {
        $("#buscar-division").appendTo("body");
        $('#buscar-division').modal('show');
        return false;
    });
    $("#dialog-division").button({
        label: 'Buscar por división',
        text: false
    })
    $("#clean-division").button({
        label: 'Limpiar búsqueda',
        text: false
    }).click(function () {
        $('#coddivision').val('');
    })

    /****************************************************************************************
                                    DIÁLOGO PARÁMETROS Y GRUPOS
    ****************************************************************************************/
    $('#dialogCampos').click(function () {
        $("#campos").appendTo("body");
        $('#campos').modal('show');
        alert("entró");
        return false;
    });
    $("#dialogCampos").button({
        label: 'Seleccionar grupos y parámetros',
        text: false
    })

    /****************************************************************************************
                                        CODIGOS DIRECCIÓN
    ****************************************************************************************/
    $('#dialog-direccion').click(function () {
        $("#buscar-direccion").appendTo("body");
        $('#buscar-direccion').modal('show');
        return false;
    });
    $("#dialog-direccion").button({
        label: 'Buscar por dirección',
        text: false
    })
    $("#clean-direccion").button({
        label: 'Limpiar búsqueda',
        text: false
    }).click(function () {
        $('#coddireccion').val('');
    })

    /****************************************************************************************
                                        CODIGOS DEPARTAMENTO
    ****************************************************************************************/
    $('#dialog-departamento').click(function () {
        $("#buscar-departamento").appendTo("body");
        $('#buscar-departamento').modal('show');
        return false;
    });
    $("#dialog-departamento").button({
        label: 'Buscar por departamento',
        text: false
    })
    $("#clean-departamento").button({
        label: 'Limpiar búsqueda',
        text: false
    }).click(function () {
        $('#coddepartamento').val('');
    })
    /****************************************************************************************
                                            CODIGOS SECCIÓN
    ****************************************************************************************/
    $('#dialog-seccion').click(function () {
        $("#buscar-seccion").appendTo("body");
        $('#buscar-seccion').modal('show');
        return false;
    });
    $("#dialog-seccion").button({
        label: 'Buscar por sección',
        text: false
    })
    $("#clean-seccion").button({
        label: 'Limpiar búsqueda',
        text: false
    }).click(function () {
        $('#codseccion').val('');
    })

    /****************************************************************************************
                               CODIGOS SECCIÓN + UBICACIÓN ADMINISTRATIVA
    ****************************************************************************************/
    //$('#dialog-seccion-ubicacion').click(function () {
    //    $("#buscar-seccion-ubicacion").appendTo("body");
    //    $('#buscar-seccion-ubicacion').modal('show');
    //    return false;
    //});
    //$("#dialog-seccion-ubicacion").button({
    //    label: 'Buscar por sección',
    //    text: false
    //})
    //$("#clean-seccion-ubicacion").button({
    //    label: 'Limpiar búsqueda',
    //    text: false
    //}).click(function () {
    //    $('#codseccion').val('');
    //})
    /****************************************************************************************
                                            CODIGO PRESUPUESTARIO
    ****************************************************************************************/
    $('#dialog-presupuesto').click(function () {
        $("#buscar-presupuesto").appendTo("body");
        $('#buscar-presupuesto').modal('show');
        return false;
    });
    $("#dialog-presupuesto").button({
        label: 'Buscar por Código Presupuestario',
        text: false
    })
    $("#clean-presupuesto").button({
        label: 'Limpiar búsqueda',
        text: false
    }).click(function () {
        $('#codpresupuesto').val('');
    })
});

/****************************************************************************************
                                CODIGOS CODIGO CENTRO COSTOS
****************************************************************************************/
$('#dialog-costos').click(function () {
    $("#buscar-costos").appendTo("body");
    $('#buscar-costos').modal('show');
    return false;
});
$("#dialog-costos").button({
    label: 'Buscar por Código Costos',
    text: false
})
$("#clean-costos").button({
    label: 'Limpiar búsqueda',
    text: false
}).click(function () {
    $('#codcostos').val('');
})

/****************************************************************************************
                                CODIGOS UBICACIÓN CONTRATO-TRABAJO
****************************************************************************************/
$('#edit_contrato').click(function () {
    $("#buscar-contrato").appendTo("body");
    $('#buscar-contrato').modal('show');
    return false;
});
$("#edit_contrato").button({
    label: 'Ubicación Contrato',
    text: false
})
$('#edit_trabajo').click(function () {
    $("#buscar-trabajo").appendTo("body");
    $('#buscar-trabajo').modal('show');
    return false;
});
$("#edit_trabajo").button({
    label: 'Ubicación Contrato',
    text: false
})


/****************************************************************************************
                                CODIGOS UBICACION ADMINISTRATIVA
****************************************************************************************/
$('#dialog-ubicacion-admin').click(function () {
    $("#buscar-ubicacion-admin").appendTo("body");
    $('#buscar-ubicacion-admin').modal('show');
    return false;
});
$("#dialog-ubicacion-admin").button({
    label: 'Buscar por sección',
    text: false
})
$("#clean-ubicacion-admin").button({
    label: 'Limpiar búsqueda',
    text: false
}).click(function () {
    $('#codseccion').val('');
})

function beginDataUbicacion() {
    if ($('#codigoseccion').val().length < 1) {
        errorUbicacion = true;
        $('#error-ubicacion-admin').removeAttr("hidden");
        $('#error-ubicacion-admin').show();
        $('#error-ubicacion-admin').html("Debe digitar el código de la sección");
        $('#target-ubicacion-admin').hide();
    }
    else {
        errorUbicacion = false;
        $('#error-ubicacion-admin').hide();
        $("#progressbarubicacion").removeAttr("hidden");
        $('#progressbarubicacion').show();
        $("#btnBuscaUbicacion").hide();
    }
}

function successDataUbicacion() {
    // Animate
    $('#progressbarubicacion').hide();
    $("#btnBuscaUbicacion").show();
    if (!errorUbicacion) {
        $('#target-ubicacion-admin').show();
    }
}




/****************************************************************************************
                                CODIGOS DIVISIÓN
****************************************************************************************/

function beginDataDivision() {
    if ($('#codigodivision').val().length < 1 &&
        $('#nomdivision').val().length < 1) {
        errorDivision = true;
        $('#error-division').removeAttr("hidden");
        $('#error-division').show();
        $('#error-division').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target-division').hide();
    }
    else {
        errorDivision = false;
        $('#error-division').hide();
        $("#progressbardivision").removeAttr("hidden");
        $('#progressbardivision').show();
        $("#btnBuscaDivision").hide();
    }
}

function successDataDivision() {
    // Animate
    $('#progressbardivision').hide();
    $("#btnBuscaDivision").show();
    if (!errorDivision) {
        $('#target-division').show();
    }
}

//$(function () {
//    $('#buscar-division').dialog({
//        autoOpen: false,
//        modal: true,
//        height: 400,
//        width: 600
//    });

//    $('#dialog-division').click(function () {
//        $('#buscar-division').dialog('open');
//        return false;
//    });
//});

//$(function () {
//    $("#dialog-division").button({
//        label: 'Buscar por división',
//        icons: {
//            primary: 'ui-icon-search'
//        },
//        text: false
//    })
//});

//$(function () {
//    $("#clean-division").button({
//        label: 'Limpiar búsqueda',
//        icons: {
//            primary: 'ui-icon-arrowrefresh-1-w'
//        },
//        text: false
//    }).click(function () {
//        $('#coddivision').val('');
//    })
//});

/****************************************************************************************
                                    CODIGOS DIRECCIÓN
****************************************************************************************/

function beginDataDireccion() {
    if ($('#codigodireccion').val().length < 1 &&
        $('#nomdireccion').val().length < 1) {
        errorDireccion = true;
        $('#error-direccion').removeAttr("hidden");
        $('#error-direccion').show();
        $('#error-direccion').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target-direccion').hide();
    }
    else {
        errorDireccion = false;
        $('#error-direccion').hide();
        $("#progressbardireccion").removeAttr("hidden");
        $('#progressbardireccion').show();

        $("#btnBuscaDireccion").hide();
    }
}

function successDataDireccion() {
    // Animate
    $('#progressbardireccion').hide();
    $("#btnBuscaDireccion").show();
    if (!errorDireccion) {
        $('#target-direccion').show();
    }
}

//$(function () {
//    $('#buscar-direccion').dialog({
//        autoOpen: false,
//        modal: true,
//        height: 400,
//        width: 600
//    });

//    $('#dialog-direccion').click(function () {
//        $('#buscar-direccion').dialog('open');
//        return false;
//    });
//});

//$(function () {
//    $("#dialog-direccion").button({
//        label: 'Buscar por dirección',
//        icons: {
//            primary: 'ui-icon-search'
//        },
//        text: false
//    })
//});

//$(function () {
//    $("#clean-direccion").button({
//        label: 'Limpiar búsqueda',
//        icons: {
//            primary: 'ui-icon-arrowrefresh-1-w'
//        },
//        text: false
//    }).click(function () {
//        $('#coddireccion').val('');
//    })
//});

/****************************************************************************************
                                CODIGOS DEPARTAMENTO
****************************************************************************************/

function beginDataDepartamento() {
    if ($('#codigodepartamento').val().length < 1 &&
        $('#nomdepartamento').val().length < 1) {
        errorDepartamento = true;
        $('#error-departamento').removeAttr("hidden");
        $('#error-departamento').show();
        $('#error-departamento').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target-departamento').hide();
    }
    else {
        errorDepartamento = false;
        $('#error-departamento').hide();
        $("#progressbardepartamento").removeAttr("hidden");
        $('#progressbardepartamento').show();

        $("#btnBuscaDepartamento").hide();
    }
}

function successDataDepartamento() {
    // Animate
    $('#progressbardepartamento').hide();
    $("#btnBuscaDepartamento").show();
    if (!errorDepartamento) {
        $('#target-departamento').show();
    }
}

//$(function () {
//    $('#buscar-departamento').dialog({
//        autoOpen: false,
//        modal: true,
//        height: 400,
//        width: 600
//    });

//    $('#dialog-departamento').click(function () {
//        $('#buscar-departamento').dialog('open');
//        return false;
//    });
//});

//$(function () {
//    $("#dialog-departamento").button({
//        label: 'Buscar por departamento',
//        icons: {
//            primary: 'ui-icon-search'
//        },
//        text: false
//    })
//});

//$(function () {
//    $("#clean-departamento").button({
//        label: 'Limpiar búsqueda',
//        icons: {
//            primary: 'ui-icon-arrowrefresh-1-w'
//        },
//        text: false
//    }).click(function () {
//        $('#coddepartamento').val('');
//    })
//});

/****************************************************************************************
                                    CODIGOS SECCIÓN
****************************************************************************************/

function beginDataSeccion() {
    if ($('#codigoseccion').val().length < 1 &&
        $('#nomseccion').val().length < 1) {
        errorSeccion = true;
        $('#error-seccion').removeAttr("hidden");
        $('#error-seccion').show();
        $('#error-seccion').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target-seccion').hide();
    }
    else {
        errorSeccion = false;
        $('#error-seccion').hide();
        $("#progressbarseccion").removeAttr("hidden");
        $('#progressbarseccion').show();
        $("#btnBuscaSeccion").hide();
    }
}

function successDataSeccion() {
    // Animate
    $('#progressbarseccion').hide();
    $("#btnBuscaSeccion").show();
    if (!errorSeccion) {
        $('#target-seccion').show();
    }
}

//$(function () {
//    $('#buscar-seccion').dialog({
//        autoOpen: false,
//        modal: true,
//        height: 400,
//        width: 600
//    });

//    $('#dialog-seccion').click(function () {
//        $('#buscar-seccion').dialog('open');
//        return false;
//    });
//});

//$(function () {
//    $("#dialog-seccion").button({
//        label: 'Buscar por sección',
//        icons: {
//            primary: 'ui-icon-search'
//        },
//        text: false
//    })
//});

//$(function () {
//    $("#clean-seccion").button({
//        label: 'Limpiar búsqueda',
//        icons: {
//            primary: 'ui-icon-arrowrefresh-1-w'
//        },
//        text: false
//    }).click(function () {
//        $('#codseccion').val('');
//    })
//});

/****************************************************************************************
                                    CODIGOS PRESUPUESTO
****************************************************************************************/


function beginDataPresupuesto() {
    $('#error-presupuesto').hide();
    $("#progressbarpresupuesto").removeAttr("hidden");
    $('#progressbarpresupuesto').show();
    $("#btnBuscaPresupuesto").hide()
    //if ($('#codigopresupuesto').val().length < 1) {
    //    errorPresupuesto = true;
    //    $('#error-presupuesto').removeAttr("hidden");
    //    $('#error-presupuesto').show();
    //    $('#error-presupuesto').html("Debe digitar el código presupuestario");
    //    $('#target-presupuesto').hide();
    //}
    //else {
    //    errorPresupuesto = false;
    //    $('#error-presupuesto').hide();
    //    $("#progressbarpresupuesto").removeAttr("hidden");
    //    $('#progressbarpresupuesto').show();
    //    $("#btnBuscaPresupuesto").hide();
    //}
}

function successDataPresupuesto() {
    $('#progressbarpresupuesto').hide();
    $("#btnBuscaPresupuesto").show();
    if (!errorPresupuesto) {
        $('#target-presupuesto').show();
    }
}

function CargarDato(texto, dialogo) {
    $('#buscar-' + dialogo).modal('hide');
    $('#cod' + dialogo).val(texto);
    //if (dialogo == "clase")
    //{

    //}
    return false;
}

function CargarDatoUbicacionGeografica(dialogo) {
    $('#buscar-' + dialogo).modal('hide');
    $('#txt_' + dialogo).val(document.getElementById('DropProvincias'+dialogo).value.concat("-", document.getElementById('DropCantones'+dialogo).value,"-", document.getElementById('DropDistritos'+dialogo).value));
   // $('#txt_' + dialogo).val(document.getElementById('DropProvincias'+dialogo).value + "-" + document.getElementById('DropCantones'+dialogo).value + "-" + document.getElementById('DropDistrito'+dialogo).value);
    //if (dialogo == "clase")
    //{

    //}
    return false;
}

function CargarDatoUbicacionAdministrativa(div, dir, dep, sec, cod){
    $('#buscar-ubicacion-admin').modal('hide');
    $('#txt_division').val(div);
    if(dir.startsWith("0-"))
    {
        $('#txt_direccion').val("");
    }
    else
    {
        $('#txt_direccion').val(dir);
    }
    if(dep.startsWith("0-"))
    {
        $('#txt_departamento').val("");
    }
    else
    {
        $('#txt_departamento').val(dep);
    }
    $('#codseccion').val(sec);
    //$('#txt_presupuesto').val(cod);
    return false;
}

function CargarDatoClase(texto, dialogo, categoria = -1) {
    $('#buscar-' + dialogo).modal('hide');
    $('#cod' + dialogo).val(texto);
    if(categoria != -1)
    {
        $('#txt_categoria').val(categoria);
    }
    //if (dialogo == "clase")
    //{

    //}
    return false;
}

function BeginCargarUnique() {
    $("#btnBuscarUnique").hide();
    $("#preloaderUnique").show();
}

function CompleteCargarUnique() {
    $("#preloaderUnique").hide();
    $("#btnBuscarUnique").show();
}

/****************************************************************************************
                                CODIGOS CENTRO COSTOS
****************************************************************************************/

function beginDataCostos() {
    if ($('#codigocostos').val().length < 1) {
        errorCostos = true;
        $('#error-costos').removeAttr("hidden");
        $('#error-costos').show();
        $('#error-costos').html("Debe digitar al menos un parámetro de búsqueda");
        $('#target-costos').hide();
    }
    else {
        errorCostos = false;
        $('#error-costos').hide();
        $("#progressbarcostos").removeAttr("hidden");
        $('#progressbarcostos').show();
        $("#btnBuscaCostos").hide();
    }
}

function successDataCostos() {
    // Animate
    $('#progressbarcostos').hide();
    $("#btnBuscaCostos").show();
    if (!errorCostos) {
        $('#target-costos').show();
    }
}

