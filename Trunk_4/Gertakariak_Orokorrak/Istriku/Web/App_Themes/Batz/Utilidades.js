/***************************************************************************************/
/* Funciones para indicar que se esta buscado (AutoCompleteExtender) */
/***************************************************************************************/
function Rellenando(sender, e) {
    var txt = sender._element; //Objeto asociado al campo "TargetControlID".
    txt.style.backgroundImage = "url(/App_Themes/Batz/Imagenes/Cargando.gif)";
    txt.style.backgroundRepeat = "no-repeat";
    txt.style.backgroundPosition = "center";
}
function Rellenado(sender, e) {
    var txt = sender._element; //Objeto asociado al campo "TargetControlID".
    txt.style.backgroundImage = "";
}
/***************************************************************************************/