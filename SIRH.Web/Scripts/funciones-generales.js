var specialKeys = new Array();
specialKeys.push(8); //Backspace

function IsNumeric(e, texto, campo) {
    if (texto != "") {
        texto = "-" + texto;
    }
    var keyCode = e.which ? e.which : e.keyCode
    var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
    if (ret == false) {
        $('#error' + texto).show();
        $('#error' + texto).html("El " + campo + " sólo puede contener valores numéricos");
    }
    else {
        $('#error' + texto).hide();
    }
    return ret;
}

function IsNotNumeric(e, texto, campo) {
    if (texto != "") {
        texto = "-" + texto;
    }
    var keyCode = e.which ? e.which : e.keyCode
    var ret = ((((keyCode >= 65 && keyCode <= 90) || keyCode == 241) || (keyCode >= 97 && keyCode <= 122)) || specialKeys.indexOf(keyCode) != -1);
    if (ret == false) {
        $('#error').show();
        $('#error').html("El campo de " + campo + " no puede contener números");
    }
    else {
        $('#error').hide();
    }
    return ret;
}

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for dd/mm/yyyy format.
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function OnBegin(btn, preloader) {
    $(`#${btn}`).hide()
    $(`#${preloader}`).show()
}

function OnComplete(btn, preloader) {
    $(`#${btn}`).show()
    $(`#${preloader}`).hide()
}