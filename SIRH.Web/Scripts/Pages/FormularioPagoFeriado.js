function radio_sistema() {
    $("#nombramientoSL").css("display", "none");
}

function radio_fisico() {
    $("#nombramientoSL").css("display", "block");
}

function radio_asueto() {
    $("#btn_feriado").css("display", "none");
    $("#btn_asueto").css("display", "block");
}

function radio_feriado() {
    $("#btn_asueto").css("display", "none");
    $("#btn_feriado").css("display", "block");
}

function siguiente() {
    $("#pagina1").css("display", "none");
    $("#pagina2").css("display", "block");
}

function atras() {
    $("#pagina2").css("display", "none");
    $("#pagina1").css("display", "block");
}

function BeginGuardarPago() {
    $('#btnAlmacenar').css("display", "none");
    $("#preloaderAlmacenar").css("display", "block");
}

function CompleteGuardarPago() {
    $('#preloaderAlmacenar').css("display", "none");
    $("#btnAlmacenar").css("display", "block");
}



function CargarDatoF(texto, id) {

    $(`#${id}`).attr('disabled', true);
    $("#radioAsueto").attr('disabled', true);
    //Calculo de salario por horas
    var salario = parseFloat($("#montoSalario").val()) / parseFloat(240);
    var tam = $('#dias_pagados >tbody >tr').length;
    //Cálculos

    //Salario por horas
    var salarioHora = parseFloat(salario).toFixed(2);

    //Calculo del salario total por hora trabajada
    var salarioHoraTotal = parseFloat(salarioHora) * parseFloat(8);


    //Obtiene el subtotal actual
    var subtotalActual;
    if ($("[id*=SubtotalDias]").val() == "") {
        subtotalActual = 0;
    } else {
        subtotalActual = $("[id*=SubtotalDias]").val(); //document.getElementById('totales').rows[1].cells[1].innerText;
        subtotalActual = subtotalActual.substring(1);
    }

    //Calcula el nuevo subtotal
    var subtotalNuevo = (parseFloat(subtotalActual) + parseFloat(salarioHoraTotal)).toFixed(2);

    //Obtener porcentaje del salario escolar
    var porcentajeSalario = parseFloat(($("#porEscolar").val()).replace(",", "."));

    //Calcula el porcentaje de salario escolar
    var salarioEscolar = ((parseFloat(subtotalNuevo) * parseFloat(porcentajeSalario)) / 100).toFixed(2);

    //Asigna los nuevos totales

    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);

    $("[id*=SubtotalDias]").val("₡" + subtotalNuevo);

    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalNuevo;
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + (parseInt(salarioEscolar) + parseInt(subtotalNuevo)).toFixed(2);

    //Dibuja un row en la tabla
    var fecha = new Date();
    var ano = fecha.getFullYear();

    var html =
        `<tr>` +
        `<td><input name='id_dia' style='border:0;' size='5' value='${id}' readonly/></td>` +
        `<td>${texto}</td>` +
        `<td><input type='text' min='0' size = '4' name='horaTabla' value='8' class='form-control form-control-sm' style='width : 60px;' onchange='HoraFeriadoChange(this)'></input></td>` +
        `<td>₡ ${$("#montoSalario").val()}</td>` +
        `<td name='salarioTabla'>₡ ${salarioHora}</td>` +
        `<td><input name='salarioTabla' size ='10' style='border:0;' value='₡ ${salarioHoraTotal}' readonly/></td>` +
        `<td><input id='anioD' name='anioTabla' size = '4' type='text' value='${new Date().getFullYear()}' class='form-control form-control-sm' style='width : 60px;' onchange='anioChange(this)'></input></td>` +
        `<td><button type="button" class="btn btn-danger btn-sm" onclick='delete_row_feriado(this)'><i class="fa fa-trash"></i></button></td>` +
        `</tr>`;

    //Recalcula las deducciones Obrero
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    if (tam2 > 0) {
        diferencias = diferencias.substring(1);
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;

            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);

            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            var aaa = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
            //var ax = document.getElementById('deduccionObrero').rows[auxO].cells[1];
            //console.log(ax);


            aaa = aaa.substring(1);
            var t = (parseFloat(aaa) + totalDeduccionO).toFixed(2);
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + t;


        }
    }
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    var auxTot = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxTot = auxTot.substring(1);
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(auxTot)).toFixed(2));
    var auxdoP = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxdoP = auxdoP.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(auxdoP));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + (parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida));

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;

    //Recalcula las deducciones Patronales
    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;

            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);

            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            var totalDP = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            totalDP = totalDP.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = (parseFloat(totalDP) + totalDeduccionO).toFixed(2);
        }

    }

    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;

            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            var totalDP = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            totalDP = totalDP.substring(1);

            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + (parseFloat(totalDP) + totalDeduccionO).toFixed(2);
        }

        tam2 = $('#deduccionPatronal >tbody >tr').length;
        document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡0.00";
        for (var i = 1; i < tam2; i++) {
            var totalDP = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            var totalDPT = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;
            totalDP = totalDP.substring(1);
            totalDPT = totalDPT.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + (parseFloat(totalDP) + parseFloat(totalDPT)).toFixed(2);
        }
    }
    $('#dias_pagados').append(html);


    tam2 = $('#deduccionObrero >tbody >tr').length;
    document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡0.00";
    for (var i = 1; i < tam2; i++) {
        var totalDO = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
        var totalDOT = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;
        totalDO = totalDO.substring(1);

        totalDOT = totalDOT.substring(1);
        var res = (parseFloat(totalDO) + parseFloat(totalDOT)).toFixed(2);
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + res;
    }
    var auxTot = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxTot = auxTot.substring(1);
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(auxTot)).toFixed(2));
    var auxdoP = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxdoP = auxdoP.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(auxdoP));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + ((parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida)).toFixed(2));

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    return false;
}

function CargarDatoA(texto, id) {

    $(`#${id}`).attr('disabled', true);

    $("#radioFeriado").attr('disabled', true);

    var salario = parseFloat($("#montoSalario").val()) / parseFloat(240);

    //Calculo de salario por horas
    var salario = parseFloat($("#montoSalario").val()) / parseFloat(240);

    //Salario por horas
    var salarioHora = parseFloat(salario).toFixed(2);

    //Calculo del salario total por hora trabajada
    var salarioHoraTotal = parseFloat(salarioHora) * parseFloat(8);

    //Obtiene el subtotal actual
    var subtotalActual;
    if ($("[id*=SubtotalDias]").val() == "") {
        subtotalActual = 0;
    } else {
        subtotalActual = $("[id*=SubtotalDias]").val();
        subtotalActual = subtotalActual.substring(1);
    }

    //Calcula el nuevo subtotal
    var subtotalNuevo = (parseFloat(subtotalActual) + parseFloat(salarioHoraTotal)).toFixed(2);
    var porcentajeSalario = parseFloat(($("#porEscolar").val()).replace(",", "."));
    //Calcula el porcentaje de salario escolar
    var salarioEscolar = ((parseFloat(subtotalNuevo) * parseFloat(porcentajeSalario)) / 100).toFixed(2);

    //Asigna los nuevos totales

    //document.getElementById('totales').rows[0].cells[1].innerText = "₡"+salarioEscolar;
    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    //document.getElementById('totales').rows[1].cells[1].innerText = subtotalNuevo;
    $("[id*=SubtotalDias]").val("₡" + subtotalNuevo);
    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalNuevo;
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + ((parseInt(salarioEscolar) + parseInt(subtotalNuevo)).toFixed(2));
    deferencias = diferencias.substring(1);
    var styleButton = "style = 'border: none; color: white; background-color: #CC0000; text-align: center; text-decoration: none;  display: inline-block;  border-radius: 5px;'";
    var fecha = new Date();
    var ano = fecha.getFullYear();

    var html = 
        `<tr>` +
        `<td><input name='id_dia' style='border:0;' size='5' value='${id}' readonly/></td>` +
        `<td>${texto}</td>` +
        `<td><input type='text' min='0' size = '4' name='horaTabla' value='8' class='form-control form-control-sm' style='width : 60px;' onchange='HoraAsuetoChange(this)'></input></td>` +
        `<td>₡ ${$("#montoSalario").val()}</td>` +
        `<td name='salarioTabla'>₡ ${salarioHora}</td>` +
        `<td><input name='salarioTabla' size ='10' style='border:0;' value='₡ ${salarioHoraTotal}' readonly/></td>` +
        `<td><input id='anioD' name='anioTabla' size = '4' type='text' value='${new Date().getFullYear()}' class='form-control form-control-sm' style='width : 60px;' onchange='anioChange(this)'></input></td>` +
        `<td><button type="button" class="btn btn-danger btn-sm" onclick='delete_row_asueto(this)'><i class="fa fa-trash"></i></button></td>` +
        `</tr>`;

    var tam2 = $('#deduccionObrero >tbody >tr').length;
    if (tam2 > 0) {
        diferencias = diferencias.substring(1);
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
        }
    }
    tam2 = $('#deduccionObrero >tbody >tr').length;
    document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
    for (var i = 1; i < tam2; i++) {
        var auxO1 = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
        auxO1 = auxO1.substring(1);

        var auxO2 = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;
        auxO2 = auxO2.substring(1);

        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxO1) + parseFloat(auxO2)).toFixed(2));

    }


    var tam2 = $('#deduccionPatronal >tbody >tr').length;

    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2));
        }
        tam2 = $('#deduccionPatronal >tbody >tr').length;
        document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            auxP1 = auxP1.substring(1);
            var auxO2 = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxP1) + parseFloat(auxO2)).toFixed(2));
        }
    }
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    var auxTot = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxTot = auxTot.substring(1);
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(auxTot)).toFixed(2));
    var auxdoP = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    auxdoP = auxdoP.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(auxdoP));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + ((parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida)).toFixed(2));

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    $('#dias_pagados').append(html);

    return false;
}

function HoraFeriadoChange(input) {
    if (input.value == "") {
        input.value = 8
    }
    if (input.value.length > 2) {
        input.value = 8
    }
    if (parseInt(input.value) <= 0) {
        input.value = 8
    }
    for (var i = 0; i < input.value.length; i++) {
        if (isNaN(input.value[i])) {
            input.value = 8
        }
    }
    var $tr = $(input).parents("tr");
    var valor = input.value;

    var salario = $tr.find("td").eq(4).html();
    salario = salario.substring(1);

    monto = (parseFloat(salario).toFixed(2) * parseFloat(valor)).toFixed(2);

    var subtotalActual = $("[id*=SubtotalDias]").val();
    subtotalActual = subtotalActual.substring(1);

    var salarioEscolarActual = $("[id*=porcentajeSalarioEscolar]").val();
    salarioEscolarActual = salarioEscolarActual.substring(1);

    var antiguoTotal = $tr.find("td").eq(5).find("input").val();
    antiguoTotal = antiguoTotal.substring(1);

    var subtotalNuevo = (parseFloat(subtotalActual) - parseFloat(antiguoTotal)).toFixed(2);
    var subtotalCambiado = (parseFloat(subtotalNuevo) + parseFloat(monto)).toFixed(2);

    $("[id*=SubtotalDias]").val("₡" + subtotalCambiado);

    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalCambiado;

    var porEscolar = parseFloat(($("#porEscolar").val()).replace(",", "."));
    var salarioEscolar = ((parseFloat(subtotalCambiado) * parseFloat(porEscolar)) / 100).toFixed(2);
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + ((parseInt(salarioEscolar) + parseInt(subtotalCambiado)).toFixed(2));
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    if (tam2 > 0) {
        diferencias = diferencias.substring(1);
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + (totalDeduccionO.toFixed(2));
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2));
        }

        tam2 = $('#deduccionObrero >tbody >tr').length;
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡0.00";
        for (var i = 1; i < tam2; i++) {
            var auxO1 = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
            auxO1 = auxO1.substring(1);

            var auxO2 = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);

            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxO1) + parseFloat(auxO2)).toFixed(2));
        }
    }

    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + (totalDeduccionO.toFixed(2));
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2));
        }
    }
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    var auxOPT = tam2;
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value)).toFixed(2));
    var totalDAux = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
    totalDAux = totalDAux.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(totalDAux));
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + diferenciaLiquida;
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + (parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida));

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + (parseFloat(document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2);
        }

        tam2 = $('#deduccionPatronal >tbody >tr').length;
        document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            auxP1 = auxP1.substring(1);
            var auxO2 = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxP1) + parseFloat(auxO2)).toFixed(2));
        }
    }
    var totalDAux = document.getElementById('deduccionObrero').rows[auxOPT].cells[1].querySelector('input').value;
    totalDAux = totalDAux.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(totalDAux));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + (parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida)).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + diferenciaLiquida;
    $tr.find("td").eq(5).find("input").val("₡" + monto);

    return false;
}

function HoraAsuetoChange(input) {
    if (input.value == "") {
        input.value = 8
    }
    if (input.value.length > 2) {
        input.value = 8
    }
    if (parseInt(input.value) <= 0) {
        input.value = 8
    }
    for (var i = 0; i < input.value.length; i++) {
        if (isNaN(input.value[i])) {
            input.value = 8;
        }
    }
    var $tr = $(input).parents("tr");
    var valor = input.value;

    var salario = $tr.find("td").eq(4).html();

    salario = salario.substring(1);

    monto = (parseFloat(salario).toFixed(2) * parseFloat(valor)).toFixed(2);

    var subtotalActual = $("[id*=SubtotalDias]").val();
    subtotalActual = subtotalActual.substring(1);

    var salarioEscolarActual = $("[id*=porcentajeSalarioEscolar]").val();
    salarioEscolarActual = salarioEscolarActual.substring(1);

    var antiguoTotal = $tr.find("td").eq(5).find("input").val();
    antiguoTotal = antiguoTotal.substring(1);

    var subtotalNuevo = (parseFloat(subtotalActual) - parseFloat(antiguoTotal)).toFixed(2);
    var subtotalCambiado = (parseFloat(subtotalNuevo) + parseFloat(monto)).toFixed(2);


    $("[id*=SubtotalDias]").val("₡" + subtotalCambiado);
    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalCambiado;

    var porEscolar = parseFloat(($("#porEscolar").val()).replace(",", "."));
    var salarioEscolar = ((parseFloat(subtotalCambiado) * parseFloat(porEscolar)) / 100).toFixed(2);

    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + ((parseInt(salarioEscolar) + parseInt(subtotalCambiado)).toFixed(2));

    var tam2 = $('#deduccionObrero >tbody >tr').length;
    diferencias = diferencias.substring(1);
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + (totalDeduccionO.toFixed(2));
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2));
        }

        tam2 = $('#deduccionObrero >tbody >tr').length;
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxO1 = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
            auxO1 = auxO1.substring(1);
            var auxO2 = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;

            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxO1) + parseFloat(auxO2)).toFixed(2));
        }
    }
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    var auxOPT = tam2;
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = (parseFloat(diferencias) - parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value)).toFixed(2);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida);

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = aguinaldoProporcional;
    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 1) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + (totalDeduccionO.toFixed(2));
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value) + totalDeduccionO).toFixed(2));
        }

        tam2 = $('#deduccionPatronal >tbody >tr').length;
        document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            auxP1 = auxP1.substring(1);
            var auxO2 = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxP1) + parseFloat(auxO2)).toFixed(2));

        }
    }
    var totalDAux = document.getElementById('deduccionObrero').rows[auxOPT].cells[1].querySelector('input').value;
    totalDAux = totalDAux.substring(1);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(totalDAux));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + (parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida));
    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + diferenciaLiquida;
    $tr.find("td").eq(5).find("input").val("₡" + monto);
    return false;
}

function anioChange(input) {
    var fecha = new Date();
    var ano = fecha.getFullYear();
    if (input.value == "") {
        input.value = ano;
    }
    if (parseInt(input.value) < 1900) {
        input.value = ano
    }
    for (var i = 0; i < input.value.length; i++) {
        if (isNaN(input.value[i])) {
            input.value = ano
        }
    }
}

function delete_row_feriado(btn) {
    var $tr = $(btn).parents("tr");
    var totalPago = $tr.find("td").eq(5).find("input").val();
    var id = $tr.find("td").eq(0).find("input").val();
    $(`#${id}`).attr('disabled', false);
    totalPago = totalPago.substring(1);
    var subtotalActual;
    if ($("[id*=SubtotalDias]").val() == "") {
        subtotalActual = 0;
    } else {
        subtotalActual = $("[id*=SubtotalDias]").val();
        subtotalActual = subtotalActual.substring(1);
    }

    var subtotalCambiado = (parseFloat(subtotalActual) - parseFloat(totalPago)).toFixed(2);

    $("[id*=SubtotalDias]").val("₡" + subtotalCambiado);
    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalCambiado;
    var porcentajeSalario = parseFloat(($("#porEscolar").val()).replace(",", "."));
    var salarioEscolar = ((parseFloat(subtotalCambiado) * parseFloat(porcentajeSalario)) / 100).toFixed(2);
    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + ((parseInt(salarioEscolar) + parseInt(subtotalCambiado)).toFixed(2));
    diferencias = diferencias.substring(1);

    $(btn).closest('tr').remove();

    var tam2 = $('#deduccionObrero >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);

        }

        document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;

        var tam2 = $('#deduccionPatronal >tbody >tr').length;
        if (tam2 > 0) {
            for (var i = 1; i < tam2; i++) {
                var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
                var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
                document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
                var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
                auxP1 = auxP1.substring(1);

                document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = (parseFloat(auxP1) + totalDeduccionO).toFixed(2);
            }

            tam2 = $('#deduccionPatronal >tbody >tr').length;
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
            for (var i = 1; i < tam2; i++) {
                var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;

                auxP1 = auxP1.substring(1);
                var auxP2 = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;

                auxP2 = auxP2.substring(1);
                document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxP1) + parseFloat(auxP2)).toFixed(2));
            }
        }
        tam2 = $('#deduccionObrero >tbody >tr').length;
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxO1 = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
            auxO1 = auxO1.substring(1);
            var auxO2 = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxO1) + parseFloat(auxO2)).toFixed(2));
        }
        var tam2 = $('#deduccionObrero >tbody >tr').length;
        var auxDiferenciaL = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
        auxDiferenciaL = auxDiferenciaL.substring(1);
        document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(auxDiferenciaL)).toFixed(2));
        var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(auxDiferenciaL));
        var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
        document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + ((parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida)).toFixed(2));

        document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    }

    var nFilas = $('#dias_pagados >tbody >tr').length;

    if (nFilas == 0) {

        $("#radioAsueto").attr('disabled', false);
    }
    return false;
}

function delete_row_asueto(btn) {
    var $tr = $(btn).parents("tr");
    var totalPago = $tr.find("td").eq(5).find("input").val();
    totalPago = totalPago.substring(1);

    var id = $tr.find("td").eq(0).find("input").val();
    $(`#${id}`).attr('disabled', false);

    var subtotalActual;
    if ($("[id*=SubtotalDias]").val() == "") {
        subtotalActual = 0;
    } else {
        subtotalActual = $("[id*=SubtotalDias]").val(); //document.getElementById('totales').rows[1].cells[1].innerText;
        subtotalActual = subtotalActual.substring(1);
    }

    var subtotalCambiado = (parseFloat(subtotalActual) - parseFloat(totalPago)).toFixed(2);


    $("[id*=SubtotalDias]").val("₡" + subtotalCambiado);

    document.getElementById('diferenciasPeriodos').rows[1].cells[1].innerText = "₡" + subtotalCambiado;
    var porcentajeSalario = parseFloat(($("#porEscolar").val()).replace(",", "."));

    var salarioEscolar = ((parseFloat(subtotalCambiado) * parseFloat(porcentajeSalario)) / 100).toFixed(2);

    $("[id*=porcentajeSalarioEscolar]").val("₡" + salarioEscolar);
    document.getElementById('diferenciasPeriodos').rows[2].cells[1].innerText = "₡" + salarioEscolar;
    var diferencias = document.getElementById('diferenciasPeriodos').rows[3].cells[1].innerText = "₡" + ((parseInt(salarioEscolar) + parseInt(subtotalCambiado)).toFixed(2));

    $(btn).closest('tr').remove();

    var tam2 = $('#deduccionObrero >tbody >tr').length;
    diferencias = diferencias.substring(1);
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionObrero').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
        }

        tam2 = $('#deduccionObrero >tbody >tr').length;
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = 0.00;
        for (var i = 1; i < tam2; i++) {

            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = (parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value) + parseFloat(document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value)).toFixed(2);
        }
    }
    var tam2 = $('#deduccionObrero >tbody >tr').length;
    document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = (parseFloat(diferencias) - parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value)).toFixed(2);
    var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value));
    var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
    document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida);

    document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = aguinaldoProporcional;

    var tam2 = $('#deduccionPatronal >tbody >tr').length;
    if (tam2 > 0) {
        for (var i = 1; i < tam2; i++) {
            var porcentaje = document.getElementById('deduccionPatronal').rows[i].cells[1].innerText;
            var totalDeduccionO = (parseFloat(porcentaje) / 100) * parseFloat(diferencias);
            document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value = "₡" + totalDeduccionO.toFixed(2);
            var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            auxP1 = auxP1.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = (parseFloat(auxP1) + totalDeduccionO).toFixed(2);
        }

        tam2 = $('#deduccionPatronal >tbody >tr').length;
        document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {

            var auxP1 = document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value;
            auxP1 = auxP1.substring(1);
            var auxP2 = document.getElementById('deduccionPatronal').rows[i].cells[2].querySelector('input').value;

            auxP2 = auxP2.substring(1);
            document.getElementById('deduccionPatronal').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxP1) + parseFloat(auxP2)).toFixed(2));

        }
        tam2 = $('#deduccionObrero >tbody >tr').length;
        document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + "0.00";
        for (var i = 1; i < tam2; i++) {
            var auxO1 = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
            auxO1 = auxO1.substring(1);
            var auxO2 = document.getElementById('deduccionObrero').rows[i].cells[2].querySelector('input').value;
            auxO2 = auxO2.substring(1);
            document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value = "₡" + ((parseFloat(auxO1) + parseFloat(auxO2)).toFixed(2));
        }
        var tam2 = $('#deduccionObrero >tbody >tr').length;
        var auxDiferenciaL = document.getElementById('deduccionObrero').rows[tam2].cells[1].querySelector('input').value;
        auxDiferenciaL = auxDiferenciaL.substring(1);
        document.getElementById('tabla_montos_totales').rows[1].cells[1].innerText = "₡" + ((parseFloat(diferencias) - parseFloat(auxDiferenciaL)).toFixed(2));
        var diferenciaLiquida = (parseFloat(diferencias) - parseFloat(auxDiferenciaL));
        var aguinaldoProporcional = (parseFloat(diferencias) / 12).toFixed(2);
        document.getElementById('tabla_montos_totales').rows[3].cells[1].innerText = "₡" + ((parseFloat(aguinaldoProporcional) + parseFloat(diferenciaLiquida)).toFixed(2));

        document.getElementById('tabla_montos_totales').rows[2].cells[1].innerText = "₡" + aguinaldoProporcional;
    }

    var nFilas = $('#dias_pagados >tbody >tr').length;
    if (nFilas == 0) {
        $("#radioFeriado").attr('disabled', false);
    }
    return false;
}

