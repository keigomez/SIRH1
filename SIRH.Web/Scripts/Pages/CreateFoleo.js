$(document).ready(function () {
    $('#preloaderBuscar').css("display", "none");
});

function UpdateFoleo() {

}

function SuccesFoleo() {
}

function CompleteFoleo() {
    $('#preloaderBuscar').css("display", "none");
    $("#btnBuscar").css("display", "block");

    var toggler = document.getElementsByClassName("caret");
    var i;

    //Agrega un evento de click a cada uno de los tomos.
    for (i = 0; i < toggler.length; i++) {
        toggler[i].addEventListener("click", function () {
            this.parentElement.querySelector(".nested").classList.toggle("active");
            this.classList.toggle("caret-down");
        });
    }
}

function BeginFoleo() {
    $("#btnBuscar").css("display", "none");
    $("#preloaderBuscar").css("display", "block");
}
