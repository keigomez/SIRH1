function OnBegin() {
    if($('#preloader').has("hidden"))
        $('#preloader').removeAttr("hidden");
    else
        $('#preloader').show();

    $('#btnRegistrar').hide();
}

function OnSuccess() {
    $('#preloader').hide();
    $('#btnRegistrar').show();
}

function update() {
    $('#thisform').submit();
}