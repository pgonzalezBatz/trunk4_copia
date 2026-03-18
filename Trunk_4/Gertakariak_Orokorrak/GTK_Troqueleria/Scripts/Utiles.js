$(function () {
    /*-------------------------------------------------------------------------------------------------*/
    //Evitamos el Request peligroso poniendo un espacio entre el carecter y '<'. ejem.: "<a" --> "< a"
    //No permite posicionar ni mover el 'cursor' dentro de la caja de texto con Win7+IE11 
	//Comportamiento extraño con los eventos 'focus select keydown keypress keyup mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave click dblclick'
    /*-------------------------------------------------------------------------------------------------*/
    $("input:text, textarea").on("change blur ", function () {
        $(this).val($(this).val().replace(/<([a-z]|[A-Z])+/g, "< $1"));
    });
    /*-------------------------------------------------------------------------------------------------*/
});