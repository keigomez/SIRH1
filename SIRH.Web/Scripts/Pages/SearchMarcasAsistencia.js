/*Métodos AJAX*/

function BeginSearch() {
    $('#error').hide();
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}

function CompleteSearch() {
    $('#btnBuscar').show();
    $('#preloader').hide();
}

/*Validación del código de acceso*/
//$("[id*=Codigo]").live('keydown', function (e) {
//    var keyCode = e.which ? e.which : e.keyCode
//    if (keyCode == 13) {
//        return true;
//    }
//    else {
//        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
//        if (ret == false) {
//            $('#error').show();
//            $('#error').html("El código de acceso sólo puede contener valores numéricos");
//        }
//        else {
//            $('#error').hide();
//        }
//        return ret;
//    }
//});

$(document).ready(function () {
    $("#Codigo").keydown(function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        if (keyCode == 13) {
            return true;
        }
        else {
            var specialKeys = new Array();
            specialKeys.push(8); //Backspace
            19
            specialKeys.push(9); //Tab
            20
            specialKeys.push(46); //Delete

            var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
            if (ret == false) {
                $('#error').removeAttr("hidden");
                $('#error').show();
                $('#error').html("El código de acceso sólo puede contener valores numéricos");
            }
            else {
                $('#error').hide();
            }
            return ret;
        }
    });

});