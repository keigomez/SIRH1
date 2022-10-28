// MÉTODOS PARA CONTROLAR LA BÚSQUEDA

function BeginCargarFuncionario() {
    $("#btnBuscar").css("display", "none");
    $("#preloader").css("display", "block");
}

function CompleteCargarFuncionario() {
    $("#preloader").css("display", "none");
    $("#btnBuscar").css("display", "block");
    //$('#CopiaCertificada').bootstrapToggle("toggle");
}

function SuccessCargarFuncionario() {
    var config = {
        locale: 'es-es',
        uiLibrary: 'bootstrap4',
        format: 'dd/mm/yyyy'
    }

    //$('#datepickerEmision').datepicker(config);
    //$('#datepickerVence').datepicker(config);



    //$('#CopiaCertificada').bootstrapToggle("toggle");
}

// METODOS PARA CONTROLAR EL GUARDADO

function BeginGuardarPoliza() {
    $('#btnGuardar').css("display", "none");
    $("#preloaderGuardar").css("display", "block");
}

function CompleteGuardarPoliza() {
    $('#preloaderGuardar').css("display", "none");
    $("#btnGuardar").css("display", "block");
}

function SuccessGuardarPoliza() {
    SuccessCargarFuncionario();
}