@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Evolución del objetivo"), Utils.Traducir("Editar"))</h3>
<hr />

@code
    Dim objetivo As ELL.Objetivo = CType(ViewData("Objetivo"), ELL.Objetivo)
    Dim listaEvolucionesObjetivo As List(Of ELL.EvolucionObjetivo) = CType(ViewData("EvolucionesObjetivo"), List(Of ELL.EvolucionObjetivo))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
    Dim objetivosHijos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListadoPorPadre(objetivo.Id)


    Dim currentCulture = System.Globalization.CultureInfo.GetCultureInfo(Session("Ticket").Culture)
    Dim decimalCharacter = currentCulture.NumberFormat.NumberDecimalSeparator
End Code

<script type="text/javascript">
    $(function () {
        $(".evolucion_hijos").click(function () {
            var periodicidad = $(this).data('periodicidad');            
            $('#modalBodyHijos').html('@Utils.Traducir("Cargando")...');
            $("#modalWindowHijos").modal('show');
            $('#modalBodyHijos').load('@Url.Action("ListarEvolucionHijos", "EvolucionObjetivos")' + '?idPadre=' + @objetivo.Id + '&periodicidad=' + periodicidad.toString() + '&timeStamp=' + @DateTime.Now.Ticks);
        });
    })
</script>

@Using Html.BeginForm("Editar", "EvolucionObjetivos", New With {.idObjetivo = objetivo.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    Dim identificadorPeriodicidad As String = String.Empty
    Dim mesActual As Integer = DateTime.Today.Month

    ' Sirve para identificar dentro de la periodicidad donde cae el mes actual. Por ejemplo Mayo. En Trimestres es el 2º trimestre, en cuatrimetres es el 2º cuatrimestre y en semestres es el 1º
    Dim posicionPeriodicidad As Integer = Integer.MinValue
    Select Case objetivo.Periodicidad
        Case ELL.Objetivo.TipoPeriodicidad.Mensual
            identificadorPeriodicidad = Utils.Traducir("Mes")
            posicionPeriodicidad = mesActual
        Case ELL.Objetivo.TipoPeriodicidad.Trimestral
            identificadorPeriodicidad = Utils.Traducir("Trimestre")
            posicionPeriodicidad = Math.Ceiling(mesActual / 3)
        Case ELL.Objetivo.TipoPeriodicidad.Cuatrimentral
            identificadorPeriodicidad = Utils.Traducir("Cuatrimestre")
            posicionPeriodicidad = Math.Ceiling(mesActual / 4)
        Case ELL.Objetivo.TipoPeriodicidad.Semestral
            identificadorPeriodicidad = Utils.Traducir("Semestre")
            posicionPeriodicidad = Math.Ceiling(mesActual / 6)
    End Select

    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Objetivo")</label>
        <label class="col-sm-5 control-label" style="text-align:left;">@objetivo.Descripcion</label>
    </div>
    @<div Class="form-group">
        <label class="col-sm-2 control-label">@Utils.Traducir("Valor objetivo")</label>
        <label class="col-sm-2 control-label" style="text-align:left;">@String.Format("{0} {1}", objetivo.ValorObjetivo, objetivo.TipoIndicador)</label>
    </div>

    For a As Integer = 0 To (12 / objetivo.Periodicidad) - 1
        Dim aux As Integer = a
        Dim evolucionObjetivo As ELL.EvolucionObjetivo = listaEvolucionesObjetivo.FirstOrDefault(Function(f) f.IdPeriodicidad = aux)

        If aux Mod 2 = 0 Then
            @:<div Class="form-group">
            @<Label Class="col-sm-2 control-label">@String.Format("{0} {1}", identificadorPeriodicidad, a + 1)</Label>
        Else
            @<Label Class="col-sm-1 control-label">@String.Format("{0} {1}", identificadorPeriodicidad, a + 1)</Label>
        End If

            @<div Class="col-sm-2">
                 <div class="input-group">
                     @code
                         Dim htmlAtributes = Nothing

                         If (a + 1 > posicionPeriodicidad AndAlso objetivo.FechaObjetivo.Year >= DateTime.Today.Year) Then
                             htmlAtributes = New With {.type = "number", .Class = "form-control text-right", .step = "any", .disabled = "disabled"}
                         Else
                             htmlAtributes = New With {.type = "number", .Class = "form-control text-right", .step = "any"}
                         End If

                         Dim valorActualDec As Decimal = Decimal.MinValue
                         If evolucionObjetivo IsNot Nothing Then
                             Decimal.TryParse(evolucionObjetivo.ValorActual, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CreateSpecificCulture(ticket.Culture), valorActualDec)
                             If (decimalCharacter = ",") Then
                                 evolucionObjetivo.ValorActual = evolucionObjetivo.ValorActual.Trim().Replace(".", ",")
                             ElseIf (decimalCharacter = ".") Then
                                 evolucionObjetivo.ValorActual = evolucionObjetivo.ValorActual.Trim().Replace(",", ".")
                             End If
                             'If (Not String.IsNullOrEmpty(evolucionObjetivo.ValorActual) AndAlso evolucionObjetivo.ValorActual <> "0" AndAlso valorActualDec = Decimal.Zero) Then

                             'End If
                         End If
                     End Code
                     @Html.TextBox(String.Format("evolucionesObjetivo[{0}].ValorActual", aux), If(evolucionObjetivo IsNot Nothing AndAlso valorActualDec > Decimal.MinValue, evolucionObjetivo.ValorActual.Replace(",", "."), String.Empty), htmlAtributes) @* forzamos a que sea un punto, ya que internamente pide un punto para decimal*@

                     @*@Html.TextBox(String.Format("evolucionesObjetivo[{0}].ValorActual", aux), If(evolucionObjetivo IsNot Nothing AndAlso evolucionObjetivo.ValorActual <> Decimal.MinValue, evolucionObjetivo.ValorActual, String.Empty), htmlAtributes)*@
                     <div class="input-group-addon">@objetivo.TipoIndicador</div>
                     @code
                         ' Si el objetivo se ha desplegado mostramos el botón para que se puedan visualizar los datos de evolución de los hijos
                         If (objetivosHijos IsNot Nothing AndAlso objetivosHijos.Count > 0) Then
                             @<div class="input-group-addon">
                                 <a style="cursor:pointer;" data-periodicidad="@a" class="evolucion_hijos">
                                     <span class="glyphicon glyphicon-signal" aria-hidden="true" title="@Utils.Traducir("Editar evolución objetivo")"></span>
                                 </a>
                             </div>
                         End If
                     end code
                 </div>

                     @Html.Hidden(String.Format("evolucionesObjetivo[{0}].Id", aux), If(evolucionObjetivo IsNot Nothing, evolucionObjetivo.Id, 0))
                     @Html.Hidden(String.Format("evolucionesObjetivo[{0}].IdObjetivo", aux), objetivo.Id)
                     @Html.Hidden(String.Format("evolucionesObjetivo[{0}].IdUsuarioAlta", aux), ticket.IdUser)
                     @Html.Hidden(String.Format("evolucionesObjetivo[{0}].IdPeriodicidad", aux), aux)
                 </div>

                         If aux Mod 2 = 1 Then
            @:</div>
        End If
                         Next
    @<div Class="form-group">
        <div class="col-sm-offset-2 col-sm-5">
            <input type="submit" id="submit" value="@Utils.Traducir("Guardar")" class="btn btn-primary input-block-level form-control" />
        </div>
    </div>  End Using

     <div class="modal fade bd-example-modal-md" id="modalWindowHijos" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
         <div class="modal-dialog modal-md" role="document">
             <div class="modal-content">
                 <div class="modal-header">
                     <h4 class="modal-title">@Utils.Traducir("Evolución objetivo en plantas hijas")</h4>
                 </div>
                 <div id="modalBodyHijos" class="modal-body">
                 </div>
                 <div class="modal-footer">
                     <button type="button" class="btn btn-secondary" data-dismiss="modal">@Utils.Traducir("Cerrar")</button>
                 </div>
             </div>
         </div>
     </div>
