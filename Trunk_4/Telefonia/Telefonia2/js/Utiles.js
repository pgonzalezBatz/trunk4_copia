
     //Realiza una busqueda de alveolos, segun se va escribiendo el codigo
        function onKeyUpBusqueda()
        {
            var listaAux = document.getElementById('ctl00$cp$lbAuxiliar');
            if(listaAux == null)
                listaAux = document.getElementById('ctl00_cp_lbAuxiliar');
            var lista = document.getElementById('ctl00$cp$lbLista');
            if (lista == null)
                lista = document.getElementById('ctl00_cp_lbLista');
            var patron = document.getElementById('ctl00$cp$txtBuscar');
            if (patron == null)
                patron = document.getElementById('ctl00_cp_txtBuscar');
            var text;      
            var encontrados;
            var regEx=new RegExp(patron.value,"i");                                    
            lista.length=0;                 
             for (var idx = 0; idx < listaAux.length; idx ++) {         
                text=listaAux[idx].text;                                                
                encontrados =text.match(regEx);          
                if(encontrados!=null) 
                {                  
                    if(encontrados.length>0) 
                        lista.options[lista.length]= new Option (text, listaAux[idx].value);              
                }                               
            }
           }          
        

    //Devuelve la cadena sin espacios al principio ni al final
    function trim(cadena)
    {
        for(i=0; i<cadena.length; )
        {
	        if(cadena.charAt(i)==" ")
		        cadena=cadena.substring(i+1, cadena.length);
	        else
		        break;
        }

        for(i=cadena.length-1; i>=0; i=cadena.length-1)
        {
	        if(cadena.charAt(i)==" ")
		        cadena=cadena.substring(0,i);
	        else
		        break;
        }
    	
        return cadena;
    }
    
    
//Cambio de color de las Filas
var TrClase;
function Sartu(TR){
 TrClase = TR.className;
 TR.className = 'trOverArrow';
}
function Irten(TR){
 TR.className = TrClase;
}

var TrClase;
function SartuY(TR){
 TrClase = TR.className;
 TR.className = 'trOverYellowArrow';
}
function IrtenY(TR){
 TR.className = TrClase;
}

var TrClase;
function SartuY(TR){
 TrClase = TR.className;
 TR.className = 'trOverYellowArrowNC';
}
function IrtenY(TR){
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

var TrClase12;
function SartuN12(TR){
 TrClase12= TR.className;
 TR.className = 'trOver12';
}
function IrtenN12(TR){
 TR.className = TrClase12;
}

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
    //(theKey == 86 && bolCtrl) || //ctr+v  
		(theKey == 9) ||     //tabulador
		(theKey == 188) ||     //coma
		(theKey == 190) ||     //punto	
		(theKey == 110))	     //punto del teclado numerico
    {
        bolRet = true;
    }
    return bolRet;

}