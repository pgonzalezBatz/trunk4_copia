/************************************************************************************/
/* Panel "Cargando Datos" */
/************************************************************************************/
$(function () {
    ////Para todos los formularos que hagan "Submit"
    //$("body").on("submit", "form", function () {
    //    if ($.validator) {
    //        // $.validator is defined
    //        if ($(this).valid()) { CargandoDatos(); }
    //    }
    //    else { CargandoDatos(); }
    //});
    ////Para todas los objetos que tengan la clase "CargandoDatos"
    //$("body").on("click", ".CargandoDatos", function () { CargandoDatos(); });

    //Jquery Ej.: $('#Button2').CargandoDatos(); // $(this).CargandoDatos();
    //$.fn.CargandoDatos = function () { CargandoDatos(); };

    //$(window).unload(function () {
        //alert("window.unload");
        //CargandoDatos();
    //});

    //Evento antes de que la pagina sea descargada
    $(window).on('beforeunload', function () { CargandoDatos(); });

})

function CargandoDatos() {
    //Cerramos todos los "Popovers" para que no se queden encima del panel "Cargando Datos"
    $('[data-toggle="popover"]').popover('hide');
    //Cerramos todas las "Modales" para que no se queden encima del panel "Cargando Datos"
    $('[role="dialog"]').modal('hide');
    $("#CargandoDatos").modal({ backdrop: "static", keyboard: false });
}
/************************************************************************************/

/************************************************************************************/
//Activamos todos los toolip de Bootstrap
/************************************************************************************/
$(function () {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
        $("body").fadeIn(200);
    });

    $(document).on('click', '.dropdown-menu .disabled', function (e) {
        e.stopPropagation();
    });

})
/************************************************************************************/

