//Cambio de color de las Filas
var TrClase;
function Sartu(TR){
 TrClase = TR.className;
 TR.className = 'trOverArrow';
}
function Irten(TR){
 TR.className = TrClase;
}

var TrClaseN;
function SartuN(TR){
 TrClaseN = TR.className;
 TR.className = 'trOver';
}
function IrtenN(TR){
 TR.className = TrClaseN;
}