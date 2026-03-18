//Solo permite la entrada de numeros,coma y punto
function soloNumeros(event) {

	var theKey;
	var bolRet = false;
	var theKey;
	var bolRet = false;
	var bolCtrl = event.ctrlKey;
	var bolShift = event.shiftKey;
	theKey = event.keyCode;
	if (bolShift)
		bolRet = false;
	else if ((theKey == 8) ||   //borrar
		(theKey == 46) ||  //suprimir
		(theKey == 37) ||  //flecha izquierda
		(theKey == 39) ||  //flecha derecha
		(theKey > 47 && theKey < 58) || //del 0 al 9 
		(theKey > 95 && theKey < 106) || //del 0 al 9 en el teclado numérico
		(theKey == 67 && bolCtrl) || //ctr+c
	    (theKey == 86 && bolCtrl) || //ctr+v  
		(theKey == 9) ||     //tabulador
		(theKey == 188) ||     //coma
		(theKey == 190) ||     //punto	
		(theKey == 110))	     //punto del teclado numerico
	{
		bolRet = true;
	}
	return bolRet;

}

//Cambio de color de las Filas
var TrClase;
function Sartu(TR) {
	TrClase = TR.className;
	TR.className = 'trOver';
}
function Irten(TR) {
	TR.className = TrClase;
}

function SartuPointer(TR) {
	TrClase = TR.className;
	TR.className = 'trOverPointer';
}
function IrtenPointer(TR) {
	TR.className = TrClase;
}