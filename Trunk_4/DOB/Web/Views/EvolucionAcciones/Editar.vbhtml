@Imports DOBLib

<h3>@String.Format("{0} - {1}", Utils.Traducir("Evolución de la acción"), Utils.Traducir("Editar"))</h3>
<hr />

@code
    Dim accion As ELL.Accion = CType(ViewData("Accion"), ELL.Accion)
    Dim objetivo As ELL.Objetivo = BLL.ObjetivosBLL.ObtenerObjetivo(accion.IdObjetivo)
    Dim listaEvolucionesAccion As List(Of ELL.EvolucionAccion) = CType(ViewData("EvolucionesAccion"), List(Of ELL.EvolucionAccion))
    Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
End Code

@Using Html.BeginForm("Editar", "EvolucionAcciones", New With {.idAccion = accion.Id}, FormMethod.Post, New With {.class = "form-horizontal"})
    Dim identificadorPeriodicidad As String = String.Empty
    Dim mesActual As Integer = DateTime.Today.Month

    ' Sirve para identificar dentro de la periodicidad donde cae el mes actual. Por ejemplo Mayo. En Trimestres es el 2º trimestre, en cuatrimetres es el 2º cuatrimestre y en semestres es el 1º
    Dim posicionPeriodicidad As Integer = Integer.MinValue
    Select Case accion.Periodicidad
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
        <label class="col-sm-2 control-label">@Utils.Traducir("Accion")</label>
        <label class="col-sm-5 control-label" style="text-align:left;">@accion.Descripcion</label>
    </div>

    For a As Integer = 0 To (12 / accion.Periodicidad) - 1
        Dim aux As Integer = a
        Dim evolucionAccion As ELL.EvolucionAccion = listaEvolucionesAccion.FirstOrDefault(Function(f) f.IdPeriodicidad = aux)

        If aux Mod 2 = 0 Then
            @:<div Class="form-group">
            @<Label Class="col-sm-2 control-label">@String.Format("{0} {1}", identificadorPeriodicidad, a + 1)</Label>
        Else
            @<Label Class="col-sm-1 control-label">@String.Format("{0} {1}", identificadorPeriodicidad, a + 1)</Label>
        End If

        @<div Class="col-sm-2">   
             @code
                 Dim htmlAtributes = Nothing

                 If (a + 1 > posicionPeriodicidad AndAlso objetivo.FechaObjetivo.Year >= DateTime.Today.Year) Then
                     htmlAtributes = New With {.type = "number", .step = "any", .Class = "form-control text-right", .disabled = "disabled"}
                 Else
                     htmlAtributes = New With {.type = "number", .step = "any", .Class = "form-control text-right"}
                 End If
             End Code
    
              
             <div class="input-group">
                 @Html.TextBox(String.Format("evolucionesAccion[{0}].Porcentaje", aux), If(evolucionAccion IsNot Nothing AndAlso evolucionAccion.Porcentaje <> Decimal.MinValue, evolucionAccion.Porcentaje, String.Empty), htmlAtributes)                 
                 <div class="input-group-addon">%</div>
             </div>
            @Html.Hidden(String.Format("evolucionesAccion[{0}].Id", aux), If(evolucionAccion IsNot Nothing, evolucionAccion.Id, 0))
            @Html.Hidden(String.Format("evolucionesAccion[{0}].IdAccion", aux), accion.Id)
            @Html.Hidden(String.Format("evolucionesAccion[{0}].IdUsuarioAlta", aux), ticket.IdUser)
            @Html.Hidden(String.Format("evolucionesAccion[{0}].IdPeriodicidad", aux), aux)
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


