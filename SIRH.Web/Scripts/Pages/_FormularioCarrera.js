$(document).ready(function () {

    $('#ImagenTitulo').on('change', function () {
        var fileName = $(this).val();
        fileName = fileName.replace("C:\\fakepath\\", "");
        $(this).next('.custom-file-label').html(fileName);
    })

    var fechas = ['#FecInicio', '#FecFinal', '#FecEmision'];

    fechas.forEach(id => {

        var config = {
            locale: 'es-es',
            uiLibrary: 'bootstrap4',
            maxDate: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()),
            format: 'dd/mm/yyyy'
        };
        
        (id == "#FecFinal") ? config["minDate"] = () => $("#FecInicio").val() : null;

        $(id).datepicker(config);
        ($(id).val() && $(id).val().charAt($(id).val().length - 1) == '.') ? $(id).val('') : null;
    })

    if ($("#ModalidadSeleccionada").val() == "1" || $("#ModalidadSeleccionada").val() == "2" || $("#ModalidadSeleccionada").val() == "3") {
        $(".hide").show()
    }

});


function readURL(input) {
    if (input.files && input.files[0]) {
        $('#pdf_preview').show();
        $('#pdf_preview')[0].href = window.URL.createObjectURL(new Blob([input.files[0]], { "type": "application/pdf" }))
    }
}

$("#ImagenTitulo").change(function () {
    readURL(this);
});

