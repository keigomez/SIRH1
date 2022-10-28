//function beginData() {
//    if ($('#query').val().length < 5) {
//        error = true;
//        $('#progressbar').hide();
//        $("#btnBusca").show();
//        $('#error').show();
//        $('#error').html("La búsqueda debe contener al menos 5 caracteres.");
//        return false;
//    }
//    else {
//        error = false;
//        $('#error').hide();
//        $('#progressbar').show();
//        $("#progressbar").progressbar({
//            value: 100
//        });
//        $("#btnBusca").hide();
//    }
//}

//$(document).ready(function () {
//    $('#btnBusca').on('click', function () {
//            if ($('#query').val().length < 5) {
//                error = true;
//                $('#preloader').hide();
//                $("#btnBusca").show();

//                $('#error').removeAttr("hidden");
//                $('#error').show();
//                $('#error').html("La búsqueda debe contener al menos 5 caracteres.");
//                return false;
//            }
//            else {
//                error = false;
//                $('#error').hide();
//                $('#preloader').removeAttr("hidden");
//                $('#preloader').show();
//                $("#btnBusca").hide();
//                //onSuccess();
//            }
//    });
//});

function onSuccess() {
    $("#btnBusca").show();
    $('#preloader').removeAttr("hidden");
    $('#preloader').hide();
   //window.location.href = window.location.href + "Home/Result?query=" + $("#query").val() + "&primera=Si";
}

function onBegin() {
    $("#btnBusca").hide();
    $('#preloader').removeAttr("hidden");
    $('#preloader').show();
}

