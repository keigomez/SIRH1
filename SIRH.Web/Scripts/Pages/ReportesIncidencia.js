$(document).ready(function () {
    $('#Filtro').on('change', GraficosArea);
});

// Controlar PopUp Reporte
function ShowPopUpReporte() {
    $("#detail-reporte").appendTo("body");
    $('#detail-reporte').modal('show');
    $("#data-reporte").css("display", "none");
    $("#preloaderR").css("display", "block");
}

function FullPopUpReporte() {
    $("#preloaderR").css("display", "none");
    $("#data-reporte").css("display", "block");
}

// Controlar el campo de texto de error
function GraficosArea(e) {
    if ($('#Filtro :selected').text() == 'Módulo' || $('#Filtro :selected').text() == 'Prioridad' || $('#Filtro :selected').text() == 'Estado'
        || $('#Filtro :selected').text() == 'Usuario del SIRH' || $('#Filtro :selected').text() == 'Funcionario del UIRH' || $('#Filtro :selected').text() == 'Tickets') {
        $('#Graficos').css("display", "block");
        $('#Circular').css("display", "block");
        $('#Donut').css("display", "block");
        $('#Horizontal').css("display", "block");
        $('#Vertical').css("display", "block");
        $('#DobleBarra').css("display", "none");
    } else if ($('#Filtro :selected').text() == 'Tiempo de atención') {
        $('#Graficos').css("display", "block");
        $('#Circular').css("display", "none");
        $('#Donut').css("display", "none");
        $('#DobleBarra').css("display", "none");
        $('#Horizontal').css("display", "block");
        $('#Vertical').css("display", "block");
    } else if ($('#Filtro :selected').text() == 'Fecha') {
        $('#Graficos').css("display", "block");
        $('#Circular').css("display", "none");
        $('#Donut').css("display", "none");
        $('#Horizontal').css("display", "none");
        $('#Vertical').css("display", "none");
        $('#DobleBarra').css("display", "block");
    }
}

function typeChart(result) {
    var tipo = result.responseJSON.Data.tipo;

    switch (tipo) {
        case '1':
            drawCircle(result);
            break;
        case '2':
            drawDonut(result);
            break;
        case '3':
            drawBar(result);
            break;
        case '4':
            drawColum(result);
            break;
        case '5':
            drawDobleBar(result);
            break;
        default:
            break;
    }
}

// Gráfico Circular
function drawCircle(result) {
    var incidencias = result.responseJSON.Data.data;
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Incidencia');
    data.addColumn('number', 'Percentage');

    incidencias.forEach(e => data.addRow([e.Dato, e.Count]));

    var options = {
        'title': result.responseJSON.Data.name
    };

    var chart = new google.visualization.PieChart(document.getElementById('myChart'));
    chart.draw(data, options);
    generaPDF(chart.getImageURI());
}

// Grafico Donut
function drawDonut(result) {
    var incidencias = result.responseJSON.Data.data;
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Incidencia');
    data.addColumn('number', 'Percentage');

    incidencias.forEach(e => data.addRow([e.Dato, e.Count]));

    var options = {
        'title': result.responseJSON.Data.name,
        pieHole: 0.4
    };

    var chart = new google.visualization.PieChart(document.getElementById('myChart'));
    chart.draw(data, options);
    generaPDF(chart.getImageURI());
}

// Gráfico Barras Horizontal
function drawColum(result) {
    var incidencias = result.responseJSON.Data.data;
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Incidencia');
    data.addColumn('number', 'Percentage');

    incidencias.forEach(e => data.addRow([e.Dato, e.Count]));

    var options = {
        'title': result.responseJSON.Data.name,
        bar: { groupWidth: "95%" },
        legend: { position: "none" },
        'hAxis': {
            title: result.responseJSON.Data.information
        },
        'vAxis': {
            title: result.responseJSON.Data.cantidad
        }
    };

    var chart = new google.visualization.ColumnChart(document.getElementById('myChart'));
    chart.draw(data, options);
    generaPDF(chart.getImageURI());
}

// Gráfico Barras Vertical
function drawBar(result) {
    var incidencias = result.responseJSON.Data.data;
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Incidencia');
    data.addColumn('number', 'Percentage');

    incidencias.forEach(e => data.addRow([e.Dato, e.Count]));

    var options = {
        'title': result.responseJSON.Data.name,
        bar: { groupWidth: "95%" },
        legend: { position: "none" },
        'hAxis': {
            title: result.responseJSON.Data.cantidad
        },
        'vAxis': {
            title: result.responseJSON.Data.information
        }
    };

    var chart = new google.visualization.BarChart(document.getElementById('myChart'));
    chart.draw(data, options);
    generaPDF(chart.getImageURI());
}

// Gráfico Doble Barra Vertical
function drawDobleBar(result) {
    console.log(result.responseJSON.Data.data);
    var incidencias = result.responseJSON.Data.data;
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Fecha');
    data.addColumn('number', 'Fecha de Inicio');
    data.addColumn('number', 'Fecha de Finalización');

    incidencias.forEach(e => data.addRow([e.Key, e.FI, e.FF]));

    var options = {
        title: result.responseJSON.Data.name,
        hAxis: {
            title: result.responseJSON.Data.information,
        },
        vAxis: {
            title: result.responseJSON.Data.cantidad,
        }
    };

    var chart = new google.visualization.ColumnChart(document.getElementById('myChart'));
    chart.draw(data, options);
    generaPDF(chart.getImageURI());
}

// GENERA PDF
function generaPDF(image) {
    var logo = "data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAAyAAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjExQzMwNkIxM0E3NDExRTc4Qzk4ODBGNzc1N0IwMDZCIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjExQzMwNkIyM0E3NDExRTc4Qzk4ODBGNzc1N0IwMDZCIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MTFDMzA2QUYzQTc0MTFFNzhDOTg4MEY3NzU3QjAwNkIiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTFDMzA2QjAzQTc0MTFFNzhDOTg4MEY3NzU3QjAwNkIiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAOQWRvYmUAZMAAAAAB/9sAhAAIBgYGBgYIBgYIDAgHCAwOCggI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";
    var doc = new jsPDF('portrait');

    doc.addImage(logo, 'JPEG', 15, 15, 35, 25);

    var f = new Date();
    doc.setFontSize(10);
    doc.text(f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear(), 180, 30, 'right');

    doc.setFontSize(12);
    doc.text('Ministerio de Obras Públicas y Transporte', 90, 30, 'center');
    doc.setFontSize(10);
    doc.text('Dirección de Gestión Institucional de Recursos Humanos', 95, 35, 'center');

    doc.setFontSize(14);
    doc.text('Reporte Estadístico', 100, 50, 'center');
    doc.addImage(image, 'JPEG', 50, 70);

    doc.save('ReporteEstadístico.pdf');
}