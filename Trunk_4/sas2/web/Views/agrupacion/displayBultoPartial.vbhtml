@imports web

@helper displayTree(lst As IEnumerable(Of Agrupacion))
    @if lst IsNot Nothing Then
@For Each gu As Agrupacion In lst
    @<div class="ag">
        @h.traducir("Bulto Nº") <strong>@gu.Id</strong>
        @h.traducir("Peso") <strong>@gu.Peso</strong> Kg
        @For Each mm As Movimiento In gu.ListOfMovimiento
            @<div class="mm">
                @mm.Numord-@mm.Numope @mm.Marca
            </div>
        Next
        @displayTree(gu.children)
    </div>
Next
    End If

End Helper

@displayTree(Model)