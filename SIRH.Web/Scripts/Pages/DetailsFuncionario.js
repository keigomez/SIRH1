$(document).ready(function () {
    //$("#tabs").tabs();
    //$("#DialogCalif").button({
    //    label: 'Ver Historial de Calificaciones',
    //    icons: {
    //        primary: 'ui-icon-clock'
    //    },
    //    text: false
    //})
    //$("#DialogEstCivil").button({
    //    label: 'Ver Historial de Estado Civil',
    //    icons: {
    //        primary: 'ui-icon-clock'
    //    },
    //    text: false
    //})

    //$('#calificaciones').dialog({
    //    autoOpen: false,
    //    modal: true,
    //    height: 200
    //});

    $('#DialogCalif').click(function () {
        $("#calificaciones").appendTo("body");
        $('#calificaciones').modal('show');
        return false;
    });
    //$('#estadoCivil').dialog({
    //    autoOpen: false,
    //    modal: true
    //});

    $('#DialogEstCivil').click(function () {
        $("#estadoCivil").appendTo("body");
        $('#estadoCivil').modal('show');
        return false;
    });

});