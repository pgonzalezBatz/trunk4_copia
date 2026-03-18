@imports web
@Code
    ViewBag.title = "Busqueda Completa"
End Code


<h3 class="my-3">
    @H.Traducir("Evolución del trabajador")
    @Model.nombre @Model.apellido1 @Model.apellido2
</h3>
<strong><a href="@Url.Action("busquedacompleta")">@h.traducir("Volver")</a></strong>
@If ViewData("secuenciasOrganigrama") IsNot Nothing Then
    @<h5>@h.traducir("Evolución de puestos y secuencias")</h5>
    @<table class="table">
        <thead class="thead-light">
            <tr>
                <th>@h.traducir("Id Trabajador")</th>
                <th>@h.traducir("Secuencia")</th>
                <th>@h.traducir("F. inicio")</th>
                <th>@h.traducir("F.Fin")</th>
                <th>@h.traducir("Dif")</th>
                <th>@h.traducir("Convenio")</th>
                <th>@h.traducir("Categoria")</th>
                <th>@h.traducir("N1")</th>
                <th>@h.traducir("N2")</th>
                <th>@h.traducir("N3")</th>
                <th>@h.traducir("N4")</th>
                <th>@h.traducir("N5")</th>
                <th>@h.traducir("N6")</th>
                <th>@h.traducir("N7")</th>
                <th>@h.traducir("N8")</th>
            </tr>
        </thead>
        @For Each t In ViewData("secuenciasOrganigrama")
            @<tr>
                <td>
                    @t.idTrabajador
                </td>

                <td>@t.idsecuencia</td>
                <td>@t.finiciosec.toshortdatestring</td>
                <td>
                    @If IsDate(t.ffinsec) Then
                        @t.ffinsec.toshortdatestring
                    End If
                </td>
                <td>
                    @t.diff
                </td>
                <td>@t.convenio</td>
                <td>@t.categoria</td>
                <td>@t.n1</td>
                <td>@t.n2</td>
                <td>@t.n3</td>
                <td>@t.n4</td>
                <td>@t.n5</td>
                <td>@t.n6</td>
                <td>@t.n7</td>
                <td>@t.n8</td>

            </tr>   Next
    </table>
End If


@If ViewData("secuencias") IsNot Nothing Then
    @<h5>@h.traducir("Evolución de secuencias")</h5>
    @<table class="table">
        <thead class="thead-light">
            <tr>
                <th>@h.traducir("Id Trabajador")</th>
                <th>@h.traducir("Secuencia")</th>
                <th>@h.traducir("F. inicio")</th>
                <th>@h.traducir("F.Fin")</th>
                <th>@h.traducir("Dif")</th>
                <th>@h.traducir("Convenio")</th>
                <th>@h.traducir("Categoria")</th>
            </tr>
        </thead>
        @For Each t In ViewData("secuencias")
            @<tr>
                <td>
                    @t.idTrabajador
                </td>

                <td>@t.idsecuencia</td>
                <td>@t.finiciosec.toshortdatestring</td>
                <td>
                    @If IsDate(t.ffinsec) Then
                        @t.ffinsec.toshortdatestring
                    End If
                </td>
                <td>
                    @t.diff
                </td>
                <td>@t.convenio</td>
                <td>@t.categoria</td>
            </tr>   Next
    </table>
End If

<div class="row">
    <div class="col">
        @If ViewData("indecesActuales") IsNot Nothing Then
            @<h5>@h.traducir("Indices actuales")</h5>
            @<table class="table">
                <thead class="thead-light">
                    <tr>
                        <th>@h.traducir("Indice")</th>
                        <th>@h.traducir("Valor")</th>
                    </tr>
                </thead>
                @For Each t In ViewData("indecesActuales")
                    @<tr>
                        <td>@t.descripcion</td>
                        <td>@t.valor</td>
                    </tr>
                Next
                <tfoot>
                    <tr>
                        <th>@h.traducir("Indice laboral")</th>
                        <th>@CType(ViewData("indecesActuales"), IEnumerable(Of Object)).Sum(Function(t) CDec(t.valor))</th>
                    </tr>
                </tfoot>
            </table>End If
    </div>
    <div class="col">
        @If ViewData("indices") IsNot Nothing Then
            @<h5>@h.traducir("Evolución de indices")</h5>
            @<table class="table">
                <thead class="thead-light">
                    <tr>
                        <th>@h.traducir("Id Trabajador")</th>
                        <th>@h.traducir("Desde")</th>
                        <th>@h.traducir("Hasta")</th>
                        <th>@h.traducir("Indice")</th>
                        <th>@h.traducir("Valor")</th>
                    </tr>
                </thead>
                @For Each t In ViewData("indices")
                    @<tr>
                        <td>
                            @t.idTrabajador
                        </td>
                        <td>@CInt(t.FechaInicio).ToString("####'-'##")</td>
                        <td>@CInt(t.FechaFin).ToString("####'-'##")</td>
                        <td>@t.descripcion</td>
                        <td>@(If(t.valorPrevio = 0, "", t.valorPrevio.ToString() + " -> "))  <strong>@t.valorActual</strong></td>
                    </tr>
                Next
            </table>
        End If
    </div>
</div>
<div class="row">
    <div class="col">
        @If ViewData("vencimientos") IsNot Nothing Then
            @<h5>@h.traducir("Vencimientos")</h5>
            @<table class="table">
                <thead class="thead-light">
                    <tr>
                        <th>@h.traducir("Tipo de vencimiento")</th>
                        <th>@h.traducir("Fecha")</th>
                    </tr>
                </thead>
                @For Each t In ViewData("vencimientos")
                    @<tr>
                        <td>@t.descripcion</td>
                        <td>@t.fecha</td>
                    </tr>
                Next
            </table>
        End If
    </div>
    <div class="col">
        @If ViewData("prorrogas") IsNot Nothing Then
            @<h5>@h.traducir("Prorrogas")</h5>
            @<table class="table">
                <thead class="thead-light">
                    <tr>
                        <th>@h.traducir("Id Trabajador")</th>
                        <th>@h.traducir("Id secuencia")</th>
                        <th>@h.traducir("Fecha inicio prorroga")</th>
                        <th>@h.traducir("Fecha fin prorroga")</th>
                    </tr>
                </thead>
                @For Each t In ViewData("Prorrogas")
                    @<tr>
                        <td>@t.idTrabajador</td>
                        <td>@t.idsecuencia</td>
                        <td>@t.fInicio</td>
                        <td>@t.fFinal</td>
                    </tr>
                Next
            </table>
        End If
    </div>
</div>






