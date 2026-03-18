//Dado un numero hace que da igual que se meta un punto o una coma
function parse_float(number, thousandPoint) {
    if (number.toString() != '') {
        if (thousandPoint != undefined) //Se quita el punto ya que si no, luego habrá problemas
            number = number.toString().replace(".", "");
        number = number.toString().replace(",", "."); //Siempre trabaja con punto
        return parseFloat(number);
    }
    else
        return 0;
}