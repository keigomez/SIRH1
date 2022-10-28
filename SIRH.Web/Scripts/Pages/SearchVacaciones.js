
function BeginSearch() {
    $('#btnBuscar').hide();
    $("#preloader").removeAttr("hidden");
    $('#preloader').show();
}
function CompleteSearch() {
    $('#preloader').hide();
    $('#btnBuscar').show();

}