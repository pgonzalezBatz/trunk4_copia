Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidacionesLineaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene lista de validaciones línea validados. Datos calculados
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
            Return DAL.ValidacionesLineaDAL.loadListByValidacion(idValidacion)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorCostGroup(ByVal idCostGroup As Integer) As List(Of ELL.ValidacionLinea)
            Return DAL.ValidacionesLineaDAL.loadListByCostGroup(idCostGroup)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorStep(ByVal idStep As Integer) As List(Of ELL.ValidacionLinea)
            Return DAL.ValidacionesLineaDAL.loadListByStep(idStep)
        End Function

        ''' <summary>
        ''' Lista de validaciones linea aprobados por cost group
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoValidadosByCostGroup(ByVal idCostGroup As Integer) As List(Of ELL.ValidacionLinea)
            Return DAL.ValidacionesLineaDAL.loadListValidadosByCostGroup(idCostGroup)
        End Function

        '''' <summary>
        '''' Obtiene lista de validaciones línea validados. Datos calculados
        '''' </summary>
        '''' <param name="idValidacion"></param>
        '''' <returns></returns>
        'Public Shared Function CargarListadoValidadosPorValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
        '    Return DAL.ValidacionesLineaDAL.loadListValidatedByValidacion(idValidacion)
        'End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoUltimoPorValidacion(ByVal idValidacion As Integer) As List(Of ELL.ValidacionLinea)
            Return DAL.ValidacionesLineaDAL.loadListUltimoByValidacion(idValidacion)
        End Function

        ''' <summary>
        ''' Para un idStep nos devuelve validación línea más reciente que esté en estado aprobado, abierto o cerrado
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerUltimoAprobado(ByVal idStep As Integer) As ELL.ValidacionLinea
            Return DAL.ValidacionesLineaDAL.getObjectUltimoAprobado(idStep)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.ValidacionLinea
            Return DAL.ValidacionesLineaDAL.getObject(id)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerPorStep(ByVal idStep As Integer) As ELL.ValidacionLinea
            Return DAL.ValidacionesLineaDAL.getObjectByIdStep(idStep)
        End Function

        ''' <summary>
        ''' Obtiene lista de validaciones línea validados por costgroup. Los datos ya están calculados
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerValidadoByCostGroup(ByVal idCostGroup As Integer) As ELL.ValidacionLinea
            Return DAL.ValidacionesLineaDAL.getObjectValidadosByCostGroup(idCostGroup)
        End Function

        ''' <summary>
        ''' Para una validación linea obtiene la validación linea de la validación anterior. Puede ser que no haya
        ''' </summary>
        ''' <param name="validacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerAnteriorAprobada(ByVal validacionLinea As ELL.ValidacionLinea) As ELL.ValidacionLinea
            ' Obtenemos los idvalidacion donde encontramos ese paso y los ordenamos
            Dim validacionesLinea As List(Of ELL.ValidacionLinea) = BLL.ValidacionesLineaBLL.CargarListadoPorStep(validacionLinea.IdStep)
            Dim listaIdValidaciones As List(Of Integer) = validacionesLinea.Select(Function(f) f.IdValidacion).OrderBy(Function(f) f).ToList()

            ' Buscamos en el listado de id la validacion que estamos tratando y sacamos su indice
            Dim indice As Integer = listaIdValidaciones.FindIndex(Function(f) f = validacionLinea.IdValidacion)

            Dim valLineaAnterior As New ELL.ValidacionLinea()
            Dim indiceAux As Integer = 1
            Dim encontrado As Boolean = False
            If (indice > 0) Then
                While (Not encontrado)
                    If (indice - indiceAux >= 0) Then
                        Dim idValidacionAnterior As Integer = listaIdValidaciones(indice - indiceAux)

                        ' Vamos al historico de la validacion linea
                        Dim historico As List(Of ELL.HistoricoEstadoLinea) = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(validacionesLinea.FirstOrDefault(Function(f) f.IdValidacion = idValidacionAnterior AndAlso f.IdStep = validacionLinea.IdStep).Id)

                        If (historico.Exists(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Approved)) Then
                            valLineaAnterior = validacionesLinea.FirstOrDefault(Function(f) f.IdValidacion = idValidacionAnterior AndAlso f.IdStep = validacionLinea.IdStep)
                            encontrado = True
                        Else
                            indiceAux += 1
                        End If
                    Else
                        encontrado = True
                    End If
                End While
            End If

            Return valLineaAnterior
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <param name="idUser"></param>
        ''' <param name="comentarios"></param>
        ''' <param name="estadoValidacion"></param>
        ''' <param name="idAccionValidacion"></param>
        ''' <param name="orden"></param>
        ''' <param name="fecha"></param>
        Public Shared Sub AñadirEstadoValidacion(ByVal idValidacionLinea As Integer, ByVal idUser As Integer, ByVal comentarios As String, ByVal estadoValidacion As Integer, ByVal idAccionValidacion As Integer, ByVal orden As Integer, Optional ByVal fecha As DateTime? = Nothing)
            DAL.ValidacionesLineaDAL.AddValidationState(idValidacionLinea, idUser, comentarios, estadoValidacion, idAccionValidacion, orden, fecha)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <param name="idUser"></param>
        ''' <param name="estadoValidacion"></param>
        ''' <param name="idAccionValidacion"></param>
        ''' <param name="orden"></param>
        ''' <param name="porcentaje"></param>
        Public Shared Sub EliminarEstadoValidacion(ByVal idValidacionLinea As Integer, ByVal idUser As Integer, ByVal estadoValidacion As Integer, ByVal idAccionValidacion As Integer, ByVal orden As Integer, ByVal porcentaje As Decimal)
            DAL.ValidacionesLineaDAL.DeleteValidationState(idValidacionLinea, idUser, estadoValidacion, idAccionValidacion, orden, porcentaje)
        End Sub

#End Region

    End Class

End Namespace