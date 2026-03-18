@imports web
@Code
    ViewBag.title = "Añadir etiqueta"
End code
    <div id="notifications">
        @h.Traducir("Desde aqui podemos introducir una etiqueta nueva para asignarle un activo o podemos introducir una etiqueta ya asignada para operar con ella.") 
    </div>
@Html.Partial("labelinput")