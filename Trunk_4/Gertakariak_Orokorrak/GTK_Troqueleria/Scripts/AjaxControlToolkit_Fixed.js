/****************************************************************************************/
/* 
Esta funcion corrige el problema del z-Index de los paneles de "AjaxControlToolkit 15.1.4.0"
reinicinando el contador 
*/
/****************************************************************************************/
onload = function () {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(resetCounter);
}
function resetCounter(sender, args) {
    try {
        Sys.Extended.UI.ModalPopupBehavior._openCount = 0;
    } catch (ex) {
        // try-catch-throw away!
        alert(ex.message);
    }
}
/****************************************************************************************/