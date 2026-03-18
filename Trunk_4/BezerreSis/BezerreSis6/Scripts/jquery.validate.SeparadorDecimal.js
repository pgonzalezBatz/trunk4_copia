/*****************************************************************************************************************************************/
//Escribe el separedor decimal dependiendo de la cultura de la interface de usuario
//Ejem:
//  HTML    -> class = "SeparadorDecimal"
//  SCRIPT  -> $(function () {$.SeparadorDecimal("@System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator");}
/*****************************************************************************************************************************************/
$.SeparadorDecimal = function (NumberDecimalSeparator) {
    /*****************************************************************************************************************************************/
    //Personalizamos la validacion para que pueda usarse el "." como separador decimal en las culturas donde le separador decimal es ","
    /*****************************************************************************************************************************************/
    $.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    }
    //$.validator.methods.number = function (value, element) {
    //    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    //}
    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }
    /*****************************************************************************************************************************************/

    $('.SeparadorDecimal').keypress(function () {
        if ((event.keyCode == 44) || (event.keyCode == 46)) { //si pulsamos el punto(46) o coma(44)
            event.preventDefault(); //anula el evento
            event.target.value += NumberDecimalSeparator; //escribe el separedor decimal en el input que generó el evento
        }
    });
}
/*****************************************************************************************************************************************/
