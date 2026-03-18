@imports DOBLib

<h3>@Utils.Traducir("Despliegue de objetivos")</h3>
<h4>@Utils.Traducir("Aplicación para la gestión del despliegue de objetivos")</h4>

@code
    If (Session("RolActual") IsNot Nothing) Then
        ' Cargamos los objetivos del año actual en los cuales el usuario es responsable
        Dim objetivos As List(Of ELL.Objetivo) = BLL.ObjetivosBLL.CargarListado(CType(Session("RolActual"), ELL.UsuarioRol).IdPlanta, CType(Session("Ticket"), SabLib.ELL.Ticket).IdUser, DateTime.Today.Year)
        Dim objetivosMostrar As New List(Of ELL.Objetivo)
        Dim objetivosMostrarAcc As New List(Of ELL.Objetivo)

        If (objetivos IsNot Nothing AndAlso objetivos.Count > 0) Then
            For Each objetivo In objetivos
                ' De cada objetivo hay que sacar su evolucion y ver si el mes actual tiene datos
                Dim evolucion As ELL.EvolucionObjetivo = BLL.EvolucionObjetivosBLL.Obtener(objetivo.Id, DateTime.Today.AddMonths(-1).Month)
                If (evolucion Is Nothing OrElse evolucion.ValorActual = Decimal.MinValue) Then
                    objetivosMostrar.Add(objetivo)
                End If

                ' De cada objetivo hay que sacar las acciones y ver si su grado de importacia suma 100.
                Dim acciones As List(Of ELL.Accion) = BLL.AccionesBLL.CargarListadoPorObjetivo(objetivo.Id)
                If (acciones.Sum(Function(f) f.GradoImportancia) <> 100) Then
                    objetivosMostrarAcc.Add(objetivo)
                End If
            Next

            If (objetivosMostrar.Count > 0) Then
                @<hr>
                @<h4 style="color:red;">@Utils.Traducir("A continuación se muestran los objetivos cuya evolución para el mes actual no contiene valor")</h4>
                @<table class="table table-striped table-hover table-condensed">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Objetivo")</th>
                            <th>@Utils.Traducir("Reto")</th>
                            <th>@Utils.Traducir("Proceso")</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        @code
                            For Each objetivo In objetivosMostrar
                                @<tr>
                                    <td>@objetivo.Descripcion</td>
                                    <td>@objetivo.TituloReto</td>
                                    <td>@objetivo.CodigoProceso</td>
                                    <td class="text-center">
                                        <a href='@Url.Action("Editar", "EvolucionObjetivos", New With {.idObjetivo = objetivo.Id})'>
                                            <span class="glyphicon glyphicon-signal" aria-hidden="true" title="@Utils.Traducir("Editar evolución objetivo")"></span>
                                        </a>
                                    </td>
                                </tr> Next
                        end code
                    </tbody>
                </table>
                            End If

                            If (objetivosMostrarAcc.Count > 0) Then
                @<hr>
                @<h4 style="color:red;">@Utils.Traducir("A continuación se muestran los objetivos cuyas acciones no suman 100% en su grado de importancia")</h4>
                @<table class="table table-striped table-hover table-condensed">
                    <thead>
                        <tr>
                            <th>@Utils.Traducir("Objetivo")</th>
                            <th>@Utils.Traducir("Reto")</th>
                            <th>@Utils.Traducir("Proceso")</th>
                        </tr>
                    </thead>
                    <tbody>
                        @code
                            For Each objetivo In objetivosMostrarAcc
                                @<tr>
                                    <td>@objetivo.Descripcion</td>
                                    <td>@objetivo.TituloReto</td>
                                    <td>@objetivo.CodigoProceso</td>

                                </tr> Next
                        end code
                    </tbody>
                </table>
                @<a href='@Url.Action("MisObjetivos", "Objetivos")'>@Utils.Traducir("Mis objetivos")</a>

                                    End If

                                End If
                            End If
End Code